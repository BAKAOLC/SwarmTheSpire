using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class HarpoonPursuit()
        : SwarmCardTemplate(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
    {
        protected override HashSet<CardTag> CanonicalTags => [];

        protected override bool ShouldGlowGoldInternal => WasLastCardPlayedHarpoon;

        public override TargetType TargetType => HasQueenPower ? TargetType.AllEnemies : TargetType.AnyEnemy;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(11m, ValueProp.Move)];

        private bool HasQueenPower => CombatManager.Instance.IsInProgress && Owner.Creature.HasPower<QueenPower>();

        private bool WasLastCardPlayedHarpoon
        {
            get
            {
                var val = CombatManager.Instance.History.CardPlaysStarted.LastOrDefault(e =>
                    e.CardPlay.Card.Owner == Owner && e.CardPlay.Card != this);
                return val != null && SwarmCardPredicates.IsHarpoon(val.CardPlay.Card);
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combatState = CombatState;
            if (!HasQueenPower)
            {
                ArgumentNullException.ThrowIfNull(cardPlay.Target);
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
                    .Execute(choiceContext);
            }
            else
            {
                ArgumentNullException.ThrowIfNull(combatState);
                foreach (var hittableEnemy in combatState.HittableEnemies)
                    await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(hittableEnemy)
                        .Execute(choiceContext);
            }

            if (WasLastCardPlayedHarpoon)
            {
                var playerCombatState = Owner.PlayerCombatState;
                ArgumentNullException.ThrowIfNull(playerCombatState);
                playerCombatState.GainEnergy(1m);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
