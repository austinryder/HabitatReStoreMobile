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
    public partial class VolunteerHomePage : ContentPage
    {
        public VolunteerHomePage()
        {
            InitializeComponent();

            var vm = new VolunteerHomeViewModel();
            vm.Navigation = Navigation;
            BindingContext = vm;
        }
    }
}