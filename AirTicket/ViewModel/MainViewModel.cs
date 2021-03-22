using AirTicket.Model;
using AirTicket.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirTicket.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public UserControl currentScreen { get; set; }
        public ObservableCollection<ItemMenuModel> menuItems { get; set; }
        public MainViewModel()
        {
            menuItems = new ObservableCollection<ItemMenuModel>()
            {
                new ItemMenuModel(){ Icon="TicketConfirmation", Name="BÁN VÉ" },
                 new ItemMenuModel(){ Icon="ViewListOutline", Name="DANH SÁCH VÉ" },
                  new ItemMenuModel(){ Icon="ChartDonutVariant", Name="THỐNG KÊ" },
                   new ItemMenuModel(){ Icon="ShieldEdit", Name="QUY ĐỊNH" },
            };
            currentScreen = new TicketSales();
        }
    }
}
