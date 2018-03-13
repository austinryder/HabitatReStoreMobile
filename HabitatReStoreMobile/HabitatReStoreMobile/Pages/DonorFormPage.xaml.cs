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
    public partial class DonorFormPage : ContentPage
    {
        public DonorFormPage()
        {
            InitializeComponent();

            var vm = new DonorFormViewModel();
            vm.Navigation = Navigation;
            BindingContext = vm;
        }
    }
}