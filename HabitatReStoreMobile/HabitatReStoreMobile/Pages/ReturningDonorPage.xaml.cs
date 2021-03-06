﻿using HabitatReStoreMobile.Interfaces;
using HabitatReStoreMobile.Models;
using HabitatReStoreMobile.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HabitatReStoreMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReturningDonorPage : ContentPage
    {
        DonorInfo donor = new DonorInfo();
        string email;

        public ReturningDonorPage()
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
                btnSubmit.IsEnabled = false;
                DependencyService.Get<IToast>().ShortAlert("Submitting... please wait.");

                GetDonorInfoFromEmail(email);
            }
        }

        private void GetDonorInfoFromEmail(string email)
        {
            try
            {
                //first removes handler so that this event can only be subscribed to once
                App.service.GetDonorFromEmailCompleted -= Service_GetDonorFromEmailCompleted;
                App.service.GetDonorFromEmailCompleted += Service_GetDonorFromEmailCompleted;
                App.service.GetDonorFromEmailAsync(email);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().LongAlert("Error, unable to access server.");
                btnSubmit.IsEnabled = true;
                Debug.WriteLine(ex);
            }
        }

        private void Service_GetDonorFromEmailCompleted(object sender, GetDonorFromEmailCompletedEventArgs e)
        {
            bool success = false;
            string message = "";
            DonorInfo result = new DonorInfo();

            if (e.Error == null)
            {
                result = e.Result;

                if (result.Email == email)
                {
                    donor = result;
                    success = true;
                    message = "Donor information succesfully retrieved.";
                }
                else
                {
                    success = false;
                    message = "Email not found.";
                }
            }
            else
            {
                success = false;
                message = "Error retrieving donor information.";
                Debug.WriteLine(e.Error);
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                DependencyService.Get<IToast>().ShortAlert(message);

                btnSubmit.IsEnabled = true;

                if (success)
                {
                    this.Navigation.PushAsync(new DonationPickupFormPage(donor));
                }
            });
        }
    }
}