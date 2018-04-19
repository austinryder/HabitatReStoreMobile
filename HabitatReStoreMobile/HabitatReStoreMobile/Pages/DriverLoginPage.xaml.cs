using HabitatReStoreMobile.Interfaces;
using HabitatReStoreMobile.Models;
using HabitatReStoreMobile.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HabitatReStoreMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DriverLoginPage : ContentPage
    {
        DriverInfo driver = new DriverInfo();
        string email;
        int driverID;

        public DriverLoginPage()
        {
            InitializeComponent();

            if (App.service == null)
            {
                App.InitializeService();
            }
        }

        private void btnSubmit_Clicked(object sender, EventArgs e)
        {
            email = entEmail.Text;

            if (!Validator.NullOrEmptyRule(email))
            {
                lblVEmail.Text = "Please enter email";
            }
            else if (!Validator.EmailCheck(email))
            {
                lblVEmail.Text = "Email Invalid";
            }
            else
            {
                if (int.TryParse(entDriverID.Text, out driverID))
                {
                    lblVID.Text = "";

                    btnSubmit.IsEnabled = false;
                    DependencyService.Get<IToast>().ShortAlert("Submitting... please wait.");

                    GetDriverInfo(email, driverID);
                }
                else
                {
                    lblVID.Text = "Driver ID Invalid";
                }
            }
        }

        private void GetDriverInfo(string email, int driverID)
        {
            try
            {
                //first removes handler so that this event can only be subscribed to once
                App.service.GetDriverFromIDAndEmailCompleted -= Service_GetDriverFromIDAndEmailCompleted;
                App.service.GetDriverFromIDAndEmailCompleted += Service_GetDriverFromIDAndEmailCompleted;
                App.service.GetDriverFromIDAndEmailAsync(driverID, email);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().LongAlert("Error, unable to access server.");
                btnSubmit.IsEnabled = true;
                Debug.WriteLine(ex);
            }
        }

        private void Service_GetDriverFromIDAndEmailCompleted(object sender, GetDriverFromIDAndEmailCompletedEventArgs e)
        {
            bool success = false;
            string message = "";
            DriverInfo result = new DriverInfo();

            if (e.Error == null)
            {
                result = e.Result;

                if (result.Email == email && result.Driver_ID == driverID)
                {
                    driver = result;
                    success = true;
                    message = "Driver login successful.";
                }
                else
                {
                    success = false;
                    message = "Driver's ID/Email not found.";
                }
            }
            else
            {
                success = false;
                message = "Driver's ID/Email not found.";
                Debug.WriteLine(e.Error);
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                DependencyService.Get<IToast>().ShortAlert(message);

                btnSubmit.IsEnabled = true;

                if (success)
                {
                    this.Navigation.PushAsync(new DriverPickupsPage(driver));
                }
            });
        }
    }
}