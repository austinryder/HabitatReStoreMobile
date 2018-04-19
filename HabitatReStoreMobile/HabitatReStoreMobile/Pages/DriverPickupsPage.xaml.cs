using HabitatReStoreMobile.Interfaces;
using HabitatReStoreMobile.Models;
using HabitatReStoreMobile.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;

namespace HabitatReStoreMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DriverPickupsPage : ContentPage
    {
        List<DonationAndPickup> donationsAndPickups = new List<DonationAndPickup>();
        DriverInfo driver;
        DonationAndPickup selected;

        public DriverPickupsPage(DriverInfo d)
        {
            InitializeComponent();

            driver = d;
            lblDriver.Text = "Welcome, " + driver.First_Name + " " + driver.Last_Name;
        }

        private void btnEnter_Clicked(object sender, EventArgs e)
        {
            int donationID;
 
            if (int.TryParse(entDonationID.Text, out donationID))
            {
                GetPickup(donationID);
            }
            else
            {
                DependencyService.Get<IToast>().ShortAlert("Invalid Donation ID");
            }
        }

        private void GetPickup(int donationID)
        {
            btnEnter.IsEnabled = false;

            try
            {
                //first removes handler so that this event can only be subscribed to once
                App.service.GetDonationAndPickupFromIDCompleted -= Service_GetDonationAndPickupFromIDCompleted;
                App.service.GetDonationAndPickupFromIDCompleted += Service_GetDonationAndPickupFromIDCompleted;
                App.service.GetDonationAndPickupFromIDAsync(donationID);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().LongAlert("Error, unable to access server.");
                btnEnter.IsEnabled = true;
                Debug.WriteLine(ex);
            }
           
        }

        private void Service_GetDonationAndPickupFromIDCompleted(object sender, GetDonationAndPickupFromIDCompletedEventArgs e)
        {
            bool success = false;
            string message = "";
            DonationAndPickup result = new DonationAndPickup();

            if (e.Error == null)
            {
                result = e.Result;

                if (result.Donation != null)
                {
                    donationsAndPickups.Add(result);
                    success = true;
                    message = "Donation pickup added";
                }
                else
                {
                    success = false;
                    message = "Donation was not found";
                }
            }
            else
            {
                success = false;
                message = "Error retrieving donation information.";
                Debug.WriteLine(e.Error);
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                DependencyService.Get<IToast>().ShortAlert(message);

                btnEnter.IsEnabled = true;

                if (success)
                {
                    RefreshPickupsList();
                }
            });
        }

        private void RefreshPickupsList()
        {
            pickupsList.ItemsSource = null;
            int count = 1;

            List<DonationPickupListItem> listPickups = new List<DonationPickupListItem>();
            
            foreach (DonationAndPickup dap in donationsAndPickups)
            {
                listPickups.Add(new DonationPickupListItem()
                {
                    DonationNumber = "Donation #" + count.ToString(),
                    DonationID = dap.Donation.Donation_ID,
                    Address = dap.Donation.Address,
                    Address2 = dap.Donation.Address2,
                    City = dap.Donation.City,
                    State = dap.Donation.State,
                    ZipCode = dap.Donation.ZipCode,
                    Date = dap.Pickup.PickupWindowStart.Date.ToString("MM/dd/yyyy"),
                    PickupStart = dap.Pickup.PickupWindowStart.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture),
                    PickupEnd = dap.Pickup.PickupWindowEnd.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture),
                    SpecialInstructions = dap.Pickup.SpecialInstructions
                });
                count++;
            }

            pickupsList.ItemsSource = listPickups;
        }

        private void btnCompleted_Clicked(object sender, EventArgs e)
        {
            var thisButton = (Button)sender;
            DonationPickupListItem selectedItem = (DonationPickupListItem)thisButton.BindingContext;

            int donationID;
            DateTime currentTime;

            selectedItem = selectedItem;
            donationID = selectedItem.DonationID;

            selected = donationsAndPickups.First(item => item.Donation.Donation_ID == donationID);

            currentTime = DateTime.Now;

            this.IsEnabled = false;
            UpdateCompleted(selected, currentTime);
            this.IsEnabled = true;
        }

        private void UpdateCompleted(DonationAndPickup selected, DateTime time)
        {
            try
            {
                //first removes handler so that this event can only be subscribed to once
                App.service.UpdateCompletedDonationPickupCompleted -= Service_UpdateCompletedDonationPickupCompleted;
                App.service.UpdateCompletedDonationPickupCompleted += Service_UpdateCompletedDonationPickupCompleted;
                App.service.UpdateCompletedDonationPickupAsync(selected.Donation, selected.Pickup, time);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().LongAlert("Error, unable to access server.");
                btnEnter.IsEnabled = true;
                Debug.WriteLine(ex);
            }
        }

        private void Service_UpdateCompletedDonationPickupCompleted(object sender, UpdateCompletedDonationPickupCompletedEventArgs e)
        {
            bool success = false;
            string message = "";
            int result;

            if (e.Error == null)
            {
                result = e.Result;

                success = true;
                message = "Pickup marked completed. Removing from list...";
            }
            else
            {
                success = false;
                message = "Error completing donation pickup.";
                Debug.WriteLine(e.Error);
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                DependencyService.Get<IToast>().ShortAlert(message);

                if (success)
                {
                    donationsAndPickups.Remove(selected);
                    RefreshPickupsList();
                }
            });
        }
    }
}