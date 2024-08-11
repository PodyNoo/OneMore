
namespace River.OneMoreAddIn.Settings
{
	partial class RemindersSheet
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.introBox = new River.OneMoreAddIn.UI.MoreMultilineLabel();
			this.layoutPanel = new System.Windows.Forms.Panel();
			this.colorCheckBox = new River.OneMoreAddIn.UI.MoreCheckBox();
			this.clickLabel = new System.Windows.Forms.Label();
			this.colorBox = new River.OneMoreAddIn.UI.MorePictureBox();
			this.colorLabel = new System.Windows.Forms.Label();
			this.layoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.colorBox)).BeginInit();
			this.SuspendLayout();
			// 
			// introBox
			// 
			this.introBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.introBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.introBox.ForeColor = System.Drawing.SystemColors.ControlText;
			this.introBox.Location = new System.Drawing.Point(9, 5);
			this.introBox.Margin = new System.Windows.Forms.Padding(2);
			this.introBox.Name = "introBox";
			this.introBox.Padding = new System.Windows.Forms.Padding(0, 0, 0, 23);
			this.introBox.Size = new System.Drawing.Size(514, 43);
			this.introBox.TabIndex = 3;
			this.introBox.Text = "Customize reminders related commands";
			this.introBox.ThemedBack = "ControlLightLight";
			this.introBox.ThemedFore = "ControlText";
			// 
			// layoutPanel
			// 
			this.layoutPanel.Controls.Add(this.colorCheckBox);
			this.layoutPanel.Controls.Add(this.clickLabel);
			this.layoutPanel.Controls.Add(this.colorBox);
			this.layoutPanel.Controls.Add(this.colorLabel);
			this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutPanel.Location = new System.Drawing.Point(9, 48);
			this.layoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.layoutPanel.Name = "layoutPanel";
			this.layoutPanel.Size = new System.Drawing.Size(514, 271);
			this.layoutPanel.TabIndex = 4;
			// 
			// colorCheckBox
			// 
			this.colorCheckBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
			this.colorCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.colorCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.colorCheckBox.Location = new System.Drawing.Point(10, 26);
			this.colorCheckBox.Name = "colorCheckBox";
			this.colorCheckBox.Size = new System.Drawing.Size(375, 15);
			this.colorCheckBox.StylizeImage = false;
			this.colorCheckBox.TabIndex = 6;
			this.colorCheckBox.Text = "Colorize when using \"Strikethrough Completed To Do Tags\" command";
			this.colorCheckBox.ThemedBack = null;
			this.colorCheckBox.ThemedFore = null;
			this.colorCheckBox.UseVisualStyleBackColor = false;
			this.colorCheckBox.CheckedChanged += new System.EventHandler(this.colorCheckBox_CheckedChanged);
			// 
			// clickLabel
			// 
			this.clickLabel.AutoSize = true;
			this.clickLabel.Location = new System.Drawing.Point(150, 60);
			this.clickLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.clickLabel.Name = "clickLabel";
			this.clickLabel.Size = new System.Drawing.Size(86, 13);
			this.clickLabel.TabIndex = 4;
			this.clickLabel.Text = "(click to change)";
			this.clickLabel.Visible = false;
			// 
			// colorBox
			// 
			this.colorBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.colorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.colorBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.colorBox.Enabled = false;
			this.colorBox.Location = new System.Drawing.Point(50, 60);
			this.colorBox.Margin = new System.Windows.Forms.Padding(2);
			this.colorBox.Name = "colorBox";
			this.colorBox.Size = new System.Drawing.Size(89, 14);
			this.colorBox.TabIndex = 1;
			this.colorBox.TabStop = false;
			this.colorBox.Click += new System.EventHandler(this.ChangeLineColor);
			// 
			// colorLabel
			// 
			this.colorLabel.AutoSize = true;
			this.colorLabel.Location = new System.Drawing.Point(10, 60);
			this.colorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.colorLabel.Name = "colorLabel";
			this.colorLabel.Size = new System.Drawing.Size(31, 13);
			this.colorLabel.TabIndex = 0;
			this.colorLabel.Text = "Color";
			// 
			// RemindersSheet
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.Controls.Add(this.layoutPanel);
			this.Controls.Add(this.introBox);
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "RemindersSheet";
			this.Padding = new System.Windows.Forms.Padding(9, 5, 10, 6);
			this.Size = new System.Drawing.Size(533, 325);
			this.layoutPanel.ResumeLayout(false);
			this.layoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.colorBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private UI.MoreMultilineLabel introBox;
		private System.Windows.Forms.Panel layoutPanel;
		private System.Windows.Forms.Label clickLabel;
		private River.OneMoreAddIn.UI.MorePictureBox colorBox;
		private System.Windows.Forms.Label colorLabel;
		private UI.MoreCheckBox colorCheckBox;
	}
}
