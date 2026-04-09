using STS2RitsuLib;
using SwarmTheSpire.Content.Descriptors;

namespace SwarmTheSpire.Content
{
    internal static class SwarmTheSpireContentRegistrar
    {
        internal static void RegisterAll()
        {
            RitsuLibFramework.CreateContentPack(Const.ModId)
                .ContentManifest(SwarmTheSpireContentManifest.ContentEntries)
                .Apply();
        }
    }
}
