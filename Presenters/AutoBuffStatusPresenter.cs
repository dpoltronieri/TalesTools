using System;
using System.Linq;
using System.Windows.Input;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Presenters
{
    public class AutoBuffStatusPresenter
    {
        private IAutoBuffStatusView view;
        private StatusRecovery statusModel;
        private DebuffsRecovery debuffsModel;

        public AutoBuffStatusPresenter(IAutoBuffStatusView view, StatusRecovery statusModel, DebuffsRecovery debuffsModel)
        {
            this.view = view;
            this.statusModel = statusModel;
            this.debuffsModel = debuffsModel;
            BindEvents();
            UpdateView();
        }

        private void BindEvents()
        {
            this.view.StatusKeyChanged += (s, e) => {
                try {
                    Key k = (Key)Enum.Parse(typeof(Key), this.view.StatusKey);
                    this.statusModel.AddKeyToBuff(EffectStatusIDs.POISON, k);
                    this.statusModel.AddKeyToBuff(EffectStatusIDs.SILENCE, k);
                    this.statusModel.AddKeyToBuff(EffectStatusIDs.BLIND, k);
                    this.statusModel.AddKeyToBuff(EffectStatusIDs.CONFUSION, k);
                    this.statusModel.AddKeyToBuff(EffectStatusIDs.HALLUCINATIONWALK, k);
                    this.statusModel.AddKeyToBuff(EffectStatusIDs.HALLUCINATION, k);
                    this.statusModel.AddKeyToBuff(EffectStatusIDs.CURSE, k);
                    SaveStatus();
                } catch {}
            };

            this.view.NewStatusKeyChanged += (s, e) => {
                try {
                    Key k = (Key)Enum.Parse(typeof(Key), this.view.NewStatusKey);
                    this.statusModel.AddKeyToBuff(EffectStatusIDs.SLOW_CAST, k);
                    this.statusModel.AddKeyToBuff(EffectStatusIDs.CRITICALWOUND, k);
                    this.statusModel.AddKeyToBuff(EffectStatusIDs.FREEZING, k);
                    this.statusModel.AddKeyToBuff(EffectStatusIDs.MANDRAGORA, k);
                    this.statusModel.AddKeyToBuff(EffectStatusIDs.BURNING, k);
                    this.statusModel.AddKeyToBuff(EffectStatusIDs.DEEP_SLEEP, k);
                    SaveStatus();
                } catch {}
            };

            this.view.DebuffKeyChanged += (s, e) => {
                try {
                    Key k = (Key)Enum.Parse(typeof(Key), e.Key);
                    if (this.debuffsModel.debuffMapping.ContainsKey(e.Id))
                        this.debuffsModel.debuffMapping.Remove(e.Id);
                    
                    if (FormUtils.IsValidKey(k))
                        this.debuffsModel.debuffMapping.Add(e.Id, k);
                    
                    SaveDebuffs();
                } catch {}
            };
        }

        public void UpdateView()
        {
            this.view.StatusKey = this.statusModel.buffMapping.ContainsKey(EffectStatusIDs.SILENCE) ? this.statusModel.buffMapping[EffectStatusIDs.SILENCE].ToString() : "None";
            this.view.NewStatusKey = this.statusModel.buffMapping.ContainsKey(EffectStatusIDs.CRITICALWOUND) ? this.statusModel.buffMapping[EffectStatusIDs.CRITICALWOUND].ToString() : "None";

            foreach (var mapping in this.debuffsModel.debuffMapping)
            {
                this.view.SetDebuffKey(mapping.Key, mapping.Value.ToString());
            }
        }

        public void UpdateModels(StatusRecovery statusModel, DebuffsRecovery debuffsModel)
        {
            this.statusModel = statusModel;
            this.debuffsModel = debuffsModel;
            UpdateView();
        }

        private void SaveStatus()
        {
            ProfileSingleton.SetConfiguration(this.statusModel);
        }

        private void SaveDebuffs()
        {
            ProfileSingleton.SetConfiguration(this.debuffsModel);
        }
    }
}
