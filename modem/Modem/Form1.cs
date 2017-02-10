using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace Modem
{
	public class Form1 : Form
	{
		public const long SKIN_CLASS_NOSKIN = 0L;

		private NotifyIconEx NotifyIcon;

		public static string PSWD;

		public static bool PWDFlag;

		private IContainer components;

		private int XOffset;

		private int YOffset;

		private RichTextBox richTextBox1;

		private PictureBox pictureBox1;

		private PictureBox pictureBox2;

		private bool _isMoving;

		private PictureBox pictureBox_setup;

		private PictureBox pictureBox_dial;

		private PictureBox pictureBox_hup;

		private PictureBox pictureBox_set;

		private PictureBox pictureBox_about;

		private PictureBox pictureBox_help;

		private PictureBox pictureBox_exit;

		private ToolTip toolTip1;

		private ContextMenu contextMenu1;

		private MenuItem menuItem1;

		private MenuItem menuItem2;

		private MenuItem menuItem3;

		private MenuItem menuItem4;

		private MenuItem menuItem5;

		private MenuItem menuItem6;

		private MenuItem menuItem7;

		private ResourceManager rm;

		private Label label1;

		private Label label2;

		private Label label3;

		public static RichTextBox richtextbox;

		public static Label label_currentconnection;

		public static Label label_flux;

		public static Label label_stat;

		public static Label label_Count;

		public static int count;

		public static DialManager DM;

		public static Thread thread1;

		private Label label_count;

		private Label label_time;

		private DateTime begin;

		private System.Windows.Forms.Timer timer1;

		public static string CurrentConnectionName;

		private Monitor[] m_PacketMonitors;

		private long _totle;

		private Random rand;

		private BackgroundWorker BW_for_flux;

		static Form1()
		{
			Form1.PSWD = "";
			Form1.PWDFlag = false;
			Form1.DM = new DialManager();
			Form1.CurrentConnectionName = "";
		}

		public Form1()
		{
            this.rm = new ResourceManager("Modem.Properties.Resources", Assembly.GetExecutingAssembly());//获取程序资源
			this.InitializeComponent();
			this.Initialize();
		}

		private void button1_Click(object sender, EventArgs e)
		{
		}

		private void BW_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			DialManager.bw = backgroundWorker;
			DialManager dialManager = new DialManager();
			dialManager.Start();
		}

		private void BW_for_flux_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.label2.Text = string.Concat("流量:", this._totle.ToString(), "byte");
		}

		private string CountTime(DateTime b, DateTime now)
		{
			TimeSpan timeSpan = now - b;
			object[] days = new object[4];
			days[0] = timeSpan.Days;
			days[1] = timeSpan.Hours;
			days[2] = timeSpan.Minutes;
			days[3] = timeSpan.Seconds;
			return string.Format("{0}天{1}时{2}分{3}秒", days);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void Form1_Closing(object sender, CancelEventArgs e)
		{
			this.NotifyIcon.Visible = false;
			log _log = new log();
			_log.write("---------------------程序关闭----------------------");
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			log _log = new log();
			_log.write("---------------------程序启动----------------------");
			this.NotifyIcon = new NotifyIconEx();
			this.NotifyIcon.Text = "无线精灵2016";
			this.NotifyIcon.Icon = base.Icon;
			this.NotifyIcon.Visible = false;
			this.NotifyIcon.ContextMenu = this.contextMenu1;
			this.NotifyIcon.Click += new EventHandler(this.OnClickIcon);
			this.NotifyIcon.DoubleClick += new EventHandler(this.notifyIcon1_DoubleClick);
			this.NotifyIcon.BalloonClick += new EventHandler(this.OnClickBalloon);
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += new DoWorkEventHandler(this.BW_DoWork);
			backgroundWorker.RunWorkerAsync();
		}

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this._isMoving = true;
				this.XOffset = e.X;
				this.YOffset = e.Y;
			}
		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			if (this._isMoving)
			{
				Point location = base.Location;
				Point point = base.Location;
				base.Location = new Point(location.X + e.X - this.XOffset, point.Y + e.Y - this.YOffset);
			}
		}

		private void Form1_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this._isMoving = false;
			}
		}

		private void Form1_NewPacket(Monitor m, Packet p)
		{
			Form1 totalLength = this;
			totalLength._totle = totalLength._totle + (long)p.TotalLength;
			if (this.rand.Next(0, 5) == 1 && !this.BW_for_flux.IsBusy)
			{
				this.BW_for_flux.RunWorkerAsync();
			}
		}

		private void Form1_Resize(object sender, EventArgs e)
		{
			if (base.WindowState == FormWindowState.Minimized)
			{
				base.Hide();
				this.NotifyIcon.Visible = true;
				this.NotifyIcon.ShowBalloon("Saro", "无线精灵正在运行", NotifyIconEx.NotifyInfoFlags.Info, 10);
			}
		}

		public static void hungup()
		{
			DialClass dialClass = new DialClass();
			log _log = new log();
			_log.write("正在挂断......");
			Form1.richtextbox.AppendText(">>正在挂断......\r\n");
			dialClass.Hangup();
			_log.write("挂断成功......");
			Form1.richtextbox.AppendText(">>挂断成功......\r\n");
			Form1.label_stat.Text = "状态:断线";
		}

		private void Initialize()
		{
			Form1.richtextbox = this.richTextBox1;
			Form1.label_currentconnection = this.label1;
			Form1.label_flux = this.label2;
			Form1.label_stat = this.label3;
			Form1.label_Count = this.label_count;
			Form1.count = 0;
			this.begin = DateTime.Now;
			this.timer1.Start();
			int millisecond = this.begin.Millisecond;
			this.rand = new Random(millisecond);
			this.label2.Tag = 1;
			this._totle = (long)0;
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox_setup = new System.Windows.Forms.PictureBox();
            this.pictureBox_dial = new System.Windows.Forms.PictureBox();
            this.pictureBox_hup = new System.Windows.Forms.PictureBox();
            this.pictureBox_set = new System.Windows.Forms.PictureBox();
            this.pictureBox_about = new System.Windows.Forms.PictureBox();
            this.pictureBox_help = new System.Windows.Forms.PictureBox();
            this.pictureBox_exit = new System.Windows.Forms.PictureBox();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_count = new System.Windows.Forms.Label();
            this.label_time = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_setup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_dial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_hup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_set)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_about)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_help)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_exit)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(225)))), ((int)(((byte)(248)))));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.ForeColor = System.Drawing.Color.Silver;
            this.richTextBox1.Location = new System.Drawing.Point(281, 229);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(677, 394);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(830, 34);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox1, "关闭");
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(770, 34);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox2, "最小化");
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseDown);
            this.pictureBox2.MouseEnter += new System.EventHandler(this.pictureBox2_MouseEnter);
            this.pictureBox2.MouseLeave += new System.EventHandler(this.pictureBox2_MouseLeave);
            // 
            // pictureBox_setup
            // 
            this.pictureBox_setup.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_setup.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_setup.Image")));
            this.pictureBox_setup.Location = new System.Drawing.Point(77, 113);
            this.pictureBox_setup.Name = "pictureBox_setup";
            this.pictureBox_setup.Size = new System.Drawing.Size(110, 38);
            this.pictureBox_setup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_setup.TabIndex = 3;
            this.pictureBox_setup.TabStop = false;
            this.pictureBox_setup.Click += new System.EventHandler(this.pictureBox_setup_Click);
            this.pictureBox_setup.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_setup_MouseDown);
            this.pictureBox_setup.MouseEnter += new System.EventHandler(this.pictureBox_setup_MouseEnter);
            this.pictureBox_setup.MouseLeave += new System.EventHandler(this.pictureBox_setup_MouseLeave);
            this.pictureBox_setup.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_setup_MouseUp);
            // 
            // pictureBox_dial
            // 
            this.pictureBox_dial.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_dial.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_dial.Image")));
            this.pictureBox_dial.Location = new System.Drawing.Point(77, 157);
            this.pictureBox_dial.Name = "pictureBox_dial";
            this.pictureBox_dial.Size = new System.Drawing.Size(110, 38);
            this.pictureBox_dial.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_dial.TabIndex = 4;
            this.pictureBox_dial.TabStop = false;
            this.pictureBox_dial.Click += new System.EventHandler(this.pictureBox_dial_Click);
            this.pictureBox_dial.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_dial_MouseDown);
            this.pictureBox_dial.MouseEnter += new System.EventHandler(this.pictureBox_dial_MouseEnter);
            this.pictureBox_dial.MouseLeave += new System.EventHandler(this.pictureBox_dial_MouseLeave);
            this.pictureBox_dial.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_dial_MouseUp);
            // 
            // pictureBox_hup
            // 
            this.pictureBox_hup.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_hup.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_hup.Image")));
            this.pictureBox_hup.Location = new System.Drawing.Point(77, 201);
            this.pictureBox_hup.Name = "pictureBox_hup";
            this.pictureBox_hup.Size = new System.Drawing.Size(110, 38);
            this.pictureBox_hup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_hup.TabIndex = 5;
            this.pictureBox_hup.TabStop = false;
            this.pictureBox_hup.Click += new System.EventHandler(this.pictureBox_hup_Click);
            this.pictureBox_hup.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_hup_MouseDown);
            this.pictureBox_hup.MouseEnter += new System.EventHandler(this.pictureBox_hup_MouseEnter);
            this.pictureBox_hup.MouseLeave += new System.EventHandler(this.pictureBox_hup_MouseLeave);
            this.pictureBox_hup.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_hup_MouseUp);
            // 
            // pictureBox_set
            // 
            this.pictureBox_set.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_set.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_set.Image")));
            this.pictureBox_set.Location = new System.Drawing.Point(77, 245);
            this.pictureBox_set.Name = "pictureBox_set";
            this.pictureBox_set.Size = new System.Drawing.Size(110, 38);
            this.pictureBox_set.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_set.TabIndex = 6;
            this.pictureBox_set.TabStop = false;
            this.pictureBox_set.Click += new System.EventHandler(this.pictureBox_set_Click);
            this.pictureBox_set.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_set_MouseDown);
            this.pictureBox_set.MouseEnter += new System.EventHandler(this.pictureBox_set_MouseEnter);
            this.pictureBox_set.MouseLeave += new System.EventHandler(this.pictureBox_set_MouseLeave);
            this.pictureBox_set.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_set_MouseUp);
            // 
            // pictureBox_about
            // 
            this.pictureBox_about.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_about.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_about.Image")));
            this.pictureBox_about.Location = new System.Drawing.Point(77, 357);
            this.pictureBox_about.Name = "pictureBox_about";
            this.pictureBox_about.Size = new System.Drawing.Size(110, 38);
            this.pictureBox_about.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_about.TabIndex = 7;
            this.pictureBox_about.TabStop = false;
            this.pictureBox_about.Click += new System.EventHandler(this.pictureBox_about_Click);
            this.pictureBox_about.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_about_MouseDown);
            this.pictureBox_about.MouseEnter += new System.EventHandler(this.pictureBox_about_MouseEnter);
            this.pictureBox_about.MouseLeave += new System.EventHandler(this.pictureBox_about_MouseLeave);
            this.pictureBox_about.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_about_MouseUp);
            // 
            // pictureBox_help
            // 
            this.pictureBox_help.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_help.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_help.Image")));
            this.pictureBox_help.Location = new System.Drawing.Point(77, 303);
            this.pictureBox_help.Name = "pictureBox_help";
            this.pictureBox_help.Size = new System.Drawing.Size(110, 38);
            this.pictureBox_help.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_help.TabIndex = 8;
            this.pictureBox_help.TabStop = false;
            this.pictureBox_help.Click += new System.EventHandler(this.pictureBox_help_Click);
            this.pictureBox_help.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_help_MouseDown);
            this.pictureBox_help.MouseEnter += new System.EventHandler(this.pictureBox_help_MouseEnter);
            this.pictureBox_help.MouseLeave += new System.EventHandler(this.pictureBox_help_MouseLeave);
            this.pictureBox_help.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_help_MouseUp);
            // 
            // pictureBox_exit
            // 
            this.pictureBox_exit.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_exit.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_exit.Image")));
            this.pictureBox_exit.Location = new System.Drawing.Point(77, 413);
            this.pictureBox_exit.Name = "pictureBox_exit";
            this.pictureBox_exit.Size = new System.Drawing.Size(110, 38);
            this.pictureBox_exit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_exit.TabIndex = 9;
            this.pictureBox_exit.TabStop = false;
            this.pictureBox_exit.Click += new System.EventHandler(this.pictureBox_exit_Click);
            this.pictureBox_exit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_exit_MouseDown);
            this.pictureBox_exit.MouseEnter += new System.EventHandler(this.pictureBox_exit_MouseEnter);
            this.pictureBox_exit.MouseLeave += new System.EventHandler(this.pictureBox_exit_MouseLeave);
            this.pictureBox_exit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_exit_MouseUp);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem5,
            this.menuItem4,
            this.menuItem2,
            this.menuItem1,
            this.menuItem6,
            this.menuItem7,
            this.menuItem3});
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 0;
            this.menuItem5.Text = "显示主窗口";
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.Text = "参数设置";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 2;
            this.menuItem2.Text = "帮助";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text = "关于";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 4;
            this.menuItem6.Text = "拨号";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 5;
            this.menuItem7.Text = "挂断";
            this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 6;
            this.menuItem3.Text = "退出";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.ForeColor = System.Drawing.Color.ForestGreen;
            this.toolTip1.InitialDelay = 300;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(459, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 15);
            this.label2.TabIndex = 11;
            this.label2.Tag = "";
            this.label2.Text = "流量:未开始统计";
            this.toolTip1.SetToolTip(this.label2, "流量统计,双击开始统计(不建议打开此功能)");
            this.label2.Click += new System.EventHandler(this.label2_Click);
            this.label2.DoubleClick += new System.EventHandler(this.label2_DoubleClick);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(302, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "当前连接:空";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(657, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 14);
            this.label3.TabIndex = 12;
            this.label3.Text = "状态:断线";
            // 
            // label_count
            // 
            this.label_count.BackColor = System.Drawing.Color.Transparent;
            this.label_count.Location = new System.Drawing.Point(302, 165);
            this.label_count.Name = "label_count";
            this.label_count.Size = new System.Drawing.Size(139, 17);
            this.label_count.TabIndex = 13;
            this.label_count.Text = "重拨次数:0";
            // 
            // label_time
            // 
            this.label_time.BackColor = System.Drawing.Color.Transparent;
            this.label_time.Location = new System.Drawing.Point(468, 125);
            this.label_time.Name = "label_time";
            this.label_time.Size = new System.Drawing.Size(152, 18);
            this.label_time.TabIndex = 14;
            this.label_time.Text = "已运行:0";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(984, 656);
            this.Controls.Add(this.label_time);
            this.Controls.Add(this.label_count);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox_exit);
            this.Controls.Add(this.pictureBox_help);
            this.Controls.Add(this.pictureBox_about);
            this.Controls.Add(this.pictureBox_set);
            this.Controls.Add(this.pictureBox_hup);
            this.Controls.Add(this.pictureBox_dial);
            this.Controls.Add(this.pictureBox_setup);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.richTextBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "无线精灵2016";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_setup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_dial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_hup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_set)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_about)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_help)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_exit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private void label2_Click(object sender, EventArgs e)
		{
		}

		private void label2_DoubleClick(object sender, EventArgs e)
		{
			if ((int)this.label2.Tag != 1)
			{
				if ((int)this.label2.Tag == 2)
				{
					this.StopFlux();
					this.label2.Tag = 1;
					this.toolTip1.SetToolTip(this.label2, "流量统计,双击开始统计(不建议打开此功能)");
					this.label2.Text = "流量:未开始统计";
				}
				return;
			}
			else
			{
				this.StartFlux();
				this.label2.Tag = 2;
				this.toolTip1.SetToolTip(this.label2, "流量统计,双击关闭统计(建议关闭此功能)");
				this.label2.Text = "流量:未开始统计";
				return;
			}
		}

		[STAThread]
		private static void Main()
		{
			bool flag = false;
			new Mutex(true, "05888279hjjkks", out flag);
			if (!flag)
			{
				MessageBox.Show("程序已经运行!");
				return;
			}
			else
			{
				Application.Run(new Form1());
				return;
			}
		}

		private void menuItem1_Click(object sender, EventArgs e)
		{
			this.pictureBox_about_Click(sender, e);
		}

		private void menuItem2_Click(object sender, EventArgs e)
		{
			this.pictureBox_help_Click(sender, e);
		}

		private void menuItem3_Click(object sender, EventArgs e)
		{
			this.pictureBox1_Click(sender, e);
		}

		private void menuItem4_Click(object sender, EventArgs e)
		{
			this.pictureBox_set_Click(sender, e);
		}

		private void menuItem5_Click(object sender, EventArgs e)
		{
			base.Show();
			base.WindowState = FormWindowState.Normal;
			this.NotifyIcon.Visible = false;
		}

		private void menuItem6_Click(object sender, EventArgs e)
		{
			this.pictureBox_dial_Click(sender, e);
		}

		private void menuItem7_Click(object sender, EventArgs e)
		{
			this.pictureBox_hup_Click(sender, e);
		}

		private void notifyIcon1_DoubleClick(object sender, EventArgs e)
		{
			base.Show();
			base.WindowState = FormWindowState.Normal;
			this.NotifyIcon.Visible = false;
		}

		private void OnClickBalloon(object sender, EventArgs e)
		{
		}

		private void OnClickIcon(object sender, EventArgs e)
		{
		}

		private void pictureBox_about_Click(object sender, EventArgs e)
		{
			about _about = new about();
			_about.ShowDialog(this);
		}

		private void pictureBox_about_MouseDown(object sender, MouseEventArgs e)
		{
			this.pictureBox_about.Image = (Image)this.rm.GetObject("about2");
		}

		private void pictureBox_about_MouseEnter(object sender, EventArgs e)
		{
			this.pictureBox_about.Image = (Image)this.rm.GetObject("about3");
		}

		private void pictureBox_about_MouseLeave(object sender, EventArgs e)
		{
			this.pictureBox_about.Image = (Image)this.rm.GetObject("about1");
		}

		private void pictureBox_about_MouseUp(object sender, MouseEventArgs e)
		{
			this.pictureBox_about.Image = (Image)this.rm.GetObject("about3");
		}

		private void pictureBox_dial_Click(object sender, EventArgs e)
		{
			DialForm dialForm = new DialForm();
			dialForm.ShowDialog(this);
		}

		private void pictureBox_dial_MouseDown(object sender, MouseEventArgs e)
		{
			this.pictureBox_dial.Image = (Image)this.rm.GetObject("dial2");
		}

		private void pictureBox_dial_MouseEnter(object sender, EventArgs e)
		{
			this.pictureBox_dial.Image = (Image)this.rm.GetObject("dial3");
		}

		private void pictureBox_dial_MouseLeave(object sender, EventArgs e)
		{
			this.pictureBox_dial.Image = (Image)this.rm.GetObject("dial1");
		}

		private void pictureBox_dial_MouseUp(object sender, MouseEventArgs e)
		{
			this.pictureBox_dial.Image = (Image)this.rm.GetObject("dial3");
		}

		private void pictureBox_exit_Click(object sender, EventArgs e)
		{
			this.pictureBox1_Click(sender, e);
		}

		private void pictureBox_exit_MouseDown(object sender, MouseEventArgs e)
		{
			this.pictureBox_exit.Image = (Image)this.rm.GetObject("exit2");
		}

		private void pictureBox_exit_MouseEnter(object sender, EventArgs e)
		{
			this.pictureBox_exit.Image = (Image)this.rm.GetObject("exit3");
		}

		private void pictureBox_exit_MouseLeave(object sender, EventArgs e)
		{
			this.pictureBox_exit.Image = (Image)this.rm.GetObject("exit1");
		}

		private void pictureBox_exit_MouseUp(object sender, MouseEventArgs e)
		{
			this.pictureBox_exit.Image = (Image)this.rm.GetObject("exit3");
		}

		private void pictureBox_help_Click(object sender, EventArgs e)
		{
			help _help = new help();
			_help.ShowDialog(this);
		}

		private void pictureBox_help_MouseDown(object sender, MouseEventArgs e)
		{
			this.pictureBox_help.Image = (Image)this.rm.GetObject("help2");
		}

		private void pictureBox_help_MouseEnter(object sender, EventArgs e)
		{
			this.pictureBox_help.Image = (Image)this.rm.GetObject("help3");
		}

		private void pictureBox_help_MouseLeave(object sender, EventArgs e)
		{
			this.pictureBox_help.Image = (Image)this.rm.GetObject("help1");
		}

		private void pictureBox_help_MouseUp(object sender, MouseEventArgs e)
		{
			this.pictureBox_help.Image = (Image)this.rm.GetObject("help3");
		}

		private void pictureBox_hup_Click(object sender, EventArgs e)
		{
			Thread thread = new Thread(new ThreadStart(Form1.hungup));
			thread.Start();
		}

		private void pictureBox_hup_MouseDown(object sender, MouseEventArgs e)
		{
			this.pictureBox_hup.Image = (Image)this.rm.GetObject("hungup2");
		}

		private void pictureBox_hup_MouseEnter(object sender, EventArgs e)
		{
			this.pictureBox_hup.Image = (Image)this.rm.GetObject("hungup3");
		}

		private void pictureBox_hup_MouseLeave(object sender, EventArgs e)
		{
			this.pictureBox_hup.Image = (Image)this.rm.GetObject("hungup1");
		}

		private void pictureBox_hup_MouseUp(object sender, MouseEventArgs e)
		{
			this.pictureBox_hup.Image = (Image)this.rm.GetObject("hungup3");
		}

		private void pictureBox_set_Click(object sender, EventArgs e)
		{
			ModemSettings setting = new ModemSettings();
			setting.ShowDialog(this);
		}

		private void pictureBox_set_MouseDown(object sender, MouseEventArgs e)
		{
			this.pictureBox_set.Image = (Image)this.rm.GetObject("settings3");
		}

		private void pictureBox_set_MouseEnter(object sender, EventArgs e)
		{
			this.pictureBox_set.Image = (Image)this.rm.GetObject("settings2");
		}

		private void pictureBox_set_MouseLeave(object sender, EventArgs e)
		{
			this.pictureBox_set.Image = (Image)this.rm.GetObject("settings1");
		}

		private void pictureBox_set_MouseUp(object sender, MouseEventArgs e)
		{
			this.pictureBox_set.Image = (Image)this.rm.GetObject("settings2");
		}

		private void pictureBox_setup_Click(object sender, EventArgs e)
		{
			SetupForm setupForm = new SetupForm();
			SetupForm.sfm = setupForm;
			setupForm.ShowDialog(this);
		}

		private void pictureBox_setup_MouseDown(object sender, MouseEventArgs e)
		{
			this.pictureBox_setup.Image = (Image)this.rm.GetObject("modem_setup2");
		}

		private void pictureBox_setup_MouseEnter(object sender, EventArgs e)
		{
			this.pictureBox_setup.Image = (Image)this.rm.GetObject("modem_setup3");
		}

		private void pictureBox_setup_MouseLeave(object sender, EventArgs e)
		{
			this.pictureBox_setup.Image = (Image)this.rm.GetObject("modem_setup1");
		}

		private void pictureBox_setup_MouseUp(object sender, MouseEventArgs e)
		{
			this.pictureBox_setup.Image = (Image)this.rm.GetObject("modem_setup3");
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			if (this.PWD())
			{
				base.Close();
				Application.Exit();
			}
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			this.pictureBox1.Image = (Image)this.rm.GetObject("RedDown");
		}

		private void pictureBox1_MouseEnter(object sender, EventArgs e)
		{
			this.pictureBox1.Image = (Image)this.rm.GetObject("RedOver");
		}

		private void pictureBox1_MouseLeave(object sender, EventArgs e)
		{
			this.pictureBox1.Image = (Image)this.rm.GetObject("Red1");
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			base.WindowState = FormWindowState.Minimized;
		}

		private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
		{
			this.pictureBox2.Image = (Image)this.rm.GetObject("GreenDown");
		}

		private void pictureBox2_MouseEnter(object sender, EventArgs e)
		{
			this.pictureBox2.Image = (Image)this.rm.GetObject("GreenOver");
		}

		private void pictureBox2_MouseLeave(object sender, EventArgs e)
		{
			this.pictureBox2.Image = (Image)this.rm.GetObject("Green1");
		}

		private bool PWD()
		{
			XML xML = XML.LoadFromXML();
			if (!xML.runPWD)
			{
				return true;
			}
			else
			{
				PWDForm pWDForm = new PWDForm();
				pWDForm.ShowDialog(this);
				if (!Form1.PWDFlag)
				{
					if (xML.PWD != Form1.PSWD)
					{
						MessageBox.Show("密码错误");
						return false;
					}
					else
					{
						return true;
					}
				}
				else
				{
					Form1.PWDFlag = false;
					return false;
				}
			}
		}

		public static void SetText(int p, string text)
		{
			if (!Form1.richtextbox.InvokeRequired)
			{
				try
				{
					string str = text;
					int num = p;
					if (num != 1)
					{
						if (num != 2)
						{
							if (num != 3)
							{
								if (num != 4)
								{
									if (num != 5)
									{
										if (num != 6)
										{
											if (num == 7)
											{
												Form1.label_currentconnection.Text = str;
											}
										}
										else
										{
											Form1.label_Count.Text = str;
										}
									}
									else
									{
										Form1.label_stat.Text = str;
									}
								}
								else
								{
									Form1.richtextbox.SelectionColor = Color.Orange;
									Form1.richtextbox.AppendText(" ");
									Form1.richtextbox.SelectionColor = Color.Orange;
									Form1.richtextbox.AppendText(str);
								}
							}
							else
							{
								Form1.richtextbox.SelectionColor = Color.Crimson;
								Form1.richtextbox.AppendText(" ");
								Form1.richtextbox.SelectionColor = Color.Crimson;
								Form1.richtextbox.AppendText(str);
							}
						}
						else
						{
							Form1.richtextbox.SelectionColor = Color.Green;
							Form1.richtextbox.AppendText(" ");
							Form1.richtextbox.SelectionColor = Color.Green;
							Form1.richtextbox.AppendText(str);
						}
					}
					else
					{
						Form1.richtextbox.SelectionColor = Color.Silver;
						Form1.richtextbox.AppendText(" ");
						Form1.richtextbox.SelectionColor = Color.Silver;
						Form1.richtextbox.AppendText(str);
					}
				}
				catch
				{
				}
				return;
			}
			else
			{
				object[] objArray = new object[2];
				objArray[0] = p;
				objArray[1] = text;
				Form1.richtextbox.BeginInvoke(new SetTextCallBack(Form1.SetText), objArray);
				return;
			}
		}

		public void StartFlux()
		{
			IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
			if ((int)addressList.Length != 0)
			{
				this.m_PacketMonitors = new Monitor[1];
				this.m_PacketMonitors[0] = new Monitor(addressList[0]);
				this.m_PacketMonitors[0].NewPacket += new Monitor.NewPacketEventHandler(this.Form1_NewPacket);
				this.m_PacketMonitors[0].Start();
				this.BW_for_flux = new BackgroundWorker();
				this.BW_for_flux.WorkerReportsProgress = true;
				this.BW_for_flux.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BW_for_flux_RunWorkerCompleted);
				return;
			}
			else
			{
				throw new NotSupportedException("This computer does not have non-loopback interfaces installed!");
			}
		}

		public void StopFlux()
		{
			this.m_PacketMonitors[0].Stop();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			this.label_time.Text = string.Concat("已运行:", this.CountTime(this.begin, DateTime.Now));
		}
	}
}