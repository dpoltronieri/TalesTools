using System;

namespace _4RTools.Presenters
{
    public interface IClientUpdaterView
    {
        int ProgressBarValue { get; set; }
        int MaxProgressBarValue { get; set; }
        void ShowMessage(string message);
        void StartContainer();
        void CloseView();
    }
}
