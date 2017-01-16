using System;
using System.Collections.ObjectModel;

namespace Runkeeper.Model
{
    public class Route
    {
        public DateTime date { get; set; }
        public ObservableCollection<DataStamp> route { get; set; }
        public double totalDistance { get; set; }

        public Route(DateTime date, ObservableCollection<DataStamp> route, double totalDistance)
        {
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
