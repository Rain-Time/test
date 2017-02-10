using System;
using System.IO;
using System.Text;
using System.Threading;

namespace My.CommBase
{
	public abstract class CommLine : CommBase
	{
		private byte[] RxBuffer;

		private uint RxBufferP;

		private CommBase.ASCII RxTerm;

		private CommBase.ASCII[] TxTerm;

		private CommBase.ASCII[] RxFilter;

		private string RxString;

		private ManualResetEvent TransFlag;

		private int TransTimeout;

		protected CommLine()
		{
			this.RxString = "";
			this.TransFlag = new ManualResetEvent(true);
		}

		protected override void OnRxChar(byte ch)
		{
			unsafe
			{
				CommBase.ASCII aSCII = (CommBase.ASCII)ch;
				if (aSCII == this.RxTerm || this.RxBufferP > this.RxBuffer.GetUpperBound(0))
				{
					lock (this.RxString)
					{
						this.RxString = Encoding.ASCII.GetString(this.RxBuffer, 0, (int)this.RxBufferP);
					}
					this.RxBufferP = 0;
					if (!this.TransFlag.WaitOne(0, false))
					{
						this.TransFlag.Set();
						return;
					}
					else
					{
						this.OnRxLine(this.RxString);
						return;
					}
				}
				else
				{
					bool flag = true;
					if (this.RxFilter != null)
					{
						for (int i = 0; i <= this.RxFilter.GetUpperBound(0); i++)
						{
							if (this.RxFilter[i] == aSCII)
							{
								flag = false;
							}
						}
					}
					if (flag)
					{
						this.RxBuffer[this.RxBufferP] = ch;
						CommLine rxBufferP = this;
						rxBufferP.RxBufferP = rxBufferP.RxBufferP + 1;
					}
					return;
				}
			}
		}

		protected virtual void OnRxLine(string s)
		{
		}

		protected void Send(string toSend)
		{
			unsafe
			{
				uint byteCount =(uint) Encoding.ASCII.GetByteCount(toSend);
				if (this.TxTerm != null)
				{
					byteCount = byteCount +(uint) this.TxTerm.GetLength(0);
				}
				byte[] txTerm = new byte[byteCount];
				byte[] bytes = Encoding.ASCII.GetBytes(toSend);
				for (int i = 0; i <= bytes.GetUpperBound(0); i++)
				{
					txTerm[i] = bytes[i];
				}
				if (this.TxTerm != null)
				{
                    int num = 0;int i = 0;
					while (num <= this.TxTerm.GetUpperBound(0))
					{
						txTerm[i] =(byte) this.TxTerm[num];
						num++;
						i++;
					}
				}
				base.Send(txTerm);
			}
		}

		protected void Setup(CommLine.CommLineSettings s)
		{
			this.RxBuffer = new byte[s.rxStringBufferSize];
			this.RxTerm = s.rxTerminator;
			this.RxFilter = s.rxFilter;
			this.TransTimeout =(int) s.transactTimeout;
			this.TxTerm = s.txTerminator;
		}

		protected string Transact(string toSend)
		{
			string rxString;
			this.Send(toSend);
			this.TransFlag.Reset();
			if (!this.TransFlag.WaitOne(this.TransTimeout, false))
			{
				base.ThrowException("Timeout");
			}
			lock (this.RxString)
			{
				rxString = this.RxString;
			}
			return rxString;
		}

		public class CommLineSettings : CommBase.CommBaseSettings
		{
			public int rxStringBufferSize;

			public CommBase.ASCII rxTerminator;

			public CommBase.ASCII[] rxFilter;

			public uint transactTimeout;

			public CommBase.ASCII[] txTerminator;

			public CommLineSettings()
			{
				this.rxStringBufferSize = 0x100;
				this.rxTerminator = CommBase.ASCII.CR;
				this.transactTimeout = 0x1f4;
			}

			public static CommLine.CommLineSettings LoadFromXML(Stream s)
			{
				return (CommLine.CommLineSettings)CommBase.CommBaseSettings.LoadFromXML(s, typeof(CommLine.CommLineSettings));
			}
		}
	}
}