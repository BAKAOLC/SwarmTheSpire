using System.Reflection;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using STS2RitsuLib;
using STS2RitsuLib.Audio;
using SwarmTheSpire.Content;
using FileAccess = Godot.FileAccess;

namespace SwarmTheSpire
{
    [ModInitializer(nameof(Initialize))]
    public static class Main
    {
        public static readonly Logger Logger = RitsuLibFramework.CreateLogger(Const.ModId);

        private static IDisposable? _evilBankDeferredInitSubscription;

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
                RitsuLibFramework.EnsureGodotScriptsRegistered(Assembly.GetExecutingAssembly(), Logger);
                QueueEvilFmodBankAfterDeferredInitialization();
                SwarmTheSpireContentRegistrar.RegisterAll();
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

        private static void QueueEvilFmodBankAfterDeferredInitialization()
        {
            if (_evilBankDeferredInitSubscription != null)
                return;

            _evilBankDeferredInitSubscription =
                RitsuLibFramework.SubscribeLifecycle<DeferredInitializationCompletedEvent>(_ =>
                {
                    try
                    {
                        if (FmodStudioServer.TryGet() is null)
                        {
                            Logger.Warn("FmodServer singleton missing; skipped FMOD bank load.");
                            return;
                        }

                        LoadEvilFmodBanksAligned();
                    }
                    finally
                    {
                        _evilBankDeferredInitSubscription?.Dispose();
                        _evilBankDeferredInitSubscription = null;
                    }
                });
        }

        private static void LoadEvilFmodBanksAligned()
        {
            if (!FmodStudioServer.TryLoadBank(Const.Paths.EvilBank))
            {
                Logger.Warn($"Failed to load FMOD bank: {Const.Paths.EvilBank}");
                return;
            }

            FmodStudioServer.TryLoadStudioGuidMappings(Const.Paths.EvilGuidsFile);
        }
    }
}
