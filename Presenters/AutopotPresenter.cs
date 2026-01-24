using System;
using System.Windows.Input;
using _4RTools.Model;

namespace _4RTools.Presenters
{
    public class AutopotPresenter
    {
        private IAutopotView view;
        private Autopot autopot;

        public AutopotPresenter(IAutopotView view, Autopot autopot)
        {
            this.view = view;
            this.autopot = autopot;
            BindEvents();
            UpdateView();
        }

        private void BindEvents()
        {
            this.view.HpKeyChanged += (s, e) => {
                try {
                    this.autopot.hpKey = (Key)Enum.Parse(typeof(Key), this.view.HpKey);
                    Save();
                } catch {}
            };
            this.view.SpKeyChanged += (s, e) => {
                try {
                    this.autopot.spKey = (Key)Enum.Parse(typeof(Key), this.view.SpKey);
                    Save();
                } catch {}
            };
            this.view.HpPercentChanged += (s, e) => {
                try {
                    this.autopot.hpPercent = int.Parse(this.view.HpPercent);
                    Save();
                } catch {}
            };
            this.view.SpPercentChanged += (s, e) => {
                try {
                    this.autopot.spPercent = int.Parse(this.view.SpPercent);
                    Save();
                } catch {}
            };
            this.view.DelayChanged += (s, e) => {
                try {
                    this.autopot.delay = int.Parse(this.view.Delay);
                    Save();
                } catch {}
            };
            this.view.HpEquipBeforeChanged += (s, e) => {
                try {
                    this.autopot.hpEquipBefore = (Key)Enum.Parse(typeof(Key), this.view.HpEquipBefore);
                    Save();
                } catch {}
            };
            this.view.HpEquipAfterChanged += (s, e) => {
                try {
                    this.autopot.hpEquipAfter = (Key)Enum.Parse(typeof(Key), this.view.HpEquipAfter);
                    Save();
                } catch {}
            };
            this.view.StopWitchFCChanged += (s, e) => {
                this.autopot.stopWitchFC = this.view.StopWitchFC;
                Save();
            };
            this.view.FirstHealChanged += (s, e) => {
                this.autopot.firstHeal = this.view.FirstHeal;
                Save();
            };
        }

        public void UpdateView()
        {
            this.view.HpKey = this.autopot.hpKey.ToString();
            this.view.SpKey = this.autopot.spKey.ToString();
            this.view.HpPercent = this.autopot.hpPercent.ToString();
            this.view.SpPercent = this.autopot.spPercent.ToString();
            this.view.Delay = this.autopot.delay.ToString();
            this.view.HpEquipBefore = this.autopot.hpEquipBefore.ToString();
            this.view.HpEquipAfter = this.autopot.hpEquipAfter.ToString();
            this.view.StopWitchFC = this.autopot.stopWitchFC;
            this.view.FirstHeal = this.autopot.firstHeal;
        }

        private void Save()
        {
            ProfileSingleton.SetConfiguration(this.autopot);
        }
        
        public void UpdateModel(Autopot newModel)
        {
            this.autopot = newModel;
            UpdateView();
        }
    }
}
