using Codesoft6_PrintDemo.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Codesoft6_PrintDemo
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }
        private void SettingForm_Load(object sender, EventArgs e)
        {
            label_CheckStatus.Text = "";
            SearchPrinter();

            if (Settings.Default.Printer_Sys == true)
            {
                radioButton_Default.Checked = true;
            }
            else
            {
                radioButton_setting.Checked = true;
            }

        }
        private void SearchPrinter()
        {
            PrintDocument print = new PrintDocument();
            string sDefault = print.PrinterSettings.PrinterName;//默认打印机名
            foreach (string sPrint in PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                comboBox2.Items.Add(sPrint);
                //if (sPrint == sDefault)
                    //comboBox2.SelectedIndex = comboBox2.Items.IndexOf(sPrint);
            }
            comboBox2.SelectedIndex = Settings.Default.PrinterID;
            if (comboBox2.Items.Count == 0)
            {
                label_CheckStatus.Text = "在您的计算机上无法找到打印机！";
            }
            else if (comboBox2.Text != Settings.Default.PrinterName)
            {
                comboBox2.SelectedIndex = 0;
            }
            else
            {
                comboBox2.SelectedIndex = Settings.Default.PrinterID;
            }
        }
        private void SettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.PrinterName = comboBox2.Text;

            if (radioButton_Default.Checked == true)
            {
                Settings.Default.Printer_Sys = true;
            }
            else
            {
                Settings.Default.Printer_Sys = false;
            }
            Settings.Default.PrinterID = comboBox2.SelectedIndex;
            Settings.Default.Save();
        }
    }
}


