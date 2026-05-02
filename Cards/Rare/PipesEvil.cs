using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class PipesEvil()
        : SwarmEvilPoolCard(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, true)
    {
        protected override HashSet<CardTag> CanonicalTags => [];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [StunIntent.GetStaticHoverTip()];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(30m, ValueProp.Move)];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combatState = CombatState;
            ArgumentNullException.ThrowIfNull(combatState);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(combatState)
                .Execute(choiceContext);
            await PowerCmd.Apply<PipesPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
            foreach (var hittableEnemy in combatState.HittableEnemies)
            {
                await CreatureCmd.Stun(hittableEnemy);
                PlayerCmd.EndTurn(Owner, false);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(10m);
        }
    }
}
