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
        ObservableCollection<DonationListItem> listItems = new ObservableCollection<DonationListItem>();
        bool prevCompleted = true;
        bool abort = false;
        int index = 1;
        int count = 0;

        public RecentItemsPage()
        {
            InitializeComponent();

            if (App.service == null)
            {
                App.InitializeService();
            }
        }

        protected override async void OnAppearing()
        {
            itemsList.ItemsSource = listItems;

            btnLoadMore.IsEnabled = false;
            await Task.Run(() => LoadMoreItems(5));
            btnLoadMore.IsEnabled = true;
        }

        private void LoadMoreItems(int number)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DependencyService.Get<IToast>().LongAlert("Loading...");
            });

            count += number;

            //gets last count items
            while (index < count && !abort)
            {
                if (prevCompleted)
                {
                    prevCompleted = false;
                    PopulateListView(index);
                    index++;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        scrollView.ScrollToAsync(0, itemsList.Height + 50, true);
                    });
                }
            }
        }

        private void PopulateListView(int index)
        {
            try
            {
                //first removes handler so that this event can only be subscribed to once
                App.service.GetItemWithImageAndDescriptionCompleted -= Service_GetItemWithImageAndDescriptionCompleted;
                App.service.GetItemWithImageAndDescriptionCompleted += Service_GetItemWithImageAndDescriptionCompleted;
                App.service.GetItemWithImageAndDescriptionAsync(index);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                abort = true;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                //itemsList.ItemsSource = null;
                //itemsList.ItemsSource = listItems;

                itemsList.HeightRequest = 100 * listItems.Count;
            });
        }

        private void Service_GetItemWithImageAndDescriptionCompleted(object sender, GetItemWithImageAndDescriptionCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                ItemInfo item = e.Result;
                if (item.Description != null)
                {
                    items.Add(item);

                    listItems.Add(new DonationListItem()
                    {
                        Description = item.Description,
                        ItemImage = ConvertToImageSource(item.Donation_Image)
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        abort = true;
                        DependencyService.Get<IToast>().ShortAlert("No more items.");
                    });
                }
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    abort = true;
                    DependencyService.Get<IToast>().ShortAlert("Unable to connect to server.");
                });
                Debug.WriteLine(e.Error);
            }

            prevCompleted = true;
        }

        private ImageSource ConvertToImageSource(byte[] byteImage)
        {
            ImageSource source = ImageSource.FromStream(() => new MemoryStream(byteImage));

            return source;
        }

        private async void btnLoadMore_Clicked(object sender, EventArgs e)
        {
            btnLoadMore.IsEnabled = false;
            await Task.Run(() => LoadMoreItems(5));
            btnLoadMore.IsEnabled = true;
        }
    }
}