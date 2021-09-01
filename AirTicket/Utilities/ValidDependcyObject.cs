using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AirTicket.Utilities
{
    class ValidDependcyObject
    {
        public static bool IsValid(DependencyObject obj)
        {
            if (Validation.GetHasError(obj))
                return false;

            // Validate all the bindings on the children
            for (int i = 0; i != VisualTreeHelper.GetChildrenCount(obj); ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (!IsValid(child)) { return false; }
            }

            return true;
        }
    }
}
