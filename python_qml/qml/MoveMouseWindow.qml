import QtQuick 2.15
import QtQuick.Controls 2.15
import QtQuick.Layouts 1.15
import QtQuick.Window 2.15

Window {
    id: moveMouseWindow
    width: 700
    height: 420
    title: "MoveBart"
    
    color: "#f0f0f0"
    
    RowLayout {
        anchors.fill: parent
        anchors.margins: 8
        spacing: 12
        
        // Visual area with movement preview
        Rectangle {
            Layout.fillWidth: true
            Layout.fillHeight: true
            color: "white"
            border.color: "#dddddd"
            border.width: 1
            radius: 6
            
            // Canvas for drawing the movement path
            Item {
                id: visualArea
                anchors.fill: parent
                anchors.margins: 10
                
                // Center marker
                Rectangle {
                    id: centerMarker
                    width: 96
                    height: 96
                    x: parent.width / 2 - width / 2
                    y: parent.height / 2 - height / 2
                    color: "#222222"
                    radius: 48
                    
                    Text {
                        anchors.centerIn: parent
                        text: "BART"
                        color: "white"
                        font.bold: true
                        font.pixelSize: 14
                    }
                    
                    // Animated rotation when running
                    RotationAnimation on rotation {
                        id: rotationAnimation
                        running: false
                        loops: Animation.Infinite
                        from: 0
                        to: 360
                        duration: 2000
                    }
                }
                
                // Path visualization (circle or random points)
                Canvas {
                    id: pathCanvas
                    anchors.fill: parent
                    
                    property int radius: sldRadius.value
                    property bool showCircle: rbCircle.checked
                    
                    onRadiusChanged: requestPaint()
                    onShowCircleChanged: requestPaint()
                    
                    onPaint: {
                        var ctx = getContext("2d")
                        ctx.clearRect(0, 0, width, height)
                        
                        var centerX = width / 2
                        var centerY = height / 2
                        
                        if (showCircle) {
                            // Draw circle path
                            ctx.strokeStyle = "#4CAF50"
                            ctx.lineWidth = 2
                            ctx.setLineDash([5, 5])
                            ctx.beginPath()
                            ctx.arc(centerX, centerY, radius, 0, 2 * Math.PI)
                            ctx.stroke()
                        }
                    }
                }
            }
        }
        
        // Right side: Tabbed controls
        Rectangle {
            Layout.preferredWidth: 300
            Layout.fillHeight: true
            color: "transparent"
            
            TabBar {
                id: tabBar
                width: parent.width
                
                TabButton {
                    text: "Basic"
                }
                TabButton {
                    text: "Advanced"
                }
            }
            
            StackLayout {
                width: parent.width
                anchors.top: tabBar.bottom
                anchors.bottom: parent.bottom
                currentIndex: tabBar.currentIndex
                
                // Basic Tab
                Item {
                    ColumnLayout {
                        anchors.fill: parent
                        anchors.margins: 8
                        spacing: 8
                        
                        Text {
                            text: "MoveBart - Controls"
                            font.pixelSize: 16
                            font.bold: true
                        }
                        
                        RowLayout {
                            spacing: 8
                            
                            Button {
                                id: btnStartMove
                                text: "Start"
                                Layout.preferredWidth: 84
                                enabled: !moveMouseController.isRunning
                                
                                background: Rectangle {
                                    color: parent.enabled ? (parent.pressed ? "#3d8b40" : "#4CAF50") : "#cccccc"
                                    radius: 4
                                }
                                
                                onClicked: {
                                    rotationAnimation.running = true
                                    // TODO: Call controller.startMovement()
                                }
                            }
                            
                            Button {
                                id: btnStopMove
                                text: "Stop"
                                Layout.preferredWidth: 84
                                enabled: false // TODO: bind to controller
                                
                                background: Rectangle {
                                    color: parent.enabled ? (parent.pressed ? "#c23a2e" : "#F44336") : "#cccccc"
                                    radius: 4
                                }
                                
                                onClicked: {
                                    rotationAnimation.running = false
                                    // TODO: Call controller.stopMovement()
                                }
                            }
                            
                            Button {
                                text: "Zentrum setzen"
                                Layout.preferredWidth: 110
                                
                                background: Rectangle {
                                    color: parent.pressed ? "#555555" : "#666666"
                                    radius: 4
                                }
                                
                                contentItem: Text {
                                    text: parent.parent.text
                                    color: "white"
                                    horizontalAlignment: Text.AlignHCenter
                                    verticalAlignment: Text.AlignVCenter
                                    font.pixelSize: 11
                                }
                                
                                onClicked: {
                                    // TODO: Set center to current mouse position
                                }
                            }
                        }
                        
                        Text {
                            text: "Radius (px)"
                            font.pixelSize: 12
                        }
                        
                        Slider {
                            id: sldRadius
                            Layout.fillWidth: true
                            from: 0
                            to: 600
                            value: 120
                            stepSize: 10
                        }
                        
                        Text {
                            text: qsTr("Radius: %1 px").arg(Math.round(sldRadius.value))
                            font.pixelSize: 11
                            color: "#666666"
                        }
                        
                        Text {
                            text: "Speed"
                            font.pixelSize: 12
                        }
                        
                        Slider {
                            id: sldSpeed
                            Layout.fillWidth: true
                            from: 1
                            to: 100
                            value: 40
                        }
                        
                        Text {
                            text: qsTr("Speed: %1").arg(Math.round(sldSpeed.value))
                            font.pixelSize: 11
                            color: "#666666"
                        }
                        
                        Text {
                            text: "Mode"
                            font.pixelSize: 12
                        }
                        
                        RowLayout {
                            RadioButton {
                                id: rbCircle
                                text: "Kreis"
                                checked: true
                            }
                            
                            RadioButton {
                                id: rbRandom
                                text: "Zufall"
                            }
                        }
                        
                        CheckBox {
                            id: cbSmooth
                            text: "Smooth/Interpolation"
                            checked: true
                        }
                        
                        GroupBox {
                            Layout.fillWidth: true
                            title: "Klicks"
                            
                            ColumnLayout {
                                anchors.fill: parent
                                
                                CheckBox {
                                    id: cbClickEnabled
                                    text: "Klicks aktivieren"
                                }
                                
                                RowLayout {
                                    enabled: cbClickEnabled.checked
                                    
                                    Text {
                                        text: "Intervall (s):"
                                        Layout.preferredWidth: 80
                                    }
                                    
                                    TextField {
                                        id: txtClickInterval
                                        text: "5"
                                        Layout.preferredWidth: 60
                                        horizontalAlignment: TextInput.AlignHCenter
                                    }
                                }
                                
                                ComboBox {
                                    id: cbClickType
                                    Layout.fillWidth: true
                                    enabled: cbClickEnabled.checked
                                    model: ["Linksklick", "Rechtsklick", "Doppelklick"]
                                }
                            }
                        }
                        
                        Item {
                            Layout.fillHeight: true
                        }
                    }
                }
                
                // Advanced Tab
                Item {
                    ScrollView {
                        anchors.fill: parent
                        
                        ColumnLayout {
                            width: parent.parent.width - 20
                            spacing: 8
                            padding: 8
                            
                            Text {
                                text: "Erweiterte Optionen"
                                font.pixelSize: 14
                                font.bold: true
                            }
                            
                            GroupBox {
                                Layout.fillWidth: true
                                title: "Follow-Modi"
                                
                                ColumnLayout {
                                    anchors.fill: parent
                                    
                                    RadioButton {
                                        id: rbFollowNone
                                        text: "Kein Follow"
                                        checked: true
                                    }
                                    
                                    RadioButton {
                                        id: rbFollowMouse
                                        text: "Follow Mouse"
                                    }
                                    
                                    RadioButton {
                                        id: rbFollowWindow
                                        text: "Follow Window"
                                    }
                                    
                                    TextField {
                                        id: txtFollowWindow
                                        Layout.fillWidth: true
                                        placeholderText: "Fenstertitel..."
                                        enabled: rbFollowWindow.checked
                                    }
                                }
                            }
                            
                            GroupBox {
                                Layout.fillWidth: true
                                title: "Presets"
                                
                                ColumnLayout {
                                    anchors.fill: parent
                                    spacing: 6
                                    
                                    ComboBox {
                                        id: cbPresets
                                        Layout.fillWidth: true
                                        model: ["Standard", "Office", "Gaming", "Custom"]
                                    }
                                    
                                    RowLayout {
                                        Button {
                                            text: "Laden"
                                            Layout.fillWidth: true
                                            
                                            background: Rectangle {
                                                color: parent.pressed ? "#1976D2" : "#2196F3"
                                                radius: 4
                                            }
                                        }
                                        
                                        Button {
                                            text: "Speichern"
                                            Layout.fillWidth: true
                                            
                                            background: Rectangle {
                                                color: parent.pressed ? "#3d8b40" : "#4CAF50"
                                                radius: 4
                                            }
                                        }
                                    }
                                }
                            }
                            
                            CheckBox {
                                id: cbStopOnManualMove
                                text: "Stop bei manueller Bewegung"
                            }
                            
                            CheckBox {
                                id: cbHideWhenRunning
                                text: "Fenster verbergen beim Ausführen"
                            }
                            
                            Item {
                                Layout.fillHeight: true
                            }
                        }
                    }
                }
            }
        }
    }
    
    // Mock controller object (to be replaced with actual controller)
    QtObject {
        id: moveMouseController
        property bool isRunning: false
    }
}
