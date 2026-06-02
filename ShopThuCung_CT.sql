CREATE DATABASE SHOPBANTHUCUNG
GO
USE SHOPBANTHUCUNG
GO

CREATE TABLE LoaiThuCung (
    MaLoai INT IDENTITY(1,1) PRIMARY KEY,
    TenLoai NVARCHAR(50) NOT NULL,
    MoTa NVARCHAR(255),
	Hinh VARCHAR(50)
);

CREATE TABLE ThuCung (
    MaThuCung INT IDENTITY(1,1) PRIMARY KEY,
    TenThuCung NVARCHAR(100) NOT NULL,
    MaLoai INT,
    GioiTinh NVARCHAR(10) CHECK (GioiTinh IN (N'Đực', N'Cái')) NOT NULL,
    NgaySinh DATE,
    MauSac NVARCHAR(50),
    Gia DECIMAL(12,2),
    MoTa NVARCHAR(MAX),
	GiaKhuyenMai DECIMAL(12,2),
    TrangThai NVARCHAR(50) CHECK (TrangThai IN (N'Còn hàng', N'Đã bán', N'Đang đặt trước')) DEFAULT N'Còn hàng',
	Hinh VARCHAR(50),
    CONSTRAINT FK_ThuCung_Loai FOREIGN KEY (MaLoai) REFERENCES LoaiThuCung(MaLoai)
);

CREATE TABLE KhachHang (
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    SDT CHAR(10),
	MatKhau CHAR(20)NOT NULL,
    Email NVARCHAR(100),
	DiaChi NVARCHAR(255)
);

