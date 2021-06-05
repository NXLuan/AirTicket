
CREATE DATABASE AIRTICKET
USE AIRTICKET

CREATE TABLE [HOADON] (
  [MaHoaDon] varchar(16) primary key,
  [ThoiGianTao] DateTime,
  [TongTien] money,
  [HoTen] nvarchar(50),
  [GioiTinh] nvarchar(10),
  [SDT] varchar(20),
  [Email] varchar(200),
  [GhiChu] nvarchar(max)
);

CREATE TABLE [VECHUYENBAY] (
  [MaVe] varchar(16) primary key,
  [GiaVe] money,
  [MaLoai] varchar(10) foreign key references [LOAIHANHKHACH]([MaLoai]),
  [HoTen] nvarchar(50),
  [GioTinh] nvarchar(10),
  [NgaySinh] Date,
  [TrangThai] nvarchar(50),
  [MaHoaDon] varchar(16) foreign key references [HOADON]([MaHoaDon])
);

CREATE TABLE [THAMSO] (
  [MaThamSo] varchar(10) primary key,
  [TenThamSo] nvarchar(200),
);

CREATE TABLE [HANGHANGKHONG] (
  [MaHang] varchar(10) primary key,
  [TenHang] nvarchar(50)
);

CREATE TABLE [SANBAY] (
  [MaSanBay] varchar(10) primary key,
  [ThanhPho] nvarchar(50)
);

CREATE TABLE [LOAIHANHKHACH] (
  [MaLoai] varchar(10) primary key,
  [TenLoai] nvarchar(50),
  [PhotoUrl] varchar(max),
  [TuoiMin] int,
  [TuoiMax] int,
  [SoLuongMin] int,
  [SoLuongMax] int
);

CREATE TABLE [QUYDINHGIAVE] (
  [MaHang] varchar(10) foreign key references [HANGHANGKHONG]([MaHang]),
  [MaLoai] varchar(10) foreign key references [LOAIHANHKHACH]([MaLoai]),
  [TiLe] float,
  [TienGiam] money,
  [TienPhi] money
  constraint PK_QDGV primary key ([MaHang],[MaLoai])
);

CREATE TABLE [NHOMNGUOIDUNG] (
  [MaNhom] varchar(10) primary key,
  [TenNhom] nvarchar(50)
);

CREATE TABLE [NGUOIDUNG] (
  [TenDangNhap] varchar(50) primary key,
  [MatKhau] varchar(50),
  [MaNhom] varchar(10) foreign key references [NHOMNGUOIDUNG]([MaNhom])
);

CREATE TABLE [CHUCNANG] (
  [MaChucNang] varchar(10) primary key,
  [TenChucNang] nvarchar(100),
  [Icon] varchar(50),
  [TenManHinhDuocLoad] nvarchar(100)
);

CREATE TABLE [PHANQUYEN] (
  [MaNhom] varchar(10) foreign key references [NHOMNGUOIDUNG]([MaNhom]),
  [MaChucNang] varchar(10) foreign key references [CHUCNANG]([MaChucNang])
  constraint PK_NCN primary key ([MaNhom],[MaChucNang])
);
