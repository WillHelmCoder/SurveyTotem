using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class ImportModel
    {
        public Boolean CompanyChecked { get; set; }
        public Boolean NameChecked { get; set; }
        public Boolean TitleChecked { get; set; }
        public Boolean EmailChecked { get; set; }
        public Boolean ExtensionChecked { get; set; }
        public Boolean PhoneNumberChecked { get; set; }
        public Boolean AltPhoneNumberChecked { get; set; }
        public Boolean AddressChecked { get; set; }
        public Boolean AltAddressChecked { get; set; }
        public Boolean CityChecked { get; set; }
        public Boolean StateChecked { get; set; }
        public Boolean CountryChecked { get; set; }
        public Boolean ZipCodeChecked { get; set; }

        public Boolean AgainstDatabase { get; set; }

    }
}