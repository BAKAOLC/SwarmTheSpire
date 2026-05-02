using MegaCrit.Sts2.Core.Models;

namespace SwarmTheSpire.Cards
{
    internal static class SwarmCardPredicates
    {
        internal static bool IsHarpoon(CardModel? card)
        {
            return card is HarpoonThrust or HarpoonPursuit or HarpoonImpale or SpareHarpoon;
        }

        internal static bool IsEvzPoolCard(CardModel? card)
        {
            return card is DualHarpoonEvil or PirateEvil or PipesEvil or TungstenCubes or NekoEvil or PlasmaGlobe
                or PirateNeuro or TheQueenOfHarpoons or CrewMemberEvil;
        }
    }
}
