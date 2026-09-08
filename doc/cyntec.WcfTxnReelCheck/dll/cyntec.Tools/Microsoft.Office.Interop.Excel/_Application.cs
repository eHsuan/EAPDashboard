using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel;

[ComImport]
[CompilerGenerated]
[DefaultMember("_Default")]
[Guid("000208D5-0000-0000-C000-000000000046")]
[TypeIdentifier]
public interface _Application
{
	void _VtblGap1_11();

	[DispId(308)]
	Workbook ActiveWorkbook
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(308)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap2_27();

	[DispId(485)]
	Sheets Sheets
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(485)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap3_5();

	[DispId(572)]
	Workbooks Workbooks
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(572)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap4_60();

	[DispId(0)]
	string _Default
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	void _VtblGap5_5();

	[DispId(343)]
	bool DisplayAlerts
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(343)]
		[LCIDConversion(0)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[LCIDConversion(0)]
		[DispId(343)]
		[param: In]
		set;
	}

	void _VtblGap6_109();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(302)]
	void Quit();

	void _VtblGap7_50();

	[DispId(392)]
	string Version
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(392)]
		[LCIDConversion(0)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(558)]
	bool Visible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[LCIDConversion(0)]
		[DispId(558)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[LCIDConversion(0)]
		[DispId(558)]
		[param: In]
		set;
	}
}
