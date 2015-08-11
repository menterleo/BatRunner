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
    /// <summary>
    /// 焦点离开时隐藏窗体
    /// </summary>
    public partial class frmHidWhenDeactivate : Form
    {
        protected frmHidWhenDeactivate()
        {
            InitializeComponent();
        }

        private static long timestamp = 0;

        protected static frmHidWhenDeactivate frm;

        private void FormResult_Deactivate(object sender, EventArgs e)
        {
            if (!TimeStampIsLangEnough("失去焦点"))
            {
                CallShow();
                return;
            }
            CallHide();
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
                CallHide();
            }
        }

        protected static void CallHide()
        {
            frm.Hide();
        }

        protected static void CallShow()
        {
            if (!TimeStampIsLangEnough("显示记录")) return;
            frm.Show();
        }
    }
}
