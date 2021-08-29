using AirTicket.Model;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
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
            if (ListInfoPassengerVM == null) ListInfoPassengerVM = new ObservableCollection<PassengerViewModel>();
            FlightListVMs = new ObservableCollection<ListFlightViewModel>()
            {
                new ListFlightViewModel()
            };

            ReturnCommand = new RelayCommand<TabControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                TabControl tab = p as TabControl;
                TabItem tabItem = tab.Items[1] as TabItem;
                tabItem.IsEnabled = false;
            });

            SearchTicketCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                //InfoDeparture = SelectedDeparture.ThanhPho;
                //InfoDestination = SelectedDestination.ThanhPho;
                //InfoDateDeparture = String.Format("{0:dd-MM}", DateDeparture);
                //ListInfoPassengerVM.Clear();
                //foreach (PassengerViewModel passengerVM in listPassengerVM)
                //{
                //    ListInfoPassengerVM.Add(new PassengerViewModel() { LHKModel = passengerVM.LHKModel, NumberOfPassenger = passengerVM.NumberOfPassenger });
                //}
                //Load();
                //Thread threadSearch = new Thread(new ThreadStart(this.SearchTicket));
                //threadSearch.IsBackground = true;
                //threadSearch.Start();

                if (IsKhuHoi)
                {
                    FlightListVMs.Add(new ListFlightViewModel());
                }
                else if (FlightListVMs.Count == 2) FlightListVMs.RemoveAt(1);
                SelectTabChoose();
            });


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
            listPassengerVM = new ObservableCollection<PassengerViewModel>();
            foreach (var p in DataProvider.Instance.DB.LOAIHANHKHACHes)
            {
                listPassengerVM.Add(new PassengerViewModel() { LHKModel = p });
                TotalPassenger += (int)p.SoLuongMin;
            }
            DateDeparture = DateTime.Today;

            IsEnableTabChoose = false;
        }
        #endregion

        #region Tra cứu chuyến bay
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


        public ObservableCollection<PassengerViewModel> listPassengerVM { get; set; }
        public ObservableCollection<FlightViewModel> listFlightVM { get; set; }
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

        #region Detail Info Flight Selected
        private string _infoDeparture;
        public string InfoDeparture
        {
            get => _infoDeparture;
            set => SetProperty(ref _infoDeparture, value);
        }

        private string _infoDestination;
        public string InfoDestination
        {
            get => _infoDestination;
            set => SetProperty(ref _infoDestination, value);
        }

        public string InfoDateDeparture { get; set; }

        public FlightViewModel FlightSelected { get; set; }

        public ObservableCollection<PassengerViewModel> ListInfoPassengerVM { get; set; }
        #endregion

        #region commands
        public ICommand ReturnCommand { get; set; }
        public ICommand SearchTicketCommand { get; set; }
        #endregion

        private void SearchTicket()
        {

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

        }
        #endregion

        #region Chọn chuyến bay
        private bool _isEnableTabChoose;
        public bool IsEnableTabChoose
        {
            get => _isEnableTabChoose;
            set => SetProperty(ref _isEnableTabChoose, value);
        }
        public ObservableCollection<ListFlightViewModel> FlightListVMs { get; set; }

        public void SelectTabChoose()
        {
            IsEnableTabChoose = true;
            SelectIndexTab = 1;
        }
        #endregion
    }
}
