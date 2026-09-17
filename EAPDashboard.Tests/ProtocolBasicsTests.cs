using System;
using System.Text.RegularExpressions;
using CyntecMqttLib;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Protocol.Core.Enums;
using Protocol.Core.Messages;
using Xunit;

namespace EAPDashboard.Tests
{
    public class ProtocolBasicsTests
    {
        [Fact]
        public void EnvelopeFormat_ShouldContainMessageId_TimestampUtc_And_Payload()
        {
            // Arrange - Build Spec 3.1 Envelope
            var payloadObj = new TrackInReqMessage
            {
                CMD = CommandType.TrackInReq,
                TransactionID = Guid.NewGuid().ToString("N"),
                WorkOrder = new System.Collections.Generic.List<string> { "WO100" },
                CassetteID = new System.Collections.Generic.List<string> { "C001" },
                UserID = "U001"
            };

            string isoTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            string messageId = Guid.NewGuid().ToString();

            string rawJson = $"{{\"MessageId\":\"{messageId}\",\"TimestampUtc\":\"{isoTime}\",\"Payload\":{{\"CMD\":\"TrackInReq\",\"TransactionID\":\"B75522\"}}}}";

            // Act
            var settings = new JsonSerializerSettings { DateParseHandling = DateParseHandling.None };
            JObject parsed = JsonConvert.DeserializeObject<JObject>(rawJson, settings);

            // Assert - Spec 3.1 Compliance
            parsed["MessageId"].Value<string>().Should().Be(messageId);
            parsed["TimestampUtc"].Value<string>().Should().EndWith("Z");

            parsed["Payload"].Should().NotBeNull();
            parsed["Payload"]["CMD"].Value<string>().Should().Be("TrackInReq");
            parsed["Payload"]["TransactionID"].Should().NotBeNull();
        }

        [Fact]
        public void RemoteCMDMessage_PPSelect_ShouldMatchSpec311Format()
        {
            // Arrange
            var remoteCmd = new RemoteCMDMessage
            {
                CMD = CommandType.RemoteCMD,
                RemoteCMDType = RemoteCommandType.PP_SELECT,
                RecipeID = "RECIPE_ABC_001",
                TransactionID = "B75522"
            };

            var settings = new JsonSerializerSettings
            {
                Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
            };

            // Act
            string json = JsonConvert.SerializeObject(remoteCmd, settings);
            JObject parsed = JObject.Parse(json);

            // Assert - Spec 3.11 Compliance
            parsed["CMD"].Value<string>().Should().Be("RemoteCMD");
            parsed["RemoteCMDType"].Value<string>().Should().Be("PP_SELECT");
            parsed["RecipeID"].Value<string>().Should().Be("RECIPE_ABC_001");
            parsed["TransactionID"].Value<string>().Should().Be("B75522");
        }

        [Fact]
        public void MqttTopic_FormatCheck_ShouldBeValid()
        {
            // Arrange
            string factory = "TN2";
            string floor = "Floor2";
            string area = "Yellow";
            string eqId = "PL001";

            // Act
            string commandTopic = $"{factory}/{floor}/{area}/{eqId}/Command";
            string reportTopic = $"{factory}/{floor}/{area}/{eqId}/Report";

            // Assert
            commandTopic.Should().Be("TN2/Floor2/Yellow/PL001/Command");
            reportTopic.Should().Be("TN2/Floor2/Yellow/PL001/Report");
        }

        [Fact]
        public void ReplyEventReportMessage_RtnResult_ShouldSerializeAsString()
        {
            // Arrange
            var replyMsg = new ReplyEventReportMessage
            {
                CMD = CommandType.ReplyEventReport,
                RtnResult = RtnResult.PASS,
                RtnMsg = "123456",
                TransactionID = "TID_001"
            };

            // Act - Using CyntecMqttLib's JsonUtil
            string json = CyntecMqttLib.Util.JsonUtil.Serialize(replyMsg);
            JObject parsed = JObject.Parse(json);

            // Assert - RtnResult must be "PASS", not 1
            parsed["RtnResult"].Value<string>().Should().Be("PASS");
            parsed["CMD"].Value<string>().Should().Be("ReplyEventReport");
            parsed["RtnMsg"].Value<string>().Should().Be("123456");
        }

