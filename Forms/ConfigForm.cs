using System;
using _4RTools.Model;
using System.Windows.Forms;
using _4RTools.Utils;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using static _4RTools.Model.AutoSwitch;

namespace _4RTools.Forms
{
    public partial class ConfigForm : Form, IObserver
    {

        private const string SKILL_SPAMMER_TAB = "tabPageSpammer";
        private const string AUTOBOSS_SKILL_TAB = "tabPageAutobuffSkill";
        private const string AUTOBOSS_STUFF_TAB = "tabPageAutobuffStuff";
        private const string AUTO_SWITCH_TAB = "tabPageAutoSwitch";
        private const string ATK_DEF_TAB = "atkDef";
        private const string MACRO_SONGS_TAB = "tabPageMacroSongs";
        private const string MACRO_SWITCH_TAB = "tabMacroSwitch";
        private const string DEBUFFS_TAB = "tabPageDebuffs";

        //Secondary Tabs
        private const string AUTOPOT_TAB = "tabPageAutopot";
        private const string YGG_AUTOPOT_TAB = "tabPageYggAutopot";
        private const string SKILL_TIMER_TAB = "tabPageSkillTimer";
        private const string AUTO_SWITCH_HEAL_TAB = "tabPageAutoSwitchHeal";

        //Autobuff Skill
        private const string SWORDMAN_GROUP = "Swordman";
        private const string ARCHER_GROUP = "Archer";
        private const string MAGE_GROUP = "Mage";
        private const string THIEF_GROUP = "Thief";
        private const string ACOLYTE_GROUP = "Acolyte";
        private const string MERCHANT_GROUP = "Merchant";
        private const string OTHERS_GROUP = "Others";
        private const string TAEKWON_GROUP = "Taekwon";
        private const string NINJA_GROUP = "Ninja";
        private const string GUNSLINGER_GROUP = "Gunslinger";

        //Autobuff Stuff
        private const string FOODS_GROUP = "FoodsGP";
        private const string POTIONS_GROUP = "PotionsGP";
        private const string BOX_GROUP = "BoxesGP";
        private const string ELEMENTALS_GROUP = "ElementalsGP";
        private const string SCROLL_GROUP = "ScrollBuffsGP";
        private const string ETC_GROUP = "EtcGP";


        public ConfigForm(Subject subject)
        {
            InitializeComponent();
            InitializeTabConfig();
            this.textReinKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.textReinKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.textReinKey.TextChanged += new EventHandler(this.textReinKey_TextChanged);

            this.ammo1textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.ammo1textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.ammo1textBox.TextChanged += new EventHandler(this.textAmmo1_TextChanged);
            this.ammo2textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.ammo2textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.ammo2textBox.TextChanged += new EventHandler(this.textAmmo2_TextChanged);


            var newListBuff = ProfileSingleton.GetCurrent().UserPreferences.autoBuffOrder;
            this.skillsListBox.MouseLeave += new System.EventHandler(this.skillsListBox_MouseLeave);
            this.skillsListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.skillsListBox_MouseDown);
            this.skillsListBox.DragOver += new DragEventHandler(this.skillsListBox_DragOver);
            this.skillsListBox.DragDrop += new DragEventHandler(this.skillsListBox_DragDrop);

            this.switchListBox.MouseLeave += new System.EventHandler(this.switchListBox_MouseLeave);
            this.switchListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.switchListBox_MouseDown);
            this.switchListBox.DragOver += new DragEventHandler(this.switchListBox_DragOver);
            this.switchListBox.DragDrop += new DragEventHandler(this.switchListBox_DragDrop);

