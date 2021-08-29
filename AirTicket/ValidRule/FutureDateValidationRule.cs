using AirTicket.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;

namespace AirTicket.ValidRule
{
    [ContentProperty("start")]
    public class FutureDateValidationRule : ValidationRule
    {
        public DateStart start { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime time;
            if (!DateTime.TryParse((value ?? "").ToString(),
                CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
                out time)) return new ValidationResult(false, "Không hợp lệ");

            var dateStart = start == null ? DateTime.Now.Date : start.Value.Date;
            string messError = start == null ? "Không thể chọn ngày trước hiện tại" : "Không thể chọn trước ngày đi";
            return time.Date < dateStart
                ? new ValidationResult(false, messError)
                : ValidationResult.ValidResult;
        }
    }

    public class DateStart : DependencyCustom<DateTime> { }
}
