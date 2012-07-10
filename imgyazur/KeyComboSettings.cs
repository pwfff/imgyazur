using System;
using System.Windows.Forms;
using System.Configuration;
using FMUtils.KeyboardHook;

namespace imgyazur
{
    public class KeyComboSettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [DefaultSettingValue("D")]
        public Keys Key
        {
            get
            {
                return ((Keys)this["Key"]);
            }
            set
            {
                this["Key"] = (Keys)value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("True")]
        public bool isAltPressed
        {
            get
            {
                return ((bool)this["isAltPressed"]);
            }
            set
            {
                this["isAltPressed"] = (bool)value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("True")]
        public bool isCtrlPressed
        {
            get
            {
                return ((bool)this["isCtrlPressed"]);
            }
            set
            {
                this["isCtrlPressed"] = (bool)value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("True")]
        public bool isShiftPressed
        {
            get
            {
                return ((bool)this["isShiftPressed"]);
            }
            set
            {
                this["isShiftPressed"] = (bool)value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("False")]
        public bool isWinPressed
        {
            get
            {
                return ((bool)this["isWinPressed"]);
            }
            set
            {
                this["isWinPressed"] = (bool)value;
            }
        }

        public void FromKeyCombo(KeyCombo keyCombo)
        {
            this["Key"] = keyCombo.Key;
            this["isCtrlPressed"] = keyCombo.isCtrlPressed;
            this["isAltPressed"] = keyCombo.isAltPressed;
            this["isShiftPressed"] = keyCombo.isShiftPressed;
            this["isWinPressed"] = keyCombo.isWinPressed;
        }

        public KeyCombo ToKeyCombo()
        {
            return new KeyCombo((Keys)this["Key"], (bool)this["isAltPressed"], (bool)this["isCtrlPressed"], (bool)this["isShiftPressed"], (bool)this["isWinPressed"]);
        }
    }
}
