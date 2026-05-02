using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace SwarmTheSpire.Cards
{
    public sealed class CookieRaid()
        : SwarmEvilPoolCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var drawPile = PileType.Draw.GetPile(Owner).Cards
                .OrderBy(c => c.Rarity)
                .ThenBy(c => c.Id)
                .ToList();

            var selected = (await CardSelectCmd.FromSimpleGrid(choiceContext, drawPile, Owner,
                new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1))).FirstOrDefault();

            var cost = 0;
            if (selected is not null)
            {
                cost = selected.EnergyCost.GetResolved();
                await CardCmd.Exhaust(choiceContext, selected, false, false);
            }

            await CreatureCmd.Heal(Owner.Creature, cost, true);
        }

        protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
    }
}
