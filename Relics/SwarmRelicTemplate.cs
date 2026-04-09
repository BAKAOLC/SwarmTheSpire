using STS2RitsuLib.Scaffolding.Content;

namespace SwarmTheSpire.Relics
{
    public abstract class SwarmRelicTemplate : ModRelicTemplate
    {
        public override string PackedIconPath => Const.Paths.Relic(GetType().Name);

        protected override string PackedIconOutlinePath => PackedIconPath;

        protected override string BigIconPath => PackedIconPath;

        public override RelicAssetProfile AssetProfile => new(PackedIconPath, PackedIconOutlinePath, BigIconPath);
    }
}
