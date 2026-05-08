using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Content;
using SwarmTheSpire;
using SwarmTheSpire.Powers;
using SwarmTheSpire.Relics;

namespace SwarmTheSpire.Cards
{
    public sealed class HarpoonImpale()
        : SwarmEvilPoolCard(1, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy, true)
    {
        protected override IEnumerable<string> RegisteredKeywordIds =>
            [ModContentRegistry.GetQualifiedKeywordId(Const.ModId, "harpoon")];

        protected override IEnumerable<string> RegisteredCardTagIds => [SwarmCardTagIds.Harpoon];

        public override TargetType TargetType => HasQueenPower ? TargetType.AllEnemies : TargetType.AnyEnemy;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CalculationBaseVar(21m),
            new ExtraDamageVar(3m),
            new CalculatedDamageVar(ValueProp.Move).WithMultiplier((_, target) =>
                target?.GetPowerAmount<MilesPower>() ?? 0),
        ];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<MilesPower>()];

        private bool HasQueenPower => CombatManager.Instance.IsInProgress && Owner.Creature.HasPower<QueenPower>();

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            var shouldTriggerFatal = cardPlay.Target.Powers.All(static power => power.ShouldOwnerDeathTriggerFatal());
            var combatState = CombatState;
            var attack = await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target)
                .Execute(choiceContext);
            TryIncrementCatch(shouldTriggerFatal, attack);

            if (!HasQueenPower)
                return;

            ArgumentNullException.ThrowIfNull(combatState);
            var queenPowerCount = Owner.Creature.GetPowerAmount<QueenPower>();
            for (var i = 0; i < queenPowerCount; i++)
            {
                foreach (var hittableEnemy in combatState.HittableEnemies)
                {
                    var followUpAttack = await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this)
                        .Targeting(hittableEnemy)
                        .Execute(choiceContext);
                    TryIncrementCatch(shouldTriggerFatal, followUpAttack);
                }
            }

            return;

            void TryIncrementCatch(bool canTriggerFatal, AttackCommand attackCommand)
            {
                if (!canTriggerFatal ||
                    !attackCommand.Results.SelectMany(static r => r)
                        .Any(static result => result is { OverkillDamage: 0, WasTargetKilled: true }))
                    return;

                MilesRelic.TryIncrementCatch(Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.CalculationBase.UpgradeValueBy(4m);
            DynamicVars.ExtraDamage.UpgradeValueBy(1m);
            EnergyCost.UpgradeBy(-1);
        }
    }
}
