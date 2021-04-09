using AirTicket.View;
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
    public class MainViewModel : BaseViewModel
    {
        private UserControl _currentScreen;
        public UserControl CurrentScreen
        {
            get => _currentScreen;
            set => SetProperty(ref _currentScreen, value);
        }
        public ObservableCollection<MenuViewModel> Menu { get; set; }
        public ICommand SelectedItemCommand { get; set; }
        public MainViewModel()
        {
            Menu = new ObservableCollection<MenuViewModel>()
            {
                new MenuViewModel(){ TypeItem = MenuViewModel.TypeControl.TICKETSALES, Icon="TicketConfirmation", Name="BÁN VÉ" },
                 new MenuViewModel(){  TypeItem = MenuViewModel.TypeControl.TICKETSALES, Icon="ViewListOutline", Name="DANH SÁCH VÉ" },
                  new MenuViewModel(){  TypeItem = MenuViewModel.TypeControl.TICKETSALES, Icon="ChartDonutVariant", Name="THỐNG KÊ" },
                   new MenuViewModel(){  TypeItem = MenuViewModel.TypeControl.TICKETSALES, Icon="ShieldEdit", Name="QUY ĐỊNH" },
            };

            SelectedItemCommand = new RelayCommand<int>((p) => { return true; }, (p) =>
            {
                switch (Menu[p].TypeItem)
                {
                    case MenuViewModel.TypeControl.TICKETSALES:
                        CurrentScreen = TicketSales.getInstance();
                        break;
                }
            });
        }
    }
}
