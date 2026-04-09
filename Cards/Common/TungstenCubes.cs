using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace SwarmTheSpire.Cards
{
    public sealed class TungstenCubes() : SwarmCardTemplate(2, CardType.Skill, CardRarity.Common, TargetType.Self, true)
    {
        public override bool GainsBlock => true;

        protected override HashSet<CardTag> CanonicalTags =>
        [
            CardTag.Defend,
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(74m, ValueProp.Move),
            new("Gold", -21m),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            await PlayerCmd.GainGold(DynamicVars["Gold"].IntValue, Owner);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Gold"].UpgradeValueBy(10m);
        }
    }
}
