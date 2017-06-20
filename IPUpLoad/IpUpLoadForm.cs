// André Betz 2004
// http://www.andrebetz.de
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.Win32; // registry

namespace IPUpLoad
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class IPUpLoadForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox FTP_IPAdresse_Feld;
		private System.Windows.Forms.Label FTPIPAdress;
		private System.Windows.Forms.Label Username;
		private System.Windows.Forms.TextBox FTP_Password_Feld;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox Directory;
		private System.Windows.Forms.TextBox DateiName;
		private FTPTransfer m_FTPConn = null;
		private System.Windows.Forms.TextBox FTP_User_Feld;
		private static string m_Advertise = "www.AndreBetz.de";

		public IPUpLoadForm()
		{
			InitializeComponent();
			if(!LoadRegistry())
			{
				this.FTP_IPAdresse_Feld.Text = m_Advertise;
				this.FTP_User_Feld.Text = "";
				this.FTP_Password_Feld.Text = "";
				this.Directory.Text = "";
				this.DateiName.Text = "Weiterleitung.html";
			}
			this.Text = MySockets.LocalIPAdress;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.FTP_IPAdresse_Feld = new System.Windows.Forms.TextBox();
			this.FTPIPAdress = new System.Windows.Forms.Label();
			this.Username = new System.Windows.Forms.Label();
			this.FTP_User_Feld = new System.Windows.Forms.TextBox();
			this.FTP_Password_Feld = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.Directory = new System.Windows.Forms.TextBox();
			this.DateiName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(8, 168);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(192, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Transfer IPAdresse";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// FTP_IPAdresse_Feld
			// 
			this.FTP_IPAdresse_Feld.Location = new System.Drawing.Point(80, 8);
			this.FTP_IPAdresse_Feld.Name = "FTP_IPAdresse_Feld";
			this.FTP_IPAdresse_Feld.Size = new System.Drawing.Size(120, 20);
			this.FTP_IPAdresse_Feld.TabIndex = 1;
			this.FTP_IPAdresse_Feld.Text = "";
			// 
			// FTPIPAdress
			// 
			this.FTPIPAdress.Location = new System.Drawing.Point(0, 8);
			this.FTPIPAdress.Name = "FTPIPAdress";
			this.FTPIPAdress.Size = new System.Drawing.Size(80, 23);
			this.FTPIPAdress.TabIndex = 2;
			this.FTPIPAdress.Text = "FTP-Adresse:";
			// 
			// Username
			// 
			this.Username.Location = new System.Drawing.Point(0, 40);
			this.Username.Name = "Username";
			this.Username.Size = new System.Drawing.Size(64, 23);
			this.Username.TabIndex = 3;
			this.Username.Text = "Username:";
			// 
			// FTP_User_Feld
			// 
			this.FTP_User_Feld.Location = new System.Drawing.Point(80, 40);
			this.FTP_User_Feld.Name = "FTP_User_Feld";
			this.FTP_User_Feld.Size = new System.Drawing.Size(120, 20);
			this.FTP_User_Feld.TabIndex = 4;
			this.FTP_User_Feld.Text = "";
			// 
			// FTP_Password_Feld
			// 
			this.FTP_Password_Feld.Location = new System.Drawing.Point(80, 72);
			this.FTP_Password_Feld.Name = "FTP_Password_Feld";
			this.FTP_Password_Feld.PasswordChar = '*';
			this.FTP_Password_Feld.Size = new System.Drawing.Size(120, 20);
			this.FTP_Password_Feld.TabIndex = 5;
			this.FTP_Password_Feld.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 72);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 23);
			this.label1.TabIndex = 6;
			this.label1.Text = "Passwort";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 104);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 23);
			this.label2.TabIndex = 7;
			this.label2.Text = "Directory";
			// 
			// Directory
			// 
			this.Directory.Location = new System.Drawing.Point(80, 104);
			this.Directory.Name = "Directory";
			this.Directory.Size = new System.Drawing.Size(120, 20);
			this.Directory.TabIndex = 8;
			this.Directory.Text = "";
			// 
			// DateiName
			// 
			this.DateiName.Location = new System.Drawing.Point(80, 136);
			this.DateiName.Name = "DateiName";
			this.DateiName.Size = new System.Drawing.Size(120, 20);
			this.DateiName.TabIndex = 9;
			this.DateiName.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(0, 136);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 23);
			this.label3.TabIndex = 10;
			this.label3.Text = "DateiName";
			// 
			// IPUpLoadForm
			// 
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(216, 197);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.DateiName);
			this.Controls.Add(this.Directory);
			this.Controls.Add(this.FTP_Password_Feld);
			this.Controls.Add(this.FTP_User_Feld);
			this.Controls.Add(this.FTP_IPAdresse_Feld);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.Username);
			this.Controls.Add(this.FTPIPAdress);
			this.Controls.Add(this.button1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "IPUpLoadForm";
			this.Text = "IPUpLoader";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new IPUpLoadForm());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			bool res;
			SaveRegistry();
			m_FTPConn = new FTPTransfer(this.FTP_IPAdresse_Feld.Text);
			if(m_FTPConn.Connect())
			{
				res = m_FTPConn.EnterUser(this.FTP_User_Feld.Text,this.FTP_Password_Feld.Text);

				if(this.Directory.Text!=null && this.Directory.Text.Length > 0)
				{
					res = m_FTPConn.Chdir(this.Directory.Text);
				}

				res = m_FTPConn.Delete(this.DateiName.Text);

				string HTMLFile = CreateHTMLFile(this.Text);

				res = m_FTPConn.Put(this.DateiName.Text,m_FTPConn.String2Byte(HTMLFile));

				m_FTPConn.Quit();
			}
		}

		private string CreateHTMLFile(string ThisIpAdress)
		{
			string HtmlFile = "";
			HtmlFile = "<html><head><title>IPUpLoad by "+m_Advertise+"</title></head>";
			HtmlFile += "<a href=\"";
			HtmlFile += ThisIpAdress;
			HtmlFile += "\">UpLink</a>";
			HtmlFile += "</body></html>";
			return HtmlFile;
		}

		private bool LoadRegistry()
		{
			try
			{
				RegistryKey OurKey = Registry.Users;
				OurKey = OurKey.OpenSubKey(".DEFAULT",true);
				OurKey = OurKey.OpenSubKey("andrebetz.de",true);
				OurKey = OurKey.OpenSubKey("IpUpLoad",true);

				long lCount = OurKey.ValueCount;
				if(lCount <= 0)
				{
					return false;
				}
				this.FTP_IPAdresse_Feld.Text = (string)OurKey.GetValue("IPAdresse");
				this.FTP_User_Feld.Text      = (string)OurKey.GetValue("Username");
				this.FTP_Password_Feld.Text  = (string)OurKey.GetValue("Password");
				this.Directory.Text          = (string)OurKey.GetValue("Directory");
				this.DateiName.Text          = (string)OurKey.GetValue("DateiName");
			}
			catch(Exception e)
			{
				return false;
			}
			return true;
		}

		private bool SaveRegistry()
		{
			try
			{
				RegistryKey OurKey = Registry.Users;
				OurKey = OurKey.OpenSubKey(".DEFAULT",true);
				RegistryKey SubKey = OurKey.OpenSubKey("andrebetz.de",true);
				if(SubKey==null)
				{
					OurKey = OurKey.CreateSubKey("andrebetz.de");
				}
				else
				{
					OurKey = SubKey;
				}
				SubKey = OurKey.OpenSubKey("IpUpLoad",true);
				if(SubKey==null)
				{
					OurKey = OurKey.CreateSubKey("IpUpLoad");
				}
				else
				{
					OurKey = SubKey;
				}

				OurKey.SetValue("IPAdresse",this.FTP_IPAdresse_Feld.Text);
				OurKey.SetValue("Username",this.FTP_User_Feld.Text);
				OurKey.SetValue("Password",this.FTP_Password_Feld.Text);
				OurKey.SetValue("Directory",this.Directory.Text);
				OurKey.SetValue("DateiName",this.DateiName.Text);
			}
			catch(Exception e)
			{
				return false;
			}
			return true;
		}
	}
}
