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
        }

        private void btnGoPickupForm_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DonationPickupFormPage());
        }

        private void btnGoDonorForm_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DonorFormPage());
        }
    }
}