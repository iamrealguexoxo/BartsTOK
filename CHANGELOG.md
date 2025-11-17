# Changelog

All notable changes to this project will be documented in this file.

## [1.1 alpha] - 2025-11-17

Deutsch
- **NEU: Python Qt Quick/QML Version**
  - Vollständige Portierung der Benutzeroberfläche nach Python Qt Quick/QML
  - Cross-Platform Support: Windows, Linux, macOS
  - 4 komplett portierte Fenster: MainWindow, MoveMouseWindow, AdvancedSettingsWindow, AboutWindow
  - Text-Automatisierung mit 3 Tastatur-Backends (pynput, pyautogui, keyboard)
  - Einstellungs-Persistenz (JSON, kompatibel mit C# Version)
  - Umfassende Dokumentation (7 Dokumente, 1.500+ Zeilen)
  - Schnellstart-Scripts für einfache Installation
  - Verzeichnis: `python_qml/`
- Status Python-Version:
  - ✅ Kern-Features: Text-Automatisierung, Einstellungen, Timer, Logging
  - ⏳ In Entwicklung: Maus-Bewegung Backend, System-Tray, Scheduler Backend

English
- **NEW: Python Qt Quick/QML Version**
  - Complete port of user interface to Python Qt Quick/QML
  - Cross-platform support: Windows, Linux, macOS
  - 4 fully ported windows: MainWindow, MoveMouseWindow, AdvancedSettingsWindow, AboutWindow
  - Text automation with 3 keyboard backends (pynput, pyautogui, keyboard)
  - Settings persistence (JSON, compatible with C# version)
  - Comprehensive documentation (7 documents, 1,500+ lines)
  - Quick-start scripts for easy installation
  - Directory: `python_qml/`
- Python version status:
  - ✅ Core features: Text automation, settings, timers, logging
  - ⏳ In development: Mouse movement backend, system tray, scheduler backend

## [1.0 alpha] - 2025-11-02

Deutsch
- Erste öffentliche Alpha-Pre-Release-Version
- Hauptfunktionen:
  - Move Mouse: Kreis/Random, Smooth/Interpolation, Klick-Optionen (Links/Rechts/Doppelklick), Follow Mouse/Window, Presets (AppData)
  - Planer/Scheduler: mehrere Einträge (Zeit + Tage), Aktionen Start/Stop
  - Erweiterte Einstellungen (experimental): AutoStart, Tray, Screen-Burn-Prevention, Lautstärke-Anpassung, Pause bei Batterie, Continue on Lock, Wiederholungen/Intervalle, File-Logging
  - System Tray: Start/Stop/Einstellungen/Über/Beenden, Minimize-to-tray
  - About-Dialog: v1 Pre-release, made with ❤️ by iamguexoxo, instagram.com/iamgue
  - GIF-Animationen (loading.gif)
- Persistenz
  - Einstellungen: %APPDATA%\\BartsTOK\\settings.json
  - Presets: %APPDATA%\\MoveBart_presets.json
- Bekannte Einschränkungen
  - Scheduler prüft im ~30s-Intervall
  - Pre-Release, Verhalten kann sich ändern

English
- First public alpha pre-release
- Main features:
  - Move Mouse: circle/random, smooth/interpolation, click options (left/right/double), follow mouse/window, presets (AppData)
  - Scheduler: multiple entries (time + days), actions start/stop
  - Advanced Settings (experimental): autostart, tray, screen-burn prevention, volume adjust, pause on battery, continue on lock, repeat intervals, file logging
  - System tray: start/stop/settings/about/exit, minimize-to-tray
  - About dialog: v1 Pre-release, made with ❤️ by iamguexoxo, instagram.com/iamgue
  - GIF animations (loading.gif)
- Persistence
  - Settings: %APPDATA%\\BartsTOK\\settings.json
  - Presets: %APPDATA%\\MoveBart_presets.json
- Known limitations
  - Scheduler checks roughly every 30s
  - Pre-release; behavior may change

---

Inspiriert von „movemouse“ und „burnstok“ von sgrottl.