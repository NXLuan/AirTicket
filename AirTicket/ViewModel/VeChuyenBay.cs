namespace AirTicket.ViewModel
{
    public class VeChuyenBay : BaseViewModel
    {
        private bool _isSelected;
        private string _GioiTinh;
        private string _HoVaTen;
        private string _MaVeChuyenBay;
        private string _MaHoaDon;
        private int _GiaTien;
        private string _TinhTrang;
        private string _NgaySinh;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
        public string GioiTinh
        {
            get => _GioiTinh;
            set => SetProperty(ref _GioiTinh, value);
        }
        public string MaHoaDon
        {
            get => _MaHoaDon;
            set => SetProperty(ref _MaHoaDon, value);
        }
        public string MaVeChuyenBay
        {
            get => _MaVeChuyenBay;
            set => SetProperty(ref _MaVeChuyenBay, value);
        }

        public string HoVaTen
        {
            get => _HoVaTen;
            set => SetProperty(ref _HoVaTen, value);
        }

        public int GiaTien
        {
            get => _GiaTien;
            set => SetProperty(ref _GiaTien, value);
        }

        public string TinhTrang
        {
            get => _TinhTrang;
            set => SetProperty(ref _TinhTrang, value);
        }
        public string NgaySinh
        {
            get => _NgaySinh;
            set => SetProperty(ref _NgaySinh, value);
        }
    }
}
