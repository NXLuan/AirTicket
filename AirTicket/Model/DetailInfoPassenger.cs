using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicket.Model
{
    class DetailInfoPassenger
    {
        public int num { get; set; }
        public VECHUYENBAY VCBModel { get; set; }

        public DetailInfoPassenger()
        {
            VCBModel = new VECHUYENBAY();
            VCBModel.NgaySinh = DateTime.Today;
        }
    }
}
