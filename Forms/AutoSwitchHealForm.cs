using System.Windows.Forms;
using System;
using System.Windows.Input;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Forms
{
    public partial class AutoSwitchHealForm : Form, IObserver
    {
        private AutoSwitchHeal autoSwitchHeal;

        public AutoSwitchHealForm(Subject subject, bool isYgg)
        {
            InitializeComponent();
            subject.Attach(this);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.autoSwitchHeal = ProfileSingleton.GetCurrent().AutoSwitchHeal;
                    InitializeApplicationForm();
                    break;
                case MessageCode.TURN_HEAL_OFF:
                    this.autoSwitchHeal.Stop();
                    break;
                case MessageCode.TURN_HEAL_ON:
                    this.autoSwitchHeal.Start();
                    break;
            }
        }

        private void InitializeApplicationForm()
        {
            SetupInputs();
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
                        textBox.TextChanged += new EventHandler(this.onTextChange);

                    }
                    if (c is NumericUpDown)
                    {
                        NumericUpDown numericUpDown = (NumericUpDown)c;
                        numericUpDown.ValueChanged += new EventHandler(this.onPercentChange);
                    }
                    FillForm(c);
                }
            }
            catch { }

        }

        private void FillForm(Control c)
        {
            var property = typeof(AutoSwitchHeal).GetProperty(c.Name.Substring(3));
            if (property != null)
            {
                c.Text = property.GetValue(this.autoSwitchHeal)?.ToString();
            }
        }

        private void onTextChange(object sender, EventArgs e)
        {
            try
            {
                TextBox txtbox = (TextBox)sender;
                Key key = (Key)Enum.Parse(typeof(Key), txtbox.Text.ToString());
                var property = typeof(AutoSwitchHeal).GetProperty(txtbox.Name.Substring(3));
                if (property != null)
                {
                    var oldValue = property.GetValue(this.autoSwitchHeal);
                    if (!Equals(oldValue, key))
                    {
                        property.SetValue(this.autoSwitchHeal, key);
                        ProfileSingleton.SetConfiguration(this.autoSwitchHeal);
                    }
                }
                this.ActiveControl = null;
            }
            catch (Exception ex)
            {
                var exception = ex;
            }
        }

        private void onPercentChange(object sender, EventArgs e)
        {
            try
            {
                NumericUpDown numericUpDown = (NumericUpDown)sender;
                int percent = Int16.Parse(numericUpDown.Text);
                var property = typeof(AutoSwitchHeal).GetProperty(numericUpDown.Name.Substring(3));
                if (property != null)
                {
                    var oldValue = property.GetValue(this.autoSwitchHeal);
                    if (!Equals(oldValue, percent))
                    {
                        property.SetValue(this.autoSwitchHeal, percent);
                        ProfileSingleton.SetConfiguration(this.autoSwitchHeal);
                    }
                }
                this.ActiveControl = null;
            }
            catch (Exception ex)
            {
                var exception = ex;
            }
        }
    }
}
