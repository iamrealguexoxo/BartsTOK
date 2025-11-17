# BartsTOK Python/QML Architecture Documentation

## Overview

This document describes the architecture of the Python/QML port of BartsTOK, originally a C# WPF application.

## Technology Stack

- **Python 3.8+**: Core programming language
- **PySide6**: Qt 6 Python bindings
- **Qt Quick/QML**: Declarative UI framework
- **PyAutoGUI**: Cross-platform GUI automation (optional)
- **keyboard**: Keyboard control library (optional)
- **pynput**: Input control library (optional)

## Project Structure

```
python_qml/
├── main.py                          # Application entry point
├── requirements.txt                 # Python dependencies
├── README.md                        # User documentation
├── ARCHITECTURE.md                  # This file
├── test_structure.py                # Structure validation script
│
├── core/                            # Core business logic
│   ├── __init__.py
│   └── settings_manager.py          # Settings persistence
│
├── qml/                             # QML UI files
│   ├── MainWindow.qml               # Main application window
│   ├── MoveMouseWindow.qml          # Mouse movement control
│   ├── AdvancedSettingsWindow.qml   # Advanced settings dialog
│   └── AboutWindow.qml              # About dialog
│
└── resources/                       # Assets (icons, images)
    └── (future: icons, animations)
```

## Architecture Pattern

### Model-View-Controller (MVC) with Qt/QML

The application follows a modified MVC pattern:

```
┌─────────────┐         ┌──────────────┐         ┌─────────────┐
│    View     │◄───────►│  Controller  │◄───────►│    Model    │
│    (QML)    │ Signals │   (Python)   │  Data   │  (Settings) │
└─────────────┘         └──────────────┘         └─────────────┘
```

1. **View (QML)**: UI definition and user interaction
2. **Controller (Python)**: Business logic and state management
3. **Model (Python)**: Data structures and persistence

## Component Details

### 1. Main Controller (`main.py`)

**Class**: `MainWindowController`

**Responsibilities**:
- Manages application state
- Handles text automation logic
- Controls timers and scheduling
- Simulates keyboard input
- Manages child windows

**Key Methods**:
- `start()`: Start text automation
- `stop()`: Stop automation
- `add_log()`: Add log entries
- `open_*_window()`: Open child windows

**Qt Signals**:
- `statusChanged`: Status text updates
- `logAdded`: New log entries
- `runningStateChanged`: Running state changes

### 2. Settings Manager (`core/settings_manager.py`)

**Classes**: 
- `AppSettings`: Application settings data class
- `ScheduleEntry`: Scheduled action data class
- `SettingsManager`: Static persistence manager

**Storage Location**:
- Windows: `%APPDATA%/BartsTOK/settings.json`
- Linux/Mac: `~/.config/BartsTOK/settings.json`

**Settings Categories**:
- Planner settings (auto-stop, scheduling)
- UI behavior (window flags, minimization)
- Automation behavior (repeat, user activity detection)
- System integration (tray, volume, battery)

### 3. QML UI Components

#### MainWindow.qml
- Main application interface
- Text input and configuration
- Start/Stop controls
- Status display
- Log viewer

**Key UI Elements**:
- Virtual keyboard selection (Pynput/PyAutoGUI/Keyboard)
- Behavior mode (Sequential/Random/Sequential+Stop)
- Typing speed configuration
- Log expander

#### MoveMouseWindow.qml
- Mouse movement control
- Visual movement preview
- Movement configuration (radius, speed, mode)
- Click configuration

**Features**:
- Circle/Random movement modes
- Smooth interpolation
- Follow modes (mouse/window)
- Presets management

#### AdvancedSettingsWindow.qml
- All advanced configuration options
- Planner settings
- UI behavior flags
- Behavior options
- Scheduler access

#### AboutWindow.qml
- Application information
- Version display
- Credits and license

## Data Flow

### Text Automation Flow

```
User Input (QML)
    ↓
start() Method
    ↓
Parse Text → Lines Array
    ↓
Start Timer
    ↓
Timer Tick → Select Line (mode-dependent)
    ↓
Send Text (char-by-char with pauses)
    ↓
Keyboard Simulation (pynput/pyautogui/keyboard)
    ↓
Update Status & Log
```

### Settings Flow

```
Application Start
    ↓
Load Settings (JSON)
    ↓
Apply to Controller
    ↓
User Modifies Settings
    ↓
Save Settings (JSON)
    ↓
Apply Changes
```

## Qt/QML Communication

### Python → QML

**Signals**: Used to notify QML of state changes
```python
statusChanged = Signal(str)
logAdded = Signal(str)
```

**Properties**: Used to expose values to QML
```python
@Property(str, notify=statusChanged)
def status(self) -> str:
    return self._status
```

### QML → Python

**Slots**: Methods callable from QML
```python
@Slot(str, float, float, str, bool, bool)
def start(self, text, line_pause, char_pause, mode, auto_newline, keyboard_type):
    # Implementation
```

**Usage in QML**:
```qml
Button {
    onClicked: controller.start(txtInput.text, ...)
}
```

### Context Properties

The controller is exposed to QML via context properties:
```python
engine.rootContext().setContextProperty("controller", controller)
```

