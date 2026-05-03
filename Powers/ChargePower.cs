using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.CardTags;
using SwarmTheSpire.Cards;

namespace SwarmTheSpire.Powers;

public sealed class ChargePower : SwarmPowerTemplate
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override bool AllowNegative => true;

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (Owner != dealer || cardSource is null)
            return 0m;

        return cardSource.HasModCardTag(SwarmCardTagIds.Harpoon) ? Amount : 0m;
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power,
        decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (!ReferenceEquals(power, this) || Amount <= 0)
            return;

        var player = Owner.Player;
        if (player is null)
            return;

        var handCards = player.PlayerCombatState?.Hand.Cards;
        if (handCards is not null && handCards.Any(static c => c is Harpoon))
            return;

        var combatState = Owner.CombatState;
        if (combatState is null)
            return;

        var created = CardFactory.GetForCombat(player, [ModelDb.Card<Harpoon>()], 1,
            player.RunState.Rng.CombatCardGeneration);
        await CardPileCmd.AddGeneratedCardsToCombat(created, PileType.Hand, player, CardPilePosition.Top);
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (Amount <= 0 || cardPlay.Card.Owner.Creature != Owner)
            return;

        if (!cardPlay.Card.HasModCardTag(SwarmCardTagIds.Harpoon))
            return;

        await PowerCmd.Remove(this);
    }
}
