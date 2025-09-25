CREATE VIEW ViewDanhSachSach AS
SELECT
    s.MaSach,
    s.TenSach,
    t.TenTacGia,
    tl.TenTheLoai,
    nxb.TenNXB,
    s.NamXuatBan,
    ISNULL(ks.SoLuongHienTai, 0) AS SoLuongHienTai,
    s.GiaSach,
    CASE 
        WHEN ISNULL(ks.TrangThaiSach, 'HetSach') = 'ConSach' THEN N'Còn Sách'
        WHEN ISNULL(ks.TrangThaiSach, 'HetSach') = 'HetSach' THEN N'Hết Sách'
        ELSE N'Hết Sách'
    END AS TrangThaiSach,
    s.AnhBia
FROM SACH s
JOIN TAC_GIA t ON s.IdTacGia = t.IdTG
JOIN THE_LOAI tl ON s.IdTheLoai = tl.IdTL
JOIN NHA_XUAT_BAN nxb ON s.IdNXB = nxb.IdNXB
LEFT JOIN Kho_Sach ks ON s.IdS = ks.MaSach;
GO

CREATE VIEW ViewDanhSachTacGia AS
SELECT
    t.MaTacGia,
    t.TenTacGia,
    ISNULL(SUM(ks.SoLuongHienTai), 0) AS SoLuongSach
FROM TAC_GIA t
LEFT JOIN SACH s ON t.IdTG = s.IdTacGia
LEFT JOIN Kho_Sach ks ON s.IdS = ks.MaSach
GROUP BY t.MaTacGia, t.TenTacGia;
GO

CREATE VIEW ViewDanhSachTheLoai AS
SELECT
    tl.MaTheLoai,
    tl.TenTheLoai,
    ISNULL(SUM(ks.SoLuongHienTai), 0) AS SoLuongSach
FROM THE_LOAI tl
LEFT JOIN SACH s ON tl.IdTL = s.IdTheLoai
LEFT JOIN Kho_Sach ks ON s.IdS = ks.MaSach
GROUP BY tl.MaTheLoai, tl.TenTheLoai;
GO

CREATE VIEW ViewDanhSachNhaXuatBan AS
SELECT
    nxb.MaNXB,
    nxb.TenNXB,
    ISNULL(SUM(ks.SoLuongHienTai), 0) AS SoLuongSach
FROM NHA_XUAT_BAN nxb
LEFT JOIN SACH s ON nxb.IdNXB = s.IdNXB
LEFT JOIN Kho_Sach ks ON s.IdS = ks.MaSach
GROUP BY nxb.MaNXB, nxb.TenNXB;
GO

CREATE VIEW ViewChiTietTheNhap AS
SELECT
    nv.MaNV AS MaNhanVien,
    tn.MaTheNhap AS MaTheNhap,
    s.MaSach AS MaSach,
    s.TenSach AS TenSach,
    tg.TenTacGia AS TenTacGia,
    nxb.TenNXB AS NhaXuatBan,
    tl.TenTheLoai AS TheLoai,
    s.NamXuatBan AS NamXuatBan,
    tn.NgayNhap AS NgayNhap,
    tn.TongSoLuongNhap AS SoLuong,
    tn.GiaNhap AS GiaNhap,
    tn.TongTienNhap AS ThanhTien,
    CASE 
        WHEN tn.TrangThai = 'ChuaNhap' THEN N'Chưa nhập'
        WHEN tn.TrangThai = 'DaNhap' THEN N'Đã nhập'
        ELSE N'Chưa nhập'
    END AS TrangThai
FROM
    The_Nhap tn
    INNER JOIN NhanVien nv ON tn.IdNV = nv.IdNV
    INNER JOIN SACH s ON tn.IdS = s.IdS
    INNER JOIN TAC_GIA tg ON s.IdTacGia = tg.IdTG
    INNER JOIN THE_LOAI tl ON s.IdTheLoai = tl.IdTL
    INNER JOIN NHA_XUAT_BAN nxb ON s.IdNXB = nxb.IdNXB;
GO

CREATE VIEW ViewLichSuNhapKho AS
SELECT
    tn.MaTheNhap,
    nv.MaNV AS MaNV,
    s.MaSach,
    nv.HoTen AS TenNhanVien,
    s.TenSach,
    tn.NgayNhap,
    tn.TongSoLuongNhap,
    tn.GiaNhap, -- Thêm cột GiaNhap
    tn.TongTienNhap,
    CASE
        WHEN tn.TrangThai = 'DaNhap' THEN N'Đã Nhập'
        WHEN tn.TrangThai = 'ChuaNhap' THEN N'Chưa Nhập'
        ELSE N'Chưa Nhập'
    END AS TrangThai
FROM The_Nhap tn
LEFT JOIN SACH s ON tn.IdS = s.IdS
LEFT JOIN NhanVien nv ON tn.IdNV = nv.IdNV;
GO

CREATE VIEW ViewChiTietNhapKho AS
SELECT
    nv.MaNV AS MaNhanVien,
    tn.MaTheNhap AS MaTheNhap,
    s.MaSach AS MaSach,
    s.TenSach AS TenSach,
    tg.MaTacGia AS MaTacGia,
    tg.TenTacGia AS TenTacGia,
    tl.MaTheLoai AS MaTheLoai,
    tl.TenTheLoai AS TheLoai,
    nxb.MaNXB AS MaNhaXuatBan,
    nxb.TenNXB AS TenNhaXuatBan,
    s.NamXuatBan AS NamXuatBan,
    ISNULL(ks.SoLuongHienTai, 0) AS SoLuong,
    tn.NgayNhap AS NgayNhap,
    tn.GiaNhap AS GiaNhap,
    tn.TongTienNhap AS ThanhTien,
    CASE 
        WHEN ISNULL(ks.TrangThaiSach, 'HetSach') = 'ConSach' THEN N'Còn Sách'
        WHEN ISNULL(ks.TrangThaiSach, 'HetSach') = 'HetSach' THEN N'Hết Sách'
        ELSE N'Hết Sách'
    END AS TrangThaiSach
FROM
    The_Nhap tn
    INNER JOIN NhanVien nv ON tn.IdNV = nv.IdNV
    INNER JOIN SACH s ON tn.IdS = s.IdS
    INNER JOIN TAC_GIA tg ON s.IdTacGia = tg.IdTG
    INNER JOIN THE_LOAI tl ON s.IdTheLoai = tl.IdTL
    INNER JOIN NHA_XUAT_BAN nxb ON s.IdNXB = nxb.IdNXB
    LEFT JOIN Kho_Sach ks ON s.IdS = ks.MaSach;
GO

