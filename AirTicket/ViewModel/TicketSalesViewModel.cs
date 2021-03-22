using AirTicket.Model;
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
    class TicketSalesViewModel : BaseViewModel
    {
        public DateTime? _validatingDate;
        public ObservableCollection<PassengerViewModel> listPassengerVM { get; }
        public ObservableCollection<FlightViewModel> listFlightVM { get; }
        public ICommand ReturnCommand { get; set; }
        public TicketSalesViewModel()
        {
            ValidatingDate = DateTime.Now.Date;
            listPassengerVM = new ObservableCollection<PassengerViewModel>()
            {
                new PassengerViewModel() { typePassenger = "Người lớn", regulationAge = "12 tuổi trở lên", NumberOfPassenger = 1 },
                new PassengerViewModel() { typePassenger = "Trẻ em", regulationAge = "Từ 12 tuổi đến dưới 12 tuổi", NumberOfPassenger = 0 },
                new PassengerViewModel() { typePassenger = "Em bé", regulationAge = "Nhỏ hơn 2 tuổi", NumberOfPassenger = 0 }
            };

            listFlightVM = new ObservableCollection<FlightViewModel>()
            {
                new FlightViewModel(){airline = "Vietjet Air", flight = "VJ135", departureTime=DateTime.Now, landingTime = DateTime.Now, priceFlight = 600000 },
                new FlightViewModel(){airline = "Vietnam Airlines", flight = "VN6003", departureTime=DateTime.Now, landingTime = DateTime.Now, priceFlight = 545000 }
            };

            ReturnCommand = new RelayCommand<TabControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                TabControl tab = p as TabControl;
                TabItem tabItem = tab.Items[1] as TabItem;

                tabItem.IsEnabled = false;
            });
        }

        public DateTime? ValidatingDate
        {
            get => _validatingDate;
            set => SetProperty(ref _validatingDate, value);
        }
    }
}
