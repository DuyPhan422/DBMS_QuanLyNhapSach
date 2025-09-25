CREATE PROCEDURE sp_NhapSach
    @MaNV VARCHAR(50),
    @TenSach NVARCHAR(255),
    @TenTacGia NVARCHAR(255),
    @TenTheLoai NVARCHAR(255),
    @TenNXB NVARCHAR(255),
    @NamXuatBan INT,
    @GiaNhap DECIMAL(10, 2),
    @SoLuong INT,
    @NgayNhap DATE
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @NamXuatBan > YEAR(GETDATE())
            THROW 50005, 'Năm xuất bản không được lớn hơn năm hiện tại!', 1;
        IF @GiaNhap < 0 OR @SoLuong < 0
            THROW 50006, 'Giá nhập hoặc số lượng không được âm!', 1;
        IF @TenSach IS NULL OR @TenSach = '' OR @TenTacGia IS NULL OR @TenTacGia = '' OR
           @TenTheLoai IS NULL OR @TenTheLoai = '' OR @TenNXB IS NULL OR @TenNXB = ''
            THROW 50008, 'Thông tin sách, tác giả, thể loại hoặc nhà xuất bản không được để trống!', 1;
        BEGIN TRANSACTION;
        DECLARE @IdNV INT;
        SELECT @IdNV = IdNV FROM NhanVien WHERE MaNV = @MaNV;
        IF @IdNV IS NULL
        BEGIN
            THROW 50001, 'Mã nhân viên không tồn tại!', 1;
        END;
        DECLARE @IdTacGia INT;
        IF NOT EXISTS (SELECT 1 FROM TAC_GIA WHERE TenTacGia = @TenTacGia)
            INSERT INTO TAC_GIA (TenTacGia) VALUES (@TenTacGia);
        SET @IdTacGia = (SELECT IdTG FROM TAC_GIA WHERE TenTacGia = @TenTacGia);
        DECLARE @IdTheLoai INT;
        IF NOT EXISTS (SELECT 1 FROM THE_LOAI WHERE TenTheLoai = @TenTheLoai)
            INSERT INTO THE_LOAI (TenTheLoai) VALUES (@TenTheLoai);
        SET @IdTheLoai = (SELECT IdTL FROM THE_LOAI WHERE TenTheLoai = @TenTheLoai);
        DECLARE @IdNXB INT;
        IF NOT EXISTS (SELECT 1 FROM NHA_XUAT_BAN WHERE TenNXB = @TenNXB)
            INSERT INTO NHA_XUAT_BAN (TenNXB) VALUES (@TenNXB);
        SET @IdNXB = (SELECT IdNXB FROM NHA_XUAT_BAN WHERE TenNXB = @TenNXB);
        DECLARE @IdS INT;
        DECLARE @MaSach VARCHAR(50);
        IF NOT EXISTS (SELECT 1 FROM SACH WHERE TenSach = @TenSach AND IdTacGia = @IdTacGia AND IdTheLoai = @IdTheLoai AND IdNXB = @IdNXB AND NamXuatBan = @NamXuatBan)
        BEGIN
            INSERT INTO SACH (TenSach, NamXuatBan, GiaSach, IdTacGia, IdTheLoai, IdNXB, AnhBia)
            VALUES (@TenSach, @NamXuatBan, @GiaNhap, @IdTacGia, @IdTheLoai, @IdNXB, NULL);
            SET @IdS = SCOPE_IDENTITY();
            SET @MaSach = (SELECT MaSach FROM SACH WHERE IdS = @IdS);
        END
        ELSE
        BEGIN
            SET @IdS = (SELECT TOP 1 IdS FROM SACH WHERE TenSach = @TenSach AND IdTacGia = @IdTacGia AND IdTheLoai = @IdTheLoai AND IdNXB = @IdNXB AND NamXuatBan = @NamXuatBan);
            SET @MaSach = (SELECT MaSach FROM SACH WHERE IdS = @IdS);
        END;
        IF @IdS IS NULL
        BEGIN
            THROW 50004, 'Không thể lấy hoặc tạo IdS cho sách!', 1;
        END;
        DECLARE @TongTienNhap DECIMAL(10, 2) = @GiaNhap * @SoLuong;
        DECLARE @IdTN INT;
        INSERT INTO The_Nhap (IdNV, IdS, NgayNhap, TrangThai, GiaNhap, TongSoLuongNhap, TongTienNhap)
        VALUES (@IdNV, @IdS, @NgayNhap, 'ChuaNhap', @GiaNhap, @SoLuong, @TongTienNhap);
        SET @IdTN = SCOPE_IDENTITY();
        DECLARE @MaTheNhap VARCHAR(50);
        SET @MaTheNhap = (SELECT MaTheNhap FROM The_Nhap WHERE IdTN = @IdTN);
        IF @MaTheNhap IS NULL
        BEGIN
            THROW 50009, 'Không thể tạo MaTheNhap!', 1;
        END;
        SELECT @MaSach AS MaSach, @MaTheNhap AS MaTheNhap;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;
