using AirTicket.Model;
using AirTicket.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace AirTicket.ValidRule
{
    [ContentProperty("DependencyLHK")]
    public class DateOfBirthValidationRule : ValidationRule
    {
        public TypePassenger DependencyLHK { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime birthday;
            DateTime now = DateTime.Today;
            if (!DateTime.TryParse((value ?? "").ToString(),
                CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
                out birthday)) return new ValidationResult(false, "Không hợp lệ");

            if (birthday.Date > DateTime.Now.Date)
                return new ValidationResult(false, "Không hợp lệ");

            int age = now.Year - birthday.Year;
            if (now < birthday.AddYears(age)) age--;

            LOAIHANHKHACH lhk = DependencyLHK.Value;

            if ((lhk.TuoiMax == null) && (age < lhk.TuoiMin)) return new ValidationResult(false, "Tuổi " + lhk.TenLoai + " >= " + lhk.TuoiMin);
            if ((lhk.TuoiMin == null) && (age > lhk.TuoiMax)) return new ValidationResult(false, "Tuổi " + lhk.TenLoai + " < " + (lhk.TuoiMax + 1));
            if (age > lhk.TuoiMax || age < lhk.TuoiMin) return new ValidationResult(false, lhk.TuoiMin + " <= Tuổi " + lhk.TenLoai + " < " + (lhk.TuoiMax + 1));

            return ValidationResult.ValidResult;
        }
    }

    public class TypePassenger : DependencyCustom<LOAIHANHKHACH>{}
}
