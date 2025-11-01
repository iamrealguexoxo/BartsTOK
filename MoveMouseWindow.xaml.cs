using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BartsTOK
{
    public partial class MoveMouseWindow : Window
    {
        // runtime behavior options (can be set by MainWindow when creating the window)
        public bool TopmostWhenRunning { get; set; } = false;
        public bool MinimiseWhenNotRunning { get; set; } = false;
        public bool HideFromTaskbar { get; set; } = false;
        public bool HideFromAltTab { get; set; } = false;
        public bool HideWindowOnOpen { get; set; } = false;
        public string OverrideTitle { get; set; } = "";
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;

    private DispatcherTimer moveTimer;
        private DateTime lastClick = DateTime.MinValue;
        private double angle = 0.0; // radians
        private Random rnd = new Random();
        private bool running = false;
    private System.Windows.Point centerScreen; // in screen coordinates
    // advanced features
    private System.Windows.Point currentTarget; // for interpolation
    private bool hasTarget = false;
    private const double TARGET_REACHED_PX = 8.0;
    private List<Preset> presets = new List<Preset>();

    private string PresetFilePath => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MoveBart_presets.json");

    private class Preset
    {
        public string Name { get; set; } = "";
        public double Radius { get; set; }
        public double Speed { get; set; }
        public string Mode { get; set; } = "Circle";
        public bool ClickOnMove { get; set; }
        public int ClickInterval { get; set; }
        public string ClickType { get; set; } = "Left";
        public int ClickDownMs { get; set; }
        public bool Smooth { get; set; }
        public bool Interpolation { get; set; }
        public bool FollowMouse { get; set; }
        public bool FollowWindow { get; set; }
        public string FollowWindowTitle { get; set; } = "";
    }

    private void BtnSavePreset_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var name = string.IsNullOrWhiteSpace(txtPresetName.Text) ? DateTime.Now.ToString("yyyyMMdd_HHmmss") : txtPresetName.Text.Trim();
            var p = new Preset
            {
                Name = name,
                Radius = sldRadius.Value,
                Speed = sldSpeed.Value,
                Mode = rbCircle.IsChecked == true ? "Circle" : "Random",
                ClickOnMove = cbClickOnMove.IsChecked == true,
                ClickInterval = int.TryParse(txtClickInterval.Text, out int iv) ? iv : 1000,
                ClickType = rbClickLeft.IsChecked == true ? "Left" : rbClickRight.IsChecked == true ? "Right" : "Double",
                ClickDownMs = int.TryParse(txtClickDownMs.Text, out int dm) ? dm : 30,
                Smooth = cbSmooth.IsChecked == true,
                Interpolation = cbInterpolation.IsChecked == true,
                FollowMouse = cbFollowMouse.IsChecked == true,
                FollowWindow = cbFollowWindow.IsChecked == true,
                FollowWindowTitle = txtFollowWindowTitle.Text ?? ""
            };
            presets.RemoveAll(x => x.Name == p.Name);
            presets.Add(p);
            SavePresetsToFile();
            RefreshPresetList();
            AddLog($"Preset '{p.Name}' saved");
        }
        catch (Exception ex) { AddLog("SavePreset error: " + ex.Message); }
    }

    private void BtnLoadPreset_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (lstPresets.SelectedItem is string s)
            {
                var p = presets.Find(x => x.Name == s);
                if (p != null)
                {
                    sldRadius.Value = p.Radius;
                    sldSpeed.Value = p.Speed;
                    rbCircle.IsChecked = p.Mode == "Circle";
                    rbRandom.IsChecked = p.Mode != "Circle";
                    cbClickOnMove.IsChecked = p.ClickOnMove;
                    txtClickInterval.Text = p.ClickInterval.ToString();
                    rbClickLeft.IsChecked = p.ClickType == "Left";
                    rbClickRight.IsChecked = p.ClickType == "Right";
                    rbClickDouble.IsChecked = p.ClickType == "Double";
                    txtClickDownMs.Text = p.ClickDownMs.ToString();
                    cbSmooth.IsChecked = p.Smooth;
                    cbInterpolation.IsChecked = p.Interpolation;
                    cbFollowMouse.IsChecked = p.FollowMouse;
                    cbFollowWindow.IsChecked = p.FollowWindow;
                    txtFollowWindowTitle.Text = p.FollowWindowTitle;
                    AddLog($"Preset '{p.Name}' loaded");
                }
            }
        }
        catch (Exception ex) { AddLog("LoadPreset error: " + ex.Message); }
    }

    private void SavePresetsToFile()
    {
        try
        {
            var dir = Path.GetDirectoryName(PresetFilePath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);
            var json = JsonSerializer.Serialize(presets);
            File.WriteAllText(PresetFilePath, json);
        }
        catch (Exception ex) { AddLog("SavePresetsToFile error: " + ex.Message); }
    }

    private void LoadPresetsFromFile()
    {
        try
        {
            if (File.Exists(PresetFilePath))
            {
                var json = File.ReadAllText(PresetFilePath);
                presets = JsonSerializer.Deserialize<List<Preset>>(json) ?? new List<Preset>();
            }
            RefreshPresetList();
        }
        catch (Exception ex) { AddLog("LoadPresetsFromFile error: " + ex.Message); }
    }

    private void RefreshPresetList()
    {
        try
        {
            lstPresets.Items.Clear();
            foreach (var p in presets) lstPresets.Items.Add(p.Name);
        }
        catch { }
    }

        public MoveMouseWindow()
        {
            InitializeComponent();

            moveTimer = new DispatcherTimer();
            moveTimer.Interval = TimeSpan.FromMilliseconds(20);
            moveTimer.Tick += MoveTimer_Tick;

            // hotkey handler when window focused
            this.PreviewKeyDown += MoveMouseWindow_PreviewKeyDown;
            // load presets
            LoadPresetsFromFile();

            // position the center marker roughly in the middle of the visual area
            this.Loaded += (s, e) =>
            {
                RepositionCenterMarkerToWindowCenter();
                ApplyRuntimeOptions();
            };

            this.MouseLeftButtonDown += (s, e) => { DragMove(); };
        }

        private void RepositionCenterMarkerToWindowCenter()
        {
            try
            {
                var windowPos = this.PointToScreen(new System.Windows.Point(0, 0));
                double x = windowPos.X + (this.ActualWidth - centerMarker.ActualWidth) / 2.0;
                double y = windowPos.Y + (this.ActualHeight - centerMarker.ActualHeight) / 2.0;
                System.Windows.Controls.Canvas.SetLeft(centerMarker, x - windowPos.X + (canvas.ActualWidth - centerMarker.ActualWidth) / 2.0);
                System.Windows.Controls.Canvas.SetTop(centerMarker, y - windowPos.Y + (canvas.ActualHeight - centerMarker.ActualHeight) / 2.0);

                // Save screen center
                centerScreen = new System.Windows.Point((int)(windowPos.X + this.ActualWidth / 2.0), (int)(windowPos.Y + this.ActualHeight / 2.0));
            }
            catch { }
        }

        private void BtnStartMove_Click(object sender, RoutedEventArgs e)
        {
            running = true;
            btnStartMove.IsEnabled = false;
            btnStopMove.IsEnabled = true;
            AddLog("Move started");
            // compute current window-centered center point
            var wp = this.PointToScreen(new System.Windows.Point(this.ActualWidth / 2.0, this.ActualHeight / 2.0));
            centerScreen = new System.Windows.Point((int)wp.X, (int)wp.Y);
            angle = 0;
            lastClick = DateTime.MinValue;
            moveTimer.Start();
            // topmost option
            try { if (TopmostWhenRunning) this.Topmost = true; } catch { }
        }

        private void BtnStopMove_Click(object sender, RoutedEventArgs e)
        {
            running = false;
            btnStartMove.IsEnabled = true;
            btnStopMove.IsEnabled = false;
            moveTimer.Stop();
            AddLog("Move stopped");
            try
            {
                if (TopmostWhenRunning) this.Topmost = false;
                if (MinimiseWhenNotRunning)
                {
                    this.WindowState = WindowState.Minimized;
                }
            }
            catch { }
        }

        private void BtnCenterHere_Click(object sender, RoutedEventArgs e)
        {
            // set center to current mouse position
            try
            {
                System.Drawing.Point p = System.Windows.Forms.Cursor.Position;
                centerScreen = new System.Windows.Point(p.X, p.Y);
                AddLog($"Center set to cursor: {p.X},{p.Y}");
            }
            catch { }
        }

        private void MoveMouseWindow_PreviewKeyDown(object? sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtHotkey.Text))
                {
                    var desired = (System.Windows.Input.Key)Enum.Parse(typeof(System.Windows.Input.Key), txtHotkey.Text, true);
                    if (e.Key == desired)
                    {
                        // toggle running state
                        if (running) BtnStopMove_Click(this, new RoutedEventArgs()); else BtnStartMove_Click(this, new RoutedEventArgs());
                        e.Handled = true;
                    }
                }
            }
            catch { }
        }

        private void MoveTimer_Tick(object? sender, EventArgs e)
        {
            if (!running) return;

            double radius = sldRadius.Value;
            double speed = sldSpeed.Value; // 1..100

            // speed controls angle increment (radians per tick)
            double increment = Math.Max(0.001, speed / 200.0);

            double x = centerScreen.X;
            double y = centerScreen.Y;

            if (rbCircle.IsChecked == true)
            {
                angle += increment;
                x = centerScreen.X + radius * Math.Cos(angle);
                y = centerScreen.Y + radius * Math.Sin(angle);
            }
            else
            {
                // random mode -> if interpolation enabled, move toward currentTarget, else pick new immediate point
                if (cbInterpolation.IsChecked == true)
                {
                    if (!hasTarget)
                    {
                        double a = rnd.NextDouble() * Math.PI * 2.0;
                        double r = rnd.NextDouble() * radius;
                        currentTarget = new System.Windows.Point(centerScreen.X + r * Math.Cos(a), centerScreen.Y + r * Math.Sin(a));
                        hasTarget = true;
                    }

                    x = currentTarget.X;
                    y = currentTarget.Y;
                }
                else
                {
                    // immediate random jump
                    double a = rnd.NextDouble() * Math.PI * 2.0;
                    double r = rnd.NextDouble() * radius;
                    x = centerScreen.X + r * Math.Cos(a);
                    y = centerScreen.Y + r * Math.Sin(a);
                }
            }

            // follow options: update centerScreen if follow is enabled
            try
            {
                if (cbFollowMouse.IsChecked == true)
                {
                    var cur = System.Windows.Forms.Cursor.Position;
                    centerScreen = new System.Windows.Point(cur.X, cur.Y);
                    // if interpolation: reset target relative to new center
                    if (cbInterpolation.IsChecked == true) hasTarget = false;
                }
                else if (cbFollowWindow.IsChecked == true && !string.IsNullOrWhiteSpace(txtFollowWindowTitle.Text))
                {
                    IntPtr h = FindWindowByCaption(IntPtr.Zero, txtFollowWindowTitle.Text);
                    if (h != IntPtr.Zero)
                    {
                        if (GetWindowRect(h, out RECT r))
                        {
                            centerScreen = new System.Windows.Point((r.Left + r.Right) / 2, (r.Top + r.Bottom) / 2);
                            if (cbInterpolation.IsChecked == true) hasTarget = false;
                        }
                    }
                }
            }
            catch { }

            if (cbInterpolation.IsChecked == true || cbSmooth.IsChecked == true)
            {
                // smooth / interpolated movement: compute current cursor and move fractionally
                var curPos = System.Windows.Forms.Cursor.Position;
                double curX = curPos.X;
                double curY = curPos.Y;

                double targetX = x;
                double targetY = y;

                // if interpolation mode, targetX/Y already set; else they are immediate target
                double smoothing = cbSmooth.IsChecked == true ? Math.Max(0.02, 1.0 - (sldSpeed.Value / 200.0)) : 1.0;
                // move fraction towards target
                double newX = curX + (targetX - curX) * (1.0 - smoothing);
                double newY = curY + (targetY - curY) * (1.0 - smoothing);

                SetCursorPos((int)Math.Round(newX), (int)Math.Round(newY));

                // if interpolation, check if reached the target
                if (cbInterpolation.IsChecked == true && hasTarget)
                {
                    double dist = Math.Sqrt((newX - currentTarget.X) * (newX - currentTarget.X) + (newY - currentTarget.Y) * (newY - currentTarget.Y));
                    if (dist <= TARGET_REACHED_PX)
                    {
                        hasTarget = false; // pick a new target on next tick
                    }
                }
            }
            else
            {
                SetCursorPos((int)Math.Round(x), (int)Math.Round(y));
            }

            // optionally click
            if (cbClickOnMove.IsChecked == true)
            {
                if (int.TryParse(txtClickInterval.Text, out int iv) && iv > 0)
                {
                    if ((DateTime.Now - lastClick).TotalMilliseconds >= iv)
                    {
                        // choose click type
                        int downMs = 30;
                        int.TryParse(txtClickDownMs.Text, out downMs);
                        if (rbClickLeft.IsChecked == true)
                            DoClickLeft(downMs);
                        else if (rbClickRight.IsChecked == true)
                            DoClickRight(downMs);
                        else if (rbClickDouble.IsChecked == true)
                            DoDoubleClick(downMs);
                        lastClick = DateTime.Now;
                        AddLog("Click");
                    }
                }
            }
        }

        private void DoClickLeft(int downMs = 30)
        {
            try
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
                System.Threading.Thread.Sleep(Math.Max(0, downMs));
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
            }
            catch { }
        }

        private void DoClickRight(int downMs = 30)
        {
            const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
            const uint MOUSEEVENTF_RIGHTUP = 0x0010;
            try
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
                System.Threading.Thread.Sleep(Math.Max(0, downMs));
                mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
            }
            catch { }
        }

        private void DoDoubleClick(int downMs = 30)
        {
            DoClickLeft(downMs);
            System.Threading.Thread.Sleep(40);
            DoClickLeft(downMs);
        }

        // follow-window helpers
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string? className, string? windowTitle);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int maxLength);

        [DllImport("user32.dll")] 
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

        private static IntPtr FindWindowByCaption(IntPtr zeroOnly, string lpWindowName)
        {
            // try exact match first
            var h = FindWindow(null, lpWindowName);
            if (h != IntPtr.Zero) return h;
            // fallback: enumerate windows and find contains
            IntPtr found = IntPtr.Zero;
            EnumWindows((hwnd, lparam) =>
            {
                var sb = new System.Text.StringBuilder(512);
                GetWindowText(hwnd, sb, sb.Capacity);
                if (sb.ToString().IndexOf(lpWindowName, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    found = hwnd; return false; // stop
                }
                return true;
            }, IntPtr.Zero);
            return found;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct RECT { public int Left; public int Top; public int Right; public int Bottom; }

        private void AddLog(string message)
        {
            try
            {
                lstLog.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {message}");
                if (lstLog.Items.Count > 200) lstLog.Items.RemoveAt(lstLog.Items.Count - 1);
            }
            catch { }
        }

        private void ApplyRuntimeOptions()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(OverrideTitle)) this.Title = OverrideTitle;
                this.ShowInTaskbar = !HideFromTaskbar;
                if (HideWindowOnOpen)
                {
                    this.Visibility = Visibility.Collapsed;
                }
                if (HideFromAltTab)
                {
                    // set WS_EX_TOOLWINDOW to hide from Alt-Tab
                    var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
                    if (hwnd != IntPtr.Zero)
                    {
                        const int GWL_EXSTYLE = -20;
                        const uint WS_EX_TOOLWINDOW = 0x00000080;
                        var old = GetWindowLongPtr(hwnd, GWL_EXSTYLE);
                        SetWindowLongPtr(hwnd, GWL_EXSTYLE, new IntPtr(old.ToInt64() | (long)WS_EX_TOOLWINDOW));
                    }
                }
            }
            catch { }
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
    }
}
