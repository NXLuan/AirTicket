using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicket.ViewModel
{
    class ListFlightViewModel : BaseViewModel
    {
        private string _departure;

        public string Departure
        {
            get => _departure;
            set => SetProperty(ref _departure, value);
        }

        private string _destination;

        public string Destination
        {
            get => _destination;
            set => SetProperty(ref _destination, value);
        }

        private DateTime? _dateDeparture;

        public DateTime? DateDeparture
        {
            get => _dateDeparture;
            set => SetProperty(ref _dateDeparture, value);
        }

        private string _isLoading;

        public string IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private string _isNoResult;

        public string IsNoResult
        {
            get => _isNoResult;
            set => SetProperty(ref _isNoResult, value);
        }

        public ObservableCollection<FlightViewModel> ListFlightVM { get; set; }

        public ListFlightViewModel()
        {
            Done();
            IsNoResult = "Hidden";
            Departure = "Hà Nội";
            Destination = "Đà Nẵng";
            DateDeparture = DateTime.Today;
            ListFlightVM = new ObservableCollection<FlightViewModel>();
            ListFlightVM.Add(new FlightViewModel() { ImgAirlineUrl = "https://plugin.datacom.vn/Resources/Images/Airline/vu.gif", priceFlight = "600,000 VND" });
        }

        public void Load()
        {
            if (ListFlightVM.Count != 0)
                ListFlightVM.Clear();
            IsLoading = "Visible";
            IsNoResult = "Hidden";
        }

        public void Done()
        {
            IsLoading = "Hidden";
        }
    }
}
