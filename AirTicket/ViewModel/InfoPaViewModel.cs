using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirTicket.ViewModel
{
    class InfoPaViewModel : BaseViewModel
    {
        public ICommand InfoPaLoadCommand { get; set; }

        public ObservableCollection<DetailInfoViewModel> ListDetailInfoVM { get; set; }


        public InfoPaViewModel()
        {
            ListDetailInfoVM = new ObservableCollection<DetailInfoViewModel>();

            InfoPaLoadCommand = new RelayCommand<TabItem>((p) => { return p == null ? false : true; }, (p) =>
            {
                ListDetailInfoVM.Clear();

                TicketSalesViewModel ticketSaleVM = p.DataContext as TicketSalesViewModel;
                int count = 1;

                foreach (PassengerViewModel pvm in ticketSaleVM.listPassengerVM)
                {
                    for (int i = 0; i < pvm.NumberOfPassenger; i++)
                    {
                        DetailInfoViewModel detailInfoViewModel = new DetailInfoViewModel()
                        {
                            num = count,
                            typePassenger = pvm.LHKModel.TenLoai
                        };
                        ListDetailInfoVM.Add(detailInfoViewModel);
                        count++;
                    }

                    if (count > ticketSaleVM.TotalPassenger) break;
                }
            });
        }
    }
}
