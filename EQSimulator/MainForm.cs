using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using CyntecMqttLib.Interfaces;
using CyntecMqttLib.Services;
using Protocol.Core.Base;
using Protocol.Core.Enums;
using Protocol.Core.Factory;
using Protocol.Core.Messages;
using Newtonsoft.Json;

namespace EQSimulator
{
    public partial class MainForm : Form
    {
        private MqttBus _bus;
        private string _eqId = "TS0003";
        private string _baseTopic = "TN2/Floor2/Yellow/TS0003/";
        private EqpDictionary _dictionary = new EqpDictionary();

        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
        };

        public MainForm()
        {
            InitializeComponent();
            InitializeStatusCombo();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            BtnConnect_Click(null, null);
        }

        private void InitializeStatusCombo()
        {
            var statuses = new[] {
                new { Value = 0, Text = "0: RUNNING" },
                new { Value = 1, Text = "1: ALARM" },
                new { Value = 2, Text = "2: PAUSED" },
                new { Value = 3, Text = "3: IDLE" },
                new { Value = 4, Text = "4: SETUP" },
                new { Value = 5, Text = "5: WAIT" },
                new { Value = 6, Text = "6: RUN" },
                new { Value = 7, Text = "7: STOP" },
                new { Value = 8, Text = "8: MANUAL" },
                new { Value = 9, Text = "9: ERROR" },
                new { Value = 10, Text = "10: DOWN" }
            };
            _cmbStatus.DataSource = statuses;
            _cmbStatus.DisplayMember = "Text";
            _cmbStatus.ValueMember = "Value";
        }

        private async void BtnConnect_Click(object sender, EventArgs e)
        {
            if (_bus != null && _bus.IsConnected)
            {
                try { await _bus.DisconnectAsync(); _bus.Dispose(); } catch { }
                _bus = null;
                _btnConnect.Text = "Connect";
                _grpActions.Enabled = false;
                Log("Disconnected from Broker.");
                return;
            }

            if (_bus != null) { try { _bus.Dispose(); } catch { } _bus = null; }

            _eqId = _txtEqId.Text;
            _baseTopic = _txtTopic.Text.TrimEnd('/') + "/";
            _dictionary = EqpDictionary.Load(_eqId);
            Log($"Dictionary loaded for {_eqId}");
            
            try
            {
                string clientId = $"Sim_{_eqId}_{Guid.NewGuid().ToString().Substring(0, 4)}";
                _bus = new MqttBus(_txtBroker.Text, 1883, clientId);
                await _bus.ConnectAsync();
                
                string cmdTopic = _baseTopic + "Command";
                await _bus.SubscribeAsync(cmdTopic, (topic, msg) => HandleIncomingCommand(msg));
                
                _btnConnect.Text = "Disconnect";
                _grpActions.Enabled = true;
                Log($"Connected as {clientId}!");
            }
            catch (Exception ex) { Log($"[ERR] Connection failed: {ex.Message}"); }
        }

        private void BtnUser_Click(object sender, EventArgs e)
        {
            var evm = new EventReportMessage 
            { 
                CMD = CommandType.EventReport, 
                EventID = _dictionary.GetCeid("USER_VERIFY"),
                Data = new List<EventItem> 
                { 
                    new EventItem { Parameter = _dictionary.GetVid("UserID"), Value = _txtUserId.Text },
                    new EventItem { Parameter = _dictionary.GetVid("Barcode"), Value = _txtBarcode.Text }
                }
            };
            SendMessage(evm);
        }

        private void BtnTrackIn_Click(object sender, EventArgs e)
        {
            SendMessage(new TrackInReqMessage 
            { 
                CMD = CommandType.TrackInReq, 
                WorkOrder = _txtWorkOrder.Text, 
                MaterialID = _txtMaterialId.Text, 
                CassetteID = _txtCassetteId.Text,
                ToolingID = _txtToolingId.Text,
                UserID = _txtUserId.Text,
                Qty = 1
            });
        }

        private void BtnTrackOut_Click(object sender, EventArgs e)
        {
            int qty = 0; int.TryParse(_txtQty.Text, out qty);
            SendMessage(new TrackOutReqMessage 
            { 
                CMD = CommandType.TrackOutReq, 
                WorkOrder = _txtWorkOrder.Text, 
                MaterialID = _txtMaterialId.Text, 
                Qty = qty, 
                UserID = _txtUserId.Text,
                CassetteID = _txtCassetteId.Text,
                ToolingID = _txtToolingId.Text,
                Result = "PASS"
            });
        }

        private void BtnAlarm_Click(object sender, EventArgs e)
        {
            SendMessage(new AlarmMessage { CMD = CommandType.ALARM, AlarmCode = "E004", AlarmMsg = "Emergency Stop" });
        }

        private void BtnPanelIn_Click(object sender, EventArgs e)
        {
            SendMessage(new EventReportMessage 
            { 
                CMD = CommandType.EventReport, 
                EventID = _dictionary.GetCeid("PANEL_IN"),
                Data = new List<EventItem> { new EventItem { Parameter = _dictionary.GetVid("LotID"), Value = _txtWorkOrder.Text } }
            });
        }

