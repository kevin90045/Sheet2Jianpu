namespace Sheet2Jianpu
{
    partial class OptionsForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBox_align = new System.Windows.Forms.CheckBox();
            this.checkBox_sharp_alter = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBox_align
            // 
            this.checkBox_align.AutoSize = true;
            this.checkBox_align.Checked = true;
            this.checkBox_align.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_align.Location = new System.Drawing.Point(25, 22);
            this.checkBox_align.Name = "checkBox_align";
            this.checkBox_align.Size = new System.Drawing.Size(108, 16);
            this.checkBox_align.TabIndex = 0;
            this.checkBox_align.Text = "對齊各部別音符";
            this.checkBox_align.UseVisualStyleBackColor = true;
            this.checkBox_align.CheckedChanged += new System.EventHandler(this.checkBox_align_CheckedChanged);
            // 
            // checkBox_sharp_alter
            // 
            this.checkBox_sharp_alter.AutoSize = true;
            this.checkBox_sharp_alter.Checked = true;
            this.checkBox_sharp_alter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_sharp_alter.Location = new System.Drawing.Point(25, 48);
            this.checkBox_sharp_alter.Name = "checkBox_sharp_alter";
            this.checkBox_sharp_alter.Size = new System.Drawing.Size(132, 16);
            this.checkBox_sharp_alter.TabIndex = 1;
            this.checkBox_sharp_alter.Text = "轉換降記號為升記號";
            this.checkBox_sharp_alter.UseVisualStyleBackColor = true;
            this.checkBox_sharp_alter.CheckedChanged += new System.EventHandler(this.checkBox_sharp_alter_CheckedChanged);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(180, 88);
            this.Controls.Add(this.checkBox_sharp_alter);
            this.Controls.Add(this.checkBox_align);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.Text = "設定";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_align;
        private System.Windows.Forms.CheckBox checkBox_sharp_alter;
    }
}