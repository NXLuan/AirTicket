using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirTicket.ViewModel
{
    class FlightViewModel : BaseViewModel
    {
        public string airline { get; set; }
        public string flight { get; set; }
        public DateTime departureTime { get; set; }
        public DateTime landingTime { get; set; }

        public decimal priceFlight { get; set; }

        public ICommand SelectFlightCommand { get; set; }
        public FlightViewModel()
        {
            SelectFlightCommand = new RelayCommand<TabControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                TabControl tab = p as TabControl;
                TabItem tabItem = tab.Items[1] as TabItem;

                tab.SelectedIndex = 1;
                tabItem.IsEnabled = true;
            });
        }
    }
}
