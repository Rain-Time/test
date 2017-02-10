using System;
using System.IO;
using System.Threading;

namespace My.CommBase
{
	public abstract class CommPingPong : CommBase
	{
		private byte[] RxByte;

		private ManualResetEvent TransFlag;

		private int TransTimeout;

		protected CommPingPong()
		{
			this.TransFlag = new ManualResetEvent(true);
		}

		protected override void OnRxChar(byte ch)
		{
			lock (this.RxByte)
			{
				this.RxByte[0] = ch;
			}
			if (!this.TransFlag.WaitOne(0, false))
			{
				this.TransFlag.Set();
			}
		}

		protected void Setup(CommPingPong.CommPingPongSettings s)
		{
			this.TransTimeout = s.transactTimeout;
		}

		protected byte Transact(byte toSend)
		{
			byte rxByte;
			if (this.RxByte == null)
			{
				this.RxByte = new byte[1];
			}
			base.Send(toSend);
			this.TransFlag.Reset();
			if (!this.TransFlag.WaitOne(this.TransTimeout, false))
			{
				base.ThrowException("Timeout");
			}
			lock (this.RxByte)
			{
				rxByte = this.RxByte[0];
			}
			return rxByte;
		}

		public class CommPingPongSettings : CommBase.CommBaseSettings
		{
			public int transactTimeout;

			public CommPingPongSettings()
			{
				this.transactTimeout = 0x1f4;
			}

			public static CommPingPong.CommPingPongSettings LoadFromXML(Stream s)
			{
				return (CommPingPong.CommPingPongSettings)CommBase.CommBaseSettings.LoadFromXML(s, typeof(CommPingPong.CommPingPongSettings));
			}
		}
	}
}