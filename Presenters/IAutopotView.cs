using System;

namespace _4RTools.Presenters
{
    public interface IAutopotView
    {
        string HpKey { get; set; }
        string SpKey { get; set; }
        string HpPercent { get; set; }
        string SpPercent { get; set; }
        string Delay { get; set; }
        string HpEquipBefore { get; set; }
        string HpEquipAfter { get; set; }
        string SpEquipBefore { get; set; }
        string SpEquipAfter { get; set; }
        bool StopWitchFC { get; set; }
        string FirstHeal { get; set; }

        event EventHandler HpKeyChanged;
        event EventHandler SpKeyChanged;
        event EventHandler HpPercentChanged;
        event EventHandler SpPercentChanged;
        event EventHandler DelayChanged;
        event EventHandler HpEquipBeforeChanged;
        event EventHandler HpEquipAfterChanged;
        event EventHandler SpEquipBeforeChanged;
        event EventHandler SpEquipAfterChanged;
        event EventHandler StopWitchFCChanged;
        event EventHandler FirstHealChanged;
    }
}
