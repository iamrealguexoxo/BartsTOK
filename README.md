# 🎉 BartsTOK - Der ultimative Inaktivitäts-Verhinderer! 🎉

![BartsTOK Logo](loading.gif)

[![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/)
[![WPF](https://img.shields.io/badge/WPF-Desktop-green.svg)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
[![Python](https://img.shields.io/badge/Python-3.8+-blue.svg)](https://www.python.org/)
[![Qt](https://img.shields.io/badge/Qt-QML-green.svg)](https://www.qt.io/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## 📝 Kurze Beschreibung / Short Description

**Deutsch:** BartsTOK ist eine App, die deinen PC wach hält, die Maus bewegt und Bildschirm-Brand verhindert. Verfügbar als **WPF-App für Windows** oder als **Python Qt Quick/QML-Version** für Windows, Linux und macOS!

**English:** BartsTOK is an app that keeps your PC awake, moves the mouse, and prevents screen burn. Available as a **WPF app for Windows** or as a **Python Qt Quick/QML version** for Windows, Linux, and macOS!

Willkommen bei **BartsTOK**! 🚀 Diese Anwendung hält deinen Computer wach, bewegt die Maus und verhindert Bildschirm-Brand. Inspiriert von den genialen Projekten "movemouse" und "burnstok" von sgrottl. Vielen Dank an sgrottl für die Inspiration! 🙏

## 🆕 Neue Python/QML Version verfügbar!

**BartsTOK ist jetzt auch als Python Qt Quick/QML Version verfügbar!**

✨ **Neu in der Python-Version:**
- 🌍 **Cross-Platform**: Läuft auf Windows, Linux und macOS
- 🎨 **Modernes UI**: Qt Quick/QML mit Hardware-Beschleunigung
- 🐍 **Python-basiert**: Einfach zu erweitern und anzupassen
- 🚀 **Schneller Start**: Ein Befehl genügt (siehe [python_qml/](python_qml/))

**Schnellstart für die Python-Version:**
```bash
cd python_qml
./quickstart.sh    # Linux/Mac
quickstart.bat     # Windows
```

📚 **Dokumentation**: Siehe [PYTHON_QML_PORT.md](PYTHON_QML_PORT.md) und [python_qml/README.md](python_qml/README.md)

---

## 🎯 Zwei Versionen zur Auswahl:

| Feature | C# WPF Version | Python QML Version |
|---------|----------------|-------------------|
| **Plattform** | Windows | Windows, Linux, macOS |
| **UI Framework** | WPF/XAML | Qt Quick/QML |
| **Installation** | .NET 9 SDK | Python 3.8+ |
| **Status** | ✅ Vollständig | ✅ Kern-Features fertig |
| **Verzeichnis** | `/` (Root) | `/python_qml/` |

---

## 🔄 Neueste Änderungen / Latest changes

<!-- CHANGELOG:START -->
Noch keine eingebetteten Einträge. Siehe vollständige Historie in [CHANGELOG.md](CHANGELOG.md).
<!-- CHANGELOG:END -->

## ✨ Features - Was kann BartsTOK?

> **Hinweis**: Die folgenden Features sind vollständig in der **C# WPF Version** implementiert. Die **Python QML Version** hat alle UI-Elemente portiert, aber einige erweiterte Features sind noch in Entwicklung. Siehe [python_qml/README.md](python_qml/README.md) für Details.

### 🖱️ Maus-Bewegung (Move Mouse)
- **Kreisförmige oder zufällige Bewegung**: Lass die Maus im Kreis drehen oder zufällig herumwandern.
- **Smooth & Interpolation**: Natürliche, flüssige Bewegungen – kein roboterhaftes Zucken!
- **Klicks während der Bewegung**: Linksklick, Rechtsklick oder Doppelklick – konfigurierbare Intervalle.
- **Follow-Modi**: Folge der Maus oder einem bestimmten Fenster.
- **Presets**: Speichere und lade deine Lieblings-Einstellungen (Radius, Speed, etc.).
- **Hotkeys**: Starte/Stoppe mit einem Tastendruck.

### ⏰ Planer & Scheduler
- **Zeitgesteuerte Aktionen**: Starte oder Stoppe automatisch zu bestimmten Zeiten.
- **Tägliche oder wöchentliche Pläne**: Everyday oder spezifische Wochentage.
- **Mehrere Einträge**: Erstelle so viele Schedules wie du willst.

### ⚙️ Erweiterte Einstellungen
- **Auto-Start**: Starte automatisch beim Systemstart.
- **Tray-Icon**: Verstecke im System-Tray mit Kontextmenü.
- **Bildschirm-Schutz**: Verhindere Screen-Burn mit Timer-basierten Bewegungen.
- **System-Events**: Reagiere auf Lock/Unlock und Batterie-Status.
- **Volume-Anpassung**: Passe die Lautstärke an, wenn die App läuft.
- **Logging**: Optionale Datei-Logs für Debugging.

### 🎨 UI & UX
- **Schöne WPF-Oberfläche**: Moderne, benutzerfreundliche Fenster.
- **GIF-Animationen**: Tanzende Barts für Loading und Header! 💃🕺
- **Minimieren & Verstecken**: Optionen zum Verstecken in Tray, Taskbar, etc.
- **Hotkeys**: F1/F2 für schnellen Start/Stop.

## 📸 Screenshots & GIFs

Hier ein paar coole GIFs und Screenshots von BartsTOK in Aktion:

### Screenshot der App:
![BartsTOK Screenshot](screenshot.png)



## 🚀 Installation & Setup

### Option 1: C# WPF Version (Windows)

1. **Voraussetzungen**:
   - Windows 10/11
   - .NET 9 SDK (kostenlos von [Microsoft](https://dotnet.microsoft.com/download))

2. **Klonen & Bauen**:
   ```powershell
   git clone https://github.com/iamrealguexoxo/BartsTOK.git
   cd BartsTOK
   dotnet build
   ```

3. **Ausführen**:
   ```powershell
   dotnet run
   ```
   Oder öffne `BartsTOK.sln` in Visual Studio und drücke F5.

### Option 2: Python QML Version (Windows, Linux, macOS)

1. **Voraussetzungen**:
   - Python 3.8 oder höher
   - pip (Python Package Manager)

2. **Schnellstart**:
   ```bash
   cd python_qml
   ./quickstart.sh    # Linux/Mac
   quickstart.bat     # Windows
   ```
   
   Der Quickstart-Script erstellt automatisch eine virtuelle Umgebung, installiert alle Abhängigkeiten und startet die App!

3. **Manuelle Installation** (optional):
   ```bash
   cd python_qml
   python -m venv venv
   source venv/bin/activate  # oder venv\Scripts\activate auf Windows
   pip install -r requirements.txt
   python main.py
   ```

📚 **Mehr Infos zur Python-Version**: Siehe [python_qml/README.md](python_qml/README.md)

## 📖 Verwendung / Usage

### Schnellstart:
1. Starte die App.
2. Drücke **Start** (oder F1) im Hauptfenster.
3. Öffne **Move Mouse** für Maus-Bewegungen.
4. Gehe zu **Erweiterte Einstellungen** für mehr Optionen.

### Tipps:
- **Vorsichtig mit Klicks**: Die App klickt wirklich – teste in einer sicheren Umgebung!
- **Presets speichern**: Erstelle Presets für verschiedene Szenarien (z.B. "Office" vs. "Gaming").
- **Scheduler**: Plane Pausen oder Aktivitäten für den ganzen Tag.

## 🛠️ Entwicklung / Development

### C# WPF Version - Projekt-Struktur:
- `MainWindow.xaml/cs`: Hauptfenster & Core-Logik.
- `MoveMouseWindow.xaml/cs`: Maus-Bewegungs-Fenster.
- `AdvancedSettingsWindow.xaml/cs`: Einstellungen-Dialog.
- `SettingsManager.cs`: Persistenz für Einstellungen.

### Python QML Version - Projekt-Struktur:
- `python_qml/main.py`: Haupteinstiegspunkt & Controller.
- `python_qml/qml/*.qml`: UI-Definitionen (MainWindow, MoveMouseWindow, etc.).
- `python_qml/core/settings_manager.py`: Einstellungs-Persistenz.
- 📚 Siehe [python_qml/ARCHITECTURE.md](python_qml/ARCHITECTURE.md) für Details.

### Beitragen:
1. Fork das Repo.
2. Erstelle einen Branch: `git checkout -b feature/awesome-feature`
3. Committe deine Änderungen: `git commit -m 'Add awesome feature'`
4. Push und erstelle einen Pull Request.

### Bekannte Einschränkungen:
- Scheduler prüft alle 30 Sekunden – nicht für Mikrosekunden-Genauigkeit.
- Einige Features haben Fallbacks für robuste Fehlerbehandlung.

## 📜 Lizenz / License

Dieses Projekt ist unter der MIT-Lizenz lizenziert. Siehe [LICENSE](LICENSE) für Details.

## 🙏 Danksagung / Acknowledgments

Ein großes Dankeschön an:
- **sgrottl** für die Inspiration durch "movemouse" und "burnstok".
- Die .NET-Community für tolle Tools und Dokumentation.

---

**Viel Spaß mit BartsTOK!** 🎊 Wenn du Fragen hast, öffne ein Issue oder kontaktiere mich. 😊
