using My.CommBase;
using System;

namespace Modem
{
	public class Class1 : CommBase
	{
		public CommBase.CommBaseSettings s;

		public Class1()
		{
			this.s = new CommBase.CommBaseSettings();
		}

		protected override CommBase.CommBaseSettings CommSettings()
		{
			return this.s;
		}

		protected override void OnRxChar(byte c)
		{
			if (this.OnReceiveChar != null)
			{
				this.OnReceiveChar(c);
			}
		}

		public void SendByte(byte[] by)
		{
			base.Send(by);
		}

		public void SendChar(byte c)
		{
			base.SendImmediate(c);
		}

		public event ComEvent OnReceiveChar;
	}
}