using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.CardTags;
using SwarmTheSpire;

namespace SwarmTheSpire.Cards;

internal static class SwarmCardPredicates
{
    internal static bool IsHarpoon(CardModel? card) =>
        card is not null && card.HasModCardTag(SwarmCardTagIds.Harpoon);
}
