using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace cyntec.Tools;

public class cyntecExcel
{
	private object OPT = Missing.Value;

	protected string m_FileName = null;

	protected Application m_Excel;

	public Worksheet this[string idx] => (Worksheet)(dynamic)m_Excel.Sheets[idx];

	public Worksheet this[int idx] => (Worksheet)(dynamic)m_Excel.Sheets[idx];

	public bool DisplayAlerts
	{
		set
		{
			if (m_Excel != null)
			{
				m_Excel.DisplayAlerts = value;
			}
		}
	}

	public string Version
	{
		get
		{
			if (m_Excel != null)
			{
				return m_Excel.Version;
			}
			return "";
		}
	}

	public bool Visible
	{
		get
		{
			if (m_Excel == null)
			{
				return false;
			}
			return m_Excel.Visible;
		}
		set
		{
			if (m_Excel != null)
			{
				m_Excel.Visible = value;
			}
		}
	}

	public bool IsSheetExist(string sheetName)
	{
		try
		{
			Worksheet worksheet = (Worksheet)(dynamic)m_Excel.Sheets[sheetName];
			if (worksheet != null)
			{
				return true;
			}
			return false;
		}
		catch
		{
			return false;
		}
	}

	~cyntecExcel()
	{
	}

	public void CopyFile(string sourceFile, string targetFile)
	{
		File.Copy(sourceFile, targetFile, overwrite: true);
	}

	public virtual void Open(string fileName)
	{
		if (m_Excel == null)
		{
			m_Excel = (Application)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
		}
		if (m_Excel == null)
		{
			throw new Exception("無法開啟office元件(Application)");
		}
		m_Excel.Workbooks.Open(fileName, OPT, OPT, OPT, OPT, OPT, OPT, OPT, OPT, OPT, OPT, OPT, OPT, OPT, OPT);
		m_FileName = fileName;
		if (Version == "14.0")
		{
			m_Excel.ActiveWorkbook.CheckCompatibility = false;
			m_Excel.ActiveWorkbook.DoNotPromptForConvert = true;
		}
	}

	public void Display(string sheetName)
	{
		Worksheet worksheet = (Worksheet)(dynamic)m_Excel.Sheets[sheetName];
		worksheet.Select(OPT);
		m_Excel.Visible = true;
	}

	public void Display()
	{
		m_Excel.Visible = true;
	}

	public void PrintOut(string sheetName)
	{
		Worksheet worksheet = (Worksheet)(dynamic)m_Excel.Sheets[sheetName];
		worksheet.Select(OPT);
		worksheet.PrintOut(OPT, OPT, OPT, OPT, OPT, OPT, OPT, OPT);
	}

	public void CopySheet(string sourceSheet, string targetSheet)
	{
		Worksheet worksheet = null;
		Worksheet worksheet2 = (Worksheet)(dynamic)m_Excel.Sheets[sourceSheet];
		worksheet = (Worksheet)(dynamic)m_Excel.Sheets[1];
		worksheet2.Copy(worksheet, OPT);
		worksheet2 = (Worksheet)(dynamic)m_Excel.Sheets[1];
		worksheet2.Name = targetSheet;
	}

	public void DeleteSheet(string sheetName)
	{
		Worksheet worksheet = (Worksheet)(dynamic)m_Excel.Sheets[sheetName];
		m_Excel.DisplayAlerts = false;
		worksheet.Delete();
		m_Excel.DisplayAlerts = true;
	}

	public void Save()
	{
		m_Excel.ActiveWorkbook.Save();
	}

	public void SaveAs(string FileName)
	{
		DisplayAlerts = false;
		m_Excel.ActiveWorkbook.Saved = true;
		m_Excel.ActiveWorkbook.SaveCopyAs(FileName);
	}

	public void Close()
	{
		m_Excel.DisplayAlerts = false;
		if (m_Excel != null)
		{
			m_Excel.ActiveWorkbook.Saved = true;
			m_Excel.Workbooks.Close();
			m_Excel.Quit();
		}
	}

	public void Close(string sheetName)
	{
		Worksheet worksheet = (Worksheet)(dynamic)m_Excel.Sheets[sheetName];
		m_Excel.DisplayAlerts = false;
		worksheet.Select(OPT);
		m_Excel.ActiveWorkbook.Save();
		m_Excel.Workbooks.Close();
		m_Excel.Quit();
	}

