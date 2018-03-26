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
        }

        private void btnGoSignUp_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new VolunteerFormPage());
        }
    }
}