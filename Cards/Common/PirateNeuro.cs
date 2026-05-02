using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace SwarmTheSpire.Cards
{
    public sealed class PirateNeuro()
        : SwarmEvilPoolCard(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
    {
        protected override HashSet<CardTag> CanonicalTags => [];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(5m, ValueProp.Move)];

        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                if (CombatState == null) return false;
                return CombatState.HittableEnemies.Any(delegate(Creature e)
                {
                    var monster = e.Monster;
                    return monster is { IntendsToAttack: true };
                });
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            ArgumentNullException.ThrowIfNull(target);
            var monster = target.Monster;
            ArgumentNullException.ThrowIfNull(monster);
            var num = !monster.IntendsToAttack ? 1 : 2;
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(num).FromCard(this)
                .Targeting(target)
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
        }
    }
}
