using HabitatReStoreMobile.Interfaces;
using HabitatReStoreMobile.Models;
using HabitatReStoreMobile.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HabitatReStoreMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DonationPickupFormPage : ContentPage
    {
        ObservableCollection<ItemCategory> itemCategories;
        byte[] bytearrayimage;
        DonorInfo donor;
        ItemInfo item;
        DonationInfo donation;

        public DonationPickupFormPage(DonorInfo d)
        {
            InitializeComponent();

            if (App.service.State == null)
            {
                App.InitializeService();
            }

            InitializePickers();

            donor = d;
        }

        private void btnSubmit_Clicked(object sender, EventArgs e)
        {
            item = new ItemInfo();
            donation = new DonationInfo();

            GetItem();
            GetDonation();

            if (ValidateAll(donation))
            {
                btnSubmit.IsEnabled = false;
                DependencyService.Get<IToast>().ShortAlert("Submitting... please wait.");

                InsertDonationToDatabase(donation, item);
            }
        }

        private void InsertDonationToDatabase(DonationInfo donation, ItemInfo item)
        {
            try
            {
                App.service.InsertDonationCompleted += new EventHandler<InsertDonationCompletedEventArgs>(Service_InsertDonationCompleted);
                App.service.InsertDonationAsync(donation, item);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().LongAlert("Error, unable to access server.");
                btnSubmit.IsEnabled = true;
                Debug.WriteLine(ex);
            }
        }

        private void Service_InsertDonationCompleted(object sender, InsertDonationCompletedEventArgs e)
        {
            bool success = false;
            string message = "";

            if (e.Error == null)
            {
                int result = e.Result;
                if (result == 1)
                {
                    success = true;
                    message = "Donation pickup form successfully submitted.";
                }
                else
                {
                    success = false;
                    message = "Error submitting donation pickup form.";
                }
            }
            else
            {
                success = false;
                message = "Error submitting donation pickup form.";
                Debug.WriteLine(e.Error);
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                OnCompleted(success, message);
            });
        }

        private void OnCompleted(bool success, string message)
        {
            DependencyService.Get<IToast>().LongAlert(message);

            if (success)
            {
                Navigation.PopToRootAsync();
            }

            btnSubmit.IsEnabled = true;
        }

        private void GetItem()
        {
            if(bytearrayimage != null)
            {
                item.Donation_Image = bytearrayimage;
            }

            item.Item_Category_ID = itemCategories[pickItemCategory.SelectedIndex].Item_Category_ID;
        }

        private void GetDonation()
        {
            donation.Donor_ID = donor.Donor_ID;
            donation.Address = entAddress.Text;
            donation.Address2 = entAddress2.Text;
            donation.City = entCity.Text;
            donation.State = pickState.SelectedItem.ToString();
            donation.ZipCode = entZIP.Text;
            donation.Store_ID = GetStoreByZIP();
            donation.Status_Map_ID = 3;
            donation.Bypass_Flag = false;
        }

        //TO-DO: GET CORRECT STORE BY ZIPCODE
        private int GetStoreByZIP()
        {
            return 1;
        }
        private bool ValidateAll(DonationInfo newDonation)
        {
            bool valid = true;

            //address check
            if (!Validator.NullOrEmptyRule(newDonation.Address))
            {
                lblVAddress.Text = "Please enter Address";
                valid = false;
            }
            else
            {
                lblVAddress.Text = "";
            }

            //zip check
            if (!Validator.NullOrEmptyRule(newDonation.ZipCode))
            {
                lblVZIP.Text = "Please enter ZIP Code";
                valid = false;
            }
            else if (!Validator.ZIPCheck(newDonation.ZipCode))
            {
                lblVZIP.Text = "Invalid ZIP Code";
                valid = false;
            }
            else
            {
                lblVZIP.Text = "";
            }

            return valid;
        }


        //Open camera and take photo
        private async void btnTakePhoto_Clicked(object sender, EventArgs e)
        {
            if (!Plugin.Media.CrossMedia.Current.IsCameraAvailable || !Plugin.Media.CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No camera is avaialble.", "OK");
            }
            else
            {
                var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                    CompressionQuality = 50,
                    SaveToAlbum = true
                });

                if (photo != null)
                {
                    imgDonationPhoto.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                    bytearrayimage = ReadToByteArray(photo.GetStream());
                }
            }
        }

        //Select photo from gallery
        private async void btnSelectPhoto_Clicked(object sender, EventArgs e)
        {
            var photo = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions()
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                CompressionQuality = 50
            });

            if (photo != null)
            {
                imgDonationPhoto.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                bytearrayimage = ReadToByteArray(photo.GetStream());
            }
        }

        //Convert image to a byte array
        private byte[] ReadToByteArray(Stream image)
        {
            MemoryStream ms = new MemoryStream();
            image.CopyTo(ms);
            return ms.ToArray();
        }

        private void InitializePickers()
        {
            List<string> stateOptions = new List<string> { "AK", "AL", "AR", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "GA", "GU", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MH", "MI", "MN", "MO", "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "PR", "PW", "RI", "SC", "SD", "TN", "TX", "UT", "VA", "VI", "VT", "WA", "WI", "WV", "WY" };
            pickState.ItemsSource = stateOptions;
            pickState.SelectedItem = "NC";

            //get possible values for item category
            try
            {
                App.service.GetItemCategoriesCompleted += new EventHandler<GetItemCategoriesCompletedEventArgs>(Service_GetItemCategoriesCompleted);
                App.service.GetItemCategoriesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void Service_GetItemCategoriesCompleted(object sender, GetItemCategoriesCompletedEventArgs e)
        {
            try
            {
                itemCategories = e.Result;
                pickItemCategory.Items.Clear();
                foreach (ItemCategory cat in itemCategories)
                {
                    pickItemCategory.Items.Add(cat.Description);
                }
                pickItemCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
            }
        }

        //Add address information from the Donor
        private void swUseDonorAddress_Toggled(object sender, ToggledEventArgs e)
        {
            if (swUseDonorAddress.IsToggled)
            {
                entAddress.Text = donor.Address;
                entAddress2.Text = donor.Address2;
                entCity.Text = donor.City;
                pickState.SelectedItem = donor.State;
                entZIP.Text = donor.ZipCode;
            }
            else
            {
                entAddress.Text = "";
                entAddress2.Text = "";
                entCity.Text = "";
                pickState.SelectedItem = "";
                entZIP.Text = "";
            }
        }
    }
}