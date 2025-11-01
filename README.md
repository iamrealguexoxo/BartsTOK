# BartsTOK — Feature-Übersicht / Feature Overview

Dieses Repository enthält die Desktop-Anwendung "BartsTOK" (WPF, .NET 9) zur Verhinderung von Inaktivität und zur Automatisierung einfacher Aktionen. Die folgende README listet alle aktuell implementierten Features sowohl auf Deutsch als auch auf Englisch.

## Deutsch — Implementierte Features

- Allgemeines
  - WPF-Desktopanwendung (.NET 9, Ziel: net9.0-windows).
  - Persistente Einstellungen via JSON in %APPDATA% (siehe `SettingsManager`).

- Hauptfunktionen (MainWindow)
  - Start / Stop der Aktionen (UI + Hotkeys: F1=Start, F2=Stop).
  - Tray-Icon (System Tray) mit Kontextmenü: Einstellungen, Start, Stop, Hilfe, Beenden.
  - Minimieren in den Tray (optionale Einstellung).
  - GIF-Animationen für Header/Loading (manuelles Frame-Handling, Fallback wenn WPF nicht automatisch animiert).
  - Systemereignisse: Reaktion auf Session-Lock/Unlock und Power-Status (mit Optionen Pause bei Batterie).
  - Bildschirm-Kontakt/Screen-burn-Prevention (Timer-basiert, optional).
  - Planer / Scheduler: mehrere geplante Einträge (Start/Stop zu bestimmten Zeiten und Tagen).
  - Optionen: AutoStart, PlannerEnabled, HideTrayIcon, TrayNotifications, ShowStatusOnMain u.v.m.

- Move Mouse Window (MoveMouseWindow)
  - Bewegt den Mauszeiger im Kreis oder zufällig in einem Radius um ein definiertes Zentrum.
  - Modi: Circle (kreisförmig), Random.
  - Interpolation und Smooth-Bewegung für natürlichere Mausbewegungen.
  - Optionen zum Klicken während der Bewegung: Linksklick, Rechtsklick, Doppelklick, einstellbare Intervalle und Klickdauer.
  - Hotkey-Unterstützung (konfigurierbar im Fenster), Start/Stop-Buttons innerhalb des Fensters.
  - Center setzen (aktuelle Cursor-Position als Zentrum speichern).
  - Follow-Funktionen: FollowMouse (Zentrum folgt der Maus) und FollowWindow (Zentrum folgt einem Fenster mit Titel).
  - Presets: Presets speichern und laden (JSON-Datei unter AppData: `MoveBart_presets.json`).
  - Laufzeit-Optionen: TopmostWhenRunning, MinimiseWhenNotRunning, HideFromTaskbar, HideFromAltTab, OverrideTitle.

- Presets
  - Presets enthalten Name, Radius, Speed, Mode, Click-Einstellungen, Smooth/Interpolation, Follow-Optionen.
  - Presets werden in einer JSON-Datei im Roaming-AppData-Ordner gespeichert und geladen.

- Scheduler / ScheduleEditorWindow
  - Schedule-Editor Fenster: Einträge mit Name, Zeit (HH:mm), Aktion (Start/Stop), Tage (Everyday oder Wochentage), Enabled.
  - Scheduler prüft periodisch (Standard: alle 30 Sekunden) und führt Aktionen aus, wenn Zeit & Tag übereinstimmen.

- Advanced Settings (AdvancedSettingsWindow)
  - Umfangreiche Konfigurationsoptionen: PlannerEnable, StopAfterMinutes, AutoStart, HideMoveMouseWindow, TopmostWhenRunning, MinimiseWhenNotRunning, HideFromTaskbar, HideFromAltTab, OverrideTitle.
  - Behaviour-Optionen: RepeatEnabled / RepeatInterval, AutoStopOnUserActivity, LaunchMoveMouseAtStartup, StartActionsWhenMoveMouseLaunched, AdjustVolumeWhenMoveMouseRunning, AdjustVolumePercent.
  - Fortgeschritten: ContinueWhenSessionLocked, PauseWhenOnBattery, EnableFileLogging, TrayNotifications, HideTrayIcon.
  - Schedules-Verwaltung (Liste der `ScheduleEntry` Einträge).

- Einstellungen / Persistenz (SettingsManager)
  - Einstellungen werden als `AppSettings` serialisiert (JSON) im Ordner `%APPDATA%\BartsTOK\settings.json`.
  - `SettingsManager.Load()` und `SettingsManager.Save()` kümmern sich um Laden/Speichern mit einfachen Fallbacks.

- Sonstige Hilfsfunktionen
  - Volume-Adjust (via winmm) wenn aktiviert.
  - Dateibasierte Logs (optional über EnableFileLogging).
  - Tray-Benachrichtigungen (optional).

## English — Implemented Features

- General
  - WPF desktop application (.NET 9, target: net9.0-windows).
  - Persistent settings stored as JSON in %APPDATA% (see `SettingsManager`).

