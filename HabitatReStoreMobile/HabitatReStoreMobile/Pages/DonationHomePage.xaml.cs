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

        private void btnGoReturningDonorForm_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ReturningDonorPage());
        }

        private void btnGoDonorForm_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DonorFormPage());
        }
    }
}