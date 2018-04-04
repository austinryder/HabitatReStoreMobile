using HabitatReStoreMobile.Interfaces;
using HabitatReStoreMobile.Models;
using HabitatReStoreMobile.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HabitatReStoreMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecentItemsPage : ContentPage
    {
        ObservableCollection<ItemInfo> items = new ObservableCollection<ItemInfo>();
        List<DonationItem> listItems = new List<DonationItem>();

        public RecentItemsPage()
        {
            InitializeComponent();

            if (App.service == null)
            {
                App.InitializeService();
            }

            PopulateListView();
        }

        private void PopulateListView()
        {
            DependencyService.Get<IToast>().ShortAlert("Loading...");

            try
            {
                //first removes handler so that this event can only be subscribed to once
                App.service.GetItemsWithImageAndDescriptionCompleted -= Service_GetItemsWithImageAndDescriptionCompleted;
                App.service.GetItemsWithImageAndDescriptionCompleted += Service_GetItemsWithImageAndDescriptionCompleted;
                App.service.GetItemsWithImageAndDescriptionAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void Service_GetItemsWithImageAndDescriptionCompleted(object sender, GetItemsWithImageAndDescriptionCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                items = e.Result;

                foreach (ItemInfo i in items)
                {
                    listItems.Add(new DonationItem()
                    {
                        Description = i.Description,
                        ItemImage = ConvertToImageSource(i.Donation_Image)
                    });
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    itemsList.ItemsSource = listItems;
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

        private ImageSource ConvertToImageSource(byte[] byteImage)
        {
            ImageSource source = ImageSource.FromStream(() => new MemoryStream(byteImage));

            return source;
        }
    }
}