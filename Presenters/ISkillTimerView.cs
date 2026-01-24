using System;
using System.Collections.Generic;
using _4RTools.Model;

namespace _4RTools.Presenters
{
    public interface ISkillTimerView
    {
        event EventHandler<SkillTimerEventArgs> KeyChanged;
        event EventHandler<SkillTimerEventArgs> DelayChanged;

        void UpdateControl(int laneId, string controlName, string value);
        void UpdateDelay(int laneId, int value);
    }

    public class SkillTimerEventArgs : EventArgs
    {
        public int LaneId { get; set; }
        public string Key { get; set; }
        public int Delay { get; set; }
    }
}
