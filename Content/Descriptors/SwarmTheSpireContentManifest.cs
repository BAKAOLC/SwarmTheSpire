using STS2RitsuLib.Scaffolding.Content;
using SwarmTheSpire.Cards;
using SwarmTheSpire.Character;
using SwarmTheSpire.Powers;
using SwarmTheSpire.Relics;

namespace SwarmTheSpire.Content.Descriptors
{
    internal static class SwarmTheSpireContentManifest
    {
        private static readonly IContentRegistrationEntry[] CharacterEntries =
        [
            new CharacterRegistrationEntry<EvilCharacter>()
                .AddStartingCard<StrikeEvil>(4)
                .AddStartingCard<DefendEvil>(4)
                .AddStartingCard<HarpoonThrust>()
                .AddStartingCard<EvilStream>()
                .AddStartingRelic<MilesRelic>(),
        ];

        private static readonly IContentRegistrationEntry[] CardEntries =
        [
            new CardRegistrationEntry<EvilCardPool, StrikeEvil>(),
            new CardRegistrationEntry<EvilCardPool, DefendEvil>(),
            new CardRegistrationEntry<EvilCardPool, HarpoonThrust>(),
            new CardRegistrationEntry<EvilCardPool, EvilStream>(),
            new CardRegistrationEntry<EvilCardPool, EvilStrike>(),
            new CardRegistrationEntry<EvilCardPool, HarpoonPursuit>(),
            new CardRegistrationEntry<EvilCardPool, PirateEvil>(),
            new CardRegistrationEntry<EvilCardPool, PirateNeuro>(),
            new CardRegistrationEntry<EvilCardPool, NekoEvil>(),
            new CardRegistrationEntry<EvilCardPool, TungstenCubes>(),
            new CardRegistrationEntry<EvilCardPool, MilesDevotion>(),
            new CardRegistrationEntry<EvilCardPool, CuteNoise>(),
            new CardRegistrationEntry<EvilCardPool, PipeDrop>(),
            new CardRegistrationEntry<EvilCardPool, Coffee>(),
            new CardRegistrationEntry<EvilCardPool, FrenchFries>(),
            new CardRegistrationEntry<EvilCardPool, Hamburger>(),
            new CardRegistrationEntry<EvilCardPool, HotChicken>(),
            new CardRegistrationEntry<EvilCardPool, IEvilTV>(),
            new CardRegistrationEntry<EvilCardPool, Location>(),
            new CardRegistrationEntry<EvilCardPool, PlasmaGlobe>(),
            new CardRegistrationEntry<EvilCardPool, SoldOut>(),
            new CardRegistrationEntry<EvilCardPool, CrazyFuckingRobotBody>(),
            new CardRegistrationEntry<EvilCardPool, CrewMemberEvil>(),
            new CardRegistrationEntry<EvilCardPool, EvilPlush>(),
            new CardRegistrationEntry<EvilCardPool, PipesEvil>(),
            new CardRegistrationEntry<EvilCardPool, TheQueenOfHarpoons>(),
            new CardRegistrationEntry<EvilCardPool, Birthday>(),
            new CardRegistrationEntry<EvilCardPool, HarpoonImpale>(),
            new ArchaicToothTranscendenceRegistrationEntry<HarpoonThrust, HarpoonImpale>(),
        ];

        private static readonly IContentRegistrationEntry[] RelicEntries =
        [
            new RelicRegistrationEntry<EvilRelicPool, MilesRelic>(),
            new RelicRegistrationEntry<EvilRelicPool, WeAreMilesRelic>(),
            new TouchOfOrobasRefinementRegistrationEntry<MilesRelic, WeAreMilesRelic>(),
        ];

        private static readonly IContentRegistrationEntry[] PowerEntries =
        [
            new PowerRegistrationEntry<MilesPower>(),
            new PowerRegistrationEntry<SwarmPower>(),
            new PowerRegistrationEntry<QueenPower>(),
            new PowerRegistrationEntry<CrazyFuckingRobotBodyPower>(),
            new PowerRegistrationEntry<PlasmaPower>(),
            new PowerRegistrationEntry<PipesPower>(),
            new PowerRegistrationEntry<CrewmemberPower>(),
        ];

        public static IReadOnlyList<IContentRegistrationEntry> ContentEntries { get; } =
            Concat(CharacterEntries, CardEntries, RelicEntries, PowerEntries);

        private static IReadOnlyList<T> Concat<T>(params IEnumerable<T>[] chunks)
        {
            return chunks.SelectMany(static chunk => chunk).ToArray();
        }
    }
}
