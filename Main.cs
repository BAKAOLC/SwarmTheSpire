using System.Reflection;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using STS2RitsuLib;
using STS2RitsuLib.Audio;
using STS2RitsuLib.Interop;
using STS2RitsuLib.Patching.Core;
using SwarmTheSpire.Content;
using SwarmTheSpire.Patches;

namespace SwarmTheSpire
{
    [ModInitializer(nameof(Initialize))]
    public static class Main
    {
        public static readonly Logger Logger = RitsuLibFramework.CreateLogger(Const.ModId);

        public static bool IsModActive { get; private set; }

        public static void Initialize()
        {
            if (IsModActive)
            {
                Logger.Debug("Mod already initialized, skipping duplicate initialization.");
                return;
            }

            Logger.Info($"Mod ID: {Const.ModId}");
            Logger.Info($"Version: {Const.Version}");
            Logger.Info("Initializing mod...");

            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, Logger);

                var gameplayPatcher = RitsuLibFramework.CreatePatcher(Const.ModId, "gameplay", "gameplay");
                gameplayPatcher.RegisterPatch<DustyTomeEvilAncientCardPatch>();
                if (!RitsuLibFramework.ApplyRequiredPatcher(gameplayPatcher, () => IsModActive = false))
                {
                    Logger.Error("Mod initialization failed: gameplay patcher could not apply.");
                    return;
                }

                FmodStudioDeferredBankRegistration.RegisterBank(Const.Paths.EvilBank);
                FmodStudioDeferredBankRegistration.RegisterStudioGuidMappings(Const.Paths.EvilGuidsFile);
                SwarmTheSpireContentRegistrar.RegisterAll();
                ModTypeDiscoveryHub.RegisterModAssembly(Const.ModId, assembly);

                IsModActive = true;
                Logger.Info("Mod initialization complete - Mod is now ACTIVE");
            }
            catch (Exception ex)
            {
                Logger.Error($"Mod initialization failed with exception: {ex.Message}");
                Logger.Error($"Stack trace: {ex.StackTrace}");
                IsModActive = false;
            }
        }

    }
}
