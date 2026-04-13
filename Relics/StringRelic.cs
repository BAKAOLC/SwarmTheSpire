using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace SwarmTheSpire.Relics
{
    public class StringRelic : SwarmRelicTemplate
    {
        public override RelicRarity Rarity => RelicRarity.Common;

        public override bool HasUponPickupEffect => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new BlockVar(2m, ValueProp.Move)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.Static(StaticHoverTip.Block)];

        public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
        {
            if (side != Owner.Creature.Side || combatState.RoundNumber > 1)
                return;

            Flash();
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, (CardPlay?)null);
        }
    }
}
