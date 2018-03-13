using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace HabitatReStoreMobile.ViewModels
{
    class FindReStoreViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Pin> _pins = new ObservableCollection<Pin>();
        private Position _currentPosition = new Position(-37.8141, 144.9633);
        public ObservableCollection<Pin> Pins { get { return _pins; } set { _pins = value; OnPropertyChanged(); } }
        public Position CurrentPosition { get { return _currentPosition; } set { _currentPosition = value; OnPropertyChanged(); } }

        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public FindReStoreViewModel()
        {
            //SetCurrentLocation();

            Pins.Add( new Pin {
                    Type = PinType.Place,
                    Position = new Position(36.124, -80.259),
                    Label = "Habitat ReStore - Winston-Salem",
                    Address = "608 Coliseum Dr NW, Winston-Salem, NC 27106"
                });
        }

        //public async void SetCurrentLocation()
        //{
            //var position = await Plugin.Geolocator.CrossGeolocator.Current.GetPositionAsync();
            //CurrentPosition = new Position(position.Latitude, position.Longitude);
        //}

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
