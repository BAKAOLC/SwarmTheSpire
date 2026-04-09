using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Relics
{
    public class WeAreMilesRelic : SwarmRelicTemplate
    {
        public override RelicRarity Rarity => RelicRarity.Ancient;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new PowerVar<MilesPower>(1m)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<MilesPower>()];

        public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
            CombatState combatState)
        {
            if (side == Owner.Creature.Side)
            {
                Flash();
                await PowerCmd.Apply<MilesPower>(combatState.HittableEnemies, DynamicVars["MilesPower"].BaseValue,
                    Owner.Creature, null);
            }
        }
    }
}