Accessible in QML as:
```qml
Text {
    text: controller.status
}
```

## Keyboard Simulation

Three methods are supported, selectable by user:

### 1. Pynput (Default)
- **Pros**: Cross-platform, reliable
- **Cons**: Slower than native methods
- **Use**: Most compatible option

### 2. PyAutoGUI
- **Pros**: Simple API, cross-platform
- **Cons**: Can be blocked by security software
- **Use**: Alternative method

### 3. Keyboard Library
- **Pros**: Fast, direct
- **Cons**: Windows-only, requires admin on Linux
- **Use**: Performance-critical scenarios

## Timer Architecture

### Main Timer
- **Purpose**: Controls text sending intervals
- **Interval**: User-configurable (line pause)
- **Behavior**: Repeats until stopped or sequence complete

### Stop-After Timer
- **Purpose**: Auto-stop after configured duration
- **Interval**: 1 second
- **Behavior**: Counts down, stops automation when reaches 0

## Window Management

Child windows are created lazily and cached:

```python
def open_move_mouse_window(self):
    if self._move_mouse_window is None:
        # Load and create window
        component = QQmlComponent(self._engine, qml_file)
        self._move_mouse_window = component.create()
    else:
        # Show existing window
        self._move_mouse_window.setProperty("visible", True)
```

## Future Enhancements

### Planned Features

1. **System Tray Integration**
   - Add QSystemTrayIcon support
   - Minimize to tray functionality
   - Tray context menu

2. **Mouse Movement Implementation**
   - Complete MoveMouse controller
   - Implement circular motion
   - Implement random movement
   - Add click automation

3. **Scheduler**
   - Time-based action triggers
   - Day-of-week scheduling
   - Multiple schedule entries

4. **Screen Burn Prevention**
   - Periodic micro-movements
   - Configurable intervals

5. **Global Hotkeys**
   - System-wide F1/F2 support
   - Requires additional library (pynput global hotkeys)

6. **Cross-Platform Improvements**
   - Test on Linux
   - Test on macOS
   - Platform-specific adjustments

### Technical Debt

- Add comprehensive error handling
- Implement unit tests
- Add integration tests
- Improve input validation
- Add configuration validation
- Implement logging levels

## Migration from C# WPF

### Completed Ports

| Feature | C# WPF | Python/QML | Status |
|---------|--------|------------|--------|
| Main Window | MainWindow.xaml | MainWindow.qml | ✅ Complete |
| Text Automation | MainWindow.xaml.cs | main.py | ✅ Complete |
| Settings | SettingsManager.cs | settings_manager.py | ✅ Complete |
| About Dialog | AboutWindow.xaml | AboutWindow.qml | ✅ Complete |
| MoveMouseWindow UI | MoveMouseWindow.xaml | MoveMouseWindow.qml | ✅ Complete |
| Advanced Settings UI | AdvancedSettingsWindow.xaml | AdvancedSettingsWindow.qml | ✅ Complete |

### Pending Ports

| Feature | Status |
|---------|--------|
| Mouse Movement Logic | ⏳ Pending |
| System Tray | ⏳ Pending |
| Scheduler | ⏳ Pending |
| Screen Burn Prevention | ⏳ Pending |
| Global Hotkeys | ⏳ Pending |
| Session Lock Detection | ⏳ Pending |
| Battery Detection | ⏳ Pending |

### Key Differences

1. **Language**: C# → Python
2. **UI Framework**: WPF/XAML → Qt Quick/QML
3. **Input Simulation**: Windows API → Cross-platform libraries
4. **Threading**: .NET Dispatcher → Qt QTimer
5. **Settings**: .NET JSON serializer → Python json module

## Development Setup

### Prerequisites
```bash
python -m venv venv
source venv/bin/activate  # or venv\Scripts\activate on Windows
pip install -r requirements.txt
```

### Running the Application
```bash
python main.py
```

### Testing Structure
```bash
python test_structure.py
```

### Adding New Features

1. **New UI Window**: Add .qml file in `qml/` directory
2. **New Controller**: Add method in MainWindowController
3. **New Settings**: Add field to AppSettings dataclass
4. **New Module**: Add .py file in `core/` directory

## Debugging

### Qt QML Debugging

Enable QML debugging:
```bash
QML_IMPORT_TRACE=1 python main.py
```

### Python Debugging

Add debug prints:
```python
print(f"Debug: {variable}")
```

Or use Python debugger:
```python
import pdb; pdb.set_trace()
```

## Performance Considerations

1. **Timer Intervals**: Minimum 10ms for responsiveness
2. **Character Pause**: Adjustable from 0.001s to several seconds
3. **QML Updates**: Use property bindings for automatic updates
4. **Memory**: Child windows are cached, not recreated

## Security Considerations

1. **Input Simulation**: Can be blocked by security software
2. **Global Hotkeys**: Require elevated privileges on some systems
3. **File Logging**: Logs may contain sensitive information
4. **Settings File**: Stored in plain text JSON

## License

MIT License - See LICENSE file for details

## Credits

- Original C# WPF version: BartsTOK
- Python/QML port: 2025
- Inspired by: movemouse and burnstok by sgrottl
