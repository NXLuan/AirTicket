using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicket.Model
{
    public class DataFlightRequest
    {
        public int Adt { get; set; }
        public int Chd { get; set; }
        public int Inf { get; set; }
        public SearchFlightInfo[] ListFlight { get; set; }
        public string ProductKey { get; set; }
        public bool ViewMode { get; set; }
    }
}
