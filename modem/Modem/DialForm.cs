using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace Modem
{
	public class DialForm : Form
	{
		private ComboBox comboBox1;

		private Button button1;

		private Container components;

		public static string cname;

		private Button button2;

		public DialForm()
		{
			this.InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (this.comboBox1.Text != null)
			{
				DialForm.cname = this.comboBox1.Text;
				BackgroundWorker backgroundWorker = new BackgroundWorker();
				backgroundWorker.WorkerReportsProgress = true;
				backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(this.BW_ProgressChanged);
				backgroundWorker.DoWork += new DoWorkEventHandler(this.BW_DoWork);
				backgroundWorker.RunWorkerAsync();
				base.Close();
				return;
			}
			else
			{
				MessageBox.Show("请选译连接");
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
			DialClass dialClass = new DialClass();
			backgroundWorker.ReportProgress(1, ">>正在拨号.......\r\n");
			backgroundWorker.ReportProgress(1, string.Concat(">>正在连接到 ", DialForm.cname, " .......\r\n"));
			string str = dialClass.Dial(DialForm.cname);
			if (str != "TRUE")
			{
				Form1.CurrentConnectionName = "";
				backgroundWorker.ReportProgress(3, string.Concat(">>拨号失败,错误:", str, "\r\n"));
				backgroundWorker.ReportProgress(5, "状态:断线");
				return;
			}
			else
			{
				Form1.CurrentConnectionName = DialForm.cname;
				backgroundWorker.ReportProgress(2, ">>拨号成功,已连接\r\n");
				backgroundWorker.ReportProgress(5, "状态:在线");
				return;
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
							if (progressPercentage != 4)
							{
								if (progressPercentage == 5)
								{
									Form1.label_stat.Text = userState;
								}
							}
							else
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

		private void DialForm_Load(object sender, EventArgs e)
		{
			PigTao pigTao = new PigTao();
			string[] strArrays = pigTao.EnumConnect();
			for (int i = 0; i < strArrays.GetLength(0) - 1; i++)
			{
				this.comboBox1.Items.Add(strArrays[i]);
			}
			if (this.comboBox1.Items.Count == 0)
			{
				MessageBox.Show("没有任何连接");
				base.Close();
				return;
			}
			else
			{
				this.comboBox1.SelectedIndex = 0;
				return;
			}
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(255)))), ((int)(((byte)(247)))));
            this.comboBox1.Location = new System.Drawing.Point(41, 50);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(255)))), ((int)(((byte)(247)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(41, 139);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(49, 26);
            this.button1.TabIndex = 1;
            this.button1.Text = "拨号";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(255)))), ((int)(((byte)(247)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(123, 139);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 26);
            this.button2.TabIndex = 2;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // DialForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackgroundImage = global::Modem.Properties.Resources.BackgroundImage;
            this.ClientSize = new System.Drawing.Size(330, 294);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Name = "DialForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "拨号";
            this.Load += new System.EventHandler(this.DialForm_Load);
            this.ResumeLayout(false);

		}

		private delegate void DelegateDial();
	}
}