using MegaCrit.Sts2.Core.Entities.Powers;

namespace SwarmTheSpire.Powers
{
    public sealed class BossPower : SwarmPowerTemplate
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Single;
    }
}
