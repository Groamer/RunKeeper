using Runkeeper.Model;
using Runkeeper.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.Storage;
using Windows.UI.Xaml.Controls.Maps;
using Microsoft.ApplicationInsights.DataContracts;

namespace Runkeeper
{
    public class DataHandler : INotifyPropertyChanged
    {
        public Route currentRoute = new Route(null, DateTime.Now, new ObservableCollection<DataStamp>(), 0);
        public ObservableCollection<Route> routeHistory { get; set; }
        public MapIcon currentposition;
        public MapPolyline calculatedRoute;
        public Geopoint startposition;
        public string from, to;
        public string name { get; set; }
        public string currentDistance { get; set; }
        public string currentSpeed { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public bool zoomCenter = true, drawOld = true;
        public bool startApp = true;
        public Time time = new Time();

        public DataHandler()
        {
            routeHistory = new ObservableCollection<Route>();
            currentDistance = "0";
            currentSpeed = "0";
        }

        public void SaveData()
        {
            //ROUTE PROTOCOL - "route" | ROUTE NAME | TOTAL DISTANCE | COMPLETION DATE
            //ROUTE DETAILS PROTOCOL - LATITUDE | LONGITUDE | CURRENT TIME | CURRENT SPEED | CURRENT TOTAL DISTANCE

            //set current values to current route
            currentRoute.totalDistance = double.Parse(currentDistance);
            currentRoute.name = name;

            //add currentroute to history list
            routeHistory.Add(currentRoute);

            //reset current distance to correct mapview values
            currentDistance = "0";

            //Set current route to null to correct mapview
            currentRoute = new Route(name, DateTime.Now, new ObservableCollection<DataStamp>(), 0);

            List<string> list = new List<string>();

            //loop all routes
            for (int v = 0; v < routeHistory.Count; v ++)
            {
                //add route to list
                list.Add("route" + "|" + routeHistory[v].name + "|" + routeHistory[v].totalDistance + "|" + routeHistory[v].date.ToString());

                //loop all details
                for (int i = 0; i < routeHistory[v].route.Count; i ++)
                {
                    //add details to routehistory
                    list.Add(routeHistory[v].route[i].location.Position.Latitude + "|" + routeHistory[v].route[i].location.Position.Longitude + "|"
                    + routeHistory[v].route[i].time.ToString() + "|" + routeHistory[v].route[i].speed + "|" + routeHistory[v].route[i].distance);
                }
            }

            //save data to file
            File.WriteAllLines(ApplicationData.Current.LocalFolder.Path + "//RouteList.txt", list);
        }

        public void DiscardData()
        {
            //Set current route to null to correct mapview
            currentRoute = new Route(name, DateTime.Now, new ObservableCollection<DataStamp>(), 0);
        }

        public void LoadData()
        {
            //ROUTE PROTOCOL - "route" | ROUTE NAME | TOTAL DISTANCE | COMPLETION DATE
            //ROUTE DETAILS PROTOCOL - LATITUDE | LONGITUDE | CURRENT TIME | CURRENT SPEED | CURRENT TOTAL DISTANCE

            if (File.Exists(ApplicationData.Current.LocalFolder.Path + "//RouteList.txt"))
            {
                routeHistory = new ObservableCollection<Route>();
                string[] list = File.ReadAllLines(ApplicationData.Current.LocalFolder.Path + "//RouteList.txt");

                foreach (string temp in list)
                {
                    System.Diagnostics.Debug.WriteLine(temp);
                }

                for (int i = 0; i < list.Length; i ++)
                {
                    //read details
                    if (!list[i].StartsWith("route"))
                    {
                        string[] items = list[i].Split('|');

                        Geopoint point = new Geopoint(new BasicGeoposition() { Latitude = Double.Parse(items[0]), Longitude = Double.Parse(items[1]) });

                        routeHistory[routeHistory.Count - 1].route.Add(new DataStamp(point, DateTime.Parse(items[2]), Double.Parse(items[3]), Double.Parse(items[4])));
                    }
                    //read routes
                    else
                    {
                        routeHistory.Add(new Route(null, DateTime.Now, new ObservableCollection<DataStamp>(), 0));
                        string[] items = list[i].Split('|');

                        routeHistory[routeHistory.Count - 1].name = items[1];
                        routeHistory[routeHistory.Count - 1].totalDistance = Double.Parse(items[2]);
                        routeHistory[routeHistory.Count - 1].date = DateTime.Parse(items[3]);
                    }
                }
            }
        }

        public string speedChanges(string speed)
        { 
            for(int i = 0; i < currentRoute.route.Count; i++)
            {
                if (currentRoute.route.Count != 0)
                {
                    DataStamp item = currentRoute.route[currentRoute.route.Count - 1];

                    currentSpeed = item.speed.ToString();
                    currentSpeed = speed;

                    //round down to 2 decimals on screen

                    double metricSpeed = Double.Parse(currentSpeed);
                    metricSpeed = Math.Round(metricSpeed*3.6, 1);
                    currentSpeed = metricSpeed.ToString();
                    NotifyPropertyChanged(nameof(currentSpeed));
                }
                else
                {
                    currentSpeed = "0";
                }
            }
          
            return currentSpeed;
        }

        public async Task<double> calculateUpdateDistance(Geopoint start, Geopoint end)
        {
            MapRouteFinderResult routeResult = await MapRouteFinder.GetWalkingRouteAsync(start, end);
            double distance = 0;
            if(routeResult.Route != null)
            {
                MapRoute b = routeResult.Route;
                distance = b.LengthInMeters;
                currentDistance = (double.Parse(currentDistance) + distance).ToString();
            }
            NotifyPropertyChanged(nameof(currentDistance));
            return distance;
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool getZoomCenter()
        {
            return zoomCenter;
        }

        public void setZoomCenter(bool zoomCenter)
        {
            this.zoomCenter = zoomCenter;
        }

        public bool getDrawOld()
        {
            return drawOld;
        }

        public void setDrawOld(bool drawOld)
        {
            this.drawOld = drawOld;
        }
    }
}