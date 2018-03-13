using HabitatReStoreMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HabitatReStoreMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DonationPickupFormPage : ContentPage
    {
        public DonationPickupFormPage()
        {
            InitializeComponent();

            var vm = new DonationPickupFormViewModel();
            vm.Navigation = Navigation;
            BindingContext = vm;
        }
    }
}