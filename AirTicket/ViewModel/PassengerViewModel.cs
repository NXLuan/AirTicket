using AirTicket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirTicket.ViewModel
{
    class PassengerViewModel : BaseViewModel
    {
        #region commands
        public ICommand AddPassengerCommand { get; set; }
        public ICommand ReducePassengerCommand { get; set; }
        #endregion

        private LOAIHANHKHACH _lhkModel;

        public LOAIHANHKHACH LHKModel {
            get
            {
                return _lhkModel;
            } 
            set
            {
                _lhkModel = value;

                if (_lhkModel.TuoiMax == null) regulationAge = _lhkModel.TuoiMin + " tuổi trở lên";
                else if (_lhkModel.TuoiMin == null) regulationAge = "Nhỏ hơn " + (++_lhkModel.TuoiMax) + " tuổi";
                else regulationAge = "Từ " + _lhkModel.TuoiMin + " đến dưới " + (++_lhkModel.TuoiMax) + " tuổi";

                NumberOfPassenger = (int)_lhkModel.SoLuongMin;
            }
        }

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

                if (_numberOfPassenger == _lhkModel.SoLuongMin)
                {
                    IsEnableReduce = false;
                    IsEnableAdd = true;
                }
                else if (_numberOfPassenger == _lhkModel.SoLuongMax)
                {
                    IsEnableReduce = true;
                    IsEnableAdd = false;
                }
                else
                {
                    IsEnableReduce = true;
                    IsEnableAdd = true;
                }
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
                p.Text = (int.Parse(p.Text) + 1).ToString();
            });

            ReducePassengerCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) =>
            {
                NumberOfPassenger--;
                p.Text = (int.Parse(p.Text) - 1).ToString();
            });
        }
    }
}
