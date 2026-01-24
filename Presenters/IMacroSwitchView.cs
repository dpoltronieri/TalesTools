using System;
using _4RTools.Model;

namespace _4RTools.Presenters
{
    public interface IMacroSwitchView
    {
        event EventHandler<MacroSwitchEventArgs> MacroChanged;
        event EventHandler<MacroSwitchEventArgs> DelayChanged;
        event EventHandler<MacroSwitchEventArgs> ClickChanged;

        void UpdateControl(int laneId, string controlName, string value);
        void UpdateDelay(int laneId, string controlName, int value);
        void UpdateClick(int laneId, string controlName, bool value);
    }

    public class MacroSwitchEventArgs : EventArgs
    {
        public int LaneId { get; set; }
        public string ControlName { get; set; }
        public string Text { get; set; } // For Key text
        public int Delay { get; set; }
        public bool Checked { get; set; }
    }
}
