using System;
using System.Linq;
using _4RTools.Model;

namespace _4RTools.Presenters
{
    public class ProfilePresenter
    {
        private IProfileView view;

        public ProfilePresenter(IProfileView view)
        {
            this.view = view;
            BindEvents();
            Initialize();
        }

        private void Initialize()
        {
            foreach (string profile in Profile.ListAll())
            {
                if (profile != "Default") { this.view.AddProfileToList(profile); };
            }
        }

        private void BindEvents()
        {
            this.view.SaveProfile += (s, e) => SaveProfile();
            this.view.RemoveProfile += (s, e) => RemoveProfile();
            this.view.AssignProfile += (s, e) => AssignProfile();
        }

        private void SaveProfile()
        {
            string newProfileName = this.view.ProfileName;
            if (string.IsNullOrEmpty(newProfileName)) { return; }

            ProfileSingleton.Create(newProfileName);
            this.view.AddProfileToList(newProfileName);
            this.view.RefreshParentProfileList();
            this.view.ProfileName = "";
        }

        private void RemoveProfile()
        {
            string selectedProfile = this.view.SelectedProfile;
            if (string.IsNullOrEmpty(selectedProfile))
            {
                this.view.ShowMessage("No profile found! To delete a profile, first select an option from the Profile list.");
                return;
            }

            if (selectedProfile == "Default")
            {
                this.view.ShowMessage("Cannot delete a Default profile!");
            }
            else
            {
                ProfileSingleton.Delete(selectedProfile);
                this.view.RemoveProfileFromList(selectedProfile);
                this.view.RefreshParentProfileList();
            }
        }

        private void AssignProfile()
        {
            Client client = ClientSingleton.GetClient();
            if (client == null)
            {
                this.view.ShowMessage("No client selected. Please select a client first.");
                return;
            }

            string selectedProfile = this.view.SelectedProfile;
            if (string.IsNullOrEmpty(selectedProfile))
            {
                this.view.ShowMessage("No profile selected. Please select a profile from the list.");
                return;
            }

            string characterName = client.ReadCharacterName();
            if (string.IsNullOrEmpty(characterName))
            {
                this.view.ShowMessage("Could not read character name from the client.");
                return;
            }

            CharacterProfileManager.SetProfile(characterName, selectedProfile);
            this.view.ShowMessage($"Profile '{selectedProfile}' assigned to character '{characterName}'.");
        }
    }
}
