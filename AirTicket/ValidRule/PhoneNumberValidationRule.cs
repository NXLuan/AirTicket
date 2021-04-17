using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirTicket.ValidRule
{
    class PhoneNumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return Regex.Match(value.ToString(), @"^(0\d{9})|^(\+84\d{9})").Success
               ? ValidationResult.ValidResult
               : new ValidationResult(false, "Số điện thoại Không hợp lệ");
        }
    }
}
