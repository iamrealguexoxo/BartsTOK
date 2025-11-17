# 🎉 BartsTOK - Python Qt Quick/QML Version

![BartsTOK Logo](../loading.gif)

## 📝 Beschreibung

Dies ist die **Python Qt Quick/QML** Version von BartsTOK - portiert von der ursprünglichen C# WPF-Anwendung. 

BartsTOK ist eine Anwendung, die:
- Deinen PC wach hält
- Text automatisch eingibt
- Die Maus bewegt (MoveBart)
- Bildschirm-Brand verhindert

## 🚀 Installation

### Voraussetzungen
- Python 3.8 oder höher
- Qt 6 (wird automatisch mit PySide6 installiert)

### Setup

1. **Klonen und in das Verzeichnis wechseln**:
   ```bash
   cd python_qml
   ```

2. **Virtuelle Umgebung erstellen (empfohlen)**:
   ```bash
   python -m venv venv
   
   # Windows
   venv\Scripts\activate
   
   # Linux/Mac
   source venv/bin/activate
   ```

3. **Abhängigkeiten installieren**:
   ```bash
   pip install -r requirements.txt
   ```

## 🎮 Verwendung

### Anwendung starten:
```bash
python main.py
```

### Funktionen:

#### Virtuelle Tastatur
- **Pynput Keyboard (Standard)**: Plattformübergreifende Tastatureingabe
- **PyAutoGUI Keyboard**: Alternative Methode
- **Keyboard Library**: Weitere Alternative (Windows)

#### Verhalten
- **Zeilen nacheinander**: Sendet alle Zeilen in Reihenfolge (Schleife)
- **Zufällige Zeile**: Wählt zufällig eine Zeile aus
- **Nacheinander und stoppen**: Sendet alle Zeilen einmal und stoppt dann

#### Tippgeschwindigkeit
- **Pause zwischen Zeilen**: Zeit zwischen dem Senden einzelner Zeilen (in Sekunden)
- **Pause zwischen Zeichen**: Zeit zwischen einzelnen Zeichen (in Sekunden)

#### Steuerung
- **F1**: Start
- **F2**: Stop

## 📂 Projektstruktur

```
python_qml/
├── main.py                      # Haupteinstiegspunkt
├── requirements.txt             # Python-Abhängigkeiten
├── core/
│   └── settings_manager.py      # Einstellungsverwaltung
├── qml/
│   ├── MainWindow.qml           # Hauptfenster UI
│   ├── MoveMouseWindow.qml      # MoveBart-Fenster (geplant)
│   ├── AdvancedSettings.qml     # Erweiterte Einstellungen (geplant)
│   └── AboutWindow.qml          # Über-Dialog (geplant)
└── resources/
    └── (Icons und Bilder)
```

## 🔧 Unterschiede zur C# Version

### Implementiert:
- ✅ Hauptfenster mit allen Grundfunktionen
- ✅ Text-Automation mit verschiedenen Modi
- ✅ Tastatur-Simulation (mehrere Methoden)
- ✅ Einstellungs-Persistenz
- ✅ Logging-Funktionalität
- ✅ Planer (Stop-After-Timer)

### Noch zu implementieren:
- ⏳ MoveBart-Fenster (Mausbewegung)
- ⏳ Erweiterte Einstellungen-Dialog
- ⏳ Über-Dialog
- ⏳ System-Tray-Integration
- ⏳ Scheduler (zeitgesteuerte Aktionen)
- ⏳ Screen-Burn-Prevention
- ⏳ Hotkeys (global)

## 🛠️ Entwicklung

### Technologie-Stack:
- **Python 3.8+**: Programmiersprache
- **PySide6**: Qt 6 Bindings für Python
- **Qt Quick/QML**: Deklarative UI-Framework
- **PyAutoGUI**: Tastatur-/Maussteuerung
- **keyboard**: Alternative Tastatursteuerung
- **pynput**: Plattformübergreifende Input-Steuerung

### QML-Komponenten erweitern:
QML-Dateien befinden sich im `qml/`-Verzeichnis und können mit jedem Texteditor bearbeitet werden. Qt Creator bietet zusätzliche Unterstützung.

### Python-Controller erweitern:
Die Business-Logik befindet sich in `main.py` und den Modulen im `core/`-Verzeichnis.

## 🐛 Bekannte Einschränkungen

- **Plattformabhängigkeit**: Einige Features funktionieren nur auf bestimmten Betriebssystemen
- **Tastatureingabe**: Je nach System und Sicherheitseinstellungen können bestimmte Eingabemethoden blockiert werden
- **Globale Hotkeys**: Noch nicht implementiert (erfordert zusätzliche Bibliothek)

## 📜 Lizenz

Dieses Projekt ist unter der MIT-Lizenz lizenziert - siehe die [LICENSE](../LICENSE)-Datei für Details.

## 🙏 Danksagung

- Original C# WPF Version von BartsTOK
- Qt Project für das Qt Framework
- Python Software Foundation

---

**Viel Spaß mit BartsTOK Python/QML!** 🎊
