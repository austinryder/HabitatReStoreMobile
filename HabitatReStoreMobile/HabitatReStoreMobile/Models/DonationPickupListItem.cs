using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitatReStoreMobile.Models
{
    public class DonationPickupListItem
    {
        public string DonationNumber { get; set; }
        public int DonationID { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PickupStart { get; set; }
        public string PickupEnd { get; set; }
        public string Date { get; set; }
        public string SpecialInstructions { get; set; }
    }
}
