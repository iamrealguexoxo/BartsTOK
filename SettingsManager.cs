using System;
using System.IO;
using System.Text.Json;

namespace BartsTOK
{
    public class AppSettings
    {
        public bool PlannerEnabled { get; set; } = false;
        public int StopAfterMinutes { get; set; } = 0;
        public bool AutoStart { get; set; } = false;

        public bool HideMoveMouseWindow { get; set; } = false;
        public bool TopmostWhenRunning { get; set; } = false;
        public bool MinimiseWhenNotRunning { get; set; } = false;
        public bool HideFromTaskbar { get; set; } = false;
        public bool HideFromAltTab { get; set; } = false;
        public string OverrideTitle { get; set; } = "";
        public bool ScreenBurnPrevention { get; set; } = false;
        public bool HideTrayIcon { get; set; } = false;
        public bool TrayNotifications { get; set; } = false;
        public bool ShowStatusOnMain { get; set; } = true;
        public bool DisableButtonAnimations { get; set; } = false;
        // Schedules for multi-entry planner
        public System.Collections.Generic.List<ScheduleEntry> Schedules { get; set; } = new System.Collections.Generic.List<ScheduleEntry>();
        // Additional behaviour fields requested
        public bool RepeatEnabled { get; set; } = false;
        public int RepeatIntervalSeconds { get; set; } = 30;
        public string RepeatIntervalUnit { get; set; } = "Seconds"; // Seconds or Minutes

        public bool AutoStopOnUserActivity { get; set; } = false;
        public bool LaunchMoveMouseAtStartup { get; set; } = false;
        public bool StartActionsWhenMoveMouseLaunched { get; set; } = false;
        public bool AdjustVolumeWhenMoveMouseRunning { get; set; } = false;
        public int AdjustVolumePercent { get; set; } = 80;
        public bool ContinueWhenSessionLocked { get; set; } = false;
        public bool PauseWhenOnBattery { get; set; } = false;
        public bool EnableFileLogging { get; set; } = false;
    }

    public class ScheduleEntry
    {
        public string Id { get; set; } = System.Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        // time in HH:mm format
        public string Time { get; set; } = "00:00";
        // action: "Start" or "Stop"
        public string Action { get; set; } = "Start";
        // days as comma-separated names (e.g. "Mon,Tue,Wed") or "Everyday"
        public string Days { get; set; } = "Everyday";
        public bool Enabled { get; set; } = true;
    }

    public static class SettingsManager
    {
        private static string SettingsFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BartsTOK");
        private static string SettingsFile => Path.Combine(SettingsFolder, "settings.json");

        public static AppSettings Load()
        {
            try
            {
                if (!Directory.Exists(SettingsFolder)) Directory.CreateDirectory(SettingsFolder);
                if (!File.Exists(SettingsFile))
                {
                    var def = new AppSettings();
                    Save(def);
                    return def;
                }

                var json = File.ReadAllText(SettingsFile);
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var s = JsonSerializer.Deserialize<AppSettings>(json, opts);
                return s ?? new AppSettings();
            }
            catch
            {
                return new AppSettings();
            }
        }

        public static void Save(AppSettings settings)
        {
            try
            {
                if (!Directory.Exists(SettingsFolder)) Directory.CreateDirectory(SettingsFolder);
                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsFile, json);
            }
            catch
            {
                // ignore save errors
            }
        }
    }
}
