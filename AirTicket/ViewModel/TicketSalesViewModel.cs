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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
namespace AirTicket.ViewModel
{
    class TicketSalesViewModel : BaseViewModel
    {
        private int _totalPassenger;
        private string _isLoading;

        public int TotalPassenger
        {
            get => _totalPassenger;
            set => SetProperty(ref _totalPassenger, value);
        }

        public string IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public DateTime? DateDeparture { get; set; }

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

        public IEnumerable<SANBAY> ListDeparture
        {
            get
            {
                return ListAirport.Where(x => x.MaSanBay != SelectedDestination);
            }
        }
        private string _selectedDeparture;
        public string SelectedDeparture
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
        public IEnumerable<SANBAY> ListDestination
        {
            get
            {
                return ListAirport.Where(x => x.MaSanBay != SelectedDeparture);
            }
        }
        private string _selectedDestination;
        public string SelectedDestination
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
        public ICommand ReturnCommand { get; set; }
        public ICommand SearchTicketCommand { get; set; }

        public TicketSalesViewModel()
        {
            Done();

            listFlightVM = new ObservableCollection<FlightViewModel>();
            listFlightVM.Add(new FlightViewModel() { airline ="https://plugin.datacom.vn/Resources/Images/Airline/VJ.gif"});

            ReturnCommand = new RelayCommand<TabControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                TabControl tab = p as TabControl;
                TabItem tabItem = tab.Items[1] as TabItem;
                tabItem.IsEnabled = false;
            });

            SearchTicketCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                Load();
                Thread threadSearch = new Thread(new ThreadStart(this.SearchTicket));
                threadSearch.IsBackground = true;
                threadSearch.Start();
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
        private void SearchTicket()
        {

            string url = "https://demo.datacom.vn/flight?Request="
                + SelectedDeparture + SelectedDestination + String.Format("{0:ddMMyyyy}", DateDeparture); //HANSGN14042021-1-0-0&Airline=VN,VJ,QH,VU";
            //add num of each typ passenger in url
            foreach (PassengerViewModel pvm in listPassengerVM)
            {
                url += "-" + pvm.NumberOfPassenger;
            }
            url += "&Airline=";
            //add airline you want search in url
            foreach (AirlineSelected airline in ListAirline)
            {
                if (airline.isSelected)
                    url += airline.HHKModel.MaHang + ",";
            }
            url = url.TrimEnd(',');
            var chromeOption = new ChromeOptions();
            chromeOption.AddArguments("headless");
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            using (var browser = new ChromeDriver(chromeDriverService, chromeOption))
            {
                browser.Navigate().GoToUrl(url);
                var wait = new WebDriverWait(browser, new TimeSpan(0, 0, 30));
                var allFlights = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("dtc-flight-main")));
                var doc = new HtmlDocument();
                doc.LoadHtml(allFlights.GetAttribute("outerHTML"));
                var nodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'dtc-flight-item ')]").ToArray();
                for (int i = 0; i < nodes.Length; i++)
                {
                    string FlightName = nodes[i].SelectSingleNode(".//p").InnerText;
                    if (FlightName.Contains("Pacific Airline"))
                        FlightName = "Pacific Airline";

                    FlightViewModel flight = new FlightViewModel()
                    {
                        airline = nodes[i].SelectSingleNode(".//img").Attributes[0].Value,
                        flight = nodes[i].SelectSingleNode(".//div[@class='dtc-flight-numb dtc-color-text']").InnerText,
                        departureTime = nodes[i].SelectNodes(".//div[@class='dtc-flight-time']")[0].InnerText,
                        landingTime = nodes[i].SelectNodes(".//div[@class='dtc-flight-time']")[1].InnerText,
                        priceFlight = nodes[i].SelectSingleNode(".//div[@class='dtc-flight-price']").InnerText
                    };

                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        listFlightVM.Add(flight);
                    });
                }
                Done();
                browser.Close();
                browser.Quit();
            }
        }

        public void Load()
        {
            if (listFlightVM.Count != 0)
                listFlightVM.Clear();
            IsLoading = "Visible";
        }

        public void Done()
        {
            IsLoading = "Hidden";
        }
    }
}
