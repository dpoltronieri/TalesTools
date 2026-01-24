using System;
using System.Collections.Generic;
using System.Windows.Forms;
using _4RTools.Utils;
using _4RTools.Model;
using System.Windows.Input;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class SkillTimerForm : Form, IObserver, ISkillTimerView
    {
        public static int TOTAL_SKILL_TIMER = 4;
        private SkillTimerPresenter presenter;
        private AutoRefreshSpammer model;

        public SkillTimerForm(Subject subject)
        {
            InitializeComponent();
            subject.Attach(this);
            this.model = ProfileSingleton.GetCurrent().AutoRefreshSpammer;
            this.presenter = new SkillTimerPresenter(this, this.model);
            ConfigureTimerLanes();
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.model = ProfileSingleton.GetCurrent().AutoRefreshSpammer;
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

        private void ConfigureTimerLanes()
        {
            for (int i = 1; i <= TOTAL_SKILL_TIMER; i++)
            {
                InitializeLane(i);
            }
        }

        private void InitializeLane(int id)
        {
            try
            {
                TextBox textBox = (TextBox)this.Controls.Find("txtSkillTimerKey" + id, true)[0];
                textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                textBox.TextChanged += (s, e) => {
                    KeyChanged?.Invoke(this, new SkillTimerEventArgs { LaneId = id, Key = textBox.Text });
                };

                NumericUpDown txtAutoRefreshDelay = (NumericUpDown)this.Controls.Find("txtAutoRefreshDelay" + id, true)[0];
                txtAutoRefreshDelay.ValueChanged += (s, e) => {
                    DelayChanged?.Invoke(this, new SkillTimerEventArgs { LaneId = id, Delay = (int)txtAutoRefreshDelay.Value });
                };
            }
            catch { }
        }

        // ISkillTimerView Implementation
        public event EventHandler<SkillTimerEventArgs> KeyChanged;
        public event EventHandler<SkillTimerEventArgs> DelayChanged;

        public void UpdateControl(int laneId, string controlName, string value)
        {
            try
            {
                Control[] c = this.Controls.Find(controlName, true);
                if (c.Length > 0 && c[0] is TextBox textBox)
                {
                    if (textBox.Text != value) textBox.Text = value;
                }
            } catch {}
        }

        public void UpdateDelay(int laneId, int value)
        {
            try
            {
                Control[] d = this.Controls.Find("txtAutoRefreshDelay" + laneId, true);
                if (d.Length > 0 && d[0] is NumericUpDown num)
                {
                    if (num.Value != value) num.Value = value;
                }
            } catch {}
        }
    }
}
