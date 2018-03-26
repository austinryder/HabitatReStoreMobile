using HabitatReStoreMobile.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HabitatReStoreMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DonationPickupFormPage : ContentPage
    {
        byte[] bytearrayimage;
        public DonationPickupFormPage()
        {
            InitializeComponent();
        }

        private void btnSubmit_Clicked(object sender, EventArgs e)
        {
            //if (ValidateAll())
            //{
            OnPropertyChanged("SubmitCommand");

            DisplayAlert("Success", "Pickup Request Form Submitted Successfully.", "OK");
            //DependencyService.Get<IToast>().LongAlert("Pickup Request Form Submitted Successfully.");
            Navigation.PopToRootAsync();


            //}
        }

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

        private byte[] ReadToByteArray(Stream image)
        {
            MemoryStream ms = new MemoryStream();
            image.CopyTo(ms);
            return ms.ToArray();
        }
    }
}