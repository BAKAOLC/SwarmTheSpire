using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class EvilStrike()
        : SwarmCardTemplate(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
    {
        protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<MilesPower>()];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(9m, ValueProp.Move),
            new PowerVar<MilesPower>(1m),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            await PowerCmd.Apply<MilesPower>(cardPlay.Target, DynamicVars["MilesPower"].BaseValue, Owner.Creature,
                this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(1m);
            DynamicVars["MilesPower"].UpgradeValueBy(1m);
        }
    }
}
