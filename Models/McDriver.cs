using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoTClient.Clients.PLC;
using IoTClient.Enums;

namespace EAPDashboard.Models
{
    public class McDriver : IPlcDriver
    {
        private MitsubishiClient _client;
        public bool IsConnected => _client?.Connected ?? false;
        public event Action<LogLevel, string> LogHandler;

        private string NormalizeAddress(string address)
        {
            if (string.IsNullOrEmpty(address)) return address;
            string addr = address.ToUpper();
            if (addr.StartsWith("DM")) return "D" + addr.Substring(2);
            return addr;
        }

        public async Task<bool> ConnectAsync(string ip, int port)
        {
            _client = new MitsubishiClient(MitsubishiVersion.Qna_3E, ip, port);
            var res = _client.Open();
            return await Task.FromResult(res.IsSucceed);
        }

        public void Disconnect() => _client?.Close();

        public async Task<ushort[]> ReadAsync(string address, ushort length)
        {
            string addr = NormalizeAddress(address);
            ushort[] data = new ushort[length];

            // 根據範例，循環讀取每個 Word 以確保型別與偏移正確
            for (int i = 0; i < length; i++)
            {
                string currentAddr = addr;
                if (i > 0 && addr.StartsWith("D"))
                {
                    if (int.TryParse(addr.Substring(1), out int baseAddr))
                        currentAddr = "D" + (baseAddr + i);
                }

                var res = _client.ReadInt16(currentAddr);
                if (res.IsSucceed)
                {
                    data[i] = (ushort)res.Value;
                }
                else
                {
                    throw new Exception($"[MC_READ_BATCH_ERROR] Failed at {currentAddr}, Error: {res.Err}");
                }
            }

            return await Task.FromResult(data);
        }

        public async Task WriteAsync(string address, ushort[] data)
        {
            string addr = NormalizeAddress(address);

            for (int i = 0; i < data.Length; i++)
            {
                string currentAddr = addr;
                if (i > 0 && addr.StartsWith("D"))
                {
                    if (int.TryParse(addr.Substring(1), out int baseAddr))
                        currentAddr = "D" + (baseAddr + i);
                }

                var res = _client.Write(currentAddr, (short)data[i]);
                if (!res.IsSucceed) throw new Exception($"MC Batch Write Failed at {currentAddr}, Error: {res.Err}");
            }
            await Task.CompletedTask;
        }

        public async Task<string> ReadAsciiAsync(string address, ushort length)
        {
            // 讀取 Word 陣列後轉為 ASCII
            var data = await ReadAsync(address, length);
            byte[] bytes = data.SelectMany(BitConverter.GetBytes).ToArray();
            return Encoding.ASCII.GetString(bytes).TrimEnd('\0', ' ');
        }

        public async Task WriteAsciiAsync(string address, string text, ushort length)
        {
            string addr = NormalizeAddress(address);
            byte[] bytes = Encoding.ASCII.GetBytes(text.PadRight(length * 2, '\0'));
            ushort[] data = new ushort[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = BitConverter.ToUInt16(bytes, i * 2);
            }
            await WriteAsync(addr, data);
        }

        public async Task<bool> ReadBitAsync(string address, int bitOffset = 0)
        {
            string addr = NormalizeAddress(address);
            if (addr.StartsWith("M") && bitOffset == 0)
            {
                // 呼叫強型別 ReadBoolean
                var res = _client.ReadBoolean(addr);
                if (res.IsSucceed) return await Task.FromResult(res.Value);
                throw new Exception($"[MC_READ_BIT_ERROR] Addr: {addr}, Error: {res.Err}");
            }
            var word = await ReadWordAsync(addr);
            return (word & (1 << bitOffset)) != 0;
        }

        public async Task WriteBitAsync(string address, bool value, int bitOffset = 0)
        {
            string addr = NormalizeAddress(address);
            if (addr.StartsWith("M") && bitOffset == 0)
            {
                var res = _client.Write(addr, value);
                if (!res.IsSucceed) throw new Exception($"MC Write Bit Failed: {addr}, Error: {res.Err}");
                await Task.CompletedTask;
                return;
            }
            var val = await ReadWordAsync(addr);
            if (value) val |= (ushort)(1 << bitOffset);
            else val &= (ushort)~(1 << bitOffset);
            await WriteWordAsync(addr, val);
        }

        public async Task<ushort> ReadWordAsync(string address)
        {
            string addr = NormalizeAddress(address);
            // 呼叫強型別 ReadInt16
            var res = _client.ReadInt16(addr);
            if (res.IsSucceed) return await Task.FromResult((ushort)res.Value);
            
            throw new Exception($"[MC_READ_WORD_ERROR] Addr: {addr}, Error: {res.Err}");
        }

        public async Task WriteWordAsync(string address, ushort value)
        {
            string addr = NormalizeAddress(address);
            var res = _client.Write(addr, (short)value);
            if (!res.IsSucceed) throw new Exception($"MC Write Word Failed: Address {addr}, Error: {res.Err}");

            await Task.CompletedTask;
        }

        public void Dispose() => Disconnect();
    }
}
