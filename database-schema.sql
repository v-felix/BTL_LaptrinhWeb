/*
    Hospital Management - cau truc co so du lieu Microsoft SQL Server.
    Nhom bang: Identity, bac si/nhan vien, ca va lich kham, don thuoc,
    dich vu xet nghiem/ket qua, goi kham va thong bao.

    Chay trong database dich da chon bang SSMS. Script dung lich su
    migration de kiem tra cac phien ban schema da ap dung.
    Chi tao/cap nhat schema; khong nhap danh muc hoac tai khoan mau.
    Quy tac model va quan he nam trong Data/ApplicationDbContext.cs.
*/

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
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [FullName] nvarchar(150) NOT NULL,
        [Photo] nvarchar(500) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
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
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [DiagnosticServices] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        [Category] nvarchar(150) NOT NULL,
        [Description] nvarchar(4000) NULL,
        [SampleType] nvarchar(100) NULL,
        [Price] decimal(18,2) NOT NULL,
        [Duration] int NOT NULL,
        [ReportHours] int NOT NULL,
        [HomeCollectionAvailable] bit NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_DiagnosticServices] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [HealthChecks] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Features] nvarchar(4000) NOT NULL,
        [Popular] bit NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_HealthChecks] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [News] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(250) NOT NULL,
        [Excerpt] nvarchar(1000) NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [Category] nvarchar(100) NOT NULL,
        [PublishedDate] date NOT NULL,
        [IsPublished] bit NOT NULL,
        CONSTRAINT [PK_News] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [Doctors] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [Designation] nvarchar(200) NOT NULL,
        [Speciality] nvarchar(150) NOT NULL,
        [Phone] nvarchar(30) NULL,
        [About] nvarchar(4000) NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Doctors] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Doctors_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [Staff] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [Designation] nvarchar(200) NULL,
        [Department] nvarchar(150) NULL,
        [Phone] nvarchar(30) NULL,
        [About] nvarchar(4000) NULL,
        CONSTRAINT [PK_Staff] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Staff_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [PackageBookings] (
        [Id] int NOT NULL IDENTITY,
        [Reference] nvarchar(30) NOT NULL,
        [PatientId] nvarchar(450) NOT NULL,
        [HealthCheckId] int NOT NULL,
        [BookingDate] date NOT NULL,
        [DepositPercent] int NOT NULL,
        [TotalAmount] decimal(18,2) NOT NULL,
        [AmountPaid] decimal(18,2) NOT NULL,
        [Status] nvarchar(20) NOT NULL,
        [CancellationReason] nvarchar(1000) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_PackageBookings] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PackageBookings_AspNetUsers_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PackageBookings_HealthChecks_HealthCheckId] FOREIGN KEY ([HealthCheckId]) REFERENCES [HealthChecks] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [Appointments] (
        [Id] int NOT NULL IDENTITY,
        [Reference] nvarchar(30) NOT NULL,
        [AccessToken] nvarchar(64) NOT NULL,
        [PatientId] nvarchar(450) NULL,
        [DoctorId] int NOT NULL,
        [AppointmentDate] date NOT NULL,
        [AppointmentTime] time NOT NULL,
        [SerialNo] int NOT NULL,
        [PatientName] nvarchar(150) NOT NULL,
        [PatientPhone] nvarchar(30) NOT NULL,
        [PatientEmail] nvarchar(256) NOT NULL,
        [PatientAge] int NOT NULL,
        [PatientGender] nvarchar(20) NOT NULL,
        [Problem] nvarchar(2000) NULL,
        [Status] nvarchar(20) NOT NULL,
        [CancellationReason] nvarchar(1000) NULL,
        [Fee] decimal(18,2) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_Appointments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Appointments_AspNetUsers_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Appointments_Doctors_DoctorId] FOREIGN KEY ([DoctorId]) REFERENCES [Doctors] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [DoctorMessages] (
        [Id] int NOT NULL IDENTITY,
        [DoctorId] int NOT NULL,
        [AdminId] nvarchar(450) NOT NULL,
        [Message] nvarchar(2000) NOT NULL,
        [IsRead] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_DoctorMessages] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DoctorMessages_AspNetUsers_AdminId] FOREIGN KEY ([AdminId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_DoctorMessages_Doctors_DoctorId] FOREIGN KEY ([DoctorId]) REFERENCES [Doctors] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [Schedules] (
        [Id] int NOT NULL IDENTITY,
        [DoctorId] int NOT NULL,
        [DayOfWeek] int NOT NULL,
        [StartTime] time NOT NULL,
        [EndTime] time NOT NULL,
        [SlotMinutes] int NOT NULL,
        [MaxPatientsPerDay] int NOT NULL,
        [Fee] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_Schedules] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Schedules_Doctors_DoctorId] FOREIGN KEY ([DoctorId]) REFERENCES [Doctors] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [DiagnosticBookings] (
        [Id] int NOT NULL IDENTITY,
        [Reference] nvarchar(30) NOT NULL,
        [PatientId] nvarchar(450) NOT NULL,
        [DiagnosticServiceId] int NOT NULL,
        [AppointmentId] int NULL,
        [BookingDate] date NOT NULL,
        [BookingTime] time NOT NULL,
        [PatientName] nvarchar(150) NOT NULL,
        [PatientPhone] nvarchar(30) NOT NULL,
        [Address] nvarchar(500) NULL,
        [Notes] nvarchar(2000) NULL,
        [Price] decimal(18,2) NOT NULL,
        [Status] nvarchar(20) NOT NULL,
        [PaymentStatus] nvarchar(20) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_DiagnosticBookings] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DiagnosticBookings_Appointments_AppointmentId] FOREIGN KEY ([AppointmentId]) REFERENCES [Appointments] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_DiagnosticBookings_AspNetUsers_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_DiagnosticBookings_DiagnosticServices_DiagnosticServiceId] FOREIGN KEY ([DiagnosticServiceId]) REFERENCES [DiagnosticServices] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [Prescriptions] (
        [Id] int NOT NULL IDENTITY,
        [AppointmentId] int NOT NULL,
        [Diagnosis] nvarchar(2000) NOT NULL,
        [MedicineDetails] nvarchar(max) NOT NULL,
        [Advice] nvarchar(4000) NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Prescriptions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Prescriptions_Appointments_AppointmentId] FOREIGN KEY ([AppointmentId]) REFERENCES [Appointments] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE TABLE [TestResults] (
        [Id] int NOT NULL IDENTITY,
        [BookingDiagnosticId] int NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [Conclusion] nvarchar(2000) NULL,
        [FileName] nvarchar(100) NULL,
        [CreatedById] nvarchar(450) NOT NULL,
        [DeliveredAt] datetime2 NOT NULL,
        CONSTRAINT [PK_TestResults] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TestResults_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TestResults_DiagnosticBookings_BookingDiagnosticId] FOREIGN KEY ([BookingDiagnosticId]) REFERENCES [DiagnosticBookings] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Appointments_DoctorId_AppointmentDate_AppointmentTime] ON [Appointments] ([DoctorId], [AppointmentDate], [AppointmentTime]) WHERE [Status] <> N''Cancelled''');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Appointments_DoctorId_AppointmentDate_SerialNo] ON [Appointments] ([DoctorId], [AppointmentDate], [SerialNo]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [IX_Appointments_PatientId] ON [Appointments] ([PatientId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Appointments_Reference] ON [Appointments] ([Reference]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [IX_DiagnosticBookings_AppointmentId] ON [DiagnosticBookings] ([AppointmentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [IX_DiagnosticBookings_DiagnosticServiceId] ON [DiagnosticBookings] ([DiagnosticServiceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [IX_DiagnosticBookings_PatientId] ON [DiagnosticBookings] ([PatientId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE UNIQUE INDEX [IX_DiagnosticBookings_Reference] ON [DiagnosticBookings] ([Reference]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [IX_DoctorMessages_AdminId] ON [DoctorMessages] ([AdminId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [IX_DoctorMessages_DoctorId] ON [DoctorMessages] ([DoctorId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Doctors_UserId] ON [Doctors] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [IX_PackageBookings_HealthCheckId] ON [PackageBookings] ([HealthCheckId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [IX_PackageBookings_PatientId] ON [PackageBookings] ([PatientId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PackageBookings_Reference] ON [PackageBookings] ([Reference]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [IX_Prescriptions_AppointmentId] ON [Prescriptions] ([AppointmentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Schedules_DoctorId_DayOfWeek] ON [Schedules] ([DoctorId], [DayOfWeek]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Staff_UserId] ON [Staff] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [IX_TestResults_BookingDiagnosticId] ON [TestResults] ([BookingDiagnosticId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    CREATE INDEX [IX_TestResults_CreatedById] ON [TestResults] ([CreatedById]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015144600_InitialHospital'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261015144600_InitialHospital', N'8.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [AspNetRoleClaims] DROP CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [PackageBookings] ADD [CurrencyCode] nvarchar(3) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [HealthChecks] ADD [CurrencyCode] nvarchar(3) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [DiagnosticServices] ADD [CurrencyCode] nvarchar(3) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [DiagnosticBookings] ADD [CurrencyCode] nvarchar(3) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [AspNetRoleClaims] ADD CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [AspNetUserClaims] ADD CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [AspNetUserLogins] ADD CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [AspNetUserRoles] ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [AspNetUserRoles] ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    ALTER TABLE [AspNetUserTokens] ADD CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261015145152_IdentityCleanupAndCurrency'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261015145152_IdentityCleanupAndCurrency', N'8.0.28');
END;
GO

COMMIT;
GO

