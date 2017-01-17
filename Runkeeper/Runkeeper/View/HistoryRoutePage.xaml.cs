using Runkeeper.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
                        orderby route.name descending
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
    }
}
