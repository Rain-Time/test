using System;
using System.IO;
using System.Xml.Serialization;

namespace Modem
{
	public class XML
	{
		public bool run;

		public bool on;

		public bool runPWD;

		public string PWD;

		public int times;

		public int wait;

		public string IP;

		public int timeout;

		public int interval;

		public string ConnectName;

		public string username;

		public string password;

		public XML()
		{
		}
        //��xml�ļ�ʱ�跴ϵ�л�����
		public static XML LoadFromXML()
		{
			string str = string.Concat(Environment.CurrentDirectory, "\\settings.xml");
			FileStream fileStream = new FileStream(str, FileMode.Open, FileAccess.Read);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(XML));
			XML xML = (XML)xmlSerializer.Deserialize(fileStream);
			fileStream.Close();
			return xML;
		}
        //�洢xml�ļ�ʱ��ϵ�л�����
		public void SaveAsXML()
		{
			string str = string.Concat(Environment.CurrentDirectory, "\\settings.xml");
			FileStream fileStream = new FileStream(str, FileMode.Create, FileAccess.Write);
			XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
			xmlSerializer.Serialize(fileStream, this);
			fileStream.Flush();
			fileStream.Close();
		}
	}
}