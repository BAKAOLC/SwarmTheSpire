using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace SwarmTheSpire.Cards
{
    public sealed class SoldOut() : SwarmCardTemplate(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
    {
        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var val = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1);
            var val2 = (await CardSelectCmd.FromHand(choiceContext, Owner, val, null, this)).FirstOrDefault();
            var cost = 0;
            if (val2 != null)
            {
                cost = val2.EnergyCost.GetResolved();
                await CardCmd.Exhaust(choiceContext, val2);
            }

            await PlayerCmd.GainGold(cost, Owner);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
