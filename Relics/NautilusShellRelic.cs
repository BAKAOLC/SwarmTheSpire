using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SwarmTheSpire.Relics
{
    public class NautilusShellRelic : StackableCatchRelicTemplate
    {
        public override RelicRarity Rarity => RelicRarity.Common;

        public override bool HasUponPickupEffect => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new PowerVar<BufferPower>(1m)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<BufferPower>()];

        public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
            ICombatState combatState)
        {
            if (side != Owner.Creature.Side || combatState.RoundNumber > 1)
                return;

            Flash();
            for (var i = 0; i < CatchStacks; i++)
                await PowerCmd.Apply<BufferPower>(choiceContext, Owner.Creature,
                    DynamicVars["BufferPower"].BaseValue, Owner.Creature, null);
        }

        public override async Task AfterObtained()
        {
            await MergeDuplicateIntoSingleSlot();
        }
    }
}
