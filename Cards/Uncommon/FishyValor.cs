using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace SwarmTheSpire.Cards
{
    public sealed class FishyValor()
        : SwarmEvilPoolCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
    {
        public interface IChoosable
        {
            Task OnChosen();
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var options = new List<CardModel>
            {
                (CardModel)ModelDb.Card<Fishy1>().MutableClone(),
                (CardModel)ModelDb.Card<Fishy2>().MutableClone(),
                (CardModel)ModelDb.Card<Fishy3>().MutableClone(),
            };

            foreach (var item in options)
            {
                item.Owner = Owner;
                if (IsUpgraded)
                    CardCmd.Upgrade(item, CardPreviewStyle.HorizontalLayout);
            }

            var pick = await CardSelectCmd.FromChooseACardScreen(choiceContext, options, Owner, false);
            if (pick is IChoosable choosable)
                await choosable.OnChosen();
        }
    }
}
