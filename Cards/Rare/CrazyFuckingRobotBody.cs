using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class CrazyFuckingRobotBody()
        : SwarmEvilPoolCard(3, CardType.Power, CardRarity.Rare, TargetType.Self, true)
    {
        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [
            HoverTipFactory.FromPower<MilesPower>(),
            HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<CrazyFuckingRobotBodyPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
            var combatState = CombatState;
            ArgumentNullException.ThrowIfNull(combatState);
            var hittableEnemies = combatState.HittableEnemies;
            foreach (var item in hittableEnemies)
                if (item.IsAlive)
                    await PowerCmd.Apply<MilesPower>(choiceContext, item, 3m, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
