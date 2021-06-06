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
    
    public class InfoRegulation : BaseViewModel 
    {
        public InfoRegulation()
        {
            Items1 = new ObservableCollection<Regulation>();
            LoadInfoRegulationCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                ShowRegulation();
            });
            DeleteRegulationCommand = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
              {
                  if (SelectedResult == null) return;

                  var RegulationTicketList = DataProvider.Instance.DB.QUYDINHGIAVEs.ToList();
                  var FlightList = DataProvider.Instance.DB.HANGHANGKHONGs.ToList();

                  Regulation tmp = SelectedResult as Regulation;
                  try
                  {
                      foreach (var regulation in RegulationTicketList)
                          if (regulation.MaHang == tmp.MaHang && regulation.MaLoai == tmp.MaLoai)
                          {
                              DataProvider.Instance.DB.QUYDINHGIAVEs.Remove(regulation);
                              bool ok = false;
                              foreach (var jregulation in RegulationTicketList)
                                  if (jregulation.MaHang == tmp.MaHang && jregulation.MaLoai != tmp.MaLoai)
                                  {
                                      ok = true;
                                      break;
                                  }
                              if (ok) break;
                              foreach (var flight in FlightList)
                                  if (tmp.MaHang == flight.MaHang)
                                  {
                                      DataProvider.Instance.DB.HANGHANGKHONGs.Remove(flight);
                                  }
                          }
                      DataProvider.Instance.DB.SaveChanges();
                      ShowRegulation();
                  }
                  catch
                  {
                      MessageBox.Show("Xóa thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                  }

              });
            AddInfoRegulationCommand = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
             {
                 if (dMaHang == null)
                 {
                     MessageBox.Show("Mã hãng không được để trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                     return;
                 }
                 if (dMaLoai == null)
                 {
                     MessageBox.Show("Mã loại không được để trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                     return;
                 }
                 var RegulationList = DataProvider.Instance.DB.QUYDINHGIAVEs.ToList();
                 foreach (var regulation in RegulationList)
                     if (regulation.MaHang == dMaHang && regulation.MaLoai == dMaLoai)
                     {
                         MessageBox.Show("Mã hãng và mã loại đã tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                         return;
                     }
                 var PassengerList = DataProvider.Instance.DB.LOAIHANHKHACHes.ToList();
                 foreach (var passenger in PassengerList)
                     if (passenger.MaLoai == dMaLoai)
                     {
                         var FlightList = DataProvider.Instance.DB.HANGHANGKHONGs.ToList();
                         bool ok = false;
                         foreach (var flight in FlightList)
                             if (flight.MaHang == dMaHang)
                             {
                                 ok = true;
                                 break;
                             }
                         if (ok == false)
                         {
                             HANGHANGKHONG hktmp = new HANGHANGKHONG();
                             hktmp.MaHang = dMaHang;
                             hktmp.TenHang = dTenHang;
                             var HangHangKhong = DataProvider.Instance.DB.HANGHANGKHONGs;
                             HangHangKhong.Add(hktmp);
                         }

                         QUYDINHGIAVE tmp = new QUYDINHGIAVE();
                         tmp.MaHang = dMaHang;
                         tmp.MaLoai = dMaLoai;
                         tmp.TiLe = dTiLe;
                         tmp.TienGiam = dTienGiam;
                         tmp.TienPhi = dTienPhi;
                         tmp.TienLaiVe = dTienLaiVe;
                         tmp.TienHuyVe = dTienHuyVe;
                         var QuyDinhGiaVe = DataProvider.Instance.DB.QUYDINHGIAVEs;
                         QuyDinhGiaVe.Add(tmp);

                         DataProvider.Instance.DB.SaveChanges();
                         ShowRegulation();

                         MessageBox.Show("thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                         dMaHang = null;
                         dTenHang = null;
                         dTienGiam = 0;
                         dTienPhi = 0;
                         dTienLaiVe = 0;
                         dTienHuyVe = 0;
                     }
             });
            SaveInfoRegulationCommand = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                try
                {
                foreach (var item in Items1)
                {
                    if (item.MaHang == null)
                    {
                        MessageBox.Show("Mã hãng không được để trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (item.MaLoai == null)
                    {
                        MessageBox.Show("Mã loại không được để trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var RegulationList = DataProvider.Instance.DB.QUYDINHGIAVEs.ToList();
                    foreach (var regulation in RegulationList)
                        if (item.MaHang == regulation.MaHang && item.MaLoai == regulation.MaLoai && (item.TenHang != regulation.HANGHANGKHONG.TenHang||item.TienGiam!=regulation.TienGiam || item.TienPhi != regulation.TienPhi || item.TiLe != regulation.TiLe || item.TienLaiVe != regulation.TienLaiVe || item.TienHuyVe != regulation.TienHuyVe))
                            {
                                regulation.HANGHANGKHONG.TenHang = item.TenHang;
                                regulation.TienGiam = item.TienGiam;
                                regulation.TienPhi = item.TienPhi;
                                regulation.TiLe = item.TiLe;
                                regulation.TienLaiVe = item.TienLaiVe;
                                regulation.TienHuyVe = item.TienHuyVe;
                            }
                    }
                    DataProvider.Instance.DB.SaveChanges();
                    MessageBox.Show("Chỉnh sửa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowRegulation();
                }
                catch
                {
                    MessageBox.Show("Thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }


                /*
                var RegulationList = DataProvider.Instance.DB.QUYDINHGIAVEs.ToList();
                foreach (var regulation in RegulationList)
                {
                    DataProvider.Instance.DB.QUYDINHGIAVEs.Remove(regulation);
                }
                var FlightList = DataProvider.Instance.DB.HANGHANGKHONGs.ToList();
                foreach (var flight in FlightList)
                {
                    DataProvider.Instance.DB.HANGHANGKHONGs.Remove(flight);
                }
                foreach(var item in Items1)
                {
                    if(item.MaHang==null)
                    {
                        MessageBox.Show("Mã hãng không được để trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (item.MaLoai == null)
                    {
                        MessageBox.Show("Mã loại không được để trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var tmpFlightList = DataProvider.Instance.DB.HANGHANGKHONGs.ToList();
                    bool ok = false;
                    foreach (var tmpflight in tmpFlightList)
                        if (tmpflight.MaHang == item.MaHang)
                            ok = true;
                    if (ok == false)
                    {
                        HANGHANGKHONG hktmp = new HANGHANGKHONG();
                        hktmp.MaHang = item.MaHang;
                        hktmp.TenHang = item.TenHang;
                        var HangHangKhong = DataProvider.Instance.DB.HANGHANGKHONGs;
                        HangHangKhong.Add(hktmp);
                    }
                    QUYDINHGIAVE tmp = new QUYDINHGIAVE();
                    tmp.MaHang = item.MaHang;
                    tmp.MaLoai = item.MaLoai;
                    tmp.TiLe = item.TiLe;
                    tmp.TienGiam = item.TienGiam;
                    tmp.TienPhi = item.TienPhi;
                    tmp.TienLaiVe = item.TienLaiVe;
                    tmp.TienHuyVe = item.TienHuyVe;
                    var QuyDinhGiaVe = DataProvider.Instance.DB.QUYDINHGIAVEs;
                    QuyDinhGiaVe.Add(tmp);
                }
                DataProvider.Instance.DB.SaveChanges();
                MessageBox.Show("chỉnh sửa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                ShowRegulation();
                */
                /*
                var RegulationList = DataProvider.Instance.DB.QUYDINHVEs.ToList();
                try
                {
                    foreach (var regulation in RegulationList)
                    {
                        Regulation tmp = p.SelectedItem as Regulation;

                        regulation.TienLaiVe = null;
                        regulation.TienHuyVe = null;
                    }
                    //}
                    //foreach (var i_regulation in Items1)
                    //{
                    //    foreach (var j_regulation in RegulationList)
                    //        if (i_regulation.MaHang == j_ticket.MaVe && i_ticket.TinhTrang == "Đã hủy" && j_ticket.TrangThai == "Thành công")
                    //        {
                    //            j_ticket.TrangThai = i_ticket.TinhTrang;
                    //        }
                    //}
                    DataProvider.Instance.DB.SaveChanges();
                    MessageBox.Show("thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
                */

            });
            SearchInfoRegulationCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) =>
            {
                if (p.Text != null && p.Text != "")
                {
                    ShowRegulation();
                    var datasearch = Items1.Where(x => (x.MaHang.Contains(p.Text)) || (x.TenHang.Contains(p.Text)) || (x.MaLoai.Contains(p.Text)) || (x.TenLoai.Contains(p.Text)) || (x.TienGiam.ToString().Contains(p.Text)) || (x.TienPhi.ToString().Contains(p.Text)) || (x.TiLe.ToString().Contains(p.Text)) || (x.TienLaiVe.ToString().Contains(p.Text)) || (x.TienHuyVe.ToString().Contains(p.Text))).ToList();
                    Items1.Clear();
                    foreach (var data in datasearch)
                    {
                        Items1.Add(new Regulation()
                        {
                            MaHang=data.MaHang,
                            TenHang = data.TenHang,
                            MaLoai = data.MaLoai,
                            TenLoai = data.TenLoai,
                            TiLe = data.TiLe,
                            TienGiam = data.TienGiam,
                            TienPhi=data.TienPhi,
                            TienLaiVe=data.TienLaiVe,
                            TienHuyVe=data.TienHuyVe,
                        });
                    }
                }
                else
                {
                    ShowRegulation();
                }

            });
        }
        public void ShowRegulation()
        {
            var RegulationTicketList = DataProvider.Instance.DB.QUYDINHGIAVEs.ToList();
            Items1.Clear();
            foreach (var regulationticket in RegulationTicketList)
            {
                Items1.Add(new Regulation()
                {
                    MaHang=  regulationticket.MaHang,
                    TenHang= regulationticket.HANGHANGKHONG.TenHang,
                    MaLoai = regulationticket.MaLoai,
                    TenLoai = regulationticket.LOAIHANHKHACH.TenLoai,
                    TiLe = Convert.ToDouble(regulationticket.TiLe),
                    TienGiam = Convert.ToInt32(regulationticket.TienGiam),
                    TienPhi = Convert.ToInt32(regulationticket.TienPhi),
                    TienLaiVe = Convert.ToInt32(regulationticket.TienLaiVe),
                    TienHuyVe = Convert.ToInt32(regulationticket.TienHuyVe)
                });

            }
        }

        private Regulation m_currentResult;
        public Regulation CurrentResult
        {
            get { return m_currentResult; }
            set
            {
                m_currentResult = value;
                OnPropertyChanged("CurrentResult");
            }
        }
        private Regulation m_selectedResult;
        public Regulation SelectedResult
        {
            get { return m_selectedResult; }
            set
            {
                m_selectedResult = value;
                OnPropertyChanged("SelectedResult");
            }
        }

        private String _dMaHang;
        public String dMaHang
        {
            get { return _dMaHang; }
            set
            {
                _dMaHang = value;
                OnPropertyChanged("dMaHang");
            }
        }
        private String _dTenHang;
        public String dTenHang
        {
            get { return _dTenHang; }
            set
            {
                _dTenHang = value;
                OnPropertyChanged("dTenHang");
            }
        }
        private float _dTiLe;
        public float dTiLe
        {
            get { return _dTiLe; }
            set
            {
                _dTiLe = value;
                OnPropertyChanged("dTiLe");
            }
        }
        private int _dTienGiam;
        public int dTienGiam
        {
            get { return _dTienGiam; }
            set
            {
                _dTienGiam = value;
                OnPropertyChanged("dTienGiam");
            }
        }
        private int _dTienPhi;
        public int dTienPhi
        {
            get { return _dTienPhi; }
            set
            {
                _dTienPhi = value;
                OnPropertyChanged("dTienPhi");
            }
        }
        private int _dTienLaiVe;
        public int dTienLaiVe
        {
            get { return _dTienLaiVe; }
            set
            {
                _dTienLaiVe = value;
                OnPropertyChanged("dTienLaiVe");
            }
        }
        private int _dTienHuyVe;
        public int dTienHuyVe
        {
            get { return _dTienHuyVe; }
            set
            {
                _dTienHuyVe = value;
                OnPropertyChanged("dTienHuyVe");
            }
        }


        private List<String> _cbbMaLoai;
        public List<String> cbbMaLoai
        {
            get
            {
                return new List<string>() { "adt", "chd", "inf" };
            }
            set
            {
                _cbbMaLoai = value;
            }

        }
        public String dMaLoai { get; set; }

        private string _searchregulation;
        public string SearchRegulation
        {
            get { return _searchregulation; }
            set
            {
                _searchregulation = value;
                OnPropertyChanged("SearchRegulation");
            }
        }

        public ICommand AddInfoRegulationCommand { get; set; }
        public ICommand SaveInfoRegulationCommand { get; set; }
        public ICommand DeleteRegulationCommand { get; set; }
        public ICommand LoadInfoRegulationCommand { get; set; }
        public ICommand SearchInfoRegulationCommand { get; set; }
        public ObservableCollection<Regulation> Items1 { get; set; }

    }
}
