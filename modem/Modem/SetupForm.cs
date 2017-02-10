using My.CommBase;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Modem
{
	public class SetupForm : Form
	{
		private GroupBox groupBox1;

		private RadioButton radioButton1;

		private RadioButton radioButton2;

		private Label label1;

		private ComboBox comboBox1;

		private Button button1;

		private Button button2;

		private bool _isMoving;

		private int XOffset;

		private int YOffset;

		private Class1 c2;

		private Timer timer1;

		private IContainer components;

		public static SetupForm sfm;

		public SetupForm()
		{
			this.c2 = new Class1();
			this.InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (this.comboBox1.Items.Count != 0)
			{
				BackgroundWorker backgroundWorker = new BackgroundWorker();
				backgroundWorker.WorkerReportsProgress = true;
				backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(this.BW_ProgressChanged);
				backgroundWorker.DoWork += new DoWorkEventHandler(this.BW_DoWork);
				backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BW_RunWorkerCompleted);
				backgroundWorker.RunWorkerAsync(this.radioButton1.Checked);
				base.Close();
				return;
			}
			else
			{
				MessageBox.Show("无可用串口!!!");
				return;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void BW_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			bool argument = (bool)e.Argument;
			string str = string.Concat(Directory.GetCurrentDirectory(), "\\MDMSaro.inf");
			InstallMainClass installMainClass = new InstallMainClass();
			backgroundWorker.ReportProgress(1, string.Concat(">>INF PATH ", str, "\r\n"));
			InstallMainClass.bw = backgroundWorker;
			if (!argument)
			{
				if (installMainClass.install(str, "SaroCDMA", 0, SetupForm.sfm.comboBox1.SelectedItem.ToString(), "", "115200"))
				{
					backgroundWorker.ReportProgress(2, ">>安装成功！！！\r\n");
					backgroundWorker.ReportProgress(1, ">>正在创建新连接！！！\r\n");
					PigTao pigTao = new PigTao();
					int num = pigTao.CreatConnect("SaroCDMA", "#777", "桑荣 6200 CDMA modem", "CARD", "CARD");
					if (num == 0 || num == 183)
					{
						backgroundWorker.ReportProgress(2, ">>创建新连接成功，拨号名SaroCDMA！！！\r\n");
						backgroundWorker.ReportProgress(1, ">>正在设置SaroCDMA为默认连接\r\n");
						SetDefaultConnection setDefaultConnection = new SetDefaultConnection();
						setDefaultConnection.Set("SaroCDMA");
						backgroundWorker.ReportProgress(2, ">>设置成功！\r\n");
						PBK pBK = new PBK();
						pBK.DO("[SaroCDMA]");
						backgroundWorker.ReportProgress(2, ">>所有任务安装设置成功！！！");
						return;
					}
					else
					{
						backgroundWorker.ReportProgress(3, string.Concat(">>创建新连接失败,出错信息:", num.ToString()));
						backgroundWorker.ReportProgress(3, ">>安装中止!!!");
						return;
					}
				}
				else
				{
					backgroundWorker.ReportProgress(3, ">>安装失败！！！");
					return;
				}
			}
			else
			{
				if (installMainClass.install(str, "SaroGPRS", 0, SetupForm.sfm.comboBox1.SelectedItem.ToString(), "at+cgdcont=1,\"ip\",\"cmnet\"", "57600"))
				{
					backgroundWorker.ReportProgress(2, ">>安装成功！！！\r\n");
					backgroundWorker.ReportProgress(1, ">>正在创建新连接！！！\r\n");
					PigTao pigTao1 = new PigTao();
					int num1 = pigTao1.CreatConnect("SaroGPRS", "*99***1#", "桑荣 3100 GPRS modem", "", "");
					if (num1 == 0 || num1 == 183)
					{
						backgroundWorker.ReportProgress(2, ">>创建新连接成功，拨号名SaroGPRS！！！\r\n");
						backgroundWorker.ReportProgress(1, ">>正在设置SaroGPRS为默认连接\r\n");
						SetDefaultConnection setDefaultConnection1 = new SetDefaultConnection();
						setDefaultConnection1.Set("SaroGPRS");
						backgroundWorker.ReportProgress(2, ">>设置成功！\r\n");
						PBK pBK1 = new PBK();
						pBK1.DO("[SaroGPRS]");
						backgroundWorker.ReportProgress(2, ">>所有任务安装设置成功！！！");
						return;
					}
					else
					{
						backgroundWorker.ReportProgress(3, string.Concat(">>创建新连接失败,出错信息:", num1.ToString()));
						backgroundWorker.ReportProgress(3, ">>安装中止!!!");
						return;
					}
				}
				else
				{
					backgroundWorker.ReportProgress(3, ">>安装失败！！！");
					return;
				}
			}
		}

		private void BW_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			try
			{
				string userState = (string)e.UserState;
				int progressPercentage = e.ProgressPercentage;
				if (progressPercentage != 1)
				{
					if (progressPercentage != 2)
					{
						if (progressPercentage != 3)
						{
							if (progressPercentage == 4)
							{
								Form1.richtextbox.SelectionColor = Color.Orange;
								Form1.richtextbox.AppendText(" ");
								Form1.richtextbox.SelectionColor = Color.Orange;
								Form1.richtextbox.AppendText(userState);
							}
						}
						else
						{
							Form1.richtextbox.SelectionColor = Color.Crimson;
							Form1.richtextbox.AppendText(" ");
							Form1.richtextbox.SelectionColor = Color.Crimson;
							Form1.richtextbox.AppendText(userState);
						}
					}
					else
					{
						Form1.richtextbox.SelectionColor = Color.Green;
						Form1.richtextbox.AppendText(" ");
						Form1.richtextbox.SelectionColor = Color.Green;
						Form1.richtextbox.AppendText(userState);
					}
				}
				else
				{
					Form1.richtextbox.SelectionColor = Color.Silver;
					Form1.richtextbox.AppendText(" ");
					Form1.richtextbox.SelectionColor = Color.Silver;
					Form1.richtextbox.AppendText(userState);
				}
			}
			catch
			{
			}
		}

		private void BW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void FillPorts(ComboBox cb)
		{
			cb.Items.Clear();
			for (int i = 0; i < 20; i++)
			{
				string str = string.Concat("COM", i.ToString());
				if (this.c2.IsPortAvailable(str) == CommBase.PortStatus.unavailable || this.c2.IsPortAvailable(str) == CommBase.PortStatus.available)
				{
					cb.Items.Add(str);
				}
			}
			if (cb.Items.Count != 0)
			{
				cb.SelectedIndex = 0;
			}
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Location = new System.Drawing.Point(16, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 92);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Modem Type";
            // 
            // radioButton1
            // 
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(23, 23);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(176, 24);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "GPRS Modem";
            // 
            // radioButton2
            // 
            this.radioButton2.Location = new System.Drawing.Point(23, 63);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(168, 24);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.Text = "CDMA Modem";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(13, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "绑定到串口：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(255)))), ((int)(((byte)(247)))));
            this.comboBox1.Location = new System.Drawing.Point(125, 131);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(32, 172);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 25);
            this.button1.TabIndex = 3;
            this.button1.Text = "确定";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(144, 172);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(85, 25);
            this.button2.TabIndex = 4;
            this.button2.Text = "取消";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // SetupForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(255)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(287, 232);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "SetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SetupForm";
            this.Load += new System.EventHandler(this.SetupForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SetupForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SetupForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SetupForm_MouseUp);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		private void SetupForm_Load(object sender, EventArgs e)
		{
			this.timer1.Start();
		}

		private void SetupForm_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this._isMoving = true;
				this.XOffset = e.X;
				this.YOffset = e.Y;
			}
		}

		private void SetupForm_MouseMove(object sender, MouseEventArgs e)
		{
			if (this._isMoving)
			{
				Point location = base.Location;
				Point point = base.Location;
				base.Location = new Point(location.X + e.X - this.XOffset, point.Y + e.Y - this.YOffset);
			}
		}

		private void SetupForm_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this._isMoving = false;
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			this.timer1.Stop();
			this.FillPorts(this.comboBox1);
		}
	}
}