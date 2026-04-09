using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace SwarmTheSpire.Powers
{
    public sealed class CrazyFuckingRobotBodyPower : SwarmPowerTemplate
    {
        public override string CustomPackedIconPath =>
            "res://SwarmTheSpire/images/powers/SwarmTheSpire-miles_power.png";

        public override string CustomBigIconPath => "res://SwarmTheSpire/images/powers/SwarmTheSpire-miles_power.png";

        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Single;

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

        public override Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card,
            bool causedByEthereal)
        {
            if (card.Owner.Creature != Owner) return Task.CompletedTask;
            var player = Owner.Player;
            ArgumentNullException.ThrowIfNull(player);
            var cards = ((PileType)2).GetPile(player).Cards;
            var combatCardSelection = player.RunState.Rng.CombatCardSelection;
            var list = cards.Where(c => c != card && c.EnergyCost.GetResolved() > 0).ToList();
            if (list.Count == 0) list = cards.Where(c => c != card).ToList();
            var obj = list.Count != 0 ? combatCardSelection.NextItem(list) : null;
            obj?.EnergyCost.SetThisCombat(0, true);
            return Task.CompletedTask;
        }

        public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(CardModel card,
            bool isAutoPlay, ResourceInfo resources, PileType pileType, CardPilePosition position)
        {
            return card.Owner.Creature != Owner ? (pileType, position) : ((PileType)4, position);
        }
    }
}
