using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Modem
{
	public class DialClass
	{
		private DialClass.RASDIALPARAMS RasDialParams;

		private int Connection;

		public DialClass()
		{
			this.Connection = 0;
			this.RasDialParams = new DialClass.RASDIALPARAMS();
			this.RasDialParams.dwSize = Marshal.SizeOf(this.RasDialParams);
		}

		[DllImport("CreatDial.dll", CharSet=CharSet.None)]
		public static extern int DDial(byte[] m_szEntryName, byte[] m_strUserName, byte[] m_strPassword, byte[] ERRORS);

		[DllImport("CreatDial.dll", CharSet=CharSet.None)]
		public static extern int DEnumConnec(byte[] name);

		[DllImport("CreatDial.dll", CharSet=CharSet.None)]
		public static extern int DHangUp();

		public string Dial(string name)
		{
			string str;
			byte[] bytes;
			byte[] numArray;
			this.Hangup();
			byte[] bytes1 = Encoding.Default.GetBytes(name);
			byte[] numArray1 = new byte[0x101];
			if (name != "SaroCDMA")
			{
				if (name != "SaroGPRS")
				{
					XML xML = XML.LoadFromXML();
					bytes = Encoding.Default.GetBytes(xML.username);
					numArray = Encoding.Default.GetBytes(xML.password);
				}
				else
				{
					bytes = Encoding.Default.GetBytes("");
					numArray = Encoding.Default.GetBytes("");
				}
			}
			else
			{
				bytes = Encoding.Default.GetBytes("CARD");
				numArray = Encoding.Default.GetBytes("CARD");
			}
			int num = DialClass.DDial(bytes1, bytes, numArray, numArray1);
			if (num == 0)
			{
				str = "TRUE";
				log _log = new log();
				_log.write(string.Format("拨号成功,拨号名:{0}", name));
				XML xML1 = XML.LoadFromXML();
				xML1.ConnectName = name;
				xML1.SaveAsXML();
			}
			else
			{
				str = Encoding.Default.GetString(numArray1);
				if (str.IndexOf("\0") != -1)
				{
					str = str.Substring(0, str.IndexOf("\0"));
				}
				str = string.Format("{0}错误,{1}", num.ToString(), str);
				log _log1 = new log();
				_log1.write(string.Format("拨号失败,拨号名:{0},错误信息:{1}", name, str));
			}
			return str;
		}

		public string Dial()
		{
			string currentConnectionName = this.GetCurrentConnectionName();
			string str = this.Dial(currentConnectionName);
			return str;
		}

		public void Dial1()
		{
			this.Connection = 0;
			this.RasDialParams.szEntryName = string.Concat(this.RasDialParams.szEntryName, "\0");
			this.RasDialParams.szUserName = string.Concat(this.RasDialParams.szUserName, "\0");
			this.RasDialParams.szPassword = string.Concat(this.RasDialParams.szPassword, "\0");
			DialClass.RasDial(0, null, ref this.RasDialParams, 0, 0, ref this.Connection);
		}

		public string GetCurrentConnectionName()
		{
			if (Form1.CurrentConnectionName == "")
			{
				XML xML = XML.LoadFromXML();
				if (xML.ConnectName == "")
				{
					return "SaroGPRS";
				}
				else
				{
					return xML.ConnectName;
				}
			}
			else
			{
				return Form1.CurrentConnectionName;
			}
		}

		public void Hangup()
		{
			DialClass.DHangUp();
		}

		[DllImport("Rasapi32.dll", CharSet=CharSet.Auto)]
		public static extern int RasDial(int lpRasDialExtensions, string lpszPhonebook, ref DialClass.RASDIALPARAMS lprasdialparams, int dwNotifierType, int lpvNotifier, ref int lphRasConn);

		[DllImport("Rasapi32.dll", CharSet=CharSet.Auto)]
		public static extern int RasEnumConnections(ref DialClass.RASCONN lprasconn, ref int lpcb, ref int lpcConnections);

		[DllImport("Rasapi32.dll", CharSet=CharSet.Auto)]
		public static extern int RasHangUp(long lphRasConn);

		public struct RASCONN
		{
			public int dwSize;

			public int hrasconn;

			public string szEntryName;

			public string szDeviceType;

			public string szDeviceName;
		}

		public struct RASDIALPARAMS
		{
			public int dwSize;

			public string szEntryName;

			public string szPhoneNumber;

			public string szCallbackNumber;

			public string szUserName;

			public string szPassword;

			public string szDomain;

			public int dwSubEntry;

			public int dwCallbackId;
		}
	}
}