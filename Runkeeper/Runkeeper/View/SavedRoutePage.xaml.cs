using Runkeeper.Model;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Runkeeper.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SavedRoutePage : Page
    {
        private Route route;
        private MapHelper maphelper;
        private Geolocator geolocator;

        public SavedRoutePage()
        {
            this.InitializeComponent();
            maphelper = new MapHelper();

            UpdateGeoPoints();
        }

        private void UpdateGeoPoints()
        {
            mapController.ZoomLevel = 17;

            mapController.MapElements.Add(maphelper.DrawSavedRoute());

            mapController.Center = maphelper.SavedRouteFocus();
        }
    }
}
