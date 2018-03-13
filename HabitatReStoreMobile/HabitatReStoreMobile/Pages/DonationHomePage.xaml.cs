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
    public partial class DonationHomePage : ContentPage
    {
        public DonationHomePage()
        {
            InitializeComponent();

            var vm = new DonationHomeViewModel();
            vm.Navigation = Navigation;
            BindingContext = vm;
        }
    }
}