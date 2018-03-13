using HabitatReStoreMobile.Models;
using HabitatReStoreMobile.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HabitatReStoreMobile.ViewModels
{
    class DonorFormViewModel : INotifyPropertyChanged
    {
        private string _fName, _lName, _email, _phone;
        private string _vfname, _vlname, _vemail, _vphone;
        public Command SubmitCommand { get { return new Command(Submit); } }
        public INavigation Navigation { get; set; }

        //Entry Text
        public string FirstName { get { return _fName; } set { _fName = value; OnPropertyChanged(); } }
        public string LastName { get { return _lName; } set { _lName = value; OnPropertyChanged(); } }
        public string Email { get { return _email; } set { _email = value; OnPropertyChanged(); } }
        public string Phone { get { return _phone; } set { _phone = value; OnPropertyChanged(); } }

        //Validation Labels for Entries
        public string VFName { get { return _vfname; } set { _vfname = value; OnPropertyChanged(); } }
        public string VLName { get { return _vlname; } set { _vlname = value; OnPropertyChanged(); } }
        public string VEmail { get { return _vemail; } set { _vemail = value; OnPropertyChanged(); } }
        public string VPhone { get { return _vphone; } set { _vphone = value; OnPropertyChanged(); } }

        public DonorFormViewModel()
        {

        }

        private void Submit()
        {
            if (ValidateAll())
            {
                OnPropertyChanged("SubmitCommand");

                Navigation.PushAsync(new DonationPickupFormPage());
            }
        }
        private bool ValidateAll()
        {
            bool valid = true;
            //first name check
            if (!Validator.NullOrEmptyRule(_fName))
            {
                VFName = "Please enter First Name";
                valid = false;
            }
            else
            {
                VFName = "";
            }

            //last name check
            if (!Validator.NullOrEmptyRule(_lName))
            {
                VLName = "Please enter Last Name";
                valid = false;
            }
            else
            {
                VLName = "";
            }

            //email check
            if (!Validator.NullOrEmptyRule(_email))
            {
                VEmail = "Please enter Email";
                valid = false;
            }
            else if (!Validator.EmailCheck(_email))
            {
                VEmail = "Email Invalid";
                valid = false;
            }
            else
            {
                VEmail = "";
            }

            //phone check
            if (!Validator.NullOrEmptyRule(_phone))
            {
                VPhone = "Please enter Phone Number";
                valid = false;
            }
            else if (!Validator.PhoneCheck(_phone))
            {
                VPhone = "Phone Number Invalid";
                valid = false;
            }
            else
            {
                VPhone = "";
            }

            return valid;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
