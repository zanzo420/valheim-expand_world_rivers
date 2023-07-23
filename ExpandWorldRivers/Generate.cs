
using UnityEngine;

namespace ExpandWorldRivers;

public class Generate
{
  public static void World()
  {
    EWS.RefreshSize();
    WorldGenerator.instance?.Pregenerate();
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
    Minimap.instance?.GenerateWorldMap();
  }
}
