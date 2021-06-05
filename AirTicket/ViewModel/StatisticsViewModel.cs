using AirTicket.Model;
using AirTicket.View;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace AirTicket.ViewModel
{
    public class StatisticsViewModel : BaseViewModel
    {
        #region handleControlVisibility
        private string _statisticsType;
        public string StatisticsType
        {
            get { return _statisticsType; }
            set
            {
                _statisticsType = value;
                OnPropertyChanged("StatisticsType");
                if (StatisticsType == "Ngày")
                {
                    DateVisibility = "Visible";
                    MonthAndYearVisibility = "Collapsed";
                    MonthVisibility = "Collapsed";
                }
                else
                {
                    DateVisibility = "Collapsed";
                    MonthAndYearVisibility = "Visible";
                    if (StatisticsType == "Tháng")
                        MonthVisibility = "Visible";
                    else
                        MonthVisibility = "Collapsed";
                }
            }
        }

        private string _monthAndYearVisibility;
        public string MonthAndYearVisibility
        {
            get { return _monthAndYearVisibility; }
            set
            {
                _monthAndYearVisibility = value;
                OnPropertyChanged("MonthAndYearVisibility");
            }
        }
        private string _monthVisibility;
        public string MonthVisibility
        {
            get { return _monthVisibility; }
            set
            {
                _monthVisibility = value;
                OnPropertyChanged("MonthVisibility");
            }
        }
        private string _dateVisibility;
        public string DateVisibility
        {
            get { return _dateVisibility; }
            set
            {
                _dateVisibility = value;
                OnPropertyChanged("DateVisibility");
            }

        }
        #endregion
        private ObservableCollection<int> _listMonth;
        public ObservableCollection<int> ListMonth
        {
            get => _listMonth;
            set => SetProperty(ref _listMonth, value);
        }
        private ObservableCollection<int> _listYear;
        public ObservableCollection<int> ListYear
        {
            get => _listYear;
            set => SetProperty(ref _listYear, value);
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");
                if (!isValid())
                {
                    StartColor = "Red";
                    EndColor = "#323942";
                }
                else
                {
                    StartColor = EndColor = "#323942";
                }
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged("EndDate");
                if (!isValid())
                {
                    EndColor = "Red";
                    StartColor = "#323942";
                }
                else
                {
                    StartColor = EndColor = "#323942";
                }
            }
        }

        private string _startColor;
        public string StartColor { get => _startColor; set { _startColor = value; OnPropertyChanged("StartColor"); } }
        private string _endColor;
        public string EndColor { get => _endColor; set { _endColor = value; OnPropertyChanged("EndColor"); } }


        private int _selectedMonth;
        public int SelectedMonth { get => _selectedMonth; set { _selectedMonth = value; OnPropertyChanged("SelectedMonth"); } }

        private int _selectedYear;
        public int SelectedYear { get => _selectedYear; set { _selectedYear = value; OnPropertyChanged("SelectedYear"); } }


        private bool isValid()
        {
            return EndDate.CompareTo(StartDate) >= 0;
        }

        public ICommand SearchStatisticsCommand { get; set; }
        public ICommand ExportExcelCommand { get; set; }
        public ICommand BtnBarChartCommand { get; set; }

        public ObservableCollection<DAILY> ListDaiLy { get; set; }


        private ObservableCollection<HOADON> _listThongKe;
        public ObservableCollection<HOADON> ListThongKe { get => _listThongKe; set { _listThongKe = value; OnPropertyChanged("ListThongKe"); } }

        private string _selectedDaiLy;
        public string SelectedDaiLy { get => _selectedDaiLy; set { _selectedDaiLy = value; OnPropertyChanged("SelectedDaiLy"); } }

        private string _detailVisibility;
        public string DetailVisibility { get => _detailVisibility; set { _detailVisibility = value; OnPropertyChanged("DetailVisibility"); } }

        private decimal _tongDoanhThu;
        public decimal TongDoanhThu { get => _tongDoanhThu; set { _tongDoanhThu = value; OnPropertyChanged("TongDoanhThu"); } }

        private decimal _tongLoiNhuan;
        public decimal TongLoiNhuan { get => _tongLoiNhuan; set { _tongLoiNhuan = value; OnPropertyChanged("TongLoiNhuan"); } }

        private int _tongSoHoaDon;
        public int TongSoHoaDon { get => _tongSoHoaDon; set { _tongSoHoaDon = value; OnPropertyChanged("TongSoHoaDon"); } }

        private int _tongSoVe;
        public int TongSoVe { get => _tongSoVe; set { _tongSoVe = value; OnPropertyChanged("TongSoVe"); } }

        private string _btnChartVisibility;
        public string BtnChartVisibility { get => _btnChartVisibility; set { _btnChartVisibility = value; OnPropertyChanged("BtnChartVisibility"); } }

        private string _titleChart;
        public string TitleChart { get => _titleChart; set { _titleChart = value; OnPropertyChanged("TitleChart"); } }

        public ObservableCollection<DataChart> ListBarChart { get; set; }

        public StatisticsViewModel()
        {
            ListMonth = new ObservableCollection<int>();
            for (int i = 1; i <= 12; i++)
                ListMonth.Add(i);

            ListYear = new ObservableCollection<int>();
            for (int i = DateTime.Now.Year - 5; i <= DateTime.Now.Year; i++)
                ListYear.Add(i);

            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            SelectedMonth = DateTime.Now.Month;
            SelectedYear = DateTime.Now.Year;

            ListDaiLy = new ObservableCollection<DAILY>(DataProvider.Instance.DB.DAILies);
            SelectedDaiLy = ListDaiLy[0].MaDaiLy;

            DetailVisibility = "Collapsed";
            BtnChartVisibility = "Collapsed";

            SearchStatisticsCommand = new RelayCommand<object>((p) =>
            {
                if (StatisticsType == "Ngày")
                    return isValid();
                return true;
            }, (p) =>
            {
                if (StatisticsType == "Ngày")
                {
                    LoadDateTable();
                    BtnChartVisibility = "Collapsed";
                }

                else if (StatisticsType == "Tháng")
                {
                    LoadMonthTable();
                    BtnChartVisibility = "Collapsed";
                }

                else
                {
                    LoadYearTable();
                    BtnChartVisibility = "Visible";
                }
                if (ListThongKe.Count > 0)
                    LoadDetailStatistics();
                else
                    DetailVisibility = "Collapsed";
            });

            ExportExcelCommand = new RelayCommand<object>((p) =>
            {
                if (ListThongKe != null)
                    return ListThongKe.Count > 0;
                return false;

            }, (p) => 
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { FileName = GetFileNameExcel(), Filter = "Excel Workbook | *.xlsx", ValidateNames = true})
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Application app = new Application();
                        Workbook wb = app.Workbooks.Add(XlSheetType.xlWorksheet);
                        Worksheet ws = (Worksheet)app.ActiveSheet;
                        app.Visible = false;
                        ws.Cells[1, 1] = "Mã hoá đơn";
                        ws.Cells[1, 2] = "Thời gian tạo";
                        ws.Cells[1, 3] = "Tổng tiền";
                        ws.Cells[1, 4] = "Số vé";
                        ws.Cells[1, 5] = "Họ tên";
                        ws.Cells[1, 6] = "Giới tính";
                        ws.Cells[1, 7] = "SĐT";
                        ws.Cells[1, 8] = "Email";
                        ws.Cells[1, 9] = "Ghi chú";
                        int i = 2;
                        foreach(var item in ListThongKe)
                        {
                            ws.Cells[i, 1] = item.MaHoaDon;
                            ws.Cells[i, 2] = item.ThoiGianTao;
                            ws.Cells[i, 3] = item.TongTien;
                            ws.Cells[i, 4] = item.SoVe;
                            ws.Cells[i, 5] = item.HoTen;
                            ws.Cells[i, 6] = item.GioiTinh;
                            ws.Cells[i, 7] = "\t" + item.SDT;
                            ws.Cells[i, 8] = item.Email;
                            ws.Cells[i, 9] = item.GhiChu;
                            i++;
                        }
                        ws.Columns.AutoFit();
                        wb.SaveAs(sfd.FileName, XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, true, false,
                            XlSaveAsAccessMode.xlExclusive, XlSaveConflictResolution.xlLocalSessionChanges,  Type.Missing, Type.Missing);
                        app.Quit();
                        Process.Start(sfd.FileName);
                    }
                }
            });
            BtnBarChartCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                BarChart barChart = new BarChart();
                ListBarChart = new ObservableCollection<DataChart>();
                for (int i = 1; i <= 12; i++)
                {
                    ListBarChart.Add(new DataChart { Month = i, DoanhThu = GetDoanhThuThang(i), LoiNhuan = GetDoanhThuThang(i) / 3});
                }
                TitleChart = "Biểu đồ doanh thu năm " + SelectedYear;
                barChart.ShowDialog();
            });
        }
        public decimal GetDoanhThuThang(int month)
        {
            decimal result = 0;
            foreach (var hoadon in ListThongKe)
            {
                if (hoadon.ThoiGianTao.Value.Month == month)
                    if (hoadon.TongTien.HasValue)
                        result += hoadon.TongTien.Value;
            }
            return result / 1000000;
        }
        public void LoadDateTable()
        {
            ListThongKe = new ObservableCollection<HOADON>(DataProvider.Instance.DB.HOADONs
                .Where(item => item.MaDaiLy.Equals(SelectedDaiLy) && item.ThoiGianTao >= StartDate && item.ThoiGianTao <= EndDate)
                .OrderBy(item => item.ThoiGianTao));
        }
        public void LoadMonthTable()
        {
            ListThongKe = new ObservableCollection<HOADON>(DataProvider.Instance.DB.HOADONs
                .Where(item => item.MaDaiLy.Equals(SelectedDaiLy) && item.ThoiGianTao.Value.Month == SelectedMonth && item.ThoiGianTao.Value.Year == SelectedYear)
                .OrderBy(item => item.ThoiGianTao));
        }
        public void LoadYearTable()
        {
            ListThongKe = new ObservableCollection<HOADON>(DataProvider.Instance.DB.HOADONs
                .Where(item => item.MaDaiLy.Equals(SelectedDaiLy) && item.ThoiGianTao.Value.Year == SelectedYear)
                .OrderBy(item => item.ThoiGianTao));
        }
        public void LoadDetailStatistics()
        {
            TongDoanhThu = TongLoiNhuan = TongSoHoaDon = TongSoVe = 0;
            foreach (var item in ListThongKe)
            {
                if (item.TongTien.HasValue)
                {
                    TongDoanhThu += item.TongTien.Value;
                    TongLoiNhuan += item.TongTien.Value;
                }
                if (item.SoVe.HasValue)
                    TongSoVe += item.SoVe.Value;
            }
            TongSoHoaDon = ListThongKe.Count;
            DetailVisibility = "Visible";
        }
        public string GetFileNameExcel()
        {
            if (StatisticsType == "Ngày")
            {
                if (StartDate.Equals(EndDate))
                    return "Doanh thu ngày " + StartDate.ToString("dd-MM-yyyy");
                return "Doanh thu từ ngày " + StartDate.ToString("dd-MM-yyyy") + " đến ngày " + EndDate.ToString("dd-MM-yyyy");
            }
            else if (StatisticsType == "Tháng")
                return "Doanh thu tháng " + SelectedMonth + " năm " + SelectedYear;
            return "Doanh thu năm " + SelectedYear;
        }
    }
}
