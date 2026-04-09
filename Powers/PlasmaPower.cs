using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace SwarmTheSpire.Powers
{
    public sealed class PlasmaPower : SwarmPowerTemplate
    {
        public override string CustomPackedIconPath =>
            "res://SwarmTheSpire/images/powers/SwarmTheSpire-miles_power.png";

        public override string CustomBigIconPath => "res://SwarmTheSpire/images/powers/SwarmTheSpire-miles_power.png";

        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<WeakPower>()];

        public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props,
            Creature? dealer, CardModel? cardSource)
        {
            if (target == null) return 1m;
            var num = target.HasPower<WeakPower>();
            var flag = target.Block > 0;
            if (!num && !flag) return 1m;
            if (!props.IsPoweredAttack()) return 1m;
            if (cardSource == null) return 1m;
            if (dealer != Owner && !Owner.Pets.Contains(dealer)) return 1m;
            return 1m + Amount * 0.01m;
        }
    }
}
