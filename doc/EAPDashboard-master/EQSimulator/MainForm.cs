using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
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
                try 
                {
                    await _bus.DisconnectAsync();
                    _bus.Dispose();
                } catch { }
                _bus = null;
                _btnConnect.Text = "Connect";
                _grpActions.Enabled = false;
                Log("Disconnected from Broker and resources released.");
                return;
            }

            if (_bus != null) { try { _bus.Dispose(); } catch { } _bus = null; }

            _eqId = _txtEqId.Text;
            _baseTopic = _txtTopic.Text.TrimEnd('/') + "/";
            
            try
            {
                string clientId = $"Sim_{_eqId}_{Guid.NewGuid().ToString().Substring(0, 4)}";
                _bus = new MqttBus(_txtBroker.Text, 1883, clientId);
                await _bus.ConnectAsync();
                
                string cmdTopic = _baseTopic + "Command";
                await _bus.SubscribeAsync(cmdTopic, (topic, msg) => HandleIncomingCommand(msg));
                
                _btnConnect.Text = "Disconnect";
                _grpActions.Enabled = true;
                Log($"Connected as {clientId}! Listening on: {cmdTopic}");
            }
            catch (Exception ex) 
            { 
                Log($"[ERR] Connection failed: {ex.Message}");
                MessageBox.Show(ex.Message); 
            }
        }

        private void BtnUser_Click(object sender, EventArgs e)
        {
            SendMessage(new UserIDVerifyMessage 
            { 
                CMD = CommandType.UserID_Verify, 
                UserID = _txtUserId.Text,
                Barcode = _txtBarcode.Text
            });
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
                UserID = _txtUserId.Text
            });
        }

        private void BtnTrackOut_Click(object sender, EventArgs e)
        {
            int qty = 0;
            int.TryParse(_txtQty.Text, out qty);

            SendMessage(new TrackOutReqMessage 
            { 
                CMD = CommandType.TrackOutReq, 
                WorkOrder = _txtWorkOrder.Text, 
                MaterialID = _txtMaterialId.Text, 
                Qty = qty, 
                UserID = _txtUserId.Text,
                CassetteID = _txtCassetteId.Text,
                ToolingID = _txtToolingId.Text
            });
        }

        private void BtnAlarm_Click(object sender, EventArgs e)
        {
            SendMessage(new AlarmMessage { CMD = CommandType.ALARM, AlarmCode = "E004", AlarmMsg = "Emergency Stop Pressed" });
        }

        private void BtnReportStatus_Click(object sender, EventArgs e)
        {
            int status = (int)_cmbStatus.SelectedValue;
            SendMessage(new StatusChangeReportMessage 
            { 
                CMD = CommandType.StatusChangeReport, 
                NewStatus = status,
                OldStatus = 0 
            });
        }

        private void HandleIncomingCommand(string json)
        {
            if (this.IsDisposed) return;
            this.Invoke((MethodInvoker)delegate {
                try {
                    string innerJson = json;
                    try 
                    {
                        var envelope = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);
                        if (envelope != null && envelope["Payload"] != null)
                        {
                            innerJson = envelope["Payload"].ToString();
                        }
                    }
                    catch { }

                    var msg = MessageFactory.Create(innerJson);
                    Log($"[RECV]\n{json}");
                    
                    switch (msg.CMD) {
                        case CommandType.AreYouThere:
                            SendMessage(new IamHereMessage { CMD = CommandType.IamHere });
                            break;
                        
                        case CommandType.PP_SELECT:
                            var pp = (PPSelectMessage)msg;
                            Log($"-> Recipe Change Requested: {pp.RecipeID}. Switching...");
                            Task.Delay(2000).ContinueWith(_ => {
                                SendMessage(new ReplyPPSelectMessage { CMD = CommandType.ReplyPP_SELECT, RtnResult = RtnResult.PASS });
                                Log($"-> Recipe {pp.RecipeID} Changed SUCCESS.");
                            });
                            break;

                        case CommandType.TimeCalibrate:
                            var tc = (TimeCalibrateMessage)msg;
                            Log($"-> Time Sync Received: {tc.Date}. Clock Updated.");
                            SendMessage(new ReplyTimeCalibrateMessage { CMD = CommandType.ReplyTimeCalibrate, RtnResult = RtnResult.PASS });
                            break;

                        case CommandType.StatusReq:
                            Log("-> Status Request Received. Reporting current state...");
                            SendMessage(new ReplyStatusReqMessage { 
                                CMD = CommandType.ReplyStatusReq, 
                                CurrentStatus = ((int)_cmbStatus.SelectedValue).ToString() 
                            });
                            break;
                    }
                } catch (Exception ex) { Log($"[ERR] Parse failed: {ex.Message}"); }
            });
        }

        private void SendMessage(BaseMessage msg)
        {
            if (_bus == null || !_bus.IsConnected) return;
            
            msg.Machine = _txtEqId.Text;
            msg.Date = DateTime.Now.ToString("yyyyMMddHHmmss");
            
            string payloadJson = JsonConvert.SerializeObject(msg, _jsonSettings);
            var envelope = CyntecMqttLib.Contracts.MessageEnvelope.Create(payloadJson);
            string finalJson = JsonConvert.SerializeObject(envelope, _jsonSettings);

            string reportTopic = _txtTopic.Text.TrimEnd('/') + "/Report";
            
            _bus.PublishAsync(reportTopic, finalJson);
            Log($"[SEND] to {reportTopic}: {finalJson}");
        }

        private void Log(string text)
        {
            if (_txtLog.InvokeRequired) { _txtLog.Invoke((MethodInvoker)(() => Log(text))); return; }
            _txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {text}{Environment.NewLine}");
        }
    }
}
