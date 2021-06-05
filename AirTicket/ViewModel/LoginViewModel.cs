using AirTicket.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirTicket.ViewModel
{
    class LoginViewModel : BaseViewModel
    {
        public NGUOIDUNG NDModel { get; set; }
        public bool IsLogin { get; set; }
        public string AccountTypte { get; set; }
        //private string _UserName;
        //public string UserName
        //{
        //    get => _UserName;
        //    set => SetProperty(ref _UserName, value);
        //}
        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _contentError;
        public string ContentError
        {
            get => _contentError;
            set => SetProperty(ref _contentError, value);
        }
        public ICommand btnLoginClickCommand { get; set; }
        public ICommand TextChangedCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand btnCloseCommand { get; set; }
        public ICommand LoadedWindowCommand { get; set; }
        public LoginViewModel()
        {
            NDModel = new NGUOIDUNG();
            Password = "";
            //UserName = "";
            LoadedWindowCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsLogin = false; 
            });
            btnLoginClickCommand = new RelayCommand<Window>((p) => { return Password != ""; }, (p) => { Login(p); });
            TextChangedCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) => { ContentError = ""; });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; ContentError = ""; });
            btnCloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
        }
        void Login(Window p)
        {
            ContentError = "";
            if (p == null) return;

            //test
            /*
            {
                IsLogin = true;
                p.Close();
                return;
            }
            */

            var count = DataProvider.Instance.DB.NGUOIDUNGs.Where(record => record.TenDangNhap == NDModel.TenDangNhap && record.MatKhau == Password).Count();
            if (count > 0)
            {
                var Account = DataProvider.Instance.DB.NGUOIDUNGs.Where(record => record.TenDangNhap == NDModel.TenDangNhap && record.MatKhau == Password).First();
                AccountTypte = Account.MaNhom;
                IsLogin = true;
                p.Close();
            }
            else
            {
                IsLogin = false;
                ContentError = "Sai tên tài khoản hoặc mật khẩu";
            }

        }
    }

}