	public void Dispose()
	{
		if (m_Excel != null)
		{
			try
			{
				m_Excel.DisplayAlerts = false;
				m_Excel.ActiveWorkbook.Saved = true;
				m_Excel.Quit();
				Marshal.FinalReleaseComObject(m_Excel);
			}
			catch
			{
			}
			m_Excel = null;
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}

	public object SelectSheet(string sourceSheet)
	{
		Worksheet worksheet = (Worksheet)(dynamic)m_Excel.Sheets[sourceSheet];
		worksheet.Select(OPT);
		return worksheet;
	}

	public void SetCellValue(string sheetName, int posY, int posX, string value)
	{
		Worksheet worksheet = null;
		try
		{
			worksheet = m_Excel.Sheets[sheetName] as Worksheet;
		}
		catch
		{
			throw new Exception("Sheet " + sheetName + " 不存在");
		}
		worksheet.Cells[posY, posX] = value;
	}

	public void SetCellValue(int sheetIdx, int posY, int posX, string value)
	{
		Worksheet worksheet = null;
		try
		{
			worksheet = m_Excel.Sheets[sheetIdx] as Worksheet;
		}
		catch
		{
			throw new Exception("Sheet " + sheetIdx + " 不存在");
		}
		worksheet.Cells[posY, posX] = value;
	}

	public object[,] GetCellValue(string sheetName, int posY, int posX, int PosYend, int posXEnd)
	{
		Worksheet worksheet = null;
		try
		{
			worksheet = m_Excel.Sheets[sheetName] as Worksheet;
		}
		catch
		{
			throw new Exception("Sheet " + sheetName + " 不存在");
		}
		return worksheet.Range[(dynamic)worksheet.Cells[posY, posX], (dynamic)worksheet.Cells[PosYend, posXEnd]].Value;
	}

	public void SetCellValue(string sheetName, int posY, int posX, int PosYend, int posXEnd, object[,] value)
	{
		Worksheet worksheet = null;
		try
		{
			worksheet = m_Excel.Sheets[sheetName] as Worksheet;
		}
		catch
		{
			throw new Exception("Sheet " + sheetName + " 不存在");
		}
		worksheet.Range[(dynamic)worksheet.Cells[posY, posX], (dynamic)worksheet.Cells[PosYend, posXEnd]].Value = value;
	}

	public string GetCell(string sheetName, int posY, int posX)
	{
		Worksheet worksheet = this[sheetName];
		return (worksheet.Cells[posY, posX] as Range).get_Value(Type.Missing)?.ToString();
	}

	public string GetCell(int idx, int posY, int posX)
	{
		Worksheet worksheet = this[idx];
		return (worksheet.Cells[posY, posX] as Range).get_Value(Type.Missing)?.ToString();
	}

	public Dictionary<string, int> ToColumns(string sheetName, int offsetY)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		Worksheet worksheet = this[sheetName];
		int num = 1;
		while (true)
		{
			string text = (string)(dynamic)(worksheet.Cells[offsetY, num] as Range).get_Value(Type.Missing);
			if (text == "" || text == null)
			{
				break;
			}
			try
			{
				if (!dictionary.ContainsKey(text))
				{
					dictionary.Add(text, num);
				}
				num++;
			}
			catch (Exception ex)
			{
				throw new Exception("Excel 欄位不正確(可能有重複的" + ex.Message);
			}
		}
		return dictionary;
	}

	public Dictionary<string, int> ToColumns(int sheetIdx, int offsetY)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		Worksheet worksheet = this[sheetIdx];
		int num = 1;
		while (true)
		{
			string text = (string)(dynamic)(worksheet.Cells[offsetY, num] as Range).get_Value(Type.Missing);
			if (text == "" || text == null)
			{
				break;
			}
			try
			{
				if (!dictionary.ContainsKey(text))
				{
					dictionary.Add(text, num);
				}
				num++;
			}
			catch (Exception ex)
			{
				throw new Exception("Excel 欄位不正確(可能有重複的" + ex.Message);
			}
		}
		return dictionary;
	}

	public void CopyInsertRow(string sourceSheetNaame, int fromRow, int toRow, string targetSheetName, int targetRow)
	{
		Worksheet worksheet = null;
		Worksheet worksheet2 = null;
		worksheet = this[sourceSheetNaame];
		Range range = (Range)(dynamic)worksheet.Rows[fromRow + ":" + toRow, OPT];
		range.Copy(OPT);
		worksheet2 = this[targetSheetName];
		Range range2 = (Range)(dynamic)worksheet2.Rows[targetRow + ":" + targetRow, OPT];
		range2.Insert(OPT, OPT);
	}

	public void RemoveSheet(string sheetName)
	{
		DisplayAlerts = false;
		this[sheetName].Delete();
		DisplayAlerts = true;
	}

	public virtual void RemoveSourceSheet()
	{
	}

	public static string[] GetFileList(string filePath, string searchKey)
	{
		List<string> list = new List<string>();
		DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
		FileInfo[] files = directoryInfo.GetFiles(searchKey);
		FileInfo[] array = files;
		foreach (FileInfo fileInfo in array)
		{
			list.Add(fileInfo.ToString());
		}
		return list.ToArray();
	}
}
