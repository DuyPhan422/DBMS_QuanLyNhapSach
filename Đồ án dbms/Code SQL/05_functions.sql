CREATE FUNCTION fn_KiemTraTrangThaiKhoSach (@p_IdS INT)  
RETURNS NVARCHAR(300)  
AS  
BEGIN  
    DECLARE @KetQua NVARCHAR(300);  
    DECLARE @MaSach NVARCHAR(50);  
    DECLARE @TenSach NVARCHAR(100);  
    DECLARE @SoLuong INT;  
    DECLARE @TrangThai NVARCHAR(50);  
    DECLARE @MaNV NVARCHAR(50);  
    DECLARE @HoTen NVARCHAR(50);  
    DECLARE @MaTheNhap NVARCHAR(50);  
    SELECT @MaSach = MaSach, @TenSach = TenSach  
    FROM SACH  
    WHERE IdS = @p_IdS;  
    IF @MaSach IS NULL  
    BEGIN  
        SET @KetQua = N'Mã sách ' + CAST(@p_IdS AS NVARCHAR(10)) + N' không tồn tại trong danh mục sách';  
        RETURN @KetQua;  
    END;  
    SELECT TOP 1   
        @MaNV = nv.MaNV,  
        @HoTen = nv.HoTen,  
        @MaTheNhap = tn.MaTheNhap,  
        @SoLuong = ISNULL(ks.SoLuongHienTai, 0),  
        @TrangThai = CASE   
            WHEN ISNULL(ks.SoLuongHienTai, 0) > 0 THEN N'Còn Sách'  
            ELSE N'Hết Sách'  
        END  
    FROM The_Nhap tn  
    LEFT JOIN NhanVien nv ON tn.IdNV = nv.IdNV  
    LEFT JOIN Kho_Sach ks ON tn.IdS = ks.MaSach  
    WHERE tn.IdS = @p_IdS AND tn.TrangThai = 'DaNhap'  
    ORDER BY tn.NgayNhap DESC;  
    IF @MaNV IS NULL  
    BEGIN  
        SET @KetQua = N'Mã sách ' + @MaSach + N' chưa có dữ liệu nhập kho';  
        RETURN @KetQua;  
    END;  
    SET @KetQua = N'Mã nhân viên: ' + @MaNV + NCHAR(13) + NCHAR(10) +  
                  N'Tên nhân viên: ' + @HoTen + NCHAR(13) + NCHAR(10) +  
                  N'Mã thẻ nhập: ' + @MaTheNhap + NCHAR(13) + NCHAR(10) +  
                  N'Mã sách: ' + @MaSach + NCHAR(13) + NCHAR(10) +  
                  N'Tên sách: ' + @TenSach + NCHAR(13) + NCHAR(10) +  
                  N'Số lượng: ' + CAST(@SoLuong AS NVARCHAR(10)) + NCHAR(13) + NCHAR(10) +  
                  N'Trạng thái: ' + @TrangThai;  
    RETURN @KetQua;  
END;  
