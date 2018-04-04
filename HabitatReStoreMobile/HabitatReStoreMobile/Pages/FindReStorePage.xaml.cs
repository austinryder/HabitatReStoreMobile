using HabitatReStoreMobile.Interfaces;
using HabitatReStoreMobile.Models;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace HabitatReStoreMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindReStorePage : ContentPage
    {
        private List<Pin> reStorePins = new List<Pin>();
        private Pin currentLocation;

        public FindReStorePage()
        {
            InitializeComponent();

            InitializeMap();
        }

        private void btnGetLocation_Clicked(object sender, EventArgs e)
        {
            SetCurrentLocation();
        }

        public void InitializeMap()
        {
            MakePins();
            SetCurrentLocation();
        }

        public void MakePins()
        {     
            reStorePins.Add(new Pin
            {
                Type = PinType.Place,
                Position = new Position(36.123541, -80.259738),
                Label = "Habitat ReStore in Winston-Salem",
                Address = "608 Coliseum Dr NW, Winston-Salem, NC 27106"
            });
            reStorePins.Add(new Pin
            {
                Type = PinType.Place,
                Position = new Position(36.128995, -80.065368),
                Label = "Habitat ReStore in Kernersville",
                Address = "619 North Main Street, Kernersville, NC 27284"
            });
            reStorePins.Add(new Pin
            {
                Type = PinType.Place,
                Position = new Position(36.097838, -80.420805),
                Label = "Habitat ReStore in Lewisville",
                Address = "6491 Shallowford Road, Lewisville, NC 27023"
            });
            reStorePins.Add(new Pin
            {
                Type = PinType.Place,
                Position = new Position(36.280439, -80.358493),
                Label = "Habitat ReStore in King",
                Address = "115 East Dalton Road, King, NC 27021"
            });

            mapReStores.Pins.Clear();
            foreach (Pin x in reStorePins) {
                mapReStores.Pins.Add(x);
            }
        }

        public async void SetCurrentLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                if (locator.IsGeolocationEnabled)
                {
                    var position = await locator.GetPositionAsync(timeout: new TimeSpan(10000));
                    mapReStores.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(5)));

                    if (currentLocation != null)
                    {
                        mapReStores.Pins.RemoveAt(mapReStores.Pins.Count - 1);
                    }

                    currentLocation = new Pin
                    {
                        Type = PinType.SearchResult,
                        Position = new Position(position.Latitude, position.Longitude),
                        Label = "You",
                    };

                    mapReStores.Pins.Add(currentLocation);            
                }
                else
                {
                    DependencyService.Get<IToast>().ShortAlert("Geolocation is disabled.");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().ShortAlert("Error occured, unable to get location.");
                Debug.WriteLine(ex);
            }
        }
    }
}