using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;
using Windows.Services.Maps;
using Runkeeper.Model;
using Windows.Devices.Geolocation.Geofencing;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Runkeeper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        private MapHelper maphelper = new MapHelper();
        private MapPolyline oldline;
        private Geolocator geolocator;
        private Boolean isRunning;
        public static MapPage instance;

        public MapPage()
        {
            instance = this;
            InitializeComponent();

            Stopbutton.IsEnabled = false;

            if(App.instance.transfer.data.currentposition != null && App.instance.transfer.data.currentRoute != null)
            {
                MapControl1.Center = App.instance.transfer.data.currentposition.Location;
                MapControl1.ZoomLevel = 100;
                UpdateRouteHistory(App.instance.transfer.data.currentposition.Location);
            }
            if(!App.instance.transfer.data.startApp)
            {
                startTracking();
            }
            this.NavigationCacheMode = NavigationCacheMode.Disabled;
            Velocity.DataContext = App.instance.transfer.data;
            Time.DataContext = App.instance.transfer.data.time;
            Afstand.DataContext = App.instance.transfer.data;

        }

        public async Task<Geoposition> GetPosition()
        {
            var accesStatus = await Geolocator.RequestAccessAsync();

            if (accesStatus != GeolocationAccessStatus.Allowed)
            {
                var succes = await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
            }

            GeofenceMonitor.Current.GeofenceStateChanged += Current_GeofenceStateChanged;
            //GeofenceMonitor.Current.StatusChanged += Current_StatusChanged;
  
            foreach (Route route in App.instance.transfer.data.routeHistory)
            {
                for (int i = 0; i < route.route.Count; i++)
                {
                    Geofence geofence = GeofenceHandler.createGeofence(route.route[i].location);
                    List<Geofence> geofences = GeofenceMonitor.Current.Geofences.ToList();
                    bool equal = false;
                    foreach (Geofence something in geofences)
                    {
                        if (something.Id == geofence.Id)
                        {
                            equal = true;
                        }
                    }
                    if(!equal)
                    {
                        GeofenceMonitor.Current.Geofences.Add(geofence);
                    }
                }
            }
            return await startLocating();
        }

        public async Task<Geoposition> startLocating()
        {
            App.instance.transfer.data.startApp = false;
            geolocator = new Geolocator { DesiredAccuracyInMeters = 0, MovementThreshold = 1 };
            geolocator.PositionChanged += Geolocator_PositionChanged;
            var position = await this.geolocator.GetGeopositionAsync();
            return position;
        }

        public void StopLocating()
        {
            App.instance.transfer.data.startApp = true;
            if(geolocator != null)
            geolocator.PositionChanged -= Geolocator_PositionChanged;
            geolocator = null;
            
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            GeofenceMonitor.Current.GeofenceStateChanged -= Current_GeofenceStateChanged;

            base.OnNavigatingFrom(e);
        }

        private async void Current_GeofenceStateChanged(GeofenceMonitor sender, object args)
        {
            if (sender.Geofences.Any())
            {
                var reports = sender.ReadReports();

                foreach (var report in reports)
                {
                    switch (report.NewState)
                    {
                        case GeofenceState.Entered:
                        {
                            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                    MainGrid.Opacity = 0.10;
                                    Popup1.IsOpen = true;
                            });
                                break;
                        }
                        case GeofenceState.Exited:
                            {
                                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                {
                                    MainGrid.Opacity = 1.0;
                                    Popup1.IsOpen = false;
                                });
                                break;
                            }
                    }
                }
            }
        }

        private async void startTracking()
        {
            MapControl1.ZoomLevel = 12;
            Geoposition x = await GetPosition();
            App.instance.transfer.data.startposition = x.Coordinate.Point;
        }

        private async void Geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                Geoposition x = await maphelper.CurrentLocation(args.Position);
                UpdateRouteHistory(x.Coordinate.Point);
            });
        }


        private void UpdateRouteHistory(Geopoint point)
        {
            if (App.instance.transfer.data.zoomCenter)
            {
                MapControl1.Center = point;
            }
            
            if (App.instance.transfer.data.currentRoute.route.Count >= 2 && isRunning)
            {
                if(App.instance.transfer.data.calculatedRoute != null)
                {
                    MapControl1.MapElements.Add(App.instance.transfer.data.calculatedRoute);
                }
                MapControl1.MapElements.Add(maphelper.DrawRoute());
            }

            if (App.instance.transfer.data.currentposition != null && !MapControl1.MapElements.Contains(App.instance.transfer.data.currentposition))
            {
                MapControl1.MapElements.Add(App.instance.transfer.data.currentposition);
            }
        }

        

        public async void FromToRoute(string from, string to)
        {
            if (isRunning)
            {
                App.instance.transfer.data.from = from;
                App.instance.transfer.data.to = to;

                MapLocationFinderResult result = await MapLocationFinder.FindLocationsAsync(from, MapControl1.Center);
                MapLocation from1 = result.Locations.First();
                MapControl1.Center = from1.Point;

                result = await MapLocationFinder.FindLocationsAsync(to, MapControl1.Center);

                maphelper.GenerateCalculatedRoute(result, from1);
                UpdateRouteHistory(App.instance.transfer.data.currentposition.Location);
            }
        }

        private async void StartRunning_Click(object sender, RoutedEventArgs e)
        {
            isRunning = true;
            StartRunning.IsEnabled = false;
            Stopbutton.IsEnabled = true;

            MapControl1.MapElements.Clear();
            App.instance.transfer.data.time.Start();
            Geoposition x = await MapPage.instance.startLocating();
            Afstand.Text = "0";
            Velocity.Text = "0";

            Velocity.DataContext = App.instance.transfer.data;
            Time.DataContext = App.instance.transfer.data.time;
            Afstand.DataContext = App.instance.transfer.data;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GeofenceMonitor.Current.GeofenceStateChanged += Current_GeofenceStateChanged;
            //GeofenceMonitor.Current.StatusChanged += Current_StatusChanged;
            IList<Geofence> list = GeofenceMonitor.Current.Geofences;
            List<Geofence> geofences = list.ToList();
            base.OnNavigatedTo(e);
            var value = (Tuple<string, string,string>)e.Parameter;
            if(value.Item1.Equals("createroute"))
            {
                FromToRoute(value.Item2, value.Item3);
            }
            MapControl1.MapElements.Clear();
        }

        private void PopButton_OnClick(object sender, RoutedEventArgs e)
        {
            MainGrid.Opacity = 1.0;
            Popup1.IsOpen = false;
        }

        private async void Stopbutton_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                //show message
                MessageDialog alert = new MessageDialog("Are you sure you want to stop your workout?");
                alert.Commands.Add(new UICommand("YES") { Id = 0 });
                alert.Commands.Add(new UICommand("NO") { Id = 1 });
                var result = await alert.ShowAsync();

                if ((int)result.Id == 0)
                {
                    //STOP TRACKING
                    isRunning = false;
                    StartRunning.IsEnabled = true;
                    Stopbutton.IsEnabled = false;
                    App.instance.transfer.data.time.Stop();
                    StopLocating();
                    MapControl1.MapElements.Clear();

                    //GEEF NAAM AAN ROUTE MEE
                    TextBox input = new TextBox();
                    input.AcceptsReturn = false;

                    ContentDialog setName = new ContentDialog();
                    setName.Content = input;
                    setName.Title = "Enter a name for this workout...";
                    setName.IsSecondaryButtonEnabled = true;
                    setName.PrimaryButtonText = "DISCARD WORKOUT";
                    setName.SecondaryButtonText = "SAVE WORKOUT";

                    if (await setName.ShowAsync() == ContentDialogResult.Secondary)
                    {
                        App.instance.transfer.data.name = input.Text;
                        App.instance.transfer.data.SaveData();
                    }
                }
            }
        }
    }
}
