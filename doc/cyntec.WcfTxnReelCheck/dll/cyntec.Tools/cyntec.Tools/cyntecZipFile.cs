using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;

namespace cyntec.Tools;

public class cyntecZipFile : ZipFile
{
	public cyntecZipFile()
	{
	}

	public cyntecZipFile(Encoding encoding)
		: base(encoding)
	{
	}

	public cyntecZipFile(string fileName)
		: base(fileName)
	{
	}

	public cyntecZipFile(string fileName, Encoding encoding)
		: base(fileName, encoding)
	{
	}

	public string ZipDirectory(string directory)
	{
		string[] array = (from f in Directory.GetFiles(directory, "*", SearchOption.AllDirectories)
			where Path.GetExtension(f).ToLowerInvariant() != ".zip"
			select f).ToArray();
		string[] array2 = array;
		foreach (string text in array2)
		{
			((ZipFile)this).AddFile(text, Path.GetDirectoryName(text).Replace(directory, string.Empty));
		}
		string text2 = Path.ChangeExtension(directory, ".zip");
		((ZipFile)this).Save(text2);
		return text2;
	}
}
