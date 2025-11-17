import QtQuick 2.15
import QtQuick.Controls 2.15
import QtQuick.Layouts 1.15
import QtQuick.Window 2.15

ApplicationWindow {
    id: mainWindow
    width: 900
    height: 900
    visible: true
    title: "Barts TOK"
    
    // Dark theme colors
    color: "#1e1e1e"
    
    // Main layout
    ColumnLayout {
        anchors.fill: parent
        anchors.margins: 12
        spacing: 10
        
        // Header with title
        RowLayout {
            Layout.alignment: Qt.AlignHCenter
            spacing: 8
            
            // Left animated GIF placeholder
            Rectangle {
                width: 32
                height: 32
                color: "#FFD700"
                radius: 16
                
                Text {
                    anchors.centerIn: parent
                    text: "🎬"
                    font.pixelSize: 20
                }
            }
            
            Text {
                text: "BARTS TOK"
                font.pixelSize: 20
                font.bold: true
                color: "#FFD700"
            }
            
            // Right animated GIF placeholder
            Rectangle {
                width: 32
                height: 32
                color: "#FFD700"
                radius: 16
                
                Text {
                    anchors.centerIn: parent
                    text: "🎬"
                    font.pixelSize: 20
                }
            }
        }
        
        // Virtual Keyboard GroupBox
        GroupBox {
            Layout.fillWidth: true
            title: "Virtuelle Tastatur"
            
            background: Rectangle {
                color: "transparent"
                border.color: "#FFD700"
                border.width: 1
                radius: 4
            }
            
            label: Text {
                text: parent.title
                color: "#FFD700"
                font.bold: true
                x: 10
            }
            
            ColumnLayout {
                anchors.fill: parent
                spacing: 6
                
                RadioButton {
                    id: rbPynput
                    text: "Pynput Keyboard (Standard)"
                    checked: true
                    
                    contentItem: Text {
                        text: rbPynput.text
                        color: "white"
                        leftPadding: rbPynput.indicator.width + rbPynput.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                RadioButton {
                    id: rbPyAutoGui
                    text: "PyAutoGUI Keyboard"
                    
                    contentItem: Text {
                        text: rbPyAutoGui.text
                        color: "white"
                        leftPadding: rbPyAutoGui.indicator.width + rbPyAutoGui.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                RadioButton {
                    id: rbKeyboard
                    text: "Keyboard Library (Alt)"
                    
                    contentItem: Text {
                        text: rbKeyboard.text
                        color: "white"
                        leftPadding: rbKeyboard.indicator.width + rbKeyboard.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
            }
        }
        
        // Behavior GroupBox
        GroupBox {
            Layout.fillWidth: true
            title: "Verhalten"
            
            background: Rectangle {
                color: "transparent"
                border.color: "#FFD700"
                border.width: 1
                radius: 4
            }
            
            label: Text {
                text: parent.title
                color: "#FFD700"
                font.bold: true
                x: 10
            }
            
            ColumnLayout {
                anchors.fill: parent
                spacing: 6
                
                RadioButton {
                    id: rbSequential
                    text: "Zeilen nacheinander"
                    checked: true
                    
                    contentItem: Text {
                        text: rbSequential.text
                        color: "white"
                        leftPadding: rbSequential.indicator.width + rbSequential.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                RadioButton {
                    id: rbRandom
                    text: "Zufällige Zeile"
                    
                    contentItem: Text {
                        text: rbRandom.text
                        color: "white"
                        leftPadding: rbRandom.indicator.width + rbRandom.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                RadioButton {
                    id: rbSequentialStop
                    text: "Nacheinander und stoppen"
                    
                    contentItem: Text {
                        text: rbSequentialStop.text
                        color: "white"
                        leftPadding: rbSequentialStop.indicator.width + rbSequentialStop.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                CheckBox {
                    id: cbAutoNewline
                    text: "Auto Enter nach jeder Zeile"
                    checked: true
                    
                    contentItem: Text {
                        text: cbAutoNewline.text
                        color: "white"
                        leftPadding: cbAutoNewline.indicator.width + cbAutoNewline.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
            }
        }
        
        // Typing Speed GroupBox
        GroupBox {
            Layout.fillWidth: true
            title: "Tippgeschwindigkeit"
            
            background: Rectangle {
                color: "transparent"
                border.color: "#FFD700"
                border.width: 1
                radius: 4
            }
            
            label: Text {
                text: parent.title
                color: "#FFD700"
                font.bold: true
                x: 10
            }
            
            ColumnLayout {
                anchors.fill: parent
                spacing: 6
                
                RowLayout {
                    spacing: 10
                    
                    Text {
                        text: "Pause zwischen Zeilen (Sek):"
                        color: "white"
                        Layout.preferredWidth: 200
                    }
                    
                    TextField {
                        id: txtLinePause
                        text: "5"
                        Layout.preferredWidth: 80
                        horizontalAlignment: TextInput.AlignHCenter
                        
                        background: Rectangle {
                            color: "#333333"
                            border.color: "#555555"
                            border.width: 1
                        }
                        
                        color: "white"
                    }
                }
                
                RowLayout {
                    spacing: 10
                    
                    Text {
                        text: "Pause zwischen Zeichen (Sek):"
                        color: "white"
                        Layout.preferredWidth: 200
                    }
                    
                    TextField {
                        id: txtCharPause
                        text: "0.01"
                        Layout.preferredWidth: 80
                        horizontalAlignment: TextInput.AlignHCenter
                        
                        background: Rectangle {
                            color: "#333333"
                            border.color: "#555555"
                            border.width: 1
                        }
                        
                        color: "white"
                    }
                }
            }
        }
        
        // Text input label
        Text {
            text: "Text Eingaben (jede Zeile wird separat behandelt):"
            color: "#FFD700"
            font.bold: true
        }
        
        // Text input area
        ScrollView {
            Layout.fillWidth: true
            Layout.preferredHeight: 120
            
            TextArea {
                id: txtInput
                placeholderText: "Geben Sie hier Ihren Text ein..."
                wrapMode: TextArea.Wrap
                
                background: Rectangle {
                    color: "#333333"
                    border.color: "#555555"
                    border.width: 1
                }
                
                color: "white"
                selectionColor: "#FFD700"
                selectedTextColor: "black"
            }
        }
        
        // Spacer
        Item {
            Layout.fillHeight: true
        }
        
        // Control buttons
        RowLayout {
            Layout.fillWidth: true
            spacing: 10
            
            Button {
                id: btnStart
                text: "▶ START (F1)"
                Layout.fillWidth: true
                enabled: !controller.is_running
                
                background: Rectangle {
                    color: btnStart.enabled ? (btnStart.pressed ? "#3d8b40" : "#4CAF50") : "#666666"
                    radius: 4
                }
                
                contentItem: Text {
                    text: btnStart.text
                    color: "black"
                    font.bold: true
                    font.pixelSize: 12
                    horizontalAlignment: Text.AlignHCenter
                    verticalAlignment: Text.AlignVCenter
                }
                
                onClicked: {
                    var keyboardType = "pynput"
                    if (rbPyAutoGui.checked) keyboardType = "pyautogui"
                    else if (rbKeyboard.checked) keyboardType = "keyboard"
                    
                    var mode = "sequential"
                    if (rbRandom.checked) mode = "random"
                    else if (rbSequentialStop.checked) mode = "sequential_stop"
                    
                    controller.start(
                        txtInput.text,
                        parseFloat(txtLinePause.text),
                        parseFloat(txtCharPause.text),
                        mode,
                        cbAutoNewline.checked,
                        keyboardType
                    )
                }
            }
            
            Button {
                id: btnStop
                text: "⏹ STOP (F2)"
                Layout.fillWidth: true
                enabled: controller.is_running
                
                background: Rectangle {
                    color: btnStop.enabled ? (btnStop.pressed ? "#c23a2e" : "#F44336") : "#666666"
                    radius: 4
                }
                
                contentItem: Text {
                    text: btnStop.text
                    color: "black"
                    font.bold: true
                    font.pixelSize: 12
                    horizontalAlignment: Text.AlignHCenter
                    verticalAlignment: Text.AlignVCenter
                }
                
                onClicked: controller.stop()
            }
            
            Button {
                id: btnMoveBart
                text: "MoveBart\n(beta)"
                Layout.preferredWidth: 110
                
                background: Rectangle {
                    color: btnMoveBart.pressed ? "#1976D2" : "#2196F3"
                    radius: 4
                }
                
                contentItem: Text {
                    text: btnMoveBart.text
                    color: "black"
                    font.bold: true
                    font.pixelSize: 10
                    horizontalAlignment: Text.AlignHCenter
                    verticalAlignment: Text.AlignVCenter
                }
                
                onClicked: controller.open_move_mouse_window()
            }
        }
        
        // Status label
        Text {
            text: "Status:"
            color: "#FFD700"
            font.bold: true
        }
        
        Rectangle {
            Layout.fillWidth: true
            Layout.preferredHeight: 40
            color: "#333333"
            border.color: "#FFD700"
            border.width: 1
            
            Text {
                id: lblStatus
                anchors.fill: parent
                anchors.margins: 10
                text: controller.status
                color: "white"
                font.pixelSize: 13
                verticalAlignment: Text.AlignVCenter
            }
        }
        
        // Minimize to tray checkbox
        CheckBox {
            id: cbMinimizeToTray
            text: "Minimiert im Tray behalten"
            checked: true
            
            contentItem: Text {
                text: cbMinimizeToTray.text
                color: "white"
                leftPadding: cbMinimizeToTray.indicator.width + cbMinimizeToTray.spacing
                verticalAlignment: Text.AlignVCenter
            }
        }
        
        // Log expander
        GroupBox {
            Layout.fillWidth: true
            title: "Log"
            
            background: Rectangle {
                color: "transparent"
                border.color: "#FFD700"
                border.width: 1
                radius: 4
            }
            
            label: Text {
                text: parent.title
                color: "#FFD700"
                font.bold: true
                x: 10
            }
            
            ScrollView {
                anchors.fill: parent
                height: 120
                
                ListView {
                    id: logBox
                    model: ListModel {
                        id: logModel
                    }
                    
                    delegate: Text {
                        text: model.message
                        color: "#00FF00"
                        font.family: "Monospace"
                        font.pixelSize: 11
                    }
                    
                    Connections {
                        target: controller
                        function onLogAdded(message) {
                            logModel.insert(0, {"message": message})
                            if (logModel.count > 50) {
                                logModel.remove(50, logModel.count - 50)
                            }
                        }
                    }
                }
            }
        }
        
        // Bottom buttons
        RowLayout {
            Layout.fillWidth: true
            spacing: 8
            
            Button {
                text: "Erweiterte Einstellungen"
                Layout.preferredWidth: 190
                
                background: Rectangle {
                    color: parent.pressed ? "#555555" : "#666666"
                    radius: 4
                }
                
                contentItem: Text {
                    text: parent.parent.text
                    color: "white"
                    horizontalAlignment: Text.AlignHCenter
                    verticalAlignment: Text.AlignVCenter
                }
                
                onClicked: controller.open_advanced_settings()
            }
            
            Button {
                text: "Über"
                Layout.preferredWidth: 90
                
                background: Rectangle {
                    color: parent.pressed ? "#555555" : "#666666"
                    radius: 4
                }
                
                contentItem: Text {
                    text: parent.parent.text
                    color: "white"
                    horizontalAlignment: Text.AlignHCenter
                    verticalAlignment: Text.AlignVCenter
                }
                
                onClicked: controller.open_about()
            }
            
            Item {
                Layout.fillWidth: true
            }
        }
    }
    
    // Keyboard shortcuts
    Shortcut {
        sequence: "F1"
        onActivated: {
            if (btnStart.enabled) {
                btnStart.clicked()
            }
        }
    }
    
    Shortcut {
        sequence: "F2"
        onActivated: {
            if (btnStop.enabled) {
                btnStop.clicked()
            }
        }
    }
}
