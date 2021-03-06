﻿using System;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Runkeeper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            InitializeComponent();
            Frame.Navigated += Frame_Navigated;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            Frame.Navigate(typeof(MapPage), new Tuple<string,string,string>("mainpage",null,null));
            RouteScreen.IsSelected = true;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ?
              AppViewBackButtonVisibility.Visible :
              AppViewBackButtonVisibility.Collapsed;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            string pagename = e.SourcePageType.ToString().Split('.').Last();

            RunList.SelectedIndex = -1;

            switch (pagename.ToLower())
            {
                default:
                    PageTitle.Text = "Map";
                    break;
                case "MapPage":
                    RunList.SelectedIndex = 0;
                    break;
                case "HistoryRoutePage":
                    RunList.SelectedIndex = 3;
                    break;
            }

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                Frame.CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            RunView.IsPaneOpen = !RunView.IsPaneOpen;
        }

        private void RunList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                RunView.IsPaneOpen = false;
                if (RouteScreen.IsSelected)
                {
                    if (Frame != null)
                    {
                        if (Frame.CanGoBack)
                            Frame.BackStack.Clear();
                        Frame.Navigate(typeof(MapPage), new Tuple<string, string, string>("mainpage", null, null));
                        PageTitle.Text = "Map";
                    }
                }

                else if (historische.IsSelected)
                {
                    Frame.Navigate(typeof(HistoryRoutePage));
                    PageTitle.Text = "Track History";
                }
               
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void MySplitviewPane_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.Cumulative.Translation.X < -50)
            {
                RunView.IsPaneOpen = false;
            }
        }

    
        private void Grid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.Cumulative.Translation.X > 20)
            {
                RunView.IsPaneOpen = true;
            }
        }

        private void RouteScreen_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            RunView.IsPaneOpen = false;
        }
    }
}
