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
        private static readonly ImgyazurIcon imgyazurIcon = new ImgyazurIcon();

        private NotifyIcon notifyIcon;

        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem changeHotkeyMenuItem;
        private ToolStripMenuItem exitMenuItem;

        private ImgyazurIcon()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.BalloonTipText = "imgyazur";
            this.notifyIcon.Text = "imgyazur";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Icon = Properties.Resources.imgur;
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.MouseClick += new MouseEventHandler(notifyIcon_MouseClick);

            this.contextMenuStrip = new ContextMenuStrip();
            this.contextMenuStrip.SuspendLayout();

            this.exitMenuItem = new ToolStripMenuItem();
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new Size(156, 22);
            this.exitMenuItem.Text = "Exit";

            this.changeHotkeyMenuItem = new ToolStripMenuItem();
            this.changeHotkeyMenuItem.Name = "changeHotkeyMenuItem";
            this.changeHotkeyMenuItem.Size = new Size(156, 22);
            this.changeHotkeyMenuItem.Text = "Change Hotkey";

            this.contextMenuStrip.Items.AddRange(new ToolStripItem[] { this.changeHotkeyMenuItem, this.exitMenuItem });
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new Size(157, 70);

            this.contextMenuStrip.ResumeLayout(false);
        }

        public static ImgyazurIcon GetIcon()
        {
            return imgyazurIcon;
        }

        void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            formImgyazur imgyazurForm = new formImgyazur();
            imgyazurForm.Show();
        }
    }
}
