using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicket.Model
{
    class FlightModel
    {
        public string AirlineID { get; set; }
        public string ImgAirlineUrl { get; set; }
        //private string _imgAirlineUrl;
        //public string ImgAirlineUrl
        //{
        //    get => _imgAirlineUrl;
        //    set
        //    {
        //        if (_imgAirlineUrl != value) AirlineID = value.Substring(value.Length - 6, 2);
        //        SetProperty(ref _imgAirlineUrl, value);
        //    }
        //}
        public string Flight { get; set; }
        public string DepartureTime { get; set; }
        public string LandingTime { get; set; }

        public string PriceFlight { get; set; }
        public decimal ChildrenPrice { get; set; }
        public decimal InfantPrice { get; set; }
        
        public string StartDate { get; set; }

    }
}
