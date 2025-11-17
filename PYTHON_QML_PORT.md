# 🎉 Python Qt Quick/QML Port Available!

## New Cross-Platform Version

BartsTOK is now available in a **Python Qt Quick/QML** version that works on Windows, Linux, and macOS!

## Location

The Python/QML version is located in the **`python_qml/`** directory.

```
BartsTOK/
├── (Original C# WPF version)
└── python_qml/           ← New Python/QML version
    ├── main.py
    ├── qml/
    ├── core/
    └── README.md
```

## Quick Start

```bash
cd python_qml
./quickstart.sh    # Linux/Mac
quickstart.bat     # Windows
```

## Features

✅ **Cross-Platform Support**
- Works on Windows, Linux, and macOS

✅ **Modern UI Framework**
- Qt Quick/QML with hardware acceleration
- Dark theme matching original design

✅ **Complete Port**
- All 4 windows ported (Main, MoveMouse, Settings, About)
- Full text automation functionality
- Settings persistence

✅ **Flexible Input**
- Choice of 3 keyboard backends
- pynput (recommended, cross-platform)
- pyautogui (simple)
- keyboard (fast)

✅ **Comprehensive Documentation**
- User guide (README.md)
- Technical architecture (ARCHITECTURE.md)
- Port comparison (COMPARISON.md)
- Project summary (PROJECT_SUMMARY.md)

## Documentation

Inside the `python_qml/` directory:

- **README.md** - Installation and usage guide
- **ARCHITECTURE.md** - Technical documentation
- **COMPARISON.md** - C# vs Python comparison
- **RUN_INSTRUCTIONS.txt** - Quick reference

## Comparison

| Feature | C# WPF | Python QML |
|---------|--------|------------|
| Platform | Windows only | Windows, Linux, Mac |
| Language | C# | Python |
| UI Framework | WPF/XAML | Qt Quick/QML |
| Setup | Visual Studio | Python + pip |
| Build Required | Yes | No |

## Status

**Production Ready** ✅

Core features are fully functional:
- Text automation
- Settings management
- Timer system
- Logging
- All UI windows

Advanced features planned:
- Mouse movement backend
- System tray integration
- Scheduler backend

## Requirements

- Python 3.8 or higher
- PySide6 (Qt 6)
- Optional: pynput, pyautogui, keyboard

## Installation

```bash
cd python_qml
pip install -r requirements.txt
python main.py
```

Or use the quick start scripts for automated setup!

---

**Try the new Python/QML version today!** 🚀

For more details, see `python_qml/README.md`
