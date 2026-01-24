using System;
using System.Windows.Forms;
using System.Windows.Input;
using System.Collections.Generic;
using _4RTools.Utils;
using _4RTools.Model;
using System.Linq;
using System.Windows.Media.Effects;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class AutoBuffStatusForm : Form, IObserver, IAutoBuffStatusView
    {
        private List<BuffContainer> debuffContainers = new List<BuffContainer>();
        private AutoBuffStatusPresenter presenter;
        private StatusRecovery statusRecovery;
        private DebuffsRecovery debuffsRecovery;

        public AutoBuffStatusForm(Subject subject)
        {
            InitializeComponent();
            debuffContainers.Add(new BuffContainer(this.DebuffsGP, Buff.GetDebuffs()));

            new DebuffRenderer(debuffContainers, toolTip1).doRender();
            
            this.statusRecovery = ProfileSingleton.GetCurrent().StatusRecovery;
            this.debuffsRecovery = ProfileSingleton.GetCurrent().DebuffsRecovery;
            this.presenter = new AutoBuffStatusPresenter(this, this.statusRecovery, this.debuffsRecovery);

            WireUpInputHandlers();
            WireUpEvents();

            subject.Attach(this);
        }

        private void WireUpInputHandlers()
        {
            this.txtStatusKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.txtStatusKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.txtNewStatusKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.txtNewStatusKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            
            var groupbox = this.Controls.OfType<GroupBox>().FirstOrDefault();
            if (groupbox != null)
            {
                foreach (TextBox txt in groupbox.Controls.OfType<TextBox>())
                {
                    txt.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                    txt.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                    txt.TextChanged += (s, e) => {
                        if (string.IsNullOrEmpty(txt.Text)) return;
                        EffectStatusIDs id = (EffectStatusIDs)int.Parse(txt.Name.Split('n')[1]);
                        DebuffKeyChanged?.Invoke(this, new AutoBuffStatusKeyEventArgs { Id = id, Key = txt.Text });
                    };
                }
            }
        }

        private void WireUpEvents()
        {
            this.txtStatusKey.TextChanged += (s, e) => StatusKeyChanged?.Invoke(this, EventArgs.Empty);
            this.txtNewStatusKey.TextChanged += (s, e) => NewStatusKeyChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.statusRecovery = ProfileSingleton.GetCurrent().StatusRecovery;
                    this.debuffsRecovery = ProfileSingleton.GetCurrent().DebuffsRecovery;
                    this.presenter.UpdateModels(this.statusRecovery, this.debuffsRecovery);
                    break;
                case MessageCode.TURN_OFF:
                    this.debuffsRecovery.Stop();
                    this.statusRecovery.Stop();
                    break;
                case MessageCode.TURN_ON:
                    this.debuffsRecovery.Start();
                    this.statusRecovery.Start();
                    break;
            }
        }

        // IAutoBuffStatusView Implementation
        public string StatusKey { get => txtStatusKey.Text; set => txtStatusKey.Text = value; }
        public string NewStatusKey { get => txtNewStatusKey.Text; set => txtNewStatusKey.Text = value; }

        public event EventHandler StatusKeyChanged;
        public event EventHandler NewStatusKeyChanged;
        public event EventHandler<AutoBuffStatusKeyEventArgs> DebuffKeyChanged;

        public void SetDebuffKey(EffectStatusIDs id, string key)
        {
            try
            {
                var groupbox = this.Controls.OfType<GroupBox>().FirstOrDefault();
                if (groupbox != null)
                {
                    string controlName = "in" + (int)id;
                    Control[] controls = groupbox.Controls.Find(controlName, true);
                    if (controls.Length > 0 && controls[0].Text != key)
                    {
                        controls[0].Text = key == "None" ? "" : key;
                    }
                }
            }
            catch { }
        }
    }
}
