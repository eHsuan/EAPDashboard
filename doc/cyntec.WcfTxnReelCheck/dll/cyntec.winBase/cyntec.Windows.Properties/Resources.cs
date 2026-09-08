using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace cyntec.Windows.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				ResourceManager resourceManager = new ResourceManager("cyntec.Windows.Properties.Resources", typeof(Resources).Assembly);
				resourceMan = resourceManager;
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static Bitmap _1328585032_accept
	{
		get
		{
			object obj = ResourceManager.GetObject("1328585032_accept", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap accepted_48
	{
		get
		{
			object obj = ResourceManager.GetObject("accepted_48", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap Error
	{
		get
		{
			object obj = ResourceManager.GetObject("Error", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap messagebox_warning
	{
		get
		{
			object obj = ResourceManager.GetObject("messagebox_warning", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap OK
	{
		get
		{
			object obj = ResourceManager.GetObject("OK", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap warning_48
	{
		get
		{
			object obj = ResourceManager.GetObject("warning_48", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal Resources()
	{
	}
}
