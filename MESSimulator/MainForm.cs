using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MESSimulator
{
    public partial class MainForm : Form
    {
        private HttpListener _listener;
        private bool _isRunning = false;

        private readonly ConcurrentDictionary<string, bool> _woAlreadyInMap = new ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, List<string>> _woRequiredCodesMap = new ConcurrentDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            Start();
        }
        private void Start()
        {
            if (_isRunning) return;

            string port = txtPort.Text;
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://localhost:{port}/");

            try
            {
                _listener.Start();
                _isRunning = true;
                UpdateUI(true);
                Log($"Server started on port {port}...");

                Task.Run(() => ListenLoop());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start server: {ex.Message}");
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (!_isRunning) return;
            
            _isRunning = false;
            _listener.Stop();
            _listener.Close();
            UpdateUI(false);
            Log("Server stopped.");
        }

        private async Task ListenLoop()
        {
            while (_isRunning)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    _ = Task.Run(() => HandleRequest(context));
                }
                catch { }
            }
        }

        private async Task HandleRequest(HttpListenerContext context)
        {
            try
            {
                string soapRequest = "";
                using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                {
                    soapRequest = await reader.ReadToEndAsync();
                }

                // 1. 解析 SOAP XML 提取 pParameter (JSON) 以利在 Header 顯示交易名稱
                string jsonReq = ExtractJsonFromSoap(soapRequest);
                string txnName = "Unknown";
                try { txnName = JObject.Parse(jsonReq)["TransactionName"]?.ToString() ?? "Unknown"; } catch { }

                this.Invoke(new Action(() => {
                    string header = $"[{DateTime.Now:HH:mm:ss.fff}] ======= {txnName} Request =======";
                    txtRequest.AppendText(header + Environment.NewLine);
                    txtRequest.AppendText(soapRequest + Environment.NewLine);
                    txtRequest.AppendText(new string('=', header.Length) + Environment.NewLine + Environment.NewLine);
                }));

                Log($"[RECV] {jsonReq}");

                // 2. 根據 TransactionName 生成回覆
                string jsonReply = GenerateMockReply(jsonReq);
                Log($"[SEND] {jsonReply}");

                // 3. 封裝成 SOAP Response
                string soapResponse = WrapInSoapResponse(jsonReply);

                byte[] buffer = Encoding.UTF8.GetBytes(soapResponse);
                context.Response.ContentType = "text/xml; charset=utf-8";
                context.Response.ContentLength64 = buffer.Length;
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                context.Response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                Log($"[ERR] {ex.Message}");
                context.Response.StatusCode = 500;
                context.Response.Close();
            }
        }

        private string ExtractJsonFromSoap(string xml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                nsmgr.AddNamespace("mes", "CyntecMES");

                XmlNode node = doc.SelectSingleNode("//mes:pParameter", nsmgr);
                return node?.InnerText ?? "";
            }
            catch { return ""; }
        }

        private bool IsHandshakePass(string txnName)
        {
            if (this.InvokeRequired)
            {
                return (bool)this.Invoke(new Func<bool>(() => IsHandshakePass(txnName)));
            }

            switch (txnName?.ToUpper())
            {
                case "USERVERIFY":
                case "USER_VERIFY":
                case "USERIDVERIFY":
                    return chkUserVerifyPass == null || chkUserVerifyPass.Checked;
                case "WOQRY":
                    return chkWOQRYPass == null || chkWOQRYPass.Checked;
                case "WOCHECKIN":
                case "WOCHECKIN_MULTI":
                    return chkWOCHECKINPass == null || chkWOCHECKINPass.Checked;
                case "WOCHECKOUT":
                case "WOCHECKOUT_MULTI":
                    return chkWOCHECKOUTPass == null || chkWOCHECKOUTPass.Checked;
                case "EQPSTATUS":
                    return chkEQPSTATUSPass == null || chkEQPSTATUSPass.Checked;
                default:
                    return true;
            }
        }

        private string GenerateMockReply(string jsonReq)
        {
            if (string.IsNullOrEmpty(jsonReq)) return "{\"Result\":\"FAIL\", \"Message\":\"Empty Request\"}";

            try
            {
                var req = JObject.Parse(jsonReq);
                string txn = req["TransactionName"]?.ToString();

                if (!IsHandshakePass(txn))
                {
                    JObject failReply = new JObject();
                    failReply["TransactionName"] = txn;
                    failReply["Result"] = "FAIL";
                    failReply["ResultCode"] = "ERR_SIM_FAIL";
                    failReply["Message"] = $"[Simulation] Forced FAIL response for {txn}";
                    return failReply.ToString(Newtonsoft.Json.Formatting.None);
                }

                JObject reply = new JObject();
                reply["TransactionName"] = txn;
                reply["Result"] = "success";

                switch (txn?.ToUpper())
                {
                    case "USERVERIFY":
                    case "USER_VERIFY":
                    case "USERIDVERIFY":
                        string barcode = req["User2DBarCode"]?.ToString() ?? "";
                        string extractedId = req["UserID"]?.ToString() ?? "";
                        if (barcode.StartsWith("{{{{"))
                        {
                            var parts = barcode.Substring(4).Split('{');
                            if (parts.Length > 0 && !string.IsNullOrEmpty(parts[0]))
                                extractedId = parts[0];
                        }
                        else if (!string.IsNullOrEmpty(barcode))
                        {
                            var match = System.Text.RegularExpressions.Regex.Match(barcode, @"\d{6}");
                            if (match.Success)
                            {
                                extractedId = match.Value;
                            }
                        }

                        // 確保工號為 6 碼純數字 (例如: 123456)
                        if (string.IsNullOrEmpty(extractedId))
                        {
                            extractedId = "123456";
                        }
                        else
                        {
                            string digitsOnly = new string(extractedId.Where(char.IsDigit).ToArray());
                            if (string.IsNullOrEmpty(digitsOnly))
                            {
                                extractedId = "123456";
                            }
                            else if (digitsOnly.Length >= 6)
                            {
                                extractedId = digitsOnly.Substring(0, 6);
                            }
                            else
                            {
                                extractedId = digitsOnly.PadLeft(6, '0');
                            }
                        }

                        reply["UserID"] = extractedId;
                        reply["UserName"] = "模擬測試員";
                        reply["Result"] = "success";
                        reply["ResultCode"] = "0000";
                        reply["Message"] = extractedId;
                        reply["RtnMsg"] = extractedId;
                        break;

                    case "WOQRY":
                        string wonoQry = req["WONO"]?.ToString() ?? "";
                        bool alreadyIn = _woAlreadyInMap.TryGetValue(wonoQry, out bool isIn) && isIn;

                        reply["Result"] = "success";
                        reply["ResultCode"] = "W001";
                        reply["Message"] = "";
                        reply["MatGroupNo"] = "CL";
                        reply["LineCode"] = "CL01";
                        reply["MatNo"] = "9032910N06N";
                        reply["WIPQty"] = "20.0";
                        reply["MatClassCode"] = "01005D";
                        reply["WorkCenterNo"] = "CLEX0008";
                        reply["FlowID"] = "100";
                        reply["AlreadyInFlag"] = alreadyIn ? "Y" : "N";
                        reply["SToPcs"] = "1";
                        reply["WToPcs"] = "5";
                        reply["ProductStage"] = "MP";
                        reply["RefInputQty"] = "4.000";
                        reply["BatchNo"] = "HPA2009000A7";
                        reply["WorkCenterName"] = "模擬測試站";
                        reply["IsUrgent"] = "N";
                        reply["CustomerDeliveryTime"] = "9999-12-31 00:00:00.000";

                        bool isRecipeMismatch = false;
                        try
                        {
                            if (this.InvokeRequired)
                            {
                                isRecipeMismatch = (bool)this.Invoke(new Func<bool>(() => chkRecipeMismatch != null && chkRecipeMismatch.Checked));
                            }
                            else
                            {
                                isRecipeMismatch = chkRecipeMismatch != null && chkRecipeMismatch.Checked;
                            }
                        }
                        catch { }

                        string mockRecipe = "SIM_RECIPE_001";
                        if (isRecipeMismatch)
                        {
                            int lastVal = !string.IsNullOrEmpty(wonoQry) ? (int)wonoQry[wonoQry.Length - 1] : 0;
                            mockRecipe = $"RECIPE_{(lastVal % 2 == 0 ? "A" : "B")}_{wonoQry}";
                        }
                        else if (wonoQry.IndexOf("DIFF", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            mockRecipe = $"SIM_RECIPE_DIFF_{wonoQry}";
                        }

                        var paramDt = new JArray();
                        paramDt.Add(new JObject { 
                            ["ParamCode"] = "001",
                            ["ParamName"] = "RECIPEID", 
                            ["RequireInput"] = "N",
                            ["DataListItem"] = "",
                            ["DefaultValue"] = mockRecipe,
                            ["RefFieldCode"] = "RECIPE ID",
                            ["UpperValue"] = "",
                            ["LowerValue"] = "",
                            ["Showable"] = "Y"
                        });
                        reply["InputParamListDT"] = paramDt.ToString(Newtonsoft.Json.Formatting.None);

                        // 根據已經進站與否判斷是 1st WOQRY 還是 2nd WOQRY
                        string reqCodeStr = alreadyIn 
                            ? (txtOutRequireInputCode != null ? txtOutRequireInputCode.Text : "EX001")
                            : (txtInRequireInputCode != null ? txtInRequireInputCode.Text : "");

                        var codeDt = new JArray();
                        var requiredList = new List<string>();
                        if (!string.IsNullOrWhiteSpace(reqCodeStr) && !reqCodeStr.Equals("無", StringComparison.OrdinalIgnoreCase) && !reqCodeStr.Equals("NONE", StringComparison.OrdinalIgnoreCase))
                        {
                            var codes = reqCodeStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var c in codes)
                            {
                                string cleanCode = c.Trim();
                                if (!string.IsNullOrEmpty(cleanCode))
                                {
                                    requiredList.Add(cleanCode);
                                    codeDt.Add(new JObject {
                                        ["InputCode"] = cleanCode,
                                        ["InputCodeName"] = $"管理項目{cleanCode}",
                                        ["RequireInput"] = "Y",
                                        ["DataListItem"] = "",
                                        ["DefaultValue"] = ""
                                    });
                                }
                            }
                        }
                        reply["InputCodeListDT"] = codeDt.ToString(Newtonsoft.Json.Formatting.None);
                        _woRequiredCodesMap[wonoQry] = requiredList;

                        bool enableNGCodeList = true;
                        try
                        {
                            if (this.InvokeRequired)
                            {
                                enableNGCodeList = (bool)this.Invoke(new Func<bool>(() => chkEnableNGCodeList == null || chkEnableNGCodeList.Checked));
                            }
                            else
                            {
                                enableNGCodeList = chkEnableNGCodeList == null || chkEnableNGCodeList.Checked;
                            }
                        }
                        catch { }

                        if (enableNGCodeList)
                        {
                            var ngArray = new JArray
                            {
                                new JObject { ["NGCode"] = "UPNG501", ["NGName"] = "機台卡片" },
                                new JObject { ["NGCode"] = "UPNG502", ["NGName"] = "人為疏失折片" },
                                new JObject { ["NGCode"] = "UPNG503", ["NGName"] = "工程驗證抽片" }
                            };
                            reply["NGCodeListDT"] = ngArray.ToString(Newtonsoft.Json.Formatting.None);
                        }
                        else
                        {
                            reply["NGCodeListDT"] = "[]";
                        }
                        break;

                    case "WOCHECKIN":
                        string wonoIn = req["WONO"]?.ToString() ?? "";
                        if (_woRequiredCodesMap.TryGetValue(wonoIn, out var reqInCodes) && reqInCodes != null && reqInCodes.Count > 0)
                        {
                            string inputListJson = req["InputListDt"]?.ToString();
                            var providedCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                            if (!string.IsNullOrEmpty(inputListJson))
                            {
                                try
                                {
                                    var arr = JArray.Parse(inputListJson);
                                    foreach (JObject item in arr)
                                    {
                                        string code = item["InputCode"]?.ToString();
                                        string val = item["InputValue"]?.ToString();
                                        if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(val))
                                        {
                                            providedCodes.Add(code.Trim());
                                        }
                                    }
                                }
                                catch { }
                            }

                            foreach (var reqCode in reqInCodes)
                            {
                                if (!providedCodes.Contains(reqCode))
                                {
                                    reply["Result"] = "FAIL";
                                    reply["ResultCode"] = "ERR_MISSING_INPUT";
                                    reply["Message"] = $"[WOCHECKIN] Missing required InputCode: {reqCode}";
                                    return reply.ToString(Newtonsoft.Json.Formatting.None);
                                }
                            }
                        }
                        _woAlreadyInMap[wonoIn] = true;
                        reply["Result"] = "success";
                        reply["Message"] = "Simulation Success";
                        break;

                    case "WOCHECKIN_MULTI":
                        var wosInMulti = new List<string>();
                        if (req["WoNoDt"] != null)
                        {
                            try
                            {
                                var arr = JArray.Parse(req["WoNoDt"].ToString());
                                foreach (var item in arr)
                                {
                                    string wo = item["WoNo"]?.ToString();
                                    if (!string.IsNullOrEmpty(wo)) wosInMulti.Add(wo);
                                }
                            }
                            catch { }
                        }
                        else if (req["WoNoList"] != null && req["WoNoList"] is JArray arrIn)
                        {
                            foreach (var item in arrIn)
                            {
                                string wo = item["WoNo"]?.ToString();
                                if (!string.IsNullOrEmpty(wo)) wosInMulti.Add(wo);
                            }
                        }

                        foreach (var wo in wosInMulti)
                        {
                            _woAlreadyInMap[wo] = true;
                        }
                        reply["Result"] = "success";
                        reply["ResultCode"] = "W001";
                        reply["Message"] = "Simulation Multi-Checkin Success";
                        break;

                    case "WOCHECKOUT":
                        string wonoOut = req["WONO"]?.ToString() ?? "";
                        if (_woRequiredCodesMap.TryGetValue(wonoOut, out var reqOutCodes) && reqOutCodes != null && reqOutCodes.Count > 0)
                        {
                            string inputListJson = req["InputListDt"]?.ToString();
                            var providedCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                            if (!string.IsNullOrEmpty(inputListJson))
                            {
                                try
                                {
                                    var arr = JArray.Parse(inputListJson);
                                    foreach (JObject item in arr)
                                    {
                                        string code = item["InputCode"]?.ToString();
                                        string val = item["InputValue"]?.ToString();
                                        if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(val))
                                        {
                                            providedCodes.Add(code.Trim());
                                        }
                                    }
                                }
                                catch { }
                            }

                            foreach (var reqCode in reqOutCodes)
                            {
                                if (!providedCodes.Contains(reqCode))
                                {
                                    reply["Result"] = "FAIL";
                                    reply["ResultCode"] = "ERR_MISSING_INPUT";
                                    reply["Message"] = $"[WOCHECKOUT] Missing required InputCode: {reqCode}";
                                    return reply.ToString(Newtonsoft.Json.Formatting.None);
                                }
                            }
                        }
                        string ngListOut = req["NGListDt"]?.ToString() ?? req["NGListDT"]?.ToString();
                        if (!string.IsNullOrEmpty(ngListOut))
                        {
                            Log($"[WOCHECKOUT] Received NGListDt for {wonoOut}: {ngListOut}");
                        }

                        _woAlreadyInMap[wonoOut] = false;
                        _woRequiredCodesMap.TryRemove(wonoOut, out _);
                        reply["Result"] = "success";
                        reply["Message"] = "Simulation Success";
                        break;

                    case "WOCHECKOUT_MULTI":
                        var wosOutMulti = new List<string>();
                        if (req["WoNoDt"] != null)
                        {
                            try
                            {
                                var arr = JArray.Parse(req["WoNoDt"].ToString());
                                foreach (var item in arr)
                                {
                                    string wo = item["WoNo"]?.ToString();
                                    if (!string.IsNullOrEmpty(wo)) wosOutMulti.Add(wo);
                                }
                            }
                            catch { }
                        }
                        else if (req["WoNoList"] != null && req["WoNoList"] is JArray arrOut)
                        {
                            foreach (var item in arrOut)
                            {
                                string wo = item["WoNo"]?.ToString();
                                if (!string.IsNullOrEmpty(wo)) wosOutMulti.Add(wo);
                            }
                        }

                        foreach (var wo in wosOutMulti)
                        {
                            _woAlreadyInMap[wo] = false;
                            _woRequiredCodesMap.TryRemove(wo, out _);
                        }
                        reply["Result"] = "success";
                        reply["ResultCode"] = "W001";
                        reply["Message"] = "Simulation Multi-Checkout Success";
                        break;

                    case "EQPSTATUS":
                        reply["Result"] = "success";
                        reply["Message"] = "Simulation Success";
                        break;

                    case "ADDPRODUCTLEFTOVER":
                    case "ADD_PRODUCT_LEFTOVER":
                        string leftoverWono = req["WONO"]?.ToString() ?? "";
                        string leftoverQty = req["Qty"]?.ToString() ?? "0";
                        string leftoverReelId = req["ReelID"]?.ToString() ?? "";
                        string leftoverEqpNo = req["EqpNo"]?.ToString() ?? "";
                        string leftoverUserId = req["UserID"]?.ToString() ?? "";

                        Log($"[AddProductLeftover] Received WONO: {leftoverWono}, Qty: {leftoverQty}, ReelID: {leftoverReelId}, EqpNo: {leftoverEqpNo}, UserID: {leftoverUserId}");
                        reply["Result"] = "success";
                        reply["ResultCode"] = "000";
                        reply["Message"] = "Simulation AddProductLeftover Success";
                        break;

                    case "USEPRODUCTLEFTOVER":
                    case "USE_PRODUCT_LEFTOVER":
                        string useWono = req["WONO"]?.ToString() ?? "";
                        string useReelId = req["ReelID"]?.ToString() ?? "";
                        string useLeftoverWo = req["LeftOverWONo"]?.ToString() ?? "";
                        string useLeftoverBatch = req["LeftOverBatchNo"]?.ToString() ?? "";
                        string useAddQty = req["AddQty"]?.ToString() ?? "0";
                        string useGapQty = req["GapQty"]?.ToString() ?? "0";
                        string useEqpNo = req["EqpNo"]?.ToString() ?? "";
                        string useUserId = req["UserID"]?.ToString() ?? "";

                        Log($"[UseProductLeftover] Received WONO: {useWono}, ReelID: {useReelId}, LeftOverWONo: {useLeftoverWo}, LeftOverBatchNo: {useLeftoverBatch}, AddQty: {useAddQty}, GapQty: {useGapQty}, EqpNo: {useEqpNo}, UserID: {useUserId}");
                        reply["Result"] = "success";
                        reply["ResultCode"] = "000";
                        reply["Message"] = "Simulation UseProductLeftover Success";
                        break;

                    case "EQPWONGADD":
                    case "EQP_WO_NG_ADD":
                        string wongWono = req["WONo"]?.ToString() ?? req["WONO"]?.ToString() ?? "";
                        string wongFlowId = req["FlowID"]?.ToString() ?? "";
                        string wongEqpNo = req["EqpNo"]?.ToString() ?? "";
                        string wongUserId = req["UserID"]?.ToString() ?? "";
                        string wongNgListDt = req["NGListDt"]?.ToString() ?? "";

                        Log($"[EqpWONGAdd] Received WONo: {wongWono}, FlowID: {wongFlowId}, EqpNo: {wongEqpNo}, UserID: {wongUserId}, NGListDt: {wongNgListDt}");
                        reply["Result"] = "success";
                        reply["ResultCode"] = "000";
                        reply["Message"] = "Simulation EqpWONGAdd Success";
                        break;

                    default:
                        reply["Result"] = "FAIL";
                        reply["Message"] = $"Unsupported Transaction: {txn}";
                        break;
                }

                return reply.ToString(Newtonsoft.Json.Formatting.None);
            }
            catch (Exception ex)
            {
                return $"{{\"Result\":\"FAIL\", \"Message\":\"Simulator Error: {ex.Message}\"}}";
            }
        }

        private string WrapInSoapResponse(string jsonReply)
        {
            return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <soap:Body>
    <EqpTransactionResponse xmlns=""CyntecMES"">
      <EqpTransactionResult>{SecurityElement.Escape(jsonReply)}</EqpTransactionResult>
    </EqpTransactionResponse>
  </soap:Body>
</soap:Envelope>";
        }

        private void UpdateUI(bool running)
        {
            if (this.InvokeRequired) { this.Invoke(new Action(() => UpdateUI(running))); return; }
            
            btnStart.Enabled = !running;
            btnStop.Enabled = running;
            txtPort.Enabled = !running;
            lblStatus.Text = running ? "● Server Running" : "● Server Stopped";
            lblStatus.ForeColor = running ? Color.Lime : Color.Red;
        }

        private void Log(string message)
        {
            if (txtLog.InvokeRequired) { txtLog.Invoke(new Action(() => Log(message))); return; }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Start();
        }
    }
}
