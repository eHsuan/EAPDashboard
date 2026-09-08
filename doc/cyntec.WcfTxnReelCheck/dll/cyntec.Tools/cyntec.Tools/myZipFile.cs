using System.Collections.Generic;
using System.IO;
using System.Linq;
using cyntec.Base;
using Ionic.Zip;

namespace cyntec.Tools;

public class myZipFile
{
	public int TotalFileQty = 0;

	public int ProcessFileQty = 0;

	public event HOnLog onZipProgess;

	public static void Zip(string ZipFilePath, List<string> sourceFile)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		ZipFile val = new ZipFile();
		foreach (string item in sourceFile)
		{
			val.AddFile(item, "");
		}
		val.Save(ZipFilePath);
	}

	public string ZipDirectory(string directory)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		ZipFile val = new ZipFile();
		val.SaveProgress += LoanZip_SaveProgress;
		string[] array = (from f in Directory.GetFiles(directory, "*", SearchOption.AllDirectories)
			where Path.GetExtension(f).ToLowerInvariant() != ".zip"
			select f).ToArray();
		string[] array2 = array;
		foreach (string text in array2)
		{
			val.AddFile(text, Path.GetDirectoryName(text).Replace(directory, string.Empty));
		}
		TotalFileQty = val.Count;
		string text2 = Path.ChangeExtension(directory, ".zip");
		val.Save(text2);
		return text2;
	}

	private void LoanZip_SaveProgress(object sender, SaveProgressEventArgs e)
	{
		if (this.onZipProgess != null && e != null && ((ZipProgressEventArgs)e).CurrentEntry != null)
		{
			ProcessFileQty++;
			this.onZipProgess(this, ((ZipProgressEventArgs)e).CurrentEntry.FileName);
		}
	}
}
