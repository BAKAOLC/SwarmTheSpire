using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SwarmTheSpire.Cards;

namespace SwarmTheSpire.Powers
{
    public sealed class CrewmemberPower : SwarmPowerTemplate
    {
        public override string CustomPackedIconPath =>
            "res://SwarmTheSpire/images/powers/SwarmTheSpire-miles_power.png";

        public override string CustomBigIconPath => "res://SwarmTheSpire/images/powers/SwarmTheSpire-miles_power.png";

        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext,
            ICombatState combatState)
        {
            var source = new List<CardModel>
            {
                ModelDb.Card<FrenchFries>(),
                ModelDb.Card<Hamburger>(),
                ModelDb.Card<Coffee>(),
                ModelDb.Card<HotChicken>(),
            };
            var ownerPlayer = Owner.Player;
            ArgumentNullException.ThrowIfNull(ownerPlayer);
            await CardPileCmd.AddGeneratedCardsToCombat(
                CardFactory.GetForCombat(ownerPlayer, source.Select(c => c), AmountOnTurnStart,
                    ownerPlayer.RunState.Rng.CombatCardGeneration), PileType.Hand, ownerPlayer, CardPilePosition.Top);
        }
    }
}
