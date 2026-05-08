using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Content;
using SwarmTheSpire;
using SwarmTheSpire.Powers;
using SwarmTheSpire.Relics;

namespace SwarmTheSpire.Cards
{
    public sealed class HarpoonPursuit()
        : SwarmEvilPoolCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
    {
        protected override IEnumerable<string> RegisteredKeywordIds =>
            [ModContentRegistry.GetQualifiedKeywordId(Const.ModId, "harpoon")];

        protected override IEnumerable<string> RegisteredCardTagIds => [SwarmCardTagIds.Harpoon];

        protected override bool ShouldGlowGoldInternal => WasLastCardPlayedHarpoon;

        public override TargetType TargetType => HasQueenPower ? TargetType.AllEnemies : TargetType.AnyEnemy;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(9m, ValueProp.Move)];

        private bool HasQueenPower => CombatManager.Instance.IsInProgress && Owner.Creature.HasPower<QueenPower>();

        private bool WasLastCardPlayedHarpoon
        {
            get
            {
                var val = CombatManager.Instance.History.CardPlaysStarted.LastOrDefault(e =>
                    e.CardPlay.Card.Owner == Owner && e.CardPlay.Card != this);
                return val != null && val.CardPlay.Card.HasModCardTag(SwarmCardTagIds.Harpoon);
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            var shouldTriggerFatal = cardPlay.Target.Powers.All(static power => power.ShouldOwnerDeathTriggerFatal());
            var combatState = CombatState;
            var attack = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
                .Execute(choiceContext);
            TryIncrementCatch(shouldTriggerFatal, attack);

            if (HasQueenPower)
            {
                ArgumentNullException.ThrowIfNull(combatState);
                var queenPowerCount = Owner.Creature.GetPowerAmount<QueenPower>();
                for (var i = 0; i < queenPowerCount; i++)
                {
                    foreach (var hittableEnemy in combatState.HittableEnemies)
                    {
                        var followUpAttack = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this)
                            .Targeting(hittableEnemy)
                            .Execute(choiceContext);
                        TryIncrementCatch(shouldTriggerFatal, followUpAttack);
                    }
                }
            }

            if (!WasLastCardPlayedHarpoon) return;
            var playerCombatState = Owner.PlayerCombatState;
            ArgumentNullException.ThrowIfNull(playerCombatState);
            playerCombatState.GainEnergy(1m);

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
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
