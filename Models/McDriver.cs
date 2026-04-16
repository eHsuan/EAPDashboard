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

        public async Task<bool> ConnectAsync(string ip, int port)
        {
            _client = new MitsubishiClient(MitsubishiVersion.Qna_3E, ip, port);
            var res = _client.Open();
            return await Task.FromResult(res.IsSucceed);
        }

        public void Disconnect() => _client?.Close();

        public async Task<ushort[]> ReadAsync(string address, ushort length)
        {
            var res = _client.Read(address, length);
            if (res.IsSucceed)
            {
                // 將 byte[] 轉換為 ushort[]
                ushort[] data = new ushort[length];
                for (int i = 0; i < length; i++)
                {
                    data[i] = BitConverter.ToUInt16(res.Value, i * 2);
                }
                return await Task.FromResult(data);
            }
            throw new Exception($"MC Read Failed: {res.Err}");
        }

        public async Task WriteAsync(string address, ushort[] data)
        {
            byte[] bytes = data.SelectMany(BitConverter.GetBytes).ToArray();
            var res = _client.Write(address, bytes);
            if (!res.IsSucceed) throw new Exception($"MC Write Failed: {res.Err}");
            await Task.CompletedTask;
        }

        public async Task<string> ReadAsciiAsync(string address, ushort length)
        {
            var data = await ReadAsync(address, length);
            byte[] bytes = data.SelectMany(BitConverter.GetBytes).ToArray();
            return Encoding.ASCII.GetString(bytes).TrimEnd('\0', ' ');
        }

        public async Task WriteAsciiAsync(string address, string text, ushort length)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(text.PadRight(length * 2, '\0'));
            ushort[] data = new ushort[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = BitConverter.ToUInt16(bytes, i * 2);
            }
            await WriteAsync(address, data);
        }

        public async Task<bool> ReadBitAsync(string address, int bitOffset = 0)
        {
            if (address.ToUpper().StartsWith("M") && bitOffset == 0)
            {
                var res = _client.ReadBoolean(address);
                return await Task.FromResult(res.Value);
            }
            var word = await ReadWordAsync(address);
            return (word & (1 << bitOffset)) != 0;
        }

        public async Task WriteBitAsync(string address, bool value, int bitOffset = 0)
        {
            if (address.ToUpper().StartsWith("M") && bitOffset == 0)
            {
                _client.Write(address, value);
                await Task.CompletedTask;
                return;
            }
            var val = await ReadWordAsync(address);
            if (value) val |= (ushort)(1 << bitOffset);
            else val &= (ushort)~(1 << bitOffset);
            await WriteWordAsync(address, val);
        }

        public async Task<ushort> ReadWordAsync(string address)
        {
            var res = await ReadAsync(address, 1);
            return res[0];
        }

        public async Task WriteWordAsync(string address, ushort value)
        {
            await WriteAsync(address, new ushort[] { value });
        }

        public void Dispose() => Disconnect();
    }
}
