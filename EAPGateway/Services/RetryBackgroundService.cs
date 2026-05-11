using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using EAPGateway.Models;

namespace EAPGateway.Services
{
    public class RetryBackgroundService : BackgroundService
    {
        private readonly DatabaseService _dbService;
        private readonly GatewayConfig _config;
        private readonly LogService _logger;

        public RetryBackgroundService(DatabaseService dbService, GatewayConfig config, LogService logger)
        {
            _dbService = dbService;
            _config = config;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var task in _dbService.RetryReader.ReadAllAsync(stoppingToken))
            {
                bool success = false;
                int retryCount = 0;

                while (!success && !stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        retryCount++;
                        await _dbService.ExecuteActionAsync(_config.SecondaryDb, task.Action, task.Request);
                        success = true;
                        _logger.Info($"[RETRY] Successfully synced Secondary DB for {task.Action} after {retryCount} attempts.", task.EqpId);
                    }
                    catch (Exception ex)
                    {
                        _logger.Warn($"[RETRY] Failed to sync Secondary DB (Attempt {retryCount}): {ex.Message}. Waiting 5s...", task.EqpId);
                        await Task.Delay(5000, stoppingToken);
                    }
                }
            }
        }
    }
}
