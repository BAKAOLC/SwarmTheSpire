using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using STS2RitsuLib.Patching.Models;
using SwarmTheSpire.Cards;
using SwarmTheSpire.Character;

namespace SwarmTheSpire.Patches
{
    /// <summary>
    ///     For <see cref="EvilCharacter" />, <see cref="DustyTome" /> always grants <see cref="Advent" /> as the ancient
    ///     card instead of rolling among unlocked ancients.
    /// </summary>
    public sealed class DustyTomeEvilAncientCardPatch : IPatchMethod
    {
        /// <inheritdoc />
        public static string PatchId => "dusty_tome_evil_ancient_card";

        /// <inheritdoc />
        public static string Description =>
            "DustyTome.SetupForPlayer: EvilCharacter uses Advent as AncientCard";

        /// <inheritdoc />
        public static bool IsCritical => false;

        /// <inheritdoc />
        public static ModPatchTarget[] GetTargets() =>
            [new(typeof(DustyTome), nameof(DustyTome.SetupForPlayer), [typeof(Player)])];

        // ReSharper disable once InconsistentNaming
        public static bool Prefix(DustyTome __instance, Player player)
        {
            if (player.Character is not EvilCharacter)
                return true;

            __instance.AncientCard = ModelDb.GetId<Advent>();
            return false;
        }
        // ReSharper restore InconsistentNaming
    }
}
