using HabitatReStoreMobile.ServiceReference1;
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
        public static EndpointAddress SERVICEADDRESS = new EndpointAddress("http://639991bf.ngrok.io/HabitatWCFService/HabitatService.svc");
        public static HabitatServiceClient service;
        public App()
        {
            InitializeComponent();

            MainPage = new HabitatReStoreMobile.Pages.MainPage();
        }

        public static void InitializeService()
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                Name = "basicHttpBinding",
                MaxBufferSize = 2147483647,
                MaxReceivedMessageSize = 2147483647
            };
            TimeSpan timeout = new TimeSpan(0, 0, 30);
            binding.SendTimeout = timeout;
            binding.OpenTimeout = timeout;
            binding.ReceiveTimeout = timeout;

            service = new HabitatServiceClient(binding, App.SERVICEADDRESS);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            InitializeService();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            service.CloseAsync();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            InitializeService();
        }
    }
}
