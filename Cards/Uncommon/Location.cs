using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace SwarmTheSpire.Cards
{
    public sealed class Location()
        : SwarmCardTemplate(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(14m, ValueProp.Move),
            new CardsVar(2),
        ];

        protected override bool ShouldGlowRedInternal => true;

        public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
        {
            if (card.Owner != Owner) return true;
            var pile = Pile;
            if (pile == null || (int)pile.Type != 2) return true;
            if (card is Location) return true;
            return (int)autoPlayType != 0;
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
                .Execute(choiceContext);
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CreateClone(), (PileType)1, true),
                2.2f);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(5m);
        }
    }
}
