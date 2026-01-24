using System;
using System.Collections.Generic;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Presenters
{
    public class SkillAutoBuffPresenter
    {
        private ISkillAutoBuffView view;
        private AutoBuffSkill model;

        public SkillAutoBuffPresenter(ISkillAutoBuffView view, AutoBuffSkill model)
        {
            this.view = view;
            this.model = model;
            BindEvents();
            UpdateView();
        }

        private void BindEvents()
        {
            this.view.DelayChanged += (s, e) => {
                this.model.delay = this.view.Delay;
                Save();
            };

            this.view.ResetRequested += (s, e) => {
                this.model.ClearKeyMapping();
                Save();
                this.view.Delay = 100;
                UpdateView();
            };
        }

        public void UpdateView()
        {
            this.view.Delay = this.model.delay;
            this.view.UpdateUI(new Dictionary<EffectStatusIDs, System.Windows.Input.Key>(this.model.buffMapping));
        }

        public void UpdateModel(AutoBuffSkill newModel)
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
