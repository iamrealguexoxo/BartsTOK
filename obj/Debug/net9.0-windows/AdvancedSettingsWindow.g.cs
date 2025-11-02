#pragma checksum "..\..\..\AdvancedSettingsWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "91584664722DFE09DAFCF62D5C8C2AC20479E9C1"
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace BartsTOK {
    
    
    /// <summary>
    /// AdvancedSettingsWindow
    /// </summary>
    public partial class AdvancedSettingsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbPlannerEnable;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtStopAfterMinutes;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbAutoStart;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbHideMoveMouseWindow;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbTopmostWhenRunning;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbMinimiseWhenNotRunning;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbHideFromTaskbar;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbHideFromAltTab;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtOverrideTitle;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbRepeatEnabled;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtRepeatInterval;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbRepeatUnit;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbAutoStopOnUserActivity;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbLaunchMoveMouseAtStartup;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbStartActionsWhenMoveLaunched;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbAdjustVolume;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtAdjustVolumePercent;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbContinueWhenLocked;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbPauseOnBattery;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbEnableFileLogging;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbScreenBurnPrevention;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbHideTrayIcon;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbTrayNotifications;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbShowStatusOnMain;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbDisableButtonAnimations;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOk;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lstSchedules;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddSchedule;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnEditSchedule;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\AdvancedSettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRemoveSchedule;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.10.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Barts Tok;component/advancedsettingswindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\AdvancedSettingsWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.10.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.cbPlannerEnable = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 2:
            this.txtStopAfterMinutes = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.cbAutoStart = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 4:
            this.cbHideMoveMouseWindow = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 5:
            this.cbTopmostWhenRunning = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 6:
            this.cbMinimiseWhenNotRunning = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 7:
            this.cbHideFromTaskbar = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 8:
            this.cbHideFromAltTab = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 9:
            this.txtOverrideTitle = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.cbRepeatEnabled = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 11:
            this.txtRepeatInterval = ((System.Windows.Controls.TextBox)(target));
            return;
            case 12:
            this.cbRepeatUnit = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 13:
            this.cbAutoStopOnUserActivity = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 14:
            this.cbLaunchMoveMouseAtStartup = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 15:
            this.cbStartActionsWhenMoveLaunched = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 16:
            this.cbAdjustVolume = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 17:
            this.txtAdjustVolumePercent = ((System.Windows.Controls.TextBox)(target));
            return;
            case 18:
            this.cbContinueWhenLocked = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 19:
            this.cbPauseOnBattery = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 20:
            this.cbEnableFileLogging = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 21:
            this.cbScreenBurnPrevention = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 22:
            this.cbHideTrayIcon = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 23:
            this.cbTrayNotifications = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 24:
            this.cbShowStatusOnMain = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 25:
            this.cbDisableButtonAnimations = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 26:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 57 "..\..\..\AdvancedSettingsWindow.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.BtnCancel_Click);
            
            #line default
            #line hidden
            return;
            case 27:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            
            #line 58 "..\..\..\AdvancedSettingsWindow.xaml"
            this.btnOk.Click += new System.Windows.RoutedEventHandler(this.BtnOk_Click);
            
            #line default
            #line hidden
            return;
            case 28:
            this.lstSchedules = ((System.Windows.Controls.ListBox)(target));
            return;
            case 29:
            this.btnAddSchedule = ((System.Windows.Controls.Button)(target));
            
            #line 65 "..\..\..\AdvancedSettingsWindow.xaml"
            this.btnAddSchedule.Click += new System.Windows.RoutedEventHandler(this.BtnAddSchedule_Click);
            
            #line default
            #line hidden
            return;
            case 30:
            this.btnEditSchedule = ((System.Windows.Controls.Button)(target));
            
            #line 66 "..\..\..\AdvancedSettingsWindow.xaml"
            this.btnEditSchedule.Click += new System.Windows.RoutedEventHandler(this.BtnEditSchedule_Click);
            
            #line default
            #line hidden
            return;
            case 31:
            this.btnRemoveSchedule = ((System.Windows.Controls.Button)(target));
            
            #line 67 "..\..\..\AdvancedSettingsWindow.xaml"
            this.btnRemoveSchedule.Click += new System.Windows.RoutedEventHandler(this.BtnRemoveSchedule_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

