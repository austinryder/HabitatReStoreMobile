using HabitatReStoreMobile.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HabitatReStoreMobile.ViewModels
{
    class DonationHomeViewModel : INotifyPropertyChanged
    {
        public Command PickupCommand { get { return new Command(GoToPickup); } }
        public Command DonorCommand { get { return new Command(GoToDonor); } }

        public INavigation Navigation { get; set; }

        private void GoToPickup()
        {
            Navigation.PushAsync(new DonationPickupFormPage());
        }

        private void GoToDonor()
        {
            Navigation.PushAsync(new DonorFormPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

