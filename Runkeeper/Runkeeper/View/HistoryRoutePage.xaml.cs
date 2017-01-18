using System;
using Runkeeper.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Runkeeper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HistoryRoutePage : Page, INotifyPropertyChanged
    {
        public HistoryRoutePage()
        {
            InitializeComponent();
            DataContext = App.instance.transfer.data;
        }

        private void SortDistance_OnClick(object sender, RoutedEventArgs e)
        {
            //order data
            var order = from route in App.instance.transfer.data.routeHistory
                        orderby route.totalDistance descending
                        select route;

            //set ordered data
            App.instance.transfer.data.routeHistory = new ObservableCollection<Route>(order);
            NotifyPropertyChanged(nameof(App.instance.transfer.data.routeHistory));
            data.ItemsSource = App.instance.transfer.data.routeHistory;
            data.DataContext = App.instance.transfer.data;
        }

        private void SortDate_OnClick(object sender, RoutedEventArgs e)
        {
            //order data
            var order = from route in App.instance.transfer.data.routeHistory
                        orderby route.date ascending
                        select route;

            //set ordered data
            App.instance.transfer.data.routeHistory = new ObservableCollection<Route>(order);
            NotifyPropertyChanged(nameof(App.instance.transfer.data.routeHistory));
            data.ItemsSource = App.instance.transfer.data.routeHistory;
            data.DataContext = App.instance.transfer.data;
        }

        private void SortName_OnClick(object sender, RoutedEventArgs e)
        {
            //order data
            var order = from route in App.instance.transfer.data.routeHistory
                        orderby route.name ascending
                        select route;

            //set ordered data
            App.instance.transfer.data.routeHistory = new ObservableCollection<Route>(order);
            NotifyPropertyChanged(nameof(App.instance.transfer.data.routeHistory));
            data.ItemsSource = App.instance.transfer.data.routeHistory;
            data.DataContext = App.instance.transfer.data;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Data_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data.SelectedIndex != -1 && data.SelectedItem != null)
            {

                Route r = App.instance.transfer.data.routeHistory.ElementAt(data.SelectedIndex);
                double speed = 0;
                foreach (DataStamp d in r.route)
                {
                    speed += d.speed;
                }
                speed = speed / r.route.Count;


                ContentDialog dialog = new ContentDialog();
                dialog.Title = r.name;
                dialog.Content = "Date : " + r.date + "\n" + "Distance: " + r.totalDistance + " m" + "\n" + "Velocity: " + Math.Round(speed,1) +"km/h";
                dialog.PrimaryButtonText = "Close";
                dialog.SecondaryButtonText = "Draw this route";

                if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                {
                }
                else
                {   
                    
                    //todo draw route

                }
            }
        }
    }
}
