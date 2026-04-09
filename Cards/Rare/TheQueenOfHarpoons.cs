using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class TheQueenOfHarpoons()
        : SwarmCardTemplate(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
    {
        protected override HashSet<CardTag> CanonicalTags => [];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<QueenPower>(Owner.Creature, 1m, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
