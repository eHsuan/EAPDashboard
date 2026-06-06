using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using System.Security;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;
using Protocol.MES.Messages;
using Protocol.MES.Utils;

namespace Protocol.MES.Factory;

public static class MESMessageFactory
{
    private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
    
    public static string TargetUrl { get; set; }

    public static MESBaseMessage Create(string json)
    {
        dynamic tmp = JsonConvert.DeserializeObject(json);
        string txn = tmp.TransactionName;
        if (!Enum.TryParse<MESTransactionName>(txn.ToString(), out var trans))
        {
            throw new Exception("Unknown MES Transaction: " + txn);
        }
        return trans switch
        {
            MESTransactionName.WOQRY => MESJsonHelper.Deserialize<WOQRYReply>(json), 
            MESTransactionName.WOCHECKIN => MESJsonHelper.Deserialize<WOCHECKINReply>(json), 
            MESTransactionName.WOCHECKOUT => MESJsonHelper.Deserialize<WOCHECKOUTReply>(json), 
            MESTransactionName.EQPSTATUS => MESJsonHelper.Deserialize<EQPSTATUSReply>(json), 
            MESTransactionName.UserVerify => MESJsonHelper.Deserialize<UserVerifyReply>(json), 
            _ => throw new NotImplementedException("Transaction " + txn + " not implemented"), 
        };
    }

    public static TReply CallMes<TRequest, TReply>(TRequest request) where TRequest : MESBaseMessage where TReply : MESBaseMessage
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        string jsonReq = MESJsonHelper.Serialize(request);
        string jsonReply;
        try
        {
            string escapedJsonReq = SecurityElement.Escape(jsonReq);
            string soapEnvelope = 
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <EqpTransaction xmlns=""CyntecMES"">
      <pParameter>{escapedJsonReq}</pParameter>
    </EqpTransaction>
  </soap:Body>
</soap:Envelope>";

            string url = string.IsNullOrEmpty(TargetUrl) 
                ? "http://twcynmes01.delta.corp/CyntecDataCenter/service/Eqp/Eqp_Portal.asmx" 
                : TargetUrl;

            using var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", "\"CyntecMES/EqpTransaction\"");

            var responseTask = _httpClient.PostAsync(url, content);
            var response = responseTask.GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();

            string xmlResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            
            var xDoc = XDocument.Parse(xmlResponse);
            XNamespace ns = "CyntecMES";
            var resultElement = xDoc.Descendants(ns + "EqpTransactionResult").FirstOrDefault();
            
            if (resultElement == null)
            {
                throw new Exception("SOAP Response missing EqpTransactionResult element.");
            }
            jsonReply = resultElement.Value;
        }
        catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
        {
            throw new Exception("MES 呼叫超時 (15s)");
        }
        catch (Exception ex)
        {
            throw new Exception("Call MES WebService failed: " + ex.Message, ex);
        }
        return MESJsonHelper.Deserialize<TReply>(jsonReply);
    }
}
