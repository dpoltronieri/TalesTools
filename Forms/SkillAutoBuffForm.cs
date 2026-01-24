using System;
using System.Windows.Forms;
using _4RTools.Utils;
using _4RTools.Model;
using System.Windows.Input;
using System.Collections.Generic;
using System.Drawing;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class SkillAutoBuffForm : Form, IObserver, ISkillAutoBuffView
    {
        private List<BuffContainer> skillContainers =  new List<BuffContainer>();
        private SkillAutoBuffPresenter presenter;
        private AutoBuffSkill model;

        public SkillAutoBuffForm(Subject subject)
        {
            this.KeyPreview = true;
            InitializeComponent();

            this.model = ProfileSingleton.GetCurrent().AutobuffSkill;
            this.presenter = new SkillAutoBuffPresenter(this, this.model);

            // Create a panel to hold the config buttons
            Panel configPanel = new Panel { Width = 520, Height = 60 };

            // Remove controls from the main form and add them to the panel
            this.Controls.Remove(this.label5);
            this.Controls.Remove(this.numericDelay);
            this.Controls.Remove(this.btnResetAutobuff);

            // Set locations relative to the panel
            this.label5.Location = new Point(10, 5);
            this.numericDelay.Location = new Point(10, 25);
            this.btnResetAutobuff.Location = new Point(80, 22);

            configPanel.Controls.Add(this.label5);
            configPanel.Controls.Add(this.numericDelay);
            configPanel.Controls.Add(this.btnResetAutobuff);

            // Add the panel to the top of the flow layout
            this.flowLayoutPanel1.Controls.Add(configPanel);


            skillContainers.Add(new BuffContainer(this.ArcherSkillsGP, Buff.GetArcherSkills()));
            skillContainers.Add(new BuffContainer(this.SwordmanSkillGP, Buff.GetSwordmanSkill()));
            skillContainers.Add(new BuffContainer(this.MageSkillGP, Buff.GetMageSkills()));
            skillContainers.Add(new BuffContainer(this.MerchantSkillsGP, Buff.GetMerchantSkills()));
            skillContainers.Add(new BuffContainer(this.ThiefSkillsGP, Buff.GetThiefSkills()));
            skillContainers.Add(new BuffContainer(this.AcolyteSkillsGP, Buff.GetAcolyteSkills()));
            skillContainers.Add(new BuffContainer(this.TKSkillGroupBox, Buff.GetTaekwonSkills()));
            skillContainers.Add(new BuffContainer(this.NinjaSkillsGP, Buff.GetNinjaSkills()));
            skillContainers.Add(new BuffContainer(this.GunsSkillsGP, Buff.GetGunsSkills()));

            foreach (var container in skillContainers)
            {
                this.Controls.Remove(container.container);
                this.flowLayoutPanel1.Controls.Add(container.container);
            }


            new BuffRenderer(skillContainers, toolTip1, ProfileSingleton.GetCurrent().AutobuffSkill.actionName, subject).doRender();
            subject.Attach(this);
            WireUpEvents();
        }

        private void WireUpEvents()
        {
            this.numericDelay.ValueChanged += (s, e) => DelayChanged?.Invoke(this, EventArgs.Empty);
            this.btnResetAutobuff.Click += (s, e) => ResetRequested?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RenderClassGroups();
        }

        private void RenderClassGroups()
        {
            this.Invoke((MethodInvoker)delegate {
                UserPreferences prefs = ProfileSingleton.GetCurrent().UserPreferences;

                // Mapping from config name to GroupBox control
                var groupMapping = new Dictionary<string, GroupBox> {
                    { "Swordman", SwordmanSkillGP },
                    { "Archer", ArcherSkillsGP },
                    { "Mage", MageSkillGP },
                    { "Thief", ThiefSkillsGP },
                    { "Acolyte", AcolyteSkillsGP },
                    { "Merchant", MerchantSkillsGP },
                };

                foreach (var entry in groupMapping)
                {
                    if (prefs.AutobuffSkillVisibility.TryGetValue(entry.Key, out bool isVisible))
                    {
                        entry.Value.Visible = isVisible;
                    }
                    else
                    {
                        entry.Value.Visible = true; // Default to visible
                    }
                }

                // Handle Taekwon
                if (prefs.AutobuffSkillVisibility.TryGetValue("Taekwon", out bool tkVisible))
                {
                    TKSkillGroupBox.Visible = tkVisible;
                }
                else
                {
                    TKSkillGroupBox.Visible = true;
                }

                // Handle Ninja
                if (prefs.AutobuffSkillVisibility.TryGetValue("Ninja", out bool ninjaVisible))
                {
                    NinjaSkillsGP.Visible = ninjaVisible;
                }
                else
                {
                    NinjaSkillsGP.Visible = true;
                }

                // Handle Gunslinger
                if (prefs.AutobuffSkillVisibility.TryGetValue("Gunslinger", out bool gunsVisible))
                {
                    GunsSkillsGP.Visible = gunsVisible;
                }
                else
                {
                    GunsSkillsGP.Visible = true;
                }
            });
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    RenderClassGroups();
                    this.model = ProfileSingleton.GetCurrent().AutobuffSkill;
                    this.presenter.UpdateModel(this.model);
                    break;
                case MessageCode.TURN_OFF:
                    this.model.Stop();
                    break;
                case MessageCode.TURN_ON:
                    this.model.Start();
                    break;
            }
        }

        // ISkillAutoBuffView Implementation
        public int Delay { get => decimal.ToInt32(numericDelay.Value); set { try { numericDelay.Value = value; } catch {} } }

        public event EventHandler DelayChanged;
        public event EventHandler ResetRequested;

        public void UpdateUI(Dictionary<EffectStatusIDs, System.Windows.Input.Key> buffMapping)
        {
            BuffRenderer.doUpdate(buffMapping, this);
        }
    }
}
