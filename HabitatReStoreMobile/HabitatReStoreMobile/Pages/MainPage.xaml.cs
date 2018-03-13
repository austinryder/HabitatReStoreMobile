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
            var menuList = new List<MasterPageNavItem>();

            var pageHome = new MasterPageNavItem() { Title = "Home", Icon = "icHome.png", TargetType = typeof(HomePage) };
            var pageFindReStore = new MasterPageNavItem() { Title = "Find a ReStore", Icon = "icFind.png", TargetType = typeof(FindReStorePage) };
            var pageDonationHome = new MasterPageNavItem() { Title = "Donate", Icon = "icDonate.png", TargetType = typeof(DonationHomePage) };
            var pageVolunteerHome = new MasterPageNavItem() { Title = "Volunteer", Icon = "icVolunteer.png", TargetType = typeof(VolunteerHomePage) };

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
            var item = (MasterPageNavItem)e.SelectedItem;
            var title = item.Title;
            Type page = item.TargetType;

            var newpage = (Page)Activator.CreateInstance(page);
            newpage.Title = item.Title;

            Detail = new NavigationPage(newpage) { BarBackgroundColor = Color.FromHex("#51B948"), BarTextColor = Color.White };
            IsPresented = false;
        }
    }
}
