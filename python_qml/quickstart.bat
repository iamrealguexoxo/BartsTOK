@echo off
REM Quick start script for BartsTOK Python/QML on Windows

echo ================================================
echo BartsTOK Python/QML Quick Start
echo ================================================
echo.

REM Check Python version
python --version
if %errorlevel% neq 0 (
    echo Error: Python not found! Please install Python 3.8 or higher.
    pause
    exit /b 1
)

REM Create virtual environment if it doesn't exist
if not exist "venv" (
    echo.
    echo Creating virtual environment...
    python -m venv venv
    echo Virtual environment created
)

REM Activate virtual environment
echo.
echo Activating virtual environment...
call venv\Scripts\activate.bat

REM Install dependencies
echo.
echo Installing dependencies...
pip install -q --upgrade pip
pip install -q -r requirements.txt
echo Dependencies installed

REM Test structure
echo.
echo Testing structure...
python test_structure.py

REM Run application
echo.
echo ================================================
echo Starting BartsTOK...
echo ================================================
echo.
python main.py

pause
