using STS2RitsuLib.Scaffolding.Content;

namespace SwarmTheSpire.Powers
{
    public abstract class SwarmPowerTemplate : ModPowerTemplate
    {
        public virtual string CustomPackedIconPath => Const.Paths.SharedPowerIcon;

        public override string? CustomBigIconPath => CustomPackedIconPath;

        public virtual string? CustomBigBetaIconPath => null;

        public override PowerAssetProfile AssetProfile => new(CustomPackedIconPath, CustomBigIconPath);
    }
}
