using System;
using _4RTools.Model;

namespace _4RTools.Presenters
{
    public interface IMacroSongView
    {
        event EventHandler<MacroEventArgs> MacroChanged;
        event EventHandler<MacroEventArgs> DelayChanged;
        event EventHandler<MacroEventArgs> ResetRequested;

        void UpdateControl(int laneId, string controlName, string value);
        void UpdateDelay(int laneId, int value);
        void ResetLane(int laneId);
    }

    public class MacroEventArgs : EventArgs
    {
        public int LaneId { get; set; }
        public string ControlName { get; set; }
        public string Text { get; set; }
        public string Tag { get; set; }
        public int Delay { get; set; }
    }
}
