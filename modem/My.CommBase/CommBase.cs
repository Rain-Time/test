using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace My.CommBase
{
	public abstract class CommBase : IDisposable
	{
		private IntPtr hPort;

		private IntPtr ptrUWO;

		private Thread rxThread;

		private bool online;

		private bool auto;

		private bool checkSends;

		private Exception rxException;

		private bool rxExceptionReported;

		private uint writeCount;

		private ManualResetEvent writeEvent;

		private ManualResetEvent startEvent;

		private int stateRTS;

		private int stateDTR;

		private int stateBRK;

		private bool[] empty;

		private bool dataQueued;

		protected bool Break
		{
			get
			{
				return this.stateBRK == 1;
			}
			set
			{
				if (this.stateBRK <= 1)
				{
					this.CheckOnline();
					if (!value)
					{
						if (!Win32Com.EscapeCommFunction(this.hPort, 9))
						{
							this.ThrowException("Unexpected Failure");
							return;
						}
						else
						{
							this.stateBRK = 0;
							return;
						}
					}
					else
					{
						if (!Win32Com.EscapeCommFunction(this.hPort, 8))
						{
							this.ThrowException("Unexpected Failure");
							return;
						}
						else
						{
							this.stateBRK = 0;
							return;
						}
					}
				}
				else
				{
					return;
				}
			}
		}

		protected bool DTR
		{
			get
			{
				return this.stateDTR == 1;
			}
			set
			{
				if (this.stateDTR <= 1)
				{
					this.CheckOnline();
					if (!value)
					{
						if (!Win32Com.EscapeCommFunction(this.hPort, 6))
						{
							this.ThrowException("Unexpected Failure");
							return;
						}
						else
						{
							this.stateDTR = 0;
							return;
						}
					}
					else
					{
						if (!Win32Com.EscapeCommFunction(this.hPort, 5))
						{
							this.ThrowException("Unexpected Failure");
							return;
						}
						else
						{
							this.stateDTR = 1;
							return;
						}
					}
				}
				else
				{
					return;
				}
			}
		}

		protected bool DTRavailable
		{
			get
			{
				return this.stateDTR < 2;
			}
		}

		public bool Online
		{
			get
			{
				if (this.online)
				{
					return this.CheckOnline();
				}
				else
				{
					return false;
				}
			}
		}

		protected bool RTS
		{
			get
			{
				return this.stateRTS == 1;
			}
			set
			{
				if (this.stateRTS <= 1)
				{
					this.CheckOnline();
					if (!value)
					{
						if (!Win32Com.EscapeCommFunction(this.hPort, 4))
						{
							this.ThrowException("Unexpected Failure");
							return;
						}
						else
						{
							this.stateRTS = 0;
							return;
						}
					}
					else
					{
						if (!Win32Com.EscapeCommFunction(this.hPort, 3))
						{
							this.ThrowException("Unexpected Failure");
							return;
						}
						else
						{
							this.stateRTS = 1;
							return;
						}
					}
				}
				else
				{
					return;
				}
			}
		}

		protected bool RTSavailable
		{
			get
			{
				return this.stateRTS < 2;
			}
		}

		protected CommBase()
		{
			this.ptrUWO = IntPtr.Zero;
			this.checkSends = true;
			this.writeEvent = new ManualResetEvent(false);
			this.startEvent = new ManualResetEvent(false);
			this.stateRTS = 2;
			this.stateDTR = 2;
			this.stateBRK = 2;
			this.empty = new bool[1];
		}

		protected virtual bool AfterOpen()
		{
			return true;
		}

		private string AltName(string s)
		{
			s.Trim();
			if (s.EndsWith(":"))
			{
				s = s.Substring(0, s.Length - 1);
			}
			if (!s.StartsWith("\\"))
			{
				return string.Concat("\\\\.\\", s);
			}
			else
			{
				return s;
			}
		}

		protected virtual void BeforeClose(bool error)
		{
		}

		private bool CheckOnline()
		{
			if (this.rxException != null && !this.rxExceptionReported)
			{
				this.rxExceptionReported = true;
				this.ThrowException("rx");
			}
			if (!this.online)
			{
				if (!this.auto || !this.Open())
				{
					this.ThrowException("Offline");
					return false;
				}
				else
				{
					return true;
				}
			}
			else
			{
				if (this.hPort == (IntPtr)(-1))
				{
					this.ThrowException("Offline");
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		private void CheckResult()
		{
			uint num = 0;
			if (this.writeCount > 0)
			{
				if (!Win32Com.GetOverlappedResult(this.hPort, this.ptrUWO, out num, this.checkSends))
				{
					if ((long)Marshal.GetLastWin32Error() != (long)0x3e4)
					{
						this.ThrowException("Write Error");
					}
				}
				else
				{
					if (this.checkSends)
					{
						CommBase commBase = this;
						commBase.writeCount = commBase.writeCount - num;
						if (this.writeCount != 0)
						{
							this.ThrowException("Send Timeout");
						}
						this.writeCount = 0;
						return;
					}
				}
			}
		}

		public void Close()
		{
			if (this.online)
			{
				this.auto = false;
				this.BeforeClose(false);
				this.InternalClose();
				this.rxException = null;
			}
		}

		protected virtual CommBase.CommBaseSettings CommSettings()
		{
			return new CommBase.CommBaseSettings();
		}

		public void Dispose()
		{
			this.Close();
		}

	/*	protected override void Finalize()
		{
			try
			{
				this.Close();
			}
			finally
			{
				//Finalize();
			}
		}
     */
		public void Flush()
		{
			this.CheckOnline();
			this.CheckResult();
		}

		protected CommBase.ModemStatus GetModemStatus()
		{
			uint num = 0;
			this.CheckOnline();
			if (!Win32Com.GetCommModemStatus(this.hPort, out num))
			{
				this.ThrowException("Unexpected failure");
			}
			return new CommBase.ModemStatus(num);
		}

		protected CommBase.QueueStatus GetQueueStatus()
		{
			Win32Com.COMSTAT cOMSTAT;
			Win32Com.COMMPROP cOMMPROP;
			uint num = 0;
			this.CheckOnline();
			if (!Win32Com.ClearCommError(this.hPort, out num, out cOMSTAT))
			{
				this.ThrowException("Unexpected failure");
			}
			if (!Win32Com.GetCommProperties(this.hPort, out cOMMPROP))
			{
				this.ThrowException("Unexpected failure");
			}
			return new CommBase.QueueStatus(cOMSTAT.Flags, cOMSTAT.cbInQue, cOMSTAT.cbOutQue, cOMMPROP.dwCurrentRxQueue, cOMMPROP.dwCurrentTxQueue);
		}

		private void InternalClose()
		{
			Win32Com.CancelIo(this.hPort);
			if (this.rxThread != null)
			{
				this.rxThread.Abort();
				this.rxThread.Join(100);
				this.rxThread = null;
			}
			Win32Com.CloseHandle(this.hPort);
			if (this.ptrUWO != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.ptrUWO);
			}
			this.stateRTS = 2;
			this.stateDTR = 2;
			this.stateBRK = 2;
			this.online = false;
		}

		protected bool IsCongested()
		{
			bool flag;
			if (this.dataQueued)
			{
				lock (this.empty)
				{
					flag = this.empty[0];
					this.empty[0] = false;
				}
				this.dataQueued = false;
				return !flag;
			}
			else
			{
				return false;
			}
		}

		public CommBase.PortStatus IsPortAvailable(string s)
		{
			IntPtr intPtr = Win32Com.CreateFile(s, 0xc0000000, 0, IntPtr.Zero, 3, 0x40000000, IntPtr.Zero);
			if (intPtr == (IntPtr)(-1))
			{
				if ((long)Marshal.GetLastWin32Error() != (long)5)
				{
                    intPtr = Win32Com.CreateFile(this.AltName(s), 0xc0000000, 0, IntPtr.Zero, 3, 0x40000000, IntPtr.Zero);
					if (intPtr == (IntPtr)(-1))
					{
						if ((long)Marshal.GetLastWin32Error() != (long)5)
						{
							return CommBase.PortStatus.absent;
						}
						else
						{
							return CommBase.PortStatus.unavailable;
						}
					}
				}
				else
				{
					return CommBase.PortStatus.unavailable;
				}
			}
			Win32Com.CloseHandle(intPtr);
			return CommBase.PortStatus.available;
		}

		protected virtual void OnBreak()
		{
		}

		protected virtual void OnRxChar(byte ch)
		{
		}

		protected virtual void OnRxException(Exception e)
		{
		}

		protected virtual void OnStatusChange(CommBase.ModemStatus mask, CommBase.ModemStatus state)
		{
		}

		protected virtual void OnTxDone()
		{
		}

		public bool Open()
		{
			Win32Com.COMMPROP cOMMPROP;
			bool flag;
			Win32Com.DCB xoffChar = new Win32Com.DCB();
			Win32Com.COMMTIMEOUTS cOMMTIMEOUT = new Win32Com.COMMTIMEOUTS();
			Win32Com.OVERLAPPED zero = new Win32Com.OVERLAPPED();
			if (!this.online)
			{
				CommBase.CommBaseSettings commBaseSetting = this.CommSettings();
				this.hPort = Win32Com.CreateFile(commBaseSetting.port, 0xc0000000, 0, IntPtr.Zero, 3, 0x40000000, IntPtr.Zero);
				if (this.hPort == (IntPtr)(-1))
				{
					if ((long)Marshal.GetLastWin32Error() != (long)5)
					{
                        this.hPort = Win32Com.CreateFile(this.AltName(commBaseSetting.port), 0xc0000000, 0, IntPtr.Zero, 3, 0x40000000, IntPtr.Zero);
						if (this.hPort == (IntPtr)(-1))
						{
							if ((long)Marshal.GetLastWin32Error() != (long)5)
							{
								throw new CommPortException("Port Open Failure");
							}
							else
							{
								return false;
							}
						}
					}
					else
					{
						return false;
					}
				}
				this.online = true;
				cOMMTIMEOUT.ReadIntervalTimeout = 0xffffffff;
				cOMMTIMEOUT.ReadTotalTimeoutConstant = 0;
				cOMMTIMEOUT.ReadTotalTimeoutMultiplier = 0;
				if (commBaseSetting.sendTimeoutMultiplier != 0)
				{
					cOMMTIMEOUT.WriteTotalTimeoutMultiplier = commBaseSetting.sendTimeoutMultiplier;
				}
				else
				{
					if (Environment.OSVersion.Platform != PlatformID.Win32NT)
					{
						cOMMTIMEOUT.WriteTotalTimeoutMultiplier = 0x2710;
					}
					else
					{
						cOMMTIMEOUT.WriteTotalTimeoutMultiplier = 0;
					}
				}
				cOMMTIMEOUT.WriteTotalTimeoutConstant = commBaseSetting.sendTimeoutConstant;
                Win32Com.DCB dCBPointer;
			     dCBPointer = xoffChar;
				if (commBaseSetting.parity == CommBase.Parity.odd)
				{
					flag = true;
				}
				else
				{
					flag = commBaseSetting.parity == CommBase.Parity.even;
				}
				dCBPointer.init(flag, commBaseSetting.txFlowCTS, commBaseSetting.txFlowDSR,(int) commBaseSetting.useDTR, commBaseSetting.rxGateDSR, !commBaseSetting.txWhenRxXoff, commBaseSetting.txFlowX, commBaseSetting.rxFlowX, (int)commBaseSetting.useRTS);
				xoffChar.BaudRate = commBaseSetting.baudRate;
				xoffChar.ByteSize = (byte)commBaseSetting.dataBits;
				xoffChar.Parity = (byte)commBaseSetting.parity;
				xoffChar.StopBits = (byte)commBaseSetting.stopBits;
				xoffChar.XoffChar = (byte)commBaseSetting.XoffChar;
                xoffChar.XonChar = (byte)commBaseSetting.XonChar;
				if ((commBaseSetting.rxQueue != 0 || commBaseSetting.txQueue != 0) && !Win32Com.SetupComm(this.hPort,(uint) commBaseSetting.rxQueue,(uint) commBaseSetting.txQueue))
				{
					this.ThrowException("Bad queue settings");
				}
				if (commBaseSetting.rxLowWater == 0 || commBaseSetting.rxHighWater == 0)
				{
					if (!Win32Com.GetCommProperties(this.hPort, out cOMMPROP))
					{
						cOMMPROP.dwCurrentRxQueue = 0;
					}
					if (cOMMPROP.dwCurrentRxQueue <= 0)
					{
						byte num = 8;
						short num1 = (short)num;
						xoffChar.XonLim = (short)num;
						xoffChar.XoffLim = num1;
					}
					else
					{
						short num2 = (short)(cOMMPROP.dwCurrentRxQueue / 10);
						short num3 = num2;
						xoffChar.XonLim = num2;
						xoffChar.XoffLim = num3;
					}
				}
				else
				{
					xoffChar.XoffLim = (short)commBaseSetting.rxHighWater;
					xoffChar.XonLim = (short)commBaseSetting.rxLowWater;
				}
				if (!Win32Com.SetCommState(this.hPort, ref xoffChar))
				{
					this.ThrowException("Bad com settings");
				}
				if (!Win32Com.SetCommTimeouts(this.hPort, ref cOMMTIMEOUT))
				{
					this.ThrowException("Bad timeout settings");
				}
				this.stateBRK = 0;
				if (commBaseSetting.useDTR == CommBase.HSOutput.none)
				{
					this.stateDTR = 0;
				}
				if (commBaseSetting.useDTR == CommBase.HSOutput.online)
				{
					this.stateDTR = 1;
				}
				if (commBaseSetting.useRTS == CommBase.HSOutput.none)
				{
					this.stateRTS = 0;
				}
				if (commBaseSetting.useRTS == CommBase.HSOutput.online)
				{
					this.stateRTS = 1;
				}
				this.checkSends = commBaseSetting.checkAllSends;
				zero.Offset = 0;
				zero.OffsetHigh = 0;
				if (!this.checkSends)
				{
					zero.hEvent = IntPtr.Zero;
				}
				else
				{
					zero.hEvent = this.writeEvent.Handle;
                   // zero.hEvent = this.writeEvent.SafeWaitHandle;

				}
				this.ptrUWO = Marshal.AllocHGlobal(Marshal.SizeOf(zero));
				Marshal.StructureToPtr(zero, this.ptrUWO, true);
				this.writeCount = 0;
				this.empty[0] = true;
				this.dataQueued = false;
				this.rxException = null;
				this.rxExceptionReported = false;
				this.rxThread = new Thread(new ThreadStart(this.ReceiveThread));
				this.rxThread.Name = "CommBaseRx";
				this.rxThread.Priority = ThreadPriority.AboveNormal;
				this.rxThread.Start();
				this.startEvent.WaitOne(0x1f4, false);
				this.auto = false;
				if (!this.AfterOpen())
				{
					this.Close();
					return false;
				}
				else
				{
					this.auto = commBaseSetting.autoReopen;
					return true;
				}
			}
			else
			{
				return false;
			}
		}

		private void ReceiveThread()
		{
			uint num;
			uint num1 = 0;
			uint num2 = 0;
			byte[] numArray = new byte[1];
			bool flag = true;
			AutoResetEvent autoResetEvent = new AutoResetEvent(false);
			Win32Com.OVERLAPPED handle = new Win32Com.OVERLAPPED();
			uint num3 = 0;
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(handle));
			IntPtr intPtr1 = Marshal.AllocHGlobal(Marshal.SizeOf(num3));
			handle.Offset = 0;
			handle.OffsetHigh = 0;
			handle.hEvent = autoResetEvent.Handle;
			Marshal.StructureToPtr(handle, intPtr, true);
			try
			{
				while (Win32Com.SetCommMask(this.hPort, 0x1fd))
				{
					Marshal.WriteInt32(intPtr1, 0);
					if (flag)
					{
						this.startEvent.Set();
						flag = false;
					}
					if (!Win32Com.WaitCommEvent(this.hPort, intPtr1, intPtr))
					{
						if ((long)Marshal.GetLastWin32Error() != (long)0x3e5)
						{
							throw new CommPortException("IO Error [002]");
						}
						else
						{
							autoResetEvent.WaitOne();
						}
					}
					num3 =(uint) Marshal.ReadInt32(intPtr1);
					if ((num3 & 128) != 0)
					{
						if (!Win32Com.ClearCommError(this.hPort, out num1, IntPtr.Zero))
						{
							throw new CommPortException("IO Error [003]");
						}
						else
						{
							int num4 = 0;
							StringBuilder stringBuilder = new StringBuilder("UART Error: ", 40);
							if ((num1 & 8) != 0)
							{
								stringBuilder = stringBuilder.Append("Framing,");
								num4++;
							}
							if ((num1 & 0x400) != 0)
							{
								stringBuilder = stringBuilder.Append("IO,");
								num4++;
							}
							if ((num1 & 2) != 0)
							{
								stringBuilder = stringBuilder.Append("Overrun,");
								num4++;
							}
							if ((num1 & 1) != 0)
							{
								stringBuilder = stringBuilder.Append("Receive Cverflow,");
								num4++;
							}
							if ((num1 & 4) != 0)
							{
								stringBuilder = stringBuilder.Append("Parity,");
								num4++;
							}
							if ((num1 & 0x100) != 0)
							{
								stringBuilder = stringBuilder.Append("Transmit Overflow,");
								num4++;
							}
							if (num4 <= 0)
							{
								if (num1 != 16)
								{
									throw new CommPortException("IO Error [003]");
								}
								else
								{
									num3 = num3 | 64;
								}
							}
							else
							{
								stringBuilder.Length = stringBuilder.Length - 1;
								throw new CommPortException(stringBuilder.ToString());
							}
						}
					}
					if ((num3 & 1) != 0)
					{
						do
						{
							num = 0;
							if (Win32Com.ReadFile(this.hPort, numArray, 1, out num, intPtr))
							{
								if (num != 1)
								{
									continue;
								}
								this.OnRxChar(numArray[0]);
							}
							else
							{
								Marshal.GetLastWin32Error();
								throw new CommPortException("IO Error [004]");
							}
						}
						while (num > 0);
					}
					if ((num3 & 4) != 0)
					{
						lock (this.empty)
						{
							this.empty[0] = true;
						}
						this.OnTxDone();
					}
					if ((num3 & 64) != 0)
					{
						this.OnBreak();
					}
					uint num5 = 0;
					if ((num3 & 8) != 0)
					{
						num5 = num5 | 16;
					}
					if ((num3 & 16) != 0)
					{
						num5 = num5 | 32;
					}
					if ((num3 & 32) != 0)
					{
						num5 = num5 | 128;
					}
					if ((num3 & 0x100) != 0)
					{
						num5 = num5 | 64;
					}
					if (num5 == 0)
					{
						continue;
					}
					if (Win32Com.GetCommModemStatus(this.hPort, out num2))
					{
						this.OnStatusChange(new CommBase.ModemStatus(num5), new CommBase.ModemStatus(num2));
					}
					else
					{
						throw new CommPortException("IO Error [005]");
					}
				}
				throw new CommPortException("IO Error [001]");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				Win32Com.CancelIo(this.hPort);
				if (intPtr1 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr1);
				}
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (exception as ThreadAbortException == null)
				{
					this.rxException = exception;
					this.OnRxException(exception);
				}
			}
		}

		protected void Send(byte[] tosend)
		{
			uint num = 0;
			this.CheckOnline();
			this.CheckResult();
			this.writeCount =(uint) tosend.GetLength(0);
			if (!Win32Com.WriteFile(this.hPort, tosend,(uint) this.writeCount, out num, this.ptrUWO))
			{
				if ((long)Marshal.GetLastWin32Error() != (long)0x3e5)
				{
					this.ThrowException("Send failed");
				}
				this.dataQueued = true;
				return;
			}
			else
			{
				CommBase commBase = this;
				commBase.writeCount = commBase.writeCount - num;
				return;
			}
		}

		protected void Send(byte tosend)
		{
			byte[] numArray = new byte[1];
			numArray[0] = tosend;
			this.Send(numArray);
		}

		protected void SendImmediate(byte tosend)
		{
			this.CheckOnline();
			if (!Win32Com.TransmitCommChar(this.hPort, tosend))
			{
				this.ThrowException("Transmission failure");
			}
		}

		protected void Sleep(int milliseconds)
		{
			Thread.Sleep(milliseconds);
		}

		protected void ThrowException(string reason)
		{
			if (Thread.CurrentThread != this.rxThread)
			{
				if (this.online)
				{
					this.BeforeClose(true);
					this.InternalClose();
				}
				if (this.rxException != null)
				{
					throw new CommPortException(this.rxException);
				}
				else
				{
					throw new CommPortException(reason);
				}
			}
			else
			{
				throw new CommPortException(reason);
			}
		}

		public enum ASCII : byte
		{
			NULL,
			SOH,
			STX,
			ETX,
			EOT,
			ENQ,
			ACK,
			BELL,
			BS,
			HT,
			LF,
			VT,
			FF,
			CR,
			SO,
			SI,
			DC1,
			DC2,
			DC3,
			DC4,
			NAK,
			SYN,
			ETB,
			CAN,
			EM,
			SUB,
			ESC,
			FS,
			GS,
			RS,
			US,
			SP,
			DEL
		}

		public class CommBaseSettings
		{
			public string port;

			public uint baudRate;

			public CommBase.Parity parity;

			public uint dataBits;

			public CommBase.StopBits stopBits;

			public bool txFlowCTS;

			public bool txFlowDSR;

			public bool txFlowX;

			public bool txWhenRxXoff;

			public bool rxGateDSR;

			public bool rxFlowX;

			public CommBase.HSOutput useRTS;

			public CommBase.HSOutput useDTR;

			public CommBase.ASCII XonChar;

			public CommBase.ASCII XoffChar;

			public int rxHighWater;

			public int rxLowWater;

			public uint sendTimeoutMultiplier;

			public uint sendTimeoutConstant;

			public uint rxQueue;

			public uint txQueue;

			public bool autoReopen;

			public bool checkAllSends;

			public CommBaseSettings()
			{
				this.port = "COM1:";
				this.baudRate = 0xe100;
				this.dataBits = 8;
				this.txWhenRxXoff = true;
				this.XonChar = CommBase.ASCII.DC1;
				this.XoffChar = CommBase.ASCII.DC3;
				this.checkAllSends = true;
			}

			public static CommBase.CommBaseSettings LoadFromXML(Stream s)
			{
				return CommBase.CommBaseSettings.LoadFromXML(s, typeof(CommBase.CommBaseSettings));
			}

			protected static CommBase.CommBaseSettings LoadFromXML(Stream s, Type t)
			{
				CommBase.CommBaseSettings commBaseSetting;
				XmlSerializer xmlSerializer = new XmlSerializer(t);
				try
				{
					commBaseSetting = (CommBase.CommBaseSettings)xmlSerializer.Deserialize(s);
				}
				catch
				{
					commBaseSetting = null;
				}
				return commBaseSetting;
			}

			public void SaveAsXML(Stream s)
			{
				XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
				xmlSerializer.Serialize(s, this);
			}

			public void SetStandard(string Port, uint Baud, CommBase.Handshake Hs)
			{
				this.dataBits = 8;
				this.stopBits = CommBase.StopBits.one;
				this.parity = CommBase.Parity.none;
				this.port = Port;
				this.baudRate = Baud;
				CommBase.Handshake hs = Hs;
				switch (hs)
				{
					case CommBase.Handshake.none:
					{
						this.txFlowCTS = false;
						this.txFlowDSR = false;
						this.txFlowX = false;
						this.rxFlowX = false;
						this.useRTS = CommBase.HSOutput.online;
						this.useDTR = CommBase.HSOutput.online;
						this.txWhenRxXoff = true;
						this.rxGateDSR = false;
						return;
					}
					case CommBase.Handshake.XonXoff:
					{
						this.txFlowCTS = false;
						this.txFlowDSR = false;
						this.txFlowX = true;
						this.rxFlowX = true;
						this.useRTS = CommBase.HSOutput.online;
						this.useDTR = CommBase.HSOutput.online;
						this.txWhenRxXoff = true;
						this.rxGateDSR = false;
						this.XonChar = CommBase.ASCII.DC1;
						this.XoffChar = CommBase.ASCII.DC3;
						return;
					}
					case CommBase.Handshake.CtsRts:
					{
						this.txFlowCTS = true;
						this.txFlowDSR = false;
						this.txFlowX = false;
						this.rxFlowX = false;
						this.useRTS = CommBase.HSOutput.handshake;
						this.useDTR = CommBase.HSOutput.online;
						this.txWhenRxXoff = true;
						this.rxGateDSR = false;
						return;
					}
					case CommBase.Handshake.DsrDtr:
					{
						this.txFlowCTS = false;
						this.txFlowDSR = true;
						this.txFlowX = false;
						this.rxFlowX = false;
						this.useRTS = CommBase.HSOutput.online;
						this.useDTR = CommBase.HSOutput.handshake;
						this.txWhenRxXoff = true;
						this.rxGateDSR = false;
						return;
					}
					default:
					{
						return;
					}
				}
			}
		}

		public enum Handshake
		{
			none,
			XonXoff,
			CtsRts,
			DsrDtr
		}

		public enum HSOutput
		{
			none = 0,
			online = 1,
			handshake = 2,
			gate = 3
		}

		public struct ModemStatus
		{
			private uint status;

			public bool cts
			{
				get
				{
					return (this.status & 16) != 0;
				}
			}

			public bool dsr
			{
				get
				{
					return (this.status & 32) != 0;
				}
			}

			public bool ring
			{
				get
				{
					return (this.status & 64) != 0;
				}
			}

			public bool rlsd
			{
				get
				{
					return (this.status & 128) != 0;
				}
			}

			internal ModemStatus(uint val)
			{
				this.status = val;
			}
		}

		public enum Parity
		{
			none,
			odd,
			even,
			mark,
			space
		}

		public enum PortStatus
		{
			absent = -1,
			unavailable = 0,
			available = 1
		}

		public struct QueueStatus
		{
			private uint status;

			private uint inQueue;

			private uint outQueue;

			private uint inQueueSize;

			private uint outQueueSize;

			public bool ctsHold
			{
				get
				{
					return (this.status & 1) != 0;
				}
			}

			public bool dsrHold
			{
				get
				{
					return (this.status & 2) != 0;
				}
			}

			public bool immediateWaiting
			{
				get
				{
					return (this.status & 64) != 0;
				}
			}

			public ulong InQueue
			{
				get
				{
					return (ulong)this.inQueue;
				}
			}

			public ulong InQueueSize
			{
				get
				{
					return (ulong)this.inQueueSize;
				}
			}

			public ulong OutQueue
			{
				get
				{
					return (ulong)this.outQueue;
				}
			}

			public ulong OutQueueSize
			{
				get
				{
					return (ulong)this.outQueueSize;
				}
			}

			public bool rlsdHold
			{
				get
				{
					return (this.status & 4) != 0;
				}
			}

			public bool xoffHold
			{
				get
				{
					return (this.status & 8) != 0;
				}
			}

			public bool xoffSent
			{
				get
				{
					return (this.status & 16) != 0;
				}
			}

			internal QueueStatus(uint stat, uint inQ, uint outQ, uint inQs, uint outQs)
			{
				this.status = stat;
				this.inQueue = inQ;
				this.outQueue = outQ;
				this.inQueueSize = inQs;
				this.outQueueSize = outQs;
			}

			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder("The reception queue is ", 60);
				if (this.inQueueSize != 0)
				{
					stringBuilder.Append(string.Concat(this.inQueueSize.ToString(), " bytes long and "));
				}
				else
				{
					stringBuilder.Append("of unknown size and ");
				}
				if (this.inQueue != 0)
				{
					if (this.inQueue != 1)
					{
						stringBuilder.Append("contains ");
						stringBuilder.Append(this.inQueue.ToString());
						stringBuilder.Append(" bytes.");
					}
					else
					{
						stringBuilder.Append("contains 1 byte.");
					}
				}
				else
				{
					stringBuilder.Append("is empty.");
				}
				stringBuilder.Append(" The transmission queue is ");
				if (this.outQueueSize != 0)
				{
					stringBuilder.Append(string.Concat(this.outQueueSize.ToString(), " bytes long and "));
				}
				else
				{
					stringBuilder.Append("of unknown size and ");
				}
				if (this.outQueue != 0)
				{
					if (this.outQueue != 1)
					{
						stringBuilder.Append("contains ");
						stringBuilder.Append(this.outQueue.ToString());
						stringBuilder.Append(" bytes. It is ");
					}
					else
					{
						stringBuilder.Append("contains 1 byte. It is ");
					}
				}
				else
				{
					stringBuilder.Append("is empty");
				}
				if (this.outQueue > 0)
				{
					if (this.ctsHold || this.dsrHold || this.rlsdHold || this.xoffHold || this.xoffSent)
					{
						stringBuilder.Append("holding on");
						if (this.ctsHold)
						{
							stringBuilder.Append(" CTS");
						}
						if (this.dsrHold)
						{
							stringBuilder.Append(" DSR");
						}
						if (this.rlsdHold)
						{
							stringBuilder.Append(" RLSD");
						}
						if (this.xoffHold)
						{
							stringBuilder.Append(" Rx XOff");
						}
						if (this.xoffSent)
						{
							stringBuilder.Append(" Tx XOff");
						}
					}
					else
					{
						stringBuilder.Append("pumping data");
					}
				}
				stringBuilder.Append(". The immediate buffer is ");
				if (!this.immediateWaiting)
				{
					stringBuilder.Append("empty.");
				}
				else
				{
					stringBuilder.Append("full.");
				}
				return stringBuilder.ToString();
			}
		}

		public enum StopBits
		{
			one,
			onePointFive,
			two
		}
	}
}