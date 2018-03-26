using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Xamarin.Forms;

namespace HabitatReStoreMobile
{
    public partial class App : Application
    {
        public static EndpointAddress SERVICEADDRESS = new EndpointAddress("http://7440753d.ngrok.io/HabitatWCFService/HabitatService.svc");
        public App()
        {
            InitializeComponent();

            MainPage = new HabitatReStoreMobile.Pages.MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
