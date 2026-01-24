using System;
using System.Collections.Generic;

namespace _4RTools.Presenters
{
    public interface IAddServerView
    {
        string HPAddress { get; set; }
        string NameAddress { get; set; }
        string SelectedProcessName { get; set; }
        List<string> AvailableProcesses { set; }
        string WindowTitle { set; }

        event EventHandler SaveClicked;
        event EventHandler LoadRequested;
        
        void ShowMessage(string message, bool isError);
        void CloseView();
    }
}
