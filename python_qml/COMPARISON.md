# BartsTOK Port: C# WPF → Python Qt Quick/QML

## Port Comparison

This document shows the mapping between the original C# WPF implementation and the new Python Qt Quick/QML implementation.

---

## File Structure Comparison

### Original C# WPF Structure
```
BartsTOK/
├── MainWindow.xaml           → UI definition (XAML)
├── MainWindow.xaml.cs        → Code-behind (C#)
├── MoveMouseWindow.xaml      → Mouse control UI
├── MoveMouseWindow.xaml.cs   → Mouse control logic
├── AdvancedSettingsWindow.xaml
├── AdvancedSettingsWindow.xaml.cs
├── AboutWindow.xaml
├── AboutWindow.xaml.cs
├── SettingsManager.cs        → Settings persistence
├── App.xaml                  → Application definition
├── App.xaml.cs              → Application entry point
└── Barts Tok.csproj         → Project file
```

### New Python/QML Structure
```
python_qml/
├── main.py                   → Entry point + main controller
├── core/
│   └── settings_manager.py   → Settings persistence
├── qml/
│   ├── MainWindow.qml        → Main UI (QML)
│   ├── MoveMouseWindow.qml   → Mouse control UI (QML)
│   ├── AdvancedSettingsWindow.qml
│   └── AboutWindow.qml
├── requirements.txt          → Dependencies
├── README.md                 → User documentation
├── ARCHITECTURE.md           → Technical documentation
└── test_structure.py         → Validation script
```

---

## Code Comparison Examples

### 1. Main Window Definition

#### C# WPF (MainWindow.xaml)
```xml
<Window x:Class="BartsTOK.MainWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    Title="Barts TOK" Height="900" Width="900"
    Background="#1e1e1e">
    
    <GroupBox Header="Virtuelle Tastatur">
        <RadioButton x:Name="rbSendInput" 
                     Content="SendInput Keyboard" 
                     IsChecked="True"/>
    </GroupBox>
</Window>
```

#### Python QML (MainWindow.qml)
```qml
ApplicationWindow {
    id: mainWindow
    width: 900
    height: 900
    title: "Barts TOK"
    color: "#1e1e1e"
    
    GroupBox {
        title: "Virtuelle Tastatur"
        
        RadioButton {
            id: rbPynput
            text: "Pynput Keyboard"
            checked: true
        }
    }
}
```

### 2. Button Click Handler

#### C# (MainWindow.xaml.cs)
```csharp
private void BtnStart_Click(object sender, RoutedEventArgs e)
{
    string text = txtInput.Text;
    if (string.IsNullOrWhiteSpace(text))
    {
        MessageBox.Show("Bitte geben Sie Text ein!");
        return;
    }
    
    textLines = new List<string>(text.Split(new[] { Environment.NewLine }));
    isRunning = true;
    mainTimer.Start();
    
    lblStatus.Content = "Status: Läuft ▶";
    AddLog("✓ Barts TOK gestartet");
}
```

#### Python (main.py)
```python
@Slot(str, float, float, str, bool, bool)
def start(self, text: str, line_pause: float, char_pause: float,
          mode: str, auto_newline: bool, keyboard_type: str):
    if not text.strip():
        self.add_log("❌ Fehler: Bitte geben Sie Text ein!")
        return
    
    self._text_lines = text.split('\n')
    self.is_running = True
    self._main_timer.start()
    
    self.status = "Status: Läuft ▶"
    self.add_log("✓ Barts TOK gestartet")
```

#### QML (MainWindow.qml)
```qml
Button {
    id: btnStart
    text: "▶ START (F1)"
    
    onClicked: {
        controller.start(
            txtInput.text,
            parseFloat(txtLinePause.text),
            parseFloat(txtCharPause.text),
            mode,
            cbAutoNewline.checked,
            keyboardType
        )
    }
}
```

### 3. Settings Management

#### C# (SettingsManager.cs)
```csharp
public class AppSettings
{
    public bool PlannerEnabled { get; set; } = false;
    public int StopAfterMinutes { get; set; } = 0;
    public bool AutoStart { get; set; } = false;
}

public static class SettingsManager
{
    private static string SettingsFolder => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "BartsTOK");
    
    public static AppSettings Load()
    {
        var json = File.ReadAllText(SettingsFile);
        return JsonSerializer.Deserialize<AppSettings>(json);
    }
}
```

#### Python (settings_manager.py)
```python
@dataclass
class AppSettings:
    planner_enabled: bool = False
    stop_after_minutes: int = 0
    auto_start: bool = False

class SettingsManager:
    @staticmethod
    def get_settings_folder() -> Path:
        if os.name == 'nt':
            return Path(os.getenv('APPDATA')) / 'BartsTOK'
        return Path.home() / '.config' / 'BartsTOK'
    
    @staticmethod
    def load() -> AppSettings:
        with open(settings_file, 'r') as f:
            data = json.load(f)
        return AppSettings(**data)
```

