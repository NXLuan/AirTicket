using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirTicket.Utilities
{
    public class DependencyCustom<T> : DependencyObject
    {
        public T Value
        {
            get { return (T)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(T),
            typeof(DependencyCustom<T>),
            new PropertyMetadata(default(T)));
    }
}
