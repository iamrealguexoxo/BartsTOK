using System;
using System.Windows;

namespace BartsTOK
{
    public partial class AdvancedSettingsWindow : Window
    {
        public bool PlannerEnabled { get; set; }
        public int StopAfterMinutes { get; set; }
        public bool AutoStart { get; set; }
        // new settings
        public bool HideMoveMouseWindow { get; set; }
        public bool TopmostWhenRunning { get; set; }
        public bool MinimiseWhenNotRunning { get; set; }
        public bool HideFromTaskbar { get; set; }
        public bool HideFromAltTab { get; set; }
        public string OverrideTitle { get; set; } = "";
        public bool ScreenBurnPrevention { get; set; }
        public bool HideTrayIcon { get; set; }
        public bool TrayNotifications { get; set; }
        public bool ShowStatusOnMain { get; set; }
        public bool DisableButtonAnimations { get; set; }
    public System.Collections.Generic.List<ScheduleEntry> Schedules { get; set; } = new System.Collections.Generic.List<ScheduleEntry>();

        // behaviour properties
        public bool RepeatEnabled { get; set; }
        public int RepeatIntervalSeconds { get; set; } = 30;
        public string RepeatIntervalUnit { get; set; } = "Seconds";
        public bool AutoStopOnUserActivity { get; set; }
        public bool LaunchMoveMouseAtStartup { get; set; }
        public bool StartActionsWhenMoveMouseLaunched { get; set; }
        public bool AdjustVolumeWhenMoveMouseRunning { get; set; }
        public int AdjustVolumePercent { get; set; } = 80;
        public bool ContinueWhenSessionLocked { get; set; }
        public bool PauseWhenOnBattery { get; set; }
        public bool EnableFileLogging { get; set; }

