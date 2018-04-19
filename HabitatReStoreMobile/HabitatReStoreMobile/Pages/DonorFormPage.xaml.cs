using HabitatReStoreMobile.Interfaces;
using HabitatReStoreMobile.Models;
using HabitatReStoreMobile.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HabitatReStoreMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DonorFormPage : ContentPage
    {
        DonorInfo newDonor;

        public DonorFormPage()
        {
            InitializeComponent();

            InitializePickers();

            if (App.service == null)
            {
                App.InitializeService();
            }
        }

        private void btnSubmit_Clicked(object sender, EventArgs e)
        {
            newDonor = GetDonor();

            if (ValidateAll(newDonor))
            {
                btnSubmit.IsEnabled = false;
                DependencyService.Get<IToast>().ShortAlert("Submitting... please wait.");

                InsertDonorToDatabase(newDonor);
            }
        }

        private DonorInfo GetDonor()
        {
            string fName, lName, mName, email, phone, city, zip, address, address2;
            string state;
            char gender;

            fName = entFName.Text;
            lName = entLName.Text;
            mName = entMName.Text;
            email = entEmail.Text;
            //strip non-numeric characters, if null return empty space
            phone = Regex.Replace(entPhone.Text ?? "", "[^0-9]", "");
            city = entCity.Text;
            zip = entZIP.Text;
            address = entAddress.Text;
            address2 = entAddress2.Text;
            gender = pickGender.SelectedItem.ToString()[0];
            state = pickState.SelectedItem.ToString();

            DonorInfo newDonor = new DonorInfo();
            newDonor.Status_Map_ID = 1;
            newDonor.First_Name = fName;
            newDonor.Middle_Name = mName;
            newDonor.Last_Name = lName;
            newDonor.Gender = gender;
            newDonor.Email = email;
            newDonor.Phone = phone;
            newDonor.ZipCode = zip;
            newDonor.City = city;
            newDonor.State = state;
            newDonor.Address = address;
            newDonor.Address2 = address2;

            return newDonor;
        }

        private void InsertDonorToDatabase(DonorInfo donor)
        {
            try
            {
                //first removes handler so that this event can only be subscribed to once
                App.service.InsertDonorCompleted -= Service_InsertDonorCompleted;
                App.service.InsertDonorCompleted += Service_InsertDonorCompleted;
                App.service.InsertDonorAsync(donor);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().LongAlert("Error, unable to access server.");
                btnSubmit.IsEnabled = true;
                Debug.WriteLine(ex);
            }
        }

        private void Service_InsertDonorCompleted(object sender, InsertDonorCompletedEventArgs e)
        {
            bool success = false;
            string message = "";

            if (e.Error == null)
            {
                success = true;
                message = "Donor form successfully submitted";
            }
            else
            {
                success = false;
                message = "Error submitting donor form.";
                Debug.WriteLine(e.Error);
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                DependencyService.Get<IToast>().LongAlert(message);

                btnSubmit.IsEnabled = true;

                if (success)
                {
                    Navigation.PushAsync(new DonationPickupFormPage(newDonor));
                }
            });
        }

        private bool ValidateAll(DonorInfo newDonor)
        {
            bool valid = true;
            //first name check
            if (!Validator.NullOrEmptyRule(newDonor.First_Name))
            {
                lblVFName.Text = "Please enter First Name";
                valid = false;
            }
            else
            {
                lblVFName.Text = "";
            }

            //last name check
            if (!Validator.NullOrEmptyRule(newDonor.Last_Name))
            {
                lblVLName.Text = "Please enter Last Name";
                valid = false;
            }
            else
            {
                lblVLName.Text = "";
            }

            //email check
            if (!Validator.NullOrEmptyRule(newDonor.Email))
            {
                lblVEmail.Text = "Please enter Email";
                valid = false;
            }
            else if (!Validator.EmailCheck(newDonor.Email))
            {
                lblVEmail.Text = "Email Invalid";
                valid = false;
            }
            else
            {
                lblVEmail.Text = "";
            }

            //phone check
            if (!Validator.NullOrEmptyRule(newDonor.Phone))
            {
                lblVPhone.Text = "Please enter Phone Number";
                valid = false;
            }
            else if (!Validator.PhoneCheck(newDonor.Phone))
            {
                lblVPhone.Text = "Phone Number Invalid";
                valid = false;
            }
            else
            {
                lblVPhone.Text = "";
            }

            return valid;
        }

        private void InitializePickers()
        {
            List<string> genderOptions = new List<string> { "M", "F", "Other" };
            List<string> stateOptions = new List<string> { "AK", "AL", "AR", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "GA", "GU", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MH", "MI", "MN", "MO", "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "PR", "PW", "RI", "SC", "SD", "TN", "TX", "UT", "VA", "VI", "VT", "WA", "WI", "WV", "WY" };

            pickGender.ItemsSource = genderOptions;
            pickState.ItemsSource = stateOptions;

            pickGender.SelectedIndex = 0;
            pickState.SelectedItem = "NC";
        }
    }
}