using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace BatRunner
{

    public partial class FormResult : Form
    {
        private FormResult()
        {
            InitializeComponent();
        }

        private static long timestamp = 0;

        private static FormResult frm;

        public static void ShowLog()
        {
            if (frm == null) frm = new FormResult();

            if (!TimeStampIsLangEnough("显示记录")) return;

            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - frm.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - frm.Height;
            frm.SetDesktopLocation(x, y);
            frm.Show();
            ScrollToEnd();
            frm.TopMost = true;
            frm.Activate();
        }

        private delegate void SetTextCallback(string text);

        public static void UpdateLog(string txt)
        {
            if (frm == null) frm = new FormResult();

            // InvokeRequired需要比较调用线程ID和创建线程ID
            // 如果它们不相同则返回true
            if (frm.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(UpdateLog);
                frm.Invoke(d, new object[] { txt });
            }
            else
            {
                frm.textBox1.Text += txt;
                ScrollToEnd();
            }
        }

        private static void ScrollToEnd()
        {
            if (frm == null) frm = new FormResult();

            if (frm.textBox1.Text.Length > 0)
                frm.textBox1.Select(frm.textBox1.Text.Length, 0);
            frm.textBox1.ScrollToCaret();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void FormResult_Deactivate(object sender, EventArgs e)
        {
            if (!TimeStampIsLangEnough("失去焦点"))
            {
                ShowLog();
                return;
            }
            this.Hide();
        }

        /// <summary>
        /// 由于窗体失去焦点时要隐藏日志，
        /// 而单击托盘图标的时候需要显示日志，
        /// 当单击托盘图标时会导致失去焦点，
        /// 这里通过时间差判断一下，只处理其中一个命令。
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        private static bool TimeStampIsLangEnough(string from)
        {
            long timestampthis = DateTime.UtcNow.Ticks;
            long timestampdiff = timestampthis - timestamp;
            //frm.textBox1.Text += from + ":" + timestampdiff.ToString() + "\r\n";
            if (timestampdiff < 2000000) return false;
            timestamp = timestampthis;
            return true;
        }

        private void FormResult_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}
