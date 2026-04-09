using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class HarpoonThrust()
        : SwarmCardTemplate(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, true)
    {
        protected override HashSet<CardTag> CanonicalTags => [];

        public override TargetType TargetType => HasQueenPower ? TargetType.AllEnemies : TargetType.AnyEnemy;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CalculationBaseVar(10m),
            new ExtraDamageVar(2m),
            new CalculatedDamageVar(ValueProp.Move).WithMultiplier((_, target) =>
                target?.GetPowerAmount<MilesPower>() ?? 0),
        ];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<MilesPower>()];

        private bool HasQueenPower => CombatManager.Instance.IsInProgress && Owner.Creature.HasPower<QueenPower>();

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combatState = CombatState;
            if (HasQueenPower)
            {
                ArgumentNullException.ThrowIfNull(combatState);
                foreach (var hittableEnemy in combatState.HittableEnemies)
                    await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).Targeting(hittableEnemy)
                        .Execute(choiceContext);
            }
            else
            {
                ArgumentNullException.ThrowIfNull(cardPlay.Target);
                await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target)
                    .Execute(choiceContext);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.CalculationBase.UpgradeValueBy(1m);
            DynamicVars.ExtraDamage.UpgradeValueBy(1m);
        }
    }
}
