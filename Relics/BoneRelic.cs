using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SwarmTheSpire.Relics
{
    public class BoneRelic : StackableCatchRelicTemplate
    {
        public override RelicRarity Rarity => RelicRarity.Common;

        public override bool HasUponPickupEffect => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new PowerVar<PlatingPower>(1m)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<PlatingPower>()];

        public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
            CombatState combatState)
        {
            if (side != Owner.Creature.Side || combatState.RoundNumber > 1)
                return;

            Flash();
            for (var i = 0; i < CatchStacks; i++)
                await PowerCmd.Apply<PlatingPower>(Owner.Creature, DynamicVars["PlatingPower"].BaseValue,
                    Owner.Creature, null);
        }

        public override async Task AfterObtained()
        {
            await MergeDuplicateIntoSingleSlot();
        }
    }
}
