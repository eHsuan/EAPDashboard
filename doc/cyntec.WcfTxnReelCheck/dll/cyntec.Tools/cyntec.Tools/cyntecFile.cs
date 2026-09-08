using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace cyntec.Tools;

public class cyntecFile
{
	public FileStream fsWrite = null;

	public StreamWriter oWriter = null;

	public Encoding oEncoding = Encoding.UTF8;

	public bool IsOverwrite = false;

	public bool IsReadOnly = false;

	public string FilePath;

	public static List<string> ErrorFileList = new List<string>();

	public long FileWirterPosition
	{
		get
		{
			if (fsWrite == null)
			{
				return 0L;
			}
			return fsWrite.Position;
		}
	}

	public void init()
	{
		if (!IsReadOnly)
		{
			if (string.IsNullOrWhiteSpace(FilePath))
			{
				throw new Exception("未指定路徑");
			}
			if (IsFileExist(FilePath) && !IsOverwrite)
			{
				fsWrite = new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
			}
			else
			{
				fsWrite = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			}
			oWriter = new StreamWriter(fsWrite, oEncoding);
		}
	}

	public void Close()
	{
		if (oWriter != null)
		{
			oWriter.Close();
			fsWrite.Close();
			oWriter.Dispose();
			fsWrite.Dispose();
			oWriter = null;
			fsWrite = null;
		}
	}

	public void Write(string msg)
	{
		if (fsWrite == null)
		{
			init();
		}
		oWriter.Write(msg);
		oWriter.Flush();
	}

	public void WriteLine(string msg)
	{
		if (fsWrite == null)
		{
			init();
		}
		oWriter.WriteLine(msg);
		oWriter.Flush();
	}

	public void WriteLineTrim(string msg)
	{
		WriteLine(msg.Trim());
	}

	public string ReadAllText()
	{
		if (!IsFileExist(FilePath))
		{
			return null;
		}
		FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		StreamReader streamReader = new StreamReader(fileStream, oEncoding);
		try
		{
			if (fsWrite == null)
			{
				init();
			}
			return streamReader.ReadToEnd();
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			fileStream.Close();
			streamReader.Close();
		}
	}

	public string[] ReadLines()
	{
		List<string> list = new List<string>();
		if (!IsFileExist(FilePath))
		{
			return list.ToArray();
		}
		FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		StreamReader streamReader = new StreamReader(fileStream, oEncoding);
		try
		{
			while (true)
			{
				string text = streamReader.ReadLine();
				if (text == null)
				{
					break;
				}
				list.Add(text);
			}
			return list.ToArray();
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			fileStream.Close();
			streamReader.Close();
		}
	}

	public void ClearFile()
	{
		if (fsWrite != null)
		{
			fsWrite.Seek(0L, SeekOrigin.Begin);
			fsWrite.SetLength(0L);
		}
	}

	public static string[] GetFileList(string filePath, string searchPattern)
	{
		List<string> list = new List<string>();
		DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
		FileInfo[] files = directoryInfo.GetFiles(searchPattern);
		FileInfo[] array = files;
		foreach (FileInfo fileInfo in array)
		{
			list.Add(fileInfo.ToString());
		}
		return list.ToArray();
	}

	public static string ToFullPath(string path, string fileName)
	{
		if (path.EndsWith("\\"))
		{
			return path + fileName;
		}
		return path + "\\" + fileName;
	}

	public static void Move(string sourcePath, string sourceFileName, string targetPath, string targetFileName, EFileMoveMode moveMode)
	{
		string sourceFileName2 = Path.Combine(sourcePath, sourceFileName);
		string text = Path.Combine(targetPath, targetFileName);
		DirectoryInfo directoryInfo = new DirectoryInfo(targetPath);
		if (!directoryInfo.Exists)
		{
			Directory.CreateDirectory(targetPath);
		}
		if (File.Exists(text))
		{
			switch (moveMode)
			{
			case EFileMoveMode.Replace:
				File.Delete(text);
				break;
			case EFileMoveMode.Rename:
			{
				FileInfo fileInfo = new FileInfo(text);
				int length = fileInfo.Name.Length - fileInfo.Extension.Length;
				text = Path.Combine(targetPath, fileInfo.Name.Substring(0, length) + Guid.NewGuid().ToString() + fileInfo.Extension);
				break;
			}
			case EFileMoveMode.Exception:
				throw new Exception("File Exist: " + text);
			}
		}
		File.Move(sourceFileName2, text);
	}

	public static void CopyFile(string sourceFile, string targetFile)
	{
		File.Copy(sourceFile, targetFile, overwrite: true);
	}

	public static FileInfo[] GetFileInfoList(string dir, string searchPattern)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(dir);
		if (!directoryInfo.Exists)
		{
			return null;
		}
		return directoryInfo.GetFiles(searchPattern);
	}

	public static void Delete(string fullName)
	{
		File.Delete(fullName);
	}

	public static List<string> Purge(double ExpireDay, string path, string searchPattern)
	{
		List<string> list = new List<string>();
		FileInfo[] fileInfoList = GetFileInfoList(path, searchPattern);
		if (fileInfoList == null)
		{
			return list;
		}
		FileInfo[] array = fileInfoList;
		foreach (FileInfo fileInfo in array)
		{
			TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - fileInfo.LastWriteTime.Ticks);
			if (!(timeSpan.TotalDays > ExpireDay) || fileInfo.IsReadOnly || ErrorFileList.Contains(fileInfo.FullName))
			{
				continue;
			}
			try
			{
				File.Delete(fileInfo.FullName);
				list.Add(fileInfo.FullName);
			}
			catch
			{
				if (ErrorFileList.Count >= 10000)
				{
					ErrorFileList.RemoveAt(0);
				}
				ErrorFileList.Add(fileInfo.FullName);
			}
		}
		return list;
	}

	public static bool IsDirExist(string dir)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(dir);
		return directoryInfo.Exists;
	}

	public static bool IsFileExist(string filePath)
	{
		return File.Exists(filePath);
	}

	public static string ToAppBasePath(params object[] items)
	{
		return ToPath(AppDomain.CurrentDomain.BaseDirectory, items);
	}

	public static string ToPath(string rootPath, params object[] items)
	{
		string text = rootPath;
		if (items != null)
		{
			foreach (object obj in items)
			{
				text = Path.Combine(text, obj.ToString());
			}
		}
		return text;
	}

	public static void CreateDir(string dir)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(dir);
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
		}
	}

	public static string GetAsssemblyInfo(Type assy)
	{
		string location = assy.Assembly.Location;
		FileInfo fileInfo = new FileInfo(location);
		return assy.Assembly.FullName + ", " + fileInfo.CreationTime.ToString();
	}

	public static void LogMsg(string msg)
	{
	}

	public static string ToFileName(string fullPath)
	{
		string[] array = fullPath.Split(new string[1] { "\\" }, StringSplitOptions.None);
		if (array.Length != 0)
		{
			return array[array.Length - 1];
		}
		return "";
	}

	public static string ToFilePath(string fullPath)
	{
		string text = ToFileName(fullPath);
		if (text.Length == fullPath.Length)
		{
			return fullPath;
		}
		return fullPath.Substring(0, fullPath.Length - text.Length);
	}
}
