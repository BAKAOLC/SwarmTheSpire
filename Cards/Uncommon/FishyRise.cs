using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SwarmTheSpire.Data;

namespace SwarmTheSpire.Cards
{
    public sealed class FishyRise()
        : SwarmEvilPoolCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(1m, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var globalCatchesCount = CatchesData.Instance.GlobalCatchesCount;
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(globalCatchesCount)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
    }
}
