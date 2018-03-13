using HabitatReStoreMobile.Models;
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
        public FindReStorePage()
        {
            InitializeComponent();

            //List<Pin> reStorePins = new List<Pin>();
            /*
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(36.124, -80.259),
                Label = "Habitat ReStore - Winston-Salem",
                Address = "608 Coliseum Dr NW, Winston-Salem, NC 27106"
            };

            ReStoreMap.Pins.Add(pin);*/
        }
    }
}