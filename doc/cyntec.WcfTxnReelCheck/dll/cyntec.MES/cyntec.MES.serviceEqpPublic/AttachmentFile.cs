using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpPublic;

[Serializable]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "CyntecMES")]
public class AttachmentFile : INotifyPropertyChanged
{
	private string _fileNameField;

	private byte[] _fileField;

	[XmlElement(Order = 0)]
	public string _fileName
	{
		get
		{
			return _fileNameField;
		}
		set
		{
			_fileNameField = value;
			RaisePropertyChanged("_fileName");
		}
	}

	[XmlElement(DataType = "base64Binary", Order = 1)]
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
