using AirTicket.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AirTicket.ViewModel
{
    class InfoPaViewModel : BaseViewModel
    {
        public ICommand InfoPaLoadCommand { get; set; }
        public ICommand btnBookClickCommand { get; set; }

        public ICommand btnSelectAgainCommand { get; set; }

        public ObservableCollection<DetailInfoPassenger> ListDetailInfo { get; set; }
        public ObservableCollection<PassengerViewModel> ListPriceTicket { get; set; }
        public HOADON HDModel { get; set; }

        public TabControl tabControl;

        private FlightViewModel _flightSelected;
        public FlightViewModel FlightSelected
        {
            get => _flightSelected;
            set => SetProperty(ref _flightSelected, value);
        }

        private string _infoDateTimeDeparture;

        public string InfoDateTimeDeparture
        {
            get => _infoDateTimeDeparture;
            set => SetProperty(ref _infoDateTimeDeparture, value);
        }

        private decimal _totalPriceTicket;
        public decimal TotalPriceTicket
        {
            get => _totalPriceTicket;
            set => SetProperty(ref _totalPriceTicket, value);
        }
        private decimal _totalNetprofitTicket;
        public decimal TotalNetprofitTicket
        {
            get => _totalNetprofitTicket;
            set => SetProperty(ref _totalNetprofitTicket, value);
        }
        private decimal _totalcancellationcostTicket;
        public decimal TotalCancellationCostTicket
        {
            get => _totalcancellationcostTicket;
            set => SetProperty(ref _totalcancellationcostTicket, value);
        }

        public InfoPaViewModel()
        {
            HDModel = new HOADON();
            ListDetailInfo = new ObservableCollection<DetailInfoPassenger>();
            ListPriceTicket = new ObservableCollection<PassengerViewModel>();

            InfoPaLoadCommand = new RelayCommand<TabItem>((p) => { return FlightSelected != null; }, (p) =>
            {
                //var QuyDinhGiaVe = DataProvider.Instance.DB.QUYDINHGIAVEs.Where(x=>x.MaHang == FlightSelected.AirlineID);
                TicketSalesViewModel ticketSaleVM = p.DataContext as TicketSalesViewModel;
                int count = 1;
                ListDetailInfo.Clear();
                ListPriceTicket.Clear();
                TotalPriceTicket = 0;
                TotalNetprofitTicket = 0;
                TotalCancellationCostTicket = 0;

                foreach (PassengerViewModel pvm in ticketSaleVM.ListInfoPassengerVM)
                {
                    if (pvm.NumberOfPassenger != 0)
                    {
                        decimal price = ParseStringToDecimal(FlightSelected.priceFlight);
                        QUYDINHGIAVE QuyDinhGiaVe = pvm.LHKModel.QUYDINHGIAVEs.Where(x => x.MaHang == FlightSelected.AirlineID).First();
                        pvm.NetProfitTicket = (decimal)QuyDinhGiaVe.TienLaiVe;
                        pvm.CancellationCostTicket = (decimal)QuyDinhGiaVe.TienHuyVe;
                        pvm.PriceTicket = (decimal)(price * (decimal)QuyDinhGiaVe.TiLe - QuyDinhGiaVe.TienGiam + QuyDinhGiaVe.TienPhi) + pvm.NetProfitTicket;
                        
                        TotalPriceTicket += pvm.TotalPriceTicket;
                        TotalNetprofitTicket += pvm.TotalNetProfitTicket;
                        TotalCancellationCostTicket += pvm.TotalCancellationCostTicket;
                        
                        
                        ListPriceTicket.Add(pvm);
                    }

                    for (int i = 0; i < pvm.NumberOfPassenger; i++)
                    {
                        DetailInfoPassenger detailInfo = new DetailInfoPassenger()
                        {
                            num = count,
                        };
                        detailInfo.VCBModel.LOAIHANHKHACH = pvm.LHKModel;
                        detailInfo.VCBModel.GiaVe = pvm.PriceTicket;
                        ListDetailInfo.Add(detailInfo);
                        count++;
                    }

                    if (count > ticketSaleVM.TotalPassenger) break;
                }

                InfoDateTimeDeparture = FlightSelected.departureTime + " " + ticketSaleVM.InfoDateDeparture;
            });

            btnBookClickCommand = new RelayCommand<object>((p) => { return IsValid(p as DependencyObject); }, (p) =>
            {
                BookTicket();
            });

            btnSelectAgainCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ReturnTicketSales();
            });
        }

        private bool IsValid(DependencyObject obj)
        {
            if (Validation.GetHasError(obj))
                return false;

            // Validate all the bindings on the children
            for (int i = 0; i != VisualTreeHelper.GetChildrenCount(obj); ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (!IsValid(child)) { return false; }
            }

            return true;
        }

        public decimal ParseStringToDecimal(string Price)
        {
            int i = 0;
            do
            {
                if (!char.IsNumber(Price[i]))
                {
                    Price = Price.Remove(i, 1);
                }
                else
                    i++;
            } while (i < Price.Length);
            return decimal.Parse(Price);
        }

        public void BookTicket()
        {
            try
            {
                HDModel.TongTien = TotalPriceTicket;
                HDModel.LoiNhuan = TotalNetprofitTicket;
                HDModel.ChiPhiHuy = TotalCancellationCostTicket;
                HDModel.TrangThai = "Thành công";
                HDModel.ThoiGianTao = DateTime.Now;
                HDModel.MaHoaDon = DataProvider.Instance.DB.Database.SqlQuery<string>("select dbo.AUTO_IDHD()").Single();        
                var VeChuyenBay = DataProvider.Instance.DB.VECHUYENBAYs;
                HDModel.SoVe = ListDetailInfo.Count;
                DataProvider.Instance.DB.HOADONs.Add(HDModel);
                foreach (DetailInfoPassenger detailInfo in ListDetailInfo)
                {
                    VECHUYENBAY VCBModel = detailInfo.VCBModel;
                    VCBModel.HOADON = HDModel;
                    VCBModel.MaVe = DataProvider.Instance.DB.Database.SqlQuery<string>("select dbo.AUTO_IDVCB()").Single();
                    VeChuyenBay.Add(VCBModel);
                    DataProvider.Instance.DB.SaveChanges();
                }
                MessageBox.Show("thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                ReturnTicketSales();
            }
            catch 
            {
                MessageBox.Show("Thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ReturnTicketSales()
        {
            TabItem tabItem1 = tabControl.Items[1] as TabItem;
            TabItem tabItem0 = tabControl.Items[0] as TabItem;

            tabItem1.IsEnabled = false;
            tabItem0.IsEnabled = true;

            tabControl.SelectedIndex = 0;
        }
    }
}
