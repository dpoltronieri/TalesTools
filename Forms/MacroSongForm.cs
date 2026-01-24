using System;
using _4RTools.Model;
using _4RTools.Utils;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Input;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class MacroSongForm : Form, IObserver, IMacroSongView
    {
        public static int TOTAL_MACRO_LANES_FOR_SONGS = 8;
        private MacroSongPresenter presenter;
        private Macro macro;

        public MacroSongForm(Subject subject)
        {
            InitializeComponent();
            subject.Attach(this);
            this.macro = ProfileSingleton.GetCurrent().SongMacro;
            this.presenter = new MacroSongPresenter(this, this.macro);
            ConfigureMacroLanes();
        }

        public void Update(ISubject subject) 
        { 
            switch((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.macro = ProfileSingleton.GetCurrent().SongMacro;
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
            for (int i = 1; i <= TOTAL_MACRO_LANES_FOR_SONGS; i++)
            {
                InitializeLane(i);
            }
        }

        private void InitializeLane(int id)
        {
            try
            {
                GroupBox p = (GroupBox)this.Controls.Find("panelMacro" + id, true)[0];
                foreach (Control c in p.Controls)
                {
                    if (c is TextBox textBox)
                    {
                        textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                        textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                        textBox.TextChanged += (s, e) => {
                            string tag = textBox.Tag?.ToString();
                            int laneId = tag != null ? short.Parse(tag.Split(':')[0]) : short.Parse(textBox.Name.Split(new[] { "mac" }, StringSplitOptions.None)[1]);
                            MacroChanged?.Invoke(this, new MacroEventArgs { LaneId = laneId, ControlName = textBox.Name, Text = textBox.Text, Tag = tag });
                        };
                    }

                    if (c is Button resetButton)
                    {
                        resetButton.Click += (s, e) => {
                            int btnResetID = Int16.Parse(resetButton.Name.Split(new[] { "btnResMac" }, StringSplitOptions.None)[1]);
                            ResetRequested?.Invoke(this, new MacroEventArgs { LaneId = btnResetID });
                        };
                    }

                    if (c is NumericUpDown numericUpDown)
                    {
                        numericUpDown.ValueChanged += (s, e) => {
                            int macroID = Int16.Parse(numericUpDown.Name.Split(new[] { "delayMac" }, StringSplitOptions.None)[1]);
                            DelayChanged?.Invoke(this, new MacroEventArgs { LaneId = macroID, Delay = decimal.ToInt32(numericUpDown.Value) });
                        };
                    }
                }
            } catch { }
        }

        // IMacroSongView Implementation
        public event EventHandler<MacroEventArgs> MacroChanged;
        public event EventHandler<MacroEventArgs> DelayChanged;
        public event EventHandler<MacroEventArgs> ResetRequested;

        public void UpdateControl(int laneId, string controlName, string value)
        {
            try
            {
                GroupBox p = (GroupBox)this.Controls.Find("panelMacro" + laneId, true)[0];
                Control[] controls = p.Controls.Find(controlName, true);
                if (controls.Length > 0 && controls[0] is TextBox textBox)
                {
                    if(textBox.Text != value) textBox.Text = value == "None" ? "" : value;
                }
            } catch { }
        }

        public void UpdateDelay(int laneId, int value)
        {
            try
            {
                GroupBox p = (GroupBox)this.Controls.Find("panelMacro" + laneId, true)[0];
                Control[] controls = p.Controls.Find("delayMac" + laneId, true);
                if (controls.Length > 0 && controls[0] is NumericUpDown num)
                {
                    if(num.Value != value) num.Value = value;
                }
            } catch { }
        }

        public void ResetLane(int laneId)
        {
            try
            {
                GroupBox p = (GroupBox)this.Controls.Find("panelMacro" + laneId, true)[0];
                FormUtils.ResetForm(p);
            } catch { }
        }
    }
}
