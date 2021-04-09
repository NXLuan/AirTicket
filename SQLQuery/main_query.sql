--Chạy từng lệnh một
CREATE DATABASE AIRTICKET
USE AIRTICKET

CREATE TABLE [HOADON] (
  [MaHoaDon] varchar(16) primary key,
  [ThoiGianTao] DateTime,
  [TongTien] money,
  [HoTen] nvarchar(50),
  [GioiTinh] nvarchar(10),
  [SDT] varchar(20),
  [Email] varchar(200)
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
  [TenThamSo] nvarchar(200) primary key,
  [GiaTri] float
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
  [TiLe] float
  constraint PK_QDGV primary key ([MaHang],[MaLoai])
);

CREATE FUNCTION AUTO_IDHD()
RETURNS VARCHAR(16)
AS
BEGIN
	DECLARE @id VARCHAR(16)
	IF (SELECT COUNT(MaHoaDon) FROM HOADON) = 0
		SET @id = 0
	ELSE
		SELECT @id = MAX(RIGHT(MaHoaDon, 5)) FROM HOADON
		SELECT @id = CASE
			WHEN @id = 99999 THEN CONVERT(VARCHAR,GETDATE(),112) + 'HD00001'
			WHEN @id >= 0 and @id < 9 THEN CONVERT(VARCHAR,GETDATE(),112) + 'HD0000' + CONVERT(CHAR, CONVERT(INT, @id) + 1)
			WHEN @id >= 9 THEN CONVERT(VARCHAR,GETDATE(),112) + 'HD000' + CONVERT(CHAR, CONVERT(INT, @id) + 1)
			WHEN @id >= 99 THEN CONVERT(VARCHAR,GETDATE(),112) + 'HD00' + CONVERT(CHAR, CONVERT(INT, @id) + 1)
			WHEN @id >= 999 THEN CONVERT(VARCHAR,GETDATE(),112) + 'HD0' + CONVERT(CHAR, CONVERT(INT, @id) + 1)
			WHEN @id >= 9999 THEN CONVERT(VARCHAR,GETDATE(),112) + 'HD' + CONVERT(CHAR, CONVERT(INT, @id) + 1)
		END
	RETURN @id
END
 
CREATE FUNCTION AUTO_IDVCB()
RETURNS VARCHAR(16)
AS
BEGIN
	DECLARE @id VARCHAR(16)
	IF (SELECT COUNT(MaVe) FROM VECHUYENBAY) = 0
		SET @id = 0
	ELSE
		SELECT @id = MAX(RIGHT(MaVe, 5)) FROM VECHUYENBAY
		SELECT @id = CASE
			WHEN @id = 99999 THEN CONVERT(VARCHAR,GETDATE(),112) + 'VCB00001'
			WHEN @id >= 0 and @id < 9 THEN CONVERT(VARCHAR,GETDATE(),112) + 'VCB0000' + CONVERT(CHAR, CONVERT(INT, @id) + 1)
			WHEN @id >= 9 THEN CONVERT(VARCHAR,GETDATE(),112) + 'VCB000' + CONVERT(CHAR, CONVERT(INT, @id) + 1)
			WHEN @id >= 99 THEN CONVERT(VARCHAR,GETDATE(),112) + 'VCB00' + CONVERT(CHAR, CONVERT(INT, @id) + 1)
			WHEN @id >= 999 THEN CONVERT(VARCHAR,GETDATE(),112) + 'VCB0' + CONVERT(CHAR, CONVERT(INT, @id) + 1)
			WHEN @id >= 9999 THEN CONVERT(VARCHAR,GETDATE(),112) + 'VCB' + CONVERT(CHAR, CONVERT(INT, @id) + 1)
		END
	RETURN @id
END

ALTER TABLE [dbo].[HoaDon] ADD  CONSTRAINT [IDHD]  DEFAULT ([dbo].[AUTO_IDHD]()) FOR [MaHoaDon]

ALTER TABLE [dbo].[VeChuyenBay] ADD  CONSTRAINT [IDVCB]  DEFAULT ([dbo].[AUTO_IDVCB]()) FOR [MaVe]
