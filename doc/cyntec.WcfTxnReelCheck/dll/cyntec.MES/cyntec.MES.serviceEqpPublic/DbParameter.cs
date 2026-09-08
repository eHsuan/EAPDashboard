using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpPublic;

[Serializable]
[XmlInclude(typeof(SqlParameter))]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "CyntecMES")]
public abstract class DbParameter : MarshalByRefObject
{
	private DbType dbTypeField;

	private ParameterDirection directionField;

	private bool isNullableField;

	private string parameterNameField;

	private byte precisionField;

	private byte scaleField;

	private int sizeField;

	private string sourceColumnField;

	private bool sourceColumnNullMappingField;

	private DataRowVersion sourceVersionField;

	private object valueField;

	[XmlElement(Order = 0)]
	public DbType DbType
	{
		get
		{
			return dbTypeField;
		}
		set
		{
			dbTypeField = value;
			RaisePropertyChanged("DbType");
		}
	}

	[XmlElement(Order = 1)]
	[DefaultValue(ParameterDirection.Input)]
	public ParameterDirection Direction
	{
		get
		{
			return directionField;
		}
		set
		{
			directionField = value;
			RaisePropertyChanged("Direction");
		}
	}

	[XmlElement(Order = 2)]
	public bool IsNullable
	{
		get
		{
			return isNullableField;
		}
		set
		{
			isNullableField = value;
			RaisePropertyChanged("IsNullable");
		}
	}

	[XmlElement(Order = 3)]
	[DefaultValue("")]
	public string ParameterName
	{
		get
		{
			return parameterNameField;
		}
		set
		{
			parameterNameField = value;
			RaisePropertyChanged("ParameterName");
		}
	}

	[XmlElement(Order = 4)]
	public byte Precision
	{
		get
		{
			return precisionField;
		}
		set
		{
			precisionField = value;
			RaisePropertyChanged("Precision");
		}
	}

	[XmlElement(Order = 5)]
	public byte Scale
	{
		get
		{
			return scaleField;
		}
		set
		{
			scaleField = value;
			RaisePropertyChanged("Scale");
		}
	}

	[XmlElement(Order = 6)]
	public int Size
	{
		get
		{
			return sizeField;
		}
		set
		{
			sizeField = value;
			RaisePropertyChanged("Size");
		}
	}

	[XmlElement(Order = 7)]
	[DefaultValue("")]
	public string SourceColumn
	{
		get
		{
			return sourceColumnField;
		}
		set
		{
			sourceColumnField = value;
			RaisePropertyChanged("SourceColumn");
		}
	}

	[XmlElement(Order = 8)]
	[DefaultValue(false)]
	public bool SourceColumnNullMapping
	{
		get
		{
			return sourceColumnNullMappingField;
		}
		set
		{
			sourceColumnNullMappingField = value;
			RaisePropertyChanged("SourceColumnNullMapping");
		}
	}

	[XmlElement(Order = 9)]
	[DefaultValue(DataRowVersion.Current)]
	public DataRowVersion SourceVersion
	{
		get
		{
			return sourceVersionField;
		}
		set
		{
			sourceVersionField = value;
			RaisePropertyChanged("SourceVersion");
		}
	}

	[XmlElement(Order = 10)]
	public object Value
	{
		get
		{
			return valueField;
		}
		set
		{
			valueField = value;
			RaisePropertyChanged("Value");
		}
	}

	public DbParameter()
	{
		directionField = ParameterDirection.Input;
		parameterNameField = "";
		sourceColumnField = "";
		sourceColumnNullMappingField = false;
		sourceVersionField = DataRowVersion.Current;
	}
}
