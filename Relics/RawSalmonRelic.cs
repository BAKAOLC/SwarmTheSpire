using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace SwarmTheSpire.Relics
{
    public class RawSalmonRelic : StackableCatchRelicTemplate
    {
        public override RelicRarity Rarity => RelicRarity.Common;

        public override bool HasUponPickupEffect => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new MaxHpVar(8m)];

        public override async Task AfterObtained()
        {
            await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
            await MergeDuplicateIntoSingleSlot();
        }
    }
}
