using System;
using System.Collections.Generic;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Presenters
{
    public interface IConfigView
    {
        bool StopBuffsCity { get; set; }
        bool StopBuffsRein { get; set; }
        bool StopHealCity { get; set; }
        bool GetOffRein { get; set; }
        string ReinKey { get; set; }
        bool SwitchAmmo { get; set; }
        string Ammo1Key { get; set; }
        string Ammo2Key { get; set; }
        bool StopWithChat { get; set; }

        event EventHandler PreferenceChanged;
        event EventHandler ReinKeyChanged;
        event EventHandler Ammo1KeyChanged;
        event EventHandler Ammo2KeyChanged;
        event EventHandler<VisibilityChangedEventArgs> VisibilityChanged;
        event EventHandler<OrderChangedEventArgs> OrderChanged;

        void ClearTabConfig();
        void AddTabVisibilityControl(string text, string name, bool isChecked, bool isSecondary);
        void AddAutobuffSkillVisibilityControl(string text, string name, bool isChecked);
        void AddAutobuffStuffVisibilityControl(string text, string name, bool isChecked);
        
        void SetSkillsList(List<string> skillNames);
        void SetSwitchList(List<string> switchNames);
    }

    public class VisibilityChangedEventArgs : EventArgs
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public string Type { get; set; } // Tab, SkillGroup, StuffGroup
    }

    public class OrderChangedEventArgs : EventArgs
    {
        public List<string> OrderedNames { get; set; }
        public string Type { get; set; } // Skills, Switch
    }
}
