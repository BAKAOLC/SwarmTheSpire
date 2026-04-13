using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Content;
using SwarmTheSpire.Powers;
using SwarmTheSpire.Relics;

namespace SwarmTheSpire.Cards
{
    public sealed class SpareHarpoon()
        : SwarmCardTemplate(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
    {
        protected override HashSet<CardTag> CanonicalTags => [];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            [CardKeyword.Exhaust, CardKeyword.Retain];

        protected override IEnumerable<string> RegisteredKeywordIds =>
            [ModContentRegistry.GetQualifiedKeywordId(Const.ModId, "harpoon")];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.Static(StaticHoverTip.Fatal)];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(9m, ValueProp.Move),
            new CardsVar(1),
        ];

        private bool HasQueenPower => CombatManager.Instance.IsInProgress && Owner.Creature.HasPower<QueenPower>();

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
                foreach (var hittableEnemy in combatState.HittableEnemies)
                {
                    var followUpAttack = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this)
                        .Targeting(hittableEnemy)
                        .Execute(choiceContext);
                    TryIncrementCatch(shouldTriggerFatal, followUpAttack);
                }
            }

            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
            return;

            void TryIncrementCatch(bool canTriggerFatal, AttackCommand attackCommand)
            {
                if (!canTriggerFatal ||
                    !attackCommand.Results.Any(static result => result is { OverkillDamage: 0, WasTargetKilled: true }))
                    return;

                MilesRelic.TryIncrementCatch(Owner);
                WeAreMilesRelic.TryIncrementCatch(Owner);
            }
        }

        public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (Pile?.Type != PileType.Discard)
                return Task.CompletedTask;

            var isHarpoon = SwarmCardPredicates.IsHarpoon(cardPlay.Card);
            if (!isHarpoon || cardPlay.Card == this || cardPlay.Card.GetType() == GetType())
                return Task.CompletedTask;

            CardPileCmd.Add(this, PileType.Hand, CardPilePosition.Top);
            return Task.CompletedTask;
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
