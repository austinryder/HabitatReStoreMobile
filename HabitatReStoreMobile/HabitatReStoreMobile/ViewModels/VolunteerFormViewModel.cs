using HabitatReStoreMobile.Interfaces;
using HabitatReStoreMobile.Models;
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
    class VolunteerFormViewModel : INotifyPropertyChanged
    {
        //Entry Strings
        private string _fName, _lName, _email, _phone, _ssn, _city, _address, _address2;

        //Picker Defaults
        private string _state = "NC";
        private string _gender = "M";
        private string _role = "Door Hanger";
        private string _contact = "Email";
        private DateTime _dob = DateTime.Today;

        //Validator Strings
        private string _vfname, _vlname, _vemail, _vphone, _vssn, _vcity, _vaddress, _vaddress2;

        //Picker Lists
        private List<string> _contactmethods = new List<string> { "Email", "Phone" };
        private List<string> _volunteerroleoptions = new List<string> { "Door Hanger", "Cashier", "Cost Researcher", "Sales Floor", "Back Room", "Pickup Helper" };
        private List<string> _genderoptions = new List<string> { "M", "F", "Other", "Prefer not to answer" };
        private List<string> _stateoptions = new List<string> { "AK", "AL", "AR", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "GA", "GU", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MH", "MI", "MN", "MO", "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "PR", "PW", "RI", "SC", "SD", "TN", "TX", "UT", "VA", "VI", "VT", "WA", "WI", "WV", "WY" };

        //Button Command
        public Command ResultCommand { get { return new Command(Submit); } }
        public INavigation Navigation { get; set; }

        //Picker Lists
        public List<string> ContactOptions { get { return _contactmethods; } }
        public List<string> VolunteerRoleOptions { get { return _volunteerroleoptions; } }
        public List<string> GenderOptions { get { return _genderoptions; } }
        public List<string> StateOptions { get { return _stateoptions; } }
        public DateTime DOB { get { return _dob; } set { _dob = value; OnPropertyChanged(); } }

        //Picker Selected
        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                OnPropertyChanged();
            }
        }
        public string State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }
        public string Contact
        {
            get { return _contact; }
            set
            {
                _contact = value;
                OnPropertyChanged();
            }
        }
        public string Role
        {
            get { return _role; }
            set
            {
                _role = value;
                OnPropertyChanged();
            }
        }

        //Entry Text
        public string FirstName
        {
            get { return _fName; }
            set
            {
                _fName = value;
                OnPropertyChanged();
            }
        }
        public string LastName
        {
            get { return _lName; }
            set
            {
                _lName = value;
                OnPropertyChanged();
            }
        }
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }
        public string Address2
        {
            get { return _address2; }
            set
            {
                _address2 = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }
        public string SSN
        {
            get { return _ssn; }
            set
            {
                _ssn = value;
                OnPropertyChanged();
            }
        }

        //Validation Labels for Entries
        public string VFName
        {
            get { return _vfname; }
            set
            {
                _vfname = value;
                OnPropertyChanged();
            }
        }
        public string VLName
        {
            get { return _vlname; }
            set
            {
                _vlname = value;
                OnPropertyChanged();
            }
        }
        public string VCity
        {
            get { return _vcity; }
            set
            {
                _vcity = value;
                OnPropertyChanged();
            }
        }
        public string VAddress
        {
            get { return _address; }
            set
            {
                _vaddress = value;
                OnPropertyChanged();
            }
        }
        public string VAddress2
        {
            get { return _vaddress2; }
            set
            {
                _vaddress2 = value;
                OnPropertyChanged();
            }
        }
        public string VEmail
        {
            get { return _vemail; }
            set
            {
                _vemail = value;
                OnPropertyChanged();
            }
        }
        public string VPhone
        {
            get { return _vphone; }
            set
            {
                _vphone = value;
                OnPropertyChanged();
            }
        }
        public string VSSN
        {
            get { return _vssn; }
            set
            {
                _vssn = value;
                OnPropertyChanged();
            }
        }

        private void Submit()
        {
            if (ValidateAll())
            {
                OnPropertyChanged("Result");

                DependencyService.Get<IToast>().LongAlert("Volunteer Form Submitted Successfully.");
                Navigation.PopToRootAsync();
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

            //city check
            if (!Validator.NullOrEmptyRule(_city))
            {
                VCity = "Please enter City";
                valid = false;
            }
            else
            {
                VCity = "";
            }

            //address check
            if (!Validator.NullOrEmptyRule(_address))
            {
                VAddress = "Please enter Address";
            }
            else
            {
                VAddress = "";
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
