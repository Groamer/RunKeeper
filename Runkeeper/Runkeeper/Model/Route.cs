using System;
using System.Collections.ObjectModel;

namespace Runkeeper.Model
{
    public class Route
    {
        public String name { get; set;  }
        public DateTime date { get; set; }
        public ObservableCollection<DataStamp> route { get; set; }
        public double totalDistance { get; set; }

        public Route(String name, DateTime date, ObservableCollection<DataStamp> route, double totalDistance)
        {
            this.name = name;
            this.date = date;
            this.route = route;
            this.totalDistance = totalDistance;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", totalDistance,date);
        }
    }
}
