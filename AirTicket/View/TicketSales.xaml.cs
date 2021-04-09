using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirTicket.View
{
    /// <summary>
    /// Interaction logic for TicketSales.xaml
    /// </summary>
    public partial class TicketSales : UserControl
    {
        private static TicketSales instance;
        public static TicketSales getInstance()
        {
            if (instance == null)
            {
                instance = new TicketSales();
            }
            return instance;
        }
        public TicketSales()
        {
            InitializeComponent();
            FutureDatePicker.BlackoutDates.AddDatesInPast();
        }
    }
}
