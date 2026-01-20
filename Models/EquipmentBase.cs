using CyntecMqttLib.Interfaces;
using log4net;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EAPDashboard.Models
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }

    // Enums based on SPEC 1.6 and 2.7
    public enum ConnectionStatus
    {
        Connected,
        Connecting,
        Disconnected
    }

    public enum EquipmentState
    {
        [Description("RUNNING")]
        Running, // Green
        [Description("ALARM")]
        Alarm, // Red
        [Description("PAUSED")]
        Paused, // Yellow
        [Description("IDLE")]
        Idle, // Yellow
        [Description("DOWN")]
        Down // Red
    }


    public abstract class EquipmentBase : INotifyPropertyChanged
    {
        private ConnectionStatus _connectionStatus;
        private EquipmentState _state;
        private string _currentRecipe;
        
        public string Id { get; set; }
        public string Name { get; set; }
        public string Protocol { get; set; }

        public ConnectionStatus Connection
        {
            get => _connectionStatus;
            set
            {
                if (_connectionStatus != value)
                {
                    _connectionStatus = value;
                    OnPropertyChanged(nameof(Connection));
                }
            }
        }

        public EquipmentState State
        {
            get => _state;
            set
            {
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged(nameof(State));
                }
            }
        }
        
        public string CurrentRecipe
        {
            get => _currentRecipe;
            set
            {
                if (_currentRecipe != value)
                {
                    _currentRecipe = value;
                    OnPropertyChanged(nameof(CurrentRecipe));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<DateTime, LogLevel, string > LogReceived;
        
        // Overload for backward compatibility or simple info logs
        protected virtual void OnLogReceived(string message )
        {
            OnLogReceived(LogLevel.Info, message );
        }

        protected virtual void OnLogReceived(LogLevel level, string message)
        {
            var timestamp = DateTime.Now;

            // Use LogicalThreadContext Stacks to provide the EquipmentID property to the layout pattern.
            using (log4net.LogicalThreadContext.Stacks["EquipmentID"].Push(this.Id))
            {
                var logger = LogManager.GetLogger(typeof(EquipmentBase));

                switch (level)
                {
                    case LogLevel.Debug:
                        logger.Debug(message);
                        break;
                    case LogLevel.Info:
                        logger.Info(message);
                        break;
                    case LogLevel.Warn:
                        logger.Warn(message);
                        break;
                    case LogLevel.Error:
                        logger.Error(message);
                        break;
                    case LogLevel.Fatal:
                        logger.Fatal(message);
                        break;
                }
            }

            // Notify UI listeners
            LogReceived?.Invoke(timestamp, level, message);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract void StartSimulation(CancellationToken cancellationToken, MqttBus _bus);
        
        // Helper to get status color for the UI strip
        public static Color GetColorForState(EquipmentState state)
        {
            switch (state)
            {
                case EquipmentState.Running:
                    return Color.LimeGreen;
                case EquipmentState.Alarm:
                case EquipmentState.Down:
                    return Color.Crimson;
                case EquipmentState.Paused:
                    return Color.Gold;
                case EquipmentState.Idle:
                    return Color.LightGray;
                default:
                    return Color.Black;
            }
        }
        
        // This is a new virtual method that can be overridden by derived classes for specific cleanup
        public virtual void StopSimulation()
        {
            // Derived classes will implement specific cancellation logic
        }
    }
}
