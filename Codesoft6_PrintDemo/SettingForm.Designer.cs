
namespace Codesoft6_PrintDemo
{
    partial class SettingForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.radioButton_Default = new System.Windows.Forms.RadioButton();
            this.radioButton_setting = new System.Windows.Forms.RadioButton();
            this.label_CheckStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(21, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 36);
            this.label1.TabIndex = 50;
            this.label1.Text = "打印机选择";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.ItemHeight = 20;
            this.comboBox2.Location = new System.Drawing.Point(27, 147);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(550, 28);
            this.comboBox2.TabIndex = 66;
            // 
            // radioButton_Default
            // 
            this.radioButton_Default.AutoSize = true;
            this.radioButton_Default.Checked = true;
            this.radioButton_Default.Location = new System.Drawing.Point(27, 77);
            this.radioButton_Default.Name = "radioButton_Default";
            this.radioButton_Default.Size = new System.Drawing.Size(330, 24);
            this.radioButton_Default.TabIndex = 67;
            this.radioButton_Default.TabStop = true;
            this.radioButton_Default.Text = "使用 Windows 打印机管理器自动选择打印机";
            this.radioButton_Default.UseVisualStyleBackColor = true;
            // 
            // radioButton_setting
            // 
            this.radioButton_setting.AutoSize = true;
            this.radioButton_setting.Location = new System.Drawing.Point(27, 107);
            this.radioButton_setting.Name = "radioButton_setting";
            this.radioButton_setting.Size = new System.Drawing.Size(120, 24);
            this.radioButton_setting.TabIndex = 68;
            this.radioButton_setting.Text = "自定义打印机";
            this.radioButton_setting.UseVisualStyleBackColor = true;
            // 
            // label_CheckStatus
            // 
            this.label_CheckStatus.AutoSize = true;
            this.label_CheckStatus.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CheckStatus.Location = new System.Drawing.Point(23, 192);
            this.label_CheckStatus.Name = "label_CheckStatus";
            this.label_CheckStatus.Size = new System.Drawing.Size(84, 20);
            this.label_CheckStatus.TabIndex = 69;
            this.label_CheckStatus.Text = "打印状态：";
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(615, 239);
            this.Controls.Add(this.label_CheckStatus);
            this.Controls.Add(this.radioButton_setting);
            this.Controls.Add(this.radioButton_Default);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "打印机选择";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingForm_FormClosing);
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.RadioButton radioButton_Default;
        private System.Windows.Forms.RadioButton radioButton_setting;
        private System.Windows.Forms.Label label_CheckStatus;
    }
}