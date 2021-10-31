using AirTicket.Model;
using AirTicket.Utilities;
using Estant.Material.Utilities;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
namespace AirTicket.ViewModel
{
    class TicketSalesViewModel : BaseViewModel
    {
        #region Constructor
        private int _selectIndexTab;

        public int SelectIndexTab
        {
            get => _selectIndexTab;
            set => SetProperty(ref _selectIndexTab, value);
        }

        public TicketSalesViewModel()
        {
            HandleInit();
            HandleEvent();
        }

        public void HandleInit()
        {
            HandleInitTabSearch();
            HandleInitTabChoose();
            HandleInitTabInfo();
        }

        public void HandleEvent()
        {
            HandleEventTabSearch();
            HandleEventTabChoose();
            HandleEventTabInfo();
        }
        #endregion

        #region Tra cứu chuyến bay

        #region Variable
        private int _totalPassenger;
        public int TotalPassenger
        {
            get => _totalPassenger;
            set => SetProperty(ref _totalPassenger, value);
        }

        private bool _isKhuHoi;
        public bool IsKhuHoi
        {
            get => _isKhuHoi;
            set
            {
                _isKhuHoi = value;
                if (value) DateReturn = _dateDeparture;
            }
        }

        private DateTime? _dateDeparture;
        public DateTime? DateDeparture
        {
            get => _dateDeparture;
            set
            {
                _dateDeparture = value;
                if (IsKhuHoi && _dateReturn < _dateDeparture) DateReturn = value;
            }

        }

        private DateTime? _dateReturn;
        public DateTime? DateReturn
        {
            get => _dateReturn;
            set => SetProperty(ref _dateReturn, value);
        }

        #endregion

        #region ObservableCollection
        public ObservableCollection<PassengerViewModel> ListPassengerVM { get; set; }
        private ObservableCollection<AirlineSelected> _listAirline;
        public ObservableCollection<AirlineSelected> ListAirline
        {
            get => _listAirline;
            set => SetProperty(ref _listAirline, value);
        }
        private ObservableCollection<SANBAY> _listAirport;
        public ObservableCollection<SANBAY> ListAirport
        {
            get => _listAirport;
            set => SetProperty(ref _listAirport, value);
        }
        #endregion

        #region commands
        public ICommand SearchTicketCommand { get; set; }
        public ICommand TabSearchLoadCommand { get; set; }
        #endregion

        #region Combobox Departure
        public IEnumerable<SANBAY> ListDeparture
        {
            get
            {
                if (SelectedDestination == null) return ListAirport;
                return ListAirport.Where(x => x.MaSanBay != SelectedDestination.MaSanBay);
            }
        }
        private SANBAY _selectedDeparture;
        public SANBAY SelectedDeparture
        {
            get { return _selectedDeparture; }
            set
            {
                if (_selectedDeparture != value)
                {
                    _selectedDeparture = value;
                    OnPropertyChanged("SelectedDeparture");
                    OnPropertyChanged("ListDestination");
                }
            }
        }
        #endregion

        #region Combobox Destination
        public IEnumerable<SANBAY> ListDestination
        {
            get
            {
                if (SelectedDeparture == null) return ListAirport;
                return ListAirport.Where(x => x.MaSanBay != SelectedDeparture.MaSanBay);
            }
        }
        private SANBAY _selectedDestination;
        public SANBAY SelectedDestination
        {
            get { return _selectedDestination; }
            set
            {
                if (_selectedDestination != value)
                {
                    _selectedDestination = value;
                    OnPropertyChanged("SelectedDestination");
                    OnPropertyChanged("ListDeparture");
                }
            }
        }
        #endregion

        private void HandleInitTabSearch()
        {
            ListAirline = new ObservableCollection<AirlineSelected>();
            var airlineList = DataProvider.Instance.DB.HANGHANGKHONGs;
            foreach (var airline in airlineList)
            {
                AirlineSelected airlineSelected = new AirlineSelected();
                airlineSelected.HHKModel = airline;
                airlineSelected.isSelected = true;
                ListAirline.Add(airlineSelected);
            }

            ListAirport = new ObservableCollection<SANBAY>(DataProvider.Instance.DB.SANBAYs);

            TotalPassenger = 0;
            ListPassengerVM = new ObservableCollection<PassengerViewModel>();
            foreach (var p in DataProvider.Instance.DB.LOAIHANHKHACHes)
            {
                ListPassengerVM.Add(new PassengerViewModel() { LHKModel = p });
                TotalPassenger += (int)p.SoLuongMin;
            }
            DateDeparture = DateTime.Today;
        }