        private void BtnPanelOut_Click(object sender, EventArgs e)
        {
            SendMessage(new EventReportMessage 
            { 
                CMD = CommandType.EventReport, 
                EventID = _dictionary.GetCeid("PANEL_OUT"),
                Data = new List<EventItem> { new EventItem { Parameter = _dictionary.GetVid("LotID"), Value = _txtWorkOrder.Text } }
            });
        }

        private void BtnReportStatus_Click(object sender, EventArgs e)
        {
            int status = Convert.ToInt32(_cmbStatus.SelectedValue);
            SendMessage(new StatusChangeReportMessage { CMD = CommandType.StatusChangeReport, NewStatus = status, OldStatus = 0 });
        }

        private void HandleIncomingCommand(string json)
        {
            if (this.IsDisposed) return;
            this.Invoke((MethodInvoker)delegate {
                try {
                    var envelope = JsonConvert.DeserializeObject<CyntecMqttLib.Contracts.MessageEnvelope>(json);
                    if (envelope == null) return;
                    
                    string innerJson = envelope.Payload?.ToString();
                    if (string.IsNullOrEmpty(innerJson)) return;

                    var msg = MessageFactory.Create(innerJson);
                    Log($"[RECV] {msg.CMD}");
                    
                    switch (msg.CMD) {
                        case CommandType.AreYouThere:
                            SendMessage(new IamHereMessage { CMD = CommandType.IamHere });
                            break;
                        
                        case CommandType.ReplyTrackInReq:
                            var rti = (ReplyTrackInReqMessage)msg;
                            Log($"-> TrackIn Result: {rti.RtnResult}, Msg: {rti.RtnMsg}");
                            break;

                        case CommandType.RemoteCMD:
                            var rcmd = (RemoteCMDMessage)msg;
                            if (rcmd.RemoteCMDType == RemoteCommandType.PP_SELECT) {
                                Log($"-> RemoteCMD: PP_SELECT. Recipe: {rcmd.RecipeID}. Switching...");
                                Task.Delay(2000).ContinueWith(_ => {
                                    SendMessage(new ReplyRemoteCMDMessage { 
                                        CMD = CommandType.ReplyRemoteCMD, 
                                        RemoteCMDType = RemoteCommandType.PP_SELECT,
                                        RtnResult = RtnResult.PASS 
                                    });
                                    Log($"-> Recipe {rcmd.RecipeID} Change SUCCESS.");
                                });
                            }
                            break;

                        case CommandType.ReplyStatusReq:
                            var rsr = (ReplyStatusReqMessage)msg;
                            Log($"-> Status Req ACK received.");
                            break;

                        case CommandType.ReplyRtnTraceData:
                            var rrtd = (ReplyRtnTraceDataMessage)msg;
                            Log($"-> TraceData ACK: {rrtd.RtnResult}, Msg: {rrtd.RtnMsg}");
                            break;

                        case CommandType.ReplyEventReport:
                            var revm = (ReplyEventReportMessage)msg;
                            Log($"-> Event Reply Result: {revm.RtnResult}, Msg: {revm.RtnMsg}");
                            break;

                        case CommandType.TimeCalibrate:
                            Log($"-> TimeCalibrate received. Syncing...");
                            SendMessage(new ReplyTimeCalibrateMessage { CMD = CommandType.ReplyTimeCalibrate, RtnResult = RtnResult.PASS });
                            break;
                        }
                        } catch (Exception ex) { Log($"[ERR] Parse failed: {ex.Message}"); }
                        });
                        }

                        private void BtnRtnTraceData_Click(object sender, EventArgs e)
                        {
                        var rtd = new RtnTraceDataMessage
                        {
                        CMD = CommandType.RtnTraceData,
                        Status = _cmbStatus.SelectedValue.ToString(),
                        CycleTime = "12.5",
                        Availability = "98.5",
                        Parameter = new List<Dictionary<string, double>>
                        {
                        new Dictionary<string, double> { { "Temperature", 25.4 } },
                        new Dictionary<string, double> { { "Pressure", 101.3 } }
                        },
                        RecipeDATA = new RecipeDataContent
                        {
                            RecipeID = "RECIPE_001",
                            RecipeParameter = new Dictionary<string, double>
                            {
                                { "TargetSpeed", 1500.0 },
                                { "LimitTime", 60.0 }
                            }
                        }
                        };
                        SendMessage(rtd);
                        }

                        private void SendMessage(BaseMessage msg)
                        {            if (_bus == null || !_bus.IsConnected) return;
            msg.Machine = _txtEqId.Text;
            msg.Date = DateTime.Now.ToString("yyyyMMddHHmmss");
            string reportTopic = _txtTopic.Text.TrimEnd('/') + "/Report";
            _bus.PublishAsync(reportTopic, msg);
            Log($"[SEND] {msg.CMD}");
        }

        private void Log(string text)
        {
            if (_txtLog.InvokeRequired) { _txtLog.Invoke((MethodInvoker)(() => Log(text))); return; }
            _txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {text}{Environment.NewLine}");
        }
    }
}
