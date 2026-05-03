using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using SwarmTheSpire;

namespace SwarmTheSpire.Cards
{
    public sealed class NekoEvil() : SwarmEvilPoolCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
    {
        protected override IEnumerable<string> RegisteredCardTagIds => [SwarmCardTagIds.Evz];

        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                var combatState = CombatState;
                return combatState != null && combatState.HittableEnemies.Any(e => e.HasPower<WeakPower>());
            }
        }

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<WeakPower>()];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(2m, ValueProp.Move),
            new RepeatVar(4),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            var num = cardPlay.Target.HasPower<WeakPower>();
            var damage = DynamicVars.Damage.BaseValue;
            if (num) damage += 1m;
            await DamageCmd.Attack(damage).FromCard(this).Targeting(cardPlay.Target)
                .Execute(choiceContext);
            var combatState = CombatState;
            ArgumentNullException.ThrowIfNull(combatState);
            await DamageCmd.Attack(damage).WithHitCount(DynamicVars.Repeat.IntValue).FromCard(this)
                .TargetingRandomOpponents(combatState)
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(1m);
        }
    }
}
