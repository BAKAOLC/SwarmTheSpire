using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace SwarmTheSpire.Powers
{
    [RegisterPower(Inherit = true)]
    public abstract class SwarmPowerTemplate : ModPowerTemplate
    {
        public virtual string CustomPackedIconPath => Const.Paths.SharedPowerIcon;

        public override string CustomBigIconPath => CustomPackedIconPath;

        public override PowerAssetProfile AssetProfile => new(CustomPackedIconPath, CustomBigIconPath);
    }
}
