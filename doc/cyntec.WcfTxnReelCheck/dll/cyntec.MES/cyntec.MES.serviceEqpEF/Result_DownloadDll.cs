using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpEF;

[Serializable]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://CyntecMES")]
public class Result_DownloadDll : Basic_Result
{
	private byte[] _fileField;

	[XmlElement(DataType = "base64Binary", Order = 0)]
	public byte[] _file
	{
		get
		{
			return _fileField;
		}
		set
		{
			_fileField = value;
			RaisePropertyChanged("_file");
		}
	}
}
