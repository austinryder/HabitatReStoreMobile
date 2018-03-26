using HabitatReStoreMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HabitatReStoreMobile.Pages
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            //initialize navigation menu
            var menuList = new List<NavigationItem>();

            var pageHome = new NavigationItem() { Title = "Home", Icon = "icHome.png", Page = typeof(HomePage) };
            var pageFindReStore = new NavigationItem() { Title = "Find a ReStore", Icon = "icFind.png", Page = typeof(FindReStorePage) };
            var pageDonationHome = new NavigationItem() { Title = "Donate", Icon = "icDonate.png", Page = typeof(DonationHomePage) };
            var pageVolunteerHome = new NavigationItem() { Title = "Volunteer", Icon = "icVolunteer.png", Page = typeof(VolunteerHomePage) };

            menuList.Add(pageHome);
            menuList.Add(pageFindReStore);
            menuList.Add(pageDonationHome);
            menuList.Add(pageVolunteerHome);

            navigationDrawerList.ItemsSource = menuList;

            var newpage = (Page)Activator.CreateInstance(typeof(HomePage));
            newpage.Title = pageHome.Title;
            Detail = new NavigationPage(newpage) { BarBackgroundColor = Color.FromHex("#51B948"), BarTextColor = Color.White };
        }

        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (NavigationItem)e.SelectedItem;
            var title = item.Title;
            Type page = item.Page;

            var newpage = (Page)Activator.CreateInstance(page);
            newpage.Title = item.Title;

            Detail = new NavigationPage(newpage) { BarBackgroundColor = Color.FromHex("#51B948"), BarTextColor = Color.White };
            IsPresented = false;
        }
    }
}
