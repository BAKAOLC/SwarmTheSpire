using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using SwarmTheSpire.Cards;

namespace SwarmTheSpire.Powers
{
    public sealed class CatchCommonTokenPower : SwarmPowerTemplate
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power,
            decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (Amount <= 0)
                return;

            var player = Owner.Player;
            if (player is null)
                return;

            CardModel[] cardsToChoose =
            [
                ModelDb.Card<FrenchFries>(),
                ModelDb.Card<Hamburger>(),
                ModelDb.Card<Coffee>(),
                ModelDb.Card<HotChicken>(),
            ];
            var forCombat =
                CardFactory.GetForCombat(player, cardsToChoose, 1, player.RunState.Rng.CombatCardGeneration);
            await CardPileCmd.AddGeneratedCardsToCombat(forCombat, PileType.Hand, player, CardPilePosition.Top);
            await PowerCmd.Decrement(this);
        }
    }
}