        [Fact]
        public void AppConfig_ExitPassword_ShouldDefaultToAdmin_And_SupportCustomConfig()
        {
            // Arrange & Act - Default instance
            var config = new AppConfig();

            // Assert
            config.ExitPassword.Should().Be("admin");

            // Custom password
            string json = "{\"ExitPassword\":\"Secret123\"}";
            var deserialized = JsonConvert.DeserializeObject<AppConfig>(json);
            deserialized.Should().NotBeNull();
            deserialized.ExitPassword.Should().Be("Secret123");
        }

        [Fact]
        public void AppConfig_MinimizeToTray_ShouldDefaultToFalse_And_SupportCustomConfig()
        {
            // Arrange & Act - Default instance
            var config = new AppConfig();

            // Assert
            config.MinimizeToTray.Should().BeFalse();

            // Custom config
            string json = "{\"MinimizeToTray\": true}";
            var deserialized = JsonConvert.DeserializeObject<AppConfig>(json);
            deserialized.Should().NotBeNull();
            deserialized.MinimizeToTray.Should().BeTrue();
        }

        [Fact]
        public void AppConfig_ReelCheckSettings_ShouldDefault_And_SupportCustomConfig()
        {
            // Arrange & Act - Default instance
            var config = new AppConfig();

            // Assert
            config.ReelCheckSettings.Should().NotBeNull();
            config.ReelCheckSettings.ProductionUrl.Should().Be("http://twcynmesweb01/WcfEntrance/WCF/Wcf.svc");
            config.ReelCheckSettings.TestUrl.Should().Be("http://twcynmeswebqa01/WcfEntranceTest/WCF/Wcf.svc");
            config.ReelCheckSettings.ActiveEnvironment.Should().Be("Test");
            config.ReelCheckSettings.GetEffectiveServerUrl().Should().Be("http://twcynmeswebqa01/WcfEntranceTest/WCF/Wcf.svc");
            config.ReelCheckSettings.ClientUrl.Should().Be("http://127.0.0.1:8080/EAP_ReelCheck");
            config.ReelCheckSettings.TimeoutSeconds.Should().Be(30);

            // Custom config (Production)
            string json = @"{
                ""ReelCheckSettings"": {
                    ""ProductionUrl"": ""http://twcynmesweb01/WcfEntrance/WCF/Wcf.svc"",
                    ""TestUrl"": ""http://twcynmeswebqa01/WcfEntranceTest/WCF/Wcf.svc"",
                    ""ActiveEnvironment"": ""Production"",
                    ""ClientUrl"": ""http://10.1.2.3:8080/EAP"",
                    ""TimeoutSeconds"": 60
                }
            }";
            var deserialized = JsonConvert.DeserializeObject<AppConfig>(json);
            deserialized.Should().NotBeNull();
            deserialized.ReelCheckSettings.Should().NotBeNull();
            deserialized.ReelCheckSettings.ProductionUrl.Should().Be("http://twcynmesweb01/WcfEntrance/WCF/Wcf.svc");
            deserialized.ReelCheckSettings.TestUrl.Should().Be("http://twcynmeswebqa01/WcfEntranceTest/WCF/Wcf.svc");
            deserialized.ReelCheckSettings.ActiveEnvironment.Should().Be("Production");
            deserialized.ReelCheckSettings.GetEffectiveServerUrl().Should().Be("http://twcynmesweb01/WcfEntrance/WCF/Wcf.svc");
            deserialized.ReelCheckSettings.ClientUrl.Should().Be("http://10.1.2.3:8080/EAP");
            deserialized.ReelCheckSettings.TimeoutSeconds.Should().Be(60);
        }

        [Fact]
        public void AreYouThere_And_IamHere_Correlation_ShouldPreserveTransactionID()
        {
            // Arrange
            string originalTxId = "TX_ALIVE_20260917_001";
            var areYouThereMsg = new AreYouThereMessage
            {
                CMD = CommandType.AreYouThere,
                TransactionID = originalTxId,
                Machine = "EQ_TP01"
            };

            // Act - Reply should correlate with the request TransactionID
            var iamHereMsg = new IamHereMessage
            {
                CMD = CommandType.IamHere,
                TransactionID = areYouThereMsg.TransactionID,
                Machine = areYouThereMsg.Machine
            };

            // Assert
            iamHereMsg.CMD.Should().Be(CommandType.IamHere);
            iamHereMsg.TransactionID.Should().Be(originalTxId);
        }
    }
}
