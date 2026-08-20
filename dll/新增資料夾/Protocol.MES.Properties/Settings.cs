using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Protocol.MES.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.13.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[SpecialSetting(SpecialSetting.WebServiceUrl)]
	[DefaultSettingValue("http://twcynmes01.delta.corp/CyntecDataCenter/service/Eqp/Eqp_Portal.asmx")]
	public string Protocol_MES_corp_delta_twcynmes01_Eqp_Portal => (string)this["Protocol_MES_corp_delta_twcynmes01_Eqp_Portal"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("https://cncynmes01.delta.corp/webapi/")]
	public string RestBaseUrl => (string)this["RestBaseUrl"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("eco_eqa_user")]
	public string RestUser => (string)this["RestUser"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Cyntec#Nov123")]
	public string RestPassword => (string)this["RestPassword"];
}
