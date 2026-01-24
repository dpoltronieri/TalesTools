using System;
using System.Windows.Forms;
using System.Windows.Input;
using System.Collections.Generic;
using _4RTools.Model;
using _4RTools.Utils;
using System.Text.RegularExpressions;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class MacroSwitchForm : Form, IObserver, IMacroSwitchView
    {
        public static int TOTAL_MACRO_LANES = 10;
        private MacroSwitchPresenter presenter;
        private Macro macro;

        public MacroSwitchForm(Subject subject)
        {
            InitializeComponent();
            subject.Attach(this);
            this.macro = ProfileSingleton.GetCurrent().MacroSwitch;
            this.presenter = new MacroSwitchPresenter(this, this.macro);
            ConfigureMacroLanes();
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.macro = ProfileSingleton.GetCurrent().MacroSwitch;
                    this.presenter.UpdateModel(this.macro);
                    break;
                case MessageCode.TURN_ON:
                    this.macro.Start();
                    break;
                case MessageCode.TURN_OFF:
                    this.macro.Stop();
                    break;
            }
        }

        private void ConfigureMacroLanes()
        {
            for (int i = 1; i <= TOTAL_MACRO_LANES; i++)
            {
                InitializeLane(i);
            }
        }

        private void InitializeLane(int id)
        {
            try
            {
                GroupBox p = (GroupBox)this.Controls.Find("chainGroup" + id, true)[0];
                foreach (Control control in p.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                        textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                        textBox.TextChanged += (s, e) => {
                            int chainID = Int16.Parse(textBox.Parent.Name.Split(new[] { "chainGroup" }, StringSplitOptions.None)[1]);
                            MacroChanged?.Invoke(this, new MacroSwitchEventArgs { LaneId = chainID, ControlName = textBox.Name, Text = textBox.Text });
                        };
                    }

                    if (control is NumericUpDown delayInput)
                    {
                        delayInput.ValueChanged += (s, e) => {
                            int chainID = Int16.Parse(delayInput.Parent.Name.Split(new[] { "chainGroup" }, StringSplitOptions.None)[1]);
                            DelayChanged?.Invoke(this, new MacroSwitchEventArgs { LaneId = chainID, ControlName = delayInput.Name, Delay = decimal.ToInt32(delayInput.Value) });
                        };
                    }

                    if (control is CheckBox checkInput)
                    {
                        checkInput.CheckedChanged += (s, e) => {
                            int chainID = Int16.Parse(checkInput.Parent.Name.Split(new[] { "chainGroup" }, StringSplitOptions.None)[1]);
                            ClickChanged?.Invoke(this, new MacroSwitchEventArgs { LaneId = chainID, ControlName = checkInput.Name, Checked = checkInput.Checked });
                        };
                    }
                }
            }
            catch { }
        }

        // IMacroSwitchView Implementation
        public event EventHandler<MacroSwitchEventArgs> MacroChanged;
        public event EventHandler<MacroSwitchEventArgs> DelayChanged;
        public event EventHandler<MacroSwitchEventArgs> ClickChanged;

        public void UpdateControl(int laneId, string controlName, string value)
        {
            try {
                GroupBox group = (GroupBox)this.Controls.Find("chainGroup" + laneId, true)[0];
                Control[] controls = group.Controls.Find(controlName, true);
                if (controls.Length > 0 && controls[0] is TextBox textBox)
                {
                    if (textBox.Text != value) textBox.Text = value == "None" ? "" : value;
                }
            } catch {}
        }

        public void UpdateDelay(int laneId, string controlName, int value)
        {
            try {
                GroupBox group = (GroupBox)this.Controls.Find("chainGroup" + laneId, true)[0];
                Control[] controls = group.Controls.Find(controlName, true);
                if (controls.Length > 0 && controls[0] is NumericUpDown num)
                {
                    if (num.Value != value) num.Value = value;
                }
            } catch {}
        }

        public void UpdateClick(int laneId, string controlName, bool value)
        {
            try {
                GroupBox group = (GroupBox)this.Controls.Find("chainGroup" + laneId, true)[0];
                Control[] controls = group.Controls.Find(controlName, true);
                if (controls.Length > 0 && controls[0] is CheckBox chk)
                {
                    if (chk.Checked != value) chk.Checked = value;
                }
            } catch {}
        }
    }
}