GO

CREATE PROCEDURE sp_CapNhatSach
    @MaTheNhap VARCHAR(50),
    @TenSach NVARCHAR(255),
    @TenTacGia NVARCHAR(255),
    @TenTheLoai NVARCHAR(255),
    @TenNXB NVARCHAR(255),
    @NamXuatBan INT,
    @GiaNhap DECIMAL(10, 2),
    @SoLuong INT,
    @NgayNhap DATE
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @NamXuatBan > YEAR(GETDATE())
            THROW 50005, 'Năm xuất bản không được lớn hơn năm hiện tại!', 1;
        IF @GiaNhap < 0 OR @SoLuong < 0
            THROW 50006, 'Giá nhập hoặc số lượng không được âm!', 1;
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM The_Nhap WHERE MaTheNhap = @MaTheNhap)
        BEGIN
            THROW 50002, 'Mã thẻ nhập không tồn tại!', 1;
        END;
        DECLARE @TrangThai VARCHAR(50);
        SELECT @TrangThai = TrangThai
        FROM The_Nhap
        WHERE MaTheNhap = @MaTheNhap;
        IF @TrangThai = 'DaNhap'
        BEGIN
            THROW 50007, 'Thẻ nhập đã được xác nhận, không thể chỉnh sửa thông tin!', 1;
        END;
        DECLARE @IdS INT;
        SELECT @IdS = IdS
        FROM The_Nhap
        WHERE MaTheNhap = @MaTheNhap;
        DECLARE @IdTacGia INT;
        IF NOT EXISTS (SELECT 1 FROM TAC_GIA WHERE TenTacGia = @TenTacGia)
            INSERT INTO TAC_GIA (TenTacGia) VALUES (@TenTacGia);
        SET @IdTacGia = (SELECT IdTG FROM TAC_GIA WHERE TenTacGia = @TenTacGia);
        DECLARE @IdTheLoai INT;
        IF NOT EXISTS (SELECT 1 FROM THE_LOAI WHERE TenTheLoai = @TenTheLoai)
            INSERT INTO THE_LOAI (TenTheLoai) VALUES (@TenTheLoai);
        SET @IdTheLoai = (SELECT IdTL FROM THE_LOAI WHERE TenTheLoai = @TenTheLoai);
        DECLARE @IdNXB INT;
        IF NOT EXISTS (SELECT 1 FROM NHA_XUAT_BAN WHERE TenNXB = @TenNXB)
            INSERT INTO NHA_XUAT_BAN (TenNXB) VALUES (@TenNXB);
        SET @IdNXB = (SELECT IdNXB FROM NHA_XUAT_BAN WHERE TenNXB = @TenNXB);
        UPDATE SACH
        SET TenSach = @TenSach,
            NamXuatBan = @NamXuatBan,
            GiaSach = @GiaNhap,
            IdTacGia = @IdTacGia,
            IdTheLoai = @IdTheLoai,
            IdNXB = @IdNXB
        WHERE IdS = @IdS;
        DECLARE @TongTienNhap DECIMAL(10, 2) = @GiaNhap * @SoLuong;
        UPDATE The_Nhap
        SET NgayNhap = @NgayNhap,
            GiaNhap = @GiaNhap,
            TongSoLuongNhap = @SoLuong,
            TongTienNhap = @TongTienNhap
        WHERE MaTheNhap = @MaTheNhap;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;
