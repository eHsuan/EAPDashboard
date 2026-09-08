using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel;

[ComImport]
[CompilerGenerated]
[Guid("000208DA-0000-0000-C000-000000000046")]
[TypeIdentifier]
public interface _Workbook
{
	void _VtblGap1_95();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(283)]
	[LCIDConversion(0)]
	void Save();

	void _VtblGap2_1();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(175)]
	[LCIDConversion(1)]
	void SaveCopyAs([Optional][In][MarshalAs(UnmanagedType.Struct)] object Filename);

	[DispId(298)]
	bool Saved
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(298)]
		[LCIDConversion(0)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[LCIDConversion(0)]
		[DispId(298)]
		[param: In]
		set;
	}

	void _VtblGap3_123();

	[DispId(2528)]
	bool CheckCompatibility
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2528)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2528)]
		[param: In]
		set;
	}

	void _VtblGap4_16();

	[DispId(2541)]
	bool DoNotPromptForConvert
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2541)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2541)]
		[param: In]
		set;
	}
}
