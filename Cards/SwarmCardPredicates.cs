using MegaCrit.Sts2.Core.Models;

namespace SwarmTheSpire.Cards
{
    internal static class SwarmCardPredicates
    {
        internal static bool IsHarpoon(CardModel? card)
        {
            return card is HarpoonThrust or HarpoonPursuit or HarpoonImpale or SpareHarpoon;
        }
    }
}
