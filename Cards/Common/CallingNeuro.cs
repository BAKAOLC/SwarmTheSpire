using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class CallingNeuro()
        : SwarmEvilPoolCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, true)
    {
        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(6m, ValueProp.Move),
            new PowerVar<NeuroFansPower>(1m),
        ];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<NeuroFansPower>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay, false);
            await PowerCmd.Apply<NeuroFansPower>(choiceContext, cardPlay.Target,
                DynamicVars["NeuroFansPower"].BaseValue, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(1m);
            DynamicVars["NeuroFansPower"].UpgradeValueBy(1m);
        }
    }
}
