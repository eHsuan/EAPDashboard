using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel;

[ComImport]
[CompilerGenerated]
[Guid("000208D8-0000-0000-C000-000000000046")]
[TypeIdentifier]
public interface _Worksheet
{
	void _VtblGap1_4();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[LCIDConversion(2)]
	[DispId(551)]
	void Copy([Optional][In][MarshalAs(UnmanagedType.Struct)] object Before, [Optional][In][MarshalAs(UnmanagedType.Struct)] object After);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[LCIDConversion(0)]
	[DispId(117)]
	void Delete();

	void _VtblGap2_5();

	[DispId(110)]
	string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(110)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(110)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	void _VtblGap3_17();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[LCIDConversion(1)]
	[DispId(235)]
	void Select([Optional][In][MarshalAs(UnmanagedType.Struct)] object Replace);

	void _VtblGap4_14();

	[DispId(238)]
	Range Cells
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(238)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap5_47();

	[DispId(197)]
	Range Range
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(197)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap6_1();

	[DispId(258)]
	Range Rows
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(258)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap7_30();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[LCIDConversion(8)]
	[DispId(1772)]
	void PrintOut([Optional][In][MarshalAs(UnmanagedType.Struct)] object From, [Optional][In][MarshalAs(UnmanagedType.Struct)] object To, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Copies, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Preview, [Optional][In][MarshalAs(UnmanagedType.Struct)] object ActivePrinter, [Optional][In][MarshalAs(UnmanagedType.Struct)] object PrintToFile, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Collate, [Optional][In][MarshalAs(UnmanagedType.Struct)] object PrToFileName);
}
