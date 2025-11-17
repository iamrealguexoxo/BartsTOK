import QtQuick 2.15
import QtQuick.Controls 2.15
import QtQuick.Layouts 1.15
import QtQuick.Window 2.15

Window {
    id: aboutWindow
    width: 500
    height: 600
    title: "Über BartsTOK"
    modality: Qt.ApplicationModal
    
    color: "#1e1e1e"
    
    ColumnLayout {
        anchors.fill: parent
        anchors.margins: 20
        spacing: 20
        
        // Logo placeholder
        Rectangle {
            Layout.alignment: Qt.AlignHCenter
            width: 96
            height: 96
            color: "#FFD700"
            radius: 48
            
            Text {
                anchors.centerIn: parent
                text: "🎬"
                font.pixelSize: 48
            }
        }
        
        // Title
        Text {
            Layout.alignment: Qt.AlignHCenter
            text: "BartsTOK"
            font.pixelSize: 28
            font.bold: true
            color: "#FFD700"
        }
        
        // Version
        Text {
            Layout.alignment: Qt.AlignHCenter
            text: "Version 1.0.0-alpha (Python/QML)"
            font.pixelSize: 14
            color: "white"
        }
        
        // Description
        Text {
            Layout.fillWidth: true
            text: "BartsTOK ist eine Anwendung, die deinen PC wach hält, Text automatisch eingibt, die Maus bewegt und Bildschirm-Brand verhindert."
            font.pixelSize: 12
            color: "white"
            wrapMode: Text.WordWrap
            horizontalAlignment: Text.AlignHCenter
        }
        
        Rectangle {
            Layout.fillWidth: true
            height: 1
            color: "#FFD700"
        }
        
        // Features
        Text {
            text: "Features:"
            font.pixelSize: 14
            font.bold: true
            color: "#FFD700"
        }
        
        ScrollView {
            Layout.fillWidth: true
            Layout.fillHeight: true
            
            ColumnLayout {
                width: parent.width
                spacing: 8
                
                Text {
                    text: "• Automatische Texteingabe"
                    color: "white"
                    font.pixelSize: 12
                }
                Text {
                    text: "• Verschiedene Tastatur-Eingabemethoden"
                    color: "white"
                    font.pixelSize: 12
                }
                Text {
                    text: "• Mausbewegung (MoveBart)"
                    color: "white"
                    font.pixelSize: 12
                }
                Text {
                    text: "• Planer und Scheduler"
                    color: "white"
                    font.pixelSize: 12
                }
                Text {
                    text: "• Bildschirm-Burn-Prevention"
                    color: "white"
                    font.pixelSize: 12
                }
                Text {
                    text: "• System-Tray-Integration"
                    color: "white"
                    font.pixelSize: 12
                }
            }
        }
        
        Rectangle {
            Layout.fillWidth: true
            height: 1
            color: "#FFD700"
        }
        
        // Credits
        Text {
            text: "Entwickelt mit:"
            font.pixelSize: 14
            font.bold: true
            color: "#FFD700"
        }
        
        ColumnLayout {
            Layout.fillWidth: true
            spacing: 4
            
            Text {
                text: "• Python 3.8+"
                color: "white"
                font.pixelSize: 12
            }
            Text {
                text: "• PySide6 (Qt 6)"
                color: "white"
                font.pixelSize: 12
            }
            Text {
                text: "• Qt Quick/QML"
                color: "white"
                font.pixelSize: 12
            }
        }
        
        Rectangle {
            Layout.fillWidth: true
            height: 1
            color: "#FFD700"
        }
        
        // License
        Text {
            Layout.fillWidth: true
            text: "Lizenziert unter der MIT-Lizenz"
            font.pixelSize: 11
            color: "#888888"
            horizontalAlignment: Text.AlignHCenter
        }
        
        // Close button
        Button {
            Layout.alignment: Qt.AlignHCenter
            Layout.preferredWidth: 120
            text: "Schließen"
            
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
            
            onClicked: aboutWindow.close()
        }
    }
}
