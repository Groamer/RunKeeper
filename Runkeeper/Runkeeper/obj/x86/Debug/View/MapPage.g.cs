﻿#pragma checksum "C:\Users\Tom Remeeus\Documents\GitHub\RunKeeper\Runkeeper\Runkeeper\View\MapPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DB15C2D46FA14CDE29BA8D82A03C980B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Runkeeper
{
    partial class MapPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.MainGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 2:
                {
                    this.MapControl1 = (global::Windows.UI.Xaml.Controls.Maps.MapControl)(target);
                }
                break;
            case 3:
                {
                    this.Popup1 = (global::Windows.UI.Xaml.Controls.Primitives.Popup)(target);
                }
                break;
            case 4:
                {
                    this.TextPop = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5:
                {
                    this.popButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 85 "..\..\..\View\MapPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.popButton).Click += this.PopButton_OnClick;
                    #line default
                }
                break;
            case 6:
                {
                    this.Afstand = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 7:
                {
                    this.Timetext = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8:
                {
                    this.StartRunning = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 57 "..\..\..\View\MapPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.StartRunning).Click += this.StartRunning_Click;
                    #line default
                }
                break;
            case 9:
                {
                    this.Afstandtext = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10:
                {
                    this.Velocitytext = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 11:
                {
                    this.Time = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 12:
                {
                    this.Velocity = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 13:
                {
                    this.Stopbutton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 62 "..\..\..\View\MapPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Stopbutton).Click += this.Stopbutton_Click;
                    #line default
                }
                break;
            case 14:
                {
                    this.Timetext_Copy = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 15:
                {
                    this.Timetext_Copy1 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

