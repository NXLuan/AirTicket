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
    
    public class InfoAccount : BaseViewModel 
    {
        public InfoAccount()
        {
            Items1 = new ObservableCollection<NGUOIDUNG>();
            LoadInfoAccountCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                ShowAccount();
            });
            DeleteAccountCommand = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
              {
                  if (SelectedResult == null) return;

                  var AccountList = DataProvider.Instance.DB.NGUOIDUNGs.ToList();

                  NGUOIDUNG tmp = SelectedResult as NGUOIDUNG;
                  try
                  {
                      foreach(var account in AccountList)
                          if(account.TenDangNhap==tmp.TenDangNhap)
                          {
                              DataProvider.Instance.DB.NGUOIDUNGs.Remove(account);
                          }
                      DataProvider.Instance.DB.SaveChanges();
                      ShowAccount();
                  }
                  catch
                  {
                      MessageBox.Show("Xóa thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                  }
              });
            AddInfoAccountCommand = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
             {
                 if (dTenDangNhap == null)
                 {
                     MessageBox.Show("Tên đăng nhập không được để trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                     return;
                 }
                 if (dMatKhau == null)
                 {
                     MessageBox.Show("Mật khẩu không được để trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                     return;
                 }
                 if (cbbMaNhom == null)
                 {
                     MessageBox.Show("Mã nhóm không được để trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                     return;
                 }
                 var AccountList = DataProvider.Instance.DB.NGUOIDUNGs.ToList();
                 foreach (var account in AccountList)
                     if (account.TenDangNhap==dTenDangNhap)
                     {
                         MessageBox.Show("Tên đăng nhập đã tồn tại trong hệ thống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                         return;
                     }
                 NGUOIDUNG tmp = new NGUOIDUNG();
                 tmp.TenDangNhap = dTenDangNhap;
                 tmp.MatKhau = dMatKhau;
                 tmp.MaNhom = dMaNhom;
                 var NguoiDung = DataProvider.Instance.DB.NGUOIDUNGs;
                 NguoiDung.Add(tmp);
                 DataProvider.Instance.DB.SaveChanges();
                 ShowAccount();
                 MessageBox.Show("thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                 dTenDangNhap = null;
                 dMatKhau = null;    
             });
            SaveInfoAccountCommand = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                try
                {
                    foreach (var item in Items1)
                    {
                        if (item.MatKhau == null)
                        {
                            MessageBox.Show("Mã hãng không được để trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (item.MaNhom!="AD" && item.MaNhom != "NV" && item.MaNhom != "QL")
                        {
                            MessageBox.Show("Mã nhóm không được để trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        var AccountList = DataProvider.Instance.DB.NGUOIDUNGs.ToList();
                        foreach (var account in AccountList)
                            if (item.TenDangNhap == account.TenDangNhap && (item.MatKhau != account.MatKhau || item.MaNhom != account.MaNhom))
                            {
                                account.MatKhau = item.MatKhau;
                                account.MaNhom = item.MaNhom;
                            }
                    }
                    DataProvider.Instance.DB.SaveChanges();
                    MessageBox.Show("Chỉnh sửa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowAccount();
                }
                catch
                {
                    MessageBox.Show("Thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
            SearchInfoAccountCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) =>
            {
                if (p.Text != null && p.Text != "")
                {
                    ShowAccount();
                    var datasearch = Items1.Where(x => (x.TenDangNhap.Contains(p.Text)) || (x.MatKhau.Contains(p.Text)) || (x.MaNhom.Contains(p.Text))).ToList();
                    Items1.Clear();
                    foreach (var data in datasearch)
                    {
                        Items1.Add(new NGUOIDUNG()
                        {
                            TenDangNhap = data.TenDangNhap,
                            MatKhau = data.MatKhau,
                            MaNhom = data.MaNhom
                        });
                    }
                }
                else
                {
                    ShowAccount();
                }

            });
        }
        public void ShowAccount()
        {
            var AccountList = DataProvider.Instance.DB.NGUOIDUNGs.ToList();
            Items1.Clear();
            foreach (var account in AccountList)
            {
                Items1.Add(new NGUOIDUNG()
                {
                    TenDangNhap=account.TenDangNhap,
                    MatKhau=account.MatKhau,
                    MaNhom=account.MaNhom
                });

            }
        }

        private NGUOIDUNG m_currentResult;
        public NGUOIDUNG CurrentResult
        {
            get { return m_currentResult; }
            set
            {
                m_currentResult = value;
                OnPropertyChanged("CurrentResult");
            }
        }
        private NGUOIDUNG m_selectedResult;
        public NGUOIDUNG SelectedResult
        {
            get { return m_selectedResult; }
            set
            {
                m_selectedResult = value;
                OnPropertyChanged("SelectedResult");
            }
        }

        private String _dTenDangNhap;
        public String dTenDangNhap
        {
            get { return _dTenDangNhap; }
            set
            {
                _dTenDangNhap = value;
                OnPropertyChanged("dTenDangNhap");
            }
        }
        private String _dMatKhau;
        public String dMatKhau
        {
            get { return _dMatKhau; }
            set
            {
                _dMatKhau = value;
                OnPropertyChanged("dMatKhau");
            }
        }

        private List<String> _cbbMaNhom;
        public List<String> cbbMaNhom
        {
            get
            {
                return new List<string>() { "AD", "NV", "QL" };
            }
            set
            {
                _cbbMaNhom = value;
            }

        }

        private string _searchaccount;
        public string SearchAccount
        {
            get { return _searchaccount; }
            set
            {
                _searchaccount = value;
                OnPropertyChanged("SearchAccount");
            }
        }
        public String dMaNhom { get; set; }
        public ICommand AddInfoAccountCommand { get; set; }
        public ICommand SaveInfoAccountCommand { get; set; }
        public ICommand DeleteAccountCommand { get; set; }
        public ICommand LoadInfoAccountCommand { get; set; }
        public ICommand SearchInfoAccountCommand { get; set; }
        public ObservableCollection<NGUOIDUNG> Items1 { get; set; }
        public IEnumerable<string> MaNhom => new[] { "AD", "NV", "QL" };

    }
}
