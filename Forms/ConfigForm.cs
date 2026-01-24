using System;
using _4RTools.Model;
using System.Windows.Forms;
using _4RTools.Utils;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class ConfigForm : Form, IObserver, IConfigView
    {
        private ConfigPresenter presenter;
        private UserPreferences preferences;
        private AutoBuffSkill autobuffSkill;
        private AutoSwitch autoSwitch;

        public ConfigForm(Subject subject)
        {
            InitializeComponent();
            this.preferences = ProfileSingleton.GetCurrent().UserPreferences;
            this.autobuffSkill = ProfileSingleton.GetCurrent().AutobuffSkill;
            this.autoSwitch = ProfileSingleton.GetCurrent().AutoSwitch;
            this.presenter = new ConfigPresenter(this, this.preferences, this.autobuffSkill, this.autoSwitch);

            WireUpInputHandlers();
            subject.Attach(this);
        }

        private void WireUpInputHandlers()
        {
            this.textReinKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.textReinKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.textReinKey.TextChanged += (s, e) => ReinKeyChanged?.Invoke(this, EventArgs.Empty);

            this.ammo1textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.ammo1textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.ammo1textBox.TextChanged += (s, e) => Ammo1KeyChanged?.Invoke(this, EventArgs.Empty);

            this.ammo2textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.ammo2textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.ammo2textBox.TextChanged += (s, e) => Ammo2KeyChanged?.Invoke(this, EventArgs.Empty);

            this.chkStopBuffsOnCity.CheckedChanged += (s, e) => PreferenceChanged?.Invoke(this, EventArgs.Empty);
            this.chkStopBuffsOnRein.CheckedChanged += (s, e) => PreferenceChanged?.Invoke(this, EventArgs.Empty);
            this.chkStopHealOnCity.CheckedChanged += (s, e) => PreferenceChanged?.Invoke(this, EventArgs.Empty);
            this.getOffReinCheckBox.CheckedChanged += (s, e) => PreferenceChanged?.Invoke(this, EventArgs.Empty);
            this.switchAmmoCheckBox.CheckedChanged += (s, e) => PreferenceChanged?.Invoke(this, EventArgs.Empty);
            this.chkStopWithChat.CheckedChanged += (s, e) => PreferenceChanged?.Invoke(this, EventArgs.Empty);

            // Drag-drop setup for listboxes
            SetupDragDrop(this.skillsListBox, "Skills");
            SetupDragDrop(this.switchListBox, "Switch");
        }

        private void SetupDragDrop(ListBox listBox, string type)
        {
            listBox.MouseDown += (s, e) => {
                if (listBox.SelectedItem == null) return;
                listBox.DoDragDrop(listBox.SelectedItem, DragDropEffects.Move);
            };
            listBox.DragOver += (s, e) => e.Effect = DragDropEffects.Move;
            listBox.DragDrop += (s, e) => {
                Point point = listBox.PointToClient(new Point(e.X, e.Y));
                int index = listBox.IndexFromPoint(point);
                if (index < 0) index = listBox.Items.Count - 1;
                object data = listBox.SelectedItem;
                listBox.Items.Remove(data);
                listBox.Items.Insert(index, data);
            };
            listBox.MouseLeave += (s, e) => {
                OrderChanged?.Invoke(this, new OrderChangedEventArgs { 
                    OrderedNames = listBox.Items.Cast<object>().Select(o => o.ToString()).ToList(),
                    Type = type
                });
            };
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.preferences = ProfileSingleton.GetCurrent().UserPreferences;
                    this.autobuffSkill = ProfileSingleton.GetCurrent().AutobuffSkill;
                    this.autoSwitch = ProfileSingleton.GetCurrent().AutoSwitch;
                    this.presenter.UpdateModels(this.preferences, this.autobuffSkill, this.autoSwitch);
                    break;
                case MessageCode.ADDED_NEW_AUTOBUFF_SKILL:
                case MessageCode.ADDED_NEW_AUTOSWITCH_PETS:
                    this.presenter.UpdateView();
                    break;
            }
        }

        // IConfigView Implementation
        public bool StopBuffsCity { get => chkStopBuffsOnCity.Checked; set => chkStopBuffsOnCity.Checked = value; }
        public bool StopBuffsRein { get => chkStopBuffsOnRein.Checked; set => chkStopBuffsOnRein.Checked = value; }
        public bool StopHealCity { get => chkStopHealOnCity.Checked; set => chkStopHealOnCity.Checked = value; }
        public bool GetOffRein { get => getOffReinCheckBox.Checked; set => getOffReinCheckBox.Checked = value; }
        public string ReinKey { get => textReinKey.Text; set => textReinKey.Text = value; }
        public bool SwitchAmmo { get => switchAmmoCheckBox.Checked; set => switchAmmoCheckBox.Checked = value; }
        public string Ammo1Key { get => ammo1textBox.Text; set => ammo1textBox.Text = value; }
        public string Ammo2Key { get => ammo2textBox.Text; set => ammo2textBox.Text = value; }
        public bool StopWithChat { get => chkStopWithChat.Checked; set => chkStopWithChat.Checked = value; }

        public event EventHandler PreferenceChanged;
        public event EventHandler ReinKeyChanged;
        public event EventHandler Ammo1KeyChanged;
        public event EventHandler Ammo2KeyChanged;
        public event EventHandler<VisibilityChangedEventArgs> VisibilityChanged;
        public event EventHandler<OrderChangedEventArgs> OrderChanged;

        public void ClearTabConfig()
        {
            this.gbTabs.Controls.Clear();
            this.gbAutobuffSkills.Controls.Clear();
            this.gbAutobuffStuffs.Controls.Clear();
        }

        public void AddTabVisibilityControl(string text, string name, bool isChecked, bool isSecondary)
        {
            int count = this.gbTabs.Controls.Count;
            int x = isSecondary ? 150 : 10;
            int y = 20 + (count % 8) * 25; // Simple layout logic
            CheckBox chk = new CheckBox { Text = text, Name = name, Location = new Point(x, y), Checked = isChecked, AutoSize = true };
            chk.CheckedChanged += (s, e) => VisibilityChanged?.Invoke(this, new VisibilityChangedEventArgs { Name = name, IsChecked = chk.Checked, Type = "Tab" });
            this.gbTabs.Controls.Add(chk);
        }

        public void AddAutobuffSkillVisibilityControl(string text, string name, bool isChecked)
        {
            int count = this.gbAutobuffSkills.Controls.Count;
            int x = 10 + (count / 3) * 90;
            int y = 20 + (count % 3) * 25;
            CheckBox chk = new CheckBox { Text = text, Name = name, Location = new Point(x, y), Checked = isChecked, Width = 80 };
            chk.CheckedChanged += (s, e) => VisibilityChanged?.Invoke(this, new VisibilityChangedEventArgs { Name = name, IsChecked = chk.Checked, Type = "SkillGroup" });
            this.gbAutobuffSkills.Controls.Add(chk);
        }

        public void AddAutobuffStuffVisibilityControl(string text, string name, bool isChecked)
        {
            int count = this.gbAutobuffStuffs.Controls.Count;
            int x = 10 + (count / 2) * 90;
            int y = 20 + (count % 2) * 25;
            CheckBox chk = new CheckBox { Text = text, Name = name, Location = new Point(x, y), Checked = isChecked, Width = 80 };
            chk.CheckedChanged += (s, e) => VisibilityChanged?.Invoke(this, new VisibilityChangedEventArgs { Name = name, IsChecked = chk.Checked, Type = "StuffGroup" });
            this.gbAutobuffStuffs.Controls.Add(chk);
        }

        public void SetSkillsList(List<string> skillNames)
        {
            this.skillsListBox.Items.Clear();
            foreach (var name in skillNames) this.skillsListBox.Items.Add(name);
        }

        public void SetSwitchList(List<string> switchNames)
        {
            this.switchListBox.Items.Clear();
            foreach (var name in switchNames) this.switchListBox.Items.Add(name);
        }
    }
}