        public AdvancedSettingsWindow()
        {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            // read values from controls into properties
            try
            {
                PlannerEnabled = cbPlannerEnable.IsChecked == true;
                AutoStart = cbAutoStart.IsChecked == true;
                HideMoveMouseWindow = cbHideMoveMouseWindow.IsChecked == true;
                TopmostWhenRunning = cbTopmostWhenRunning.IsChecked == true;
                MinimiseWhenNotRunning = cbMinimiseWhenNotRunning.IsChecked == true;
                HideFromTaskbar = cbHideFromTaskbar.IsChecked == true;
                HideFromAltTab = cbHideFromAltTab.IsChecked == true;
                OverrideTitle = txtOverrideTitle.Text ?? "";
                ScreenBurnPrevention = cbScreenBurnPrevention.IsChecked == true;
                HideTrayIcon = cbHideTrayIcon.IsChecked == true;
                TrayNotifications = cbTrayNotifications.IsChecked == true;
                ShowStatusOnMain = cbShowStatusOnMain.IsChecked == true;
                DisableButtonAnimations = cbDisableButtonAnimations.IsChecked == true;
                if (!int.TryParse(txtStopAfterMinutes.Text, out int mins)) mins = 0;
                StopAfterMinutes = Math.Max(0, mins);

                // behaviour reads
                RepeatEnabled = cbRepeatEnabled.IsChecked == true;
                if (!int.TryParse(txtRepeatInterval.Text, out int rint)) rint = 30;
                RepeatIntervalSeconds = Math.Max(1, rint);
                RepeatIntervalUnit = (cbRepeatUnit.SelectedItem is System.Windows.Controls.ComboBoxItem ui) ? ui.Content.ToString() ?? "Seconds" : "Seconds";
                AutoStopOnUserActivity = cbAutoStopOnUserActivity.IsChecked == true;
                LaunchMoveMouseAtStartup = cbLaunchMoveMouseAtStartup.IsChecked == true;
                StartActionsWhenMoveMouseLaunched = cbStartActionsWhenMoveLaunched.IsChecked == true;
                AdjustVolumeWhenMoveMouseRunning = cbAdjustVolume.IsChecked == true;
                if (!int.TryParse(txtAdjustVolumePercent.Text, out int vp)) vp = 80;
                AdjustVolumePercent = Math.Clamp(vp, 0, 100);
                ContinueWhenSessionLocked = cbContinueWhenLocked.IsChecked == true;
                PauseWhenOnBattery = cbPauseOnBattery.IsChecked == true;
                EnableFileLogging = cbEnableFileLogging.IsChecked == true;

                this.DialogResult = true;
            }
            catch
            {
                this.DialogResult = false;
            }
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        // helper to initialize controls from existing property values
        public void LoadValues()
        {
            cbPlannerEnable.IsChecked = PlannerEnabled;
            cbAutoStart.IsChecked = AutoStart;
            txtStopAfterMinutes.Text = StopAfterMinutes.ToString();
            cbHideMoveMouseWindow.IsChecked = HideMoveMouseWindow;
            cbTopmostWhenRunning.IsChecked = TopmostWhenRunning;
            cbMinimiseWhenNotRunning.IsChecked = MinimiseWhenNotRunning;
            cbHideFromTaskbar.IsChecked = HideFromTaskbar;
            cbHideFromAltTab.IsChecked = HideFromAltTab;
            txtOverrideTitle.Text = OverrideTitle ?? "";
            cbScreenBurnPrevention.IsChecked = ScreenBurnPrevention;
            cbHideTrayIcon.IsChecked = HideTrayIcon;
            cbTrayNotifications.IsChecked = TrayNotifications;
            cbShowStatusOnMain.IsChecked = ShowStatusOnMain;
            cbDisableButtonAnimations.IsChecked = DisableButtonAnimations;
            // load schedules into listbox
            try
            {
                lstSchedules.Items.Clear();
                foreach (var s in Schedules)
                    lstSchedules.Items.Add($"{(s.Enabled?"[x]":"[ ]")} {s.Time} {s.Action} - {s.Name} ({s.Days})");
            }
            catch { }
            // behaviour controls
            try
            {
                cbRepeatEnabled.IsChecked = RepeatEnabled;
                txtRepeatInterval.Text = RepeatIntervalSeconds.ToString();
                if (RepeatIntervalUnit == "Minutes") cbRepeatUnit.SelectedIndex = 1; else cbRepeatUnit.SelectedIndex = 0;
                cbAutoStopOnUserActivity.IsChecked = AutoStopOnUserActivity;
                cbLaunchMoveMouseAtStartup.IsChecked = LaunchMoveMouseAtStartup;
                cbStartActionsWhenMoveLaunched.IsChecked = StartActionsWhenMoveMouseLaunched;
                cbAdjustVolume.IsChecked = AdjustVolumeWhenMoveMouseRunning;
                txtAdjustVolumePercent.Text = AdjustVolumePercent.ToString();
                cbContinueWhenLocked.IsChecked = ContinueWhenSessionLocked;
                cbPauseOnBattery.IsChecked = PauseWhenOnBattery;
                cbEnableFileLogging.IsChecked = EnableFileLogging;
            }
            catch { }
        }

        private void BtnAddSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new ScheduleEditorWindow();
                dlg.Entry = new ScheduleEntry();
                dlg.LoadEntry();
                dlg.Owner = this;
                if (dlg.ShowDialog() == true)
                {
                    Schedules.Add(dlg.Entry);
                    LoadValues();
                }
            }
            catch { }
        }

        private void BtnEditSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idx = lstSchedules.SelectedIndex;
                if (idx >= 0 && idx < Schedules.Count)
                {
                    var entry = Schedules[idx];
                    var dlg = new ScheduleEditorWindow();
                    dlg.Entry = entry;
                    dlg.LoadEntry();
                    dlg.Owner = this;
                    if (dlg.ShowDialog() == true)
                    {
                        Schedules[idx] = dlg.Entry;
                        LoadValues();
                    }
                }
            }
            catch { }
        }

        private void BtnRemoveSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idx = lstSchedules.SelectedIndex;
                if (idx >= 0 && idx < Schedules.Count)
                {
                    Schedules.RemoveAt(idx);
                    LoadValues();
                }
            }
            catch { }
        }
    }
}
