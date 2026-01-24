using System;
using System.Collections.Generic;
using _4RTools.Model;

namespace _4RTools.Presenters
{
    public interface IAHKView
    {
        string AHKDelay { get; set; }
        bool NoShift { get; set; }
        bool MouseFlick { get; set; }
        string AHKMode { get; set; }

        event EventHandler AHKDelayChanged;
        event EventHandler NoShiftChanged;
        event EventHandler MouseFlickChanged;
        event EventHandler AHKModeChanged;
        
        // Handling dynamic checkboxes for AHK entries
        event EventHandler<KeyConfigEventArgs> CheckboxChanged;
    }

    public class KeyConfigEventArgs : EventArgs
    {
        public string Name { get; set; }
        public bool Checked { get; set; }
        public string Key { get; set; } // The key associated with the checkbox
    }
}
