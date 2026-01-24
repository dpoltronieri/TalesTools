using System;
using System.Collections.Generic;
using System.Windows.Input;
using _4RTools.Model;

namespace _4RTools.Presenters
{
    public class SkillTimerPresenter
    {
        private ISkillTimerView view;
        private AutoRefreshSpammer model;

        public SkillTimerPresenter(ISkillTimerView view, AutoRefreshSpammer model)
        {
            this.view = view;
            this.model = model;
            BindEvents();
            UpdateView();
        }

        private void BindEvents()
        {
            this.view.KeyChanged += (s, e) => {
                try {
                    Key key = (Key)Enum.Parse(typeof(Key), e.Key);
                    if (this.model.skillTimer.ContainsKey(e.LaneId)) {
                        this.model.skillTimer[e.LaneId].key = key;
                    } else {
                        this.model.skillTimer.Add(e.LaneId, new MacroKey(key, 5));
                    }
                    Save();
                } catch {}
            };

            this.view.DelayChanged += (s, e) => {
                try {
                    if (this.model.skillTimer.ContainsKey(e.LaneId)) {
                        this.model.skillTimer[e.LaneId].delay = e.Delay;
                    } else {
                        this.model.skillTimer.Add(e.LaneId, new MacroKey(Key.None, e.Delay));
                    }
                    Save();
                } catch {}
            };
        }

        public void UpdateView()
        {
            foreach (var timer in this.model.skillTimer)
            {
                this.view.UpdateControl(timer.Key, "txtSkillTimerKey" + timer.Key, timer.Value.key.ToString());
                this.view.UpdateDelay(timer.Key, timer.Value.delay);
            }
        }

        public void UpdateModel(AutoRefreshSpammer newModel)
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
