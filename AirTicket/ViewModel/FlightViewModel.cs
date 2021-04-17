using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirTicket.ViewModel
{
    class FlightViewModel : BaseViewModel
    {
        public string AirlineID { get; set; }
        private string _imgAirlineUrl;
        public string ImgAirlineUrl
        {
            get => _imgAirlineUrl;
            set
            {
                if (_imgAirlineUrl != value) AirlineID = value.Substring(value.Length - 6, 2);
                SetProperty(ref _imgAirlineUrl, value);
            }
        }
        public string flight { get; set; }
        public string departureTime { get; set; }
        public string landingTime { get; set; }

        public string priceFlight { get; set; }

        public ICommand SelectFlightCommand { get; set; }

        public FlightViewModel()
        {
            SelectFlightCommand = new RelayCommand<TabControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                TabControl tab = p as TabControl;
                TabItem tabItem1 = tab.Items[1] as TabItem;
                TabItem tabItem0 = tab.Items[0] as TabItem;

                tab.SelectedIndex = 1;
                tabItem1.IsEnabled = true;
                tabItem0.IsEnabled = false;

                InfoPaViewModel infoPaVM = tabItem1.DataContext as InfoPaViewModel;
                infoPaVM.FlightSelected = this;
                infoPaVM.tabControl = tab;
            });
        }
    }
}
