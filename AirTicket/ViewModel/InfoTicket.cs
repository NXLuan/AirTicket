using AirTicket.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirTicket.ViewModel
{
    
    public class InfoTicket : BaseViewModel 
    {
        public InfoTicket()
        {
            TabValue = 0;
            Items1 = new ObservableCollection<VeChuyenBay>();
            Items2 = new ObservableCollection<HOADON>();
            LoadInfoTicketCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                ShowTicket();
                ShowBill();
            });
            SearchInfoTicketCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) =>
            {
                if (p.Text != null && p.Text != "")
                {
                    ShowTicket();
                    var datasearch = Items1.Where(x => (x.HoVaTen.Contains(p.Text)) || (x.GiaTien.ToString().Contains(p.Text)) || (x.MaVeChuyenBay.Contains(p.Text)) || (x.MaHoaDon.Contains(p.Text)) || (x.GioiTinh.Contains(p.Text)) || (x.TinhTrang.Contains(p.Text)) || (x.NgaySinh.Contains(p.Text))).ToList();
                    Items1.Clear();
                    foreach (var data in datasearch)
                    {
                        Items1.Add(new VeChuyenBay()
                        {
                            MaVeChuyenBay = data.MaVeChuyenBay,
                            MaHoaDon = data.MaHoaDon,
                            HoVaTen = data.HoVaTen,
                            GioiTinh = data.GioiTinh,
                            GiaTien = data.GiaTien,
                            TinhTrang = data.TinhTrang,
                            NgaySinh = data.NgaySinh
                        });
                    }
                }
                else
                {
                    ShowTicket();
                }

            });
            SearchInfoBillsCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) =>
            {
                if (p.Text != null && p.Text != "")
                {
                    ShowBill();
                    var datasearch = Items2.Where(x => (x.HoTen.Contains(p.Text)) || (x.TongTien.ToString().Contains(p.Text)) || ((x.GhiChu != null) && (x.GhiChu.Contains(p.Text))) || (x.MaHoaDon.Contains(p.Text)) || (x.GioiTinh.Contains(p.Text)) || (x.Email.Contains(p.Text)) || ((x.ThoiGianTao.ToString()).Contains(p.Text)) || (x.SDT.Contains(p.Text))).ToList();
                    Items2.Clear();
                    foreach (var bill in datasearch)
                    {
                        Items2.Add(new HOADON()
                        {
                            SoVe = bill.SoVe,
                            MaHoaDon = bill.MaHoaDon,
                            HoTen = bill.HoTen,
                            GioiTinh = bill.GioiTinh,
                            ThoiGianTao = bill.ThoiGianTao,
                            TongTien = bill.TongTien,
                            SDT = bill.SDT,
                            Email = bill.Email,
                            GhiChu = bill.GhiChu
                        });
                    }
                }
                else
                {
                    ShowBill();

                }

            });
            TransferTicketBillCommand = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                if (p.SelectedIndex == -1)
                    return;
                //      MessageBox.Show(p.SelectedIndex.ToString());
                //     MessageBox.Show(p.CurrentColumn.DisplayIndex.ToString());
                if (p.CurrentColumn.DisplayIndex == 1)
                {
                    //MessageBox.Show(TabValue.ToString());
                    TabValue = 1;
                    //MessageBox.Show(TabValue.ToString());
                    VeChuyenBay tmp = p.SelectedItem as VeChuyenBay;
                    SearchBill = tmp.MaHoaDon;
                }
            });
            TransferBillTicketCommand = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
             {
                 if (p.SelectedIndex == -1)
                     return;
                 if (p.CurrentColumn.DisplayIndex == 0)
                 {
                     TabValue = 0;
                     HOADON tmp = p.SelectedItem as HOADON;
                     SearchTicket = tmp.MaHoaDon;
                 }
             });
            SaveTicketCommand = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                var ticketList = DataProvider.Instance.DB.VECHUYENBAYs.ToList();
                try
                {
                    foreach(var i_ticket in Items1)
                    {
                        foreach(var j_ticket in ticketList)
                            if(i_ticket.MaVeChuyenBay==j_ticket.MaVe&&i_ticket.TinhTrang=="Đã hủy"&&j_ticket.TrangThai=="Thành công")
                            {
                                j_ticket.TrangThai = i_ticket.TinhTrang;
                            }
                    }
                    DataProvider.Instance.DB.SaveChanges();
                    MessageBox.Show("thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
        public void ShowTicket()
        {
            var ticketList = DataProvider.Instance.DB.VECHUYENBAYs.ToList();
            Items1.Clear();
            foreach (var ticket in ticketList)
            {
                Items1.Add(new VeChuyenBay()
                {
                    MaVeChuyenBay = ticket.MaVe,
                    MaHoaDon = ticket.MaHoaDon,
                    HoVaTen = ticket.HoTen,
                    GioiTinh = ticket.GioTinh,
                    GiaTien = decimal.Parse(ticket.GiaVe.ToString()),
                    TinhTrang = ticket.TrangThai,
                    NgaySinh = ticket.NgaySinh.Value.ToString("dd/MM/yyyy")
                }) ;
            }
        }
        public void ShowBill()
        {
            var billList = DataProvider.Instance.DB.HOADONs.ToList();
            Items2.Clear();
            foreach (var bill in billList)
            {
                Items2.Add(new HOADON()
                {
                    SoVe = bill.SoVe,
                    MaHoaDon = bill.MaHoaDon,
                    HoTen=bill.HoTen,
                    GioiTinh=bill.GioiTinh,
                    ThoiGianTao=bill.ThoiGianTao,
                    TongTien=bill.TongTien,
                    SDT=bill.SDT,
                    Email=bill.Email,
                    GhiChu=bill.GhiChu
                });
            }
        }
        /*
        private static ObservableCollection<VeChuyenBay> CreateData()
        {
            var ticketList = DataProvider.Instance.DB.VECHUYENBAYs;
            return new ObservableCollection<VeChuyenBay>
            { 
                new VeChuyenBay
                {
                    MaVeChuyenBay = "MVCB0524202101",
                    GioiTinh = "Nam",
                    HoVaTen = "Nguyễn Hữu Hiếu",
                    GiaTien= 470000,
                    MaHoaDon="HD0524202101"
                },
                new VeChuyenBay
                {
                    MaVeChuyenBay = "MVCB0524202102",
                    GioiTinh = "Nữ",
                    HoVaTen = "Nguyễn Xuân Luân",
                    TinhTrang = "Đã hủy",
                    GiaTien= 480000,
                    MaHoaDon="HD0524202102"
                },
                new VeChuyenBay
                {
                    MaVeChuyenBay = "MVCB0524202103",
                    GioiTinh = "Nam",
                    HoVaTen = "Nguyễn Phúc Dương Long",
                    GiaTien= 490000,
                    MaHoaDon="HD0524202103"
                }
            };
        }
        
        */

        public ICommand SaveTicketCommand { get; set; }
        public ICommand LoadInfoTicketCommand { get; set; }
        public ICommand SearchInfoTicketCommand { get; set; }
        public ICommand SearchInfoBillsCommand { get; set; }
        public ICommand TransferTicketBillCommand { get; set; }
        public ICommand TransferBillTicketCommand { get; set; }
        public ObservableCollection<VeChuyenBay> Items1 { get; set; }
        public ObservableCollection<HOADON> Items2 { get; set; }
        private string _searchticket;
        public string SearchTicket
        {
            get { return _searchticket; }
            set
            {
                _searchticket = value;
                OnPropertyChanged("SearchTicket");
            }
        }
        private string _searchbill;
        public string SearchBill
        {
            get { return _searchbill; }
            set
            {
                _searchbill = value;
                OnPropertyChanged("SearchBill");
            }
        }
        private int _tabvalue;
        public int TabValue
        {
            get { return _tabvalue; }
            set
            {
             //   if (_tabvalue != value)
                {
                    _tabvalue = value;
                    OnPropertyChanged("TabValue");

                }
            }
        }
        public IEnumerable<string> TinhTrang => new[] { "Thành công", "Đã hủy"};
        public IEnumerable<string> GioiTinh => new[] { "Nam", "Nữ" };
    }
}
