using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace cyntec.Tools;

public class cyntecHTTP
{
	public string URL = "http";

	public string JsonData = "jsonData";

	public int Timeout = 10;

	public Encoding encoding = Encoding.UTF8;

	public string Method = "POST";

	public string ContentType = "text/xml";

	public string ReplyString = "";

	public bool IsHttpError = false;

	public void DoPost()
	{
		ReplyString = HttpPost(URL, JsonData, Timeout, encoding, ref IsHttpError);
	}

	public static string HttpPost(string posturl, string postData, int Timeout, Encoding encoding, ref bool IsHttpPostError)
	{
		IsHttpPostError = false;
		byte[] bytes = encoding.GetBytes(postData);
		try
		{
			HttpWebRequest httpWebRequest = null;
			GC.Collect();
			httpWebRequest = WebRequest.Create(posturl) as HttpWebRequest;
			CookieContainer cookieContainer = new CookieContainer();
			httpWebRequest.AllowAutoRedirect = true;
			httpWebRequest.Method = "POST";
			httpWebRequest.Timeout = Timeout;
			httpWebRequest.Proxy = null;
			httpWebRequest.ContentType = "text/xml";
			httpWebRequest.ContentLength = bytes.Length;
			httpWebRequest.KeepAlive = true;
			httpWebRequest.CookieContainer = cookieContainer;
			ServicePointManager.DefaultConnectionLimit = 200;
			using (Stream stream = httpWebRequest.GetRequestStream())
			{
				stream.Write(bytes, 0, bytes.Length);
				stream.Close();
			}
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream, encoding);
			string text = streamReader.ReadToEnd();
			string empty = string.Empty;
			streamReader.Close();
			if (httpWebRequest.ContentType == "application/json")
			{
				return text;
			}
			XDocument xDocument = XDocument.Parse(text);
			return xDocument.Root.Value;
		}
		catch (Exception ex)
		{
			IsHttpPostError = true;
			return ex.ToString();
		}
	}
}
