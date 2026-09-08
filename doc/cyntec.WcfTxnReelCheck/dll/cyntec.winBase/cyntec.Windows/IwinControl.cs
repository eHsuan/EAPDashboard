using System.Collections.Generic;
using System.Windows.Forms;

namespace cyntec.Windows;

public interface IwinControl
{
	Control Me { get; }

	object txnConn { get; set; }

	object log { get; set; }

	object config { get; set; }

	void ReceiveMsg(object msg);

	void Init();

	void Clear();

	void Refresh(Dictionary<string, object> paramList);

	void Display();
}
