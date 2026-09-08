using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel;

[ComImport]
[CompilerGenerated]
[Guid("00020846-0000-0000-C000-000000000046")]
[InterfaceType(2)]
[TypeIdentifier]
public interface Range
{
	void _VtblGap1_35();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(551)]
	[return: MarshalAs(UnmanagedType.Struct)]
	object Copy([Optional][In][MarshalAs(UnmanagedType.Struct)] object Destination);

	void _VtblGap2_9();

	[IndexerName("_Default")]
	[DispId(0)]
	object this[[Optional][In][MarshalAs(UnmanagedType.Struct)] object RowIndex, [Optional][In][MarshalAs(UnmanagedType.Struct)] object ColumnIndex]
	{
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[param: Optional]
		[param: In]
		[param: MarshalAs(UnmanagedType.Struct)]
		set;
	}

	void _VtblGap3_44();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(252)]
	[return: MarshalAs(UnmanagedType.Struct)]
	object Insert([Optional][In][MarshalAs(UnmanagedType.Struct)] object Shift, [Optional][In][MarshalAs(UnmanagedType.Struct)] object CopyOrigin);

	void _VtblGap4_81();

	[DispId(6)]
	object Value
	{
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[param: Optional]
		[param: In]
		[param: MarshalAs(UnmanagedType.Struct)]
		set;
	}
}
