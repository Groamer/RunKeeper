using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Controls.Maps;

namespace Runkeeper.Model
{
    public class MapHelper: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<Geoposition> CurrentLocation(Geoposition position)
        {
            if (App.instance.transfer.data.currentposition == null)
            {
                App.instance.transfer.data.currentposition = new MapIcon();
                App.instance.transfer.data.currentposition.ZIndex = 3;
                App.instance.transfer.data.currentposition.NormalizedAnchorPoint = new Point(0.5, 1.0);
                App.instance.transfer.data.currentposition.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapIcon.png"));
            }
            App.instance.transfer.data.currentposition.Location = position.Coordinate.Point;
            double speed = Double.Parse(App.instance.transfer.data.speedChanges(position.Coordinate.Speed.ToString()));
            NotifyPropertyChanged(nameof(App.instance.transfer.data.currentSpeed));

            if (App.instance.transfer.data.currentRoute.route.Count != 0)
            {
                double distance = await App.instance.transfer.data.calculateUpdateDistance(App.instance.transfer.data.currentRoute.route[App.instance.transfer.data.currentRoute.route.Count - 1].location, position.Coordinate.Point);
                App.instance.transfer.data.currentRoute.route.Add(new DataStamp(position.Coordinate.Point, DateTime.Now, speed, distance));
            }
            else
            {
                App.instance.transfer.data.currentRoute.route.Add(new DataStamp(position.Coordinate.Point, DateTime.Now, 0, 0));
            }

            return position;
        }

        private void NotifyPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public MapPolyline DrawRoute()
        {
            MapPolyline currentline = new MapPolyline
            {
                StrokeThickness = 6,
                StrokeColor = Colors.Green,
                StrokeDashed = false,
                ZIndex = 2
            };
            List<BasicGeoposition> positions = new List<BasicGeoposition>();
            for (int i = 0; i < App.instance.transfer.data.currentRoute.route.Count; i++)
            {
                positions.Add(new BasicGeoposition() { Latitude = App.instance.transfer.data.currentRoute.route[i].location.Position.Latitude, Longitude = App.instance.transfer.data.currentRoute.route[i].location.Position.Longitude });
            }
            currentline.Path = new Geopath(positions);
            return currentline;
        }
    }
}
