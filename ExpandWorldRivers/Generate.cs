
using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering;

namespace ExpandWorldRivers;

public class Generate
{
  public static void World()
  {
    EWS.RefreshWaterLevel();
    EWD.RefreshWaterLevel();
    Patcher.Patch(EWR.harmony);
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
    __instance.m_riverPoints = new();
    __instance.m_rivers = new();
    __instance.m_streams = new();
    __instance.m_lakes = new();
    __instance.m_cachedRiverGrid = new Vector2i(-999999, -999999);
    __instance.m_cachedRiverPoints = new WorldGenerator.RiverPoint[0];
    __instance.m_riverCacheLock.ExitWriteLock();
  }
}

[HarmonyPatch(typeof(WorldGenerator), nameof(WorldGenerator.VersionSetup))]
public class VersionSetup
{
  static void Prefix()
  {
    EWS.RefreshWaterLevel();
    EWD.RefreshWaterLevel();
    Patcher.Patch(EWR.harmony);
  }
}