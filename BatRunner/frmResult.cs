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

    public partial class frmResult : frmHidWhenDeactivate
    {
        private frmResult()
        {
            InitializeComponent();
        }

        public static void ShowLog()
        {
            if (frm == null) frm = new frmResult();

            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - frm.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - frm.Height;
            frm.SetDesktopLocation(x, y);
            CallShow();
            ScrollToEnd();
            frm.TopMost = true;
            frm.Activate();
        }

        private static frmResult thisFrm { get { return (frmResult)frm; } }

        private delegate void SetTextCallback(string text);

        public static void UpdateLog(string txt)
        {
            if (frm == null) frm = new frmResult();

            // InvokeRequired需要比较调用线程ID和创建线程ID
            // 如果它们不相同则返回true
            if (thisFrm.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(UpdateLog);
                frm.Invoke(d, new object[] { txt });
            }
            else
            {
                thisFrm.textBox1.Text += txt;
                ScrollToEnd();
            }
        }

        private static void ScrollToEnd()
        {
            if (frm == null) frm = new frmResult();

            if (thisFrm.textBox1.Text.Length > 0)
                thisFrm.textBox1.Select(thisFrm.textBox1.Text.Length, 0);
            thisFrm.textBox1.ScrollToCaret();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CallHide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}
