using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cyntec.Windows;

public class frmFunctionForm : Form
{
	private IContainer components = null;

	protected SaveFileDialog dlgSave;

	public frmFunctionForm()
	{
		InitializeComponent();
	}

	public static string GET_TAG_VALUE(object tag)
	{
		return "";
	}

	private void frmFunctionForm_Load(object sender, EventArgs e)
	{
	}

	private void frmPaintEvent(object sender, PaintEventArgs e)
	{
		if (sender != null && !(sender is Form))
		{
		}
	}

	private void labelTextChange(object sender, EventArgs e)
	{
		if (sender != null && sender is Label)
		{
			Label label = (Label)sender;
			Control control = label.Parent;
		}
	}

	private static string controlType(Control ctl)
	{
		if (ctl is DateTimePicker)
		{
			return "DateTimePicker";
		}
		if (ctl is MaskedTextBox)
		{
			return "MaskedTextBox";
		}
		if (ctl is ComboBox)
		{
			return "ComboBox";
		}
		if (ctl is WebBrowser)
		{
			return "WebBrowser";
		}
		if (ctl is TreeView)
		{
			return "TreeView";
		}
		if (ctl is ListView)
		{
			return "ListView";
		}
		if (ctl is MenuStrip)
		{
			return "MenuStrip";
		}
		if (ctl is ContextMenuStrip)
		{
			return "ContextMenuStrip";
		}
		if (ctl is ToolStrip)
		{
			return "ToolStrip";
		}
		if (ctl is StatusStrip)
		{
			return "StatusStrip";
		}
		return ctl.GetType().Name;
	}

	public virtual void SwitchLanguage(string language)
	{
		throw new Exception("SwitchLanguage no implemented");
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
		this.dlgSave = new System.Windows.Forms.SaveFileDialog();
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(292, 273);
		base.Name = "frmFunctionForm";
		base.Tag = "";
		this.Text = "frmFunctionForm";
		base.Load += new System.EventHandler(frmFunctionForm_Load);
		base.ResumeLayout(false);
	}
}
