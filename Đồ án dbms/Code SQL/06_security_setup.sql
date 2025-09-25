CREATE ROLE AdminRole;
CREATE ROLE NhanVienRole;
ALTER ROLE NhanVienRole ADD MEMBER AdminRole;
GO

-- Gán quyền cho NhanVienRole (quyền cơ bản trên từng bảng)
GRANT SELECT, INSERT, UPDATE, DELETE ON TaiKhoan TO NhanVienRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON NhanVien TO NhanVienRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON TAC_GIA TO NhanVienRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON THE_LOAI TO NhanVienRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON NHA_XUAT_BAN TO NhanVienRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON SACH TO NhanVienRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON The_Nhap TO NhanVienRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON Kho_Sach TO NhanVienRole;
GRANT SELECT ON [Admin] TO NhanVienRole;

-- Gán quyền thực thi stored procedure cho NhanVienRole
GRANT EXECUTE ON sp_NhapSach TO NhanVienRole;
GRANT EXECUTE ON sp_CapNhatSach TO NhanVienRole;
GRANT EXECUTE ON sp_CapNhatKhoSach TO NhanVienRole;
GRANT EXECUTE ON sp_CapNhatAnhBia TO NhanVienRole;
GRANT EXECUTE ON sp_ThongKeSachDaNhap TO NhanVienRole;
GRANT EXECUTE ON sp_ThongKeNhapSachTheoNgay TO NhanVienRole;

-- Từ chối quyền thực thi sp_XacNhanNhap cho NhanVienRole
DENY EXECUTE ON sp_XacNhanNhap TO NhanVienRole;

-- Gán quyền thực thi hàm cho NhanVienRole
GRANT EXECUTE ON fn_KiemTraTrangThaiKhoSach TO NhanVienRole;

-- Gán quyền xem view cho NhanVienRole
GRANT SELECT ON ViewDanhSachSach TO NhanVienRole;
GRANT SELECT ON ViewDanhSachTheLoai TO NhanVienRole;
GRANT SELECT ON ViewDanhSachTacGia TO NhanVienRole;
GRANT SELECT ON ViewDanhSachNhaXuatBan TO NhanVienRole;
GRANT SELECT ON ViewLichSuNhapKho TO NhanVienRole;
GRANT SELECT ON ViewChiTietNhapKho TO NhanVienRole;
GRANT SELECT ON ViewChiTietTheNhap TO NhanVienRole;
GO

-- Gán quyền bổ sung cho AdminRole (chỉ quyền không kế thừa từ NhanVienRole)
GRANT SELECT, INSERT, UPDATE, DELETE ON [Admin] TO AdminRole;
GRANT EXECUTE ON sp_XacNhanNhap TO AdminRole;
GO