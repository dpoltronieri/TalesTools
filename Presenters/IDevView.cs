using System;
using System.Collections.Generic;

namespace _4RTools.Presenters
{
    public interface IDevView
    {
        string BuffId { get; set; }
        string BuffName { get; set; }
        event EventHandler SaveBuffName;
        event EventHandler Reload;
        event EventHandler MonitorTick;

        void UpdateBuffList(Dictionary<int, string> buffs);
        void ShowMessage(string message);
        void ClearBuffList();
    }
}
