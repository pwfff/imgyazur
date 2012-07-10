using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
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

        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public KeyComboSettings keyComboSettings = new KeyComboSettings();

        public NotifyIcon notifyIcon;
        public static string defaultText = "imgyazur";

        formImgyazur imgyazurForm = null;

        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem startOnBootMenuItem;
        private ToolStripMenuItem copyAfterUploadMenuItem;
        private ToolStripMenuItem openAfterUploadMenuItem;
        private ToolStripMenuItem changeHotkeyMenuItem;
        private ToolStripMenuItem exitMenuItem;

        private ImgyazurIcon()
        {
            contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.SuspendLayout();

            exitMenuItem = new ToolStripMenuItem();
            exitMenuItem.Name = "exitMenuItem";
            exitMenuItem.Text = "Exit";
            exitMenuItem.Click += new EventHandler(exitMenuItem_Click);

            startOnBootMenuItem = new ToolStripMenuItem();
            startOnBootMenuItem.Name = "startOnBootMenuItem";
            startOnBootMenuItem.Text = "Start On Boot";
            startOnBootMenuItem.CheckOnClick = true;
            if (rkApp.GetValue("imgyazur") == null)
                startOnBootMenuItem.Checked = false;
            else
                startOnBootMenuItem.Checked = true;
            startOnBootMenuItem.CheckedChanged += new EventHandler(startOnBootMenuItem_CheckedChanged);

            copyAfterUploadMenuItem = new ToolStripMenuItem();
            copyAfterUploadMenuItem.Name = "copyAfterUploadMenuItem";
            copyAfterUploadMenuItem.Text = "Copy URL After Upload";
            copyAfterUploadMenuItem.CheckOnClick = true;
            copyAfterUploadMenuItem.Checked = Properties.Settings.Default.copyToClipboard;
            copyAfterUploadMenuItem.CheckedChanged += new EventHandler(copyAfterUploadMenuItem_CheckedChanged);

            openAfterUploadMenuItem = new ToolStripMenuItem();
            openAfterUploadMenuItem.Name = "openAfterUploadMenuItem";
            openAfterUploadMenuItem.Text = "Open URL After Upload";
            openAfterUploadMenuItem.CheckOnClick = true;
            openAfterUploadMenuItem.Checked = Properties.Settings.Default.openAfterUpload;
            openAfterUploadMenuItem.CheckedChanged += new EventHandler(openAfterUploadMenuItem_CheckedChanged);

            changeHotkeyMenuItem = new ToolStripMenuItem();
            changeHotkeyMenuItem.Name = "changeHotkeyMenuItem";
            changeHotkeyMenuItem.Text = "Change Hotkey";
            changeHotkeyMenuItem.Click += new EventHandler(changeHotkeyMenuItem_Click);

            contextMenuStrip.Items.AddRange(new ToolStripItem[] { startOnBootMenuItem, copyAfterUploadMenuItem, openAfterUploadMenuItem, changeHotkeyMenuItem, exitMenuItem });
            contextMenuStrip.Name = "contextMenuStrip";

            contextMenuStrip.ResumeLayout(false);

            notifyIcon = new NotifyIcon();
            notifyIcon.BalloonTipText = "imgyazur";
            notifyIcon.Text = defaultText;
            notifyIcon.Visible = true;
            notifyIcon.Icon = Properties.Resources.imgur;
            notifyIcon.ContextMenuStrip = contextMenuStrip;
            notifyIcon.MouseClick += new MouseEventHandler(notifyIcon_MouseClick);
            notifyIcon.ShowBalloonTip(10000, "imgyazur is now running", "Press " + keyComboSettings.ToKeyCombo().ToString() + " or click this icon to start capturing.", ToolTipIcon.Info);

            Hook KeyboardHook = new Hook("imgyazur keyboard hook");
            KeyboardHook.KeyDownEvent += GlobalKeyDown;
        }

        void openAfterUploadMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.openAfterUpload = openAfterUploadMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        void copyAfterUploadMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.copyToClipboard = copyAfterUploadMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        void startOnBootMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (startOnBootMenuItem.Checked)
                rkApp.SetValue("imgyazur", Application.ExecutablePath.ToString());
            else
                rkApp.DeleteValue("imgyazur");
        }

        void changeHotkeyMenuItem_Click(object sender, EventArgs e)
        {
            new formHotkey().ShowDialog();
        }

        void startCapture()
        {
            if (imgyazurForm != null && !imgyazurForm.IsDisposed)
                return;

            imgyazurForm = new formImgyazur();
            imgyazurForm.Show();
        }

        void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                startCapture();
        }

        void exitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void GlobalKeyDown(KeyboardHookEventArgs e)
        {
            KeyCombo pressedKeys = KeyCombo.FromKeyboardHookEventArgs(e);
            if (pressedKeys.Equals(keyComboSettings.ToKeyCombo()))
                startCapture();
        }
    }
}
