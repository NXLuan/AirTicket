using AirTicket.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicket.ViewModel
{
    class TicketSalesViewModel: BaseViewModel
    {
        public DateTime? _validatingDate;
        public ObservableCollection<PassengerModel> Passengers { get; set; }
        public TicketSalesViewModel()
        {
            ValidatingDate = DateTime.Now.Date;
            Passengers = new ObservableCollection<PassengerModel>()
            {
                new PassengerModel(){ typePassenger="Người lớn"},
                 new PassengerModel(){ typePassenger="Trẻ em"},
                  new PassengerModel(){ typePassenger="Em bé"},
            };
        }

        public DateTime? ValidatingDate
        {
            get => _validatingDate;
            set => SetProperty(ref _validatingDate, value);
        }
    }
}
