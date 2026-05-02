using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models.Characters;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;
using SwarmTheSpire.Content.Descriptors;

namespace SwarmTheSpire.Character
{
    [RegisterCharacter]
    public sealed class EvilCharacter : ModCharacterTemplate<EvilCardPool, EvilRelicPool, EvilPotionPool>
    {
        public static readonly Color Color = new("8080ff");

        public static readonly Color EnergyOutlineColor = new("1a1aff");

        public override bool RequiresEpochAndTimeline => false;

        public override Color NameColor => Color;

        public override Color MapDrawingColor => Color;

        public override Color EnergyLabelOutlineColor => EnergyOutlineColor;

        public override CharacterGender Gender => CharacterGender.Feminine;

        public override int StartingHp => 75;

        public override int StartingGold => 99;

        public override float AttackAnimDelay => 0.15f;

        public override float CastAnimDelay => 0.25f;

        public override CharacterAssetProfile AssetProfile => EvilCharacterAssets.Profile;

        protected override Type UnlocksAfterRunAsType => typeof(Ironclad);

        public override List<string> GetArchitectAttackVfx()
        {
            return
            [
                "vfx/vfx_attack_blunt",
                "vfx/vfx_heavy_blunt",
                "vfx/vfx_attack_slash",
                "vfx/vfx_bloody_impact",
                "vfx/vfx_rock_shatter",
            ];
        }
    }
}
