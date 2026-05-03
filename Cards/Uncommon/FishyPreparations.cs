using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class FishyPreparations()
        : SwarmEvilPoolCard(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new PowerVar<ChargePower>(1m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<ChargePower>(choiceContext, Owner.Creature, DynamicVars["ChargePower"].BaseValue,
                Owner.Creature, this, false);
            await PowerCmd.Apply<FishyPreparationsPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this, false);
            var harpoon = CardFactory.GetForCombat(Owner, [ModelDb.Card<Harpoon>()], 1,
                Owner.RunState.Rng.CombatCardGeneration);
            await CardPileCmd.AddGeneratedCardsToCombat(harpoon, PileType.Hand, Owner, CardPilePosition.Top);
        }

        protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);
    }
}
