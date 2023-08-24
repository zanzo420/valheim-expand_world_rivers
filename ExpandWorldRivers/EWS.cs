using System.Reflection;
using BepInEx.Bootstrap;
using HarmonyLib;

namespace ExpandWorldRivers;

public class EWS
{
  public const string GUID = "expand_world_size";
  private static Assembly? Assembly;
  private static MethodInfo? SetWaterLevel;
  public static void Run()
  {
    if (!Chainloader.PluginInfos.TryGetValue(GUID, out var info)) return;
    Assembly = info.Instance.GetType().Assembly;
    var type = Assembly.GetType("ExpandWorldSize.WorldInfo");
    if (type == null) return;
    SetWaterLevel = AccessTools.Method(type, "SetWaterLevel");
    if (SetWaterLevel == null) return;
    EWR.Log.LogInfo("\"Expand World Size\" detected. Applying compatibility.");
  }

  public static void RefreshWaterLevel()
  {
    if (SetWaterLevel == null) return;
    SetWaterLevel.Invoke(null, new object[] { Configuration.WaterLevel });
  }
}
