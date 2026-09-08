using System.Collections.Generic;

namespace cyntec.Windows;

public interface ImdiForm
{
	cyntecWinEntryBase oEntry { get; set; }

	void Command(string formID, string comandID, Dictionary<string, object> parmList);

	bool Init();

	void Stop();
}
