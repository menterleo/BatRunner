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
    public partial class FormMain : Form
    {
        string filepath = System.AppDomain.CurrentDomain.BaseDirectory + "commands";
        ExcuteCmd excmd = new ExcuteCmd();
        UpdateIcon updateicon = new UpdateIcon();

        public FormMain()
        {
            InitializeComponent();
            RefeshMenu();
            this.notifyIcon1.Icon = BatRunner.Properties.Resources.PerfCenterCpl;
            excmd.OnStartEvent += excmd_OnStartEvent;
            excmd.OnEndEvent += excmd_OnEndEvent;
            excmd.OnMessageEvent += excmd_OnMessageEvent;
            updateicon.OnWaiteImg += updateicon_OnWaiteImg;
        }

        void excmd_OnMessageEvent(string retStr)
        {
            frmResult.UpdateLog(retStr + "\r\n");
        }

        void updateicon_OnWaiteImg(Icon icon)
        {
            this.notifyIcon1.Icon = icon;
        }

        void excmd_OnEndEvent()
        {
            updateicon.EndWaite();
        }

        void excmd_OnStartEvent()
        {
            contextMenuStrip1.Hide();
            updateicon.StartWaite();
        }

        private void RefeshMenu()
        {
            contextMenuStrip1.Items.Clear();

            ToolStripMenuItem items = LoadMenus(new DirectoryInfo(filepath));

            for (int i = items.DropDownItems.Count - 1; i >= 0; i--)
            {
                contextMenuStrip1.Items.Insert(0, items.DropDownItems[i]);
            }

            System.Windows.Forms.ToolStripSeparator tls = new ToolStripSeparator();
            contextMenuStrip1.Items.Add(tls);

            System.Windows.Forms.ToolStripMenuItem itemEdit = new ToolStripMenuItem();
            itemEdit.Text = "Edit";
            contextMenuStrip1.Items.Add(itemEdit);
            itemEdit.Click += item_Click;

            System.Windows.Forms.ToolStripMenuItem itemrefsh = new ToolStripMenuItem();
            itemrefsh.Text = "Refsh";
            contextMenuStrip1.Items.Add(itemrefsh);
            itemrefsh.Click += item_Click;

            System.Windows.Forms.ToolStripMenuItem itemexit = new ToolStripMenuItem();
            itemexit.Text = "Exit";
            contextMenuStrip1.Items.Add(itemexit);
            itemexit.Click += item_Click;
        }

        private System.Windows.Forms.ToolStripMenuItem LoadMenus(DirectoryInfo dirRoot)
        {
            System.Windows.Forms.ToolStripMenuItem itemThis = new ToolStripMenuItem();
  
            string itemtext = dirRoot.Name;
            string[] itemtexts = dirRoot.Name.Split('_');
            if (itemtexts.Length > 1)
            {
                itemtext = dirRoot.Name.Replace(itemtexts[0] + "_", "");
            }
            itemThis.Text = itemtext;

            FileSystemInfo [] fsis = dirRoot.GetFileSystemInfos();

            foreach (FileSystemInfo fsi in fsis)
            {
                if (fsi is FileInfo)
                {
                    FileInfo fi = fsi as FileInfo;
                    if (fi.Extension.ToLower() != ".bat") continue;
                    System.Windows.Forms.ToolStripMenuItem item = new ToolStripMenuItem();

                    itemtext = fi.Name.Replace(fi.Extension, "");
                    itemtexts = itemtext.Split('_');
                    if (itemtexts.Length > 1)
                    {
                        itemtext = itemtext.Replace(itemtexts[0] + "_", "");
                    }

                    item.Text = itemtext;
                    item.Tag = fi.FullName;
                    itemThis.DropDownItems.Add(item);
                    item.Click += item_Click;
                }
                else if (fsi is DirectoryInfo)
                {
                    itemThis.DropDownItems.Add(LoadMenus(fsi as DirectoryInfo));
                }
            }
            return itemThis;
        }

        void item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item.Text == "Exit")
            {
                Application.ExitThread();
            }
            else if (item.Text == "Refsh")
            {
                RefeshMenu();
                this.Refresh();
                this.Update();
            }
            else if (item.Text == "Edit")
            {
                System.Diagnostics.Process.Start("explorer.exe", filepath);
            }
            else if (item.Tag != null)
            {
                frmResult.ShowLog();
                excmd.Execute(item.Text, item.Tag.ToString());
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                frmResult.ShowLog();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
