using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace SwarmTheSpire.Cards
{
    public sealed class HotChicken() : SwarmCardTemplate(6, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };

        protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
        {
            new PowerVar<StrengthPower>(3m),
            new CardsVar(1),
        };

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<StrengthPower>()];

        protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            return Task.CompletedTask;
        }

        public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card,
            bool causedByEthereal)
        {
            EnergyCost.GetResolved();
            if (card == this)
            {
                NPowerUpVfx.CreateNormal(Owner.Creature);
                await PowerCmd.Apply<StrengthPower>(Owner.Creature, DynamicVars["StrengthPower"].BaseValue,
                    Owner.Creature, this);
                await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
            }
        }

        public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
        {
            if (card != this) return Task.CompletedTask;
            EnergyCost.AddThisCombat(-3);
            return Task.CompletedTask;
        }

        protected override void OnUpgrade()
        {
            DynamicVars["StrengthPower"].UpgradeValueBy(3m);
        }
    }
}
