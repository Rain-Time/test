using System;
using System.IO;
using System.Windows.Forms;

namespace Modem
{
	public class PBK
	{
		public PBK()
		{
		}

		public void DO(string xxx)
		{
			string str = this.getpath();
			if (File.Exists(str))
			{
				FileStream fileStream = new FileStream(str, FileMode.Open, FileAccess.ReadWrite);
				StreamReader streamReader = new StreamReader(fileStream);
				StreamWriter streamWriter = new StreamWriter(fileStream);
				string end = streamReader.ReadToEnd();
				string str1 = end.Substring(0, end.IndexOf(xxx));
				string str2 = end.Substring(end.IndexOf(xxx));
				if (str2.IndexOf("[", 2) != -1)
				{
					str2 = str2.Substring(0, str2.IndexOf("[", 2));
				}
				if (str2.IndexOf("HwFlowControl=1") == -1)
				{
					str2 = string.Concat(str2, "HwFlowControl=0\r\n");
				}
				else
				{
					str2 = str2.Replace("HwFlowControl=1", "HwFlowControl=0");
				}
				if (str2.IndexOf("Protocol=1") == -1)
				{
					str2 = string.Concat(str2, "Protocol=0\r\n");
				}
				else
				{
					str2 = str2.Replace("Protocol=1", "Protocol=0");
				}
				if (str2.IndexOf("Compression=1") == -1)
				{
					str2 = string.Concat(str2, "Compression=0\r\n");
				}
				else
				{
					str2 = str2.Replace("Compression=1", "Compression=0");
				}
				fileStream.Seek((long)0, SeekOrigin.Begin);
				streamWriter.Write(string.Concat(str1, str2));
				streamWriter.Close();
				streamReader.Close();
				fileStream.Close();
				return;
			}
			else
			{
				MessageBox.Show("未找到pbk文件");
				return;
			}
		}

		private string getpath()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			return string.Concat(folderPath, "\\Microsoft\\Network\\Connections\\Pbk\\rasphone.pbk");
		}
	}
}