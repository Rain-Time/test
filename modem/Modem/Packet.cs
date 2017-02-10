using System;

namespace Modem
{
	public class Packet
	{
		private byte[] m_Raw;

		private DateTime m_Time;

		private int m_HeaderLength;

		private Precedence m_Precedence;

		private Delay m_Delay;

		private Throughput m_Throughput;

		private Reliability m_Reliability;

		private int m_TotalLength;

		private int m_Identification;

		private int m_TimeToLive;

		private Protocol m_Protocol;

		private byte[] m_Checksum;

		private string m_SourceAddress;

		private string m_DestinationAddress;

		private int m_SourcePort;

		private int m_DestinationPort;

		public string Destination
		{
			get
			{
				if (this.m_DestinationPort == -1)
				{
					return this.DestinationAddress.ToString();
				}
				else
				{
					return string.Concat(this.DestinationAddress.ToString(), ":", this.m_DestinationPort.ToString());
				}
			}
		}

		public string DestinationAddress
		{
			get
			{
				return this.m_DestinationAddress;
			}
		}

		public Protocol Protocol
		{
			get
			{
				return this.m_Protocol;
			}
		}

		public string Source
		{
			get
			{
				if (this.m_SourcePort == -1)
				{
					return this.SourceAddress.ToString();
				}
				else
				{
					return string.Concat(this.SourceAddress.ToString(), ":", this.m_SourcePort.ToString());
				}
			}
		}

		public string SourceAddress
		{
			get
			{
				return this.m_SourceAddress;
			}
		}

		public DateTime Time
		{
			get
			{
				return this.m_Time;
			}
		}

		public int TotalLength
		{
			get
			{
				return this.m_TotalLength;
			}
		}

		public Packet()
		{
		}

		public Packet(byte[] raw, DateTime time)
		{
			if (raw != null)
			{
				if ((int)raw.Length >= 20)
				{
					this.m_Raw = raw;
					this.m_Time = time;
					this.m_HeaderLength = (raw[0] & 15) * 4;
					if ((raw[0] & 15) >= 5)
					{
						this.m_Precedence = (Precedence)((raw[1] & 224) >> 5);
						this.m_Delay = (Delay)((raw[1] & 16) >> 4);
						this.m_Throughput = (Throughput)((raw[1] & 8) >> 3);
						this.m_Reliability = (Reliability)((raw[1] & 4) >> 2);
						this.m_TotalLength = raw[2] * 0x100 + raw[3];
						if (this.m_TotalLength == (int)raw.Length)
						{
							this.m_Identification = raw[4] * 0x100 + raw[5];
							this.m_TimeToLive = raw[8];
							this.m_Protocol = (Protocol)raw[9];
							this.m_Checksum = new byte[2];
							this.m_Checksum[0] = raw[11];
							this.m_Checksum[1] = raw[10];
							try
							{
								this.m_SourceAddress = this.GetIPAddress(raw, 12);
								this.m_DestinationAddress = this.GetIPAddress(raw, 16);
							}
							catch
							{
								throw;
							}
							if (this.m_Protocol == Protocol.Tcp || this.m_Protocol == Protocol.Udp)
							{
								this.m_SourcePort = raw[this.m_HeaderLength] * 0x100 + raw[this.m_HeaderLength + 1];
								this.m_DestinationPort = raw[this.m_HeaderLength + 2] * 0x100 + raw[this.m_HeaderLength + 3];
								return;
							}
							else
							{
								this.m_SourcePort = -1;
								this.m_DestinationPort = -1;
								return;
							}
						}
						else
						{
							throw new ArgumentException();
						}
					}
					else
					{
						throw new ArgumentException();
					}
				}
				else
				{
					throw new ArgumentException();
				}
			}
			else
			{
				throw new ArgumentNullException();
			}
		}

		public string GetIPAddress(byte[] bArray, int nStart)
		{
			byte[] numArray = new byte[4];
			if ((int)bArray.Length > nStart + 2)
			{
				numArray[0] = bArray[nStart];
				numArray[1] = bArray[nStart + 1];
				numArray[2] = bArray[nStart + 2];
				numArray[3] = bArray[nStart + 3];
			}
			object[] objArray = new object[7];
			objArray[0] = numArray[0];
			objArray[1] = ".";
			objArray[2] = numArray[1];
			objArray[3] = ".";
			objArray[4] = numArray[2];
			objArray[5] = ".";
			objArray[6] = numArray[3];
			return string.Concat(objArray);
		}
	}
}