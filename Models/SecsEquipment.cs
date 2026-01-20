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

        public override void StartSimulation(CancellationToken externalCancellationToken, MqttBus _bus)
        {
            // Link an internal CTS to the external one, allowing local cancellation via StopSimulation
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);
            CancellationToken cancellationToken = _linkedCts.Token;

            //Task.Run(async () =>
            //{
            //    // Initial State
            //    OnLogReceived(LogLevel.Info, $"Simulation starting for SECS Equipment at {Ip}:{Port}...");
            //    Connection = ConnectionStatus.Connected; // SECS is often connection-oriented
            //    OnLogReceived(LogLevel.Info, $"Connection state set to: {Connection}");
            //    State = EquipmentState.Idle;
            //    OnLogReceived(LogLevel.Debug, $"Initial state set to: {State}");
            //    CurrentRecipe = "DEFAULT.RCP";
            //    OnLogReceived(LogLevel.Info, $"Initial recipe set to: {CurrentRecipe}");
            //    try
            //    {
            //        await Task.Delay(_random.Next(1000, 3000), cancellationToken);
            //    }
            //    catch (TaskCanceledException) { OnLogReceived(LogLevel.Warn, "Simulation task cancelled during initial delay."); return; }

            //    while (!cancellationToken.IsCancellationRequested)
            //    {
            //        int nextAction = _random.Next(100);
            //        var previousState = State;

            //        if (State == EquipmentState.Idle)
            //        {
            //            State = EquipmentState.Running;
            //        }
            //        else if (State == EquipmentState.Running)
            //        {
            //            if (nextAction < 15) State = EquipmentState.Down;
            //            else State = EquipmentState.Idle;
            //        }
            //        else if (State == EquipmentState.Down)
            //        {
            //             OnLogReceived(LogLevel.Error, "Equipment is DOWN.");
            //             State = EquipmentState.Idle;
            //        }

            //        if (previousState != State)
            //        {
            //            OnLogReceived(LogLevel.Debug, $"State changed from {previousState} to {State}");
            //        }

            //        try
            //        {
            //            await Task.Delay(_random.Next(5000, 10000), cancellationToken);
            //        }
            //        catch (TaskCanceledException) { OnLogReceived(LogLevel.Warn, "Simulation task cancelled during main loop delay."); break; }
            //    }
            //    OnLogReceived(LogLevel.Info, "Simulation stopped.");
            //}, cancellationToken);
        }

        public override void StopSimulation()
        {
            _linkedCts?.Cancel();
            _linkedCts?.Dispose(); // Dispose the CancellationTokenSource
        }
    }
}
