using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
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
        public DateTime? _validatingDate;
        private int _totalPassenger;
        private string _isLoading;
        private bool _isEnableButtonSearch;

        public DateTime? ValidatingDate
        {
            get => _validatingDate;
            set => SetProperty(ref _validatingDate, value);
        }

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

        public bool IsEnableButtonSearch
        {
            get => _isEnableButtonSearch;
            set => SetProperty(ref _isEnableButtonSearch, value);
        }

        public ObservableCollection<PassengerViewModel> listPassengerVM { get; }
        private ObservableCollection<FlightViewModel> _listFlightVM;
        public ObservableCollection<FlightViewModel> listFlightVM { get => _listFlightVM; set => SetProperty(ref _listFlightVM, value); }
        public ICommand ReturnCommand { get; set; }
        public ICommand SearchTicketCommand { get; set; }

        // public IList
        public TicketSalesViewModel()
        {
            ValidatingDate = DateTime.Now.Date;
            Done();

            listPassengerVM = new ObservableCollection<PassengerViewModel>()
            {
                new PassengerViewModel() { typePassenger = "Người lớn", regulationAge = "12 tuổi trở lên", NumberOfPassenger = 1 },
                new PassengerViewModel() { typePassenger = "Trẻ em", regulationAge = "Từ 2 đến dưới 12 tuổi", NumberOfPassenger = 0 },
                new PassengerViewModel() { typePassenger = "Em bé", regulationAge = "Nhỏ hơn 2 tuổi", NumberOfPassenger = 0 }
            };

            TotalPassenger = 0;
            foreach (PassengerViewModel vm in listPassengerVM) TotalPassenger += vm.NumberOfPassenger;

            listFlightVM = new ObservableCollection<FlightViewModel>();

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
            string url = "https://demo.datacom.vn/flight?Request=HANSGN14042021-1-0-0&Airline=VN,VJ,QH,VU";
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
            IsEnableButtonSearch = false;
        }

        public void Done()
        {
            IsLoading = "Hidden";
            IsEnableButtonSearch = true;
        }
    }
}
