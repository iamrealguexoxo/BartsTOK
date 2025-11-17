#!/usr/bin/env python3
"""Test script to verify Python/QML structure."""

import sys
from pathlib import Path

def test_imports():
    """Test if all necessary modules can be imported."""
    print("Testing imports...")
    
    errors = []
    
    # Test core modules
    try:
        from core.settings_manager import SettingsManager, AppSettings, ScheduleEntry
        print("✓ Settings manager imported successfully")
    except ImportError as e:
        errors.append(f"✗ Settings manager: {e}")
    
    # Test Qt imports
    try:
        from PySide6.QtCore import QObject, Signal, Slot, QTimer
        from PySide6.QtGui import QGuiApplication
        from PySide6.QtQml import QQmlApplicationEngine
        print("✓ PySide6 imports successful")
    except ImportError as e:
        errors.append(f"✗ PySide6: {e}")
    
    # Test optional imports
    try:
        import pyautogui
        print("✓ pyautogui imported")
    except ImportError:
        print("⚠ pyautogui not available (optional)")
    
    try:
        import keyboard
        print("✓ keyboard imported")
    except ImportError:
        print("⚠ keyboard not available (optional)")
    
    try:
        from pynput.keyboard import Controller, Key
        print("✓ pynput imported")
    except ImportError:
        print("⚠ pynput not available (optional)")
    
    return errors

def test_structure():
    """Test if all required files exist."""
    print("\nTesting file structure...")
    
    required_files = [
        "main.py",
        "requirements.txt",
        "README.md",
        "core/__init__.py",
        "core/settings_manager.py",
        "qml/MainWindow.qml",
        "qml/AboutWindow.qml",
    ]
    
    errors = []
    for file in required_files:
        path = Path(file)
        if path.exists():
            print(f"✓ {file} exists")
        else:
            errors.append(f"✗ {file} missing")
    
    return errors

def test_settings():
    """Test settings manager."""
    print("\nTesting settings manager...")
    
    try:
        from core.settings_manager import SettingsManager, AppSettings
        
        # Test creating default settings
        settings = AppSettings()
        print(f"✓ Default settings created")
        
        # Test saving
        SettingsManager.save(settings)
        print(f"✓ Settings saved to: {SettingsManager.get_settings_file()}")
        
        # Test loading
        loaded_settings = SettingsManager.load()
        print(f"✓ Settings loaded")
        
        return []
    except Exception as e:
        return [f"✗ Settings manager test failed: {e}"]

def main():
    """Run all tests."""
    print("=" * 60)
    print("BartsTOK Python/QML Structure Test")
    print("=" * 60)
    
    all_errors = []
    
    # Run tests
    all_errors.extend(test_imports())
    all_errors.extend(test_structure())
    all_errors.extend(test_settings())
    
    # Print summary
    print("\n" + "=" * 60)
    if all_errors:
        print("ERRORS FOUND:")
        for error in all_errors:
            print(error)
        print("\nSome tests failed. Please install dependencies:")
        print("  pip install -r requirements.txt")
        return 1
    else:
        print("✓ All tests passed!")
        print("\nYou can now run the application:")
        print("  python main.py")
        return 0

if __name__ == "__main__":
    sys.exit(main())
