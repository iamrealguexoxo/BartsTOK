import QtQuick 2.15
import QtQuick.Controls 2.15
import QtQuick.Layouts 1.15
import QtQuick.Window 2.15

Window {
    id: advancedSettingsWindow
    width: 450
    height: 750
    title: "Erweiterte Einstellungen (experimental)"
    modality: Qt.ApplicationModal
    
    color: "#1e1e1e"
    
    ColumnLayout {
        anchors.fill: parent
        anchors.margins: 12
        spacing: 10
        
        // Scrollable content
        ScrollView {
            Layout.fillWidth: true
            Layout.fillHeight: true
            clip: true
            
            ColumnLayout {
                width: advancedSettingsWindow.width - 50
                spacing: 10
                
                // General Settings
                Text {
                    text: "Allgemeine Einstellungen"
                    font.bold: true
                    color: "#FFD700"
                    font.pixelSize: 14
                }
                
                CheckBox {
                    id: cbPlannerEnable
                    text: "Planer aktivieren"
                    
                    contentItem: Text {
                        text: cbPlannerEnable.text
                        color: "white"
                        leftPadding: cbPlannerEnable.indicator.width + cbPlannerEnable.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                RowLayout {
                    spacing: 10
                    
                    Text {
                        text: "Stop nach (Min):"
                        color: "white"
                        Layout.preferredWidth: 120
                    }
                    
                    TextField {
                        id: txtStopAfterMinutes
                        text: "0"
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
                
                CheckBox {
                    id: cbAutoStart
                    text: "Programm automatisch starten beim App-Start"
                    
                    contentItem: Text {
                        text: cbAutoStart.text
                        color: "white"
                        leftPadding: cbAutoStart.indicator.width + cbAutoStart.spacing
                        verticalAlignment: Text.AlignVCenter
                        wrapMode: Text.WordWrap
                    }
                    Layout.fillWidth: true
                }
                
                Rectangle {
                    Layout.fillWidth: true
                    height: 1
                    color: "#444444"
                }
                
                // UI & Behavior Settings
                Text {
                    text: "Aussehen & Verhalten (MoveMouse / UI)"
                    font.bold: true
                    color: "#FFD700"
                    font.pixelSize: 14
                }
                
                CheckBox {
                    id: cbHideMoveMouseWindow
                    text: "Hide Move Mouse window"
                    
                    contentItem: Text {
                        text: cbHideMoveMouseWindow.text
                        color: "white"
                        leftPadding: cbHideMoveMouseWindow.indicator.width + cbHideMoveMouseWindow.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                CheckBox {
                    id: cbTopmostWhenRunning
                    text: "Show topmost when running"
                    
                    contentItem: Text {
                        text: cbTopmostWhenRunning.text
                        color: "white"
                        leftPadding: cbTopmostWhenRunning.indicator.width + cbTopmostWhenRunning.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                CheckBox {
                    id: cbMinimiseWhenNotRunning
                    text: "Minimise when not running"
                    
                    contentItem: Text {
                        text: cbMinimiseWhenNotRunning.text
                        color: "white"
                        leftPadding: cbMinimiseWhenNotRunning.indicator.width + cbMinimiseWhenNotRunning.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                CheckBox {
                    id: cbHideFromTaskbar
                    text: "Hide from taskbar"
                    
                    contentItem: Text {
                        text: cbHideFromTaskbar.text
                        color: "white"
                        leftPadding: cbHideFromTaskbar.indicator.width + cbHideFromTaskbar.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                CheckBox {
                    id: cbHideFromAltTab
                    text: "Hide from Alt-Tab"
                    
                    contentItem: Text {
                        text: cbHideFromAltTab.text
                        color: "white"
                        leftPadding: cbHideFromAltTab.indicator.width + cbHideFromAltTab.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                RowLayout {
                    spacing: 10
                    
                    Text {
                        text: "Override window title:"
                        color: "white"
                        Layout.preferredWidth: 140
                    }
                    
                    TextField {
                        id: txtOverrideTitle
                        Layout.fillWidth: true
                        
                        background: Rectangle {
                            color: "#333333"
                            border.color: "#555555"
                            border.width: 1
                        }
                        
                        color: "white"
                    }
                }
                
                Rectangle {
                    Layout.fillWidth: true
                    height: 1
                    color: "#444444"
                }
                
                // Behavior Options
                Text {
                    text: "Verhalten"
                    font.bold: true
                    color: "#FFD700"
                    font.pixelSize: 14
                }
                
                RowLayout {
                    spacing: 6
                    
                    CheckBox {
                        id: cbRepeatEnabled
                        text: "Repeat actions every"
                        
                        contentItem: Text {
                            text: cbRepeatEnabled.text
                            color: "white"
                            leftPadding: cbRepeatEnabled.indicator.width + cbRepeatEnabled.spacing
                            verticalAlignment: Text.AlignVCenter
                        }
                    }
                    
                    TextField {
                        id: txtRepeatInterval
                        text: "30"
                        Layout.preferredWidth: 50
                        horizontalAlignment: TextInput.AlignHCenter
                        
                        background: Rectangle {
                            color: "#333333"
                            border.color: "#555555"
                            border.width: 1
                        }
                        
                        color: "white"
                    }
                    
                    ComboBox {
                        id: cbRepeatUnit
                        Layout.preferredWidth: 90
                        model: ["Seconds", "Minutes"]
                        currentIndex: 0
                        
                        background: Rectangle {
                            color: "#333333"
                            border.color: "#555555"
                            border.width: 1
                        }
                        
                        contentItem: Text {
                            text: cbRepeatUnit.displayText
                            color: "white"
                            verticalAlignment: Text.AlignVCenter
                            leftPadding: 8
                        }
                    }
                }
                
                CheckBox {
                    id: cbAutoStopOnUserActivity
                    text: "Automatically stop when user activity detected"
                    
                    contentItem: Text {
                        text: cbAutoStopOnUserActivity.text
                        color: "white"
                        leftPadding: cbAutoStopOnUserActivity.indicator.width + cbAutoStopOnUserActivity.spacing
                        verticalAlignment: Text.AlignVCenter
                        wrapMode: Text.WordWrap
                    }
                    Layout.fillWidth: true
                }
                
                CheckBox {
                    id: cbLaunchMoveMouseAtStartup
                    text: "Launch Move Mouse at start-up"
                    
                    contentItem: Text {
                        text: cbLaunchMoveMouseAtStartup.text
                        color: "white"
                        leftPadding: cbLaunchMoveMouseAtStartup.indicator.width + cbLaunchMoveMouseAtStartup.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                CheckBox {
                    id: cbStartActionsWhenMoveLaunched
                    text: "Start actions when Move Mouse is launched"
                    
                    contentItem: Text {
                        text: cbStartActionsWhenMoveLaunched.text
                        color: "white"
                        leftPadding: cbStartActionsWhenMoveLaunched.indicator.width + cbStartActionsWhenMoveLaunched.spacing
                        verticalAlignment: Text.AlignVCenter
                        wrapMode: Text.WordWrap
                    }
                    Layout.fillWidth: true
                }
                
                RowLayout {
                    spacing: 6
                    
                    CheckBox {
                        id: cbAdjustVolume
                        text: "Adjust volume when Move Mouse is running"
                        
                        contentItem: Text {
                            text: cbAdjustVolume.text
                            color: "white"
                            leftPadding: cbAdjustVolume.indicator.width + cbAdjustVolume.spacing
                            verticalAlignment: Text.AlignVCenter
                            wrapMode: Text.WordWrap
                        }
                    }
                    
                    Text {
                        text: "%:"
                        color: "white"
                    }
                    
                    TextField {
                        id: txtAdjustVolumePercent
                        text: "80"
                        Layout.preferredWidth: 50
                        horizontalAlignment: TextInput.AlignHCenter
                        
                        background: Rectangle {
                            color: "#333333"
                            border.color: "#555555"
                            border.width: 1
                        }
                        
                        color: "white"
                    }
                }
                
                CheckBox {
                    id: cbContinueWhenLocked
                    text: "Continue performing actions when session is locked"
                    
                    contentItem: Text {
                        text: cbContinueWhenLocked.text
                        color: "white"
                        leftPadding: cbContinueWhenLocked.indicator.width + cbContinueWhenLocked.spacing
                        verticalAlignment: Text.AlignVCenter
                        wrapMode: Text.WordWrap
                    }
                    Layout.fillWidth: true
                }
                
                CheckBox {
                    id: cbPauseOnBattery
                    text: "Pause when running on battery"
                    
                    contentItem: Text {
                        text: cbPauseOnBattery.text
                        color: "white"
                        leftPadding: cbPauseOnBattery.indicator.width + cbPauseOnBattery.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                CheckBox {
                    id: cbEnableFileLogging
                    text: "Enable file logging"
                    
                    contentItem: Text {
                        text: cbEnableFileLogging.text
                        color: "white"
                        leftPadding: cbEnableFileLogging.indicator.width + cbEnableFileLogging.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                CheckBox {
                    id: cbScreenBurnPrevention
                    text: "Enable screen burn prevention"
                    
                    contentItem: Text {
                        text: cbScreenBurnPrevention.text
                        color: "white"
                        leftPadding: cbScreenBurnPrevention.indicator.width + cbScreenBurnPrevention.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                CheckBox {
                    id: cbHideTrayIcon
                    text: "Hide system tray icon"
                    
                    contentItem: Text {
                        text: cbHideTrayIcon.text
                        color: "white"
                        leftPadding: cbHideTrayIcon.indicator.width + cbHideTrayIcon.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                CheckBox {
                    id: cbTrayNotifications
                    text: "Show tray notifications"
                    
                    contentItem: Text {
                        text: cbTrayNotifications.text
                        color: "white"
                        leftPadding: cbTrayNotifications.indicator.width + cbTrayNotifications.spacing
                        verticalAlignment: Text.AlignVCenter
                    }
                }
                
                Rectangle {
                    Layout.fillWidth: true
                    height: 1
                    color: "#444444"
                }
                
                // Scheduler section
                Text {
                    text: "Scheduler"
                    font.bold: true
                    color: "#FFD700"
                    font.pixelSize: 14
                }
                
                Button {
                    text: "Scheduler bearbeiten..."
                    Layout.fillWidth: true
                    
                    background: Rectangle {
                        color: parent.pressed ? "#1976D2" : "#2196F3"
                        radius: 4
                    }
                    
                    contentItem: Text {
                        text: parent.parent.text
                        color: "white"
                        horizontalAlignment: Text.AlignHCenter
                        verticalAlignment: Text.AlignVCenter
                    }
                    
                    onClicked: {
                        // TODO: Open scheduler editor
                    }
                }
                
                Item {
                    Layout.preferredHeight: 20
                }
            }
        }
        
        // Bottom buttons
        RowLayout {
            Layout.fillWidth: true
            spacing: 10
            
            Button {
                text: "OK"
                Layout.fillWidth: true
                
                background: Rectangle {
                    color: parent.pressed ? "#3d8b40" : "#4CAF50"
                    radius: 4
                }
                
                contentItem: Text {
                    text: parent.parent.text
                    color: "white"
                    font.bold: true
                    horizontalAlignment: Text.AlignHCenter
                    verticalAlignment: Text.AlignVCenter
                }
                
                onClicked: {
                    // TODO: Save settings via controller
                    advancedSettingsWindow.close()
                }
            }
            
            Button {
                text: "Abbrechen"
                Layout.fillWidth: true
                
                background: Rectangle {
                    color: parent.pressed ? "#c23a2e" : "#F44336"
                    radius: 4
                }
                
                contentItem: Text {
                    text: parent.parent.text
                    color: "white"
                    font.bold: true
                    horizontalAlignment: Text.AlignHCenter
                    verticalAlignment: Text.AlignVCenter
                }
                
                onClicked: advancedSettingsWindow.close()
            }
        }
    }
}
