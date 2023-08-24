using System.Reflection;
using BepInEx.Bootstrap;
using HarmonyLib;

namespace ExpandWorldRivers;

public class EWD
{
  public const string GUID = "expand_world_data";
  private static Assembly? Assembly;
  private static MethodInfo? SetWaterLevel;
  public static void Run()
  {
    if (!Chainloader.PluginInfos.TryGetValue(GUID, out var info)) return;
    Assembly = info.Instance.GetType().Assembly;
    var type = Assembly.GetType("ExpandWorldData.WorldInfo");
    if (type == null) return;
    SetWaterLevel = AccessTools.Method(type, "SetWaterLevel");
    if (SetWaterLevel == null) return;
    EWR.Log.LogInfo("\"Expand World Data\" detected. Applying compatibility.");
  }

  public static void RefreshWaterLevel()
  {
    if (SetWaterLevel == null) return;
    SetWaterLevel.Invoke(null, [Configuration.WaterLevel]);
  }
}
