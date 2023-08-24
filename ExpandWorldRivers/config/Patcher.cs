using HarmonyLib;

namespace ExpandWorldRivers;

[HarmonyPatch]
public static class Patcher
{
  private static Harmony? Harmony;
  public static void Init(Harmony harmony)
  {
    Harmony = harmony;
    Patch(WorldGenerator.instance);
  }
  public static void Patch(WorldGenerator? wg)
  {
    if (Harmony == null) throw new("Harmony not initialized.");
    Harmony.UnpatchSelf();
    if (wg != null)
    {
      EWS.RefreshWaterLevel();
      EWD.RefreshWaterLevel();
    }
    Harmony.PatchAll();
    CheckRivers(Harmony, wg);
    CheckStreams(Harmony, wg);
  }
  // Server sync reads config after ZNet.Awake. So this is the first possible moment for the patches.
  [HarmonyPatch(typeof(ZNet), nameof(ZNet.Awake)), HarmonyPostfix, HarmonyPriority(Priority.Last)]
  static void GamePatch(ZNet __instance)
  {
    if (__instance.IsServer())
      Patch(WorldGenerator.instance);
  }
  // Main menu doesn't have ZNet, so it needs different point to clean up patches.
  [HarmonyPatch(typeof(WorldGenerator), nameof(WorldGenerator.VersionSetup)), HarmonyPostfix, HarmonyPriority(Priority.Last)]
  static void MainMenuPatch(WorldGenerator __instance)
  {
    if (__instance.m_world.m_menu)
      Patch(__instance);
  }

  private static void CheckRivers(Harmony harmony, WorldGenerator? wg)
  {
    if (wg != null && !wg.m_world.m_menu && !Configuration.Rivers) return;
    var method = AccessTools.Method(typeof(WorldGenerator), nameof(WorldGenerator.PlaceRivers));
    var patch = AccessTools.Method(typeof(PlaceRivers), nameof(PlaceRivers.Prefix));
    harmony.Unpatch(method, patch);
    method = AccessTools.Method(typeof(WorldGenerator), nameof(WorldGenerator.FindLakes));
    patch = AccessTools.Method(typeof(FindLakes), nameof(FindLakes.Prefix));
    harmony.Unpatch(method, patch);
  }
  private static void CheckStreams(Harmony harmony, WorldGenerator? wg)
  {
    if (wg != null && !wg.m_world.m_menu && !Configuration.Streams) return;
    var method = AccessTools.Method(typeof(WorldGenerator), nameof(WorldGenerator.PlaceStreams));
    var patch = AccessTools.Method(typeof(PlaceStreams), nameof(PlaceStreams.Prefix));
    harmony.Unpatch(method, patch);
  }

}
