using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Modbus.Device;

namespace EAPDashboard.Models
{
    public class ModbusDriver : IPlcDriver
    {
        private TcpClient _tcpClient;
        private IModbusMaster _master;
        private string _ip;
        private int _port;

        public bool IsConnected => _tcpClient != null && _tcpClient.Connected;
        public event Action<LogLevel, string> LogHandler;

        public async Task<bool> ConnectAsync(string ip, int port)
        {
            try
            {
                _ip = ip; _port = port;
                if (_tcpClient != null) { _tcpClient.Close(); _tcpClient.Dispose(); }
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(ip, port);
                _master = ModbusIpMaster.CreateIp(_tcpClient);
                _master.Transport.Retries = 0;
                _master.Transport.ReadTimeout = 2000;
                _master.Transport.WriteTimeout = 2000;
                return true;
            }
            catch { return false; }
        }

        public void Disconnect()
        {
            _master?.Dispose();
            _tcpClient?.Close();
            _tcpClient?.Dispose();
        }

        public async Task<ushort[]> ReadAsync(string address, ushort length)
        {
            ushort startAddr = ParseAddress(address);
            return await _master.ReadHoldingRegistersAsync(0, startAddr, length);
        }

        public async Task WriteAsync(string address, ushort[] data)
        {
            ushort startAddr = ParseAddress(address);
            await _master.WriteMultipleRegistersAsync(0, startAddr, data);
        }

        public async Task<string> ReadAsciiAsync(string address, ushort length)
        {
            var res = await ReadAsync(address, length);
            var bytes = res.SelectMany(BitConverter.GetBytes).ToArray();
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
            ushort val = await ReadWordAsync(address);
            return (val & (1 << bitOffset)) != 0;
        }

        public async Task WriteBitAsync(string address, bool value, int bitOffset = 0)
        {
            ushort val = await ReadWordAsync(address);
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

        private ushort ParseAddress(string address)
        {
            // Modbus 防呆：處理 40001 格式或純數字字串
            if (address.StartsWith("4"))
            {
                if (ushort.TryParse(address.Substring(1), out ushort addr)) return (ushort)(addr - 1);
            }
            if (ushort.TryParse(address, out ushort a)) return a;
            return 0;
        }

        public void Dispose() => Disconnect();
    }
}
