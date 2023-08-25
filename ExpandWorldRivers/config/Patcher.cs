using HarmonyLib;

namespace ExpandWorldRivers;

[HarmonyPatch]
public static class Patcher
{
  public static WorldGenerator? WG;
  public static bool IsMenu => WG == null || WG.m_world.m_menu;
  private static Harmony? Harmony;
  public static void Init(Harmony harmony)
  {
    Harmony = harmony;
    Patch();
  }
  public static void Patch()
  {
    if (Harmony == null) throw new("Harmony not initialized.");
    Harmony.UnpatchSelf();
    if (WG != null)
    {
      EWS.RefreshWaterLevel();
      EWD.RefreshWaterLevel();
    }
    Harmony.PatchAll();
    CheckRivers(Harmony);
    CheckStreams(Harmony);
  }

  [HarmonyPatch(typeof(WorldGenerator), nameof(WorldGenerator.VersionSetup)), HarmonyPostfix, HarmonyPriority(Priority.Last)]
  static void PatchOnLoad(WorldGenerator __instance)
  {
    WG = __instance;
    Patch();
  }

  private static void CheckRivers(Harmony harmony)
  {
    if (!IsMenu && !Configuration.Rivers) return;
    var method = AccessTools.Method(typeof(WorldGenerator), nameof(WorldGenerator.PlaceRivers));
    var patch = AccessTools.Method(typeof(PlaceRivers), nameof(PlaceRivers.Prefix));
    harmony.Unpatch(method, patch);
    method = AccessTools.Method(typeof(WorldGenerator), nameof(WorldGenerator.FindLakes));
    patch = AccessTools.Method(typeof(FindLakes), nameof(FindLakes.Prefix));
    harmony.Unpatch(method, patch);
  }
  private static void CheckStreams(Harmony harmony)
  {
    if (!IsMenu && !Configuration.Streams) return;
    var method = AccessTools.Method(typeof(WorldGenerator), nameof(WorldGenerator.PlaceStreams));
    var patch = AccessTools.Method(typeof(PlaceStreams), nameof(PlaceStreams.Prefix));
    harmony.Unpatch(method, patch);
  }

}
