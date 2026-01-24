using System.Windows.Forms;
using System;
using System.Windows.Input;
using _4RTools.Model;
using _4RTools.Utils;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class AutoSwitchHealForm : Form, IObserver, IAutoSwitchHealView
    {
        private AutoSwitchHeal autoSwitchHeal;
        private AutoSwitchHealPresenter presenter;

        public AutoSwitchHealForm(Subject subject, bool isYgg)
        {
            InitializeComponent();
            subject.Attach(this);
            this.autoSwitchHeal = ProfileSingleton.GetCurrent().AutoSwitchHeal;
            this.presenter = new AutoSwitchHealPresenter(this, this.autoSwitchHeal);
            SetupInputs();
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.autoSwitchHeal = ProfileSingleton.GetCurrent().AutoSwitchHeal;
                    this.presenter.UpdateModel(this.autoSwitchHeal);
                    break;
                case MessageCode.TURN_HEAL_OFF:
                    if (this.autoSwitchHeal != null)
                    {
                        this.autoSwitchHeal.Stop();
                    }
                    break;
                case MessageCode.TURN_HEAL_ON:
                    if (this.autoSwitchHeal != null)
                    {
                        this.autoSwitchHeal.Start();
                    }
                    break;
            }
        }

        public void SetupInputs()
        {
            try
            {
                foreach (Control c in this.Controls)
                {
                    if (c is TextBox)
                    {
                        TextBox textBox = (TextBox)c;
                        textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                        textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                        textBox.TextChanged += (s, e) => PropertyChanged?.Invoke(this, new AutoSwitchHealEventArgs { PropertyName = textBox.Name.Substring(3), Value = textBox.Text });
                    }
                    if (c is NumericUpDown)
                    {
                        NumericUpDown numericUpDown = (NumericUpDown)c;
                        numericUpDown.ValueChanged += (s, e) => PropertyChanged?.Invoke(this, new AutoSwitchHealEventArgs { PropertyName = numericUpDown.Name.Substring(3), Value = numericUpDown.Value.ToString() });
                    }
                }
            }
            catch { }
        }

        public event EventHandler<AutoSwitchHealEventArgs> PropertyChanged;

        public void SetControlValue(string propertyName, string value)
        {
            try
            {
                string controlName = "txt" + propertyName; // Conventional prefix, or iterate all
                Control[] controls = this.Controls.Find(controlName, true);
                if (controls.Length == 0)
                {
                    // Fallback to numeric or other prefixes if necessary
                    controls = this.Controls.Find("num" + propertyName, true);
                }

                if (controls.Length > 0)
                {
                    Control c = controls[0];
                    if (c is TextBox && c.Text != value) c.Text = value;
                    if (c is NumericUpDown num)
                    {
                        decimal val = decimal.Parse(value);
                        if (num.Value != val) num.Value = val;
                    }
                }
            }
            catch { }
        }
    }
}
