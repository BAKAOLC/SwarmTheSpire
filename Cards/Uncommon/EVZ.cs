using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
using SwarmTheSpire;

namespace SwarmTheSpire.Cards
{
    [RegisterOwnedCardTag("evz")]
    public sealed class EVZ()
        : SwarmEvilPoolCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
    {
        private CardModel? _mockSelectedCard;

        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            CardModel? pick;
            if (_mockSelectedCard is null)
            {
                var pool = Owner.Character.CardPool
                    .GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
                    .Where(static c => c.HasModCardTag(SwarmCardTagIds.Evz))
                    .ToList();

                var choices = CardFactory.GetDistinctForCombat(Owner, pool, 3, Owner.RunState.Rng.CombatCardGeneration)
                    .ToList();

                pick = await CardSelectCmd.FromChooseACardScreen(choiceContext, choices, Owner, true);
            }
            else
            {
                pick = _mockSelectedCard;
            }

            if (pick is null)
                return;

            pick.SetToFreeThisTurn();
            await CardPileCmd.AddGeneratedCardToCombat(pick, PileType.Hand, Owner, CardPilePosition.Top);
        }

        protected override void OnUpgrade() => RemoveKeyword(CardKeyword.Exhaust);

        public void MockSelectedCard(CardModel card)
        {
            AssertMutable();
            _mockSelectedCard = card;
        }
    }
}
