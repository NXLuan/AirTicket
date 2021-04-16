using AirTicket.Model;
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
    [ContentProperty("Type")]
    public class DateOfBirthValidationRule : ValidationRule
    {
        public TypePassenger Type { get; set; }

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

            LOAIHANHKHACH lhk = Type.Value;

            if ((lhk.TuoiMax == null) && (age < lhk.TuoiMin)) return new ValidationResult(false, "Tuổi " + lhk.TenLoai + " >= " + lhk.TuoiMin);
            if ((lhk.TuoiMin == null) && (age > lhk.TuoiMax)) return new ValidationResult(false, "Tuổi " + lhk.TenLoai + " < " + (lhk.TuoiMax + 1));
            if (age > lhk.TuoiMax || age < lhk.TuoiMin) return new ValidationResult(false, lhk.TuoiMin + " <= Tuổi " + lhk.TenLoai + " < " + (lhk.TuoiMax + 1));

            return ValidationResult.ValidResult;
        }
    }

    public class TypePassenger : DependencyObject
    {
        public LOAIHANHKHACH Value
        {
            get { return (LOAIHANHKHACH)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(LOAIHANHKHACH),
            typeof(TypePassenger),
            new PropertyMetadata(default(LOAIHANHKHACH)));
    }

    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}
