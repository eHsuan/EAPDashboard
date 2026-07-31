using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Protocol.MES.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum MESTransactionName
{
	WOQRY,
	WOCHECKIN,
	WOCHECKOUT,
	EQPSTATUS
}
