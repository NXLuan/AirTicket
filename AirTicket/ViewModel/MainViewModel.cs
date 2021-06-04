using AirTicket.Model;
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
        public ObservableCollection<CHUCNANG> MenuFunction { get; set; }
        public ObservableCollection<CHUCNANG> MenuAccount { get; set; }
        public ICommand SelectedItemFunctionCommand { get; set; }
        public MainViewModel()
        {
            MenuFunction = new ObservableCollection<CHUCNANG>();
            var NhomNguoiDung = DataProvider.Instance.DB.NHOMNGUOIDUNGs.Where(x => x.MaNhom == "NV").First();

            foreach (CHUCNANG CN in NhomNguoiDung.CHUCNANGs)
            {
                MenuFunction.Add(CN);
            }

            MenuAccount = new ObservableCollection<CHUCNANG>()
            {
                new CHUCNANG(){ Icon="AccountCog", TenChucNang="THÔNG TIN TÀI KHOẢN" },
                 new CHUCNANG(){  Icon="Logout", TenChucNang="ĐĂNG XUẤT" },
            };

            SelectedItemFunctionCommand = new RelayCommand<object>((p) => { return p != null; }, (p) =>
              {
                  CHUCNANG ChucNang = p as CHUCNANG;

                  if (ChucNang.TenManHinhDuocLoad == "TicketSales")
                      CurrentScreen = new TicketSales();
                  else if (ChucNang.TenManHinhDuocLoad == "InforTicket")
                  {
                      CurrentScreen = new InforTicket();
                  }
                  else CurrentScreen = null;
              });
        }
    }
}
