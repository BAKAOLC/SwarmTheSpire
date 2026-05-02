using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class EvilBite()
        : SwarmEvilPoolCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, true)
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Sly];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<MilesPower>(3m),
            new PowerVar<WeakPower>(3m),
        ];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [
            HoverTipFactory.FromPower<MilesPower>(),
            HoverTipFactory.FromPower<WeakPower>(),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            await PowerCmd.Apply<MilesPower>(choiceContext, cardPlay.Target, DynamicVars["MilesPower"].BaseValue,
                Owner.Creature, this, false);
            await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, DynamicVars["WeakPower"].BaseValue,
                Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["MilesPower"].UpgradeValueBy(1m);
            DynamicVars["WeakPower"].UpgradeValueBy(1m);
        }
    }
}
