using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Content;
using STS2RitsuLib.Keywords;

namespace SwarmTheSpire.Powers
{
    public sealed class DualHarpoonEvilPower : SwarmPowerTemplate
    {
        private CardModel? _trackedCard;
        private bool _wasUsedThisTurn;

        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Single;

        protected override IEnumerable<string> RegisteredKeywordIds =>
            [ModContentRegistry.GetQualifiedKeywordId(Const.ModId, "harpoon")];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [
            HoverTipFactory.FromKeyword(CardKeyword.Retain),
            HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
        ];

        public override Task BeforeCardPlayed(CardPlay cardPlay)
        {
            if (_wasUsedThisTurn || _trackedCard is not null || cardPlay.Card.Owner != Owner.Player || !cardPlay.Card
                    .GetModKeywordIds()
                    .Contains(ModContentRegistry.GetQualifiedKeywordId(Const.ModId, "harpoon")))
                return Task.CompletedTask;

            _trackedCard = cardPlay.Card;
            return Task.CompletedTask;
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (cardPlay.Card != _trackedCard)
                return;

            var count = (int)Math.Max(0m, Amount);
            for (var i = 0; i < count; i++)
            {
                var clone = cardPlay.Card.CreateClone();
                CardCmd.ApplyKeyword(clone, CardKeyword.Retain);
                CardCmd.ApplyKeyword(clone, CardKeyword.Exhaust);
                await CardPileCmd.AddGeneratedCardToCombat(clone, PileType.Hand, Owner.Player, CardPilePosition.Top);
            }

            _wasUsedThisTurn = true;
            _trackedCard = null;
        }

        public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
            ICombatState combatState)
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
