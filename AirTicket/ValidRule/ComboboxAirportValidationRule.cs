using AirTicket.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirTicket.ValidRule
{
    class ComboboxAirportValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return (value == null)
                ? new ValidationResult(false, "yêu cầu chọn sân bay")
                : ValidationResult.ValidResult;
        }
    }
}
