"""Settings manager for BartsTOK application."""
import json
import os
from pathlib import Path
from typing import List, Dict, Any
from dataclasses import dataclass, asdict, field


@dataclass
class ScheduleEntry:
    """Represents a scheduled action."""
    id: str = ""
    name: str = ""
    time: str = "00:00"
    action: str = "Start"
    days: str = "Everyday"
    enabled: bool = True

    def __post_init__(self):
        if not self.id:
            import uuid
            self.id = str(uuid.uuid4())


@dataclass
class AppSettings:
    """Application settings data class."""
    # Planner settings
    planner_enabled: bool = False
    stop_after_minutes: int = 0
    auto_start: bool = False
    
    # UI settings
    hide_move_mouse_window: bool = False
    topmost_when_running: bool = False
    minimise_when_not_running: bool = False
    hide_from_taskbar: bool = False
    hide_from_alt_tab: bool = False
    override_title: str = ""
    screen_burn_prevention: bool = False
    hide_tray_icon: bool = False
    tray_notifications: bool = False
    show_status_on_main: bool = True
    disable_button_animations: bool = False
    
    # Schedules
    schedules: List[Dict[str, Any]] = field(default_factory=list)
    
    # Behavior settings
    repeat_enabled: bool = False
    repeat_interval_seconds: int = 30
    repeat_interval_unit: str = "Seconds"
    auto_stop_on_user_activity: bool = False
    launch_move_mouse_at_startup: bool = False
    start_actions_when_move_mouse_launched: bool = False
    adjust_volume_when_move_mouse_running: bool = False
    adjust_volume_percent: int = 80
    continue_when_session_locked: bool = False
    pause_when_on_battery: bool = False
    enable_file_logging: bool = False


class SettingsManager:
    """Manages application settings persistence."""
    
    @staticmethod
    def get_settings_folder() -> Path:
        """Get the settings folder path."""
        if os.name == 'nt':  # Windows
            app_data = os.getenv('APPDATA')
            if app_data:
                return Path(app_data) / 'BartsTOK'
        # Linux/Mac
        home = Path.home()
        return home / '.config' / 'BartsTOK'
    
    @staticmethod
    def get_settings_file() -> Path:
        """Get the settings file path."""
        return SettingsManager.get_settings_folder() / 'settings.json'
    
    @staticmethod
    def load() -> AppSettings:
        """Load settings from file."""
        try:
            settings_folder = SettingsManager.get_settings_folder()
            settings_file = SettingsManager.get_settings_file()
            
            if not settings_folder.exists():
                settings_folder.mkdir(parents=True, exist_ok=True)
            
            if not settings_file.exists():
                default_settings = AppSettings()
                SettingsManager.save(default_settings)
                return default_settings
            
            with open(settings_file, 'r', encoding='utf-8') as f:
                data = json.load(f)
            
            # Convert snake_case from JSON back to Python dataclass
            return AppSettings(**data)
        except Exception as e:
            print(f"Error loading settings: {e}")
            return AppSettings()
    
    @staticmethod
    def save(settings: AppSettings) -> None:
        """Save settings to file."""
        try:
            settings_folder = SettingsManager.get_settings_folder()
            settings_file = SettingsManager.get_settings_file()
            
            if not settings_folder.exists():
                settings_folder.mkdir(parents=True, exist_ok=True)
            
            # Convert dataclass to dict
            data = asdict(settings)
            
            with open(settings_file, 'w', encoding='utf-8') as f:
                json.dump(data, f, indent=2, ensure_ascii=False)
        except Exception as e:
            print(f"Error saving settings: {e}")
