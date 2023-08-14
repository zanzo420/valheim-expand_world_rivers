using HarmonyLib;

namespace ExpandWorldRivers;

public static class Patcher
{
  public static void Patch(Harmony harmony)
  {
    harmony.UnpatchSelf();
    harmony.PatchAll();
    CheckRivers(harmony);
    CheckStreams(harmony);
  }

  private static void CheckRivers(Harmony harmony)
  {
    if (WorldGenerator.instance != null && !WorldGenerator.instance.m_world.m_menu && !Configuration.Rivers) return;
    var method = AccessTools.Method(typeof(WorldGenerator), nameof(WorldGenerator.PlaceRivers));
    var patch = AccessTools.Method(typeof(PlaceRivers), nameof(PlaceRivers.Prefix));
    harmony.Unpatch(method, patch);
    method = AccessTools.Method(typeof(WorldGenerator), nameof(WorldGenerator.FindLakes));
    patch = AccessTools.Method(typeof(FindLakes), nameof(FindLakes.Prefix));
    harmony.Unpatch(method, patch);
  }
  private static void CheckStreams(Harmony harmony)
  {
    if (WorldGenerator.instance != null && !WorldGenerator.instance.m_world.m_menu && !Configuration.Streams) return;
    var method = AccessTools.Method(typeof(WorldGenerator), nameof(WorldGenerator.PlaceStreams));
    var patch = AccessTools.Method(typeof(PlaceStreams), nameof(PlaceStreams.Prefix));
    harmony.Unpatch(method, patch);
  }

}
