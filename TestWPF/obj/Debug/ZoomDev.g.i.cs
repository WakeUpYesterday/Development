﻿#pragma checksum "..\..\ZoomDev.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "75FCB808F3309338C91772762BF1D008"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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
using TestWPF;
using ZoomAndPan;


namespace TestWPF {
    
    
    /// <summary>
    /// ZoomDev
    /// </summary>
    public partial class ZoomDev : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\ZoomDev.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scroller;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\ZoomDev.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ZoomAndPan.ZoomAndPanControl zoomAndPanControl;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\ZoomDev.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas content;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\ZoomDev.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Start;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\ZoomDev.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Zoommo;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TestWPF;component/zoomdev.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ZoomDev.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.scroller = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 2:
            this.zoomAndPanControl = ((ZoomAndPan.ZoomAndPanControl)(target));
            
            #line 27 "..\..\ZoomDev.xaml"
            this.zoomAndPanControl.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.zoomAndPanControl_MouseDown);
            
            #line default
            #line hidden
            
            #line 28 "..\..\ZoomDev.xaml"
            this.zoomAndPanControl.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.zoomAndPanControl_MouseUp);
            
            #line default
            #line hidden
            
            #line 29 "..\..\ZoomDev.xaml"
            this.zoomAndPanControl.MouseMove += new System.Windows.Input.MouseEventHandler(this.zoomAndPanControl_MouseMove);
            
            #line default
            #line hidden
            
            #line 30 "..\..\ZoomDev.xaml"
            this.zoomAndPanControl.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(this.zoomAndPanControl_MouseWheel);
            
            #line default
            #line hidden
            return;
            case 3:
            this.content = ((System.Windows.Controls.Canvas)(target));
            return;
            case 4:
            this.Start = ((System.Windows.Controls.Button)(target));
            
            #line 56 "..\..\ZoomDev.xaml"
            this.Start.Click += new System.Windows.RoutedEventHandler(this.Start_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Zoommo = ((System.Windows.Controls.Button)(target));
            
            #line 59 "..\..\ZoomDev.xaml"
            this.Zoommo.Click += new System.Windows.RoutedEventHandler(this.Zoom_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

