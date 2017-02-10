using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Modem
{
	public class ModemSettings : Form
	{
		private CheckBox checkBox1;

		private CheckBox checkBox2;

		private GroupBox groupBox1;

		private Label label1;

		private Label label2;

		private GroupBox groupBox2;

		private Label label4;

		private TextBox textBox3;

		private Label label5;

		private NumericUpDown numericUpDown2;

		private Label label6;

		private NumericUpDown numericUpDown4;

		private Button button1;

		private Button button2;

		private GroupBox groupBox3;

		private bool _isMoving;

		private int XOffset;

		private int YOffset;

		private CheckBox checkBox3;

		private Label label3;

		private TextBox textBox1;

		private ToolTip toolTip1;

		private TextBox textBox_pwd;

		private TextBox textBox_name;

		private IContainer components;

		public ModemSettings()
		{
			this.InitializeComponent();
		}

		private void autorun(bool run)
		{
			string str = string.Concat(Application.StartupPath, "\\Modem.exe");
			RegistryKey localMachine = Registry.LocalMachine;
			RegistryKey registryKey = null;
			if (!run)
			{
				try
				{
					registryKey = localMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
					registryKey.DeleteValue("Power_YDT");
					registryKey.Close();
				}
				catch
				{
				}
				if (registryKey == null)
				{
					try
					{
						RegistryKey registryKey1 = localMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
						registryKey1.DeleteValue("Power_YDT");
						registryKey1.Close();
					}
					catch
					{
					}
				}
			}
			else
			{
				try
				{
					registryKey = localMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    registryKey.SetValue("Power_YDT", str);
					registryKey.Close();
				}
				catch
				{
				}
				if (registryKey == null)
				{
					try
					{
						RegistryKey registryKey2 = localMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                        registryKey2.SetValue("Power_YDT", str);
						registryKey2.Close();
					}
					catch
					{
					}
				}
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			XML mXml = XML.LoadFromXML();
			mXml.interval = Convert.ToInt32(this.numericUpDown4.Value);//通讯时间间隔
			mXml.IP = this.textBox3.Text;                              //通讯IP地址
			mXml.on = this.checkBox2.Checked;  //是否自动重拨
			mXml.run = this.checkBox1.Checked;//开机是否自动启动
			mXml.timeout = Convert.ToInt32(this.numericUpDown2.Value);//超时时间
			mXml.runPWD = this.checkBox3.Checked;//退出时是否输入密码
			mXml.PWD = this.textBox1.Text;       //密码
			mXml.username = this.textBox_name.Text;//拨号用户名，根据移动通讯服务商不同，用户名不同，联通默认为"CARD"
			mXml.password = this.textBox_pwd.Text;//拨号密码
			mXml.SaveAsXML();
			this.autorun(this.checkBox1.Checked);
			base.Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_pwd = new System.Windows.Forms.TextBox();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.BackColor = System.Drawing.Color.Transparent;
            this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBox1.Location = new System.Drawing.Point(10, 15);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(111, 24);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "开机后自动运行";
            this.checkBox1.UseVisualStyleBackColor = false;
            // 
            // checkBox2
            // 
            this.checkBox2.BackColor = System.Drawing.Color.Transparent;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBox2.Location = new System.Drawing.Point(151, 14);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(104, 24);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "启动自动重拨";
            this.checkBox2.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.textBox_pwd);
            this.groupBox1.Controls.Add(this.textBox_name);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(11, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(291, 66);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // textBox_pwd
            // 
            this.textBox_pwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_pwd.Location = new System.Drawing.Point(115, 43);
            this.textBox_pwd.Name = "textBox_pwd";
            this.textBox_pwd.PasswordChar = '*';
            this.textBox_pwd.Size = new System.Drawing.Size(140, 21);
            this.textBox_pwd.TabIndex = 4;
            this.textBox_pwd.Text = "CARD";
            // 
            // textBox_name
            // 
            this.textBox_name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_name.Location = new System.Drawing.Point(115, 16);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(141, 21);
            this.textBox_name.TabIndex = 3;
            this.textBox_name.Text = "CARD";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "拨号密码：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.label2, "拨号失败后，进行下一次拨号的时间。");
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "拨号用户名：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.numericUpDown4);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numericUpDown2);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(11, 142);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(292, 110);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ping";
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDown4.Location = new System.Drawing.Point(124, 80);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(73, 21);
            this.numericUpDown4.TabIndex = 6;
            this.numericUpDown4.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(17, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "间隔时间(秒)";
            this.toolTip1.SetToolTip(this.label6, "两次检查是否仍在线的时间间隔");
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDown2.Location = new System.Drawing.Point(125, 49);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(74, 21);
            this.numericUpDown2.TabIndex = 4;
            this.numericUpDown2.Value = new decimal(new int[] {
            700,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(15, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 18);
            this.label5.TabIndex = 2;
            this.label5.Text = "超时时间(毫秒):";
            this.toolTip1.SetToolTip(this.label5, "建议1200-4000ms之间");
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox3.Location = new System.Drawing.Point(125, 20);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(139, 21);
            this.textBox3.TabIndex = 1;
            this.textBox3.Text = "www.google.com";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "IP或域名";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(255)))), ((int)(((byte)(247)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(42, 256);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 27);
            this.button1.TabIndex = 4;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(255)))), ((int)(((byte)(247)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(165, 255);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(78, 27);
            this.button2.TabIndex = 5;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.checkBox3);
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Controls.Add(this.checkBox2);
            this.groupBox3.Location = new System.Drawing.Point(11, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(290, 65);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(187, 37);
            this.textBox1.Name = "textBox1";
            this.textBox1.PasswordChar = '*';
            this.textBox1.Size = new System.Drawing.Size(86, 21);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "textBox1";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(146, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "密码:";
            // 
            // checkBox3
            // 
            this.checkBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBox3.Location = new System.Drawing.Point(10, 40);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(107, 19);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Text = "退出时输入密码";
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.ForeColor = System.Drawing.Color.ForestGreen;
            this.toolTip1.InitialDelay = 300;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // ModemSettings
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(255)))), ((int)(((byte)(247)))));
            this.BackgroundImage = global::Modem.Properties.Resources.BackgroundImage;
            this.ClientSize = new System.Drawing.Size(327, 295);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ModemSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Settings_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Settings_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Settings_MouseUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

		}
        //从配置文件读取通讯参数
		private void LoadXML()
		{
			if (File.Exists("settings.xml"))
			{
				XML xML = XML.LoadFromXML();
				this.numericUpDown4.Value = xML.interval;
				this.textBox3.Text = xML.IP;
				this.checkBox2.Checked = xML.on;
				this.checkBox1.Checked = xML.run;
				this.numericUpDown2.Value = xML.timeout;
				this.checkBox3.Checked = xML.runPWD;
				this.textBox1.Text = xML.PWD;
				this.textBox_name.Text = xML.username;
				this.textBox_pwd.Text = xML.password;
				return;
			}
			else
			{
				MessageBox.Show("找不到参数设置文件，请确认当前目录存在settings.xml文件");
				return;
			}
		}

		private void Settings_Load(object sender, EventArgs e)
		{
			this.LoadXML();
		}

		private void Settings_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this._isMoving = true;
				this.XOffset = e.X;
				this.YOffset = e.Y;
			}
		}

		private void Settings_MouseMove(object sender, MouseEventArgs e)
		{
			if (this._isMoving)
			{
				Point location = base.Location;
				Point point = base.Location;
				base.Location = new Point(location.X + e.X - this.XOffset, point.Y + e.Y - this.YOffset);
			}
		}

		private void Settings_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this._isMoving = false;
			}
		}
	}
}