using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class MilesDevotion()
        : SwarmEvilPoolCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
    {
        protected override HashSet<CardTag> CanonicalTags =>
        [
            CardTag.Strike,
            CardTag.Defend,
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(7m, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            var val = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            if (cardPlay.Target.HasPower<MilesPower>())
                await CreatureCmd.GainBlock(Owner.Creature,
                    val.Results.SelectMany(static r => r).Sum(static r => r.TotalDamage + r.OverkillDamage),
                    ValueProp.Move, cardPlay);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
        }
    }
}
