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

            if (Type.Value == "Người lớn" && age < 12) return new ValidationResult(false, "Tuổi người lớn >= 12");
            if (Type.Value == "Trẻ em" && (age >= 12 || age < 2)) return new ValidationResult(false, "2 <= Tuổi trẻ em < 12");
            if (Type.Value == "Em bé" && (age >= 2)) return new ValidationResult(false, "Tuổi em bé < 2");
            return ValidationResult.ValidResult;
        }
    }

    public class TypePassenger : DependencyObject
    {
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(string),
            typeof(TypePassenger),
            new PropertyMetadata(default(string)));
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
