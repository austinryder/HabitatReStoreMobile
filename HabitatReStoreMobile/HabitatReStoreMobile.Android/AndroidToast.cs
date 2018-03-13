using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HabitatReStoreMobile.Interfaces;
using HabitatReStoreMobile.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidToast))]
namespace HabitatReStoreMobile.Droid
{
    class AndroidToast : IToast
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}