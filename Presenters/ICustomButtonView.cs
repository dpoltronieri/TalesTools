using System;

namespace _4RTools.Presenters
{
    public interface ICustomButtonView
    {
        string TransferKey { get; set; }
        string PriorityKey { get; set; }
        string PriorityDelay { get; set; }

        event EventHandler TransferKeyChanged;
        event EventHandler PriorityKeyChanged;
        event EventHandler PriorityDelayChanged;
    }
}
