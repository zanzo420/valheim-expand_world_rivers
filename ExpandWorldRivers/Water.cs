using HarmonyLib;
using UnityEngine;
namespace ExpandWorldRivers;

public class WaterHelper
{
  public static WaterVolume[] Get() => Object.FindObjectsOfType<WaterVolume>();
  public static void SetLevel(WaterVolume obj)
  {
    if (!obj.m_useGlobalWind) return;
    var position = obj.transform.position;
    position.y = Configuration.WaterLevel;
    obj.transform.position = position;
    position = obj.m_waterSurface.transform.position;
    position.y = Configuration.WaterLevel;
    obj.m_waterSurface.transform.position = position;
  }
  public static void SetLevel(ZoneSystem obj)
  {
    if (obj != null) obj.m_waterLevel = Configuration.WaterLevel;
  }
  public static void SetLevel(ClutterSystem obj)
  {
    if (obj != null) obj.m_waterLevel = Configuration.WaterLevel;
  }
  public static Vector3 OriginalScale = Vector3.zero;
  public static void SetWaveSize(WaterVolume obj)
  {
    if (!obj.m_useGlobalWind) return;
    if (OriginalScale == Vector3.zero) OriginalScale = obj.m_waterSurface.transform.localScale;
    var scale = OriginalScale;
    if (Configuration.WaveOnlyHeight) scale.y *= Configuration.WaveMultiplier;
    else scale *= Configuration.WaveMultiplier;
    obj.m_waterSurface.transform.localScale = scale;
  }
}

[HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.Awake))]
public class SetZoneSystemWaterLevel
{
  static void Postfix(ZoneSystem __instance) => WaterHelper.SetLevel(__instance);
}
[HarmonyPatch(typeof(ClutterSystem), nameof(ClutterSystem.Awake))]
public class SetClutterSystemWaterLevel
{
  static void Postfix(ClutterSystem __instance) => WaterHelper.SetLevel(__instance);
}
[HarmonyPatch(typeof(WaterVolume), nameof(WaterVolume.Awake))]
public class SetWaterVolumeWaterLevel
{
  static void Postfix(WaterVolume __instance)
  {
    WaterHelper.SetLevel(__instance);
    WaterHelper.SetWaveSize(__instance);
  }
}
[HarmonyPatch(typeof(WaterVolume), nameof(WaterVolume.CalcWave), new[] { typeof(Vector3), typeof(float), typeof(float), typeof(float) })]
public class CalcWave
{
  static float Postfix(float result) => result * Configuration.WaveMultiplier;
}
[HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.Awake))]
public class Reset
{
  static void Postfix() => WaterHelper.OriginalScale = Vector3.zero;
}

