using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using _4RTools.Model;

namespace _4RTools.Presenters
{
    public class MacroSwitchPresenter
    {
        private IMacroSwitchView view;
        private Macro model;

        public MacroSwitchPresenter(IMacroSwitchView view, Macro model)
        {
            this.view = view;
            this.model = model;
            BindEvents();
            UpdateView();
        }

        private void BindEvents()
        {
            this.view.MacroChanged += (s, e) => {
                try {
                    Key key = (Key)Enum.Parse(typeof(Key), e.Text);
                    ChainConfig chainConfig = this.model.chainConfigs.Find(config => config.id == e.LaneId);
                    
                    if (chainConfig == null) {
                        this.model.chainConfigs.Add(new ChainConfig(e.LaneId, Key.None));
                        chainConfig = this.model.chainConfigs.Find(config => config.id == e.LaneId);
                    }

                    // Default delay 50 if new
                    int currentDelay = chainConfig.macroEntries.ContainsKey(e.ControlName) ? chainConfig.macroEntries[e.ControlName].delay : 50;
                    chainConfig.macroEntries[e.ControlName] = new MacroKey(key, currentDelay);

                    // Check if it's the trigger (first input)
                    if (Regex.IsMatch(e.ControlName, $"in1mac{e.LaneId}")) {
                        chainConfig.trigger = key;
                    }

                    Save();
                } catch {}
            };

            this.view.DelayChanged += (s, e) => {
                try {
                    ChainConfig chainConfig = this.model.chainConfigs.Find(config => config.id == e.LaneId);
                    // Extract base name from delay control name (e.g. "in1mac1delay" -> "in1mac1")
                    string baseName = e.ControlName.Replace("delay", "");
                    
                    if (chainConfig != null && chainConfig.macroEntries.ContainsKey(baseName)) {
                        chainConfig.macroEntries[baseName].delay = e.Delay;
                        Save();
                    }
                } catch {}
            };

            this.view.ClickChanged += (s, e) => {
                try {
                    ChainConfig chainConfig = this.model.chainConfigs.Find(config => config.id == e.LaneId);
                    // Extract base name from click control name (e.g. "in1mac1click" -> "in1mac1")
                    string baseName = e.ControlName.Replace("click", "");

                    if (chainConfig != null && chainConfig.macroEntries.ContainsKey(baseName)) {
                        chainConfig.macroEntries[baseName].hasClick = e.Checked;
                        Save();
                    }
                } catch {}
            };
        }

        public void UpdateView()
        {
            foreach (var config in this.model.chainConfigs)
            {
                UpdateLane(config.id);
            }
        }

        private void UpdateLane(int id)
        {
            ChainConfig config = this.model.chainConfigs.Find(c => c.id == id);
            if (config == null) return;

            foreach (var entry in config.macroEntries)
            {
                this.view.UpdateControl(id, entry.Key, entry.Value.key.ToString());
                this.view.UpdateDelay(id, entry.Key + "delay", entry.Value.delay);
                this.view.UpdateClick(id, entry.Key + "click", entry.Value.hasClick);
            }
        }

        public void UpdateModel(Macro newModel)
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
