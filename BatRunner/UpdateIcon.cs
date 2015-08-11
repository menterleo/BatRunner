using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BatRunner
{
    public class UpdateIcon
    {

        public delegate void SendWaiteImg(System.Drawing.Icon icon);
        public event SendWaiteImg OnWaiteImg;

        private System.Threading.Thread thread;
        private bool IsSetEnd = false;
        private int waitetime = 100;

        private void SetWaiteIcon()
        {
            while (!IsSetEnd)
            {
                System.Threading.Thread.Sleep(waitetime);
                OnWaiteImg(BatRunner.Properties.Resources.w1);
                if (IsSetEnd) break;
                System.Threading.Thread.Sleep(waitetime);
                OnWaiteImg(BatRunner.Properties.Resources.w2);
                if (IsSetEnd) break;
                System.Threading.Thread.Sleep(waitetime);
                OnWaiteImg(BatRunner.Properties.Resources.w3);
                if (IsSetEnd) break;
                System.Threading.Thread.Sleep(waitetime);
                OnWaiteImg(BatRunner.Properties.Resources.w4);
                if (IsSetEnd) break;
                System.Threading.Thread.Sleep(waitetime);
                OnWaiteImg(BatRunner.Properties.Resources.w5);
                if (IsSetEnd) break;
                System.Threading.Thread.Sleep(waitetime);
                OnWaiteImg(BatRunner.Properties.Resources.w6);
                if (IsSetEnd) break;
                System.Threading.Thread.Sleep(waitetime);
                OnWaiteImg(BatRunner.Properties.Resources.w7);
                if (IsSetEnd) break;
                System.Threading.Thread.Sleep(waitetime);
                OnWaiteImg(BatRunner.Properties.Resources.w8);
            }
            OnWaiteImg(BatRunner.Properties.Resources.PerfCenterCpl);
        }

        public void StartWaite()
        {
            IsSetEnd = false;
            thread = new System.Threading.Thread(new System.Threading.ThreadStart(SetWaiteIcon));
            thread.Start();
        }

        public void EndWaite()
        {
            IsSetEnd = true;
        }
    }
}
