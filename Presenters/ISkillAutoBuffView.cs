using System;
using System.Collections.Generic;
using _4RTools.Utils;

namespace _4RTools.Presenters
{
    public interface ISkillAutoBuffView
    {
        int Delay { get; set; }
        event EventHandler DelayChanged;
        event EventHandler ResetRequested;
        
        void UpdateUI(Dictionary<EffectStatusIDs, System.Windows.Input.Key> buffMapping);
    }
}