        private void HandleEventTabSearch()
        {
            TabSearchLoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                SelectTabSearch();
            });

            SearchTicketCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                if (IsKhuHoi)
                {
                    if (FlightListVMs.Count == 1)
                        FlightListVMs.Add(new ListFlightViewModel());
                }
                else if (FlightListVMs.Count == 2) FlightListVMs.RemoveAt(1);
                SelectTabChoose(true);
            });
        }

        //private void SearchTicket()
        //{
        //string url = "https://demo.datacom.vn/flight?Request="
        //    + SelectedDeparture.MaSanBay + SelectedDestination.MaSanBay + String.Format("{0:ddMMyyyy}", DateDeparture); //HANSGN14042021-1-0-0&Airline=VN,VJ,QH,VU";
        ////add num of each typ passenger in url
        //foreach (PassengerViewModel pvm in listPassengerVM)
        //{
        //    url += "-" + pvm.NumberOfPassenger;
        //}
        //url += "&Airline=";
        ////add airline you want search in url
        //foreach (AirlineSelected airline in ListAirline)
        //{
        //    if (airline.isSelected)
        //        url += airline.HHKModel.MaHang + ",";
        //}
        //url = url.TrimEnd(',');
        //var chromeOption = new ChromeOptions();
        //chromeOption.AddArguments("headless");
        //var chromeDriverService = ChromeDriverService.CreateDefaultService();
        //chromeDriverService.HideCommandPromptWindow = true;
        //using (var browser = new ChromeDriver(chromeDriverService, chromeOption))
        //{
        //    try
        //    {
        //        browser.Navigate().GoToUrl(url);
        //        var wait = new WebDriverWait(browser, new TimeSpan(0, 0, 30));
        //        var allFlights = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("dtc-flight-main")));
        //        var doc = new HtmlDocument(); 
        //        doc.LoadHtml(allFlights.GetAttribute("outerHTML"));
        //        var htmlNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'dtc-flight-item ')]");
        //        if (htmlNodes != null)
        //        {
        //            var nodes = htmlNodes.ToArray();
        //            for (int i = 0; i < nodes.Length; i++)
        //            {
        //                //string FlightName = nodes[i].SelectSingleNode(".//p").InnerText;
        //                //if (FlightName.Contains("Pacific Airline"))
        //                //    FlightName = "Pacific Airline";

        //                FlightViewModel flight = new FlightViewModel()
        //                {
        //                    ImgAirlineUrl = nodes[i].SelectSingleNode(".//img").Attributes[0].Value,
        //                    flight = nodes[i].SelectSingleNode(".//div[@class='dtc-flight-numb dtc-color-text']").InnerText,
        //                    departureTime = nodes[i].SelectNodes(".//div[@class='dtc-flight-time']")[0].InnerText,
        //                    landingTime = nodes[i].SelectNodes(".//div[@class='dtc-flight-time']")[1].InnerText,
        //                    priceFlight = nodes[i].SelectSingleNode(".//div[@class='dtc-flight-price']").InnerText
        //                };

        //                App.Current.Dispatcher.Invoke((Action)delegate
        //                {
        //                    listFlightVM.Add(flight);
        //                });
        //            }
        //        }
        //        else
        //        {
        //            IsNoResult = "Visible";
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        IsNoResult = "Visible";
        //    }
        //    Done();
        //    browser.Close();
        //    browser.Quit();
        //}
        //}

        public void SelectTabSearch()
        {
            IsEnableTabChoose = false;
            IsEnableTabInfo = false;
            SelectIndexTab = 0;
        }

        #endregion

        #region Chọn chuyến bay
        private bool _isEnableTabChoose;
        public bool IsEnableTabChoose
        {
            get => _isEnableTabChoose;
            set => SetProperty(ref _isEnableTabChoose, value);
        }

        /// <summary>
        /// FlightListVMs[0] => list chuyến đi, FlightListVMs[1] => list chuyến về
        /// </summary>
        public ObservableCollection<ListFlightViewModel> FlightListVMs { get; set; }

        #region command
        public ICommand SelectFlightCommand { get; set; }
        public ICommand TabChooseLoadCommand { get; set; }
        #endregion

        private void HandleInitTabChoose()
        {
            FlightListVMs = new ObservableCollection<ListFlightViewModel>()
            {
                new ListFlightViewModel()
            };
        }

        private void HandleEventTabChoose()
        {
            TabChooseLoadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectIndexTab == 0) return;
                SelectTabChoose();
            });

            SelectFlightCommand = new RelayCommand<FlightModel>((p) => { return p != null; }, (p) =>
            {
                if (IsKhuHoi && FlightListVMs[1].ListFlightVM.Contains(p))
                    FlightListVMs[1].FlightSelected = FlightListVMs[1].FlightSelected == null ? p : null;
                else
                    FlightListVMs[0].FlightSelected = FlightListVMs[0].FlightSelected == null ? p : null;

                foreach (var listFlight in FlightListVMs)
                    if (listFlight.FlightSelected == null) return;

                // nếu đã chọn đủ chuyến bay chuyển sang tab thông tin hành khách 
                SelectTabInfoPassenger();
            });
        }

        public void SelectTabChoose(bool isSearch = false)
        {
            IsEnableTabChoose = true;
            IsEnableTabInfo = false;
            SelectIndexTab = 1;

            if (isSearch) SearchFlight();
            else
                // Nếu không phải chuyển từ trang search sang, thì clear chuyến bay đã chọn
                foreach (var listFlight in FlightListVMs) listFlight.FlightSelected = null;
        }

        public async void SearchFlight()
        {
            int count = IsKhuHoi ? 2 : 1;

            // trước khi xử lý
            for (int i = 0; i < count; i++)
            {
                // 0 là chuyến đi, 1 là chuyến về
                FlightListVMs[i].Departure = i == 0 ? SelectedDeparture.ThanhPho : SelectedDestination.ThanhPho;
                FlightListVMs[i].Destination = i == 0 ? SelectedDestination.ThanhPho : SelectedDeparture.ThanhPho;

                var flightDate = i == 0 ? DateDeparture : DateReturn;
                var culture = new CultureInfo("vi-VI");
                var day = culture.DateTimeFormat.GetDayName(flightDate.Value.DayOfWeek);
                FlightListVMs[i].FlightDate = String.Format("{0}, {1:dd-MM-yyyy}", day, flightDate);
                FlightListVMs[i].DepartureDate = String.Format("{0:dd-MM}", flightDate);
                FlightListVMs[i].Load();
            }

            // chờ xử lý
            List<ObservableCollection<FlightModel>> ListFlights = await GetDataFlightAsync();

            // sau khi xử lý xong
            for (int i = 0; i < count; i++)
            {
                FlightListVMs[i].ListFlightVM = ListFlights[i];
                FlightListVMs[i].Done();
            }
        }

        public DataFlightRequest CreateDataRequest(string AirlineCode)
        {
            var objDataFlightRequest = new DataFlightRequest
            {
                Adt = ListPassengerVM[0].NumberOfPassenger,
                Chd = ListPassengerVM[1].NumberOfPassenger,
                Inf = ListPassengerVM[2].NumberOfPassenger,
                ProductKey = "r1e0q6z8md6akul",
                ViewMode = false
            };
            if (IsKhuHoi)
            {
                objDataFlightRequest.ListFlight = new SearchFlightInfo[2]
                {
                    new SearchFlightInfo
                    {
                        Airline = AirlineCode,
                        DepartDate = String.Format("{0:ddMMyyyy}", DateDeparture),
                        EndPoint = SelectedDestination.MaSanBay,
                        StartPoint = SelectedDeparture.MaSanBay,
                    },
                    new SearchFlightInfo
                    {
                        Airline = AirlineCode,
                        DepartDate = String.Format("{0:ddMMyyyy}", DateReturn),
                        EndPoint = SelectedDeparture.MaSanBay,
                        StartPoint = SelectedDestination.MaSanBay,
                    }
                };
            }
            else
            {
                objDataFlightRequest.ListFlight = new SearchFlightInfo[1]
                {
                    new SearchFlightInfo
                    {
                        Airline = AirlineCode,
                        DepartDate = String.Format("{0:ddMMyyyy}", DateDeparture),
                        EndPoint = SelectedDestination.MaSanBay,
                        StartPoint = SelectedDeparture.MaSanBay,
                    }
                };
            }
            return objDataFlightRequest;
        }
        public async Task<ObservableCollection<FlightModel>> GetListFlight(string AirlineCode)
        {
            var objDataFlightRequest = CreateDataRequest(AirlineCode);
            var stringPayload = JsonConvert.SerializeObject(objDataFlightRequest);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync("https://plugin.datacom.vn/flightsearch", httpContent);
                var responseString = await response.Content.ReadAsStringAsync();
                JObject objJson = JObject.Parse(responseString);
                ObservableCollection<FlightModel> lstFlightModels = new ObservableCollection<FlightModel>();
                foreach (var data in objJson["DomesticDatas"])
                {
                    lstFlightModels.Add(new FlightModel
                    {
                        ImgAirlineUrl = "https://plugin.datacom.vn/Resources/Images/Airline/" + AirlineCode + ".gif",
                        AirlineID = data["Airline"].ToString(),
                        Flight = data["FlightNumber"].ToString(),
                        DepartureTime = data["StartTime"].ToString(),
                        LandingTime = data["EndTime"].ToString(),
                        PriceFlight = data["FareAdtFull"].ToString(),
                        ChildrenPrice = Convert.ToDecimal(data["FareChdFull"]),
                        InfantPrice = Convert.ToDecimal(data["FareInfFull"]),
                        StartDate = data["StartDate"].ToString()
                    });

                }
                return lstFlightModels;
            }
        }

        public async Task<List<ObservableCollection<FlightModel>>> GetDataFlightAsync()
        {
            List<ObservableCollection<FlightModel>> data = new List<ObservableCollection<FlightModel>>();
            ObservableCollection<FlightModel> lstDepart = new ObservableCollection<FlightModel>();
            ObservableCollection<FlightModel> lstReturn = new ObservableCollection<FlightModel>();

            var requestHelper = new MultiRequestHelper<ObservableCollection<FlightModel>>();
            foreach (AirlineSelected airline in ListAirline)
            {
                if (airline.isSelected)
                {
                    requestHelper.AddRequest(GetListFlight(airline.HHKModel.MaHang));
                }
            }

            var responses = await requestHelper.Execute();
            foreach (var lstFlight in responses)
            {
                if (IsKhuHoi)
                {
                    int i;
                    for (i = 0; i < lstFlight.Count; i++)
                    {
                        if (lstFlight[i].StartDate == String.Format("{0:dd-MM-yyyy}", DateDeparture))
                        {
                            lstDepart.Add(lstFlight[i]);
                        }
                        else
                        {
                            lstReturn.Add(lstFlight[i]);
                        }
                    }
                }
                else
                {
                    lstDepart = new ObservableCollection<FlightModel>(lstDepart.Concat(lstFlight));
                }
            }

            // set await trước hàm call api xử lý bất đồng bộ 
            // xử lý data trả về

            // Test data[0] => list chuyến đi, data[1] => list chuyến về
            //int count = IsKhuHoi ? 2 : 1;
            //for (int i = 0; i < count; i++)
            //{
            //    ObservableCollection<FlightModel> listFlightVM = new ObservableCollection<FlightModel>();
            //    listFlightVM.Add(new FlightModel() { AirlineID = "vu", ImgAirlineUrl = "https://plugin.datacom.vn/Resources/Images/Airline/vu.gif", Flight = "VU140", DepartureTime = "9:35", LandingTime = "14:00", PriceFlight = "600,000 VND" });
            //    listFlightVM.Add(new FlightModel() { AirlineID = "vj", ImgAirlineUrl = "https://plugin.datacom.vn/Resources/Images/Airline/vj.gif", Flight = "VJ140", DepartureTime = "9:35", LandingTime = "14:00", PriceFlight = "700,000 VND" });
            //    data.Add(listFlightVM);
            //}
            data.Add(lstDepart);
            if (IsKhuHoi)
            {
                data.Add(lstReturn);
            }
            return data;
        }
        #endregion

        #region Thông tin hành khách

        #region Variable
        private bool _isEnableTabInfo;
        public bool IsEnableTabInfo
        {
            get => _isEnableTabInfo;
            set => SetProperty(ref _isEnableTabInfo, value);
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

        public HOADON HDModel { get; set; }

        private List<PassengerViewModel> tickets;
        #endregion

        #region ObservableCollection
        public ObservableCollection<DetailInfoPassenger> ListDetailInfo { get; set; }
        public ObservableCollection<PassengerViewModel> ListPriceTicket { get; set; }
        #endregion

        #region Command
        public ICommand btnBookClickCommand { get; set; }
        public ICommand btnSelectAgainCommand { get; set; }
        #endregion

        private void HandleInitTabInfo()
        {
            HDModel = new HOADON();
            ListDetailInfo = new ObservableCollection<DetailInfoPassenger>();
            ListPriceTicket = new ObservableCollection<PassengerViewModel>();
        }

        private void HandleEventTabInfo()
        {
            btnBookClickCommand = new RelayCommand<object>((p) => { return ValidDependcyObject.IsValid(p as DependencyObject); }, (p) =>
            {
                BookTicket();
            });

            btnSelectAgainCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SelectTabChoose();
            });
        }

        public void SelectTabInfoPassenger()
        {
            IsEnableTabInfo = true;
            SelectIndexTab = 2;
            InitDataTabInfo();
        }

        public void InitDataTabInfo()
        {
            //var QuyDinhGiaVe = DataProvider.Instance.DB.QUYDINHGIAVEs.Where(x=>x.MaHang == FlightSelected.AirlineID);
            int count = 1;
            ListDetailInfo.Clear();
            ListPriceTicket.Clear();
            TotalPriceTicket = 0;
            TotalNetprofitTicket = 0;
            TotalCancellationCostTicket = 0;

            // xử lý thông tin vé
            tickets = new List<PassengerViewModel>();
            foreach (PassengerViewModel pvm in ListPassengerVM)
            {
                if (pvm.NumberOfPassenger != 0)
                {
                    foreach (var listFlight in FlightListVMs)
                    {
                        var FlightSelected = listFlight.FlightSelected;
                        var ticket = new PassengerViewModel()
                        {
                            LHKModel = pvm.LHKModel,
                            NumberOfPassenger = pvm.NumberOfPassenger,
                        };

                        decimal price = ParseStringToDecimal(FlightSelected.PriceFlight);
                        QUYDINHGIAVE QuyDinhGiaVe = pvm.LHKModel.QUYDINHGIAVEs.Where(x => x.MaHang.ToLower() == FlightSelected.AirlineID.ToLower()).First();
                        ticket.NetProfitTicket = (decimal)QuyDinhGiaVe.TienLaiVe;
                        ticket.CancellationCostTicket = (decimal)QuyDinhGiaVe.TienHuyVe;
                        ticket.PriceTicket = (decimal)(price * (decimal)QuyDinhGiaVe.TiLe - QuyDinhGiaVe.TienGiam + QuyDinhGiaVe.TienPhi) + ticket.NetProfitTicket;

                        pvm.PriceTicket += ticket.PriceTicket;
                        TotalPriceTicket += ticket.TotalPriceTicket;
                        TotalNetprofitTicket += ticket.TotalNetProfitTicket;
                        TotalCancellationCostTicket += ticket.TotalCancellationCostTicket;

                        tickets.Add(ticket);
                    }
                    ListPriceTicket.Add(pvm);

                    // xử lý thông tin danh sách hành khách
                    for (int i = 0; i < pvm.NumberOfPassenger; i++)
                    {
                        DetailInfoPassenger detailInfo = new DetailInfoPassenger()
                        {
                            num = count,
                        };
                        detailInfo.VCBModel.LOAIHANHKHACH = pvm.LHKModel;
                        ListDetailInfo.Add(detailInfo);
                        count++;
                    }
                }
            }

            // InfoDateTimeDeparture = FlightSelected.DepartureTime + " " + InfoDateDeparture;
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
                HDModel.SoVe = tickets.Count;
                DataProvider.Instance.DB.HOADONs.Add(HDModel);
                var details = ListDetailInfo.ToList();
                foreach (var ticket in tickets)
                {
                    var detail = details.Find(o => o.VCBModel.LOAIHANHKHACH.MaLoai == ticket.LHKModel.MaLoai);

                    var VCBModel = new VECHUYENBAY()
                    {
                        HOADON = HDModel,
                        MaVe = DataProvider.Instance.DB.Database.SqlQuery<string>("select dbo.AUTO_IDVCB()").Single(),
                        GiaVe = ticket.PriceTicket,
                        LOAIHANHKHACH = ticket.LHKModel,
                        GioTinh = detail.VCBModel.GioTinh,
                        HoTen = detail.VCBModel.HoTen,
                        NgaySinh = detail.VCBModel.NgaySinh
                    };
                    VeChuyenBay.Add(VCBModel);
                    DataProvider.Instance.DB.SaveChanges();
                }
                MessageBox.Show("Đặt vé thành công mã HD: " + HDModel.MaHoaDon, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                SelectTabSearch();
            }
            catch (Exception e)
            {
                MessageBox.Show("Thất bại: " + e.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
