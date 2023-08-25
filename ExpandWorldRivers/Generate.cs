
using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering;

namespace ExpandWorldRivers;

public class Generate
{
  public static void World()
  {
    Patcher.Patch();
    if (WorldGenerator.instance == null) return;
    EWR.Log.LogInfo("Regenerating the world.");
    WorldGenerator.instance.Pregenerate();
    foreach (var heightmap in Object.FindObjectsOfType<Heightmap>())
    {
      heightmap.m_buildData = null;
      heightmap.Regenerate();
    }
    ClutterSystem.instance?.ClearAll();
    foreach (var obj in WaterHelper.Get()) obj.Start();
    if (Configuration.RegenerateMap) Map();
  }
  public static void Map()
  {
    if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Null)
      Minimap.instance?.GenerateWorldMap();
  }
}

[HarmonyPatch(typeof(WorldGenerator), nameof(WorldGenerator.Pregenerate)), HarmonyPriority(Priority.HigherThanNormal)]
public class Pregenerate
{
  static void Prefix(WorldGenerator __instance)
  {
    // River points must at least be cleaned.
    // But better clean up everything.
    __instance.m_riverCacheLock.EnterWriteLock();
    __instance.m_riverPoints = [];
    __instance.m_rivers = [];
    __instance.m_streams = [];
    __instance.m_lakes = [];
    __instance.m_cachedRiverGrid = new(-999999, -999999);
    __instance.m_cachedRiverPoints = [];
    __instance.m_riverCacheLock.ExitWriteLock();
  }
}
