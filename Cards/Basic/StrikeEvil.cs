using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using SwarmTheSpire.Character;

namespace SwarmTheSpire.Cards
{
    [RegisterCharacterStarterCard(typeof(EvilCharacter), 4)]
    public sealed class StrikeEvil()
        : SwarmEvilPoolCard(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, true)
    {
        protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(6m, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
