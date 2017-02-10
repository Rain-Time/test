using System;
using System.Runtime.InteropServices;

namespace My.CommBase
{
	internal class Win32Com
	{
		internal const uint ERROR_FILE_NOT_FOUND = 2;

		internal const uint ERROR_INVALID_NAME = 123;

		internal const uint ERROR_ACCESS_DENIED = 5;

		internal const uint ERROR_IO_PENDING = 0x3e5;

		internal const uint ERROR_IO_INCOMPLETE = 0x3e4;

		internal const int INVALID_HANDLE_VALUE = -1;

		internal const uint FILE_FLAG_OVERLAPPED = 0x40000000;

		internal const uint OPEN_EXISTING = 3;

		internal const uint GENERIC_READ = 0x80000000;

		internal const uint GENERIC_WRITE = 0x40000000;

		internal const uint MAXDWORD = 0xffffffff;

		internal const uint EV_RXCHAR = 1;

		internal const uint EV_RXFLAG = 2;

		internal const uint EV_TXEMPTY = 4;

		internal const uint EV_CTS = 8;

		internal const uint EV_DSR = 16;

		internal const uint EV_RLSD = 32;

		internal const uint EV_BREAK = 64;

		internal const uint EV_ERR = 128;

		internal const uint EV_RING = 0x100;

		internal const uint EV_PERR = 0x200;

		internal const uint EV_RX80FULL = 0x400;

		internal const uint EV_EVENT1 = 0x800;

		internal const uint EV_EVENT2 = 0x1000;

		internal const uint SETXOFF = 1;

		internal const uint SETXON = 2;

		internal const uint SETRTS = 3;

		internal const uint CLRRTS = 4;

		internal const uint SETDTR = 5;

		internal const uint CLRDTR = 6;

		internal const uint RESETDEV = 7;

		internal const uint SETBREAK = 8;

		internal const uint CLRBREAK = 9;

		internal const uint MS_CTS_ON = 16;

		internal const uint MS_DSR_ON = 32;

		internal const uint MS_RING_ON = 64;

		internal const uint MS_RLSD_ON = 128;

		internal const uint CE_RXOVER = 1;

		internal const uint CE_OVERRUN = 2;

		internal const uint CE_RXPARITY = 4;

		internal const uint CE_FRAME = 8;

		internal const uint CE_BREAK = 16;

		internal const uint CE_TXFULL = 0x100;

		internal const uint CE_PTO = 0x200;

		internal const uint CE_IOE = 0x400;

		internal const uint CE_DNS = 0x800;

		internal const uint CE_OOP = 0x1000;

		internal const uint CE_MODE = 0x8000;

