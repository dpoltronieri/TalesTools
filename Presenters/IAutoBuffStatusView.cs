using System;
using _4RTools.Utils;

namespace _4RTools.Presenters
{
    public interface IAutoBuffStatusView
    {
        string StatusKey { get; set; }
        string NewStatusKey { get; set; }

        event EventHandler StatusKeyChanged;
        event EventHandler NewStatusKeyChanged;
        event EventHandler<AutoBuffStatusKeyEventArgs> DebuffKeyChanged;

        void SetDebuffKey(EffectStatusIDs id, string key);
    }

    public class AutoBuffStatusKeyEventArgs : EventArgs
    {
        public EffectStatusIDs Id { get; set; }
        public string Key { get; set; }
    }
}
