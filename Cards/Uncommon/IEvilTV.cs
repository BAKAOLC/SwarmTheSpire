using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class IEvilTV() : SwarmEvilPoolCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new PowerVar<MilesPower>(2m)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [
            HoverTipFactory.FromPower<VulnerablePower>(),
            HoverTipFactory.FromPower<MilesPower>(),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combatState = CombatState;
            ArgumentNullException.ThrowIfNull(combatState);
            var hittableEnemies = combatState.HittableEnemies;
            foreach (var item in hittableEnemies)
                await PowerCmd.Apply<MilesPower>(choiceContext, item, DynamicVars["MilesPower"].BaseValue,
                    Owner.Creature, this);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["MilesPower"].UpgradeValueBy(1m);
        }
    }
}