GO

CREATE PROCEDURE sp_XacNhanNhap
    @MaTheNhap VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM The_Nhap WHERE MaTheNhap = @MaTheNhap AND TrangThai = 'ChuaNhap')
        BEGIN
            THROW 50003, 'Mã thẻ nhập không tồn tại hoặc đã được xác nhận!', 1;
        END;
        DECLARE @IdS INT, @SoLuong INT;
        SELECT @IdS = IdS, @SoLuong = TongSoLuongNhap
        FROM The_Nhap
        WHERE MaTheNhap = @MaTheNhap;
        UPDATE The_Nhap
        SET TrangThai = 'DaNhap'
        WHERE MaTheNhap = @MaTheNhap;
        DECLARE @ResultMessage NVARCHAR(4000);
        EXEC sp_CapNhatKhoSach @IdS, @SoLuong, @ResultMessage OUTPUT;
        IF @ResultMessage != N'Cập nhật thành công'
        BEGIN
            THROW 50004, @ResultMessage, 1;
        END;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

CREATE PROCEDURE sp_XoaSach
    @MaTheNhap VARCHAR(50),
    @MaSach VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM The_Nhap WHERE MaTheNhap = @MaTheNhap)
        BEGIN
            THROW 50002, 'Mã thẻ nhập không tồn tại!', 1;
        END;
        DECLARE @TrangThai VARCHAR(50);
        SELECT @TrangThai = TrangThai
        FROM The_Nhap
        WHERE MaTheNhap = @MaTheNhap;
        IF @TrangThai = 'DaNhap'
        BEGIN
            THROW 50007, 'Thẻ nhập đã được xác nhận, không thể xóa!', 1;
        END;
        IF NOT EXISTS (SELECT 1 FROM SACH WHERE MaSach = @MaSach)
        BEGIN
            THROW 50010, 'Mã sách không tồn tại!', 1;
        END;
        DECLARE @IdS INT;
        SELECT @IdS = IdS
        FROM The_Nhap
        WHERE MaTheNhap = @MaTheNhap;
        DECLARE @IdTacGia INT, @IdTheLoai INT, @IdNXB INT;
        SELECT @IdTacGia = IdTacGia, @IdTheLoai = IdTheLoai, @IdNXB = IdNXB
        FROM SACH
        WHERE MaSach = @MaSach;
        BEGIN TRANSACTION;
        DELETE FROM The_Nhap WHERE MaTheNhap = @MaTheNhap;
        DELETE FROM SACH WHERE MaSach = @MaSach;
        IF NOT EXISTS (SELECT 1 FROM SACH WHERE IdTacGia = @IdTacGia)
        BEGIN
            DELETE FROM TAC_GIA WHERE IdTG = @IdTacGia;
        END;
        IF NOT EXISTS (SELECT 1 FROM SACH WHERE IdTheLoai = @IdTheLoai)
        BEGIN
            DELETE FROM THE_LOAI WHERE IdTL = @IdTheLoai;
        END;
        IF NOT EXISTS (SELECT 1 FROM SACH WHERE IdNXB = @IdNXB)
        BEGIN
            DELETE FROM NHA_XUAT_BAN WHERE IdNXB = @IdNXB;
        END;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;
GO

CREATE PROCEDURE sp_CapNhatAnhBia
    @MaSach VARCHAR(50),
    @AnhBia VARBINARY(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM SACH WHERE MaSach = @MaSach)
            THROW 50010, 'Mã sách không tồn tại!', 1;
        UPDATE SACH
        SET AnhBia = @AnhBia
        WHERE MaSach = @MaSach;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE sp_CapNhatKhoSach  
    @IdS INT,  
    @SoLuongThem INT,  
    @ResultMessage NVARCHAR(4000) OUTPUT  