- Main features (MainWindow)
  - Start / Stop actions (UI + hotkeys: F1 = Start, F2 = Stop).
  - System tray icon with context menu: Settings, Start, Stop, Help, Exit.
  - Option to minimize to tray.
  - GIF animations for header/loading (manual frame handling with fallback).
  - Handles system events: session lock/unlock and power mode changes (with options to pause on battery).
  - Screen burn prevention (timer-based, optional).
  - Planner / Scheduler: multiple schedule entries (Start/Stop at configured times/days).
  - Options: AutoStart, PlannerEnabled, HideTrayIcon, TrayNotifications, ShowStatusOnMain, etc.

- Move Mouse Window (MoveMouseWindow)
  - Moves the mouse cursor in a circle or randomly within a radius around a defined center.
  - Modes: Circle and Random.
  - Interpolation and smoothing for natural-looking movement.
  - Optional clicks while moving: left, right, double click — configurable interval and down-time.
  - Hotkey support (configurable in the window) plus Start/Stop buttons.
  - Set center to current cursor position.
  - Follow options: FollowMouse and FollowWindow (center follows a window by title).
  - Presets: Save and load presets (JSON in AppData: `MoveBart_presets.json`).
  - Runtime options: TopmostWhenRunning, MinimiseWhenNotRunning, HideFromTaskbar, HideFromAltTab, OverrideTitle.

- Presets
  - Presets contain name, radius, speed, mode, click settings, smooth/interpolation, follow options.
  - Persisted as JSON in the roaming AppData folder and reloaded on startup.

- Scheduler / ScheduleEditorWindow
  - Schedule editor window: entries with Name, Time (HH:mm), Action (Start/Stop), Days (Everyday or specific weekdays), Enabled flag.
  - Scheduler checks periodically (every ~30s) and triggers actions when time & day match.

- Advanced Settings (AdvancedSettingsWindow)
  - Many configuration toggles: PlannerEnable, StopAfterMinutes, AutoStart, HideMoveMouseWindow, TopmostWhenRunning, MinimiseWhenNotRunning, HideFromTaskbar, HideFromAltTab, OverrideTitle.
  - Behavior options: RepeatEnabled / RepeatInterval, AutoStopOnUserActivity, LaunchMoveMouseAtStartup, StartActionsWhenMoveMouseLaunched, AdjustVolumeWhenMoveMouseRunning, AdjustVolumePercent.
  - Advanced: ContinueWhenSessionLocked, PauseWhenOnBattery, EnableFileLogging, TrayNotifications, HideTrayIcon.
  - Schedules management (list of `ScheduleEntry`).

- Settings / Persistence (SettingsManager)
  - App settings serialized as `AppSettings` (JSON) under `%APPDATA%\BartsTOK\settings.json`.
  - `SettingsManager.Load()` / `Save()` handle load/save with safe fallbacks.

- Other utilities
  - Volume adjustment using winmm when enabled.
  - Optional file logging (EnableFileLogging).
  - Optional tray notifications.

## Where files are stored

- Application settings: `%APPDATA%\BartsTOK\settings.json` (AppSettings JSON).
- MoveMouse presets: `%APPDATA%\MoveBart_presets.json`.

## Kurz-Anleitung / Quick Start

1. Starten Sie die Anwendung (Visual Studio oder `dotnet run` mit der Lösung `BartsTOK.sln`).
2. Im Hauptfenster: Start drücken oder F1 drücken, Stop mit Stop-Button oder F2.
3. Öffnen Sie "Move Mouse"-Fenster, konfigurieren Sie Radius/Speed/Mode, speichern Sie Presets wenn gewünscht.
4. Öffnen Sie "Erweiterte Einstellungen" um Planer, AutoStart, Tray-Optionen und Verhalten anzupassen.
5. Schedules: Im Advanced Settings können Sie Zeitpläne hinzufügen/ändern (ScheduleEditorWindow).

## Build / Run

- Benötigt: .NET 9 SDK (oder kompatiblen .NET SDK), Visual Studio (Windows) oder `dotnet` CLI.
- Projekt: `BartsTOK.sln` bzw. `Barts Tok.csproj` (Zielframework: net9.0-windows).
- Empfohlener Ablauf: Öffnen Sie die Lösung in Visual Studio und starten Sie die Anwendung für beste Integration mit WPF-Designer.

## Hinweise / Known limitations

- Einige Funktionen (z. B. Icon-Laden aus Ressourcen, GIF-Metadaten) haben Fallbacks und loggen Fehler still, um Abstürze zu vermeiden.
- Scheduler prüft im 30s-Intervall; sehr genaue Timeranforderungen sind nicht implementiert.
- Die App führt echte Eingaben (Cursor-Bewegung, Klicks) aus — verwenden Sie die Einstellungen vorsichtig, insbesondere Click-Intervalle.

---
Wenn Sie möchten, kann ich diese README noch erweitern (Screenshots, detaillierte UI-Dokumentation, Anleitung für Entwickler oder Unit-Tests). Sollen wir noch ein kurzes „How to develop / build“ mit VS-spezifischen Schritten hinzufügen?
