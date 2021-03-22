using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirTicket.ValidRule
{
    class PassengerValidationRule : ValidationRule
    {
        public string TypePassenger { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //if (int.Parse(value as string) > 9)
            throw new NotImplementedException();
        }
    }
}
