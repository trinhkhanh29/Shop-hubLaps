IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE TABLE [ChuDe] (
        [machude] int NOT NULL IDENTITY,
        [tenchude] nvarchar(60) NOT NULL,
        [slug] varchar(70) NOT NULL,
        [hinh] varchar(70) NOT NULL,
        CONSTRAINT [PK_ChuDe] PRIMARY KEY ([machude])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE TABLE [DonHang] (
        [madon] int NOT NULL IDENTITY,
        [thanhtoan] bit NULL,
        [giaohang] bit NULL,
        [ngaydat] datetime2 NULL,
        [ngaygiao] datetime2 NULL,
        [makh] nvarchar(128) NOT NULL,
        [tinhtrang] char(1) NOT NULL,
        CONSTRAINT [PK_DonHang] PRIMARY KEY ([madon])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE TABLE [Hang] (
        [mahang] int NOT NULL IDENTITY,
        [tenhang] nvarchar(30) NOT NULL,
        [hinh] varchar(70) NOT NULL,
        CONSTRAINT [PK_Hang] PRIMARY KEY ([mahang])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE TABLE [LienHe] (
        [malienhe] int NOT NULL IDENTITY,
        [hoten] nvarchar(50) NOT NULL,
        [email] varchar(254) NOT NULL,
        [dienthoai] varchar(50) NOT NULL,
        [website] nvarchar(100) NOT NULL,
        [noidung] ntext NOT NULL,
        [trangthai] bit NULL,
        CONSTRAINT [PK_LienHe] PRIMARY KEY ([malienhe])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE TABLE [NhuCau] (
        [manhucau] int NOT NULL IDENTITY,
        [tennhucau] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_NhuCau] PRIMARY KEY ([manhucau])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE TABLE [QuangCao] (
        [maqc] int NOT NULL IDENTITY,
        [tenqc] nvarchar(255) NOT NULL,
        [tencongty] nvarchar(200) NOT NULL,
        [hinhnen] varchar(100) NOT NULL,
        [link] varchar(100) NOT NULL,
        [ngaybatdau] smalldatetime NULL,
        [ngayhethan] smalldatetime NULL,
        [trangthai] bit NOT NULL,
        CONSTRAINT [PK_QuangCao] PRIMARY KEY ([maqc])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE TABLE [TinTuc] (
        [matin] int NOT NULL IDENTITY,
        [tieude] nvarchar(255) NOT NULL,
        [hinhnen] varchar(70) NOT NULL,
        [tomtat] nvarchar(255) NOT NULL,
        [slug] nvarchar(100) NOT NULL,
        [noidung] ntext NOT NULL,
        [luotxem] int NULL,
        [ngaycapnhat] smalldatetime NULL,
        [xuatban] bit NULL,
        [machude] int NULL,
        [ChuDemachude] int NOT NULL,
        CONSTRAINT [PK_TinTuc] PRIMARY KEY ([matin]),
        CONSTRAINT [FK_TinTuc_ChuDe_ChuDemachude] FOREIGN KEY ([ChuDemachude]) REFERENCES [ChuDe] ([machude]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE TABLE [Laptop] (
        [malaptop] int NOT NULL IDENTITY,
        [tenlaptop] nvarchar(100) NOT NULL,
        [giaban] decimal(18,0) NULL,
        [mota] ntext NOT NULL,
        [hinh] varchar(70) NOT NULL,
        [mahang] int NULL,
        [manhucau] int NULL,
        [cpu] nvarchar(100) NOT NULL,
        [gpu] nvarchar(100) NOT NULL,
        [ram] nvarchar(100) NOT NULL,
        [hardware] nvarchar(100) NOT NULL,
        [manhinh] nvarchar(100) NOT NULL,
        [ngaycapnhat] datetime2 NULL,
        [soluongton] int NULL,
        [pin] nvarchar(100) NOT NULL,
        [trangthai] bit NULL,
        [Hangmahang] int NOT NULL,
        [NhuCaumanhucau] int NOT NULL,
        CONSTRAINT [PK_Laptop] PRIMARY KEY ([malaptop]),
        CONSTRAINT [FK_Laptop_Hang_Hangmahang] FOREIGN KEY ([Hangmahang]) REFERENCES [Hang] ([mahang]) ON DELETE CASCADE,
        CONSTRAINT [FK_Laptop_NhuCau_NhuCaumanhucau] FOREIGN KEY ([NhuCaumanhucau]) REFERENCES [NhuCau] ([manhucau]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE TABLE [BinhLuan] (
        [mabinhluan] int NOT NULL IDENTITY,
        [ten] nvarchar(50) NOT NULL,
        [noidung] ntext NOT NULL,
        [vote] int NULL,
        [ngaybinhluan] datetime2 NULL,
        [matin] int NULL,
        [trangthai] bit NULL,
        [TinTucmatin] int NOT NULL,
        CONSTRAINT [PK_BinhLuan] PRIMARY KEY ([mabinhluan]),
        CONSTRAINT [FK_BinhLuan_TinTuc_TinTucmatin] FOREIGN KEY ([TinTucmatin]) REFERENCES [TinTuc] ([matin]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE TABLE [DanhGia] (
        [madanhgia] int NOT NULL IDENTITY,
        [ten] nvarchar(50) NOT NULL,
        [noidung] ntext NOT NULL,
        [vote] int NULL,
        [ngaydanhgia] datetime2 NULL,
        [malaptop] int NULL,
        [trangthai] bit NULL,
        [Laptopmalaptop] int NOT NULL,
        CONSTRAINT [PK_DanhGia] PRIMARY KEY ([madanhgia]),
        CONSTRAINT [FK_DanhGia_Laptop_Laptopmalaptop] FOREIGN KEY ([Laptopmalaptop]) REFERENCES [Laptop] ([malaptop]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE TABLE [MetaLaptop] (
        [mameta] int NOT NULL IDENTITY,
        [keymeta] nvarchar(255) NOT NULL,
        [valuemeta] nvarchar(255) NOT NULL,
        [malaptop] int NULL,
        [Laptopmalaptop] int NOT NULL,
        CONSTRAINT [PK_MetaLaptop] PRIMARY KEY ([mameta]),
        CONSTRAINT [FK_MetaLaptop_Laptop_Laptopmalaptop] FOREIGN KEY ([Laptopmalaptop]) REFERENCES [Laptop] ([malaptop]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE INDEX [IX_BinhLuan_TinTucmatin] ON [BinhLuan] ([TinTucmatin]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE INDEX [IX_DanhGia_Laptopmalaptop] ON [DanhGia] ([Laptopmalaptop]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE INDEX [IX_Laptop_Hangmahang] ON [Laptop] ([Hangmahang]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE INDEX [IX_Laptop_NhuCaumanhucau] ON [Laptop] ([NhuCaumanhucau]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE INDEX [IX_MetaLaptop_Laptopmalaptop] ON [MetaLaptop] ([Laptopmalaptop]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    CREATE INDEX [IX_TinTuc_ChuDemachude] ON [TinTuc] ([ChuDemachude]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020122802_UpdateDataModel'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241020122802_UpdateDataModel', N'8.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020125136_UpdateChiTietDonHang'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[DonHang]') AND [c].[name] = N'makh');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [DonHang] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [DonHang] DROP COLUMN [makh];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020125136_UpdateChiTietDonHang'
)
BEGIN
    ALTER TABLE [DonHang] ADD [UserId] nvarchar(450) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020125136_UpdateChiTietDonHang'
)
BEGIN
    CREATE TABLE [ChiTietDonHang] (
        [madon] int NOT NULL,
        [malaptop] int NOT NULL,
        [soluong] int NULL,
        [dongia] decimal(18,0) NULL,
        CONSTRAINT [PK_ChiTietDonHang] PRIMARY KEY ([madon], [malaptop]),
        CONSTRAINT [FK_ChiTietDonHang_DonHang_madon] FOREIGN KEY ([madon]) REFERENCES [DonHang] ([madon]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ChiTietDonHang_Laptop_malaptop] FOREIGN KEY ([malaptop]) REFERENCES [Laptop] ([malaptop]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020125136_UpdateChiTietDonHang'
)
BEGIN
    CREATE TABLE [IdentityUser] (
        [Id] nvarchar(450) NOT NULL,
        [UserName] nvarchar(max) NULL,
        [NormalizedUserName] nvarchar(max) NULL,
        [Email] nvarchar(max) NULL,
        [NormalizedEmail] nvarchar(max) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_IdentityUser] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020125136_UpdateChiTietDonHang'
)
BEGIN
    CREATE INDEX [IX_DonHang_UserId] ON [DonHang] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020125136_UpdateChiTietDonHang'
)
BEGIN
    CREATE INDEX [IX_ChiTietDonHang_malaptop] ON [ChiTietDonHang] ([malaptop]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020125136_UpdateChiTietDonHang'
)
BEGIN
    ALTER TABLE [DonHang] ADD CONSTRAINT [FK_DonHang_IdentityUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [IdentityUser] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020125136_UpdateChiTietDonHang'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241020125136_UpdateChiTietDonHang', N'8.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020131426_UpdateSampleUserAndRelations'
)
BEGIN
    ALTER TABLE [DonHang] DROP CONSTRAINT [FK_DonHang_IdentityUser_UserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020131426_UpdateSampleUserAndRelations'
)
BEGIN
    DROP TABLE [IdentityUser];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020131426_UpdateSampleUserAndRelations'
)
BEGIN
    CREATE TABLE [SampleUsers] (
        [Id] nvarchar(450) NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [NgaySinh] datetime2 NULL,
        [Profile] nvarchar(max) NOT NULL,
        [Avatar] nvarchar(max) NOT NULL,
        [HoTen] nvarchar(max) NOT NULL,
        [DiaChi] nvarchar(max) NOT NULL,
        [UserName] nvarchar(max) NULL,
        [NormalizedUserName] nvarchar(max) NULL,
        [Email] nvarchar(max) NULL,
        [NormalizedEmail] nvarchar(max) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_SampleUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020131426_UpdateSampleUserAndRelations'
)
BEGIN
    ALTER TABLE [DonHang] ADD CONSTRAINT [FK_DonHang_SampleUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [SampleUsers] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020131426_UpdateSampleUserAndRelations'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241020131426_UpdateSampleUserAndRelations', N'8.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    ALTER TABLE [DonHang] DROP CONSTRAINT [FK_DonHang_SampleUsers_UserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    DROP INDEX [IX_DonHang_UserId] ON [DonHang];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ChiTietDonHang]') AND [c].[name] = N'soluong');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [ChiTietDonHang] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [ChiTietDonHang] DROP COLUMN [soluong];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SampleUsers]') AND [c].[name] = N'UserName');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [SampleUsers] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [SampleUsers] ALTER COLUMN [UserName] nvarchar(256) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    DROP INDEX [UserNameIndex] ON [SampleUsers];
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SampleUsers]') AND [c].[name] = N'NormalizedUserName');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [SampleUsers] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [SampleUsers] ALTER COLUMN [NormalizedUserName] nvarchar(256) NULL;
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [SampleUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    DROP INDEX [EmailIndex] ON [SampleUsers];
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SampleUsers]') AND [c].[name] = N'NormalizedEmail');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [SampleUsers] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [SampleUsers] ALTER COLUMN [NormalizedEmail] nvarchar(256) NULL;
    CREATE INDEX [EmailIndex] ON [SampleUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SampleUsers]') AND [c].[name] = N'LastName');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [SampleUsers] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [SampleUsers] ALTER COLUMN [LastName] nvarchar(100) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SampleUsers]') AND [c].[name] = N'HoTen');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [SampleUsers] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [SampleUsers] ALTER COLUMN [HoTen] nvarchar(50) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SampleUsers]') AND [c].[name] = N'FirstName');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [SampleUsers] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [SampleUsers] ALTER COLUMN [FirstName] nvarchar(100) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SampleUsers]') AND [c].[name] = N'Email');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [SampleUsers] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [SampleUsers] ALTER COLUMN [Email] nvarchar(256) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SampleUsers]') AND [c].[name] = N'DiaChi');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [SampleUsers] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [SampleUsers] ALTER COLUMN [DiaChi] nvarchar(200) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SampleUsers]') AND [c].[name] = N'Avatar');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [SampleUsers] DROP CONSTRAINT [' + @var10 + '];');
    ALTER TABLE [SampleUsers] ALTER COLUMN [Avatar] nvarchar(70) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ChiTietDonHang]') AND [c].[name] = N'dongia');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [ChiTietDonHang] DROP CONSTRAINT [' + @var11 + '];');
    ALTER TABLE [ChiTietDonHang] ALTER COLUMN [dongia] decimal(18,0) NOT NULL;
    ALTER TABLE [ChiTietDonHang] ADD DEFAULT 0.0 FOR [dongia];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    DECLARE @var12 sysname;
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ChiTietDonHang]') AND [c].[name] = N'malaptop');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [ChiTietDonHang] DROP CONSTRAINT [' + @var12 + '];');
    ALTER TABLE [ChiTietDonHang] ALTER COLUMN [malaptop] int NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    DECLARE @var13 sysname;
    SELECT @var13 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ChiTietDonHang]') AND [c].[name] = N'madon');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [ChiTietDonHang] DROP CONSTRAINT [' + @var13 + '];');
    ALTER TABLE [ChiTietDonHang] ALTER COLUMN [madon] int NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    CREATE TABLE [DonHang] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NULL,
        CONSTRAINT [PK_DonHang] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DonHang_SampleUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [SampleUsers] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    CREATE INDEX [IX_DonHang_UserId] ON [DonHang] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241020173758_UpdateDatabaseSchema'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241020173758_UpdateDatabaseSchema', N'8.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241021054306_AddSoluongToChiTietDonHang'
)
BEGIN
    ALTER TABLE [ChiTietDonHang] ADD [soluong] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241021054306_AddSoluongToChiTietDonHang'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241021054306_AddSoluongToChiTietDonHang', N'8.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241021054715_RemoveHangAndNhuCauFromLaptop'
)
BEGIN
    ALTER TABLE [Laptop] DROP CONSTRAINT [FK_Laptop_Hang_Hangmahang];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241021054715_RemoveHangAndNhuCauFromLaptop'
)
BEGIN
    ALTER TABLE [Laptop] DROP CONSTRAINT [FK_Laptop_NhuCau_NhuCaumanhucau];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241021054715_RemoveHangAndNhuCauFromLaptop'
)
BEGIN
    DECLARE @var14 sysname;
    SELECT @var14 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Laptop]') AND [c].[name] = N'Hangmahang');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Laptop] DROP CONSTRAINT [' + @var14 + '];');
    ALTER TABLE [Laptop] DROP COLUMN [Hangmahang];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241021054715_RemoveHangAndNhuCauFromLaptop'
)
BEGIN
    DECLARE @var15 sysname;
    SELECT @var15 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Laptop]') AND [c].[name] = N'NhuCaumanhucau');
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Laptop] DROP CONSTRAINT [' + @var15 + '];');
    ALTER TABLE [Laptop] DROP COLUMN [NhuCaumanhucau];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241021054715_RemoveHangAndNhuCauFromLaptop'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241021054715_RemoveHangAndNhuCauFromLaptop', N'8.0.10');
END;
GO

COMMIT;
GO

