using STS2RitsuLib;

namespace SwarmTheSpire.Content
{
    internal static class SwarmTheSpireContentRegistrar
    {
        internal static void RegisterAll()
        {
            RitsuLibFramework.CreateContentPack(Const.ModId).Apply();
        }
    }
}
