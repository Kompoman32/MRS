﻿#pragma checksum "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B49F0256BDAC3878B9140ABB65D2E508DF59EE04"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using CourseWork;
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


namespace CourseWork {
    
    
    /// <summary>
    /// AdminAddDocuments
    /// </summary>
    public partial class AdminAddDocuments : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbDocuments;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bNewDoc;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbTitle;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbDiscription;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpSignDate;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bDelDoc;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bSaveDoc;
        
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
            System.Uri resourceLocater = new System.Uri("/CourseWork;component/windows/admin/adminadddocuments.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
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
            
            #line 9 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
            ((CourseWork.AdminAddDocuments)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.AdminAddDocuments_OnClosing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.lbDocuments = ((System.Windows.Controls.ListBox)(target));
            
            #line 17 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
            this.lbDocuments.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lbDocuments_OnSelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.bNewDoc = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
            this.bNewDoc.Click += new System.Windows.RoutedEventHandler(this.bNewDoc_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.tbTitle = ((System.Windows.Controls.TextBox)(target));
            
            #line 22 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
            this.tbTitle.KeyDown += new System.Windows.Input.KeyEventHandler(this.TexBox_OnKeyDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.tbDiscription = ((System.Windows.Controls.TextBox)(target));
            
            #line 24 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
            this.tbDiscription.KeyDown += new System.Windows.Input.KeyEventHandler(this.TexBox_OnKeyDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.dpSignDate = ((System.Windows.Controls.DatePicker)(target));
            
            #line 27 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
            this.dpSignDate.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.dpSignDate_OnSelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.bDelDoc = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
            this.bDelDoc.Click += new System.Windows.RoutedEventHandler(this.bDelDoc_OnClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.bSaveDoc = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\..\Windows\Admin\AdminAddDocuments.xaml"
            this.bSaveDoc.Click += new System.Windows.RoutedEventHandler(this.bSaveDoc_OnClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
