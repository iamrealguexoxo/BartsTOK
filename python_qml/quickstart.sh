#!/bin/bash
# Quick start script for BartsTOK Python/QML

echo "================================================"
echo "BartsTOK Python/QML Quick Start"
echo "================================================"
echo ""

# Check Python version
python_version=$(python3 --version 2>&1 | awk '{print $2}')
echo "✓ Python version: $python_version"

# Create virtual environment if it doesn't exist
if [ ! -d "venv" ]; then
    echo ""
    echo "Creating virtual environment..."
    python3 -m venv venv
    echo "✓ Virtual environment created"
fi

# Activate virtual environment
echo ""
echo "Activating virtual environment..."
if [[ "$OSTYPE" == "msys" || "$OSTYPE" == "win32" ]]; then
    source venv/Scripts/activate
else
    source venv/bin/activate
fi
echo "✓ Virtual environment activated"

# Install dependencies
echo ""
echo "Installing dependencies..."
pip install -q --upgrade pip
pip install -q -r requirements.txt
echo "✓ Dependencies installed"

# Test structure
echo ""
echo "Testing structure..."
python test_structure.py

# Run application
echo ""
echo "================================================"
echo "Starting BartsTOK..."
echo "================================================"
echo ""
python main.py
