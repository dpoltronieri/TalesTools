using System;
using _4RTools.Model;

namespace _4RTools.Presenters
{
    public interface IAutoSwitchHealView
    {
        event EventHandler<AutoSwitchHealEventArgs> PropertyChanged;
        void SetControlValue(string propertyName, string value);
    }

    public class AutoSwitchHealEventArgs : EventArgs
    {
        public string PropertyName { get; set; }
        public string Value { get; set; }
    }
}
