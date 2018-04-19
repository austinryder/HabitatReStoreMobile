using HabitatReStoreMobile.Interfaces;
using HabitatReStoreMobile.Models;
using HabitatReStoreMobile.ServiceReference1;
using Plugin.Media.Abstractions;
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
        List<DonationListItem> listItems = new List<DonationListItem>();
        ObservableCollection<ItemInfo> items = new ObservableCollection<ItemInfo>();
        byte[] bytearrayimage;
        DonorInfo donor;
        int donationID;
        bool abort = false;
        bool success = false;
        bool prevCompleted;
        string message = "";

        public DonationPickupFormPage(DonorInfo d)
        {
            InitializeComponent();

            if (App.service == null)
            {
                App.InitializeService();
            }

            donor = d;

            if (itemCategories == null)
                InitializePickers();
        }

        private void btnAddItem_Clicked(object sender, EventArgs e)
        {
            ItemInfo newItem = new ItemInfo();
            newItem = GetItem();
            items.Add(newItem);

            listItems.Add(new DonationListItem()
            {
                Category = pickItemCategory.SelectedItem.ToString(),
                Description = newItem.Description,
                ItemImage = imgDonationPhoto.Source
            });

            //reset item source - solves issue with only displaying first item
            itemsList.ItemsSource = null;
            itemsList.ItemsSource = listItems;
            itemsList.HeightRequest = (60 * listItems.Count);

            //clear items entries for next item
            imgDonationPhoto.Source = "noimage.png";
            entDescription.Text = "";
            pickItemCategory.SelectedIndex = 0;
        }

        private void btnSubmit_Clicked(object sender, EventArgs e)
        {
            DonationInfo donation = GetDonation();
            DonationPickupInfo pickupInfo = GetPickupInfo();

            if (ValidateAll(donation, pickupInfo))
            {
                btnSubmit.IsEnabled = false;
                DependencyService.Get<IToast>().ShortAlert("Submitting... please wait.");

                InsertDonationToDatabase(donation, items, pickupInfo);

                InsertItems(items);
            }
        }

        private void InsertDonationToDatabase(DonationInfo donation, ObservableCollection<ItemInfo> items, DonationPickupInfo pickupInfo)
        {
            try
            {
                //first removes handler so that this event can only be subscribed to once
                App.service.InsertDonationCompleted -= Service_InsertDonationCompleted;
                App.service.InsertDonationCompleted += Service_InsertDonationCompleted;
                App.service.InsertDonationAsync(donation, pickupInfo);
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
            if (e.Error == null)
            {
                prevCompleted = true;
                donationID = e.Result;
                success = true;
                message = "Donation pickup form successfully submitted.";
            }
            else
            {
                abort = true;
                success = false;
                message = "Error submitting donation pickup form.";
                
                Debug.WriteLine(e.Error);
            }
        }

        private void InsertItems(ObservableCollection<ItemInfo> items)
        {
            try
            {
                //first removes handler so that this event can only be subscribed to once
                App.service.InsertItemCompleted -= Service_InsertItemCompleted;
                App.service.InsertItemCompleted += Service_InsertItemCompleted;

                int counter = 0;

                //must loop to wait for other inserts to complete, if error occurs, aborts.
                while (counter < items.Count && !abort)
                {
                    if (prevCompleted)
                    {
                        prevCompleted = false;
                        App.service.InsertItemAsync(items[counter], donationID);
                        counter++;
                    }
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().LongAlert("Error, unable to access server.");
                btnSubmit.IsEnabled = true;
                Debug.WriteLine(ex);
            }

            DependencyService.Get<IToast>().LongAlert(message);

            if (success)
            {
                Navigation.PopToRootAsync();
            }
        }

        private void Service_InsertItemCompleted(object sender, InsertItemCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                success = true;
                prevCompleted = true;
                message = "Donation pickup form successfully submitted";
            }
            else
            {
                abort = true;
                success = false;
                message = "Error submitting donation pickup form.";
                Debug.WriteLine(e.Error);

                Device.BeginInvokeOnMainThread(() =>
                {
                    edtSpecialInstructions.Text = e.Error.ToString();
                });
            }
        }

        private ItemInfo GetItem()
        {
            ItemInfo newItem = new ItemInfo();

            if (bytearrayimage != null)
            {
                newItem.Donation_Image = bytearrayimage;
            }

            newItem.Description = entDescription.Text;
            newItem.Item_Category_ID = itemCategories[pickItemCategory.SelectedIndex].Item_Category_ID;

            return newItem;
        }

        private DonationInfo GetDonation()
        {
            DonationInfo donation = new DonationInfo();

            donation.Donor_ID = donor.Donor_ID;
            donation.Address = entAddress.Text;
            donation.Address2 = entAddress2.Text;
            donation.City = entCity.Text;
            donation.State = pickState.SelectedItem.ToString();
            donation.ZipCode = entZIP.Text;
            donation.Store_ID = GetStoreByZIP();
            donation.Status_Map_ID = 3;
            donation.Bypass_Flag = false;

            return donation;
        }

        private DonationPickupInfo GetPickupInfo()
        {
            DonationPickupInfo pickupInfo = new DonationPickupInfo();

            DateTime day = new DateTime();
            DateTime windowStart = new DateTime();
            DateTime windowEnd = new DateTime();

            day = pickDay.Date.Date;
            windowStart = day + pickTimeStart.Time;
            windowEnd = day + pickTimeEnd.Time;

            pickupInfo.PickupWindowStart = windowStart;
            pickupInfo.PickupWindowEnd = windowEnd;
            pickupInfo.SpecialInstructions = edtSpecialInstructions.Text;

            return pickupInfo;
        }

        //TO-DO: GET CORRECT STORE BY ZIPCODE
        private int GetStoreByZIP()
        {
            return 1;
        }

        private bool ValidateAll(DonationInfo newDonation, DonationPickupInfo newPickUp)
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

            //check pickup times
            if (newPickUp.PickupWindowStart > newPickUp.PickupWindowEnd)
            {
                lblVTimeWindow.Text = "Pickup window start must be before end";
                valid = false;
            }
            else if (newPickUp.PickupWindowStart.Date <= DateTime.Today)
            {
                lblVTimeWindow.Text = "Pickup must be a date in the future";
                valid = false;
            }
            else
            {
                lblVTimeWindow.Text = "";
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
                    PhotoSize = PhotoSize.Small,
                    CompressionQuality = 30,
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
                PhotoSize = PhotoSize.Small,
                CompressionQuality = 30
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
                //first removes handler so that this event can only be subscribed to once
                App.service.GetItemCategoriesCompleted -= Service_GetItemCategoriesCompleted;
                App.service.GetItemCategoriesCompleted += Service_GetItemCategoriesCompleted;
                App.service.GetItemCategoriesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void Service_GetItemCategoriesCompleted(object sender, GetItemCategoriesCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                itemCategories = e.Result;
                pickItemCategory.Items.Clear();
                foreach (ItemCategory cat in itemCategories)
                {
                    pickItemCategory.Items.Add(cat.Description);
                }
                pickItemCategory.SelectedIndex = 0;
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