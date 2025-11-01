using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Threading.Tasks;
using WinForms = System.Windows.Forms;
using Drawing = System.Drawing;
using Microsoft.Win32;
using System.IO;

namespace BartsTOK
{
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
    private const uint KEYEVENTF_KEYUP = 2;
    private DispatcherTimer mainTimer = new DispatcherTimer();
        private bool isRunning = false;
        private List<string> textLines = new List<string>();
        private int currentLineIndex = 0;
        private Random random = new Random();
    private WinForms.NotifyIcon? trayIcon;
        private bool allowClose = false;
    // Input anchor (send text to a target window)
    private IntPtr inputTargetHwnd = IntPtr.Zero;
    private string inputTargetTitle = "";
        // GIF animation support (frames + timer)
        private List<System.Windows.Media.Imaging.BitmapSource>? gifFrames;
        private List<int>? gifFrameDelays; // in milliseconds
        private DispatcherTimer? gifTimer;
        private int currentGifIndex = 0;
    // Set-Input-Target countdown timer (when user clicks Set Target)
    private DispatcherTimer? setTargetTimer;
    private int setTargetCountdown = 0;
    private bool isSettingTarget = false;
    // screen burn prevention timers
    private DispatcherTimer? burnTimer;
    private DispatcherTimer? burnRestoreTimer;
    private System.Drawing.Point burnOriginalPos;
    // user activity detection
    [StructLayout(LayoutKind.Sequential)]
    private struct LASTINPUTINFO { public uint cbSize; public uint dwTime; }
    [DllImport("user32.dll")] private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
    private uint startTickAtRun = 0;

