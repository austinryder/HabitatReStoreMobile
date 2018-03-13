using HabitatReStoreMobile.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HabitatReStoreMobile.ViewModels
{
    class DonationPickupFormViewModel : INotifyPropertyChanged
    {
        private ImageSource _picturesource;
        public Command SubmitCommand { get { return new Command(Submit); } }
        public Command AddPictureCommand { get { return new Command(AddPicture); } }
        public INavigation Navigation { get; set; }

        public ImageSource PictureSource
        {
            get
            {
                return _picturesource;
            }
            set
            {
                _picturesource = value;
                OnPropertyChanged("PictureSource");
            }
        }

        private void Submit()
        {
            //if (ValidateAll())
            //{
            OnPropertyChanged("SubmitCommand");

            DependencyService.Get<IToast>().LongAlert("Pickup Request Form Submitted Successfully.");
            Navigation.PopToRootAsync();
            //}
        }

        private void AddPicture()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
