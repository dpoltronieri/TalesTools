using System;
using System.Collections.Generic;

namespace _4RTools.Presenters
{
    public interface IProfileView
    {
        string ProfileName { get; set; }
        string SelectedProfile { get; }
        
        event EventHandler SaveProfile;
        event EventHandler RemoveProfile;
        event EventHandler AssignProfile;

        void AddProfileToList(string profileName);
        void RemoveProfileFromList(string profileName);
        void ShowMessage(string message);
        void RefreshParentProfileList();
    }
}
