using Codesoft6_PrintDemo.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Codesoft6_PrintDemo
{
    public partial class MainForm : Form
    {

        // 不得不说，复制粘贴是真的快乐，简单粗暴
        // 看看这代码，疯狂复制粘贴，笑死
        public MainForm()
        {
            InitializeComponent();
        }
        public static string path = new DirectoryInfo("../../").FullName;
        public string fileName;
        bool locked;
        int scan_count = 0;
        int Print_count = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            Type dgvType = this.dataGridView_value.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(this.dataGridView_value, true, null);

            Type dgvType_his = this.dataGridView_history.GetType();
            PropertyInfo pi_his = dgvType_his.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi_his.SetValue(this.dataGridView_history, true, null);

            label_CheckStatus.Text = string.Empty;

            textBox_A.Text = Settings.Default.SeBarcode_A;
            textBox_B.Text = Settings.Default.SeBarcode_B;
            textBox_C.Text = Settings.Default.SeBarcode_C;
            textBox_D.Text = Settings.Default.SeBarcode_D;
            textBox_W.Text = Settings.Default.SeBarcode_W;

            textBox_A_LEN.Text = Settings.Default.SeBarcode_A_LEN.ToString();
            textBox_B_LEN.Text = Settings.Default.SeBarcode_B_LEN.ToString();
            textBox_C_LEN.Text = Settings.Default.SeBarcode_C_LEN.ToString();
            textBox_D_LEN.Text = Settings.Default.SeBarcode_D_LEN.ToString();
            textBox_W_LEN.Text = Settings.Default.SeBarcode_W_LEN.ToString();


            numericUpDown_Barcode_num.Value = Settings.Default.Barcode_num;
            numericUpDown_PrintNum.Value = Settings.Default.PrintNum;

            checkBox_CheckSNfuction.Checked = Settings.Default.CheckSNfuction;
            textBox_TempPath.Text = Settings.Default.Temple_Path;

            locked = Settings.Default.locked;

            SettingLock(locked);
            Barcode_num_ValueChanged();
            PrinterTips();

        }

        private void Print(string pathlab, string BarcodeA, string BarcodeB, string BarcodeC, string BarcodeD, string BarcodeW, int print_num)
        {
            LabelManager2.Document doc = null;
            string modelPath = textBox_TempPath.Text;
            LabelManager2.ApplicationClass labApp = null;
            labApp = new LabelManager2.ApplicationClass();
            labApp.Documents.Open(pathlab, true);// 调用设计好的label文件
            doc = labApp.ActiveDocument;

            if (Settings.Default.Printer_Sys == false)
            {
                doc.Printer.SwitchTo(Settings.Default.PrinterName);//选择打印机
                //doc.Printer.SwitchTo("ZDesigner ZT410-300dpi ZPL", "USB0001", true);//选择打印机
            }

            //调用填充器
            if (!string.IsNullOrEmpty(BarcodeA)) doc.Variables.FormVariables.Item("A").Value = BarcodeA;
            if (!string.IsNullOrEmpty(BarcodeB)) doc.Variables.FormVariables.Item("B").Value = BarcodeB;
            if (!string.IsNullOrEmpty(BarcodeC)) doc.Variables.FormVariables.Item("C").Value = BarcodeC;
            if (!string.IsNullOrEmpty(BarcodeD)) doc.Variables.FormVariables.Item("D").Value = BarcodeD;
            if (!string.IsNullOrEmpty(BarcodeW)) doc.Variables.FormVariables.Item("W").Value = BarcodeW;

            try
            {
                //当用doc.printLabel(1)打印100个标签时，会生成1个100页的PDF文件
                //当用doc.Printdocument(1)打印100个标签时，会生成100个1页的PDF文件。

                //doc.PrintLabel(1, 1, 1, print_num, 1, "");//打印
                doc.PrintDocument(print_num);
                            
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                doc.FormFeed();
                doc.Close();
                labApp.Documents.CloseAll(true);
                labApp.Quit();
                
            }
        }
        
        private void ScanLocationTips()
        {
            textBox_A.BackColor = SystemColors.Window;
            textBox_B.BackColor = SystemColors.Window;
            textBox_C.BackColor = SystemColors.Window;
            textBox_D.BackColor = SystemColors.Window;
            textBox_W.BackColor = SystemColors.Window;

            textBox_A_LEN.BackColor = SystemColors.Window;
            textBox_B_LEN.BackColor = SystemColors.Window;
            textBox_C_LEN.BackColor = SystemColors.Window;
            textBox_D_LEN.BackColor = SystemColors.Window;
            textBox_W_LEN.BackColor = SystemColors.Window;

            switch (scan_count)
            {
                case 0:
                    toolStripStatusLabel_Location.Text = "当前扫码位置：A";
                    label_locationtips.Text = "请扫描或输入条码 A";
                    textBox_A.BackColor = Color.PaleGreen;
                    textBox_A_LEN.BackColor = Color.PaleGreen;
                    break;
                case 1:
                    toolStripStatusLabel_Location.Text = "当前扫码位置：B";
                    label_locationtips.Text = "请扫描或输入条码 B";
                    textBox_B.BackColor = Color.PaleGreen;
                    textBox_B_LEN.BackColor = Color.PaleGreen;
                    break;
                case 2:
                    toolStripStatusLabel_Location.Text = "当前扫码位置：C";
                    label_locationtips.Text = "请扫描或输入条码 C";
                    textBox_C.BackColor = Color.PaleGreen;
                    textBox_C_LEN.BackColor = Color.PaleGreen;
                    break;
                case 3:
                    toolStripStatusLabel_Location.Text = "当前扫码位置：D";
                    label_locationtips.Text = "请扫描或输入条码 D";
                    textBox_D.BackColor = Color.PaleGreen;
                    textBox_D_LEN.BackColor = Color.PaleGreen;
                    break;
                case 4:
                    toolStripStatusLabel_Location.Text = "当前扫码位置：W";
                    label_locationtips.Text = "请扫描或输入条码 W";
                    textBox_W.BackColor = Color.PaleGreen;
                    textBox_W_LEN.BackColor = Color.PaleGreen;
                    break;
            }
        }
        private void historyDGV_insert(string BarcodeA, string BarcodeB, string BarcodeC, string BarcodeD, string BarcodeW)
        {
            try
            {
                if (dataGridView_history.Rows.Count >= 1000)
                {
                    dataGridView_history.Rows.RemoveAt(0);
                }

                int rows = dataGridView_history.Rows.Add();
                dataGridView_history.Rows[rows].Cells["ColBarcodeA"].Value = BarcodeA;
                dataGridView_history.Rows[rows].Cells["ColBarcodeB"].Value = BarcodeB;
                dataGridView_history.Rows[rows].Cells["ColBarcodeC"].Value = BarcodeC;
                dataGridView_history.Rows[rows].Cells["ColBarcodeD"].Value = BarcodeD;
                dataGridView_history.Rows[rows].Cells["ColBarcodeW"].Value = BarcodeW;
                dataGridView_history.Rows[rows].Cells["ColTime"].Value = DateTime.Now.ToString();

                //显示最后一行
                dataGridView_history.FirstDisplayedScrollingRowIndex = dataGridView_history.Rows.Count - 1;
                dataGridView_history.CurrentCell = dataGridView_history.Rows[rows].Cells[0];

            }
            catch (Exception dgv_ex)
            {
                MessageBox.Show("尝试插入数据到表格时出错\n\n" + dgv_ex, "程序异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void valueDGV_insert(string Name ,string Value)
        {
            try
            {
                int rows = dataGridView_value.Rows.Add();
                dataGridView_value.Rows[rows].Cells["ColName"].Value = Name;
                dataGridView_value.Rows[rows].Cells["ColValue"].Value = Value;

                //显示最后一行
                dataGridView_value.FirstDisplayedScrollingRowIndex = dataGridView_value.Rows.Count - 1;
                dataGridView_value.CurrentCell = dataGridView_value.Rows[rows].Cells[0];
            }
            catch (Exception dgv_ex)
            {
                MessageBox.Show("尝试插入数据到表格时出错\n\n" + dgv_ex, "程序异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void textBox_Barcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                label_CheckStatus.Text = string.Empty;

                if (!File.Exists(textBox_TempPath.Text))
                {
                    MessageBox.Show("当前的选中的模板，路径为：\n" + textBox_TempPath.Text + "\n\n找不到该文件，请解锁设置并重新选择模板", "模板文件路径错误或丢失", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox_Barcode.Text = string.Empty;
                    return;
                }

                if (string.IsNullOrEmpty(textBox_Barcode.Text)) return;

                string Barcode = textBox_Barcode.Text;

                string BarcodeCa_A = textBox_A.Text;
                string BarcodeCa_B = textBox_B.Text;
                string BarcodeCa_C = textBox_C.Text;
                string BarcodeCa_D = textBox_D.Text;
                string BarcodeCa_W = textBox_W.Text;

                int BarcodeCa_A_LEN = int.Parse(textBox_A_LEN.Text);
                int BarcodeCa_B_LEN = int.Parse(textBox_B_LEN.Text);
                int BarcodeCa_C_LEN = int.Parse(textBox_C_LEN.Text);
                int BarcodeCa_D_LEN = int.Parse(textBox_D_LEN.Text);
                int BarcodeCa_W_LEN = int.Parse(textBox_W_LEN.Text);

                int barcode_num = (int)numericUpDown_Barcode_num.Value;

                #region 位置遍历，通过 Scan_Count 作为位置和统计一共要打印多少个条码
                if (scan_count == 0) // A 位置
                {
                    if (BarcodeCa_A_LEN != 0 & Barcode.Length != BarcodeCa_A_LEN)
                    {
                        MessageBox.Show("扫入条码的长度与变量 A 条码长度不匹配，请检查条码是否正确\n扫入长度：" + Barcode.Length, "条码长度不匹配", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (checkBox_CheckSNfuction.Checked & !Barcode.StartsWith(BarcodeCa_A))
                    {
                        MessageBox.Show("扫入的条码与变量 A 前缀不匹配，请检查条码是否正确\n\n扫描条码：" + Barcode + "\n变量条码：" + BarcodeCa_A, "条码前缀校验失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else { valueDGV_insert("A", Barcode); scan_count++; }
                }
                else if (scan_count == 1) // B 位置
                {
                    if (BarcodeCa_B_LEN != 0 & Barcode.Length != BarcodeCa_B_LEN)
                    {
                        MessageBox.Show("扫入条码的长度与变量 B 条码长度不匹配，请检查条码是否正确\n扫入长度：" + Barcode.Length, "条码长度不匹配", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (checkBox_CheckSNfuction.Checked & !Barcode.StartsWith(BarcodeCa_B))
                    {
                        MessageBox.Show("扫入的条码与变量 B 前缀不匹配，请检查条码是否正确\n\n扫描条码：" + Barcode + "\n变量条码：" + BarcodeCa_B, "条码前缀校验失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else { valueDGV_insert("B", Barcode); scan_count++; }
                }
                else if (scan_count == 2) // C 位置
                {
                    if (BarcodeCa_C_LEN != 0 & Barcode.Length != BarcodeCa_C_LEN)
                    {
                        MessageBox.Show("扫入条码的长度与变量 C 条码长度不匹配，请检查条码是否正确\n扫入长度：" + Barcode.Length, "条码长度不匹配", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (checkBox_CheckSNfuction.Checked & !Barcode.StartsWith(BarcodeCa_C))
                    {
                        MessageBox.Show("扫入的条码与变量 C 前缀不匹配，请检查条码是否正确\n\n扫描条码：" + Barcode + "\n变量条码：" + BarcodeCa_C, "条码前缀校验失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else { valueDGV_insert("C", Barcode); scan_count++; }
                }
                else if (scan_count == 3) // D 位置
                {
                    if (BarcodeCa_D_LEN != 0 & Barcode.Length != BarcodeCa_D_LEN)
                    {
                        MessageBox.Show("扫入条码的长度与变量 D 条码长度不匹配，请检查条码是否正确\n扫入长度：" + Barcode.Length, "条码长度不匹配", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (checkBox_CheckSNfuction.Checked & !Barcode.StartsWith(BarcodeCa_D))
                    {
                        MessageBox.Show("扫入的条码与变量 D 前缀不匹配，请检查条码是否正确\n\n扫描条码：" + Barcode + "\n变量条码：" + BarcodeCa_D, "条码前缀校验失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else { valueDGV_insert("D", Barcode); scan_count++; }
                }
                else if (scan_count == 4) // W 位置
                {
                    if (BarcodeCa_W_LEN != 0 & Barcode.Length != BarcodeCa_W_LEN)
                    {
                        MessageBox.Show("扫入条码的长度与变量 W 条码长度不匹配，请检查条码是否正确\n扫入长度：" + Barcode.Length, "条码长度不匹配", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (checkBox_CheckSNfuction.Checked & !Barcode.StartsWith(BarcodeCa_B))
                    {
                        MessageBox.Show("扫入的条码与变量 W 前缀不匹配，请检查条码是否正确\n\n扫描条码：" + Barcode + "\n变量条码：" + BarcodeCa_W, "条码前缀校验失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else { valueDGV_insert("W", Barcode); scan_count++; }
                }
                #endregion


                if (scan_count >= barcode_num)
                {
                    string Barcode_A = null;
                    string Barcode_B = null;
                    string Barcode_C = null;
                    string Barcode_D = null;
                    string Barcode_W = null;

                    for (int i = 0; i < dataGridView_value.RowCount; i++)
                    {
                        string dgv_value_barcode = dataGridView_value.Rows[i].Cells["ColValue"].Value.ToString();
                        switch (i)
                        { 
                            case 0:
                                Barcode_A = dgv_value_barcode;
                                break;
                            case 1:
                                Barcode_B = dgv_value_barcode;
                                break;
                            case 2:
                                Barcode_C = dgv_value_barcode;
                                break;
                            case 3:
                                Barcode_D = dgv_value_barcode;
                                break;
                            case 4:
                                Barcode_W = dgv_value_barcode;
                                break;
                        }
                    }
                    historyDGV_insert(Barcode_A, Barcode_B, Barcode_C, Barcode_D, Barcode_W);

                    //Printer_Sys PrinterName
                    try
                    {
                        Print(textBox_TempPath.Text, Barcode_A, Barcode_B, Barcode_C, Barcode_D, Barcode_W, (int)numericUpDown_PrintNum.Value);
                        label_CheckStatus.Text = "已成功向 Windows 打印机管理发送打印任务！";
                        Print_count++;
                        toolStripStatusLabel_PrintCount.Text = "打印数量：" + Print_count.ToString();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "未将对象引用设置到对象的实例。")
                        {
                            MessageBox.Show("打印失败，请将以下信息反馈给工程部！\n\n请尝试检查以下问题：\n1. 打印模板中无法找到对应的变量\n2. CodeSoft 6 尚未安装\n\n异常报告：\n" + ex.Message, "打印时发生异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            label_CheckStatus.Text = "打印失败！在打印模板中无法找到对应的变量：" + ex.Message;
                        }
                        else
                        {
                            MessageBox.Show("打印时程序异常\n\n" + ex, "打印时发生异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            label_CheckStatus.Text = "打印时程序异常" + ex.Message;
                        }

                    }
                    finally
                    {
                        scan_count = 0;
                        dataGridView_value.Rows.Clear();
                    }
                }

                textBox_Barcode.Text = string.Empty;
                ScanLocationTips();
                
            }
        }



        private void button_lock_Click(object sender, EventArgs e)
        {
            if (locked == true)
            {
                LockForm lockForm = new LockForm();
                lockForm.ShowDialog();
                if (lockForm.Check)
                {
                    SettingLock(false);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(textBox_TempPath.Text))
                {
                    MessageBox.Show("模板尚未选择，请先选好模板", "模板没有设置", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!File.Exists(textBox_TempPath.Text))
                {
                    MessageBox.Show("当前的选中的模板，路径为：\n" + textBox_TempPath.Text + "\n\n找不到该文件，请重新选择模板", "模板文件路径错误或丢失", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                if (numericUpDown_Barcode_num.Value < 1 || numericUpDown_Barcode_num.Value > 5)
                {
                    MessageBox.Show("条码数量设置必须在 1 ~ 5 区间内", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(textBox_A_LEN.Text)) { textBox_A_LEN.Text = "0"; }
                if (string.IsNullOrEmpty(textBox_B_LEN.Text)) { textBox_B_LEN.Text = "0"; }
                if (string.IsNullOrEmpty(textBox_C_LEN.Text)) { textBox_C_LEN.Text = "0"; }
                if (string.IsNullOrEmpty(textBox_D_LEN.Text)) { textBox_D_LEN.Text = "0"; }
                if (string.IsNullOrEmpty(textBox_W_LEN.Text)) { textBox_W_LEN.Text = "0"; }

                if (!int.TryParse(textBox_A_LEN.Text, out int angA)) { MessageBox.Show("条码 A 的长度设置值不是一个整数，请输入正确的整数", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!int.TryParse(textBox_B_LEN.Text, out int angB)) { MessageBox.Show("条码 B 的长度设置值不是一个整数，请输入正确的整数", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!int.TryParse(textBox_C_LEN.Text, out int angC)) { MessageBox.Show("条码 C 的长度设置值不是一个整数，请输入正确的整数", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!int.TryParse(textBox_D_LEN.Text, out int angD)) { MessageBox.Show("条码 D 的长度设置值不是一个整数，请输入正确的整数", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!int.TryParse(textBox_W_LEN.Text, out int angW)) { MessageBox.Show("条码 W 的长度设置值不是一个整数，请输入正确的整数", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if(checkBox_CheckSNfuction.Checked)
                {
                    if (numericUpDown_Barcode_num.Value >= 1)
                    {
                        if (string.IsNullOrEmpty(textBox_A.Text)) { MessageBox.Show("条码 A 的前缀设置为空，请设置条码前缀字符！", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                    }
                    if (numericUpDown_Barcode_num.Value >= 2)
                    {
                        if (string.IsNullOrEmpty(textBox_B.Text)) { MessageBox.Show("条码 B 的前缀设置为空，请设置条码前缀字符！", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                    }
                    if (numericUpDown_Barcode_num.Value >= 3)
                    {
                        if (string.IsNullOrEmpty(textBox_C.Text)) { MessageBox.Show("条码 C 的前缀设置为空，请设置条码前缀字符！", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                    }
                    if (numericUpDown_Barcode_num.Value >= 4)
                    {
                        if (string.IsNullOrEmpty(textBox_D.Text)) { MessageBox.Show("条码 D 的前缀设置为空，请设置条码前缀字符！", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                    }
                    if (numericUpDown_Barcode_num.Value >= 5)
                    {
                        if (string.IsNullOrEmpty(textBox_W.Text)) { MessageBox.Show("条码 W 的前缀设置为空，请设置条码前缀字符！", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                    }
                }
                if (MessageBox.Show("请确认扫码前的基础设置，确认是否锁定并开始扫码打印准备？","准备就绪",MessageBoxButtons.OKCancel,MessageBoxIcon.Question) == DialogResult.OK)
                {
                    SettingLock(true);
                }

                
            }

        }

        private void SettingLock(bool locked_TF)
        {
            if (locked_TF == true) //上锁，锁定设置界面
            {
                button_lock.Text = "解锁";
                locked = true;

                ScanLocationTips();
                textBox_A.Enabled = false;
                textBox_B.Enabled = false;
                textBox_C.Enabled = false;
                textBox_D.Enabled = false;
                textBox_W.Enabled = false;

                textBox_A_LEN.Enabled = false;
                textBox_B_LEN.Enabled = false;
                textBox_C_LEN.Enabled = false;
                textBox_D_LEN.Enabled = false;
                textBox_W_LEN.Enabled = false;

                numericUpDown_Barcode_num.Enabled = false;
                numericUpDown_PrintNum.Enabled = false;

                checkBox_CheckSNfuction.Enabled = false;

                textBox_Barcode.Enabled = true;
                textBox_Barcode.Focus();
            }
            else //解锁
            {
                button_lock.Text = "锁定";
                locked = false;
                toolStripStatusLabel_Location.Text = "准备就绪";
                label_locationtips.Text = "打印准备就绪"; 

                scan_count = 0;
                dataGridView_value.Rows.Clear();


                Barcode_num_ValueChanged();
                numericUpDown_Barcode_num.Enabled = true;
                numericUpDown_PrintNum.Enabled = true;

                checkBox_CheckSNfuction.Enabled = true;

                textBox_Barcode.Enabled = false;

                textBox_A.BackColor = SystemColors.Window;
                textBox_B.BackColor = SystemColors.Window;
                textBox_C.BackColor = SystemColors.Window;
                textBox_D.BackColor = SystemColors.Window;
                textBox_W.BackColor = SystemColors.Window;

                textBox_A_LEN.BackColor = SystemColors.Window;
                textBox_B_LEN.BackColor = SystemColors.Window;
                textBox_C_LEN.BackColor = SystemColors.Window;
                textBox_D_LEN.BackColor = SystemColors.Window;
                textBox_W_LEN.BackColor = SystemColors.Window;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (locked) //锁定时，保存配置
            {
                Settings.Default.SeBarcode_A = textBox_A.Text;
                Settings.Default.SeBarcode_B = textBox_B.Text;
                Settings.Default.SeBarcode_C = textBox_C.Text;
                Settings.Default.SeBarcode_D = textBox_D.Text;
                Settings.Default.SeBarcode_W = textBox_W.Text;

                Settings.Default.SeBarcode_A_LEN = int.Parse(textBox_A_LEN.Text);
                Settings.Default.SeBarcode_B_LEN = int.Parse(textBox_B_LEN.Text);
                Settings.Default.SeBarcode_C_LEN = int.Parse(textBox_C_LEN.Text);
                Settings.Default.SeBarcode_D_LEN = int.Parse(textBox_D_LEN.Text);
                Settings.Default.SeBarcode_W_LEN = int.Parse(textBox_W_LEN.Text);

                Settings.Default.CheckSNfuction = checkBox_CheckSNfuction.Checked;
            }
            else //未锁定时，恢复默认设置
            {
                Settings.Default.SeBarcode_A = string.Empty;
                Settings.Default.SeBarcode_B = string.Empty;
                Settings.Default.SeBarcode_C = string.Empty;
                Settings.Default.SeBarcode_D = string.Empty;
                Settings.Default.SeBarcode_W = string.Empty;

                Settings.Default.SeBarcode_A_LEN = 0;
                Settings.Default.SeBarcode_B_LEN = 0;
                Settings.Default.SeBarcode_C_LEN = 0;
                Settings.Default.SeBarcode_D_LEN = 0;
                Settings.Default.SeBarcode_W_LEN = 0;

                Settings.Default.CheckSNfuction = false;
            }

            Settings.Default.locked = locked;
            Settings.Default.Temple_Path = textBox_TempPath.Text;
            Settings.Default.Barcode_num = (int)numericUpDown_Barcode_num.Value;
            Settings.Default.PrintNum = (int)numericUpDown_PrintNum.Value;
            Settings.Default.Save();




        }

        public void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (locked == true)
            {
                LockForm lockForm = new LockForm();
                lockForm.ShowDialog();

                if (!lockForm.Check)
                {
                    return;
                }
                else
                {
                    SettingLock(false);
                }
            }

            SettingLock(false);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Codesoft 6 模板文件|*.lab";
            ofd.ShowDialog();
            fileName = ofd.FileName; //获得选择的文件路径
            textBox_TempPath.Text = fileName;
            //System.IO.File.WriteAllText(RecordPath, fileName, Encoding.UTF8);
            //path = fileName;

        }

        private void 打开打印机控制面板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string controlpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "control.exe"); // path to %windir%\system32\control.exe
            Process.Start(controlpath, "/name Microsoft.DevicesAndPrinters");// (ensures the correct control.exe)

        }


        private void 打开当前模板文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileFullPath = textBox_TempPath.Text;
            if (string.IsNullOrEmpty(fileFullPath))
            {
                MessageBox.Show("没有设置模板路径，无法打开", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process.Start(fileFullPath);
            }
            catch(Exception ex)
            {
                if (ex.Message == "系统找不到指定的文件。") { MessageBox.Show("无法打开当前模板，模板丢失，请检查是否被删除","模板丢失",MessageBoxButtons.OK,MessageBoxIcon.Error); }
                else { MessageBox.Show("无法打开当前模板，模板可能已损坏或者丢失\n\n" + ex.Message, "模板无法打开", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        public void PrinterTips()
        {
            if (Settings.Default.Printer_Sys == false)
            {
                toolStripStatusLabel_Printer.Text = $"自定义打印机：" + Settings.Default.PrinterName;//选择打印机
            }
            else
            {
                toolStripStatusLabel_Printer.Text = "使用系统打印机自动管理打印";//选择打印机
            }
        }

        private void 打印机基本设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (locked == true)
            {
                LockForm lockForm = new LockForm();
                lockForm.ShowDialog();
                if (!lockForm.Check)
                {
                    return;
                }
                else
                {
                    SettingLock(false);
                }
            }
            SettingForm settingForm = new SettingForm();
            settingForm.ShowDialog();
            PrinterTips();


        }

        private void Barcode_num_ValueChanged()
        {
            if (!locked)
            {
                if (numericUpDown_Barcode_num.Value >= 1)
                {
                    textBox_A.Enabled = true;
                    textBox_A_LEN.Enabled = true;
                }
                else
                {
                    textBox_A.Enabled = false;
                    textBox_A_LEN.Enabled = false;
                }

                if (numericUpDown_Barcode_num.Value >= 2)
                {
                    textBox_B.Enabled = true;
                    textBox_B_LEN.Enabled = true;
                }
                else
                {
                    textBox_B.Enabled = false;
                    textBox_B_LEN.Enabled = false;
                }

                if (numericUpDown_Barcode_num.Value >= 3)
                {
                    textBox_C.Enabled = true;
                    textBox_C_LEN.Enabled = true;
                }
                else
                {
                    textBox_C.Enabled = false;
                    textBox_C_LEN.Enabled = false;
                }

                if (numericUpDown_Barcode_num.Value >= 4)
                {
                    textBox_D.Enabled = true;
                    textBox_D_LEN.Enabled = true;
                }
                else
                {
                    textBox_D.Enabled = false;
                    textBox_D_LEN.Enabled = false;
                }

                if (numericUpDown_Barcode_num.Value >= 5)
                {
                    textBox_W.Enabled = true;
                    textBox_W_LEN.Enabled = true;
                }
                else
                {
                    textBox_W.Enabled = false;
                    textBox_W_LEN.Enabled = false;
                }
            }
            
            
        }

        private void numericUpDown_Barcode_num_ValueChanged(object sender, EventArgs e)
        {
            Barcode_num_ValueChanged();
        }

        private void dataGridView_value_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, new SolidBrush(Color.Black), e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);
            }
            catch (Exception ex)
            {
                MessageBox.Show("表格在添加行号时发生错误，错误信息：\n\n" + ex, "程序异常");
            }
        }

        private void dataGridView_history_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            
            try
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, new SolidBrush(Color.Black), e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);
            }
            catch (Exception ex)
            {
                MessageBox.Show("表格在添加行号时发生错误，错误信息：\n\n" + ex, "程序异常");
            }
        }

        private void dataGridView_history_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult dsr = MessageBox.Show("确实要重复打印当前选中行的条码吗？", "重复打印确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dsr == DialogResult.OK)
            {
                BarcodeRePrint();
            }

        }

        private void BarcodeRePrint()
        {
            if (dataGridView_history.Rows.Count <= 0) return;

            int index = dataGridView_history.CurrentRow.Index;

            string BarcodeA = null;
            string BarcodeB = null;
            string BarcodeC = null;
            string BarcodeD = null;
            string BarcodeW = null;

            object DGV_BA = dataGridView_history.Rows[index].Cells["ColBarcodeA"].Value;
            object DGV_BB = dataGridView_history.Rows[index].Cells["ColBarcodeB"].Value;
            object DGV_BC = dataGridView_history.Rows[index].Cells["ColBarcodeC"].Value;
            object DGV_BD = dataGridView_history.Rows[index].Cells["ColBarcodeD"].Value;
            object DGV_BW = dataGridView_history.Rows[index].Cells["ColBarcodeW"].Value;

            if (DGV_BA != null)
            {
                BarcodeA = dataGridView_history.Rows[index].Cells["ColBarcodeA"].Value.ToString();//主要条码
            }
            if (DGV_BB != null)
            {
                BarcodeB = dataGridView_history.Rows[index].Cells["ColBarcodeA"].Value.ToString();//主要条码
            }
            if (DGV_BC != null)
            {
                BarcodeC = dataGridView_history.Rows[index].Cells["ColBarcodeA"].Value.ToString();//主要条码
            }
            if (DGV_BD != null)
            {
                BarcodeD = dataGridView_history.Rows[index].Cells["ColBarcodeA"].Value.ToString();//主要条码
            }
            if (DGV_BW != null)
            {
                BarcodeW = dataGridView_history.Rows[index].Cells["ColBarcodeA"].Value.ToString();//主要条码
            }

            try
            {
                Print(textBox_TempPath.Text, BarcodeA, BarcodeB, BarcodeC, BarcodeD, BarcodeW, (int)numericUpDown_PrintNum.Value);
                label_CheckStatus.Text = "【重复打印】已成功向 Windows 打印机管理发送打印任务！";
            }
            catch (Exception ex)
            {
                if (ex.Message == "未将对象引用设置到对象的实例。")
                {
                    MessageBox.Show("打印失败\n\n请尝试检查以下问题：\n1. 打印模板中无法找到对应的变量\n2. CodeSoft 6 尚未安装\n\n异常报告：\n" + ex.Message, "打印时发生异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    label_CheckStatus.Text = "打印失败！在打印模板中无法找到对应的变量：" + ex.Message;
                }
                else
                {
                    MessageBox.Show("打印时程序异常\n\n" + ex, "打印时发生异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    label_CheckStatus.Text = "打印时程序异常" + ex.Message;
                }
            }
        }

        private void dataGridView_history_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {

            
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    dataGridView_history.ClearSelection();
                    dataGridView_history.Rows[e.RowIndex].Selected = true;
                    contextMenuStrip_dgv_history.Show(MousePosition.X, MousePosition.Y);
                }
                /**
                //只选中一行时设置活动单元格
                if (dataGridView_history.SelectedRows.Count == 1)
                {
                    dataGridView_history.CurrentCell = dataGridView_history.Rows[e.RowIndex].Cells[e.ColumnIndex];
                }
                **/
                //弹出操作菜单
                
            }
            
        }

        private void toolStripMenuItem_RePrint_Click(object sender, EventArgs e)
        {
            BarcodeRePrint();
        }

        private void toolStripMenuItem_Copy_Click(object sender, EventArgs e)
        {
            //tb2 = tb1.row[i].copy(); i你是想要复制那一行
            int index = dataGridView_history.CurrentRow.Index;
            //Clipboard.SetDataObject(dataGridView_history.Rows[index].Cells);

            if (dataGridView_history.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                try
                {
                    // Add the selection to the clipboard.
                    Clipboard.SetDataObject(dataGridView_history.GetClipboardContent());

                    // Replace the text box contents with the clipboard text.
                    MessageBox.Show(Clipboard.GetText());

                }
                catch (System.Runtime.InteropServices.ExternalException ex)
                {
                    //MessageBox.Show("The Clipboard could not be accessed. Please try again.\n\n" + ex);
                    MessageBox.Show("粘贴板无法访问，复制数据失败，请检查粘贴板状态\n\n" + ex,"粘贴板异常",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }


        }
    }
}
