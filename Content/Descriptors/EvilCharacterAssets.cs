using STS2RitsuLib.Scaffolding.Characters;

namespace SwarmTheSpire.Content.Descriptors
{
    internal static class EvilCharacterAssets
    {
        private static readonly CharacterAssetProfile BaseProfile = CharacterAssetProfiles.Ironclad();

        internal static CharacterAssetProfile Profile { get; } = BaseProfile
            .WithScenes(BaseProfile.Scenes! with
            {
                VisualsPath = Const.Paths.CharacterVisualsScene,
                MerchantAnimPath = Const.Paths.CharacterMerchantScene,
                RestSiteAnimPath = Const.Paths.CharacterRestSiteScene,
            })
            .WithUi(BaseProfile.Ui! with
            {
                IconTexturePath = Const.Paths.CharacterIconTexture,
                IconPath = Const.Paths.CharacterIconScene,
                CharacterSelectBgPath = Const.Paths.CharacterSelectBgScene,
                CharacterSelectIconPath = Const.Paths.CharacterSelectIcon,
                CharacterSelectLockedIconPath = Const.Paths.CharacterSelectLockedIcon,
            })
            .WithMultiplayer(BaseProfile.Multiplayer! with
            {
                ArmPointingTexturePath = Const.Paths.ArmPointingTexture,
            })
            .WithAudio(BaseProfile.Audio! with
            {
                CharacterTransitionSfx = Const.Paths.CharacterTransitionSfx,
            });
    }
}
