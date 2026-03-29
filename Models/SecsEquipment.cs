using System;
using System.Threading;
using System.Threading.Tasks;
using CyntecMqttLib.Interfaces;

namespace EAPDashboard.Models
{
    public class SecsEquipment : EquipmentBase
    {
        private readonly Random _random = new Random();
        private CancellationTokenSource _linkedCts; // Use a linked CTS to manage internal cancellation

        public string Ip { get; set; }
        public int Port { get; set; }

        public SecsEquipment()
        {
            Protocol = "SECS";
        }

        public override void Start(CancellationToken externalCancellationToken, MqttBus _bus, int heartbeatIntervalSeconds)
        {
            // Link an internal CTS to the external one, allowing local cancellation via Stop
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);
            CancellationToken cancellationToken = _linkedCts.Token;
            
            // SECS 目前暫無實作
        }

        public override void Stop()
        {
            _linkedCts?.Cancel();
            _linkedCts?.Dispose(); // Dispose the CancellationTokenSource
        }
    }
}
