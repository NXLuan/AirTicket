using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirTicket.ValidRule
{
    class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                MailAddress m = new MailAddress(value.ToString());

                return ValidationResult.ValidResult;
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "Email không hợp lệ"); ;
            }
        }
    }
}
