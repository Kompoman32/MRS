﻿#pragma checksum "..\..\..\..\Windows\Admin\AdWindowDelMeterTabPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "D7B39CC00854B61BE19454EE5BF6F6D36475D87D"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using CourseWork.Windows.Admin;
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


namespace CourseWork.Windows.Admin {
    
    
    /// <summary>
    /// AddWindowDelMeterTabPage
    /// </summary>
    public partial class AddWindowDelMeterTabPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\..\Windows\Admin\AdWindowDelMeterTabPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbUsersDM;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\Windows\Admin\AdWindowDelMeterTabPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbMetersDM;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\Windows\Admin\AdWindowDelMeterTabPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame frMeterInfoDM;
        
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
            System.Uri resourceLocater = new System.Uri("/CourseWork;component/windows/admin/adwindowdelmetertabpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\Admin\AdWindowDelMeterTabPage.xaml"
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
            this.cbUsersDM = ((System.Windows.Controls.ComboBox)(target));
            
            #line 14 "..\..\..\..\Windows\Admin\AdWindowDelMeterTabPage.xaml"
            this.cbUsersDM.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cbUsersDM_OnSelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.cbMetersDM = ((System.Windows.Controls.ComboBox)(target));
            
            #line 16 "..\..\..\..\Windows\Admin\AdWindowDelMeterTabPage.xaml"
            this.cbMetersDM.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cbMetersDM_OnSelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.frMeterInfoDM = ((System.Windows.Controls.Frame)(target));
            return;
            case 4:
            
            #line 23 "..\..\..\..\Windows\Admin\AdWindowDelMeterTabPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.bDeleteDM_OnClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
