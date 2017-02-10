using Microsoft.VisualBasic.Devices;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Modem
{
	public class DialManager
	{
		private string _ip;

		private int _interval;

		private int _interval2;

		private int _maxcount;

		private int _timeout;

		private bool _isrun;

		private bool on;

		public static BackgroundWorker bw;

		private Network network;

		public int Interval
		{
			get
			{
				return this._interval;
			}
			set
			{
				this._interval = value;
			}
		}

		public int Interval2
		{
			get
			{
				return this._interval2;
			}
			set
			{
				this._interval2 = value;
			}
		}

		public string IP
		{
			get
			{
				return this._ip;
			}
			set
			{
				this._ip = value;
			}
		}

		public bool IsRun
		{
			get
			{
				return this._isrun;
			}
		}

		public int MaxCount
		{
			get
			{
				return this._maxcount;
			}
			set
			{
				this._maxcount = value;
			}
		}

		public int Timeout
		{
			get
			{
				return this._timeout;
			}
			set
			{
				this._timeout = value;
			}
		}

		public DialManager()
		{
			this.network = new Network();
		}

		private bool Dial()
		{
			bool flag;
			DialClass dialClass = new DialClass();
			string currentConnectionName = dialClass.GetCurrentConnectionName();
			Form1.count = Form1.count + 1;
			Form1.SetText(6, string.Concat("重拨次数:", Form1.count.ToString()));
			Form1.SetText(1, ">>正在拨号.......\r\n");
			Form1.SetText(1, string.Concat(">>正在连接到", currentConnectionName, ".......\r\n"));
			string str = dialClass.Dial(currentConnectionName);
			if (str != "TRUE")
			{
				Form1.SetText(1, string.Concat(">>拨号失败,错误:", str, "\r\n"));
				flag = false;
			}
			else
			{
				Form1.CurrentConnectionName = currentConnectionName;
				Form1.SetText(1, ">>拨号成功,已连接");
				flag = true;
				Form1.SetText(5, "状态:在线");
				Form1.SetText(7, string.Concat("当前连接:", currentConnectionName));
				XML xML = XML.LoadFromXML();
				xML.ConnectName = currentConnectionName;
				xML.SaveAsXML();
			}
			return flag;
		}

		private bool IsOnline()
		{
			char[] chrArray = new char[2];
			chrArray[0] = ',';
			chrArray[1] = ';';
			string[] strArrays = this._ip.Split(chrArray);
			log _log = new log();
			for (int i = 0; i < 5; i++)
			{
				string[] strArrays1 = strArrays;
				int num = 0;
				while (num < (int)strArrays1.Length)
				{
					string str = strArrays1[num];
					bool flag = false;
					try
					{
						_log.write(string.Format("Ping:<{0}>,Timeout:<{1}>", str.Trim(), this._timeout + i * 0x3e8));
						flag = this.network.Ping(str.Trim(), this._timeout + i * 0x3e8);
					}
					catch (Exception exception1)
					{
						Exception exception = exception1;
						_log.write(string.Format("Ping ERROR:{0}", exception.Message));
						flag = false;
					}
					if (!flag)
					{
						_log.write(string.Format("Ping Fail! retry times:{0}", i.ToString()));
						num++;
					}
					else
					{
						_log.write("Ping Success");
						bool flag1 = true;
						return flag1;
					}
				}
			}
			_log.write(string.Format("Retry 4 times,Ping:<{0}>,all is fail", this._ip));
			return false;
		}

		public void Start()
		{
			string str = string.Concat(Environment.CurrentDirectory, "\\settings.xml");
			if (File.Exists(str))
			{
				XML xML = XML.LoadFromXML();
				this._maxcount = xML.times;
				char[] chrArray = new char[1];
				chrArray[0] = ' ';
				this._ip = xML.IP.Trim(chrArray);
				this._timeout = xML.timeout;
				this._interval = xML.interval;
				this._interval2 = xML.wait;
				this.on = xML.on;
				if (!this.on || this._isrun)
				{
					return;
				}
				else
				{
					this._isrun = true;
					while (true)
					{
						Thread.Sleep(this._interval * 0x3e8);
						try
						{
							if (!this.IsOnline())
							{
								Form1.SetText(1, ">>已断线....正准备重拨\r\n");
								log _log = new log();
								_log.write("已断线....正准备重拨");
								Form1.SetText(5, "状态:断线");
								while (!this.Dial() && !this.IsOnline())
								{
									Thread.Sleep(0xbb8);
								}
							}
						}
						catch
						{
						}
					}
				}
			}
			else
			{
				MessageBox.Show("找不到参数设置文件，请确认当前目录存在settings.xml文件");
				return;
			}
		}
	}
}