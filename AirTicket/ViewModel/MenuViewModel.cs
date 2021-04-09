using AirTicket.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirTicket.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        public enum TypeControl { TICKETSALES}
        public TypeControl TypeItem { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public MenuViewModel()
        {
        }
    }
}