AS  
BEGIN  
    SET NOCOUNT ON;  
    BEGIN TRY
        DECLARE @SoLuongHienTai INT;  
        DECLARE @TongSoLuongNhap INT;  
        DECLARE @HasNhapKho BIT;  
        DECLARE @SoLuongMoi INT;  
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM SACH WHERE IdS = @IdS)  
        BEGIN  
            SET @ResultMessage = N'Không có thông tin trong kho';  
            ROLLBACK TRANSACTION;
            RETURN 1;  
        END;  
        SET @HasNhapKho = (SELECT COUNT(*) FROM The_Nhap WHERE IdS = @IdS AND TrangThai = 'DaNhap');  
        IF @HasNhapKho = 0  
        BEGIN  
            SET @ResultMessage = N'Không có thông tin trong kho';  
            ROLLBACK TRANSACTION;
            RETURN 1;  
        END;  
        SELECT @SoLuongHienTai = ISNULL(SoLuongHienTai, 0)  
        FROM Kho_Sach  
        WHERE MaSach = @IdS;  
        SET @SoLuongMoi = @SoLuongHienTai + @SoLuongThem;  
        IF @SoLuongMoi < 0  
        BEGIN  
            SET @ResultMessage = N'Số lượng trong kho hiện tại là ' + CAST(@SoLuongHienTai AS NVARCHAR(10)) + N' không đủ sách để giảm ' + CAST(ABS(@SoLuongThem) AS NVARCHAR(10)) + N'\nCập nhật thất bại';  
            ROLLBACK TRANSACTION;
            RETURN 1;  
        END;  
        SELECT @TongSoLuongNhap = ISNULL(SUM(TongSoLuongNhap), 0)  
        FROM The_Nhap  
        WHERE IdS = @IdS AND TrangThai = 'DaNhap';  
        IF @SoLuongMoi > @TongSoLuongNhap  
        BEGIN  
            SET @ResultMessage = N'Số lượng thêm vượt quá số lượng đã nhập là ' + CAST(@TongSoLuongNhap AS NVARCHAR(10)) + N'\nCập nhật thất bại';  
            ROLLBACK TRANSACTION;
            RETURN 1;  
        END;  
        IF EXISTS (SELECT 1 FROM Kho_Sach WHERE MaSach = @IdS)  
        BEGIN  
            UPDATE Kho_Sach  
            SET SoLuongHienTai = @SoLuongMoi,  
                TrangThaiSach = CASE WHEN @SoLuongMoi > 0 THEN N'ConSach' ELSE N'HetSach' END  
            WHERE MaSach = @IdS;  
        END  
        ELSE IF @SoLuongThem > 0  
        BEGIN  
            INSERT INTO Kho_Sach (MaSach, SoLuongHienTai, TrangThaiSach)  
            VALUES (@IdS, @SoLuongThem, N'ConSach');  
        END;  
        SET @ResultMessage = N'Cập nhật thành công';  
        COMMIT TRANSACTION;
        RETURN 0;  
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        SET @ResultMessage = @ErrorMessage;
        RETURN 1;
    END CATCH;
END;
GO

CREATE PROCEDURE sp_ThongKeSachDaNhap
    @TuNgay DATE = NULL,
    @DenNgay DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        s.MaSach,
        s.TenSach,
        SUM(tn.TongSoLuongNhap) AS TongSoLuongNhap
    FROM The_Nhap tn
    INNER JOIN SACH s ON tn.IdS = s.IdS
    WHERE tn.TrangThai = 'DaNhap'
        AND (@TuNgay IS NULL OR tn.NgayNhap >= @TuNgay)
        AND (@DenNgay IS NULL OR tn.NgayNhap <= @DenNgay)
    GROUP BY s.MaSach, s.TenSach
    ORDER BY s.MaSach;
END;
GO

CREATE PROCEDURE sp_ThongKeNhapSachTheoNgay
    @TuNgay DATE = NULL,
    @DenNgay DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        tn.NgayNhap AS NgayNhap,
        SUM(tn.TongSoLuongNhap) AS TongSoLuongNhap
    FROM The_Nhap tn
    WHERE tn.TrangThai = 'DaNhap'
        AND (@TuNgay IS NULL OR tn.NgayNhap >= @TuNgay)
        AND (@DenNgay IS NULL OR tn.NgayNhap <= @DenNgay)
    GROUP BY tn.NgayNhap
    ORDER BY tn.NgayNhap;
END;
GO