    // volume adjust (winmm)
    [DllImport("winmm.dll")] private static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);
    [DllImport("winmm.dll")] private static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
    private uint previousWaveVolume = 0;
    private bool volumeAdjusted = false;
    // Planner / advanced settings (stored values; configured via AdvancedSettingsWindow)
    private DispatcherTimer? stopAfterTimer;
    private int stopAfterSecondsRemaining = 0;
        private bool plannerEnabled = false;
    private int stopAfterMinutes = 0;
    private bool autoStart = false;
    // scheduler
    private DispatcherTimer? scheduleTimer;
    private System.Collections.Generic.Dictionary<string, DateTime> scheduleLastRun = new System.Collections.Generic.Dictionary<string, DateTime>();
    // persisted app settings
    private AppSettings currentSettings = new AppSettings();

        public MainWindow()
        {
            InitializeComponent();
            mainTimer.Tick += MainTimer_Tick;

            // Planner timer (stop-after) initialization
            stopAfterTimer = new DispatcherTimer();
            stopAfterTimer.Interval = TimeSpan.FromSeconds(1);
            stopAfterTimer.Tick += StopAfterTimer_Tick;

            // Load persisted settings
            try
            {
                currentSettings = SettingsManager.Load();
                plannerEnabled = currentSettings.PlannerEnabled;
                stopAfterMinutes = currentSettings.StopAfterMinutes;
                autoStart = currentSettings.AutoStart;
                // reflect other toggles if you want to apply them to UI immediately
            }
            catch { }

            // No auto-start by default; advanced settings dialog will control these values

            // Anchor mouse feature was removed — no timer initialization here

            // --- Tray (NotifyIcon) setup ---------------------------------
            try
            {
                trayIcon = new WinForms.NotifyIcon();

                var context = new WinForms.ContextMenuStrip();

                var miSettings = new WinForms.ToolStripMenuItem("Einstellungen ...");
                miSettings.Click += (s, e) =>
                {
                    // For now show a simple dialog - replace with a real settings window if you have one
                    System.Windows.MessageBox.Show("Einstellungen öffnen (Platzhalter)", "Einstellungen");
                };

                var miStart = new WinForms.ToolStripMenuItem("Bart Starten");
                miStart.Click += (s, e) => Dispatcher.Invoke(() => BtnStart_Click(this, new RoutedEventArgs()));

                var miStop = new WinForms.ToolStripMenuItem("Bart Stoppen");
                miStop.Click += (s, e) => Dispatcher.Invoke(() => BtnStop_Click(this, new RoutedEventArgs()));

                var miHelp = new WinForms.ToolStripMenuItem("Hilfe Anzeigen ...");
                miHelp.Click += (s, e) => System.Windows.MessageBox.Show("Hilfe (Platzhalter)", "Hilfe");

                var miExit = new WinForms.ToolStripMenuItem("Beenden");
                miExit.Click += (s, e) =>
                {
                    // Allow the app to actually close when exit is selected from the tray
                    allowClose = true;
                    if (trayIcon != null)
                    {
                        trayIcon.Visible = false;
                        trayIcon.Dispose();
                    }
                    System.Windows.Application.Current.Shutdown();
                };

                context.Items.Add(miSettings);
                context.Items.Add(miStart);
                context.Items.Add(miStop);
                context.Items.Add(new WinForms.ToolStripSeparator());
                context.Items.Add(miHelp);
                context.Items.Add(new WinForms.ToolStripSeparator());
                context.Items.Add(miExit);

                trayIcon.ContextMenuStrip = context;

                // Try to load the embedded resource icon first (pack URI), fallback to file in app folder
                try
                {
                    var res = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/bart.ico"));
                    if (res != null)
                    {
                        trayIcon.Icon = new Drawing.Icon(res.Stream);
                    }
                    else
                    {
                        var file = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bart.ico");
                        if (System.IO.File.Exists(file))
                            trayIcon.Icon = new Drawing.Icon(file);
                    }
                }
                catch
                {
                    // ignore icon load errors; tray will appear with default icon if necessary
                }

                    // honor persisted setting: only show tray icon if not explicitly hidden
                    try { trayIcon.Visible = !(currentSettings?.HideTrayIcon == true); } catch { trayIcon.Visible = false; }

                trayIcon.DoubleClick += (s, e) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        Show();
                        WindowState = WindowState.Normal;
                        trayIcon.Visible = false;
                        // hide tray icon again when restoring if user prefers
                        try { if (trayIcon != null && !(currentSettings?.HideTrayIcon == true)) trayIcon.Visible = false; } catch { if (trayIcon != null) trayIcon.Visible = false; }
                        Activate();
                    });
                };
            }
            catch
            {
                // If NotifyIcon can't be created, we silently ignore so app still runs.
            }
            // ---------------------------------------------------------------
            // Load GIF frames for imgLoading so we can animate manually if WPF doesn't auto-play
            try
            {
                var streamInfo = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/loading.gif"));
                if (streamInfo != null)
                {
                    using (var s = streamInfo.Stream)
                    {
                        var decoder = new System.Windows.Media.Imaging.GifBitmapDecoder(s, System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat, System.Windows.Media.Imaging.BitmapCacheOption.OnLoad);
                        gifFrames = new List<System.Windows.Media.Imaging.BitmapSource>();
                        gifFrameDelays = new List<int>();
                        foreach (var frame in decoder.Frames)
                        {
                            gifFrames.Add(frame);
                            int delay = 100; // default 100ms
                            try
                            {
                                if (frame.Metadata is System.Windows.Media.Imaging.BitmapMetadata meta)
                                {
                                    // GIF frame delay is stored in /grctlext/Delay in hundredths of a second
                            object? q = null;
                                    if (meta.ContainsQuery("/grctlext/Delay"))
                                        q = meta.GetQuery("/grctlext/Delay");
                                    if (q is ushort us)
                                        delay = Math.Max(20, us * 10); // convert to ms, enforce min
                                    else if (q is byte b)
                                        delay = Math.Max(20, b * 10);
                                }
                            }
                            catch
                            {
                                // ignore metadata read errors
                            }
                            gifFrameDelays.Add(delay);
                        }

                        if (gifFrames.Count > 0)
                        {
                            // set header images to first frame (they'll animate via the timer)
                            try { if (imgHeaderLeft != null) imgHeaderLeft.Source = gifFrames[0]; } catch { }
                            try { if (imgHeaderRight != null) imgHeaderRight.Source = gifFrames[0]; } catch { }
                        }

                        if (gifFrames.Count > 1)
                        {
                            gifTimer = new DispatcherTimer();
                            gifTimer.Tick += (s, e) =>
                            {
                                try
                                {
                                    if (gifFrames != null && gifFrames.Count > 0)
                                    {
                                        currentGifIndex = (currentGifIndex + 1) % gifFrames.Count;
                                        // update all images that use the frames
                                        try { if (imgHeaderLeft != null) imgHeaderLeft.Source = gifFrames[currentGifIndex]; } catch { }
                                        try { if (imgHeaderRight != null) imgHeaderRight.Source = gifFrames[currentGifIndex]; } catch { }
                                        try { if (imgLoading != null) imgLoading.Source = gifFrames[currentGifIndex]; } catch { }

                                        // update interval to next frame delay
                                        if (gifFrameDelays != null && gifFrameDelays.Count > currentGifIndex)
                                            gifTimer.Interval = TimeSpan.FromMilliseconds(gifFrameDelays[currentGifIndex]);
                                    }
                                }
                                catch { }
                            };

                            // start animating header/footer immediately
                            gifTimer.Interval = TimeSpan.FromMilliseconds(gifFrameDelays[0]);
                            gifTimer.Start();
                        }
                    }
                }
            }
            catch
            {
                // ignore GIF load errors
            }

            // Handle minimize action -> optionally send to tray
            this.StateChanged += (s, e) =>
            {
                try
                {
                    if (WindowState == WindowState.Minimized && cbMinimizeToTray != null && cbMinimizeToTray.IsChecked == true)
                    {
                        Hide();
                        // only show tray icon if user hasn't chosen to hide it globally
                        if (trayIcon != null && !(currentSettings?.HideTrayIcon == true))
                            trayIcon.Visible = true;
                    }
                }
                catch
                {
                    // ignore if UI element not ready or tray not available
                }
            };

            this.KeyDown += (s, e) =>
            {
            if (e.Key == Key.F1) BtnStart_Click(this, new RoutedEventArgs());
        if (e.Key == Key.F2) BtnStop_Click(this, new RoutedEventArgs());
            };

            // honor auto-start if requested
            try
            {
                if (currentSettings != null && currentSettings.AutoStart && !isRunning)
                {
                    // start after initialization
                    Dispatcher.BeginInvoke(new Action(() => BtnStart_Click(this, new RoutedEventArgs())), DispatcherPriority.ApplicationIdle);
                }
            }
            catch { }

            // initialize burn prevention timers according to setting
            try
            {
                if (currentSettings != null && currentSettings.ScreenBurnPrevention)
                {
                    InitBurnPrevention();
                }
            }
            catch { }

            // initialize scheduler if we have saved schedules
            try
            {
                if (currentSettings != null && currentSettings.Schedules != null && currentSettings.Schedules.Count > 0)
                    InitScheduler();
            }
            catch { }

            // Hook system events for session/power
            try
            {
                SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
                SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
            }
            catch { }
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            try
            {
                if (e.Reason == SessionSwitchReason.SessionLock)
                {
                    if (!currentSettings.ContinueWhenSessionLocked && isRunning)
                    {
                        AddLog("Session locked: stopping because setting Pause on lock is enabled");
                        BtnStop_Click(this, new RoutedEventArgs());
                    }
                }
                else if (e.Reason == SessionSwitchReason.SessionUnlock)
                {
                    // no automatic resume for now
                }
            }
            catch { }
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            try
            {
                if (e.Mode == PowerModes.StatusChange)
                {
                    var ps = WinForms.SystemInformation.PowerStatus;
                    bool onBattery = ps.PowerLineStatus != WinForms.PowerLineStatus.Online;
                    if (onBattery && currentSettings.PauseWhenOnBattery && isRunning)
                    {
                        AddLog("Power: switched to battery - pausing actions");
                        BtnStop_Click(this, new RoutedEventArgs());
                    }
                    else if (!onBattery && currentSettings.PauseWhenOnBattery)
                    {
                        // optionally resume if user wants - we won't auto-resume to avoid surprises
                    }
                }
            }
            catch { }
        }

        private void InitScheduler()
        {
            try
            {
                // stop existing
                scheduleTimer?.Stop();
                scheduleLastRun.Clear();

                // create timer to check every 30 seconds
                if (scheduleTimer == null)
                {
                    scheduleTimer = new DispatcherTimer();
                    scheduleTimer.Interval = TimeSpan.FromSeconds(30);
                    scheduleTimer.Tick += ScheduleTimer_Tick;
                }
                scheduleTimer.Start();
            }
            catch { }
        }

        private void ScheduleTimer_Tick(object? sender, EventArgs? e)
        {
            try
            {
                var now = DateTime.Now;
                if (currentSettings?.Schedules == null) return;
                foreach (var s in currentSettings.Schedules)
                {
                    try
                    {
                        if (!s.Enabled) continue;
                        if (string.IsNullOrWhiteSpace(s.Time)) continue;
                        if (!TimeSpan.TryParse(s.Time, out TimeSpan t)) continue;

                        // day match
                        bool dayMatch = false;
                        if (string.Equals(s.Days, "Everyday", StringComparison.OrdinalIgnoreCase)) dayMatch = true;
                        else
                        {
                            var parts = (s.Days ?? "").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                            var today = now.DayOfWeek;
                            foreach (var p in parts)
                            {
                                if (Enum.TryParse<DayOfWeek>(p, true, out var dw)) { if (dw == today) { dayMatch = true; break; } }
                                // accept short names Mon,Tue...
                                switch (p.Trim())
                                {
                                    case "Mon": if (today == DayOfWeek.Monday) dayMatch = true; break;
                                    case "Tue": if (today == DayOfWeek.Tuesday) dayMatch = true; break;
                                    case "Wed": if (today == DayOfWeek.Wednesday) dayMatch = true; break;
                                    case "Thu": if (today == DayOfWeek.Thursday) dayMatch = true; break;
                                    case "Fri": if (today == DayOfWeek.Friday) dayMatch = true; break;
                                    case "Sat": if (today == DayOfWeek.Saturday) dayMatch = true; break;
                                    case "Sun": if (today == DayOfWeek.Sunday) dayMatch = true; break;
                                }
                                if (dayMatch) break;
                            }
                        }
                        if (!dayMatch) continue;

                        // time match to minute
                        if (now.Hour == t.Hours && now.Minute == t.Minutes)
                        {
                            // avoid retrigger in same day
                            if (scheduleLastRun.TryGetValue(s.Id, out var last) && last.Date == now.Date) continue;

                            // trigger action
                            if (string.Equals(s.Action, "Stop", StringComparison.OrdinalIgnoreCase))
                            {
                                Dispatcher.Invoke(() => BtnStop_Click(this, new RoutedEventArgs()));
                                AddLog($"Scheduler: Stop triggered by '{s.Name}' at {s.Time}");
                            }
                            else
                            {
                                Dispatcher.Invoke(() => BtnStart_Click(this, new RoutedEventArgs()));
                                AddLog($"Scheduler: Start triggered by '{s.Name}' at {s.Time}");
                            }
                            scheduleLastRun[s.Id] = now;
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }

        private void InitBurnPrevention()
        {
            try
            {
                if (burnTimer == null)
                {
                    burnTimer = new DispatcherTimer();
                    burnTimer.Interval = TimeSpan.FromMinutes(5);
                    burnTimer.Tick += BurnTimer_Tick;
                }
                if (burnRestoreTimer == null)
                {
                    burnRestoreTimer = new DispatcherTimer();
                    burnRestoreTimer.Interval = TimeSpan.FromMilliseconds(800);
                    burnRestoreTimer.Tick += BurnRestoreTimer_Tick;
                }
                burnTimer.Start();
            }
            catch { }
        }

        private void BurnTimer_Tick(object? sender, EventArgs? e)
        {
            try
            {
                if (burnTimer != null) burnTimer.Stop();
                burnOriginalPos = System.Windows.Forms.Cursor.Position;
                try
                {
                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(burnOriginalPos.X + 1, burnOriginalPos.Y);
                }
                catch { }
                try { burnRestoreTimer?.Start(); } catch { }
            }
            catch { }
        }

        private void BurnRestoreTimer_Tick(object? sender, EventArgs? e)
        {
            try
            {
                burnRestoreTimer?.Stop();
                try { System.Windows.Forms.Cursor.Position = burnOriginalPos; } catch { }
                try { burnTimer?.Start(); } catch { }
            }
            catch { }
        }

    private void BtnStart_Click(object? sender, RoutedEventArgs? e)
        {
            string text = txtInput.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                System.Windows.MessageBox.Show("Bitte geben Sie Text ein!");
                return;
            }

            if (!double.TryParse(txtLinePause.Text, out double linePause) || linePause < 0)
            {
                System.Windows.MessageBox.Show("Ungültige Pause!");
                return;
            }

            textLines = new List<string>(text.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
            currentLineIndex = 0;
            isRunning = true;

            // decide interval: either global repeat setting or per-line pause
            try
            {
                if (currentSettings != null && currentSettings.RepeatEnabled)
                {
                    int secs = currentSettings.RepeatIntervalSeconds;
                    if (string.Equals(currentSettings.RepeatIntervalUnit, "Minutes", StringComparison.OrdinalIgnoreCase)) secs = Math.Max(1, secs * 60);
                    mainTimer.Interval = TimeSpan.FromSeconds(Math.Max(1, secs));
                }
                else
                {
                    mainTimer.Interval = TimeSpan.FromSeconds(linePause);
                }

                // if PauseWhenOnBattery is enabled and we are on battery, don't start
                try
                {
                    if (currentSettings != null && currentSettings.PauseWhenOnBattery)
                    {
                        var ps = WinForms.SystemInformation.PowerStatus;
                        bool onBattery = ps.PowerLineStatus != WinForms.PowerLineStatus.Online;
                        if (onBattery)
                        {
                            AddLog("Start aborted: system on battery and PauseWhenOnBattery is enabled");
                            return;
                        }
                    }
                }
                catch { }

                mainTimer.Start();
            }
            catch { mainTimer.Start(); }

            // If planner 'stop after' is enabled, start the stopAfterTimer
            try
            {
                if (plannerEnabled && stopAfterMinutes > 0)
                {
                    stopAfterSecondsRemaining = stopAfterMinutes * 60;
                    stopAfterTimer?.Start();
                    AddLog($"Planer aktiviert: Stoppe nach {stopAfterMinutes} Minuten");
                }
            }
            catch { }

            // record input tick at start so we can detect user activity later
            try
            {
                if (currentSettings != null && currentSettings.AutoStopOnUserActivity)
                {
                    LASTINPUTINFO li = new LASTINPUTINFO(); li.cbSize = (uint)Marshal.SizeOf<LASTINPUTINFO>(); li.dwTime = 0;
                    if (GetLastInputInfo(ref li)) startTickAtRun = li.dwTime; else startTickAtRun = (uint)Environment.TickCount;
                }
                else startTickAtRun = 0;
            }
            catch { startTickAtRun = 0; }

            // adjust volume if requested
            try
            {
                if (currentSettings != null && currentSettings.AdjustVolumeWhenMoveMouseRunning)
                {
                    if (waveOutGetVolume(IntPtr.Zero, out previousWaveVolume) == 0)
                    {
                        int vp = Math.Clamp(currentSettings.AdjustVolumePercent, 0, 100);
                        ushort v = (ushort)((vp * 0xFFFF) / 100);
                        uint newVol = ((uint)v) | ((uint)v << 16);
                        waveOutSetVolume(IntPtr.Zero, newVol);
                        volumeAdjusted = true;
                    }
                }
            }
            catch { }

            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            lblStatus.Content = "Status: Läuft ▶";
            try
            {
                if (imgLoading != null)
                {
                    imgLoading.Visibility = Visibility.Visible;
                    if (gifFrames != null && gifFrames.Count > 0)
                    {
                        currentGifIndex = 0;
                        imgLoading.Source = gifFrames[0];
                        if (gifTimer != null && gifFrameDelays != null && gifFrameDelays.Count > 0)
                        {
                            gifTimer.Interval = TimeSpan.FromMilliseconds(gifFrameDelays[0]);
                            gifTimer.Start();
                        }
                    }
                }
            }
            catch { }
            AddLog("✓ Barts TOK gestartet");

            // show tray notification if enabled
            try
            {
                if (currentSettings != null && currentSettings.TrayNotifications && trayIcon != null && !(currentSettings.HideTrayIcon))
                {
                    trayIcon.ShowBalloonTip(2500, "Barts TOK", "Barts TOK gestartet", WinForms.ToolTipIcon.Info);
                }
            }
            catch { }
        }

    private void BtnStop_Click(object? sender, RoutedEventArgs? e)
        {
            isRunning = false;
            mainTimer.Stop();

            // stop planner timer if running
            try { stopAfterTimer?.Stop(); stopAfterSecondsRemaining = 0; } catch { }

            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            lblStatus.Content = "Status: Gestoppt ⏹";
            try
            {
                if (gifTimer != null)
                {
                    gifTimer.Stop();
                    currentGifIndex = 0;
                }
                if (imgLoading != null)
                {
                    imgLoading.Visibility = Visibility.Collapsed;
                    // reset to first frame if available
                    if (gifFrames != null && gifFrames.Count > 0)
                        imgLoading.Source = gifFrames[0];
                }
            }
            catch { }
            AddLog("✗ Barts TOK gestoppt");

            // show tray notification if enabled
            try
            {
                if (currentSettings != null && currentSettings.TrayNotifications && trayIcon != null && !(currentSettings.HideTrayIcon))
                {
                    trayIcon.ShowBalloonTip(2500, "Barts TOK", "Barts TOK gestoppt", WinForms.ToolTipIcon.Info);
                }
            }
            catch { }

            // restore volume if we changed it
            try
            {
                if (volumeAdjusted)
                {
                    waveOutSetVolume(IntPtr.Zero, previousWaveVolume);
                    volumeAdjusted = false;
                }
            }
            catch { }
        }

        private void BtnTray_Click(object? sender, RoutedEventArgs? e)
        {
            // minimize to tray
            Hide();
            WindowState = WindowState.Minimized;
            if (trayIcon != null && !(currentSettings?.HideTrayIcon == true))
                trayIcon.Visible = true;
        }

        private void BtnOpenMoveBart_Click(object? sender, RoutedEventArgs? e)
        {
            try
            {
                var win = new MoveMouseWindow();
                win.Owner = this;
                // apply runtime options from persisted settings
                try
                {
                    win.TopmostWhenRunning = currentSettings.TopmostWhenRunning;
                    win.MinimiseWhenNotRunning = currentSettings.MinimiseWhenNotRunning;
                    win.HideFromTaskbar = currentSettings.HideFromTaskbar;
                    win.HideFromAltTab = currentSettings.HideFromAltTab;
                    win.HideWindowOnOpen = currentSettings.HideMoveMouseWindow;
                    win.OverrideTitle = currentSettings.OverrideTitle ?? "";
                }
                catch { }
                win.Show();
                // optionally start actions when MoveMouse is launched
                try
                {
                    if (currentSettings != null && currentSettings.StartActionsWhenMoveMouseLaunched)
                    {
                        if (!isRunning) BtnStart_Click(this, new RoutedEventArgs());
                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Fehler beim Öffnen von MoveBart: " + ex.Message, "Fehler");
            }
        }

        private void CreateOrRemoveStartupShortcut(bool create)
        {
            try
            {
                var startup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                var exe = System.Reflection.Assembly.GetEntryAssembly()?.Location ?? System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
                if (string.IsNullOrWhiteSpace(exe)) return;
                var lnk = Path.Combine(startup, "BartsTOK - MoveMouse.lnk");
                if (!create)
                {
                    if (File.Exists(lnk)) File.Delete(lnk);
                    return;
                }

                // create via WScript.Shell COM
                try
                {
                    var t = Type.GetTypeFromProgID("WScript.Shell");
                    if (t != null)
                    {
                        object? shellObj = Activator.CreateInstance(t);
                        if (shellObj != null)
                        {
                            dynamic shell = shellObj;
                            dynamic shortcut = shell.CreateShortcut(lnk);
                            shortcut.TargetPath = exe;
                            shortcut.WorkingDirectory = Path.GetDirectoryName(exe);
                            shortcut.WindowStyle = 1;
                            shortcut.Description = "Barts TOK - Move Mouse";
                            shortcut.Save();
                        }
                    }
                }
                catch { }
            }
            catch { }
        }

        private void BtnAdvancedSettings_Click(object? sender, RoutedEventArgs? e)
        {
            try
            {
                var dlg = new AdvancedSettingsWindow();
                // initialize with current values (from persisted settings)
                dlg.PlannerEnabled = currentSettings.PlannerEnabled;
                dlg.StopAfterMinutes = currentSettings.StopAfterMinutes;
                dlg.AutoStart = currentSettings.AutoStart;
                dlg.HideMoveMouseWindow = currentSettings.HideMoveMouseWindow;
                dlg.TopmostWhenRunning = currentSettings.TopmostWhenRunning;
                dlg.MinimiseWhenNotRunning = currentSettings.MinimiseWhenNotRunning;
                dlg.HideFromTaskbar = currentSettings.HideFromTaskbar;
                dlg.HideFromAltTab = currentSettings.HideFromAltTab;
                dlg.OverrideTitle = currentSettings.OverrideTitle;
                dlg.ScreenBurnPrevention = currentSettings.ScreenBurnPrevention;
                dlg.HideTrayIcon = currentSettings.HideTrayIcon;
                dlg.TrayNotifications = currentSettings.TrayNotifications;
                dlg.ShowStatusOnMain = currentSettings.ShowStatusOnMain;
                dlg.DisableButtonAnimations = currentSettings.DisableButtonAnimations;
                dlg.LoadValues();
                dlg.Owner = this;
                bool? res = dlg.ShowDialog();
                if (res == true)
                {
                    // read back values and persist
                    currentSettings.PlannerEnabled = dlg.PlannerEnabled;
                    currentSettings.StopAfterMinutes = dlg.StopAfterMinutes;
                    currentSettings.AutoStart = dlg.AutoStart;
                    currentSettings.HideMoveMouseWindow = dlg.HideMoveMouseWindow;
                    currentSettings.TopmostWhenRunning = dlg.TopmostWhenRunning;
                    currentSettings.MinimiseWhenNotRunning = dlg.MinimiseWhenNotRunning;
                    currentSettings.HideFromTaskbar = dlg.HideFromTaskbar;
                    currentSettings.HideFromAltTab = dlg.HideFromAltTab;
                    currentSettings.OverrideTitle = dlg.OverrideTitle ?? "";
                    currentSettings.ScreenBurnPrevention = dlg.ScreenBurnPrevention;
                    currentSettings.HideTrayIcon = dlg.HideTrayIcon;
                    currentSettings.TrayNotifications = dlg.TrayNotifications;
                    currentSettings.ShowStatusOnMain = dlg.ShowStatusOnMain;
                    currentSettings.DisableButtonAnimations = dlg.DisableButtonAnimations;
                    currentSettings.Schedules = dlg.Schedules ?? new System.Collections.Generic.List<BartsTOK.ScheduleEntry>();
                    // new behaviour fields
                    currentSettings.RepeatEnabled = dlg.RepeatEnabled;
                    currentSettings.RepeatIntervalSeconds = dlg.RepeatIntervalSeconds;
                    currentSettings.RepeatIntervalUnit = dlg.RepeatIntervalUnit;
                    currentSettings.AutoStopOnUserActivity = dlg.AutoStopOnUserActivity;
                    currentSettings.LaunchMoveMouseAtStartup = dlg.LaunchMoveMouseAtStartup;
                    currentSettings.StartActionsWhenMoveMouseLaunched = dlg.StartActionsWhenMoveMouseLaunched;
                    currentSettings.AdjustVolumeWhenMoveMouseRunning = dlg.AdjustVolumeWhenMoveMouseRunning;
                    currentSettings.AdjustVolumePercent = dlg.AdjustVolumePercent;
                    currentSettings.ContinueWhenSessionLocked = dlg.ContinueWhenSessionLocked;
                    currentSettings.PauseWhenOnBattery = dlg.PauseWhenOnBattery;
                    currentSettings.EnableFileLogging = dlg.EnableFileLogging;

                    // reinitialize scheduler with new schedules
                    try { InitScheduler(); } catch { }

                    // copy to runtime fields used elsewhere
                    plannerEnabled = currentSettings.PlannerEnabled;
                    stopAfterMinutes = currentSettings.StopAfterMinutes;
                    autoStart = currentSettings.AutoStart;

                    // persist
                    try { SettingsManager.Save(currentSettings); } catch { }

                    // create or remove startup shortcut according to setting
                    try { CreateOrRemoveStartupShortcut(currentSettings.LaunchMoveMouseAtStartup); } catch { }

                    AddLog($"Erweiterte Einstellungen gespeichert: Planer={(plannerEnabled?"ein":"aus")}, StopAfter={stopAfterMinutes}min, AutoStart={(autoStart?"ein":"aus")} ");

                    // if auto-start requested right away and bot is not running, start it
                    if (currentSettings.AutoStart && !isRunning)
                    {
                        BtnStart_Click(this, new RoutedEventArgs());
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog("Fehler beim Öffnen der Erweiterten Einstellungen: " + ex.Message);
            }
        }

        private void StopAfterTimer_Tick(object? sender, EventArgs? e)
        {
            try
            {
                if (stopAfterSecondsRemaining > 0)
                {
                    stopAfterSecondsRemaining--;
                    // update status with remaining time (minutes:seconds)
                    if (lblStatus != null)
                    {
                        var ts = TimeSpan.FromSeconds(stopAfterSecondsRemaining);
                        lblStatus.Content = "Status: Läuft ▶ - verbleibend " + ts.ToString(@"hh\:mm\:ss");
                    }
                    if (stopAfterSecondsRemaining <= 0)
                    {
                        stopAfterTimer?.Stop();
                        AddLog("Planer: Laufzeit abgelaufen, stoppe Bart");
                        BtnStop_Click(this, new RoutedEventArgs());
                    }
                }
            }
            catch { }
        }

        // Anchor mouse feature removed

        private void BtnSetInputTarget_Click(object? sender, RoutedEventArgs? e)
        {
            // Start a 3-second countdown, then capture the window under the mouse
            try
            {
                if (isSettingTarget)
                {
                    AddLog("SetInputTarget: bereits im Countdown");
                    return;
                }

                isSettingTarget = true;
                setTargetCountdown = 3;
                if (lblTargetWindow != null)
                    lblTargetWindow.Content = $"Setzt Ziel in {setTargetCountdown}...";
                if (btnSetInputTarget != null) btnSetInputTarget.IsEnabled = false;

                // initialize timer if needed
                if (setTargetTimer == null)
                {
                    setTargetTimer = new DispatcherTimer();
                    setTargetTimer.Interval = TimeSpan.FromSeconds(1);
                    setTargetTimer.Tick += SetTargetTimer_Tick;
                }
                setTargetTimer.Start();
            }
            catch (Exception ex)
            {
                AddLog("SetInputTarget start error: " + ex.Message);
                isSettingTarget = false;
                if (btnSetInputTarget != null) btnSetInputTarget.IsEnabled = true;
            }
        }

        private void BtnHelpTarget_Click(object? sender, RoutedEventArgs? e)
        {
            try
            {
                string msg = "Zielfenster festlegen:\n\nBewege die Maus über das gewünschte Fenster und klicke 'Set Input Target'.\nEin 3‑Sekunden-Countdown startet, danach wird das Fenster unter dem Cursor als Ziel gesetzt.\n\nTipp: Aktiviere 'Anchor Input', damit der Text an dieses Fenster gesendet wird, auch wenn Du in anderen Fenstern arbeitest.";
                System.Windows.MessageBox.Show(msg, "Hilfe: Zielfenster festlegen");
            }
            catch { }
        }

        private void SetTargetTimer_Tick(object? sender, EventArgs? e)
        {
            try
            {
                setTargetCountdown--;
                if (setTargetCountdown > 0)
                {
                    if (lblTargetWindow != null)
                        lblTargetWindow.Content = $"Setzt Ziel in {setTargetCountdown}...";
                    return;
                }

                // stop timer and perform capture
                setTargetTimer?.Stop();
                PerformSetInputTarget();
            }
            catch (Exception ex)
            {
                AddLog("SetTargetTimer_Tick error: " + ex.Message);
                setTargetTimer?.Stop();
                isSettingTarget = false;
                if (btnSetInputTarget != null) btnSetInputTarget.IsEnabled = true;
            }
        }

        private void PerformSetInputTarget()
        {
            try
            {
                if (!GetCursorPos(out POINT pt))
                {
                    AddLog("SetInputTarget: konnte Mausposition nicht lesen");
                    return;
                }

                var h = WindowFromPoint(pt);
                if (h != IntPtr.Zero)
                    h = GetAncestor(h, GA_ROOT);

                inputTargetHwnd = h;
                var sb = new System.Text.StringBuilder(512);
                GetWindowText(h, sb, sb.Capacity);
                inputTargetTitle = sb.ToString();
                if (string.IsNullOrWhiteSpace(inputTargetTitle)) inputTargetTitle = h == IntPtr.Zero ? "(none)" : "(unnamed)";
                if (lblTargetWindow != null) lblTargetWindow.Content = inputTargetTitle;
                AddLog($"Input target gesetzt: {inputTargetTitle} (hwnd: {h})");
            }
            catch (Exception ex)
            {
                AddLog("PerformSetInputTarget error: " + ex.Message);
            }
            finally
            {
                isSettingTarget = false;
                if (btnSetInputTarget != null) btnSetInputTarget.IsEnabled = true;
            }
        }

        // Anchor mouse feature removed

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern short VkKeyScan(char ch);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    private static extern IntPtr WindowFromPoint(POINT Point);

    [DllImport("user32.dll")]
    private static extern IntPtr GetAncestor(IntPtr hwnd, uint gaFlags);

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT { public int X; public int Y; }

    private const uint GA_ROOT = 2;

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

        private IntPtr SwitchToTargetWindow(IntPtr target)
        {
            if (target == IntPtr.Zero) return IntPtr.Zero;
            try
            {
                IntPtr prev = GetForegroundWindow();
                if (prev == target) return prev;
                uint prevThread = GetWindowThreadProcessId(prev, out _);
                uint targetThread = GetWindowThreadProcessId(target, out _);
                uint curThread = GetCurrentThreadId();
                // attach input to allow SetForeground
                AttachThreadInput(curThread, targetThread, true);
                SetForegroundWindow(target);
                AttachThreadInput(curThread, targetThread, false);
                return prev;
            }
            catch { return IntPtr.Zero; }
        }

        private void RestoreForegroundWindow(IntPtr prev)
        {
            try
            {
                if (prev != IntPtr.Zero)
                {
                    SetForegroundWindow(prev);
                }
            }
            catch { }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // If the user has chosen minimize-to-tray, cancel close and hide instead
            try
            {
                if (!allowClose && cbMinimizeToTray != null && cbMinimizeToTray.IsChecked == true)
                {
                    e.Cancel = true;
                    Hide();
                    WindowState = WindowState.Minimized;
                    if (trayIcon != null && !(currentSettings?.HideTrayIcon == true))
                        trayIcon.Visible = true;
                    return;
                }
            }
            catch { }

            if (trayIcon != null)
            {
                trayIcon.Visible = false;
                trayIcon.Dispose();
                trayIcon = null;
            }
            try
            {
                SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
                SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;
            }
            catch { }

            // restore volume if adjusted
            try
            {
                if (volumeAdjusted)
                {
                    waveOutSetVolume(IntPtr.Zero, previousWaveVolume);
                    volumeAdjusted = false;
                }
            }
            catch { }
            base.OnClosing(e);
        }

        private void MainTimer_Tick(object? sender, EventArgs? e)
        {
            if (!isRunning) return;

            // if AutoStopOnUserActivity is enabled, check last input time
            try
            {
                if (currentSettings != null && currentSettings.AutoStopOnUserActivity && startTickAtRun != 0)
                {
                    LASTINPUTINFO li = new LASTINPUTINFO(); li.cbSize = (uint)Marshal.SizeOf<LASTINPUTINFO>(); li.dwTime = 0;
                    if (GetLastInputInfo(ref li))
                    {
                        if (li.dwTime > startTickAtRun)
                        {
                            AddLog("Benutzeraktivität erkannt - stoppe Bart");
                            BtnStop_Click(this, new RoutedEventArgs());
                            return;
                        }
                    }
                }
            }
            catch { }

            string lineToSend = "";

            if (rbRandom.IsChecked == true)
            {
                lineToSend = textLines[random.Next(textLines.Count)];
            }
            else if (rbSequential.IsChecked == true || rbSequentialStop.IsChecked == true)
            {
                lineToSend = textLines[currentLineIndex];
                currentLineIndex++;

                if (currentLineIndex >= textLines.Count)
                {
                    if (rbSequentialStop.IsChecked == true)
                    {
                        BtnStop_Click(this, new RoutedEventArgs());
                        return;
                    }
                    currentLineIndex = 0;
                }
            }

            SendTextWithPause(lineToSend);

            if (cbAutoNewline.IsChecked == true)
            {
                SendKey(0x0D);
            }

            lblStatus.Content = "Status: Läuft ▶ - " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void SendTextWithPause(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            if (!double.TryParse(txtCharPause.Text, out double charPause))
                charPause = 0.01;

            // If input anchoring is enabled and a target window is set, switch to it,
            // send the text, then restore the previous foreground window.
            IntPtr prev = IntPtr.Zero;
            bool usedAnchor = false;
            try
            {
                if (cbAnchorInput != null && cbAnchorInput.IsChecked == true && inputTargetHwnd != IntPtr.Zero)
                {
                    prev = SwitchToTargetWindow(inputTargetHwnd);
                    usedAnchor = true;
                    // small delay to let target window gain focus
                    Thread.Sleep(80);
                }

                foreach (char c in text)
                {
                    SendChar(c);
                    if (charPause > 0)
                        Thread.Sleep((int)(charPause * 1000));
                }
            }
            finally
            {
                try
                {
                    if (usedAnchor)
                    {
                        // ensure small delay before restoring
                        Thread.Sleep(40);
                        RestoreForegroundWindow(prev);
                    }
                }
                catch { }
            }

            AddLog("→ Gesendet: '" + text + "'");
        }

        private void SendChar(char c)
        {
            short vk = VkKeyScan(c);
            byte key = (byte)(vk & 0xFF);
            byte shift = (byte)((vk >> 8) & 0xFF);

            if ((shift & 1) != 0) keybd_event(0x10, 0, 0, UIntPtr.Zero);
            keybd_event(key, 0, 0, UIntPtr.Zero);
            keybd_event(key, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            if ((shift & 1) != 0) keybd_event(0x10, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        private void SendKey(byte key)
        {
            keybd_event(key, 0, 0, UIntPtr.Zero);
            keybd_event(key, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            Thread.Sleep(50);
        }

        private void AddLog(string message)
        {
            try
            {
                logBox.Items.Insert(0, "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + message);
                if (logBox.Items.Count > 50) logBox.Items.RemoveAt(logBox.Items.Count - 1);
            }
            catch { }

            // optional file logging
            try
            {
                if (currentSettings != null && currentSettings.EnableFileLogging)
                {
                    var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BartsTOK");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    var logfile = Path.Combine(folder, "app.log");
                    File.AppendAllText(logfile, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
                }
            }
            catch { }
        }
    }
}