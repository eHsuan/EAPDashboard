using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Channels;
using Microsoft.Data.SqlClient;
using EAPGateway.Models;

namespace EAPGateway.Services
{
    public class DatabaseService
    {
        private readonly GatewayConfig _config;
        private readonly LogService _logger;
        private readonly Channel<RetryTask> _retryChannel;

        public DatabaseService(GatewayConfig config, LogService logger)
        {
            _config = config;
            _logger = logger;
            _retryChannel = Channel.CreateUnbounded<RetryTask>();
        }

        public ChannelReader<RetryTask> RetryReader => _retryChannel.Reader;

        public async Task<bool> BindAsync(BindRequest req)
        {
            return await ExecuteDualWriteAsync("Bind", req, req.EqpId);
        }

        public async Task<bool> UnbindAsync(UnbindRequest req)
        {
            return await ExecuteDualWriteAsync("Unbind", req, req.EqpId);
        }

        public async Task<QueryResponse?> QueryBindingAsync(string cassetteId, string eqpId)
        {
            try
            {
                return await ExecuteQueryAsync(_config.PrimaryDb, cassetteId, eqpId);
            }
            catch (Exception ex)
            {
                _logger.Warn($"[DB] Primary Query Failed for {cassetteId}: {ex.Message}. Falling back to Secondary...", eqpId);
                try
                {
                    return await ExecuteQueryAsync(_config.SecondaryDb, cassetteId, eqpId);
                }
                catch (Exception ex2)
                {
                    _logger.Error($"[DB] Secondary Query Failed for {cassetteId}: {ex2.Message}", eqpId);
                    return null;
                }
            }
        }

        private async Task<QueryResponse?> ExecuteQueryAsync(string connStr, string cassetteId, string eqpId)
        {
            if (string.IsNullOrEmpty(connStr)) throw new ArgumentException("Connection string is empty");
            using var conn = new SqlConnection(connStr);
            await conn.OpenAsync();
            
            string woSql = "SELECT WorkOrder FROM EAP_CassetteBind WHERE CassetteID = @cid";
            using var cmdWo = new SqlCommand(woSql, conn);
            cmdWo.Parameters.AddWithValue("@cid", cassetteId);
            var woObj = await cmdWo.ExecuteScalarAsync();
            if (woObj == null) return null;

            var res = new QueryResponse { WorkOrder = woObj.ToString()! };

            string pSql = "SELECT PanelID FROM EAP_PanelBind WHERE CassetteID = @cid ORDER BY SlotNo";
            using var cmdP = new SqlCommand(pSql, conn);
            cmdP.Parameters.AddWithValue("@cid", cassetteId);
            using var reader = await cmdP.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                res.Panels.Add(reader["PanelID"].ToString()!);
            }

            _logger.Debug($"[DB] Query Success for {cassetteId} from {new SqlConnectionStringBuilder(connStr).DataSource}", eqpId);
            return res;
        }

        private async Task<bool> ExecuteDualWriteAsync(string action, object req, string eqpId)
        {
            var taskA = ExecuteActionAsync(_config.PrimaryDb, action, req);
            var taskB = ExecuteActionAsync(_config.SecondaryDb, action, req);

            try
            {
                await Task.WhenAll(taskA, taskB);
                _logger.Info($"[DB] {action} Success on all databases.", eqpId);
                return true; 
            }
            catch (Exception ex)
            {
                if (taskA.IsCompletedSuccessfully)
                {
                    _logger.Warn($"[DB] Primary {action} Success, Secondary Failed ({ex.InnerException?.Message ?? ex.Message}). Enqueueing retry...", eqpId);
                    await _retryChannel.Writer.WriteAsync(new RetryTask { Action = action, Request = req, EqpId = eqpId });
                    return true; 
                }
                _logger.Error($"[DB] Primary {action} Failed: {ex.Message}", eqpId);
                return false;
            }
        }

        public async Task ExecuteActionAsync(string connStr, string action, object req)
        {
            if (string.IsNullOrEmpty(connStr)) throw new ArgumentException("Connection string is empty");
            using var conn = new SqlConnection(connStr);
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();
            try
            {
                if (action == "Bind" && req is BindRequest br)
                {
                    string sqlC = @"IF EXISTS (SELECT 1 FROM EAP_CassetteBind WHERE CassetteID = @cid)
                                    UPDATE EAP_CassetteBind SET WorkOrder = @wo, BindTime = GETDATE() WHERE CassetteID = @cid
                                    ELSE
                                    INSERT INTO EAP_CassetteBind (CassetteID, WorkOrder, BindTime) VALUES (@cid, @wo, GETDATE())";
                    using var cmdC = new SqlCommand(sqlC, conn, tx);
                    cmdC.Parameters.AddWithValue("@cid", br.CassetteId);
                    cmdC.Parameters.AddWithValue("@wo", br.WorkOrder);
                    await cmdC.ExecuteNonQueryAsync();

                    using var cmdD = new SqlCommand("DELETE FROM EAP_PanelBind WHERE CassetteID = @cid", conn, tx);
                    cmdD.Parameters.AddWithValue("@cid", br.CassetteId);
                    await cmdD.ExecuteNonQueryAsync();

                    for (int i = 0; i < br.Panels.Count; i++)
                    {
                        using var cmdI = new SqlCommand("INSERT INTO EAP_PanelBind (PanelID, CassetteID, SlotNo, BindTime) VALUES (@pid, @cid, @slot, GETDATE())", conn, tx);
                        cmdI.Parameters.AddWithValue("@pid", br.Panels[i]);
                        cmdI.Parameters.AddWithValue("@cid", br.CassetteId);
                        cmdI.Parameters.AddWithValue("@slot", i + 1);
                        await cmdI.ExecuteNonQueryAsync();
                    }
                }
                else if (action == "Unbind" && req is UnbindRequest ur)
                {
                    using var cmd = new SqlCommand("DELETE FROM EAP_CassetteBind WHERE CassetteID = @cid", conn, tx);
                    cmd.Parameters.AddWithValue("@cid", ur.CassetteId);
                    await cmd.ExecuteNonQueryAsync();
                }
                await tx.CommitAsync();
            }
            catch { await tx.RollbackAsync(); throw; }
        }
    }

    public class RetryTask
    {
        public string Action { get; set; } = "";
        public object Request { get; set; } = new object();
        public string EqpId { get; set; } = "Gateway";
    }
}