		public Win32Com()
		{
		}

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool BuildCommDCBAndTimeouts(string lpDef, ref Win32Com.DCB lpDCB, ref Win32Com.COMMTIMEOUTS lpCommTimeouts);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool CancelIo(IntPtr hFile);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool ClearCommError(IntPtr hFile, out uint lpErrors, IntPtr lpStat);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool ClearCommError(IntPtr hFile, out uint lpErrors, out Win32Com.COMSTAT cs);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool EscapeCommFunction(IntPtr hFile, uint dwFunc);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool GetCommModemStatus(IntPtr hFile, out uint lpModemStat);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool GetCommProperties(IntPtr hFile, out Win32Com.COMMPROP cp);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool GetCommState(IntPtr hFile, ref Win32Com.DCB lpDCB);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool GetCommTimeouts(IntPtr hFile, out Win32Com.COMMTIMEOUTS lpCommTimeouts);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool GetOverlappedResult(IntPtr hFile, IntPtr lpOverlapped, out uint nNumberOfBytesTransferred, bool bWait);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, out uint nNumberOfBytesRead, IntPtr lpOverlapped);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool SetCommMask(IntPtr hFile, uint dwEvtMask);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool SetCommState(IntPtr hFile, ref Win32Com.DCB lpDCB);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool SetCommTimeouts(IntPtr hFile, ref Win32Com.COMMTIMEOUTS lpCommTimeouts);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool SetupComm(IntPtr hFile, uint dwInQueue, uint dwOutQueue);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool TransmitCommChar(IntPtr hFile, byte cChar);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool WaitCommEvent(IntPtr hFile, IntPtr lpEvtMask, IntPtr lpOverlapped);

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		internal static extern bool WriteFile(IntPtr fFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

		internal struct COMMPROP
		{
			internal ushort wPacketLength;

			internal ushort wPacketVersion;

			internal uint dwServiceMask;

			internal uint dwReserved1;

			internal uint dwMaxTxQueue;

			internal uint dwMaxRxQueue;

			internal uint dwMaxBaud;

			internal uint dwProvSubType;

			internal uint dwProvCapabilities;

			internal uint dwSettableParams;

			internal uint dwSettableBaud;

			internal ushort wSettableData;

			internal ushort wSettableStopParity;

			internal uint dwCurrentTxQueue;

			internal uint dwCurrentRxQueue;

			internal uint dwProvSpec1;

			internal uint dwProvSpec2;

			internal byte wcProvChar;
		}

		internal struct COMMTIMEOUTS
		{
			internal uint ReadIntervalTimeout;

			internal uint ReadTotalTimeoutMultiplier;

			internal uint ReadTotalTimeoutConstant;

			internal uint WriteTotalTimeoutMultiplier;

			internal uint WriteTotalTimeoutConstant;
		}

		internal struct COMSTAT
		{
			internal const uint fCtsHold = 1;

			internal const uint fDsrHold = 2;

			internal const uint fRlsdHold = 4;

			internal const uint fXoffHold = 8;

			internal const uint fXoffSent = 16;

			internal const uint fEof = 32;

			internal const uint fTxim = 64;

			internal uint Flags;

			internal uint cbInQue;

			internal uint cbOutQue;
		}

		internal struct DCB
		{
			internal int DCBlength;

			internal uint BaudRate;

			internal int PackedValues;

			internal short wReserved;

			internal short XonLim;

			internal short XoffLim;

			internal byte ByteSize;

			internal byte Parity;

			internal byte StopBits;

			internal byte XonChar;

			internal byte XoffChar;

			internal byte ErrorChar;

			internal byte EofChar;

			internal byte EvtChar;

			internal short wReserved1;

			internal void init(bool parity, bool outCTS, bool outDSR, int dtr, bool inDSR, bool txc, bool xOut, bool xIn, int rts)
			{
				this.DCBlength = 28;
				this.PackedValues = 0x4001;
				if (parity)
				{
					Win32Com.DCB packedValues = this;
					packedValues.PackedValues = packedValues.PackedValues | 2;
				}
				if (outCTS)
				{
					Win32Com.DCB dCB = this;
					dCB.PackedValues = dCB.PackedValues | 4;
				}
				if (outDSR)
				{
					Win32Com.DCB packedValues1 = this;
					packedValues1.PackedValues = packedValues1.PackedValues | 8;
				}
				Win32Com.DCB dCB1 = this;
				dCB1.PackedValues = dCB1.PackedValues | (dtr & 3) << 4;
				if (inDSR)
				{
					Win32Com.DCB packedValues2 = this;
					packedValues2.PackedValues = packedValues2.PackedValues | 64;
				}
				if (txc)
				{
					Win32Com.DCB dCB2 = this;
					dCB2.PackedValues = dCB2.PackedValues | 128;
				}
				if (xOut)
				{
					Win32Com.DCB packedValues3 = this;
					packedValues3.PackedValues = packedValues3.PackedValues | 0x100;
				}
				if (xIn)
				{
					Win32Com.DCB dCB3 = this;
					dCB3.PackedValues = dCB3.PackedValues | 0x200;
				}
				Win32Com.DCB packedValues4 = this;
				packedValues4.PackedValues = packedValues4.PackedValues | (rts & 3) << 12;
			}
		}

		internal struct OVERLAPPED
		{
			internal UIntPtr Internal;

			internal UIntPtr InternalHigh;

			internal uint Offset;

			internal uint OffsetHigh;

			internal IntPtr hEvent;
		}
	}
}