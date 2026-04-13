using System.Linq;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Keywords;

namespace SwarmTheSpire.Powers
{
    public sealed class DualHarpoonEvilPower : SwarmPowerTemplate
    {
        private bool _wasUsedThisTurn;
        private CardModel? _trackedCard;

        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Single;

        protected override IEnumerable<string> RegisteredKeywordIds =>
            [STS2RitsuLib.Content.ModContentRegistry.GetQualifiedKeywordId(Const.ModId, "harpoon")];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [
            HoverTipFactory.FromKeyword(CardKeyword.Retain),
            HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
        ];

        public override Task BeforeCardPlayed(CardPlay cardPlay)
        {
            if (_wasUsedThisTurn || _trackedCard is not null)
                return Task.CompletedTask;
            if (cardPlay.Card.Owner != Owner.Player)
                return Task.CompletedTask;
            if (!cardPlay.Card.GetModKeywordIds().Contains(STS2RitsuLib.Content.ModContentRegistry.GetQualifiedKeywordId(Const.ModId, "harpoon")))
                return Task.CompletedTask;

            _trackedCard = cardPlay.Card;
            return Task.CompletedTask;
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (cardPlay.Card != _trackedCard)
                return;

            var clone = cardPlay.Card.CreateClone();
            CardCmd.ApplyKeyword(clone, CardKeyword.Retain);
            CardCmd.ApplyKeyword(clone, CardKeyword.Exhaust);
            await CardPileCmd.AddGeneratedCardToCombat(clone, PileType.Hand, true, CardPilePosition.Top);
            _wasUsedThisTurn = true;
            _trackedCard = null;
        }

        public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
        {
            if (side != Owner.Side)
                return Task.CompletedTask;

            _wasUsedThisTurn = false;
            _trackedCard = null;
            return Task.CompletedTask;
        }

        public override Task AfterCombatEnd(CombatRoom room)
        {
            _wasUsedThisTurn = false;
            _trackedCard = null;
            return Task.CompletedTask;
        }
    }
}
