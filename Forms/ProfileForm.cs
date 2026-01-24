using System;
using _4RTools.Model;
using System.Windows.Forms;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class ProfileForm: Form, IProfileView
    {
        private Container container;
        private ProfilePresenter presenter;

        public ProfileForm(Container container)
        {
            InitializeComponent();
            this.container = container;
            this.presenter = new ProfilePresenter(this);

            this.btnSave.Click += (s, e) => SaveProfile?.Invoke(this, EventArgs.Empty);
            this.btnRemoveProfile.Click += (s, e) => RemoveProfile?.Invoke(this, EventArgs.Empty);
            this.btnAssignProfile.Click += (s, e) => AssignProfile?.Invoke(this, EventArgs.Empty);
        }

        // IProfileView Implementation
        public string ProfileName { get => txtProfileName.Text; set => txtProfileName.Text = value; }
        public string SelectedProfile => lbProfilesList.SelectedItem?.ToString();

        public event EventHandler SaveProfile;
        public event EventHandler RemoveProfile;
        public event EventHandler AssignProfile;

        public void AddProfileToList(string profileName)
        {
            this.lbProfilesList.Items.Add(profileName);
        }

        public void RemoveProfileFromList(string profileName)
        {
            this.lbProfilesList.Items.Remove(profileName);
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void RefreshParentProfileList()
        {
            this.container.refreshProfileList();
        }
    }
}
