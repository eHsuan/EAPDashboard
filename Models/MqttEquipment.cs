using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using CyntecMqttLib.Interfaces;
using CyntecMqttLib.Models;
using CyntecMqttLib.Services;
using Protocol.Core.Base;
using Protocol.Core.Enums;
using Protocol.Core.Factory;
using Protocol.Core.Messages;
using Protocol.MES.Factory;
using Protocol.MES.Messages;
using Protocol.MES.Utils;


namespace EAPDashboard.Models
{
    public class MqttEquipment : EquipmentBase
    {
        private readonly Random _random = new Random();
        private CancellationTokenSource _linkedCts; // Use a linked CTS to manage internal cancellation
       
        public string Topic { get; set; }
        public MqttBus _bus;
        public MqttEquipment()
        {
            Protocol = "MQTT";
         
        }

        public override void StartSimulation(CancellationToken externalCancellationToken , MqttBus _bus)
        {
            // Link an internal CTS to the external one, allowing local cancellation via StopSimulation
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);
            CancellationToken cancellationToken = _linkedCts.Token;

            Task.Run(async () =>
            {
                if (_bus.IsConnected)
                {
                    OnLogReceived(LogLevel.Info, $"Simulation starting... Topic: {Topic}");
                    await _bus.SubscribeAsync(Topic, (Topic, Message) => HandleCommand(Message, Topic));
                    State = EquipmentState.Idle;
                    Connection = ConnectionStatus.Connected;
                }
            }, cancellationToken);
          
        }

        public override void StopSimulation()
        {
            _linkedCts?.Cancel();
            _linkedCts?.Dispose(); // Dispose the CancellationTokenSource
        }
        private static readonly Dictionary<int, EquipmentState> StatusMap = new Dictionary<int, EquipmentState>()
        {
           { 0, EquipmentState.Running },
           { 6, EquipmentState.Running },
           { 1, EquipmentState.Alarm },
           { 10, EquipmentState.Alarm },
           { 11, EquipmentState.Alarm },
           { 2, EquipmentState.Paused },
           { 3, EquipmentState.Paused },
           { 4, EquipmentState.Paused },
           { 5, EquipmentState.Paused },
           { 7, EquipmentState.Paused },
           { 8, EquipmentState.Paused },
           { 9, EquipmentState.Paused },
           { 12, EquipmentState.Paused }
       };
        private void HandleCommand(string Message , string topic )  //邏輯判斷
        {
            if (Message == null || Message == string.Empty) return;
            
            OnLogReceived(LogLevel.Info, $"收到資料: {Message}");  // Message = 交握json
                                                           
            BaseMessage msg = MessageFactory.Create(Message);     //對 Json 做序列化

            switch (msg.CMD) //處理個別 CMD
            {
                case CommandType.ALARM:
                    var _Alarm = (AlarmMessage)msg;
                    State = EquipmentState.Alarm;
                    var req = new EQPSTATUSRequest
                    {
                        
                        EqpNo = _Alarm.Machine,
                        WONO = "",
                        UserID = "762171",
                        StatusCode = "1" , //Alarm
                        CycleTime = "0",
                        OutPutQty = "0",
                        TotalOutPutQty = "0",
                        Target_OutPutQty = "0", 
                        ErrorCode = _Alarm.AlarmCode, ///機台 errorcode
                        Availability = "0",
                        ParameterList = null,
                        EqpPartList = null
                    };
                    string jsonReq = MESJsonHelper.Serialize(req);
                    OnLogReceived(LogLevel.Info, $"解析完成: {jsonReq}");
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            EQPSTATUSReply reply = MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req); //呼叫MES
                            OnLogReceived(LogLevel.Info, $"呼叫MES結果: {reply.Result}");
                        }
                        catch (Exception ex)
                        {
                            OnLogReceived(LogLevel.Error, $"呼叫MES異常: {ex.Message}");
                        }
                    });
                    //呼叫MES
                    break;
                case CommandType.StatusChangeReport:
                    var _Status = (StatusChangeReportMessage)msg;
                    if (StatusMap.TryGetValue(_Status.NewStatus, out var state))
                    {
                        State = state;
                    }
                    //呼叫MES 更新機台狀態
                    var StatusChang = new EQPSTATUSRequest
                    {

                        EqpNo = _Status.Machine,
                        WONO = "",
                        UserID = "762171",
                        StatusCode = _Status.NewStatus.ToString(), 
                        CycleTime = "0",
                        OutPutQty = "0",
                        TotalOutPutQty = "0",
                        Target_OutPutQty = "0",
                        ErrorCode = "",
                        Availability = "0",
                        ParameterList = null,
                        EqpPartList = null
                    };
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            EQPSTATUSReply reply = MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(StatusChang);
                            OnLogReceived(LogLevel.Info, $"呼叫MES結果: {reply.Result}");
                        }
                        catch (Exception ex)
                        {
                            OnLogReceived(LogLevel.Error, $"呼叫MES異常: {ex.Message}");
                        }
                    });
                    break;
                default:
                    break;
            } 
        }
    }
}
