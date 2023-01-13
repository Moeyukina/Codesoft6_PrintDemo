using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Codesoft6_PrintDemo
{
    public partial class LockForm : Form
    {
        public LockForm()
        {
            InitializeComponent();
        }

        public bool Check = false;

        private void button_OK_Click(object sender, EventArgs e)
        {
            check();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                check();
            }
        }

        private void check()
        {
            if (string.IsNullOrEmpty(textBox1.Text)) return;
            if (checkpassword(textBox1.Text))
            {
                Check = true;
                Close();
            }
            else
            {
                MessageBox.Show("密码错误，请重新输入", "密码错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = string.Empty;
            }
        }

        private bool checkpassword(string password)
        {
            
            string re = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
            string[] reg = re.Split(new char[] { '-' });
            string reg_all = reg[0] + reg[1] + reg[2] + reg[3] + reg[4];
            //比如 2023-01-08 01：05，则密码为202301080105

            /** 淘汰加密方法
            string re1 = DateTime.Now.Year.ToString();
            string re2 = DateTime.Now.Month.ToString();
            string re3 = DateTime.Now.Day.ToString();
            string re4 = (DateTime.Now.Hour * 2).ToString();
            string re5 = DateTime.Now.Minute.ToString();
            string re_all = re1 + re2 + re3 + re4 + re5; 
            //注意去零，比如 2023-01-08 01：05，则密码为20231825
            **/

            string key = "738724881C7145FB88BD5A159D6D0D7B";

            
            if (password == reg_all | password == "gongchengbu" | password == key)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void LockForm_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}
