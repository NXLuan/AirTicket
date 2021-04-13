using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirTicket.ValidRule
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public string contentError { get; set; } = "Yêu cầu nhập thông tin";
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
               ? new ValidationResult(false, contentError)
               : ValidationResult.ValidResult;
        }
    }
}
