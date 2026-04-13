using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace SwarmTheSpire.Relics
{
    public class RottenFleshRelic : StackableCatchRelicTemplate
    {
        public override RelicRarity Rarity => RelicRarity.Common;

        public override bool HasUponPickupEffect => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new HealVar(3m)];

        public override async Task AfterObtained()
        {
            await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
            await MergeDuplicateIntoSingleSlot();
        }
    }
}
