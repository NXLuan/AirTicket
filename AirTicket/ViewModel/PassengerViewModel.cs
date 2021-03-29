using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AirTicket.Model;

namespace AirTicket.ViewModel
{
    class PassengerViewModel : BaseViewModel
    {
        #region commands
        public ICommand AddPassengerCommand { get; set; }
        public ICommand ReducePassengerCommand { get; set; }
        #endregion

        public string typePassenger { get; set; }
        public string regulationAge { get; set; }

        private int _numberOfPassenger;
        private bool _isEnableAdd;
        private bool _isEnableReduce;

        public int NumberOfPassenger
        {
            get => _numberOfPassenger;
            set
            {
                SetProperty(ref _numberOfPassenger, value);
                // enable button reduce passenger
                if (!IsEnableReduce)
                {
                    if (_numberOfPassenger > 1) IsEnableReduce = true;
                    else if (NumberOfPassenger > 0 && !typePassenger.Equals("Người lớn")) IsEnableReduce = true;
                }
                else
                {
                    if (_numberOfPassenger == 1 && typePassenger.Equals("Người lớn")) IsEnableReduce = false;
                    else if (_numberOfPassenger == 0) IsEnableReduce = false;
                }

                // enable button add passenger
                if (_numberOfPassenger == 9) IsEnableAdd = false;
                else IsEnableAdd = true;
            }
        }

        public bool IsEnableAdd
        {
            get => _isEnableAdd;
            set => SetProperty(ref _isEnableAdd, value);
        }

        public bool IsEnableReduce
        {
            get => _isEnableReduce;
            set => SetProperty(ref _isEnableReduce, value);
        }

        public PassengerViewModel()
        {
            AddPassengerCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) =>
            {
                NumberOfPassenger++;
                TextBox total = p as TextBox;
                total.Text = (int.Parse(total.Text) + 1).ToString();
            });

            ReducePassengerCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NumberOfPassenger--;
            });
        }
    }
}
