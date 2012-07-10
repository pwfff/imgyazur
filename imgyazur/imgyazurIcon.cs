using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FMUtils.KeyboardHook;

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

        public KeyComboSettings keyComboSettings = new KeyComboSettings();
        public KeyCombo keyCombo;

        public NotifyIcon notifyIcon;
        public static string defaultText = "imgyazur";

        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem captureImageMenuItem;
        private ToolStripMenuItem exitMenuItem;

        private ImgyazurIcon()
        {
            contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.SuspendLayout();

            exitMenuItem = new ToolStripMenuItem();
            exitMenuItem.Name = "exitMenuItem";
            exitMenuItem.Text = "Exit";
            exitMenuItem.Click += new EventHandler(exitMenuItem_Click);

            captureImageMenuItem = new ToolStripMenuItem();
            captureImageMenuItem.Name = "captureImageMenuItem";
            captureImageMenuItem.Text = "Capture Image";
            captureImageMenuItem.Click += new EventHandler(captureImageMenuItem_Click);

            contextMenuStrip.Items.AddRange(new ToolStripItem[] { captureImageMenuItem, exitMenuItem });
            contextMenuStrip.Name = "contextMenuStrip";

            contextMenuStrip.ResumeLayout(false);

            notifyIcon = new NotifyIcon();
            notifyIcon.BalloonTipText = "imgyazur";
            notifyIcon.Text = defaultText;
            notifyIcon.Visible = true;
            notifyIcon.Icon = Properties.Resources.imgur;
            notifyIcon.ContextMenuStrip = contextMenuStrip;
            notifyIcon.MouseClick += new MouseEventHandler(notifyIcon_MouseClick);

            keyCombo = keyComboSettings.ToKeyCombo();

            Hook KeyboardHook = new Hook("imgyazur keyboard hook");
            KeyboardHook.KeyDownEvent += GlobalKeyDown;
        }

        void startCapture()
        {
            formImgyazur imgyazurForm = new formImgyazur();
            imgyazurForm.Show();
        }

        void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                startCapture();
        }

        void captureImageMenuItem_Click(object sender, EventArgs e)
        {
            startCapture();
        }

        void exitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void GlobalKeyDown(KeyboardHookEventArgs e)
        {
            KeyCombo pressedKeys = KeyCombo.FromKeyboardHookEventArgs(e);
            if (keyCombo.Equals(pressedKeys))
                startCapture();
        }
    }
}
