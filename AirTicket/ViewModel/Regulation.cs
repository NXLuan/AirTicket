namespace AirTicket.ViewModel
{
    public class Regulation : BaseViewModel
    {
        private string _MaHang;
        private string _TenHang;
        private string _MaLoai;
        private string _TenLoai;
        private double _TiLe;
        private int _TienGiam;
        private int _TienPhi;
        private int _TienLaiVe;
        private int _TienHuyVe;


        public string MaHang
        {
            get => _MaHang;
            set => SetProperty(ref _MaHang, value);
        }
        public string TenHang
        {
            get => _TenHang;
            set => SetProperty(ref _TenHang, value);
        }
        public string MaLoai
        {
            get => _MaLoai;
            set => SetProperty(ref _MaLoai, value);
        }
        public string TenLoai
        {
            get => _TenLoai;
            set => SetProperty(ref _TenLoai, value);
        }
        public double TiLe
        {
            get => _TiLe;
            set => SetProperty(ref _TiLe, value);
        }
        public int TienGiam
        {
            get => _TienGiam;
            set => SetProperty(ref _TienGiam, value);
        }
        public int TienPhi
        {
            get => _TienPhi;
            set => SetProperty(ref _TienPhi, value);
        }
        public int TienLaiVe
        {
            get => _TienLaiVe;
            set => SetProperty(ref _TienLaiVe, value);
        }
        public int TienHuyVe
        {
            get => _TienHuyVe;
            set => SetProperty(ref _TienHuyVe, value);
        }
    }
}
