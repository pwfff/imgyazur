using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace imgyazur
{
    class ImgyazurIcon
    {
        private static ImgyazurIcon instance = null;

        public static ImgyazurIcon Instance
        {
            get
            {
                if (ImgyazurIcon.instance == null)
                    ImgyazurIcon.instance = new ImgyazurIcon();
                return ImgyazurIcon.instance;
            }
        }


        public NotifyIcon notifyIcon;
        public static string defaultText = "imgyazur";

        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem captureImageMenuItem;
        private ToolStripMenuItem exitMenuItem;

        private ImgyazurIcon()
        {
            this.contextMenuStrip = new ContextMenuStrip();
            this.contextMenuStrip.SuspendLayout();

            this.exitMenuItem = new ToolStripMenuItem();
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new EventHandler(exitMenuItem_Click);

            this.captureImageMenuItem = new ToolStripMenuItem();
            this.captureImageMenuItem.Name = "captureImageMenuItem";
            this.captureImageMenuItem.Text = "Capture Image";
            this.captureImageMenuItem.Click += new EventHandler(captureImageMenuItem_Click);

            this.contextMenuStrip.Items.AddRange(new ToolStripItem[] { this.captureImageMenuItem, this.exitMenuItem });
            this.contextMenuStrip.Name = "contextMenuStrip";

            this.contextMenuStrip.ResumeLayout(false);

            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.BalloonTipText = "imgyazur";
            this.notifyIcon.Text = defaultText;
            this.notifyIcon.Visible = true;
            this.notifyIcon.Icon = Properties.Resources.imgur;
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.MouseClick += new MouseEventHandler(notifyIcon_MouseClick);
        }

        void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                formImgyazur imgyazurForm = new formImgyazur();
                imgyazurForm.Show();
            }
        }

        void captureImageMenuItem_Click(object sender, EventArgs e)
        {
            formImgyazur imgyazurForm = new formImgyazur();
            imgyazurForm.Show();
        }

        void exitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
