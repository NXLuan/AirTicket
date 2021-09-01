using AirTicket.Model;
using AirTicket.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

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

        private string _flightDate;

        public string FlightDate
        {
            get => _flightDate;
            set => SetProperty(ref _flightDate, value);
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

        private ObservableCollection<FlightModel> _listFlightVM;
        public ObservableCollection<FlightModel> ListFlightVM
        {
            get => _listFlightVM;
            set => SetProperty(ref _listFlightVM, value);
        }

        private FlightModel _flightSelected;
        public FlightModel FlightSelected
        {
            get => _flightSelected;
            set
            {
                if (ListFlightVM != null && ListFlightVM.Count != 0)
                {
                    if (value != null)
                        filter.AddFilter("FlightSelected", element => ((FlightModel)element).Equals(value));
                    else filter.ClearFilters();
                }
                SetProperty(ref _flightSelected, value);
            }
        }

        public FilterSupport filter { get; set; }

        public ListFlightViewModel()
        {
            filter = new FilterSupport();
        }

        public void AddFlight(FlightModel flight)
        {
            ListFlightVM.Add(flight);
        }

        public void Load()
        {
            if (ListFlightVM != null)
                ListFlightVM = null;
            IsLoading = "Visible";
            IsNoResult = "Hidden";
        }

        public void Done()
        {
            IsLoading = "Hidden";

            if (ListFlightVM != null && ListFlightVM.Count != 0)
                filter.FilterView = (CollectionView)CollectionViewSource.GetDefaultView(ListFlightVM);
            else IsNoResult = "Visible";
        }
    }
}
