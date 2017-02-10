using System;
using System.IO;

namespace Modem
{
	public class log
	{
		public log()
		{
		}

		public void write(string path, string log)
		{
			try
			{
				FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write);
				StreamWriter streamWriter = new StreamWriter(fileStream);
				DateTime now = DateTime.Now;
				string str = now.ToString();
				string str1 = string.Format("{0} >> {1}\r\n", str, log);
				streamWriter.Write(str1);
				streamWriter.Close();
				fileStream.Close();
			}
			catch
			{
			}
		}

		public void write(string log)
		{
			this.write("LogDial.txt", log);
		}
	}
}