            toolTip1.SetToolTip(switchAmmoCheckBox, "Intercala entre as munições");
            toolTip2.SetToolTip(textReinKey, "atalho rédea");
            toolTip3.SetToolTip(ammo1textBox, "atalho ammo 1");
            toolTip4.SetToolTip(ammo2textBox, "atalho ammo 2");
            subject.Attach(this);
        }

        private void InitializeTabConfig()
        {
            // Main Tabs
            var mainTabs = new List<Tuple<string, string>> {
                Tuple.Create("Skill Spammer", SKILL_SPAMMER_TAB),
                Tuple.Create("Autobuff - Skills", AUTOBOSS_SKILL_TAB),
                Tuple.Create("Autobuff - Stuffs", AUTOBOSS_STUFF_TAB),
                Tuple.Create("Auto Switch", AUTO_SWITCH_TAB),
                Tuple.Create("ATK x DEF", ATK_DEF_TAB),
                Tuple.Create("Macro Songs", MACRO_SONGS_TAB),
                Tuple.Create("Macro Switch", MACRO_SWITCH_TAB),
                Tuple.Create("Debuffs", DEBUFFS_TAB)
            };

            int y = 20;
            foreach (var tab in mainTabs)
            {
                CheckBox chk = new CheckBox { Text = tab.Item1, Name = tab.Item2, Location = new Point(10, y) };
                chk.CheckedChanged += OnTabVisibilityChanged;
                if (ProfileSingleton.GetCurrent().UserPreferences.TabVisibility.TryGetValue(chk.Name, out bool isVisible))
                {
                    chk.Checked = isVisible;
                }
                else
                {
                    chk.Checked = true; // Default to visible
                }
                this.gbTabs.Controls.Add(chk);
                y += 25;
            }

            // Secondary Tabs
            var secondaryTabs = new List<Tuple<string, string>> {
                Tuple.Create("Autopot", AUTOPOT_TAB),
                Tuple.Create("Yggdrasil", YGG_AUTOPOT_TAB),
                Tuple.Create("Skill Timer", SKILL_TIMER_TAB),
                Tuple.Create("AutoSwitch Heal", AUTO_SWITCH_HEAL_TAB)
            };

            y = 20;
            foreach (var tab in secondaryTabs)
            {
                CheckBox chk = new CheckBox { Text = tab.Item1, Name = tab.Item2, Location = new Point(150, y) };
                chk.CheckedChanged += OnTabVisibilityChanged;
                if (ProfileSingleton.GetCurrent().UserPreferences.TabVisibility.TryGetValue(chk.Name, out bool isVisible))
                {
                    chk.Checked = isVisible;
                }
                else
                {
                    chk.Checked = true; // Default to visible
                }
                this.gbTabs.Controls.Add(chk);
                y += 25;
            }

            // Autobuff Skills Granular
            var skillGroups = new List<Tuple<string, string>> { 
                Tuple.Create("Swordman", SWORDMAN_GROUP), Tuple.Create("Archer", ARCHER_GROUP),
                Tuple.Create("Mage", MAGE_GROUP), Tuple.Create("Thief", THIEF_GROUP),
                Tuple.Create("Acolyte", ACOLYTE_GROUP), Tuple.Create("Merchant", MERCHANT_GROUP),
                Tuple.Create("Taekwon", TAEKWON_GROUP), Tuple.Create("Ninja", NINJA_GROUP), Tuple.Create("Gunslinger", GUNSLINGER_GROUP)
            };

            int skillY = 20, skillX = 10;
            foreach (var group in skillGroups)
            {
                CheckBox chk = new CheckBox { Text = group.Item1, Name = group.Item2, Location = new Point(skillX, skillY), Width = 80 };
                chk.CheckedChanged += OnAutobuffSkillVisibilityChanged;
                if (ProfileSingleton.GetCurrent().UserPreferences.AutobuffSkillVisibility.TryGetValue(chk.Name, out bool isVisible))
                {
                    chk.Checked = isVisible;
                }
                else
                {
                    chk.Checked = true;
                }
                this.gbAutobuffSkills.Controls.Add(chk);
                skillY += 25;
                if (skillY > 70) { skillY = 20; skillX += 90; }
            }

            // Autobuff Stuffs Granular
            var stuffGroups = new List<Tuple<string, string>> {
                Tuple.Create("Foods", FOODS_GROUP), Tuple.Create("Potions", POTIONS_GROUP),
                Tuple.Create("Boxes", BOX_GROUP), Tuple.Create("Elementals", ELEMENTALS_GROUP),
                Tuple.Create("Scrolls", SCROLL_GROUP), Tuple.Create("Etc", ETC_GROUP)
            };

            int stuffY = 20, stuffX = 10;
            foreach (var group in stuffGroups)
            {
                CheckBox chk = new CheckBox { Text = group.Item1, Name = group.Item2, Location = new Point(stuffX, stuffY), Width = 80 };
                chk.CheckedChanged += OnAutobuffStuffVisibilityChanged;
                if (ProfileSingleton.GetCurrent().UserPreferences.AutobuffStuffVisibility.TryGetValue(chk.Name, out bool isVisible))
                {
                    chk.Checked = isVisible;
                }
                else
                {
                    chk.Checked = true;
                }
                this.gbAutobuffStuffs.Controls.Add(chk);
                stuffY += 25;
                if (stuffY > 45) { stuffY = 20; stuffX += 90; }
            }
        }

        private void OnTabVisibilityChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ProfileSingleton.GetCurrent().UserPreferences.TabVisibility[chk.Name] = chk.Checked;
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
            new Subject().Notify(new Utils.Message(MessageCode.TURN_OFF_TABS, null));
        }

        private void OnAutobuffSkillVisibilityChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ProfileSingleton.GetCurrent().UserPreferences.AutobuffSkillVisibility[chk.Name] = chk.Checked;
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
        }

        private void OnAutobuffStuffVisibilityChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ProfileSingleton.GetCurrent().UserPreferences.AutobuffStuffVisibility[chk.Name] = chk.Checked;
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.gbTabs.Controls.Clear();
                    this.gbAutobuffSkills.Controls.Clear();
                    this.gbAutobuffStuffs.Controls.Clear();
                    InitializeTabConfig();
                    UpdateUI(subject);
                    break;
                case MessageCode.ADDED_NEW_AUTOBUFF_SKILL:
                case MessageCode.ADDED_NEW_AUTOSWITCH_PETS:
                    UpdateUI(subject);
                    break;
            }
        }

        public void UpdateSwitch()
        {
            try
            {
                var buffsList = ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchMapping.Where(x => x.itemKey != Key.None && x.skillId != EffectStatusIDs.THURISAZ).Select(x => x.skillId).ToList();
                var newBuffList = ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchOrder;
                if (buffsList.Count > ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchOrder.Count)
                {

                    var newBuffs = buffsList.FindAll(item => !newBuffList.Contains(item));
                    foreach (var buff in newBuffs)
                    {
                        newBuffList.Add(buff);
                    }
                    ProfileSingleton.GetCurrent().AutoSwitch.SetAutoSwitchOrder(newBuffList);
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutoSwitch);
                }
                
                if (ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchOrder.Count > 0)
                {
                    var tessste = ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchOrder;
                    ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchOrder.RemoveAll(item => !buffsList.Contains(item));
                    buffsList = ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchOrder;
                }

                switchListBox.Items.Clear();

                foreach (var tswitch in buffsList)
                {
                    switchListBox.Items.Add(tswitch.ToDescriptionString());
                }
            }
            catch (Exception ex)
            {
                var teste = ex;
            }
        }

        public void UpdateUI(ISubject subject)
        {
            try
            {
                AutoBuffSkill currentBuffs = (AutoBuffSkill)(subject as Subject).Message.data;

                if (currentBuffs == null)
                {
                    currentBuffs = ProfileSingleton.GetCurrent().AutobuffSkill;
                }

                var buffsList = currentBuffs.buffMapping.Keys.ToList();
                skillsListBox.Items.Clear();

                foreach (var buff in buffsList)
                {
                    skillsListBox.Items.Add(buff.ToDescriptionString());
                }
                UpdateSwitch();

                this.chkStopBuffsOnCity.Checked = ProfileSingleton.GetCurrent().UserPreferences.stopBuffsCity;
                this.chkStopBuffsOnRein.Checked = ProfileSingleton.GetCurrent().UserPreferences.stopBuffsRein;
                this.chkStopHealOnCity.Checked = ProfileSingleton.GetCurrent().UserPreferences.stopHealCity;
                this.getOffReinCheckBox.Checked = ProfileSingleton.GetCurrent().UserPreferences.getOffRein;
                this.textReinKey.Text = ProfileSingleton.GetCurrent().UserPreferences.getOffReinKey.ToString();
                this.switchAmmoCheckBox.Checked = ProfileSingleton.GetCurrent().UserPreferences.switchAmmo;
                this.ammo1textBox.Text = ProfileSingleton.GetCurrent().UserPreferences.ammo1Key.ToString();
                this.ammo2textBox.Text = ProfileSingleton.GetCurrent().UserPreferences.ammo2Key.ToString();
                this.chkStopWithChat.Checked = ProfileSingleton.GetCurrent().UserPreferences.stopWithChat;
            }
            catch (Exception ex)
            {
                var exception = ex;
            }
        }

        private void skillsListBox_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                var autoBuffSkill = ProfileSingleton.GetCurrent().AutobuffSkill;
                var newOrderList = new List<EffectStatusIDs>();
                var orderedBuffList = skillsListBox.Items;
                Dictionary<EffectStatusIDs, Key> currentList = new Dictionary<EffectStatusIDs, Key>(autoBuffSkill.buffMapping);
                Dictionary<EffectStatusIDs, Key> newOrderedBuffList = new Dictionary<EffectStatusIDs, Key>();
                if (currentList.Count > 0)
                {

                    foreach (var buff in orderedBuffList)
                    {
                        var buffId = buff.ToString().ToEffectStatusId();
                        newOrderList.Add(buffId);
                        var findBuff = currentList.FirstOrDefault(t => t.Key == buffId);
                        newOrderedBuffList.Add(findBuff.Key, findBuff.Value);
                    }
                    ProfileSingleton.GetCurrent().UserPreferences.SetAutoBuffOrder(newOrderList);
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);

                    ProfileSingleton.GetCurrent().AutobuffSkill.ClearKeyMapping();
                    ProfileSingleton.GetCurrent().AutobuffSkill.setBuffMapping(newOrderedBuffList);
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutobuffSkill);

                    newOrderedBuffList.Clear();
                }
            }
            catch { }
        }
        private void skillsListBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.skillsListBox.SelectedItem == null) return;
            this.skillsListBox.DoDragDrop(this.skillsListBox.SelectedItem, DragDropEffects.Move);
        }

        private void skillsListBox_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void skillsListBox_DragDrop(object sender, DragEventArgs e)
        {
            Point point = skillsListBox.PointToClient(new Point(e.X, e.Y));
            int index = this.skillsListBox.IndexFromPoint(point);
            if (index < 0) index = this.skillsListBox.Items.Count - 1;
            object data = skillsListBox.SelectedItem;
            this.skillsListBox.Items.Remove(data);
            this.skillsListBox.Items.Insert(index, data);
        }

        private void switchListBox_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                var autoSwitchPets = ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchOrder;
                var newOrderList = new List<EffectStatusIDs>();
                var orderedBuffList = switchListBox.Items;
                List<EffectStatusIDs> currentList = ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchMapping.Where(x => x.itemKey != Key.None && x.skillId != EffectStatusIDs.THURISAZ).Select(x => x.skillId).ToList();

                List<EffectStatusIDs> newOrderedBuffList = new List<EffectStatusIDs>();
                if (currentList.Count > 0)
                {

                    foreach (var buff in orderedBuffList)
                    {
                        var buffId = buff.ToString().ToEffectStatusId();
                        newOrderList.Add(buffId);
                        var findBuff = currentList.FirstOrDefault(t => t == buffId);
                        newOrderedBuffList.Add(findBuff);
                    }
                    ProfileSingleton.GetCurrent().AutoSwitch.SetAutoSwitchOrder(newOrderList);
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutoSwitch);
                    newOrderedBuffList.Clear();
                }
            }
            catch { }
        }
        private void switchListBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.switchListBox.SelectedItem == null) return;
            this.switchListBox.DoDragDrop(this.switchListBox.SelectedItem, DragDropEffects.Move);
        }

        private void switchListBox_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void switchListBox_DragDrop(object sender, DragEventArgs e)
        {
            Point point = switchListBox.PointToClient(new Point(e.X, e.Y));
            int index = this.switchListBox.IndexFromPoint(point);
            if (index < 0) index = this.switchListBox.Items.Count - 1;
            object data = switchListBox.SelectedItem;
            this.switchListBox.Items.Remove(data);
            this.switchListBox.Items.Insert(index, data);
        }

        private void chkStopBuffsOnRein_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ProfileSingleton.GetCurrent().UserPreferences.stopBuffsRein = chk.Checked;
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
        }

        private void chkStopBuffsOnCity_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ProfileSingleton.GetCurrent().UserPreferences.stopBuffsCity = chk.Checked;
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
        }
        private void chkStopHealOnCity_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ProfileSingleton.GetCurrent().UserPreferences.stopHealCity = chk.Checked;
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
        }

        private void getOffReinCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ProfileSingleton.GetCurrent().UserPreferences.getOffRein = chk.Checked;
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
        }

        private void chkStopWithChat_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ProfileSingleton.GetCurrent().UserPreferences.stopWithChat = chk.Checked;
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
        }

        private void textReinKey_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                if (txtBox.Text.ToString() != String.Empty)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txtBox.Text.ToString());
                    ProfileSingleton.GetCurrent().UserPreferences.getOffReinKey = key;
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
                }
            }
            catch { }
        }

        private void switchAmmoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ProfileSingleton.GetCurrent().UserPreferences.switchAmmo = chk.Checked;
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
        }

        private void textAmmo1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                if (txtBox.Text.ToString() != String.Empty)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txtBox.Text.ToString());
                    ProfileSingleton.GetCurrent().UserPreferences.ammo1Key = key;
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
                }
            }
            catch { }
        }

        private void textAmmo2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                if (txtBox.Text.ToString() != String.Empty)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txtBox.Text.ToString());
                    ProfileSingleton.GetCurrent().UserPreferences.ammo2Key = key;
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
                }
            }
            catch { }
        }

    }
}
