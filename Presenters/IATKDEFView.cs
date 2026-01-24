using System;
using _4RTools.Model;

namespace _4RTools.Presenters
{
    public interface IATKDEFView
    {
        event EventHandler<ATKDEFEventArgs> ChangeRequested;
        void UpdateControlValue(int laneId, string controlType, string value);
        void UpdateEquipValue(int laneId, string type, int index, string value);
    }

    public class ATKDEFEventArgs : EventArgs
    {
        public int LaneId { get; set; }
        public string ControlType { get; set; } // spammerKey, spammerDelay, switchDelay, spammerClick, ATK, DEF
        public int Index { get; set; } // for ATK/DEF equips
        public string Value { get; set; }
        public string ControlName { get; set; } // for dictionary keys in Model
    }
}
