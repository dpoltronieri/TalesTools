using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Presenters
{
    public class ConfigPresenter
    {
        private IConfigView view;
        private UserPreferences preferences;
        private AutoBuffSkill autobuffSkill;
        private AutoSwitch autoSwitch;

        public ConfigPresenter(IConfigView view, UserPreferences preferences, AutoBuffSkill autobuffSkill, AutoSwitch autoSwitch)
        {
            this.view = view;
            this.preferences = preferences;
            this.autobuffSkill = autobuffSkill;
            this.autoSwitch = autoSwitch;
            BindEvents();
            Initialize();
        }

        private void Initialize()
        {
            this.view.ClearTabConfig();
            
            // Main Tabs
            AddTabs(new List<Tuple<string, string>> {
                Tuple.Create("Skill Spammer", "tabPageSpammer"),
                Tuple.Create("Autobuff - Skills", "tabPageAutobuffSkill"),
                Tuple.Create("Autobuff - Stuffs", "tabPageAutobuffStuff"),
                Tuple.Create("Auto Switch", "tabPageAutoSwitch"),
                Tuple.Create("ATK x DEF", "atkDef"),
                Tuple.Create("Macro Songs", "tabPageMacroSongs"),
                Tuple.Create("Macro Switch", "tabMacroSwitch"),
                Tuple.Create("Debuffs", "tabPageDebuffs")
            }, false);

            // Secondary Tabs
            AddTabs(new List<Tuple<string, string>> {
                Tuple.Create("Autopot", "tabPageAutopot"),
                Tuple.Create("Yggdrasil", "tabPageYggAutopot"),
                Tuple.Create("Skill Timer", "tabPageSkillTimer"),
                Tuple.Create("AutoSwitch Heal", "tabPageAutoSwitchHeal")
            }, true);

            // Autobuff Skill Groups
            var skillGroups = new List<Tuple<string, string>> {
                Tuple.Create("Swordman", "Swordman"), Tuple.Create("Archer", "Archer"),
                Tuple.Create("Mage", "Mage"), Tuple.Create("Thief", "Thief"),
                Tuple.Create("Acolyte", "Acolyte"), Tuple.Create("Merchant", "Merchant"),
                Tuple.Create("Taekwon", "Taekwon"), Tuple.Create("Ninja", "Ninja"), Tuple.Create("Gunslinger", "Gunslinger")
            };
            foreach(var g in skillGroups) {
                bool isVisible = !this.preferences.AutobuffSkillVisibility.ContainsKey(g.Item2) || this.preferences.AutobuffSkillVisibility[g.Item2];
                this.view.AddAutobuffSkillVisibilityControl(g.Item1, g.Item2, isVisible);
            }

            // Autobuff Stuff Groups
            var stuffGroups = new List<Tuple<string, string>> {
                Tuple.Create("Foods", "FoodsGP"), Tuple.Create("Potions", "PotionsGP"),
                Tuple.Create("Boxes", "BoxesGP"), Tuple.Create("Elementals", "ElementalsGP"),
                Tuple.Create("Scrolls", "ScrollBuffsGP"), Tuple.Create("Etc", "EtcGP")
            };
            foreach (var g in stuffGroups) {
                bool isVisible = !this.preferences.AutobuffStuffVisibility.ContainsKey(g.Item2) || this.preferences.AutobuffStuffVisibility[g.Item2];
                this.view.AddAutobuffStuffVisibilityControl(g.Item1, g.Item2, isVisible);
            }

            UpdateView();
        }

        private void AddTabs(List<Tuple<string, string>> tabs, bool isSecondary)
        {
            foreach (var tab in tabs)
            {
                bool isVisible = !this.preferences.TabVisibility.ContainsKey(tab.Item2) || this.preferences.TabVisibility[tab.Item2];
                this.view.AddTabVisibilityControl(tab.Item1, tab.Item2, isVisible, isSecondary);
            }
        }

        private void BindEvents()
        {
            this.view.PreferenceChanged += (s, e) => {
                this.preferences.stopBuffsCity = this.view.StopBuffsCity;
                this.preferences.stopBuffsRein = this.view.StopBuffsRein;
                this.preferences.stopHealCity = this.view.StopHealCity;
                this.preferences.getOffRein = this.view.GetOffRein;
                this.preferences.switchAmmo = this.view.SwitchAmmo;
                this.preferences.stopWithChat = this.view.StopWithChat;
                SavePreferences();
            };

            this.view.ReinKeyChanged += (s, e) => {
                try { this.preferences.getOffReinKey = (Key)Enum.Parse(typeof(Key), this.view.ReinKey); SavePreferences(); } catch {}
            };
            this.view.Ammo1KeyChanged += (s, e) => {
                try { this.preferences.ammo1Key = (Key)Enum.Parse(typeof(Key), this.view.Ammo1Key); SavePreferences(); } catch {}
            };
            this.view.Ammo2KeyChanged += (s, e) => {
                try { this.preferences.ammo2Key = (Key)Enum.Parse(typeof(Key), this.view.Ammo2Key); SavePreferences(); } catch {}
            };

            this.view.VisibilityChanged += (s, e) => {
                if (e.Type == "Tab") {
                    this.preferences.TabVisibility[e.Name] = e.IsChecked;
                    new Subject().Notify(new Message(MessageCode.TURN_OFF_TABS, null));
                } else if (e.Type == "SkillGroup") {
                    this.preferences.AutobuffSkillVisibility[e.Name] = e.IsChecked;
                } else if (e.Type == "StuffGroup") {
                    this.preferences.AutobuffStuffVisibility[e.Name] = e.IsChecked;
                }
                SavePreferences();
            };

            this.view.OrderChanged += (s, e) => {
                if (e.Type == "Skills") {
                    List<EffectStatusIDs> newOrder = e.OrderedNames.Select(n => n.ToEffectStatusId()).ToList();
                    this.preferences.SetAutoBuffOrder(newOrder);
                    SavePreferences();

                    Dictionary<EffectStatusIDs, Key> newMapping = new Dictionary<EffectStatusIDs, Key>();
                    foreach(var id in newOrder) {
                        if (this.autobuffSkill.buffMapping.ContainsKey(id))
                            newMapping.Add(id, this.autobuffSkill.buffMapping[id]);
                    }
                    this.autobuffSkill.ClearKeyMapping();
                    this.autobuffSkill.setBuffMapping(newMapping);
                    ProfileSingleton.SetConfiguration(this.autobuffSkill);
                } else if (e.Type == "Switch") {
                    List<EffectStatusIDs> newOrder = e.OrderedNames.Select(n => n.ToEffectStatusId()).ToList();
                    this.autoSwitch.SetAutoSwitchOrder(newOrder);
                    ProfileSingleton.SetConfiguration(this.autoSwitch);
                }
            };
        }

        public void UpdateView()
        {
            this.view.StopBuffsCity = this.preferences.stopBuffsCity;
            this.view.StopBuffsRein = this.preferences.stopBuffsRein;
            this.view.StopHealCity = this.preferences.stopHealCity;
            this.view.GetOffRein = this.preferences.getOffRein;
            this.view.ReinKey = this.preferences.getOffReinKey.ToString();
            this.view.SwitchAmmo = this.preferences.switchAmmo;
            this.view.Ammo1Key = this.preferences.ammo1Key.ToString();
            this.view.Ammo2Key = this.preferences.ammo2Key.ToString();
            this.view.StopWithChat = this.preferences.stopWithChat;

            // Update Skills List
            this.view.SetSkillsList(this.autobuffSkill.buffMapping.Keys.Select(k => k.ToDescriptionString()).ToList());

            // Update Switch List
            var buffsList = this.autoSwitch.autoSwitchMapping
                .Where(x => x.itemKey != Key.None && x.skillId != EffectStatusIDs.THURISAZ)
                .Select(x => x.skillId).ToList();
            
            var currentOrder = this.autoSwitch.autoSwitchOrder;
            // Sync order if new items added
            if (buffsList.Count > currentOrder.Count) {
                foreach(var buff in buffsList.Where(b => !currentOrder.Contains(b)))
                    currentOrder.Add(buff);
                this.autoSwitch.SetAutoSwitchOrder(currentOrder);
                ProfileSingleton.SetConfiguration(this.autoSwitch);
            }
            currentOrder.RemoveAll(item => !buffsList.Contains(item));
            
            this.view.SetSwitchList(currentOrder.Select(k => k.ToDescriptionString()).ToList());
        }

        public void UpdateModels(UserPreferences prefs, AutoBuffSkill skill, AutoSwitch @switch)
        {
            this.preferences = prefs;
            this.autobuffSkill = skill;
            this.autoSwitch = @switch;
            UpdateView();
        }

        private void SavePreferences()
        {
            ProfileSingleton.SetConfiguration(this.preferences);
        }
    }
}