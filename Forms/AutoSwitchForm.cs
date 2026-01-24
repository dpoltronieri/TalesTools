using System;
using System.Windows.Forms;
using System.Windows.Input;
using System.Collections.Generic;
using _4RTools.Utils;
using _4RTools.Model;
using System.Linq;
using static _4RTools.Model.AutoSwitch;
using System.Diagnostics;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class AutoSwitchForm : Form, IObserver, IAutoSwitchView
    {
        private List<Buff> allBuffs = new List<Buff>();
        private Subject _subject;
        string OldTextKey = string.Empty;
        private AutoSwitchPresenter presenter;
        private AutoSwitch autoSwitch;

        class ComboboxValue
        {
            public int Id { get; private set; }
            public string Name { get; private set; }

            public ComboboxValue(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public AutoSwitchForm(Subject subject)
        {
            InitializeComponent();

            setupInputs();
            _subject = subject;
            this.autoSwitch = ProfileSingleton.GetCurrent().AutoSwitch;
            this.presenter = new AutoSwitchPresenter(this, this.autoSwitch);

            this.allBuffs.Add(new Buff("Incenso Queimado", EffectStatusIDs.SPIRIT, Resources._4RTools.Icons.burnt_incense));
            this.allBuffs.AddRange(Buff.GetArcherSkills());
            this.allBuffs.AddRange(Buff.GetSwordmanSkill());
            this.allBuffs.AddRange(Buff.GetMageSkills());
            this.allBuffs.AddRange(Buff.GetMerchantSkills());
            this.allBuffs.AddRange(Buff.GetThiefSkills());
            this.allBuffs.AddRange(Buff.GetAcolyteSkills());
            this.allBuffs.AddRange(Buff.GetTaekwonSkills());
            this.allBuffs.AddRange(Buff.GetNinjaSkills());
            this.allBuffs.AddRange(Buff.GetGunsSkills());

            foreach (var skill in this.allBuffs.OrderBy(o => o.name).ToList())
            {
                this.skillCB.Items.Add(skill.name);
            }

            subject.Attach(this);
            WireUpEvents();
        }

        private void WireUpEvents()
        {
            this.numDelay.ValueChanged += (s, e) => DelayChanged?.Invoke(this, EventArgs.Empty);
            this.numSwitchDelay.ValueChanged += (s, e) => SwitchDelayChanged?.Invoke(this, EventArgs.Empty);
        }

        private void setupInputs()
        {
            GroupBox group = (GroupBox)this.Controls.Find("ProcSwitchGP", true)[0];

            foreach (Control c in group.Controls)
            {
                if (c is TextBox)
                {
                    TextBox textBox = (TextBox)c;
                    textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                    textBox.TextChanged += new EventHandler(this.onTextChange);
                    textBox.GotFocus += new EventHandler(this.onFocus);
                }
            }
        }

        private void loadCustomSkills(Subject subject)
        {
            List<Buff> filteredBuffs = new List<Buff>();
            foreach (var skill in this.autoSwitch.autoSwitchGenericMapping)
            {
                if (this.allBuffs.Exists(x => x.effectStatusID == skill.skillId))
                {
                    filteredBuffs.Add(this.allBuffs.FirstOrDefault(x => x.effectStatusID == skill.skillId));
                }
            }

            AutoSwitchContainer skillContainer = new AutoSwitchContainer();
            skillContainer = new AutoSwitchContainer(this.AutoSwitchGP, filteredBuffs);
            new AutoSwitchRenderer(skillContainer, toolTip1, subject, this).doRender();
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.autoSwitch = ProfileSingleton.GetCurrent().AutoSwitch;
                    this.presenter.UpdateModel(this.autoSwitch); // Helper to refresh view
                    loadCustomSkills(subject as Subject);
                    break;
                case MessageCode.TURN_OFF:
                    this.autoSwitch.Stop();
                    break;
                case MessageCode.TURN_ON:
                    this.autoSwitch.Start();
                    break;
                case MessageCode.CHANGED_AUTOSWITCH_SKILL:
                    loadCustomSkills(subject as Subject);
                    break;
            }
        }

        // Removed loadExclusiveSkills() as it is now handled by Presenter via SetKey

        private void onFocus(object sender, EventArgs e)
        { 
            TextBox txtBox = (TextBox)sender;
            OldTextKey = txtBox.Text;
        }

        private void onTextChange(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                bool textChanged = this.OldTextKey != String.Empty && this.OldTextKey != txtBox.Text.ToString();
                if ((txtBox.Text.ToString() != String.Empty) && textChanged)
                {
                    EffectStatusIDs statusID = (EffectStatusIDs)Int16.Parse(txtBox.Name.Split(new[] { "in" }, StringSplitOptions.None)[1]);
                    string type = txtBox.Name.Split(new[] { "in" }, StringSplitOptions.None)[0];
                    string key = txtBox.Text.ToString();

                    KeyChanged?.Invoke(this, new AutoSwitchKeyEventArgs { SkillId = statusID, Type = type, Key = key });
                    
                    _subject.Notify(new Utils.Message(Utils.MessageCode.ADDED_NEW_AUTOSWITCH_PETS, null));
                }
                this.ActiveControl = null;
            }
            catch { }
        }

        private void btnNewSwitch(object sender, EventArgs e)
        {
            var txtSkill = skillCB.Text;
            var skill = this.allBuffs.FirstOrDefault(x => x.name == txtSkill);
            
            if (skill != null)
            {
                AddNewSkill?.Invoke(this, skill.effectStatusID);
                _subject.Notify(new Utils.Message(Utils.MessageCode.CHANGED_AUTOSWITCH_SKILL, null));
            }
        }

        // IAutoSwitchView Implementation
        public string Delay { get => numDelay.Value.ToString(); set { try { numDelay.Value = decimal.Parse(value); } catch {} } }
        public string SwitchDelay { get => numSwitchDelay.Value.ToString(); set { try { numSwitchDelay.Value = decimal.Parse(value); } catch {} } }

        public event EventHandler DelayChanged;
        public event EventHandler SwitchDelayChanged;
        public event EventHandler<AutoSwitchKeyEventArgs> KeyChanged;
        public event EventHandler<EffectStatusIDs> AddNewSkill;

        public void SetKey(EffectStatusIDs id, string type, string key)
        {
            try
            {
                string controlName = $"{type}in{(int)id}";
                // Note: The original code parsed "in" as separator. 
                // Name format in Designer seems to be like "ITEMin34", "SKILLin34".
                // Let's verify control finding.
                GroupBox group = (GroupBox)this.Controls.Find("ProcSwitchGP", true)[0];
                Control[] controls = group.Controls.Find(controlName, true);
                if (controls.Length > 0)
                {
                    if (controls[0].Text != key)
                        controls[0].Text = key == "None" ? "" : key; // Or just key? Original used Keys.None.ToString() which is "None"
                }
            }
            catch {}
        }
    }
}
