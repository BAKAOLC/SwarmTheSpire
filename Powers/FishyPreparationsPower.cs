using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace SwarmTheSpire.Powers
{
    public sealed class FishyPreparationsPower : SwarmPowerTemplate
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override string CustomPackedIconPath =>
            "res://SwarmTheSpire/images/powers/SwarmTheSpire-miles_power.png";

        public override string CustomBigIconPath =>
            "res://SwarmTheSpire/images/powers/SwarmTheSpire-miles_power.png";

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [
            HoverTipFactory.FromPower<VigorPower>(),
            HoverTipFactory.Static(StaticHoverTip.Block),
        ];

        public override async Task BeforeTurnEndEarly(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (side != Owner.Side)
                return;

            Flash();
            await PowerCmd.Apply<VigorPower>(choiceContext, Owner, Amount, Owner, null);
            await CreatureCmd.GainBlock(Owner, Owner.GetPowerAmount<VigorPower>(), ValueProp.Move, null);
        }
    }
}
