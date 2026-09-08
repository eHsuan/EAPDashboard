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
public class Result_Get_Numbering : Basic_Result
{
	private LP_SerialNo _ori_modelField;

	private LP_SerialNo _new_modelField;

	private string _req_key_typeField;

	private string _req_key_prefixField;

	private int _next_numberField;

	private bool _is_new_recordField;

	[XmlElement(Order = 0)]
	public LP_SerialNo _ori_model
	{
		get
		{
			return _ori_modelField;
		}
		set
		{
			_ori_modelField = value;
			RaisePropertyChanged("_ori_model");
		}
	}

	[XmlElement(Order = 1)]
	public LP_SerialNo _new_model
	{
		get
		{
			return _new_modelField;
		}
		set
		{
			_new_modelField = value;
			RaisePropertyChanged("_new_model");
		}
	}

	[XmlElement(Order = 2)]
	public string _req_key_type
	{
		get
		{
			return _req_key_typeField;
		}
		set
		{
			_req_key_typeField = value;
			RaisePropertyChanged("_req_key_type");
		}
	}

	[XmlElement(Order = 3)]
	public string _req_key_prefix
	{
		get
		{
			return _req_key_prefixField;
		}
		set
		{
			_req_key_prefixField = value;
			RaisePropertyChanged("_req_key_prefix");
		}
	}

	[XmlElement(Order = 4)]
	public int _next_number
	{
		get
		{
			return _next_numberField;
		}
		set
		{
			_next_numberField = value;
			RaisePropertyChanged("_next_number");
		}
	}

	[XmlElement(Order = 5)]
	public bool _is_new_record
	{
		get
		{
			return _is_new_recordField;
		}
		set
		{
			_is_new_recordField = value;
			RaisePropertyChanged("_is_new_record");
		}
	}
}
