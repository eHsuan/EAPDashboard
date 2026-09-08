using System.Collections.Generic;

namespace cyntec.WCF;

public interface IwcfTransaction : IwcfMessage
{
	Dictionary<string, object> serverParmList { get; set; }

	double DBAccumulation { get; set; }

	bool IsOnEage { get; set; }

	int OnEageCount { get; set; }

	IwcfMessage serverExecute();
}
