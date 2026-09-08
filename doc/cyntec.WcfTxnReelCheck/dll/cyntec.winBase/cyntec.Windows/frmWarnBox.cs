using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using cyntec.Base;
using cyntec.Windows.Properties;

namespace cyntec.Windows;

public class frmWarnBox : Form
{
	public bool Result;

	public int m_Panel1_Height;

	public int m_Panel2_Height;

	public int m_Split_Height;

	public int m_Form_Height;

	public bool m_IsShowDetail = false;

	private cyntecThread m_Voice = null;

	private cyntecThread m_KeepActive = null;

	public bool m_IsKeepActive = true;

	private bool m_IsVoice = false;

	private IContainer components = null;

	private Button btnCancel;

	private Panel panel1;

	private Button btnOK;

	private Panel panel2;

	private Label lblMessage;

	private PictureBox pictureBox1;

	private CheckBox chkDontShow;

	public Button btnDetail;

	private SplitContainer splitContainer1;

	private TextBox txtMessage;

	private TextBox txtDetail;

	public CheckBox chkIsVoice;

	public messageStyle style
	{
		set
		{
			switch (value)
			{
			case messageStyle.askOKCancel:
				btnOK.Tag = "BTN_OK";
				btnCancel.Tag = "BTN_CANCEL";
				pictureBox1.Image = Resources.accepted_48;
				break;
			case messageStyle.askYesNo:
				btnOK.Tag = "BTN_YES";
				btnCancel.Tag = "BTN_NO";
				pictureBox1.Image = Resources.accepted_48;
				break;
			case messageStyle.confirmation:
				btnOK.Tag = "BTN_OK";
				btnCancel.Visible = false;
				pictureBox1.Image = Resources.OK;
				break;
			case messageStyle.Prompt:
				btnOK.Tag = "BTN_OK";
				btnCancel.Visible = false;
				pictureBox1.Image = Resources.accepted_48;
				break;
			case messageStyle.Waring:
				btnOK.Tag = "BTN_OK";
				btnCancel.Visible = false;
				pictureBox1.Image = Resources.warning_48;
				break;
			case messageStyle.Error:
				btnOK.Tag = "BTN_OK";
				btnCancel.Visible = false;
				pictureBox1.Image = Resources.Error;
				break;
			}
		}
	}

	public bool DontShowMessageAgain => chkDontShow.Checked;

	[DllImport("kernel32.dll", ExactSpelling = true)]
	public static extern int Beep(int dwFreq, int dwDuration);

