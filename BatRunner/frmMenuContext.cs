using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServiceManager
{
    public partial class frmMenuContext : frmHidWhenDeactivate
    {
        public frmMenuContext()
        {
            InitializeComponent();
        }

        string filepath = System.AppDomain.CurrentDomain.BaseDirectory + "commands";

        private void RefeshMenu()
        {
            flowLayoutPanel1.Controls.Clear();

            if (!Directory.Exists(filepath)) Directory.CreateDirectory(filepath);

            string[] s1 = Directory.GetFiles(filepath);

            foreach (string str in s1)
            {
                FileInfo fi = new FileInfo(str);
                if (fi.Extension.ToLower() != ".bat") continue;
                MenuItem item = new MenuItem();
                item.Text = fi.Name.Replace(fi.Extension, "");
                item.Tag = fi.FullName;
                contextMenuStrip1.Items.Add(item);
            }

            System.Windows.Forms.ToolStripSeparator tls = new ToolStripSeparator();
            contextMenuStrip1.Items.Add(tls);

            System.Windows.Forms.ToolStripMenuItem itemEdit = new ToolStripMenuItem();
            itemEdit.Text = "Edit";
            contextMenuStrip1.Items.Add(itemEdit);

            System.Windows.Forms.ToolStripMenuItem itemrefsh = new ToolStripMenuItem();
            itemrefsh.Text = "Refsh";
            contextMenuStrip1.Items.Add(itemrefsh);

            System.Windows.Forms.ToolStripMenuItem itemexit = new ToolStripMenuItem();
            itemexit.Text = "Exit";
            contextMenuStrip1.Items.Add(itemexit);
        }

        private class MenuItem : Panel
        {
            Label lbltext = new Label();

            protected MenuItem()
            {
                this.Controls.Add(lbltext);
                this.ParentChanged += MenuItem_ParentChanged;
            }

            void MenuItem_ParentChanged(object sender, EventArgs e)
            {
                this.Width = this.Parent.Width;
            }

            string Text
            {
                get { return lbltext.Text; }
                set { lbltext.Text = value; }
            }
        }
    }
}
