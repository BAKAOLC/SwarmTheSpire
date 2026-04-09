using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Scaffolding.Content;

namespace SwarmTheSpire.Cards
{
    public abstract class SwarmCardTemplate(
        int baseCost,
        CardType type,
        CardRarity rarity,
        TargetType targetType,
        bool shouldShowInCardLibrary)
        : ModCardTemplate(baseCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
        public override CardAssetProfile AssetProfile =>
            new(Const.Paths.Card(GetType().Name), Const.Paths.Card(GetType().Name));
    }
}
