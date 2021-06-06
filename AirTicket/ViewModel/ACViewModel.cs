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
using System.Collections;
using AirTicket.View;

namespace AirTicket.ViewModel
{
    public class ACViewModel : BaseViewModel
    {
        private static Dictionary<string, bool> _checkBoxMap;
        public Dictionary<string, bool> checkBoxMap
        {

            get { return _checkBoxMap; }
            set
            {
                _checkBoxMap = value;
                OnPropertyChanged();
            }
        }
        public ICommand CheckBoxCommand { get; set; }
        public ICommand LoadedWinDowCommand { get; set; }

        public ACViewModel()
        {
            AIRTICKETEntities db = new AIRTICKETEntities();
            List<NHOMNGUOIDUNG> NhomNguoiDung = db.NHOMNGUOIDUNGs.ToList();

            #region Init checkBoxMap, very very ugly, non reusable, don't watch!!
            checkBoxMap = new Dictionary<string, bool>();

            checkBoxMap.Add("AD_BV", false); // Admin
            checkBoxMap.Add("AD_QLV", false);
            checkBoxMap.Add("AD_TK", false);
            checkBoxMap.Add("AD_QD", false);
            checkBoxMap.Add("AD_QLTK", false);

            checkBoxMap.Add("QL_BV", false); // Quản lý
            checkBoxMap.Add("QL_QLV", false);
            checkBoxMap.Add("QL_TK", false);
            checkBoxMap.Add("QL_QD", false);
            checkBoxMap.Add("QL_QLTK", false);

            checkBoxMap.Add("NV_BV", false); // Nhân viên
            checkBoxMap.Add("NV_QLV", false);
            checkBoxMap.Add("NV_TK", false);
            checkBoxMap.Add("NV_QD", false);
            checkBoxMap.Add("NV_QLTK", false);

            foreach (NHOMNGUOIDUNG NND in NhomNguoiDung)
            {
                foreach (CHUCNANG CN in NND.CHUCNANGs)
                {

                    string checkBoxName = NND.MaNhom + "_" + CN.MaChucNang;

                    bool tmp = false;
                    if (checkBoxMap.TryGetValue(checkBoxName, out tmp))
                    {
                        //if key exist; can't modify but adding if not for this shit :D 
                        checkBoxMap[checkBoxName] = true;
                    }

                }
            }

            #endregion

            LoadedWinDowCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {

            });

            CheckBoxCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CheckBox checkBox = p as CheckBox;
                string[] splited_info = checkBox.Name.ToString().Split('_');

                string temp_MaNhom = splited_info[0];
                string temp_MaChucNang = splited_info[1];

                NHOMNGUOIDUNG NND = NhomNguoiDung.Where(x => x.MaNhom == temp_MaNhom).SingleOrDefault();

                if (checkBox.IsChecked == true)
                {
                    CHUCNANG temp_CN = db.CHUCNANGs.Where(x => x.MaChucNang == temp_MaChucNang).SingleOrDefault();
                    if (NND != null)
                    {
                        NND.CHUCNANGs.Add(temp_CN);
                    }
                }
                else
                {
                    try 
                    {
                        CHUCNANG CN = NND.CHUCNANGs.Where(x => x.MaChucNang == temp_MaChucNang).SingleOrDefault();
                        NND.CHUCNANGs.Remove(CN);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }

                int tmp = db.SaveChanges();
            });
        }
    }
}
