using System;
using System.Linq;
using System.Windows.Input;
using _4RTools.Model;

namespace _4RTools.Presenters
{
    public class ATKDEFPresenter
    {
        private IATKDEFView view;
        private ATKDEFMode model;

        public ATKDEFPresenter(IATKDEFView view, ATKDEFMode model)
        {
            this.view = view;
            this.model = model;
            BindEvents();
            UpdateView();
        }

        private void BindEvents()
        {
            this.view.ChangeRequested += (s, e) => {
                try {
                    EquipConfig config = this.model.equipConfigs.FirstOrDefault(x => x.id == e.LaneId);
                    if (config == null) return;

                    switch (e.ControlType)
                    {
                        case "spammerKey":
                            config.keySpammer = (Key)Enum.Parse(typeof(Key), e.Value);
                            break;
                        case "spammerDelay":
                            config.ahkDelay = int.Parse(e.Value);
                            break;
                        case "switchDelay":
                            config.switchDelay = int.Parse(e.Value);
                            break;
                        case "spammerClick":
                            config.keySpammerWithClick = bool.Parse(e.Value);
                            break;
                        case "ATK":
                        case "DEF":
                            Key key = (Key)Enum.Parse(typeof(Key), e.Value);
                            this.model.AddSwitchItem(e.LaneId, e.ControlName, key, e.ControlType);
                            break;
                    }
                    Save();
                } catch {}
            };
        }

        public void UpdateView()
        {
            foreach (var config in this.model.equipConfigs)
            {
                this.view.UpdateControlValue(config.id, "spammerKey", config.keySpammer.ToString());
                this.view.UpdateControlValue(config.id, "spammerDelay", config.ahkDelay.ToString());
                this.view.UpdateControlValue(config.id, "switchDelay", config.switchDelay.ToString());
                this.view.UpdateControlValue(config.id, "spammerClick", config.keySpammerWithClick.ToString());

                for (int i = 1; i <= 6; i++)
                {
                    string atkControlName = $"in{config.id}Atk{i}";
                    string defControlName = $"in{config.id}Def{i}";
                    
                    string atkVal = config.atkKeys.ContainsKey(atkControlName) ? config.atkKeys[atkControlName].ToString() : "None";
                    string defVal = config.defKeys.ContainsKey(defControlName) ? config.defKeys[defControlName].ToString() : "None";

                    this.view.UpdateEquipValue(config.id, "Atk", i, atkVal);
                    this.view.UpdateEquipValue(config.id, "Def", i, defVal);
                }
            }
        }

        public void UpdateModel(ATKDEFMode newModel)
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
