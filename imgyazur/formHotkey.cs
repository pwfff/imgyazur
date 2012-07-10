using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FMUtils.KeyboardHook;

namespace imgyazur
{
    public partial class formHotkey : Form
    {
        KeyCombo CurrentKeyCombo;
        Hook ShortcutHook;

        public formHotkey()
        {
            InitializeComponent();

            txtCombo.Text = ImgyazurIcon.Instance.keyComboSettings.ToKeyCombo().ToString();

            this.ShortcutHook = new Hook("Shortcut Capture Hook");
            this.ShortcutHook.KeyDownEvent += e =>
            {
                if (txtCombo.Focused)
                {
                    CurrentKeyCombo = KeyCombo.FromKeyboardHookEventArgs(e);
                    UpdateShortcutDisplay();
                }
            };
        }

        private void UpdateShortcutDisplay()
        {
            if (txtCombo.InvokeRequired)
                this.BeginInvoke(new MethodInvoker(() => UpdateShortcutDisplay()));

            txtCombo.Text = (CurrentKeyCombo ?? ImgyazurIcon.Instance.keyComboSettings.ToKeyCombo()).ToString();
        }

        private void formHotkey_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                ImgyazurIcon.Instance.keyComboSettings.FromKeyCombo(CurrentKeyCombo);
                ImgyazurIcon.Instance.keyComboSettings.Save();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
