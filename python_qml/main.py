"""Main window controller for BartsTOK application."""
import sys
import time
import random
from datetime import datetime
from typing import List, Optional
from pathlib import Path

from PySide6.QtCore import QObject, Signal, Slot, QTimer, Property, QUrl
from PySide6.QtGui import QGuiApplication
from PySide6.QtQml import QQmlApplicationEngine, QQmlComponent

try:
    import pyautogui
    import keyboard
    from pynput.keyboard import Controller, Key
except ImportError:
    print("Warning: Some input libraries not available. Install requirements.txt")
    pyautogui = None
    keyboard = None
    Controller = None
    Key = None

from core.settings_manager import SettingsManager, AppSettings


class MainWindowController(QObject):
    """Controller for the main window."""
    
    # Signals
    statusChanged = Signal(str)
    logAdded = Signal(str)
    runningStateChanged = Signal(bool)
    
    def __init__(self, engine: QQmlApplicationEngine):
        super().__init__()
        
        self._engine = engine
        
        # State variables
        self._is_running = False
        self._text_lines: List[str] = []
        self._current_line_index = 0
        self._status = "Status: Gestoppt ⏹"
        
        # Settings
        self.settings: AppSettings = SettingsManager.load()
        
        # Timers
        self._main_timer = QTimer()
        self._main_timer.timeout.connect(self._on_main_timer_tick)
        
        self._stop_after_timer = QTimer()
        self._stop_after_timer.timeout.connect(self._on_stop_after_timer_tick)
        self._stop_after_seconds_remaining = 0
        
        # Input simulation
        if Controller:
            self._keyboard = Controller()
        else:
            self._keyboard = None
        
        # Random number generator
        self._random = random.Random()
        
        # Log entries
        self._log_entries: List[str] = []
        
        # Child windows
        self._about_window = None
        self._move_mouse_window = None
        self._advanced_settings_window = None
    
    # Properties
    @Property(str, notify=statusChanged)
    def status(self) -> str:
        """Get current status."""
        return self._status
    
    @status.setter
    def status(self, value: str):
        """Set current status."""
        if self._status != value:
            self._status = value
            self.statusChanged.emit(value)
    
    @Property(bool, notify=runningStateChanged)
    def is_running(self) -> bool:
        """Check if the application is running."""
        return self._is_running
    
    @is_running.setter
    def is_running(self, value: bool):
        """Set running state."""
        if self._is_running != value:
            self._is_running = value
            self.runningStateChanged.emit(value)
    
    # Slots
    @Slot(str, float, float, str, bool, bool)
    def start(self, text: str, line_pause: float, char_pause: float, 
              mode: str, auto_newline: bool, keyboard_type: str):
        """Start the text automation."""
        if not text.strip():
            self.add_log("❌ Fehler: Bitte geben Sie Text ein!")
            return
        
        if line_pause < 0:
            self.add_log("❌ Fehler: Ungültige Pause!")
            return
        
        # Parse text into lines
        self._text_lines = text.split('\n')
        self._current_line_index = 0
        self.is_running = True
        
        # Store parameters for use in timer
        self._line_pause = line_pause
        self._char_pause = char_pause
        self._mode = mode
        self._auto_newline = auto_newline
        self._keyboard_type = keyboard_type
        
        # Set up timer
        interval_ms = int(line_pause * 1000)
        self._main_timer.setInterval(interval_ms)
        self._main_timer.start()
        
        # Start stop-after timer if enabled
        if self.settings.planner_enabled and self.settings.stop_after_minutes > 0:
            self._stop_after_seconds_remaining = self.settings.stop_after_minutes * 60
            self._stop_after_timer.start(1000)  # 1 second interval
            self.add_log(f"⏱ Planer aktiviert: Stoppe nach {self.settings.stop_after_minutes} Minuten")
        
        self.status = "Status: Läuft ▶"
        self.add_log("✓ Barts TOK gestartet")
    
    @Slot()
    def stop(self):
        """Stop the text automation."""
        self.is_running = False
        self._main_timer.stop()
        self._stop_after_timer.stop()
        self._stop_after_seconds_remaining = 0
        
        self.status = "Status: Gestoppt ⏹"
        self.add_log("✗ Barts TOK gestoppt")
    
    @Slot(str)
    def add_log(self, message: str):
        """Add a log entry."""
        timestamp = datetime.now().strftime("%H:%M:%S")
        log_entry = f"[{timestamp}] {message}"
        self._log_entries.insert(0, log_entry)
        
        # Keep only last 50 entries
        if len(self._log_entries) > 50:
            self._log_entries = self._log_entries[:50]
        
        self.logAdded.emit(log_entry)
        
        # Optional file logging
        if self.settings.enable_file_logging:
            try:
                log_folder = SettingsManager.get_settings_folder()
                log_file = log_folder / "app.log"
                with open(log_file, 'a', encoding='utf-8') as f:
                    f.write(f"{log_entry}\n")
            except Exception as e:
                print(f"Error writing log: {e}")
    
    def _on_main_timer_tick(self):
        """Handle main timer tick."""
        if not self.is_running:
            return
        
        line_to_send = ""
        
        if self._mode == "random":
            line_to_send = self._random.choice(self._text_lines)
        elif self._mode == "sequential" or self._mode == "sequential_stop":
            if self._current_line_index < len(self._text_lines):
                line_to_send = self._text_lines[self._current_line_index]
                self._current_line_index += 1
                
                if self._current_line_index >= len(self._text_lines):
                    if self._mode == "sequential_stop":
                        self.stop()
                        return
                    self._current_line_index = 0
        
        # Send the text
        self._send_text_with_pause(line_to_send)
        
        # Send newline if enabled
        if self._auto_newline:
            self._send_key('enter')
        
        # Update status
        current_time = datetime.now().strftime("%H:%M:%S")
        self.status = f"Status: Läuft ▶ - {current_time}"
    
    def _on_stop_after_timer_tick(self):
        """Handle stop-after timer tick."""
        if self._stop_after_seconds_remaining > 0:
            self._stop_after_seconds_remaining -= 1
            
            # Update status with remaining time
            hours = self._stop_after_seconds_remaining // 3600
            minutes = (self._stop_after_seconds_remaining % 3600) // 60
            seconds = self._stop_after_seconds_remaining % 60
            self.status = f"Status: Läuft ▶ - verbleibend {hours:02d}:{minutes:02d}:{seconds:02d}"
            
            if self._stop_after_seconds_remaining <= 0:
                self._stop_after_timer.stop()
                self.add_log("⏱ Planer: Laufzeit abgelaufen, stoppe Bart")
                self.stop()
    
    def _send_text_with_pause(self, text: str):
        """Send text with character pause."""
        if not text:
            return
        
        try:
            for char in text:
                self._send_char(char)
                if self._char_pause > 0:
                    time.sleep(self._char_pause)
            
            self.add_log(f"→ Gesendet: '{text}'")
        except Exception as e:
            self.add_log(f"❌ Fehler beim Senden: {e}")
    
    def _send_char(self, char: str):
        """Send a single character."""
        try:
            if self._keyboard_type == "pyautogui" and pyautogui:
                pyautogui.write(char, interval=0)
            elif self._keyboard_type == "keyboard" and keyboard:
                keyboard.write(char)
            elif self._keyboard_type == "pynput" and self._keyboard:
                self._keyboard.type(char)
            else:
                # Fallback to pynput
                if self._keyboard:
                    self._keyboard.type(char)
        except Exception as e:
            print(f"Error sending char: {e}")
    
    def _send_key(self, key_name: str):
        """Send a special key."""
        try:
            if self._keyboard_type == "pyautogui" and pyautogui:
                pyautogui.press(key_name)
            elif self._keyboard_type == "keyboard" and keyboard:
                keyboard.press_and_release(key_name)
            elif self._keyboard_type == "pynput" and self._keyboard:
                if key_name == 'enter':
                    self._keyboard.press(Key.enter)
                    self._keyboard.release(Key.enter)
            else:
                # Fallback
                if self._keyboard:
                    if key_name == 'enter':
                        self._keyboard.press(Key.enter)
                        self._keyboard.release(Key.enter)
        except Exception as e:
            print(f"Error sending key: {e}")
    
    @Slot()
    def open_move_mouse_window(self):
        """Open the Move Mouse window."""
        try:
            if self._move_mouse_window is None:
                qml_file = Path(__file__).parent / "qml" / "MoveMouseWindow.qml"
                component = QQmlComponent(self._engine, QUrl.fromLocalFile(str(qml_file)))
                self._move_mouse_window = component.create()
                if self._move_mouse_window:
                    self.add_log("✓ MoveBart-Fenster geöffnet")
                else:
                    self.add_log(f"❌ Fehler beim Laden: {component.errorString()}")
            else:
                # Show existing window
                self._move_mouse_window.setProperty("visible", True)
                self.add_log("✓ MoveBart-Fenster angezeigt")
        except Exception as e:
            self.add_log(f"❌ Fehler beim Öffnen von MoveBart: {e}")
    
    @Slot()
    def open_advanced_settings(self):
        """Open advanced settings window."""
        try:
            if self._advanced_settings_window is None:
                qml_file = Path(__file__).parent / "qml" / "AdvancedSettingsWindow.qml"
                component = QQmlComponent(self._engine, QUrl.fromLocalFile(str(qml_file)))
                self._advanced_settings_window = component.create()
                if self._advanced_settings_window:
                    self.add_log("✓ Erweiterte Einstellungen geöffnet")
                else:
                    self.add_log(f"❌ Fehler beim Laden: {component.errorString()}")
            else:
                # Show existing window
                self._advanced_settings_window.setProperty("visible", True)
                self.add_log("✓ Erweiterte Einstellungen angezeigt")
        except Exception as e:
            self.add_log(f"❌ Fehler beim Öffnen der Einstellungen: {e}")
    
    @Slot()
    def open_about(self):
        """Open about window."""
        try:
            if self._about_window is None:
                qml_file = Path(__file__).parent / "qml" / "AboutWindow.qml"
                component = QQmlComponent(self._engine, QUrl.fromLocalFile(str(qml_file)))
                self._about_window = component.create()
                if self._about_window:
                    self.add_log("✓ Über-Fenster geöffnet")
                else:
                    self.add_log(f"❌ Fehler beim Laden: {component.errorString()}")
            else:
                # Show existing window
                self._about_window.setProperty("visible", True)
                self.add_log("✓ Über-Fenster angezeigt")
        except Exception as e:
            self.add_log(f"❌ Fehler beim Öffnen des Über-Fensters: {e}")


def main():
    """Main entry point."""
    app = QGuiApplication(sys.argv)
    app.setOrganizationName("BartsTOK")
    app.setApplicationName("BartsTOK")
    
    # Create QML engine
    engine = QQmlApplicationEngine()
    
    # Create controller with engine reference
    controller = MainWindowController(engine)
    
    # Expose controller to QML
    engine.rootContext().setContextProperty("controller", controller)
    
    # Load main QML file
    qml_file = Path(__file__).parent / "qml" / "MainWindow.qml"
    engine.load(QUrl.fromLocalFile(str(qml_file)))
    
    if not engine.rootObjects():
        sys.exit(-1)
    
    sys.exit(app.exec())


if __name__ == "__main__":
    main()
