using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using dshow;
using dshow.Core;

namespace motion
{

	public class CaptureDeviceForm : System.Windows.Forms.Form
	{
		FilterCollection filters;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox deviceCombo;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private string device;

		
		private System.ComponentModel.Container components = null;

	
		public string Device
		{
			get { return device; }
		}

	
		public CaptureDeviceForm()
		{
		
			InitializeComponent();

			//
			try
			{
				filters = new FilterCollection(FilterCategory.VideoInputDevice);

				foreach (Filter filter in filters)
				{
					deviceCombo.Items.Add(filter.Name);
				}
			}
			catch (ApplicationException)
			{
				deviceCombo.Items.Add("没有");
				deviceCombo.Enabled = false;
				okButton.Enabled = false;
			}

			deviceCombo.SelectedIndex = 0;
		}

		
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.deviceCombo = new System.Windows.Forms.ComboBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择设备:";
            // 
            // deviceCombo
            // 
            this.deviceCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deviceCombo.Location = new System.Drawing.Point(12, 32);
            this.deviceCombo.Name = "deviceCombo";
            this.deviceCombo.Size = new System.Drawing.Size(390, 20);
            this.deviceCombo.TabIndex = 6;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Location = new System.Drawing.Point(216, 84);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(90, 25);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okButton.Location = new System.Drawing.Point(26, 84);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(90, 25);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "Ok";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // CaptureDeviceForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(344, 113);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.deviceCombo);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CaptureDeviceForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "探头";
            this.ResumeLayout(false);

		}
		#endregion

		
		private void okButton_Click(object sender, System.EventArgs e)
		{
			device = filters[deviceCombo.SelectedIndex].MonikerString;
		}
	}
}
