using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HabitatReStoreMobile.Models
{
    class Validator
    {
        public static bool NullOrEmptyRule(string value)
        {
            if (value == null)
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(value);
        }
        public static bool EmailCheck(string value)
        {
            if (value == null)
            {
                return false;
            }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(value);

            return match.Success;
        }

        public static bool PhoneCheck(string value)
        {
            if (value == null)
            {
                return false;
            }

            Regex regex = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
            Match match = regex.Match(value);

            return match.Success;
        }

        public static bool SSNCheck(string value)
        {
            if (value == null)
            {
                return false;
            }

            Regex regex = new Regex(@"^(?!219099999|078051120)(?!666|000|9\d{2})\d{3}(?!00)\d{2}(?!0{4})\d{4}$");
            Match match = regex.Match(value);

            return match.Success;
        }

        public static bool ZIPCheck(string value)
        {
            if (value == null)
            {
                return false;
            }

            Regex regex = new Regex(@"^\d{5}(?:[-\s]\d{4})?$");
            Match match = regex.Match(value);

            return match.Success;
        }
    }
}
