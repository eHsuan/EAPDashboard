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
public class SqlParameter : DbParameter
{
	private SqlCompareOptions compareInfoField;

	private string xmlSchemaCollectionDatabaseField;

	private string xmlSchemaCollectionOwningSchemaField;

	private string xmlSchemaCollectionNameField;

	private bool forceColumnEncryptionField;

	private int localeIdField;

	private SqlDbType sqlDbTypeField;

	private object sqlValueField;

	private string udtTypeNameField;

	private string typeNameField;

	private int offsetField;

	[XmlElement(Order = 0)]
	public SqlCompareOptions CompareInfo
	{
		get
		{
			return compareInfoField;
		}
		set
		{
			compareInfoField = value;
			RaisePropertyChanged("CompareInfo");
		}
	}

	[XmlElement(Order = 1)]
	public string XmlSchemaCollectionDatabase
	{
		get
		{
			return xmlSchemaCollectionDatabaseField;
		}
		set
		{
			xmlSchemaCollectionDatabaseField = value;
			RaisePropertyChanged("XmlSchemaCollectionDatabase");
		}
	}

	[XmlElement(Order = 2)]
	public string XmlSchemaCollectionOwningSchema
	{
		get
		{
			return xmlSchemaCollectionOwningSchemaField;
		}
		set
		{
			xmlSchemaCollectionOwningSchemaField = value;
			RaisePropertyChanged("XmlSchemaCollectionOwningSchema");
		}
	}

	[XmlElement(Order = 3)]
	public string XmlSchemaCollectionName
	{
		get
		{
			return xmlSchemaCollectionNameField;
		}
		set
		{
			xmlSchemaCollectionNameField = value;
			RaisePropertyChanged("XmlSchemaCollectionName");
		}
	}

	[XmlElement(Order = 4)]
	[DefaultValue(false)]
	public bool ForceColumnEncryption
	{
		get
		{
			return forceColumnEncryptionField;
		}
		set
		{
			forceColumnEncryptionField = value;
			RaisePropertyChanged("ForceColumnEncryption");
		}
	}

	[XmlElement(Order = 5)]
	public int LocaleId
	{
		get
		{
			return localeIdField;
		}
		set
		{
			localeIdField = value;
			RaisePropertyChanged("LocaleId");
		}
	}

	[XmlElement(Order = 6)]
	public SqlDbType SqlDbType
	{
		get
		{
			return sqlDbTypeField;
		}
		set
		{
			sqlDbTypeField = value;
			RaisePropertyChanged("SqlDbType");
		}
	}

	[XmlElement(Order = 7)]
	public object SqlValue
	{
		get
		{
			return sqlValueField;
		}
		set
		{
			sqlValueField = value;
			RaisePropertyChanged("SqlValue");
		}
	}

	[XmlElement(Order = 8)]
	public string UdtTypeName
	{
		get
		{
			return udtTypeNameField;
		}
		set
		{
			udtTypeNameField = value;
			RaisePropertyChanged("UdtTypeName");
		}
	}

	[XmlElement(Order = 9)]
	public string TypeName
	{
		get
		{
			return typeNameField;
		}
		set
		{
			typeNameField = value;
			RaisePropertyChanged("TypeName");
		}
	}

	[XmlElement(Order = 10)]
	public int Offset
	{
		get
		{
			return offsetField;
		}
		set
		{
			offsetField = value;
			RaisePropertyChanged("Offset");
		}
	}

	public SqlParameter()
	{
		forceColumnEncryptionField = false;
	}
}
