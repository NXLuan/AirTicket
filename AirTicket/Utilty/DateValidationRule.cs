using System;
using System.Globalization;
using System.Windows.Controls;

namespace AirTicket.Utilty
{
    public class DateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime time;
            if (!DateTime.TryParse((value ?? "").ToString(),
                CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
                out time)) return new ValidationResult(false, "Không hợp lệ");

            return time.Date < DateTime.Now.Date
                ? new ValidationResult(false, "Không thể chọn ngày trước hiện tại")
                : ValidationResult.ValidResult;
        }
    }
}
