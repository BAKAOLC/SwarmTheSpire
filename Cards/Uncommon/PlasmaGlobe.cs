using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class PlasmaGlobe() : SwarmCardTemplate(2, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
    {
        protected override HashSet<CardTag> CanonicalTags => [];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<WeakPower>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<PlasmaPower>(Owner.Creature, 30m, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
