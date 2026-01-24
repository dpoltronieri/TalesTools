using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using _4RTools.Model;
using _4RTools.Utils;
using static _4RTools.Model.AutoSwitch;

namespace _4RTools.Presenters
{
    public class AutoSwitchPresenter
    {
        private IAutoSwitchView view;
        private AutoSwitch model;

        public AutoSwitchPresenter(IAutoSwitchView view, AutoSwitch model)
        {
            this.view = view;
            this.model = model;
            BindEvents();
            UpdateView();
        }

        private void BindEvents()
        {
            this.view.DelayChanged += (s, e) => {
                try {
                    this.model.delay = int.Parse(this.view.Delay);
                    Save();
                } catch {}
            };

            this.view.SwitchDelayChanged += (s, e) => {
                try {
                    this.model.switchEquipDelay = int.Parse(this.view.SwitchDelay);
                    Save();
                } catch {}
            };

            this.view.KeyChanged += (s, e) => {
                try {
                    Key key = (Key)Enum.Parse(typeof(Key), e.Key);
                    AutoSwitchConfig config = this.model.autoSwitchMapping.Find(cfg => cfg.skillId == e.SkillId);
                    
                    if (config != null)
                    {
                        switch (e.Type)
                        {
                            case AutoSwitch.item:
                                config.itemKey = key;
                                break;
                            case AutoSwitch.skill:
                                config.skillKey = key;
                                break;
                            case AutoSwitch.nextItem:
                                config.nextItemKey = key;
                                break;
                        }
                    }
                    else
                    {
                        this.model.autoSwitchMapping.Add(new AutoSwitchConfig(e.SkillId, key, e.Type));
                    }
                    Save();
                } catch {}
            };

            this.view.AddNewSkill += (s, id) => {
                OnAddNewSkill(id);
            };
        }
        
        // Overload for AddNewSkill handling
        public void OnAddNewSkill(EffectStatusIDs id)
        {
             var _autoSwitchGenericMapping = this.model.autoSwitchGenericMapping;
            if (!_autoSwitchGenericMapping.Exists(x => x.skillId == id))
            {
                _autoSwitchGenericMapping.Add(new AutoSwitchConfig(id, Key.None));
                Save();
            }
        }

        public void UpdateView()
        {
            this.view.Delay = this.model.delay.ToString();
            this.view.SwitchDelay = this.model.switchEquipDelay.ToString();

            foreach (var config in this.model.autoSwitchMapping)
            {
                this.view.SetKey(config.skillId, AutoSwitch.item, config.itemKey.ToString());
                this.view.SetKey(config.skillId, AutoSwitch.skill, config.skillKey.ToString());
                this.view.SetKey(config.skillId, AutoSwitch.nextItem, config.nextItemKey.ToString());
            }
        }

        public void UpdateModel(AutoSwitch newModel)
        {
            this.model = newModel;
            UpdateView();
        }

        private void Save()
        {
            ProfileSingleton.SetConfiguration(this.model);
        }
    }
}
