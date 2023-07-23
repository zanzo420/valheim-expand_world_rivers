using System.Reflection;
using BepInEx.Bootstrap;
using HarmonyLib;

namespace ExpandWorldRivers;

public class EWS
{
  public const string GUID = "expand_world_size";
  private static Assembly? Assembly;
  private static MethodInfo? SetSize;
  public static void Run()
  {
    if (!Chainloader.PluginInfos.TryGetValue(GUID, out var info)) return;
    Assembly = info.Instance.GetType().Assembly;
    var type = Assembly.GetType("ExpandWorldSize.World");
    if (type == null) return;
    SetSize = AccessTools.Method(type, "Set");
    if (SetSize == null) return;
    EWR.Log.LogInfo("\"Expand World Size\" detected. Applying compatibility.");
  }

  public static void RefreshSize()
  {
    if (SetSize == null) return;
    SetSize.Invoke(null, new object[] { Configuration.WaterLevel });
  }
}