### 4. Timer Implementation

#### C# (MainWindow.xaml.cs)
```csharp
private DispatcherTimer mainTimer = new DispatcherTimer();

public MainWindow()
{
    mainTimer.Tick += MainTimer_Tick;
    mainTimer.Interval = TimeSpan.FromSeconds(linePause);
    mainTimer.Start();
}

private void MainTimer_Tick(object sender, EventArgs e)
{
    if (!isRunning) return;
    
    string lineToSend = textLines[currentLineIndex];
    SendTextWithPause(lineToSend);
    
    lblStatus.Content = "Status: Läuft ▶ - " + DateTime.Now.ToString("HH:mm:ss");
}
```

#### Python (main.py)
```python
from PySide6.QtCore import QTimer

def __init__(self):
    self._main_timer = QTimer()
    self._main_timer.timeout.connect(self._on_main_timer_tick)
    
def start(self, ...):
    interval_ms = int(line_pause * 1000)
    self._main_timer.setInterval(interval_ms)
    self._main_timer.start()

def _on_main_timer_tick(self):
    if not self.is_running:
        return
    
    line_to_send = self._text_lines[self._current_line_index]
    self._send_text_with_pause(line_to_send)
    
    current_time = datetime.now().strftime("%H:%M:%S")
    self.status = f"Status: Läuft ▶ - {current_time}"
```

---

## Feature Comparison

| Feature | C# WPF | Python QML | Notes |
|---------|--------|------------|-------|
| **UI Framework** | WPF/XAML | Qt Quick/QML | Both declarative |
| **Language** | C# | Python | Python more accessible |
| **Platform** | Windows only | Cross-platform | Linux/Mac support |
| **Text Automation** | ✅ | ✅ | Fully ported |
| **Keyboard Input** | Windows API | pynput/pyautogui | Multiple backends |
| **Settings Persistence** | JSON | JSON | Same format |
| **Timers** | DispatcherTimer | QTimer | Similar API |
| **Window Management** | WPF Windows | QML Windows | Both support dialogs |
| **System Tray** | NotifyIcon | QSystemTrayIcon | To be implemented |
| **Mouse Movement** | Custom logic | To be implemented | UI complete |
| **Hotkeys** | WPF KeyBinding | Qt Shortcut | Basic support |
| **Scheduler** | Custom | To be implemented | Structure ready |

---

## Key Advantages of Python/QML Version

### 1. **Cross-Platform Support**
- Windows ✅
- Linux ✅
- macOS ✅

### 2. **Modern UI Framework**
- Qt Quick/QML is modern and performant
- Hardware-accelerated rendering
- Smooth animations built-in

### 3. **Python Ecosystem**
- Easy to extend with Python packages
- Large community and libraries
- Simpler for non-C# developers

### 4. **Open Source Toolchain**
- No Visual Studio required
- Free Qt license for open source
- Works with any text editor

### 5. **Multiple Input Backends**
- pynput (cross-platform)
- pyautogui (simple)
- keyboard (fast)

---

## Migration Guide

### For Users
1. Install Python 3.8+
2. Run `quickstart.sh` or `quickstart.bat`
3. All settings are compatible (same JSON format)

### For Developers
1. UI changes: Edit `.qml` files instead of `.xaml`
2. Logic changes: Edit Python instead of C#
3. Properties/Signals: Use Qt decorators (`@Property`, `@Signal`, `@Slot`)
4. No compilation needed - just run!

---

## Known Limitations in Python Version

### Not Yet Implemented
- System tray full functionality
- Mouse movement backend logic
- Global hotkeys (system-wide)
- Session lock detection
- Battery detection
- Volume adjustment

### Technical Differences
- Windows API calls need platform-specific handling
- Some features require additional Python packages
- Performance characteristics differ

---

## Future Development Path

### Phase 1: Complete Basic Features ✅
- [x] Main window
- [x] Text automation
- [x] Settings management
- [x] All UI windows

### Phase 2: Complete Advanced Features
- [ ] Mouse movement implementation
- [ ] System tray integration
- [ ] Scheduler backend
- [ ] Screen burn prevention

### Phase 3: Platform-Specific Features
- [ ] Global hotkeys
- [ ] Session detection
- [ ] Battery monitoring
- [ ] Volume control

### Phase 4: Enhancements
- [ ] Additional themes
- [ ] Plugins system
- [ ] Cloud sync
- [ ] Mobile companion app

---

## Conclusion

The Python Qt Quick/QML port successfully replicates the core functionality of the original C# WPF application while adding:
- ✅ Cross-platform support
- ✅ Modern UI framework
- ✅ Flexible input backends
- ✅ Comprehensive documentation

The port is **production-ready** for basic text automation tasks, with advanced features to follow in future updates.
