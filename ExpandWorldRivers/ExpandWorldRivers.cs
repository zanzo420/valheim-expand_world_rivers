using System;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Service;

namespace ExpandWorldRivers;
[BepInPlugin(GUID, NAME, VERSION)]
public class EWR : BaseUnityPlugin
{
  public const string GUID = "expand_world_rivers";
  public const string NAME = "Expand World Rivers";
  public const string VERSION = "1.0";
#nullable disable
  public static ManualLogSource Log;
#nullable enable
  public static ServerSync.ConfigSync ConfigSync = new(GUID)
  {
    DisplayName = NAME,
    CurrentVersion = VERSION,
    ModRequired = true,
    IsLocked = true
  };
  public static string ConfigName = $"{GUID}.cfg";
  public void Awake()
  {
    Log = Logger;
    var didMigration = CheckMigration();
    ConfigWrapper wrapper = new(Config, ConfigSync, InvokeRegenerate);
    if (didMigration) CleanConfig();
    Configuration.Init(wrapper);
    Harmony harmony = new(GUID);
    harmony.PatchAll();
    try
    {
      SetupWatcher();
    }
    catch (Exception e)
    {
      Log.LogError(e);
    }
  }
  private bool CheckMigration()
  {
    if (File.Exists(Path.Combine(Paths.ConfigPath, ConfigName))) return false;
    Log.LogWarning("Migrating old config file.");
    File.Copy(Path.Combine(Paths.ConfigPath, "expand_world.cfg"), Path.Combine(Paths.ConfigPath, ConfigName));
    return true;
  }
  private void CleanConfig()
  {
    if (File.Exists(Path.Combine(Paths.ConfigPath, ConfigName)))
      File.Delete(Path.Combine(Paths.ConfigPath, ConfigName));
    Config.Save();
  }
  public void InvokeRegenerate()
  {
    // Debounced for smooth config editing.
    CancelInvoke("Regenerate");
    Invoke("Regenerate", 1.0f);
  }
  public void Regenerate() => Generate.World();
  public void Start()
  {
    EWS.Run();
  }
#pragma warning disable IDE0051
  private void OnDestroy()
  {
    Config.Save();
  }
#pragma warning restore IDE0051

  private void SetupWatcher()
  {
    FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigName);
    watcher.Changed += ReadConfigValues;
    watcher.Created += ReadConfigValues;
    watcher.Renamed += ReadConfigValues;
    watcher.IncludeSubdirectories = true;
    watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
    watcher.EnableRaisingEvents = true;
  }
  private void ReadConfigValues(object sender, FileSystemEventArgs e)
  {
    if (!File.Exists(Config.ConfigFilePath)) return;
    try
    {
      Config.Reload();
    }
    catch
    {
      Log.LogError($"There was an issue loading your {Config.ConfigFilePath}");
      Log.LogError("Please check your config entries for spelling and format!");
    }
  }

}
