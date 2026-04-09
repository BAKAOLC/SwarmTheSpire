using MegaCrit.Sts2.Core.Entities.Powers;

namespace SwarmTheSpire.Powers
{
    public sealed class QueenPower : SwarmPowerTemplate
    {
        public override string CustomPackedIconPath =>
            "res://SwarmTheSpire/images/powers/SwarmTheSpire-queen_power.png";

        public override string CustomBigIconPath => "res://SwarmTheSpire/images/powers/SwarmTheSpire-queen_power.png";

        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Single;
    }
}
