using HabitatReStoreMobile.Interfaces;
using HabitatReStoreMobile.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HabitatReStoreMobile.ServiceReference1;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace HabitatReStoreMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VolunteerFormPage : ContentPage
    {
        ObservableCollection<VolunteerCategory> volunteerCategories = new ObservableCollection<VolunteerCategory>();
        List<VolunteerRoleItem> volunteerRoleItems = new List<VolunteerRoleItem>();

        public VolunteerFormPage()
        {
            InitializeComponent();

            if (App.service == null)
            {
                App.InitializeService();
            }

            InitializePickers();
        }

        private void btnSubmit_Clicked(object sender, EventArgs e)
        {
            VolunteerInfo newVolunteer = GetVolunteer();

            if (ValidateAll(newVolunteer))
            {
                btnSubmit.IsEnabled = false;
                DependencyService.Get<IToast>().ShortAlert("Submitting... please wait.");

                InsertVolunteerToDatabase(newVolunteer);
            }
        }

        private VolunteerInfo GetVolunteer()
        {
            string fName, lName, mName, email, phone, ssn, city, zip, address, address2;
            string state, role, contactMethod;
            char gender;
            DateTime dob;
            ObservableCollection<int> roleIDs = new ObservableCollection<int>();

            fName = entFName.Text;
            lName = entLName.Text;
            mName = entMName.Text;
            email = entEmail.Text;
            //strip non-numeric characters, if null return empty space
            phone = Regex.Replace(entPhone.Text ?? "", "[^0-9]", "");
            ssn = entSSN.Text;
            city = entCity.Text;
            zip = entZIP.Text;
            address = entAddress.Text;
            address2 = entAddress2.Text;
            gender = pickGender.SelectedItem.ToString()[0];
            state = pickState.SelectedItem.ToString();
            contactMethod = pickContactMethod.SelectedItem.ToString();

            //get selected roles
            foreach (VolunteerRoleItem vc in rolePickerList.ItemsSource)
            {
                if (vc.Selected)
                {
                    roleIDs.Add(vc.Category_ID);
                }
            }

            dob = pickDOB.Date;

            VolunteerInfo newVolunteer = new VolunteerInfo();
            newVolunteer.Status_Map_ID = 1;
            newVolunteer.First_Name = fName;
            newVolunteer.Middle_Name = mName;
            newVolunteer.Last_Name = lName;
            newVolunteer.Gender = gender;
            newVolunteer.DOB = dob;
            newVolunteer.Email = email;
            newVolunteer.Phone = phone;
            newVolunteer.ZipCode = zip;
            newVolunteer.City = city;
            newVolunteer.State = state;
            newVolunteer.Address = address;
            newVolunteer.Address2 = address2;
            newVolunteer.SSN = ssn;
            newVolunteer.Selected_Role_IDs = roleIDs;

            return newVolunteer;
        }

        private bool ValidateAll(VolunteerInfo newVolunteer)
        {
            bool valid = true;

            //first name check
            if (!Validator.NullOrEmptyRule(newVolunteer.First_Name))
            {
                lblVFName.Text = "Please enter First Name";
                valid = false;
            }
            else
            {
                lblVFName.Text = "";
            }

            //last name check
            if (!Validator.NullOrEmptyRule(newVolunteer.Last_Name))
            {
                lblVLName.Text = "Please enter Last Name";
                valid = false;
            }
            else
            {
                lblVLName.Text = "";
            }

            //city check
            if (!Validator.NullOrEmptyRule(newVolunteer.City))
            {
                lblVCity.Text = "Please enter City";
                valid = false;
            }
            else
            {
                lblVCity.Text = "";
            }

            //address check
            if (!Validator.NullOrEmptyRule(newVolunteer.Address))
            {
                lblVAddress.Text = "Please enter Address";
                valid = false;
            }
            else
            {
                lblVAddress.Text = "";
            }

            //zip check
            if (!Validator.NullOrEmptyRule(newVolunteer.ZipCode))
            {
                lblVZIP.Text = "Please enter ZIP Code";
                valid = false;
            }
            else if (!Validator.ZIPCheck(newVolunteer.ZipCode))
            {
                lblVZIP.Text = "Invalid ZIP Code";
                valid = false;
            }
            else
            {
                lblVZIP.Text = "";
            }

            //email check
            if (!Validator.NullOrEmptyRule(newVolunteer.Email))
            {
                lblVEmail.Text = "Please enter Email";
                valid = false;
            }
            else if (!Validator.EmailCheck(newVolunteer.Email))
            {
                lblVEmail.Text = "Email Invalid";
                valid = false;
            }
            else
            {
                lblVEmail.Text = "";
            }

            //phone check
            if (!Validator.NullOrEmptyRule(newVolunteer.Phone))
            {
                lblVPhone.Text = "Please enter Phone Number";
                valid = false;
            }
            else if (!Validator.PhoneCheck(newVolunteer.Phone))
            {
                lblVPhone.Text = "Phone Number Invalid";
                valid = false;
            }
            else
            {
                lblVPhone.Text = "";
            }

            //at least 14 check
            if (GetAge(newVolunteer.DOB) < 14)
            {
                lblVDOB.Text = "You must be at least 14 years of age to volunteer";
                valid = false;
            }
            else
            {
                lblVDOB.Text = "";
            }

            //has selected at least one role check
            if (newVolunteer.Selected_Role_IDs.Count == 0)
            {
                lblVDOB.Text = "Please select at least one role.";
                valid = false;
            }
            else
            {
                lblVRoles.Text = "";
            }

            return valid;
        }

        public int GetAge(DateTime birthdate)
        {
            int age;
            DateTime now = DateTime.Now;
            age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || now.Month == birthdate.Month && now.Day < birthdate.Day)
            {
                age--;
            }

            return age;
        }

        private void InsertVolunteerToDatabase(VolunteerInfo volunteer)
        {
            try
            {
                //first removes handler so that this event can only be subscribed to once
                App.service.InsertVolunteerCompleted -= Service_InsertVolunteerCompleted;
                App.service.InsertVolunteerCompleted += Service_InsertVolunteerCompleted;
                App.service.InsertVolunteerAsync(volunteer);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().LongAlert("Error, unable to access server.");
                btnSubmit.IsEnabled = true;
                Debug.WriteLine(ex);
            }
        }

        private void Service_InsertVolunteerCompleted(object sender, InsertVolunteerCompletedEventArgs e)
        {
            bool success = false;
            string message = "";

            if (e.Error == null)
            {
                int result = e.Result;
                if (result == 1)
                {
                    success = true;
                    message = "Volunteer form successfully submitted";
                }
                else
                {
                    success = false;
                    message = "Error submitting volunteer form.";
                }
            }
            else
            {
                success = false;
                message = "Error submitting volunteer form.";
                Debug.WriteLine(e.Error);
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                DependencyService.Get<IToast>().LongAlert(message);

                btnSubmit.IsEnabled = true;

                if (success)
                {
                    Navigation.PopToRootAsync();
                }
            });
        }

        private void InitializePickers()
        {
            List<string> contactMethods = new List<string> { "Email", "Phone" };
            List<string> genderOptions = new List<string> { "M", "F", "Other" };
            List<string> stateOptions = new List<string> { "AK", "AL", "AR", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "GA", "GU", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MH", "MI", "MN", "MO", "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "PR", "PW", "RI", "SC", "SD", "TN", "TX", "UT", "VA", "VI", "VT", "WA", "WI", "WV", "WY" };

            pickGender.ItemsSource = genderOptions;
            pickState.ItemsSource = stateOptions;
            pickContactMethod.ItemsSource = contactMethods;

            pickGender.SelectedIndex = 0;
            pickState.SelectedItem = "NC";
            pickContactMethod.SelectedIndex = 0;

            pickDOB.Date = DateTime.Today;

            //get possible values from volunteer category
            try
            {
                //first removes handler so that this event can only be subscribed to once
                App.service.GetVolunteerCategoriesCompleted -= Service_GetVolunteerCategoriesCompleted;
                App.service.GetVolunteerCategoriesCompleted += Service_GetVolunteerCategoriesCompleted;
                App.service.GetVolunteerCategoriesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void Service_GetVolunteerCategoriesCompleted(object sender, GetVolunteerCategoriesCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                volunteerCategories = e.Result;

                foreach (VolunteerCategory vc in volunteerCategories)
                {
                    volunteerRoleItems.Add(new VolunteerRoleItem()
                    {
                        Category = vc.Description,
                        Category_ID = vc.Volunteer_Category_ID,
                        Selected = false
                    });
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    rolePickerList.ItemsSource = volunteerRoleItems;
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Get<IToast>().ShortAlert("Unable to connect to server.");
                });
                Debug.WriteLine(e.Error);
            }
        }

        //deselect item when selected, so that selected wont show in UI
        private void rolePickerList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            rolePickerList.SelectedItem = null;
        }

        private void swRole_Toggled(object sender, ToggledEventArgs e)
        {
            
        }
    }
}