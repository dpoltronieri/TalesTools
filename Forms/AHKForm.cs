using System;
using System.Collections.Generic;
using System.Windows.Forms;
using _4RTools.Utils;
using _4RTools.Model;
using System.Windows.Input;
using System.Web;
using System.Diagnostics.Tracing;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class AHKForm : Form, IObserver, IAHKView
    {
        private AHK ahk;
        private AHKPresenter presenter;

        public AHKForm(Subject subject)
        {
            InitializeComponent();
            subject.Attach(this);
            this.ahk = ProfileSingleton.GetCurrent().AHK;
            this.presenter = new AHKPresenter(this, this.ahk);
            
            InitializeCheckAsThreeState();
            SetLegendDefaultValues();
            WireUpEvents();
        }

        private void WireUpEvents()
        {
            this.txtSpammerDelay.TextChanged += (s, e) => AHKDelayChanged?.Invoke(this, EventArgs.Empty);
            this.chkNoShift.CheckedChanged += (s, e) => NoShiftChanged?.Invoke(this, EventArgs.Empty);
            this.chkMouseFlick.CheckedChanged += (s, e) => MouseFlickChanged?.Invoke(this, EventArgs.Empty);
            
            foreach (Control c in this.groupAhkConfig.Controls)
            {
                if (c is RadioButton rb)
                {
                    rb.CheckedChanged += (s, e) => {
                        if(rb.Checked) AHKModeChanged?.Invoke(this, EventArgs.Empty);
                        DisableControlsIfSpeedBoost();
                    };
                }
            }
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.ahk = ProfileSingleton.GetCurrent().AHK;
                    this.presenter.UpdateModel(this.ahk);
                    InitializeApplicationForm();
                    break;
                case MessageCode.TURN_ON:
                    if (this.ahk != null) this.ahk.Start();
                    break;
                case MessageCode.TURN_OFF:
                    if (this.ahk != null) this.ahk.Stop();
                    break;
            }
        }

        private void InitializeApplicationForm()
        {
            RemoveHandlers();
            FormUtils.ResetCheckboxForm(this);
            SetLegendDefaultValues();
            
            // Re-apply values from model (via presenter update would be better, but we need to handle dynamic checkboxes here)
            this.presenter.UpdateView(); // Updates basic fields
            
            // Handle Radio Button manually for now as it maps to string
            RadioButton rdAhkMode = (RadioButton)this.groupAhkConfig.Controls[this.ahk.ahkMode];
            if (rdAhkMode != null) { rdAhkMode.Checked = true; };

            DisableControlsIfSpeedBoost();

            // Handle Checkboxes
            Dictionary<string, KeyConfig> ahkClones = new Dictionary<string, KeyConfig>(this.ahk.AhkEntries);
            foreach (KeyValuePair<string, KeyConfig> config in ahkClones)
            {
                ToggleCheckboxByName(config.Key, config.Value.ClickActive);
            }
            
            InitializeCheckAsThreeState();
        }

        private void onCheckChange(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            bool isChecked = (checkbox.CheckState == CheckState.Checked || checkbox.CheckState == CheckState.Indeterminate);
            bool isClickActive = (checkbox.CheckState == CheckState.Checked);
            string keyString = checkbox.Tag != null ? checkbox.Tag.ToString() : checkbox.Text;

            this.presenter.ProcessCheckboxChange(checkbox.Name, isChecked, isClickActive, keyString);
        }

        // IAHKView Implementation
        public string AHKDelay { get => txtSpammerDelay.Value.ToString(); set { try { txtSpammerDelay.Value = decimal.Parse(value); } catch {} } }
        public bool NoShift { get => chkNoShift.Checked; set => chkNoShift.Checked = value; }
        public bool MouseFlick { get => chkMouseFlick.Checked; set => chkMouseFlick.Checked = value; }
        public string AHKMode 
        { 
            get 
            {
                foreach (Control c in this.groupAhkConfig.Controls)
                    if (c is RadioButton rb && rb.Checked) return rb.Name;
                return AHK.COMPATIBILITY;
            }
            set 
            {
                 // Handled in InitializeApplicationForm usually, but setter can be here too
            }
        }

        public event EventHandler AHKDelayChanged;
        public event EventHandler NoShiftChanged;
        public event EventHandler MouseFlickChanged;
        public event EventHandler AHKModeChanged;
        public event EventHandler<KeyConfigEventArgs> CheckboxChanged;

        // Legacy / Helper methods kept for UI logic
        private void ToggleCheckboxByName(string Name, bool state)
        {
            try
            {
                CheckBox checkBox = (CheckBox)this.Controls.Find(Name, true)[0];
                checkBox.CheckState = state ? CheckState.Checked : CheckState.Indeterminate;
            }
            catch { }
        }

        private void RemoveHandlers()
        {
            foreach (Control c in this.Controls)
                if (c is CheckBox)
                {
                    CheckBox check = (CheckBox)c;
                    check.CheckStateChanged -= onCheckChange;
                }
        }

        private void InitializeCheckAsThreeState()
        {
            foreach (Control c in this.Controls)
                if (c is CheckBox)
                {
                    CheckBox check = (CheckBox)c;
                    if ((check.Name.Split(new[] { "chk" }, StringSplitOptions.None).Length == 2))
                    {
                        check.ThreeState = true;
                    };

                    if (check.Enabled)
                        check.CheckStateChanged += onCheckChange;
                }
        }

        private void SetLegendDefaultValues()
        {
            this.cbWithNoClick.ThreeState = true;
            this.cbWithNoClick.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.cbWithNoClick.AutoCheck = false;
            this.cbWithClick.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbWithClick.ThreeState = true;
            this.cbWithClick.AutoCheck = false;
        }

        private void DisableControlsIfSpeedBoost()
        {
            if (this.ahk.ahkMode == AHK.SPEED_BOOST)
            {
                this.chkMouseFlick.Enabled = false;
                this.chkNoShift.Enabled = false;
            } else
            {
                this.chkMouseFlick.Enabled = true;
                this.chkNoShift.Enabled = true;
            }
        }
    }
}
