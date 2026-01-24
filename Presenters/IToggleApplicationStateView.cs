using System;

namespace _4RTools.Presenters
{
    public interface IToggleApplicationStateView
    {
        string ToggleKey { get; set; }
        string ToggleHealKey { get; set; }
        string StatusText { set; }
        System.Drawing.Color StatusColor { set; }
        string ToggleButtonText { set; }
        System.Drawing.Color ToggleButtonBackColor { set; }
        
        string HealStatusText { set; }
        System.Drawing.Color HealStatusColor { set; }
        string ToggleHealButtonText { set; }
        System.Drawing.Color ToggleHealButtonBackColor { set; }

        event EventHandler ToggleKeyChanged;
        event EventHandler ToggleHealKeyChanged;
        event EventHandler ToggleStatus;
        event EventHandler ToggleHealStatus;
        event EventHandler ShutdownApplication;

        void SetIcon(System.Drawing.Icon icon);
        void PlaySound(System.IO.Stream sound);
    }
}
