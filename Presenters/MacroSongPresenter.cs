using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using _4RTools.Model;

namespace _4RTools.Presenters
{
    public class MacroSongPresenter
    {
        private IMacroSongView view;
        private Macro model;

        public MacroSongPresenter(IMacroSongView view, Macro model)
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

                    if (!string.IsNullOrEmpty(e.Tag)) {
                        string[] inputTag = e.Tag.Split(':');
                        string type = inputTag[1];
                        switch (type) {
                            case "Dagger": chainConfig.daggerKey = key; break;
                            case "Instrument": chainConfig.instrumentKey = key; break;
                            case "Trigger": chainConfig.trigger = key; break;
                        }
                    } else {
                        chainConfig.macroEntries[e.ControlName] = new MacroKey(key, chainConfig.delay);
                    }
                    Save();
                } catch {}
            };

            this.view.DelayChanged += (s, e) => {
                try {
                    ChainConfig chainConfig = this.model.chainConfigs.Find(config => config.id == e.LaneId);
                    if (chainConfig != null) {
                        chainConfig.delay = e.Delay;
                        foreach (var entry in chainConfig.macroEntries.Values) {
                            entry.delay = chainConfig.delay;
                        }
                        Save();
                    }
                } catch {}
            };

            this.view.ResetRequested += (s, e) => {
                try {
                    this.model.ResetMacro(e.LaneId);
                    Save();
                    UpdateLane(e.LaneId);
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
            if (config == null) return; // Should not happen if model is initialized correctly

            this.view.UpdateControl(id, "inTriggerMacro" + id, config.trigger.ToString());
            this.view.UpdateControl(id, "inDaggerMacro" + id, config.daggerKey.ToString());
            this.view.UpdateControl(id, "inInstrumentMacro" + id, config.instrumentKey.ToString());
            
            foreach (var entry in config.macroEntries)
            {
                this.view.UpdateControl(id, entry.Key, entry.Value.key.ToString());
            }
            
            this.view.UpdateDelay(id, config.delay);
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
