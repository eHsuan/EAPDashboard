using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class EQP_OP_Record_Save : TransactionBase<EQP_OP_Record_Save, EQP_OP_Record_SaveResult>
{
	public List<EQP_OP_Record> _EqpOpRecordList = new List<EQP_OP_Record>();

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
