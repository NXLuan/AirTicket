using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicket.ViewModel
{
    class DetailInfoViewModel : BaseViewModel
    {
        public int num { get; set; }

        public string typePassenger { get; set; }

        public string Name { get; set; }

        public string Sex { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DetailInfoViewModel()
        {
            DateOfBirth = DateTime.Today;
        }
    }
}
