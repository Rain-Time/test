using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Modem
{
	public class NotifyIconEx : Component
	{
		private uint m_id;

		private IntPtr m_handle;

		private static NotifyIconEx.NotifyIconTarget m_messageSink;

		private static uint m_nextId;

		private string m_text;

		private Icon m_icon;

		private ContextMenu m_contextMenu;

		private bool m_visible;

		private bool m_doubleClick;

		public ContextMenu ContextMenu
		{
			get
			{
				return this.m_contextMenu;
			}
			set
			{
				this.m_contextMenu = value;
			}
		}

		public Icon Icon
		{
			get
			{
				return this.m_icon;
			}
			set
			{
				this.m_icon = value;
				this.CreateOrUpdate();
			}
		}

		public string Text
		{
			get
			{
				return this.m_text;
			}
			set
			{
				if (this.m_text != value)
				{
					this.m_text = value;
					this.CreateOrUpdate();
				}
			}
		}

		public bool Visible
		{
			get
			{
				return this.m_visible;
			}
			set
			{
				if (this.m_visible != value)
				{
					this.m_visible = value;
					this.CreateOrUpdate();
				}
			}
		}

		static NotifyIconEx()
		{
			NotifyIconEx.m_messageSink = new NotifyIconEx.NotifyIconTarget();
			NotifyIconEx.m_nextId = 1;
		}

		public NotifyIconEx()
		{
			this.m_text = "";
		}

		private void Create(uint id)
		{
			NotifyIconEx.NotifyIconData mHandle = new NotifyIconEx.NotifyIconData();
			mHandle.cbSize =(uint) Marshal.SizeOf(mHandle);
			this.m_handle = NotifyIconEx.m_messageSink.Handle;
			mHandle.hWnd = this.m_handle;
			this.m_id = id;
			mHandle.uID = this.m_id;
			mHandle.uCallbackMessage = 0x400;
			mHandle.uFlags = mHandle.uFlags | NotifyIconEx.NotifyFlags.Message;
			mHandle.hIcon = this.m_icon.Handle;
			mHandle.uFlags = mHandle.uFlags | NotifyIconEx.NotifyFlags.Icon;
			mHandle.szTip = this.m_text;
			mHandle.uFlags = mHandle.uFlags | NotifyIconEx.NotifyFlags.Tip;
			if (!this.m_visible)
			{
				mHandle.dwState = NotifyIconEx.NotifyState.Hidden;
			}
			mHandle.dwStateMask = mHandle.dwStateMask | NotifyIconEx.NotifyState.Hidden;
			NotifyIconEx.Shell_NotifyIcon(NotifyIconEx.NotifyCommand.Add, ref mHandle);
			NotifyIconEx.m_messageSink.ClickNotify += new NotifyIconEx.NotifyIconTarget.NotifyIconHandler(this.OnClick);
			NotifyIconEx.m_messageSink.DoubleClickNotify += new NotifyIconEx.NotifyIconTarget.NotifyIconHandler(this.OnDoubleClick);
			NotifyIconEx.m_messageSink.RightClickNotify += new NotifyIconEx.NotifyIconTarget.NotifyIconHandler(this.OnRightClick);
			NotifyIconEx.m_messageSink.ClickBalloonNotify += new NotifyIconEx.NotifyIconTarget.NotifyIconHandler(this.OnClickBalloon);
			NotifyIconEx.m_messageSink.TaskbarCreated += new EventHandler(this.OnTaskbarCreated);
		}

		private void CreateOrUpdate()
		{
			if (!base.DesignMode)
			{
				if (this.m_id != 0)
				{
					this.Update();
				}
				else
				{
					if (this.m_icon != null)
					{
						uint mNextId = NotifyIconEx.m_nextId;
						NotifyIconEx.m_nextId = mNextId + 1;
						this.Create(mNextId);
						return;
					}
				}
				return;
			}
			else
			{
				return;
			}
		}

		protected override void Dispose(bool disposing)
		{
			this.Remove();
			base.Dispose(disposing);
		}

		[DllImport("User32.Dll", CharSet=CharSet.None)]
		private static extern int GetCursorPos(ref NotifyIconEx.POINT point);

		private void OnClick(object sender, uint id)
		{
			if (id == this.m_id)
			{
				if (!this.m_doubleClick && this.Click != null)
				{
					this.Click(this, EventArgs.Empty);
				}
				this.m_doubleClick = false;
			}
		}

		private void OnClickBalloon(object sender, uint id)
		{
			if (id == this.m_id && this.BalloonClick != null)
			{
				this.BalloonClick(this, EventArgs.Empty);
			}
		}

		private void OnDoubleClick(object sender, uint id)
		{
			if (id == this.m_id)
			{
				this.m_doubleClick = true;
				if (this.DoubleClick != null)
				{
					this.DoubleClick(this, EventArgs.Empty);
				}
			}
		}

		private void OnRightClick(object sender, uint id)
		{
			if (id == this.m_id && this.m_contextMenu != null)
			{
				NotifyIconEx.POINT pOINT = new NotifyIconEx.POINT();
				NotifyIconEx.GetCursorPos(ref pOINT);
				NotifyIconEx.SetForegroundWindow(NotifyIconEx.m_messageSink.Handle);
				object[] empty = new object[1];
				empty[0] = EventArgs.Empty;
				this.m_contextMenu.GetType().InvokeMember("OnPopup", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, this.m_contextMenu, empty);
				NotifyIconEx.TrackPopupMenuEx(this.m_contextMenu.Handle, 64, pOINT.x, pOINT.y, NotifyIconEx.m_messageSink.Handle, IntPtr.Zero);
			}
		}

		private void OnTaskbarCreated(object sender, EventArgs e)
		{
			if (this.m_id != 0)
			{
				this.Create(this.m_id);
			}
		}

		public void Remove()
		{
			if (this.m_id != 0)
			{
				NotifyIconEx.NotifyIconData mHandle = new NotifyIconEx.NotifyIconData();
				mHandle.cbSize =(uint) Marshal.SizeOf(mHandle);
				mHandle.hWnd = this.m_handle;
				mHandle.uID = this.m_id;
				NotifyIconEx.Shell_NotifyIcon(NotifyIconEx.NotifyCommand.Delete, ref mHandle);
				this.m_id = 0;
			}
		}

		[DllImport("User32.Dll", CharSet=CharSet.None)]
		private static extern int SetForegroundWindow(IntPtr hWnd);

		[DllImport("shell32.Dll", CharSet=CharSet.None)]
		private static extern int Shell_NotifyIcon(NotifyIconEx.NotifyCommand cmd, ref NotifyIconEx.NotifyIconData data);

		public void ShowBalloon(string title, string text, NotifyIconEx.NotifyInfoFlags type, int timeoutInMilliSeconds)
		{
			if (timeoutInMilliSeconds >= 0)
			{
				NotifyIconEx.NotifyIconData handle = new NotifyIconEx.NotifyIconData();
				handle.cbSize =(uint) Marshal.SizeOf(handle);
				handle.hWnd = NotifyIconEx.m_messageSink.Handle;
				handle.uID = this.m_id;
				handle.uFlags = NotifyIconEx.NotifyFlags.Info;
				handle.uTimeoutOrVersion =(uint) timeoutInMilliSeconds;
				handle.szInfoTitle = title;
				handle.szInfo = text;
				handle.dwInfoFlags = type;
				NotifyIconEx.Shell_NotifyIcon(NotifyIconEx.NotifyCommand.Modify, ref handle);
				return;
			}
			else
			{
				throw new ArgumentException("The parameter must be positive", "timeoutInMilliseconds");
			}
		}

		[DllImport("User32.Dll", CharSet=CharSet.None)]
		private static extern int TrackPopupMenuEx(IntPtr hMenu, uint uFlags, int x, int y, IntPtr hWnd, IntPtr ignore);

		private void Update()
		{
			NotifyIconEx.NotifyIconData handle = new NotifyIconEx.NotifyIconData();
			handle.cbSize =(uint) Marshal.SizeOf(handle);
			handle.hWnd = NotifyIconEx.m_messageSink.Handle;
			handle.uID = this.m_id;
			handle.hIcon = this.m_icon.Handle;
			handle.uFlags = handle.uFlags | NotifyIconEx.NotifyFlags.Icon;
			handle.szTip = this.m_text;
			handle.uFlags = handle.uFlags | NotifyIconEx.NotifyFlags.Tip;
			handle.uFlags = handle.uFlags | NotifyIconEx.NotifyFlags.State;
			if (!this.m_visible)
			{
				handle.dwState = NotifyIconEx.NotifyState.Hidden;
			}
			handle.dwStateMask = handle.dwStateMask | NotifyIconEx.NotifyState.Hidden;
			NotifyIconEx.Shell_NotifyIcon(NotifyIconEx.NotifyCommand.Modify, ref handle);
		}

		public event EventHandler BalloonClick;

		public event EventHandler Click;

		public event EventHandler DoubleClick;

		private enum NotifyCommand
		{
			Add = 0,
			Modify = 1,
			Delete = 2
		}

		private enum NotifyFlags
		{
			Message = 1,
			Icon = 2,
			Tip = 4,
			State = 8,
			Info = 16
		}

		private struct NotifyIconData
		{
			public uint cbSize;

			public IntPtr hWnd;

			public uint uID;

			public NotifyIconEx.NotifyFlags uFlags;

			public uint uCallbackMessage;

			public IntPtr hIcon;

			public string szTip;

			public NotifyIconEx.NotifyState dwState;

			public NotifyIconEx.NotifyState dwStateMask;

			public string szInfo;

			public uint uTimeoutOrVersion;

			public string szInfoTitle;

			public NotifyIconEx.NotifyInfoFlags dwInfoFlags;
		}

		private class NotifyIconTarget : Form
		{
			public NotifyIconTarget()
			{
				this.Text = "Hidden NotifyIconTarget Window";
			}

			protected override void DefWndProc(ref Message msg)
			{
				if (msg.Msg != 0x400)
				{
					if (msg.Msg != 0xc086)
					{
						base.DefWndProc(ref msg);
					}
					else
					{
						if (this.TaskbarCreated != null)
						{
							this.TaskbarCreated(this, EventArgs.Empty);
							return;
						}
					}
				}
				else
				{
					uint lParam = (uint)msg.LParam;
					uint wParam = (uint)msg.WParam;
					uint num = lParam;
					if (num == 0x200 || num == 0x201 || num == 0x204)
					{
						return;
					}
					else if (num == 0x202)
					{
						if (this.ClickNotify == null)
						{
							return;
						}
						this.ClickNotify(this, wParam);
						return;
					}
					else if (num == 0x203)
					{
						if (this.DoubleClickNotify == null)
						{
							return;
						}
						this.DoubleClickNotify(this, wParam);
						return;
					}
					else if (num == 0x205)
					{
						if (this.RightClickNotify == null)
						{
							return;
						}
						this.RightClickNotify(this, wParam);
						return;
					}
					if (num == 0x402 || num == 0x403 || num == 0x404)
					{
						return;
					}
					else if (num == 0x405)
					{
						if (this.ClickBalloonNotify == null)
						{
							return;
						}
						this.ClickBalloonNotify(this, wParam);
						return;
					}
					return;
				}
			}

			public event NotifyIconEx.NotifyIconTarget.NotifyIconHandler ClickBalloonNotify;

			public event NotifyIconEx.NotifyIconTarget.NotifyIconHandler ClickNotify;

			public event NotifyIconEx.NotifyIconTarget.NotifyIconHandler DoubleClickNotify;

			public event NotifyIconEx.NotifyIconTarget.NotifyIconHandler RightClickNotify;

			public event EventHandler TaskbarCreated;

			public delegate void NotifyIconHandler(object sender, uint id);
		}

		public enum NotifyInfoFlags
		{
			None = 0,
			Info = 1,
			Warning = 2,
			Error = 3
		}

		private enum NotifyState
		{
			Hidden = 1
		}

		private struct POINT
		{
			public int x;

			public int y;
		}
	}
}