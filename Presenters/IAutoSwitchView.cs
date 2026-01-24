using System;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Presenters
{
    public interface IAutoSwitchView
    {
        string Delay { get; set; }
        string SwitchDelay { get; set; }

        event EventHandler DelayChanged;
        event EventHandler SwitchDelayChanged;
        event EventHandler<AutoSwitchKeyEventArgs> KeyChanged;
        event EventHandler<EffectStatusIDs> AddNewSkill;

        void SetKey(EffectStatusIDs id, string type, string key);
    }

    public class AutoSwitchKeyEventArgs : EventArgs
    {
        public EffectStatusIDs SkillId { get; set; }
        public string Type { get; set; }
        public string Key { get; set; }
    }
}
