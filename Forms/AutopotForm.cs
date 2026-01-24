using System.Windows.Forms;
using System;
using System.Windows.Input;
using _4RTools.Model;
using _4RTools.Utils;
using System.Drawing;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class AutopotForm : Form, IObserver, IAutopotView
    {
        private Autopot autopot;
        private bool isYgg;
        private AutopotPresenter presenter;

        public AutopotForm(Subject subject, bool isYgg)
        {
            InitializeComponent();
            this.isYgg = isYgg;
            
            // Initial model load
            this.autopot = this.isYgg ? ProfileSingleton.GetCurrent().AutopotYgg : ProfileSingleton.GetCurrent().Autopot;
            this.presenter = new AutopotPresenter(this, this.autopot);

            if (isYgg)
            {
                this.picBoxHP.Image = Resources._4RTools.ETCResource.Yggdrasil;
                this.picBoxSP.Image = Resources._4RTools.ETCResource.Yggdrasil;
                this.chkStopWitchFC.Hide();
                this.lblequipBefore.Hide();
                this.lblequipAfter.Hide();
                this.txtHpEquipAfter.Hide();
                this.txtHpEquipBefore.Hide();
                this.txtSpEquipBefore.Hide();
                this.txtSpEquipAfter.Hide();
            }
            subject.Attach(this);
            
            WireUpEvents();
            WireUpInputHandlers();
        }

        private void WireUpEvents()
        {
            this.txtHpKey.TextChanged += (s, e) => HpKeyChanged?.Invoke(this, EventArgs.Empty);
            this.txtSPKey.TextChanged += (s, e) => SpKeyChanged?.Invoke(this, EventArgs.Empty);
            this.txtHPpct.ValueChanged += (s, e) => HpPercentChanged?.Invoke(this, EventArgs.Empty); // NumericUpDown uses ValueChanged
            this.txtHPpct.KeyUp += (s, e) => HpPercentChanged?.Invoke(this, EventArgs.Empty); // Also catch text edits
            this.txtSPpct.ValueChanged += (s, e) => SpPercentChanged?.Invoke(this, EventArgs.Empty);
            this.txtSPpct.KeyUp += (s, e) => SpPercentChanged?.Invoke(this, EventArgs.Empty);
            this.txtAutopotDelay.TextChanged += (s, e) => DelayChanged?.Invoke(this, EventArgs.Empty);
            this.txtHpEquipBefore.TextChanged += (s, e) => HpEquipBeforeChanged?.Invoke(this, EventArgs.Empty);
            this.txtHpEquipAfter.TextChanged += (s, e) => HpEquipAfterChanged?.Invoke(this, EventArgs.Empty);
            this.txtSpEquipBefore.TextChanged += (s, e) => SpEquipBeforeChanged?.Invoke(this, EventArgs.Empty);
            this.txtSpEquipAfter.TextChanged += (s, e) => SpEquipAfterChanged?.Invoke(this, EventArgs.Empty);
            this.chkStopWitchFC.CheckedChanged += (s, e) => StopWitchFCChanged?.Invoke(this, EventArgs.Empty);
            
            this.firstHP.CheckedChanged += (s, e) => { if (firstHP.Checked) FirstHealChanged?.Invoke(this, EventArgs.Empty); };
            this.firstSP.CheckedChanged += (s, e) => { if (firstSP.Checked) FirstHealChanged?.Invoke(this, EventArgs.Empty); };
        }

        private void WireUpInputHandlers()
        {
            txtHpKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtHpKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtSPKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtSPKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtHpEquipBefore.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtHpEquipBefore.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtHpEquipAfter.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtHpEquipAfter.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtSpEquipBefore.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtSpEquipBefore.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtSpEquipAfter.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtSpEquipAfter.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.autopot = this.isYgg ? ProfileSingleton.GetCurrent().AutopotYgg : ProfileSingleton.GetCurrent().Autopot;
                    this.presenter.UpdateModel(this.autopot);
                    break;
                case MessageCode.TURN_HEAL_OFF:
                    if (this.autopot != null) this.autopot.Stop();
                    break;
                case MessageCode.TURN_HEAL_ON:
                    if (this.autopot != null) this.autopot.Start();
                    break;
            }
        }

        // IAutopotView Implementation
        public string HpKey { get => txtHpKey.Text; set => txtHpKey.Text = value; }
        public string SpKey { get => txtSPKey.Text; set => txtSPKey.Text = value; }
        public string HpPercent { get => txtHPpct.Text; set => txtHPpct.Text = value; }
        public string SpPercent { get => txtSPpct.Text; set => txtSPpct.Text = value; }
        public string Delay { get => txtAutopotDelay.Text; set => txtAutopotDelay.Text = value; }
        public string HpEquipBefore { get => txtHpEquipBefore.Text; set => txtHpEquipBefore.Text = value; }
        public string HpEquipAfter { get => txtHpEquipAfter.Text; set => txtHpEquipAfter.Text = value; }
        public string SpEquipBefore { get => txtSpEquipBefore.Text; set => txtSpEquipBefore.Text = value; }
        public string SpEquipAfter { get => txtSpEquipAfter.Text; set => txtSpEquipAfter.Text = value; }
        public bool StopWitchFC { get => chkStopWitchFC.Checked; set => chkStopWitchFC.Checked = value; }
        public string FirstHeal
        {
            get => firstHP.Checked ? Autopot.FIRSTHP : Autopot.FIRSTSP;
            set
            {
                if (value == Autopot.FIRSTHP) firstHP.Checked = true;
                else firstSP.Checked = true;
            }
        }

        public event EventHandler HpKeyChanged;
        public event EventHandler SpKeyChanged;
        public event EventHandler HpPercentChanged;
        public event EventHandler SpPercentChanged;
        public event EventHandler DelayChanged;
        public event EventHandler HpEquipBeforeChanged;
        public event EventHandler HpEquipAfterChanged;
        public event EventHandler SpEquipBeforeChanged;
        public event EventHandler SpEquipAfterChanged;
        public event EventHandler StopWitchFCChanged;
        public event EventHandler FirstHealChanged;
    }
}
