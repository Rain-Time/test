using System;
using System.Net;
using System.Net.Sockets;

namespace Modem
{
	public class Monitor
	{
		private const int IOC_VENDOR = 0x18000000;

		private const int IOC_IN = -2147483648;

		private const int SIO_RCVALL = -1744830463;

		private const int SECURITY_BUILTIN_DOMAIN_RID = 32;

		private const int DOMAIN_ALIAS_RID_ADMINS = 0x220;

		private Socket m_Monitor;

		private IPAddress m_Ip;

		private byte[] m_Buffer;

		public byte[] Buffer
		{
			get
			{
				return this.m_Buffer;
			}
		}

		public IPAddress IP
		{
			get
			{
				return this.m_Ip;
			}
		}

		public Monitor()
		{
			this.m_Buffer = new byte[0xffff];
		}

		public Monitor(IPAddress IpAddress)
		{
			this.m_Buffer = new byte[0xffff];
			if (Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Version.Major >= 5)
			{
				this.m_Ip = IpAddress;
				return;
			}
			else
			{
				throw new NotSupportedException("This program requires Windows 2000, Windows XP or Windows .NET Server!");
			}
		}

		protected void OnNewPacket(Packet p)
		{
			this.NewPacket(this, p);
		}

		public void OnReceive(IAsyncResult ar)
		{
			try
			{
				int num = this.m_Monitor.EndReceive(ar);
				try
				{
					if (this.m_Monitor != null)
					{
						byte[] numArray = new byte[num];
						Array.Copy(this.Buffer, 0, numArray, 0, num);
						this.OnNewPacket(new Packet(numArray, DateTime.Now));
					}
				}
				catch
				{
					throw;
				}
				this.m_Monitor.BeginReceive(this.Buffer, 0, (int)this.Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), null);
			}
			catch
			{
			}
		}

		public void Start()
		{
			if (this.m_Monitor == null)
			{
				try
				{
					this.m_Monitor = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
					this.m_Monitor.Bind(new IPEndPoint(this.IP, 0));
					this.m_Monitor.IOControl(-1744830463, BitConverter.GetBytes(1), null);
					this.m_Monitor.BeginReceive(this.m_Buffer, 0, (int)this.m_Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), null);
				}
				catch
				{
					this.m_Monitor = null;
				}
			}
		}

		public void Stop()
		{
			if (this.m_Monitor != null)
			{
				this.m_Monitor.Close();
			}
			this.m_Monitor = null;
		}

		public event Monitor.NewPacketEventHandler NewPacket;

		public delegate void NewPacketEventHandler(Monitor m, Packet p);
	}
}