CREATE TABLE NhanVien (
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    ChucVu NVARCHAR(50),
    SDT CHAR(10),
    Email NVARCHAR(100)

);
CREATE TABLE TinhTrang (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    TinhTrangHoaDon NVARCHAR(50)
);
CREATE TABLE HoaDon (
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    NgayLap DATE DEFAULT GETDATE(),
    MaKH INT,
    MaNV INT,
    TongTien DECIMAL(12,2),
	TinhTrang INT FOREIGN KEY REFERENCES TinhTrang(ID),
    DiaChiGiaoHang NVARCHAR(255),
    DaThanhToan BIT
    CONSTRAINT FK_HoaDon_KhachHang FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    CONSTRAINT FK_HoaDon_NhanVien FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
------------------=============

CREATE TABLE LoaiDichVu (
    MaLoai INT IDENTITY(1,1) PRIMARY KEY,
    TenLoai NVARCHAR(50) NOT NULL,
    MoTa NVARCHAR(255),
	Hinh VARCHAR(50)
);
CREATE TABLE DichVu (
    MaDichVu INT IDENTITY(1,1) PRIMARY KEY,
    TenDichVu NVARCHAR(100) NOT NULL,
    MaLoai INT,
    Gia DECIMAL(12,2),
	GiaKhuyenMai DECIMAL(12,2),
    MoTa NVARCHAR(MAX),
    TrangThai NVARCHAR(50) CHECK (TrangThai IN (N'Còn hàng', N'Đã bán', N'Đang đặt trước')) DEFAULT N'Còn hàng',
	Hinh VARCHAR(50),
    CONSTRAINT FK_DichVu_Loai FOREIGN KEY (MaLoai) REFERENCES LoaiDichVu(MaLoai)
);

CREATE TABLE ChamSocSucKhoe (
    MaLoai INT IDENTITY(1,1) PRIMARY KEY,
    TenLoai NVARCHAR(50) NOT NULL,
    MoTa NVARCHAR(255),
	Hinh VARCHAR(50)
);

CREATE TABLE DichVuChamSocSucKhoe (
    MaDichVu INT IDENTITY(1,1) PRIMARY KEY,
    TenDichVu NVARCHAR(100) NOT NULL,
    MaLoai INT,
    Gia DECIMAL(12,2),
    MoTa NVARCHAR(MAX),
    TrangThai NVARCHAR(50) CHECK (TrangThai IN (N'Còn hàng', N'Đã bán', N'Đang đặt trước')) DEFAULT N'Còn hàng',
	Hinh VARCHAR(50),
    CONSTRAINT FK_DichVuChamSocSucKhoe_Loai FOREIGN KEY (MaLoai) REFERENCES ChamSocSucKhoe(MaLoai)
);

CREATE TABLE LoaiPhuKien (
    MaLoai INT IDENTITY(1,1) PRIMARY KEY,
    TenLoai NVARCHAR(50) NOT NULL,
    MoTa NVARCHAR(255),
	Hinh VARCHAR(50)
);

CREATE TABLE PhuKien(
    MaPhuKien INT IDENTITY(1,1) PRIMARY KEY,
    TenPhuKien NVARCHAR(100) NOT NULL,
    MaLoai INT,
    Gia DECIMAL(12,2),
	GiaKhuyenMai DECIMAL(12,2),
    MoTa NVARCHAR(MAX),
    TrangThai NVARCHAR(50) CHECK (TrangThai IN (N'Còn hàng', N'Đã bán', N'Đang đặt trước')) DEFAULT N'Còn hàng',
	Hinh VARCHAR(50),
    CONSTRAINT FK_PhuKien_Loai FOREIGN KEY (MaLoai) REFERENCES LoaiPhuKien(MaLoai)
);
go

CREATE TABLE QuanLy(
	MaQL INT IDENTITY(1,1) PRIMARY KEY,
	TenQL NVARCHAR(100),
	Email NVARCHAR(100) NOT NULL,
	MatKhau VARCHAR(50)NOT NULL,
)
--------------------------=============================
CREATE TABLE ChiTietHoaDon (
    MaHD INT,
    MaThuCung INT,
	MaPhuKien INT,
    SoLuong INT DEFAULT 1,
    DonGia DECIMAL(12,2),
    PRIMARY KEY (MaHD, MaThuCung),
    CONSTRAINT FK_CTHD_HoaDon FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
	  CONSTRAINT FK_CTHD_PhuKien FOREIGN KEY (MaPhuKien) REFERENCES PhuKien(MaPhuKien),
    CONSTRAINT FK_CTHD_ThuCung FOREIGN KEY (MaThuCung) REFERENCES ThuCung(MaThuCung)
);

CREATE TABLE BANNER(
MaBANNER INT IDENTITY(1,1) PRIMARY KEY,
HinhBANNER VARCHAR(50)
)
go

--------------------------------------------------------------------------------
USE SHOPBANTHUCUNG
GO
CREATE TABLE HinhAnhPhu (
    MaAnh INT PRIMARY KEY IDENTITY(1,1),
    MaThuCung INT, -- Khóa ngoại liên kết bảng ThuCung
    MaPhuKien INT, -- Khóa ngoại liên kết bảng PhuKien 
    HinhAnh NVARCHAR(250),
    CONSTRAINT FK_HinhAnhPhu_ThuCung FOREIGN KEY (MaThuCung) REFERENCES ThuCung(MaThuCung),
	CONSTRAINT FK_HinhAnhPhu_PhuKien FOREIGN KEY (MaPhuKien) REFERENCES PhuKien(MaPhuKien)
);
CREATE TABLE BinhLuan (
    MaBL INT PRIMARY KEY IDENTITY(1,1),
    MaKH INT, -- Khóa ngoại liên kết KhachHang
    MaThuCung INT, -- Khóa ngoại liên kết ThuCung
	MaPhuKien INT,
    NoiDung NVARCHAR(MAX),
    NgayDang DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
	FOREIGN KEY (MaPhuKien) REFERENCES PhuKien(MaPhuKien),
    FOREIGN KEY (MaThuCung) REFERENCES ThuCung(MaThuCung)
);

--------------------------------------------------------------------------------
-- 1. CHÈN DỮ LIỆU VÀO CÁC BẢNG DANH MỤC (Không cần SET IDENTITY_INSERT)
--------------------------------------------------------------------------------

--==============================================================

use SHOPBANTHUCUNG
go
CREATE TABLE BANNERCHINH (
	MaHinh INT IDENTITY(1,1) PRIMARY KEY,
	BANNERChinh varchar(100)
)
INSERT INTO BANNERCHINH (BANNERChinh) VALUES 
('bannerchinh(1).jpg'),
('bannerchinh(2).jpg'),
('bannerchinh(3).jpg');

use SHOPBANTHUCUNG
go
-- 1. Bảng Khuyến Mãi
CREATE TABLE KhuyenMai (
    MaKM INT IDENTITY(1,1) PRIMARY KEY,
    TenKhuyenMai NVARCHAR(100) NOT NULL,
    CodeKM VARCHAR(20) UNIQUE, -- Mã giảm giá (ví dụ: GIAM20)
    PhanTramGiam INT CHECK (PhanTramGiam >= 0 AND PhanTramGiam <= 100),
    NgayBatDau DATE,
    NgayKetThuc DATE,
    MoTa NVARCHAR(MAX),
    HinhKM VARCHAR(50)
);

-- 2. Bảng Tin Tức
CREATE TABLE TinTuc (
    MaTin INT IDENTITY(1,1) PRIMARY KEY,
    TieuDe NVARCHAR(255) NOT NULL,
    NoiDungNgan NVARCHAR(500),
    NoiDungChiTiet NVARCHAR(MAX),
    NgayDang DATETIME DEFAULT GETDATE(),
    TacGia NVARCHAR(100),
    HinhTinTuc VARCHAR(50)
);

-- 3. Bảng Đặt Lịch 
CREATE TABLE  DatLich (
    MaDatLich INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT,
    HoTenKhach NVARCHAR(100), -- Để dự phòng khách chưa đăng ký
    SDTKhach CHAR(10),
    NgayHen DATETIME,
    GhiChu NVARCHAR(MAX),
    TinhTrang NVARCHAR(50) DEFAULT N'Chờ xác nhận', -- Chờ xác nhận, Xác nhận, Đã xong, Đã hủy
    CONSTRAINT FK_DatLich_KhachHang FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
);
GO


--==============================================================
-- TinhTrang (Trạng thái hóa đơn)
INSERT INTO TinhTrang (TinhTrangHoaDon) VALUES
(N'Chờ xác nhận'),
(N'Đã xác nhận'),
(N'Đang giao hàng'),
(N'Đã giao hàng'),
(N'Đã hủy');

-- QuanLy (Quản lý)
INSERT INTO QuanLy(TenQL,Email,MatKhau) VALUES
(N'Quản lý 1','Admin1@gmail.com','123'),
(N'Quản lý 2','Admin2@gmail.com','123456'),
(N'Quản lý 3','Admin3@gmail.com','Admin123');

-- LoaiThuCung (Loại thú cưng)
INSERT INTO LoaiThuCung (TenLoai, MoTa, Hinh) VALUES
(N'Chó', N'Từ Poodle, Corgi đến Alaska – mỗi chú chó đều được chăm sóc tận tình, đảm bảo sức khỏe và tính cách thân thiện. Chọn ngay người bạn trung thành luôn sẵn sàng mang lại niềm vui cho bạn.','cho1.jpg'),
(N'Mèo', N'Những bé mèo như Anh Lông Ngắn, Ba Tư hay Munchkin đáng yêu luôn sẵn sàng tìm mái ấm mới. Mỗi bé đều được chăm sóc, tắm rửa sạch sẽ và huấn luyện cơ bản.','pettt.jpg'),
(N'Thú nhỏ khác', N'Khám phá các loài thú nhỏ như thỏ mini, hamster. Phù hợp với không gian nhỏ gọn, dễ chăm sóc và vô cùng thân thiện.','tcc.jpg');

-- ThuCung (Thú cưng)
INSERT INTO ThuCung (TenThuCung, MaLoai, GioiTinh, NgaySinh, MauSac, Gia, MoTa, TrangThai,Hinh)
VALUES
(N'Shiba Inu', 1, N'Đực', '2024-01-05', N'Nâu trắng', 4500000, N'Chó Nhật Bản, thân thiện và thông minh', N'Còn hàng','Shiba.jpg'),
(N'Husky Siberian', 1, N'Cái', '2023-12-20', N'Xám trắng', 5500000, N'Chó tuyết, lông dày và thân thiện', N'Đang đặt trước','Husky.jpg'),
(N'Mèo Ba Tư', 2, N'Cái', '2024-03-10', N'Trắng', 3000000, N'Mèo Ba Tư lông dài, hiền lành', N'Còn hàng','meo-ba-tu.jpg'),
(N'Mèo Munchkin', 2, N'Đực', '2024-05-15', N'Vàng', 4200000, N'Mèo chân ngắn đáng yêu', N'Còn hàng','meo1.jpg'),
(N'Thỏ Holland Lop', 3, N'Cái', '2024-05-10', N'Trắng nâu', 700000, N'Thỏ tai cụp, hiền lành', N'Còn hàng','Tho-Holland-Lop.jpg'),
(N'Hamster Bear', 3, N'Cái', '2024-07-01', N'Xám trắng', 100000, N'Hamster thân thiện, dễ nuôi', N'Còn hàng','Hamster-Bear.jpg'),
(N'Mèo Anh lông dài', 2, N'Đực', '2024-06-12', N'Xám bạc', 3500000, N'Mèo Anh lông dài có bộ lông dày, tính tình hiền lành, thân thiện với trẻ nhỏ.', N'Còn hàng','meo2.jpg'),
(N'Mèo Scottish tai cụp', 2, N'Cái', '2024-07-05', N'Trắng đen', 3800000, N'Mèo tai cụp dễ thương, tính tình nhẹ nhàng, thân thiện.', N'Còn hàng','meo4.jpg'),
(N'Chó Ngao Anh', 1, N'Đực', '2023-11-10', N'Nâu đậm', 9500000, N'Giống chó to lớn, trung thành và rất dũng mãnh.', N'Còn hàng','cho3.jpg'),
(N'Chó Alaska Malamute', 1, N'Cái', '2024-02-18', N'Nâu trắng', 7500000, N'Chó kéo xe tuyết mạnh mẽ, hiền lành và thân thiện.', N'Còn hàng','cho2.jpg'),
(N'Chó Dogo Argentino', 1, N'Đực', '2023-10-25', N'Trắng tuyết', 8500000, N'Giống chó săn dũng cảm, mạnh mẽ, rất trung thành với chủ.', N'Còn hàng','cho4.jpg'),
(N'Hamster Winter White', 3, N'Cái', '2024-08-03', N'Trắng xám', 120000, N'Hamster Winter White thân thiện, dễ nuôi và rất hiếu động.', N'Còn hàng','hamster_1.jpg'),
(N'Hamster Campell', 3, N'Đực', '2024-09-12', N'Nâu sáng', 110000, N'Hamster Campell nhỏ nhắn, hoạt bát, dễ chăm sóc.', N'Còn hàng','hamster_2.jpg'),
(N'Chó Poodle trắng', 1, N'Cái', '2024-02-10', N'Trắng', 4800000, N'Chó Poodle thông minh, dễ huấn luyện', N'Còn hàng', '1.jpg'),
(N'Chó Poodle nâu', 1, N'Đực', '2024-01-18', N'Nâu', 5000000, N'Poodle lông xoăn, thân thiện', N'Còn hàng', '2.jpg'),
(N'Chó Corgi chân ngắn', 1, N'Cái', '2023-12-05', N'Vàng trắng', 6500000, N'Corgi chân ngắn đáng yêu', N'Còn hàng', '3.jpg'),
(N'Chó Corgi tam thể', 1, N'Đực', '2024-03-22', N'Đen vàng trắng', 6800000, N'Corgi hoạt bát, thông minh', N'Còn hàng', '4.jpg'),
(N'Chó Pug', 1, N'Đực', '2024-01-30', N'Vàng nhạt', 4200000, N'Chó Pug mặt xệ dễ thương', N'Còn hàng', '5.jpg'),
(N'Chó Pug đen', 1, N'Cái', '2024-02-12', N'Đen', 4500000, N'Pug đen hiếm, dễ nuôi', N'Còn hàng', '6.jpg'),
(N'Chó Chihuahua', 1, N'Đực', '2024-04-01', N'Nâu vàng', 3800000, N'Chihuahua nhỏ gọn, lanh lợi', N'Còn hàng', '7.jpg'),
(N'Chó Chihuahua lông dài', 1, N'Cái', '2024-04-15', N'Trắng', 4200000, N'Chihuahua lông dài quý hiếm', N'Còn hàng', '8.jpg'),
(N'Chó Golden Retriever', 1, N'Đực', '2023-11-20', N'Vàng', 8500000, N'Golden thân thiện, trung thành', N'Còn hàng', '9.jpg'),
(N'Chó Golden cái', 1, N'Cái', '2023-12-02', N'Vàng nhạt', 8200000, N'Golden hiền lành, thông minh', N'Còn hàng', '10.jpg'),
(N'Chó Samoyed', 1, N'Đực', '2024-02-08', N'Trắng tuyết', 9000000, N'Samoyed lông trắng mượt', N'Còn hàng', '11.jpg'),
(N'Chó Samoyed cái', 1, N'Cái', '2024-02-18', N'Trắng', 9200000, N'Samoyed thân thiện', N'Còn hàng', '12.jpg'),
(N'Chó Beagle', 1, N'Đực', '2024-03-10', N'Tam thể', 4700000, N'Beagle săn mồi, hiếu động', N'Còn hàng', '13.jpg'),
(N'Chó Beagle cái', 1, N'Cái', '2024-03-20', N'Tam thể', 4800000, N'Beagle dễ nuôi', N'Còn hàng', '14.jpg'),
(N'Chó Bulldog Anh', 1, N'Đực', '2023-10-15', N'Nâu trắng', 7800000, N'Bulldog mập mạp, hiền', N'Còn hàng', '15.jpg'),
(N'Chó Bulldog Pháp', 1, N'Cái', '2024-01-25', N'Trắng đen', 7500000, N'Bulldog Pháp tai dơi', N'Còn hàng', '16.jpg'),
(N'Chó Doberman', 1, N'Đực', '2023-09-12', N'Đen nâu', 8800000, N'Doberman mạnh mẽ, trung thành', N'Còn hàng', '17.jpg'),
(N'Chó Rottweiler', 1, N'Đực', '2023-08-05', N'Đen nâu', 9500000, N'Rottweiler bảo vệ tốt', N'Còn hàng', '18.jpg'),
(N'Chó Border Collie', 1, N'Cái', '2024-02-22', N'Đen trắng', 8200000, N'Border Collie cực kỳ thông minh', N'Còn hàng', '19.jpg'),
(N'Chó Dachshund', 1, N'Cái', '2024-03-30', N'Nâu', 4300000, N'Chó lạp xưởng vui vẻ', N'Còn hàng', '20.jpg'),
(N'Mèo Anh lông ngắn', 2, N'Đực', '2024-04-05', N'Xám xanh', 3200000, N'Mèo Anh lông ngắn dễ thương', N'Còn hàng', '21.jpg'),
(N'Mèo Anh lông ngắn cái', 2, N'Cái', '2024-04-15', N'Trắng', 3300000, N'Mèo hiền lành', N'Còn hàng', '22.jpg'),
(N'Mèo Bengal', 2, N'Đực', '2024-03-01', N'Vàng đốm', 6500000, N'Mèo Bengal hoang dã', N'Còn hàng', '23.jpg'),
(N'Mèo Bengal cái', 2, N'Cái', '2024-03-12', N'Nâu đốm', 6700000, N'Mèo Bengal lanh lợi', N'Còn hàng', '24.jpg'),
(N'Mèo Ragdoll', 2, N'Cái', '2024-02-20', N'Trắng xanh', 7000000, N'Mèo Ragdoll hiền', N'Còn hàng', '25.jpg'),
(N'Mèo Ragdoll đực', 2, N'Đực', '2024-02-25', N'Trắng xám', 7200000, N'Mèo bông xù', N'Còn hàng', '26.jpg'),
(N'Mèo Xiêm', 2, N'Cái', '2024-01-15', N'Kem nâu', 3000000, N'Mèo Xiêm thông minh', N'Còn hàng', '27.jpg'),
(N'Mèo Xiêm đực', 2, N'Đực', '2024-01-22', N'Kem', 3100000, N'Mèo Xiêm lanh lợi', N'Còn hàng', '28.jpg'),
(N'Mèo Ba Tư xám', 2, N'Cái', '2024-02-10', N'Xám', 3500000, N'Mèo Ba Tư lông dài', N'Còn hàng', '29.jpg'),
(N'Mèo Ba Tư vàng', 2, N'Đực', '2024-02-18', N'Vàng', 3600000, N'Mèo mặt tịt đáng yêu', N'Còn hàng', '30.jpg'),
(N'Mèo Sphynx', 2, N'Đực', '2024-03-05', N'Hồng', 6800000, N'Mèo không lông độc đáo', N'Còn hàng', '31.jpg'),
(N'Mèo Scottish tai thẳng', 2, N'Cái', '2024-04-01', N'Trắng xám', 3900000, N'Mèo Scottish tai thẳng', N'Còn hàng', '32.jpg'),
(N'Mèo Munchkin chân ngắn', 2, N'Đực', '2024-03-18', N'Vàng', 4200000, N'Mèo chân ngắn vui nhộn', N'Còn hàng', '33.jpg'),
(N'Mèo Mỹ lông ngắn', 2, N'Cái', '2024-02-28', N'Nâu sọc', 2800000, N'Mèo Mỹ dễ nuôi', N'Còn hàng', '34.jpg'),
(N'Mèo Chartreux', 2, N'Đực', '2024-03-14', N'Xám xanh', 5200000, N'Mèo Chartreux quý hiếm', N'Còn hàng', '35.jpg'),
(N'Mèo Turkish Van', 2, N'Cái', '2024-04-10', N'Trắng đỏ', 6000000, N'Mèo Turkish Van thích nước', N'Còn hàng', '36.jpg'),
(N'Mèo Maine Coon', 2, N'Đực', '2024-01-05', N'Nâu đen', 8500000, N'Mèo to lớn, hiền', N'Còn hàng', '37.jpg'),
(N'Mèo Abyssinian', 2, N'Cái', '2024-02-06', N'Nâu đỏ', 5500000, N'Mèo Aby nhanh nhẹn', N'Còn hàng', '38.jpg'),
(N'Mèo Exotic', 2, N'Đực', '2024-03-25', N'Trắng', 4300000, N'Mèo Ba Tư lông ngắn', N'Còn hàng', '39.jpg'),
(N'Mèo Havana Brown', 2, N'Cái', '2024-04-18', N'Nâu socola', 6200000, N'Mèo Havana hiếm', N'Còn hàng', '40.jpg'),
(N'Thỏ Mini Rex', 3, N'Cái', '2024-06-01', N'Xám', 600000, N'Thỏ lông mịn', N'Còn hàng', '41.jpg'),
(N'Thỏ Lionhead', 3, N'Đực', '2024-06-10', N'Trắng', 650000, N'Thỏ đầu sư tử', N'Còn hàng', '42.jpg'),
(N'Hamster Robo', 3, N'Đực', '2024-07-01', N'Nâu trắng', 90000, N'Hamster nhỏ nhanh nhẹn', N'Còn hàng', '43.jpg'),
(N'Hamster Syrian', 3, N'Cái', '2024-07-05', N'Vàng', 120000, N'Hamster to, dễ nuôi', N'Còn hàng', '44.jpg'),
(N'Sóc bay Úc', 3, N'Cái', '2024-05-20', N'Xám', 1500000, N'Sóc bay thân thiện', N'Còn hàng', '45.jpg'),
(N'Chuột Lang', 3, N'Đực', '2024-05-15', N'Nâu trắng', 350000, N'Chuột lang hiền', N'Còn hàng', '46.jpg'),
(N'Nhím kiểng', 3, N'Cái', '2024-06-20', N'Nâu', 1200000, N'Nhím kiểng độc đáo', N'Còn hàng', '47.jpg'),
(N'Rùa tai đỏ', 3, N'Đực', '2024-04-01', N'Xanh', 400000, N'Rùa cảnh dễ nuôi', N'Còn hàng', '48.jpg'),
(N'Cóc kiểng', 3, N'Cái', '2024-03-10', N'Xanh nâu', 300000, N'Cóc cảnh', N'Còn hàng', '49.jpg'),
(N'Cá cảnh Betta', 3, N'Đực', '2024-02-01', N'Đỏ xanh', 150000, N'Cá Betta đẹp', N'Còn hàng', '50.jpg');


-- KhachHang (Khách hàng)
INSERT INTO KhachHang (HoTen, SDT,MatKhau, Email, DiaChi) VALUES
(N'Nguyễn Văn A', '0909123456','1234', 'vana@gmail.com','123 duong a'),
(N'Trần Thị B', '0912233445','3451', 'thib@gmail.com','123 duong a'),
(N'Lê Thị Mai', '0933224455','mai123', 'maile@gmail.com','123 duong a'),
(N'Phan Quốc Huy', '0908887766','abc', 'huyphan@gmail.com','123 duong a'),
(N'Trương Ngọc Lan', '0988776655','dcjhg', 'lantruong@gmail.com','123 duong a'),
(N'Đỗ Thành Công', '0977665544','khgu', 'congdothanh@gmail.com','123 duong a'),
(N'Vũ Hồng Nhung', '0966888777','mgbn', 'nhungvu@gmail.com','123 duong a');

-- NhanVien (Nhân viên)
INSERT INTO NhanVien (HoTen, ChucVu, SDT, Email) VALUES
(N'Lê Minh', N'Thu ngân', '0988123123', 'leminh@petshop.vn'),
(N'Phạm Hương', N'Bán hàng', '0977334455', 'phamhuong@petshop.vn'),
(N'Nguyễn Thị Hồng', N'Quản lý', '0988000111', 'hong.nguyen@petshop.vn'),
(N'Lâm Văn Dũng', N'Bán hàng', '0977000999', 'dung.lam@petshop.vn'),
(N'Trần Hữu Tài', N'Chăm sóc thú cưng', '0911222333', 'tai.tran@petshop.vn'),
(N'Phan Thị Mỹ', N'Thu ngân', '0909888777', 'my.phan@petshop.vn');

-- BANNER
INSERT INTO BANNER (HinhBANNER) VALUES
('banner(1).jpg'),
('banner(2).jpg'),
('banner(3).jpg'),
('banner(4).jpg'),
('banner(5).jpg');

-- LoaiDichVu (Loại dịch vụ)
INSERT INTO LoaiDichVu (TenLoai, MoTa,Hinh) VALUES
(N'Spa', N'Dịch vụ tắm, cắt tỉa lông và massage giúp thú cưng thư giãn, sạch sẽ và luôn rạng rỡ. Hãy để bé yêu được chăm sóc như một ngôi sao thực thụ!','spa.jpg'),
(N'Pet hotel', N'Khi bạn đi công tác hoặc du lịch, hãy để PetShop chăm sóc thú cưng của bạn với dịch vụ khách sạn tiện nghi, an toàn và đầy ắp tình yêu thương.','hotel.jpg'),
(N'Chăm sóc sức khoẻ', N'Khám định kỳ, tiêm phòng, tư vấn dinh dưỡng và điều trị bệnh lý – giúp thú cưng luôn khỏe mạnh và tràn đầy năng lượng.','sk.jpg');

-- DichVu (Dịch vụ)
INSERT INTO DichVu (TenDichVu, MaLoai, Gia, MoTa, TrangThai, Hinh)
VALUES
(N'Tắm gội cơ bản cho chó nhỏ', 1, 150000, N'Tắm rửa bằng sữa tắm dịu nhẹ, sấy khô và chải lông mượt mà cho chó nhỏ dưới 10kg.', N'Còn hàng', 'tamcho.jpg'),
(N'Cắt tỉa lông chuyên nghiệp', 1, 300000, N'Cắt tỉa theo phong cách hiện đại, phù hợp từng giống thú cưng, giúp bé luôn gọn gàng, đáng yêu.', N'Còn hàng', 'catlong.jpg'),
(N'Lưu trú 1 đêm (Pet Hotel)', 2, 200000, N'Dịch vụ khách sạn thú cưng – nơi bé được chăm sóc, ăn uống và vui chơi an toàn khi bạn vắng nhà.', N'Còn hàng', 'hotel1.jpg'),
(N'Lưu trú VIP (Pet Hotel)', 2, 350000, N'Phòng riêng có camera, điều hòa và chăm sóc đặc biệt cho thú cưng yêu quý của bạn.', N'Còn hàng', 'hotelvip.jpg'),
(N'Tiêm phòng cơ bản', 3, 250000, N'Tiêm phòng các bệnh truyền nhiễm phổ biến giúp bảo vệ sức khỏe toàn diện cho thú cưng.', N'Còn hàng', 'tiemphong.jpg'),
(N'Tư vấn dinh dưỡng', 3, 100000, N'Tư vấn khẩu phần ăn, loại thức ăn phù hợp và cách chăm sóc dinh dưỡng cho từng loại thú cưng.', N'Còn hàng', 'dinhduong.jpg');

-- ChamSocSucKhoe (Loại dịch vụ Chăm sóc sức khỏe)
INSERT INTO ChamSocSucKhoe (TenLoai, MoTa,Hinh) VALUES
(N'Khám Sức Khỏe', N' Đưa thú cưng đi kiểm tra sức khỏe định kỳ giúp phát hiện sớm các vấn đề tiềm ẩn và đảm bảo cuộc sống khỏe mạnh, lâu dài cho người bạn nhỏ của bạn.','dk.jpg'),
(N'Tiêm Phòng ', N'Giúp thú cưng tránh xa bệnh tật bằng lịch tiêm phòng và kiểm tra định kỳ. Đừng quên tham khảo ý kiến bác sĩ thú y để có kế hoạch phù hợp nhất cho thú cưng của bạn.','tp.jpg'),
(N'Chăm Sóc Răng Miệng', N'Răng miệng khỏe mạnh giúp thú cưng ăn ngon, tiêu hóa tốt và giảm nguy cơ mắc bệnh. Học ngay các phương pháp vệ sinh răng miệng đúng cách để bảo vệ nụ cười đáng yêu của thú cưng!','rm.jpg');

-- DichVuChamSocSucKhoe (Dịch vụ Chăm sóc sức khỏe)
INSERT INTO DichVuChamSocSucKhoe (TenDichVu, MaLoai, Gia, MoTa, TrangThai, Hinh)
VALUES
(N'Khám tổng quát', 1, 200000, N'Kiểm tra sức khỏe toàn thân, đo cân nặng, nhiệt độ, và tư vấn chăm sóc định kỳ.', N'Còn hàng', 'kham.jpg'),
(N'Tiêm phòng dại', 2, 180000, N'Tiêm phòng dại giúp bảo vệ thú cưng và cả gia đình khỏi bệnh dại nguy hiểm.', N'Còn hàng', 'tiemphongdai.jpg'),
(N'Vệ sinh răng miệng', 3, 150000, N'Làm sạch mảng bám, giảm mùi hôi miệng và phòng ngừa bệnh răng lợi cho thú cưng.', N'Còn hàng', 'rangmieng.jpg');

-- LoaiPhuKien (Loại Phụ kiện)
INSERT INTO LoaiPhuKien (TenLoai, MoTa, Hinh) VALUES
(N'Thức Ăn Cao Cấp', N'Cung cấp đầy đủ dinh dưỡng, hỗ trợ tiêu hóa và giúp thú cưng phát triển khỏe mạnh.','ta.png'),
(N'Đồ Chơi Tương Tác', N'Giúp thú cưng vận động, giảm stress và tăng khả năng phản xạ thông qua các hoạt động vui chơi.','dc.jpg'),
(N'Trang Phục', N'Tăng sự đáng yêu cho thú cưng đồng thời bảo vệ cơ thể trước thời tiết nắng, mưa, gió hoặc lạnh.','tphuc.jpg'),
(N'Giường Ngủ Êm Ái', N'Mang lại cảm giác thoải mái, giữ ấm và giúp thú cưng có giấc ngủ sâu và an toàn hơn.','bed.jpg');

-- PhuKien (Phụ kiện)
INSERT INTO PhuKien (TenPhuKien, MaLoai, Gia, MoTa, TrangThai, Hinh)
VALUES
(N'Thức ăn hạt cho mèo vị cá ngừ', 1, 220000, N'Giàu protein, dễ tiêu hóa, giúp lông mèo mềm mượt và khỏe mạnh.', N'Còn hàng', 'ta2.jpg'),
(N'Thức ăn hạt Pedigree cho chó', 1, 80000, N'Thành phần gồm rau và thịt, hổ trợ tiêu hóa cung cấp các chất dinh dưỡng cần thiết cho thú cưng của bạn. ', N'Còn hàng', 'ta3.jpg'),
(N'Thức ăn cỏ Timothy cho thỏ', 1, 50000, N'Thành phần gồm cỏ,cung cấp các chất dinh dưỡng cần thiết cho thú cưng của bạn. ', N'Còn hàng', 'ta6.jpg'),
(N'Đồ chơi bóng cao su phát sáng', 2, 90000, N'Đồ chơi phát sáng giúp thú cưng vận động, vui chơi cả ban đêm.', N'Còn hàng', 'bong.jpg'),
(N'Đồ chơi gối gặm cho chó', 2, 100000, N'Sản phẩm giúp chó cưng giải tỏa năng lượng, giảm stress và làm sạch răng miệng. Chất liệu vải bền, an toàn cho thú cưng, dễ giặt và tái sử dụng.', N'Còn hàng', 'chogam.jpg'),
(N'Áo hoodie khủng long cho chó mèo', 3, 150000, N'Chất liệu cotton mềm mại, giữ ấm cho thú cưng trong mùa lạnh.', N'Còn hàng', 'tp2.jpg'),
(N'Áo chó mèo', 3, 170000, N'Chất liệu cotton mềm mại, giữ ấm cho thú cưng trong mùa lạnh.', N'Còn hàng', 'sk2.jpg'),
(N'Áo sinh nhật cho chó mèo', 3, 270000, N'Chất liệu cotton mềm mại, giữ ấm cho thú cưng trong mùa lạnh.', N'Còn hàng', 'tp3.jpg'),
(N'trang phuc cho hamster', 3, 120000, N'Chất liệu cotton mềm mại, giữ ấm cho thú cưng trong mùa lạnh.', N'Còn hàng', 'cn.jpg'),
(N'trang phuc cool ngầu cho hamster', 3, 270000, N'Chất liệu cotton mềm mại, giữ ấm cho thú cưng trong mùa lạnh.', N'Còn hàng', 'tp6.jpg'),
(N'cài cho hamster', 3, 20000, N'Thiết kế dễ thương, phù hợp với mọi loại hamster', N'Còn hàng', 'tp5.jpg'),
(N'dây dắt cho thỏ', 3, 30000, N'Thiết kế dễ thương, đáng yêu', N'Còn hàng', 'tp4.jpg'),
(N'Giường ngủ hình chử nhật', 4, 320000, N'giúp thú cưng nghỉ ngơi thoải mái và dễ chịu.', N'Còn hàng', 'g.jpg'),
(N'Giường ngủ lông mềm hình tròn', 4, 320000, N'Thiết kế êm ái, giúp thú cưng nghỉ ngơi thoải mái và dễ chịu.', N'Còn hàng', 'dogbed.jpg'),
(N'Thức ăn hạt Royal Canin cho mèo', 1, 280000, N'Dinh dưỡng cân bằng, tốt cho hệ tiêu hóa mèo.', N'Còn hàng', '51.jpg'),
(N'Thức ăn hạt Royal Canin cho chó', 1, 300000, N'Hỗ trợ xương khớp và lông cho chó.', N'Còn hàng', '52.jpg'),
(N'Pate cá hồi cho mèo', 1, 45000, N'Pate mềm giàu omega 3, giúp lông mượt.', N'Còn hàng', '53.jpg'),
(N'Xương gặm canxi cho chó', 1, 90000, N'Giúp làm sạch răng và bổ sung canxi.', N'Còn hàng', '54.jpg'),
(N'Sữa bột cho thú cưng sơ sinh', 1, 120000, N'Sữa chuyên dụng cho chó mèo nhỏ.', N'Còn hàng', '55.jpg'),
(N'Đồ chơi gặm cao su cho chó', 2, 85000, N'Đồ chơi giúp chó giải tỏa stress.', N'Còn hàng', '56.jpg'),
(N'Đồ chơi chuột giả cho mèo', 2, 60000, N'Kích thích bản năng săn mồi của mèo.', N'Còn hàng', '57.jpg'),
(N'Đồ chơi laser cho mèo', 2, 75000, N'Laser vui nhộn giúp mèo vận động.', N'Còn hàng', '58.jpg'),
(N'Bóng lăn phát nhạc cho thú cưng', 2, 110000, N'Bóng tự phát nhạc khi lăn.', N'Còn hàng', '59.jpg'),
(N'Đồ chơi cầu tuột hamster', 2, 65000, N'Giúp hamster vận động mỗi ngày.', N'Còn hàng', '60.jpg'),
(N'Áo mưa cho chó mèo', 3, 120000, N'Chống nước, dễ mặc, bảo vệ thú cưng khi đi mưa.', N'Còn hàng', '61.jpg'),
(N'Áo len mùa đông cho chó', 3, 150000, N'Giữ ấm cơ thể thú cưng.', N'Còn hàng', '62.jpg'),
(N'Váy công chúa cho mèo', 3, 180000, N'Trang phục dễ thương cho mèo cái.', N'Còn hàng', '63.jpg'),
(N'Áo hoodie thể thao cho thú cưng', 3, 200000, N'Phong cách trẻ trung, năng động.', N'Còn hàng', '64.jpg'),
(N'Khăn quàng cổ thú cưng', 3, 50000, N'Phụ kiện thời trang nhẹ nhàng.', N'Còn hàng', '65.jpg'),
(N'Giường ngủ thú cưng hình xương', 4, 350000, N'Giường mềm mại, thoáng khí.', N'Còn hàng', '66.jpg'),
(N'Giường ngủ thú cưng cao cấp', 4, 480000, N'Chất liệu cao cấp, êm ái.', N'Còn hàng', '67.jpg'),
(N'Nhà ngủ cho mèo bằng nỉ', 4, 420000, N'Giữ ấm cho mèo trong mùa lạnh.', N'Còn hàng', '68.jpg'),
(N'Ổ nằm thú cưng chống thấm', 4, 380000, N'Chống nước, dễ vệ sinh.', N'Còn hàng', '69.jpg'),
(N'Giường ngủ thú cưng mini', 4, 280000, N'Phù hợp cho thú cưng nhỏ.', N'Còn hàng', '70.jpg');
GO

--------------------------------------------------------------------------------
-- 2. CHÈN DỮ LIỆU VÀO BẢNG HOADON VÀ CHITIETHOADON (Cần SET IDENTITY_INSERT)
--------------------------------------------------------------------------------

-- Bật chế độ chèn thủ công cho cột tự tăng (MaHD)
SET IDENTITY_INSERT HoaDon ON;

-- Chèn 10 Hóa đơn (đã bao gồm các cập nhật về trạng thái, địa chỉ, thanh toán)
INSERT INTO HoaDon (MaHD, NgayLap, MaKH, MaNV, TongTien, TinhTrang, DiaChiGiaoHang, DaThanhToan) VALUES
(1, '2024-09-01', 3, 2, 4500000, 4, N'123 Đường A, Quận 1, TP.HCM', 1),
(2, '2024-09-03', 4, 1, 700000, 3, N'456 Đường B, Quận 3, TP.HCM', 0),
(3, '2024-09-05', 5, 4, 3000000, 2, N'789 Đường C, Quận Gò Vấp, TP.HCM', 1), -- Đã điều chỉnh Tổng tiền HĐ 3 thành 3,000,000
(4, '2024-09-08', 2, 3, 700000, 5, N'101 Đường D, Quận Tân Bình, TP.HCM', 0), -- Đã điều chỉnh Tổng tiền HĐ 4 thành 700,000
(5, '2024-09-12', 1, 1, 4200000, 1, N'202 Đường E, Quận Bình Thạnh, TP.HCM', 0), -- Đã điều chỉnh Tổng tiền HĐ 5 thành 4,200,000
(6, '2024-09-15', 6, 4, 3500000, 4, N'303 Đường F, Quận 7, TP.HCM', 1),
(7, '2024-09-17', 7, 2, 4200000, 1, N'404 Đường G, Quận 10, TP.HCM', 0),
(8, '2024-09-19', 1, 1, 9500000, 3, N'505 Đường H, Quận 4, TP.HCM', 0),
(9, '2024-09-22', 3, 3, 5000000, 5, N'606 Đường I, Quận 5, TP.HCM', 0),
(10, '2024-09-25', 5, 2, 7500000, 2, N'707 Đường K, Quận Thủ Đức, TP.HCM', 1);

-- Tắt chế độ chèn thủ công cho cột tự tăng (MaHD)
SET IDENTITY_INSERT HoaDon OFF;

-- Chèn Chi tiết hóa đơn (10 Hóa đơn)
INSERT INTO ChiTietHoaDon (MaHD, MaThuCung, SoLuong, DonGia) VALUES
(1, 1, 1, 4500000),  -- HĐ 1: Shiba Inu (4.5M)
(2, 5, 1, 700000),   -- HĐ 2: Thỏ Holland Lop (700K)
(3, 3, 1, 3000000),  -- HĐ 3: Mèo Ba Tư (3M)
(4, 5, 1, 700000),   -- HĐ 4: Thỏ Holland Lop (700K)
(5, 4, 1, 4200000),  -- HĐ 5: Mèo Munchkin (4.2M)
(6, 7, 1, 3500000),  -- HĐ 6: Mèo Anh lông dài (3.5M)
(7, 4, 1, 4200000),  -- HĐ 7: Mèo Munchkin (4.2M)
(8, 9, 1, 9500000),  -- HĐ 8: Chó Ngao Anh (9.5M)
(9, 1, 1, 4500000),  -- HĐ 9: Shiba Inu (4.5M)
(9, 6, 5, 100000),   -- HĐ 9: Hamster Bear (5*100K = 500K). Tổng = 5M
(10, 10, 1, 7500000); -- HĐ 10: Chó Alaska Malamute (7.5M)

UPDATE PhuKien SET GiaKhuyenMai = 27000 WHERE TenPhuKien LIKE N'%Pedigree%';
UPDATE PhuKien SET GiaKhuyenMai = 120000 WHERE TenPhuKien LIKE N'%Áo hoodie%';

-- 2. Cập nhật cho Thú Cưng
UPDATE ThuCung SET GiaKhuyenMai = 4000000 WHERE TenThuCung = N'Shiba Inu';
UPDATE ThuCung SET GiaKhuyenMai = 2500000 WHERE TenThuCung = N'Mèo Ba Tư';

-- 3. Cập nhật cho Dịch Vụ
UPDATE DichVu SET GiaKhuyenMai = 120000 WHERE TenDichVu = N'Tắm gội cơ bản cho chó nhỏ';
select * from DatLich;

-- Dữ liệu Khuyến Mãi
INSERT INTO KhuyenMai (TenKhuyenMai, CodeKM, PhanTramGiam, NgayBatDau, NgayKetThuc, MoTa, HinhKM) VALUES
(N'Chào hè rực rỡ', 'HE2024', 20, '2024-06-01', '2024-08-31', N'Giảm giá 20% cho tất cả phụ kiện.', 'km1.jpg'),
(N'Khai trương chi nhánh mới', 'WELCOME', 10, '2024-09-01', '2024-10-01', N'Ưu đãi cho đơn hàng thú cưng đầu tiên.', 'km2.jpg');


-- Dữ liệu Khuyến Mãi
INSERT INTO KhuyenMai (TenKhuyenMai, CodeKM, PhanTramGiam, NgayBatDau, NgayKetThuc, MoTa, HinhKM) VALUES
(N'Giảm giá cuối năm', 'YEAR2025', 20, '2024-06-01', '2026-08-31', N'Giảm giá 20% cho tất cả phụ kiện.', 'km1.jpg');

-- Dữ liệu Tin Tức
INSERT INTO TinTuc (TieuDe, NoiDungNgan, NoiDungChiTiet, TacGia, HinhTinTuc) VALUES
(N'Giảm giá cuối năm!! Săn ngay liền tay', N'Nhập mã "YEAR2025" để được giảm giá 20%', N'Giảm giá trên tất cả mặt hàng', N'Admin', 'km1.jpg'),
(N'Hot! Chó Shinba Innu giá gốc 4500 nay chỉ còn 4000', N'Nhanh tay nhanh tay rước người bạn thú cưng đáng yêu về nhà', N'Nội dung phân tích các loại hạt như Royal Canin, Pedigree...', N'Admin', 'Shiba.jpg');

-- Dữ liệu Đặt Lịch mẫu
INSERT INTO DatLich (MaKH, HoTenKhach, SDTKhach, NgayHen, GhiChu) VALUES
(1, N'Nguyễn Văn Anh Tú', '0909123456', '2024-12-25 09:00:00', N'Đặt lịch tắm cho chó Shiba'),
(NULL, N'Khách vãng lai', '0988111222', '2024-12-26 14:30:00', N'Tư vấn dinh dưỡng cho thỏ');
USE SHOPBANTHUCUNG
GO


-- 1.1 Thêm ảnh cho tất cả THÚ CƯNG
DECLARE @idTC INT
DECLARE curTC CURSOR FOR SELECT MaThuCung FROM ThuCung
OPEN curTC
FETCH NEXT FROM curTC INTO @idTC
WHILE @@FETCH_STATUS = 0
BEGIN
    INSERT INTO HinhAnhPhu (MaThuCung, MaPhuKien, HinhAnh) 
    VALUES (@idTC, NULL, 'haphu1.jpg'), (@idTC, NULL, 'haphu2.jpg'), (@idTC, NULL, 'haphu3.jpg');
    FETCH NEXT FROM curTC INTO @idTC
END
CLOSE curTC
DEALLOCATE curTC;

-- 1.2 Thêm ảnh cho tất cả PHỤ KIỆN
DECLARE @idPK INT
DECLARE curPK CURSOR FOR SELECT MaPhuKien FROM PhuKien
OPEN curPK
FETCH NEXT FROM curPK INTO @idPK
WHILE @@FETCH_STATUS = 0
BEGIN
    INSERT INTO HinhAnhPhu (MaThuCung, MaPhuKien, HinhAnh) 
    VALUES (NULL, @idPK, 'haphu1.jpg'), (NULL, @idPK, 'haphu2.jpg'), (NULL, @idPK, 'haphu3.jpg');
    FETCH NEXT FROM curPK INTO @idPK
END
CLOSE curPK
DEALLOCATE curPK;
GO


USE SHOPBANTHUCUNG
GO

-- 2.1 Bình luận cho THÚ CƯNG
DECLARE @idTC_BL INT
DECLARE curTC_BL CURSOR FOR SELECT MaThuCung FROM ThuCung
OPEN curTC_BL
FETCH NEXT FROM curTC_BL INTO @idTC_BL
WHILE @@FETCH_STATUS = 0
BEGIN
    INSERT INTO BinhLuan (MaKH, MaThuCung, NoiDung, NgayDang)
    VALUES 
    ((SELECT TOP 1 MaKH FROM KhachHang ORDER BY NEWID()), @idTC_BL, N'Sản phẩm tuyệt vời, bé rất khỏe mạnh!', GETDATE()),
    ((SELECT TOP 1 MaKH FROM KhachHang ORDER BY NEWID()), @idTC_BL, N'Shop giao hàng nhanh, tư vấn nhiệt tình.', GETDATE());
    FETCH NEXT FROM curTC_BL INTO @idTC_BL
END
CLOSE curTC_BL
DEALLOCATE curTC_BL;

-- 2.2 Bình luận cho PHỤ KIỆN (Lưu ý: Bạn cần thêm cột MaPhuKien vào bảng BinhLuan nếu muốn bình luận cho cả phụ kiện)
-- Nếu bảng BinhLuan chưa có cột MaPhuKien, hãy chạy lệnh này trước:
-- ALTER TABLE BinhLuan ADD MaPhuKien INT FOREIGN KEY REFERENCES PhuKien(MaPhuKien);

DECLARE @idPK_BL INT
DECLARE curPK_BL CURSOR FOR SELECT MaPhuKien FROM PhuKien
OPEN curPK_BL
FETCH NEXT FROM curPK_BL INTO @idPK_BL
WHILE @@FETCH_STATUS = 0
BEGIN
    INSERT INTO BinhLuan (MaKH, MaPhuKien, NoiDung, NgayDang) -- Nếu chưa sửa bảng thì tạm gán vào mẫu này
    VALUES 
    ((SELECT TOP 1 MaKH FROM KhachHang ORDER BY NEWID()), NULL, N'Phụ kiện chất lượng tốt, giá cả phải chăng.', GETDATE()),
    ((SELECT TOP 1 MaKH FROM KhachHang ORDER BY NEWID()), NULL, N'Đóng gói cẩn thận, hàng đẹp như hình.', GETDATE());
    FETCH NEXT FROM curPK_BL INTO @idPK_BL
END
CLOSE curPK_BL
DEALLOCATE curPK_BL;
GO


USE SHOPBANTHUCUNG
GO

INSERT INTO BinhLuan (MaKH, MaThuCung, NoiDung, NgayDang) VALUES 
(1, 1, N'Bé Shiba rất thông minh, về nhà là biết đi vệ sinh đúng chỗ ngay.', GETDATE()),
(2, 1, N'Shop tư vấn nhiệt tình, bé cún khỏe mạnh lắm.', GETDATE()),
(3, 3, N'Mèo Ba Tư lông rất dày và mượt, đúng như mô tả.', GETDATE()),
(4, 7, N'Bé mèo Anh lông dài rất quấn chủ, cảm ơn shop nhiều!', GETDATE()),
(5, 9, N'Chó Ngao Anh nhìn rất uy mãnh, ăn uống tốt.', GETDATE()),
(6, 4, N'Mèo Munchkin chân ngắn chạy nhảy nhìn cưng xỉu luôn.', GETDATE()),
(7, 5, N'Thỏ Holland Lop tai cụp xinh xắn, con gái mình rất thích.', GETDATE()),
(1, 13, N'Beagle rất tinh nghịch, chạy suốt cả ngày.', GETDATE()),
(2, 21, N'Mèo Anh lông ngắn mặt tròn xoe, rất đáng yêu.', GETDATE()),
(3, 33, N'Bé Munchkin màu vàng này hiếu động quá trời.', GETDATE());
GO
GO