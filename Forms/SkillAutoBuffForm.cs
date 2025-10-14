using System;
using System.Windows.Forms;
using _4RTools.Utils;
using _4RTools.Model;
using System.Windows.Input;
using System.Collections.Generic;

namespace _4RTools.Forms
{
    public partial class SkillAutoBuffForm : Form, IObserver
    {

        private List<BuffContainer> skillContainers =  new List<BuffContainer>();

        public SkillAutoBuffForm(Subject subject)
        {
            this.KeyPreview = true;
            InitializeComponent();

            skillContainers.Add(new BuffContainer(this.ArcherSkillsGP, Buff.GetArcherSkills()));
            skillContainers.Add(new BuffContainer(this.SwordmanSkillGP, Buff.GetSwordmanSkill()));
            skillContainers.Add(new BuffContainer(this.MageSkillGP, Buff.GetMageSkills()));
            skillContainers.Add(new BuffContainer(this.MerchantSkillsGP, Buff.GetMerchantSkills()));
            skillContainers.Add(new BuffContainer(this.ThiefSkillsGP, Buff.GetThiefSkills()));
            skillContainers.Add(new BuffContainer(this.AcolyteSkillsGP, Buff.GetAcolyteSkills()));
            skillContainers.Add(new BuffContainer(this.TKSkillGroupBox, Buff.GetTaekwonSkills()));
            skillContainers.Add(new BuffContainer(this.NinjaSkillsGP, Buff.GetNinjaSkills()));
            skillContainers.Add(new BuffContainer(this.GunsSkillsGP, Buff.GetGunsSkills()));

            new BuffRenderer(skillContainers, toolTip1, ProfileSingleton.GetCurrent().AutobuffSkill.actionName, subject).doRender();
            subject.Attach(this);

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
                    BuffRenderer.doUpdate(new Dictionary<EffectStatusIDs, Key>(ProfileSingleton.GetCurrent().AutobuffSkill.buffMapping), this);
                    this.numericDelay.Value = ProfileSingleton.GetCurrent().AutobuffSkill.delay;
                    break;
                case MessageCode.TURN_OFF:
                    ProfileSingleton.GetCurrent().AutobuffSkill.Stop();
                    break;
                case MessageCode.TURN_ON:
                    ProfileSingleton.GetCurrent().AutobuffSkill.Start();
                    break;
            }
        }

        private void btnResetAutobuff_Click(object sender, EventArgs e)
        {
            ProfileSingleton.GetCurrent().AutobuffSkill.ClearKeyMapping();
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutobuffSkill);
            BuffRenderer.doUpdate(new Dictionary<EffectStatusIDs, Key>(ProfileSingleton.GetCurrent().AutobuffSkill.buffMapping), this);
            this.numericDelay.Value = 100;
        }

        private void numericDelay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ProfileSingleton.GetCurrent().AutobuffSkill.delay = Convert.ToInt16(this.numericDelay.Value);
                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutobuffSkill);
                this.ActiveControl = null;
            }
            catch { }
        }
    }
}
