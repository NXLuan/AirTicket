﻿using AirTicket.Model;
using AirTicket.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirTicket.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private int _selectedIndexMenuAccount;
        public int SelectedIndexMenuAccount
        {
            get => _selectedIndexMenuAccount;
            set => SetProperty(ref _selectedIndexMenuAccount, value);
        }
        public ICommand LoadedWinDowCommand { get; set; }
        private UserControl _currentScreen;
        public UserControl CurrentScreen
        {
            get => _currentScreen;
            set => SetProperty(ref _currentScreen, value);
        }
        static public ObservableCollection<CHUCNANG> MenuFunction { get; set; }
        public ObservableCollection<CHUCNANG> MenuAccount { get; set; }
        public ICommand SelectedItemFunctionCommand { get; set; }
        public ICommand SelectedAccountCommand { get; set; }
        public ICommand ChangeUserControlCommand { get; set; }
        public MainViewModel()
        {
            MenuFunction = new ObservableCollection<CHUCNANG>();
            LoadedWinDowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                ShowLogin(p);
            });

            MenuAccount = new ObservableCollection<CHUCNANG>()
            {
                new CHUCNANG(){ Icon="AccountCog", TenChucNang="THÔNG TIN TÀI KHOẢN" },
                 new CHUCNANG(){  Icon="Logout", TenChucNang="ĐĂNG XUẤT" },
            };

            SelectedAccountCommand = new RelayCommand<Window>((p) => { return SelectedIndexMenuAccount >= 0; }, (p) =>
              {
                  switch (SelectedIndexMenuAccount)
                  {
                      case 1:
                          ShowLogin(p);                       
                          return;
                  }
              });
            SelectedItemFunctionCommand = new RelayCommand<object>((p) => { return p != null; }, (p) =>
              {
                  CHUCNANG ChucNang = p as CHUCNANG;
                  Type type = Type.GetType("AirTicket.View." + ChucNang.TenManHinhDuocLoad);
                  CurrentScreen = Activator.CreateInstance(type) as UserControl;
              });
        }
        public static void update(AIRTICKETEntities db)
        {
            LoginWindow loginWindow = new LoginWindow();
            var loginVM = loginWindow.DataContext as LoginViewModel;
            MenuFunction.Clear();
            DataProvider.Instance.DB.NHOMNGUOIDUNGs = db.NHOMNGUOIDUNGs;
            DataProvider.Instance.DB.SaveChanges();
            var NhomNguoiDung = db.NHOMNGUOIDUNGs.Where(x => x.MaNhom == loginVM.AccountTypte).First();
            foreach (CHUCNANG CN in NhomNguoiDung.CHUCNANGs)
            {
                MenuFunction.Add(CN);
            }
        }

        public void ShowLogin(Window p)
        {
            if (p == null)
                return;
            p.Hide();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();

            if (loginWindow.DataContext == null)
                return;
            var loginVM = loginWindow.DataContext as LoginViewModel;

            if (loginVM.IsLogin)
            {
                MenuFunction.Clear();


                /*
                CHUCNANG a = new CHUCNANG();
                a.Icon = "ViewListOutline";
                a.MaChucNang = "QLV";
                a.TenChucNang = "QUẢN LÝ VÉ";
                a.TenManHinhDuocLoad = "InforTicket";
                MenuFunction.Add(a);
                */

                var NhomNguoiDung = DataProvider.Instance.DB.NHOMNGUOIDUNGs.Where(x => x.MaNhom == loginVM.AccountTypte).First();
                foreach (CHUCNANG CN in NhomNguoiDung.CHUCNANGs)
                {
                    MenuFunction.Add(CN);
                }
                CurrentScreen = null;
                SelectedIndexMenuAccount = -1;
                p.Show();
            }
            else
            {
                p.Close();
            }
        }
    }
}