	public frmWarnBox()
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		InitializeComponent();
		m_Split_Height = splitContainer1.Height;
		m_Panel1_Height = splitContainer1.Panel1.Height;
		m_Panel2_Height = splitContainer1.Panel2.Height;
		m_Form_Height = base.Height;
		m_Voice = new cyntecThread();
		m_Voice.onProcess += new ThreadProcessEvent(m_Voice_onProcess);
		m_Voice.SleepTime = 200;
		m_Voice.Start();
		m_KeepActive = new cyntecThread();
		m_KeepActive.onProcess += new ThreadProcessEvent(m_KeepActive_onProcess);
		m_KeepActive.SleepTime = 200;
		m_KeepActive.Start();
	}

	private void m_Voice_onProcess()
	{
		if (m_IsVoice)
		{
			Beep(659, 300);
			cyntecThread.Sleep(1);
			Beep(784, 300);
			cyntecThread.Sleep(1);
			Beep(1047, 300);
		}
	}

	private void m_KeepActive_onProcess()
	{
		if (m_IsKeepActive)
		{
			Invoke((HuiInvoke)delegate
			{
				Activate();
			});
		}
	}

	public void show(string message)
	{
		txtMessage.Text = message;
		m_ShowDetail(IsShowDetail: false);
	}

	public void show(string msgID, string msgDetail)
	{
		lblMessage.Text = msgID;
		txtMessage.Text = msgDetail;
		m_ShowDetail(IsShowDetail: false);
	}

	public void show(string msgID, string msg, string detail)
	{
		lblMessage.Text = msgID;
		txtMessage.Text = msg;
		txtDetail.Text = detail;
		m_ShowDetail(IsShowDetail: false);
	}

	public void m_ShowDetail(bool IsShowDetail)
	{
		m_IsShowDetail = IsShowDetail;
		if (IsShowDetail)
		{
			splitContainer1.Panel2Collapsed = false;
		}
		else
		{
			splitContainer1.Panel2Collapsed = true;
		}
	}

	private void btnOK_Click(object sender, EventArgs e)
	{
		Result = true;
		Hide();
		chkIsVoice.Checked = false;
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		Result = false;
		Hide();
		chkIsVoice.Checked = false;
	}

	private void frmMessageBox_Resize(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Normal;
	}

	private void chkDontShow_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void frmMsgBox_Load(object sender, EventArgs e)
	{
	}

	private void btnDetail_Click(object sender, EventArgs e)
	{
		m_ShowDetail(!m_IsShowDetail);
		btnOK.Focus();
	}

	private void txtDetail_TextChanged(object sender, EventArgs e)
	{
	}

	private void frmMsgBox_FormClosed(object sender, FormClosedEventArgs e)
	{
	}

	private void frmMsgBox_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			m_Voice.Stop();
			m_KeepActive.Stop();
		}
		catch
		{
		}
	}

	private void chkIsVoice_CheckedChanged(object sender, EventArgs e)
	{
		m_IsVoice = chkIsVoice.Checked;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(cyntec.Windows.frmWarnBox));
		this.btnCancel = new System.Windows.Forms.Button();
		this.panel1 = new System.Windows.Forms.Panel();
		this.chkIsVoice = new System.Windows.Forms.CheckBox();
		this.btnDetail = new System.Windows.Forms.Button();
		this.chkDontShow = new System.Windows.Forms.CheckBox();
		this.btnOK = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.lblMessage = new System.Windows.Forms.Label();
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.txtMessage = new System.Windows.Forms.TextBox();
		this.txtDetail = new System.Windows.Forms.TextBox();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnCancel.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.Location = new System.Drawing.Point(1041, 0);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(119, 32);
		this.btnCancel.TabIndex = 1;
		this.btnCancel.Text = "取消(Cancel)";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.panel1.Controls.Add(this.chkIsVoice);
		this.panel1.Controls.Add(this.btnDetail);
		this.panel1.Controls.Add(this.chkDontShow);
		this.panel1.Controls.Add(this.btnOK);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.btnCancel);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel1.Location = new System.Drawing.Point(0, 459);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1160, 32);
		this.panel1.TabIndex = 2;
		this.chkIsVoice.AutoSize = true;
		this.chkIsVoice.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		this.chkIsVoice.Location = new System.Drawing.Point(235, 7);
		this.chkIsVoice.Name = "chkIsVoice";
		this.chkIsVoice.Size = new System.Drawing.Size(75, 20);
		this.chkIsVoice.TabIndex = 8;
		this.chkIsVoice.Text = "警報聲";
		this.chkIsVoice.UseVisualStyleBackColor = true;
		this.chkIsVoice.CheckedChanged += new System.EventHandler(chkIsVoice_CheckedChanged);
		this.btnDetail.Dock = System.Windows.Forms.DockStyle.Left;
		this.btnDetail.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDetail.Location = new System.Drawing.Point(0, 0);
		this.btnDetail.Name = "btnDetail";
		this.btnDetail.Size = new System.Drawing.Size(52, 32);
		this.btnDetail.TabIndex = 7;
		this.btnDetail.Text = "細節";
		this.btnDetail.UseVisualStyleBackColor = true;
		this.btnDetail.Visible = false;
		this.btnDetail.Click += new System.EventHandler(btnDetail_Click);
		this.chkDontShow.AutoSize = true;
		this.chkDontShow.Location = new System.Drawing.Point(12, 11);
		this.chkDontShow.Name = "chkDontShow";
		this.chkDontShow.Size = new System.Drawing.Size(77, 16);
		this.chkDontShow.TabIndex = 6;
		this.chkDontShow.Tag = "dontShowAgain";
		this.chkDontShow.Text = "checkBox1";
		this.chkDontShow.UseVisualStyleBackColor = true;
		this.chkDontShow.Visible = false;
		this.chkDontShow.CheckedChanged += new System.EventHandler(chkDontShow_CheckedChanged);
		this.btnOK.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnOK.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnOK.Location = new System.Drawing.Point(911, 0);
		this.btnOK.Name = "btnOK";
		this.btnOK.Size = new System.Drawing.Size(125, 32);
		this.btnOK.TabIndex = 5;
		this.btnOK.Text = "確認(OK)";
		this.btnOK.UseVisualStyleBackColor = true;
		this.btnOK.Click += new System.EventHandler(btnOK_Click);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
		this.panel2.Location = new System.Drawing.Point(1036, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(5, 32);
		this.panel2.TabIndex = 4;
		this.lblMessage.AutoSize = true;
		this.lblMessage.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		this.lblMessage.ForeColor = System.Drawing.Color.Black;
		this.lblMessage.Location = new System.Drawing.Point(47, 9);
		this.lblMessage.Name = "lblMessage";
		this.lblMessage.Size = new System.Drawing.Size(20, 16);
		this.lblMessage.TabIndex = 5;
		this.lblMessage.Tag = "lblWarning";
		this.lblMessage.Text = "--";
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
		this.splitContainer1.Panel1.Controls.Add(this.txtMessage);
		this.splitContainer1.Panel2.Controls.Add(this.txtDetail);
		this.splitContainer1.Size = new System.Drawing.Size(1160, 459);
		this.splitContainer1.SplitterDistance = 197;
		this.splitContainer1.TabIndex = 7;
		this.txtMessage.BackColor = System.Drawing.Color.White;
		this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtMessage.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMessage.ForeColor = System.Drawing.Color.Blue;
		this.txtMessage.Location = new System.Drawing.Point(0, 0);
		this.txtMessage.Multiline = true;
		this.txtMessage.Name = "txtMessage";
		this.txtMessage.ReadOnly = true;
		this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.txtMessage.Size = new System.Drawing.Size(1160, 197);
		this.txtMessage.TabIndex = 4;
		this.txtMessage.Text = "TEST";
		this.txtDetail.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
		this.txtDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDetail.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtDetail.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDetail.ForeColor = System.Drawing.Color.Blue;
		this.txtDetail.Location = new System.Drawing.Point(0, 0);
		this.txtDetail.Multiline = true;
		this.txtDetail.Name = "txtDetail";
		this.txtDetail.ReadOnly = true;
		this.txtDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.txtDetail.Size = new System.Drawing.Size(1160, 258);
		this.txtDetail.TabIndex = 5;
		this.txtDetail.TextChanged += new System.EventHandler(txtDetail_TextChanged);
		this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Right;
		this.pictureBox1.ErrorImage = null;
		this.pictureBox1.Image = cyntec.Windows.Properties.Resources.OK;
		this.pictureBox1.InitialImage = null;
		this.pictureBox1.Location = new System.Drawing.Point(934, 0);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(226, 197);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 6;
		this.pictureBox1.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.btnCancel;
		base.ClientSize = new System.Drawing.Size(1160, 491);
		base.ControlBox = false;
		base.Controls.Add(this.splitContainer1);
		base.Controls.Add(this.lblMessage);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmWarnBox";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = " ";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmMsgBox_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmMsgBox_FormClosed);
		base.Load += new System.EventHandler(frmMsgBox_Load);
		base.Resize += new System.EventHandler(frmMessageBox_Resize);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel1.PerformLayout();
		this.splitContainer1.Panel2.ResumeLayout(false);
		this.splitContainer1.Panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
