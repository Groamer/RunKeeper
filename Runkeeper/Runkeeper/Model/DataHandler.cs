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

namespace Runkeeper
{
    public class DataHandler : INotifyPropertyChanged
    {
        public Route currentRoute = new Route(DateTime.Now, new ObservableCollection<DataStamp>(), 0);
        public ObservableCollection<Route> routeHistory { get; set; }
        public MapIcon currentposition;
        public MapPolyline calculatedRoute;
        public Geopoint startposition;
        public string from, to;
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

        public void saveData(String routeName)
        {
            currentRoute.totalDistance = double.Parse(currentDistance);
            routeHistory.Add(currentRoute);
            currentDistance = "0";
            currentRoute = new Route(DateTime.Now, new ObservableCollection<DataStamp>(), 0);
            List<string> list = new List<string>();
            for (int v = 0; v < routeHistory.Count; v++)
            {
                list.Add("route" + "|" + routeName + "|" + routeHistory[v].totalDistance + "|" + routeHistory[v].date.ToString());
                for (int i = 0; i < routeHistory[v].route.Count; i++)
                {
                    list.Add(routeHistory[v].route[i].location.Position.Latitude + "|" + routeHistory[v].route[i].location.Position.Longitude + "|"
                    + routeHistory[v].route[i].time.ToString() + "|" + routeHistory[v].route[i].speed + "|" + routeHistory[v].route[i].distance);
                }
            }
            File.WriteAllLines(ApplicationData.Current.LocalFolder.Path + "//RouteList.txt", list);
        }

        public void loadData()
        {
            if (File.Exists(ApplicationData.Current.LocalFolder.Path + "//RouteList.txt"))
            {
                routeHistory = new ObservableCollection<Route>();
                string[] list = File.ReadAllLines(ApplicationData.Current.LocalFolder.Path + "//RouteList.txt");
                
                foreach (String temp in list)
                {
                    System.Diagnostics.Debug.WriteLine(temp);
                }

                /*
                for (int i = 0; i < list.Length; i++)
                {
                    if (!list[i].StartsWith("route"))
                    {
                        string[] items = list[i].Split('|');
                        Geopoint point = new Geopoint(new BasicGeoposition() { Latitude = Double.Parse(items[1]), Longitude = Double.Parse(items[2]) });
                        routeHistory[routeHistory.Count - 2].route.Add(new DataStamp(point, DateTime.Parse(items[3]), Double.Parse(items[4]), Double.Parse(items[5])));
                    }
                    else
                    {
                        routeHistory.Add(new Route(DateTime.Now, new ObservableCollection<DataStamp>(), 0));
                        string[] items = list[i].Split('|');
                        routeHistory[routeHistory.Count - 1].totalDistance = Double.Parse(items[2]);
                        routeHistory[routeHistory.Count - 1].date = DateTime.Parse(items[3]);
                    }
                }*/
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
