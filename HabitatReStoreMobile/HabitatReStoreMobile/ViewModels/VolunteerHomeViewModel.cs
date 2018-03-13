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
    class VolunteerHomeViewModel : INotifyPropertyChanged
    {
        public Command SignUpCommand { get { return new Command(SignUp); } }

        public INavigation Navigation { get; set; }

        private void SignUp()
        {
            Navigation.PushAsync(new VolunteerFormPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
