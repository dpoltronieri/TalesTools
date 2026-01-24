using System;
using System.Collections.Generic;
using System.Windows.Forms;
using _4RTools.Utils;
using _4RTools.Model;
using System.Windows.Input;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class ATKDEFForm : Form, IObserver, IATKDEFView
    {
        public static int TOTAL_ATKDEF_LANES = 8;
        public static int TOTAL_EQUIPS = 6;
        private ATKDEFPresenter presenter;
        private ATKDEFMode model;

        public ATKDEFForm(Subject subject)
        {
            InitializeComponent();
            subject.Attach(this);
            this.model = ProfileSingleton.GetCurrent().AtkDefMode;
            this.presenter = new ATKDEFPresenter(this, this.model);
            SetupInputs();
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.model = ProfileSingleton.GetCurrent().AtkDefMode;
                    this.presenter.UpdateModel(this.model);
                    break;
                case MessageCode.TURN_ON:
                    this.model.Start();
                    break;
                case MessageCode.TURN_OFF:
                    this.model.Stop();
                    break;
            }
        }

        public event EventHandler<ATKDEFEventArgs> ChangeRequested;

        public void UpdateControlValue(int laneId, string controlType, string value)
        {
            try
            {
                string suffix = controlType == "spammerKey" ? "SpammerKey" :
                                controlType == "spammerDelay" ? "SpammerDelay" :
                                controlType == "switchDelay" ? "SwitchDelay" :
                                controlType == "spammerClick" ? "SpammerClick" : "";
                
                string controlName = "in" + laneId + suffix;
                Control[] controls = this.Controls.Find(controlName, true);
                if (controls.Length > 0)
                {
                    Control c = controls[0];
                    if (c is TextBox && c.Text != value) c.Text = value;
                    else if (c is NumericUpDown num)
                    {
                        decimal val = decimal.Parse(value);
                        if (num.Value != val) num.Value = val;
                    }
                    else if (c is CheckBox chk)
                    {
                        bool val = bool.Parse(value);
                        if (chk.Checked != val) chk.Checked = val;
                    }
                }
            }
            catch { }
        }

        public void UpdateEquipValue(int laneId, string type, int index, string value)
        {
            try
            {
                string controlName = "in" + laneId + type + index;
                Control[] controls = this.Controls.Find(controlName, true);
                if (controls.Length > 0 && controls[0].Text != value)
                {
                    controls[0].Text = value == "None" ? "" : value;
                }
            }
            catch { }
        }

        public void SetupInputs()
        {
            try
            {
                foreach (Control c in FormUtils.GetAll(this, typeof(TextBox)))
                {
                    if (c is TextBox textBox)
                    {
                        textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                        textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                        textBox.TextChanged += (s, e) => {
                            if (string.IsNullOrEmpty(textBox.Text)) return;
                            string[] inputTag = textBox.Tag.ToString().Split(':');
                            int id = int.Parse(inputTag[0]);
                            string type = inputTag[1];
                            if (type == "spammerKey")
                                ChangeRequested?.Invoke(this, new ATKDEFEventArgs { LaneId = id, ControlType = type, Value = textBox.Text });
                            else
                            {
                                string baseType = type.Remove(type.Length - 1).ToUpper();
                                ChangeRequested?.Invoke(this, new ATKDEFEventArgs { LaneId = id, ControlType = baseType, Value = textBox.Text, ControlName = textBox.Name });
                            }
                        };
                    }
                }

                foreach (Control c in FormUtils.GetAll(this, typeof(NumericUpDown)))
                {
                    if (c is NumericUpDown numeric)
                    {
                        numeric.ValueChanged += (s, e) => {
                            string[] inputTag = numeric.Tag.ToString().Split(':');
                            ChangeRequested?.Invoke(this, new ATKDEFEventArgs { LaneId = int.Parse(inputTag[0]), ControlType = inputTag[1], Value = numeric.Value.ToString() });
                        };
                    }
                }

                foreach (Control c in FormUtils.GetAll(this, typeof(CheckBox)))
                {
                    if (c is CheckBox chk)
                    {
                        chk.CheckedChanged += (s, e) => {
                            string[] inputTag = chk.Tag.ToString().Split(':');
                            ChangeRequested?.Invoke(this, new ATKDEFEventArgs { LaneId = int.Parse(inputTag[0]), ControlType = "spammerClick", Value = chk.Checked.ToString() });
                        };
                    }
                }
            }
            catch { }
        }
    }
}
