/*
    Hospital Management - cau truc co so du lieu Microsoft SQL Server.
    Nhom bang: Identity, bac si/nhan vien, ca va lich kham, don thuoc,
    dich vu xet nghiem/ket qua, goi kham va thong bao.

    Chay trong database dich da chon bang SSMS. Script theo doi phien ban
    bang lich su migration va chuan hoa danh muc cu sang tieng Viet, VND.
    Tren database rong, script khong tao tai khoan hay nhap danh muc mau.
    Gia co san la du lieu minh hoa cua do an.
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

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016032026_VietnameseCatalogAndVnd'
)
BEGIN
    -- Gia mau VND cua do an: nhan 1.000 cac so tien BDT da nhap.
    -- Day la bang gia minh hoa, khong phai ty gia thi truong.
    UPDATE [DiagnosticServices] SET [Price] = [Price] * 1000, [CurrencyCode] = N'VND' WHERE [CurrencyCode] = N'BDT';
    UPDATE [DiagnosticServices] SET [CurrencyCode] = N'VND' WHERE [CurrencyCode] <> N'VND';
    UPDATE [HealthChecks] SET [Price] = [Price] * 1000, [CurrencyCode] = N'VND' WHERE [CurrencyCode] = N'BDT';
    UPDATE [HealthChecks] SET [CurrencyCode] = N'VND' WHERE [CurrencyCode] <> N'VND';
    UPDATE [DiagnosticBookings] SET [Price] = [Price] * 1000, [CurrencyCode] = N'VND' WHERE [CurrencyCode] = N'BDT';
    UPDATE [DiagnosticBookings] SET [CurrencyCode] = N'VND' WHERE [CurrencyCode] <> N'VND';
    UPDATE [PackageBookings] SET [TotalAmount] = [TotalAmount] * 1000, [AmountPaid] = [AmountPaid] * 1000, [CurrencyCode] = N'VND' WHERE [CurrencyCode] = N'BDT';
    UPDATE [PackageBookings] SET [CurrencyCode] = N'VND' WHERE [CurrencyCode] <> N'VND';
    UPDATE [AspNetUsers] SET [FullName] = N'Nguyễn Minh Đức' WHERE [Id] = N'legacy-2' AND [FullName] = N'Dr. Xihad Ansari';
    UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa II - Nội tổng quát' WHERE [UserId] = N'legacy-2' AND [Designation] = N'MBBS, FCPS (Medicine)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Nội tổng quát, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-2' AND [About] = N'Mental health & counseling.';
    UPDATE [Doctors] SET [Speciality] = N'Nội tổng quát' WHERE [Speciality] = N'General Physician';
    UPDATE [AspNetUsers] SET [FullName] = N'Trần Quốc Huy' WHERE [Id] = N'legacy-3' AND [FullName] = N'Dr. Mahmud Hasan';
    UPDATE [Doctors] SET [Designation] = N'Thạc sĩ, Bác sĩ - Tim mạch' WHERE [UserId] = N'legacy-3' AND [Designation] = N'MBBS, MD (Cardiology)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Tim mạch, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-3' AND [About] = N'Heart care and cardiac consultation.';
    UPDATE [Doctors] SET [Speciality] = N'Tim mạch' WHERE [Speciality] = N'Cardiology';
    UPDATE [AspNetUsers] SET [FullName] = N'Lê Thu Hà' WHERE [Id] = N'legacy-4' AND [FullName] = N'Dr. Farzana Akter';
    UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa I - Thần kinh' WHERE [UserId] = N'legacy-4' AND [Designation] = N'MBBS, MD (Neurology)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Thần kinh, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-4' AND [About] = N'Neurological disorders & headache clinic.';
    UPDATE [Doctors] SET [Speciality] = N'Thần kinh' WHERE [Speciality] = N'Neurology';
    UPDATE [AspNetUsers] SET [FullName] = N'Phạm Hoàng Nam' WHERE [Id] = N'legacy-5' AND [FullName] = N'Dr. Shanto Roy';
    UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa II - Chấn thương chỉnh hình' WHERE [UserId] = N'legacy-5' AND [Designation] = N'MBBS, MS (Orthopedics)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Chấn thương chỉnh hình, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-5' AND [About] = N'Bone, joint & sports injury specialist.';
    UPDATE [Doctors] SET [Speciality] = N'Chấn thương chỉnh hình' WHERE [Speciality] = N'Orthopedics';
    UPDATE [AspNetUsers] SET [FullName] = N'Nguyễn Ngọc Lan' WHERE [Id] = N'legacy-6' AND [FullName] = N'Dr. Nabila Islam';
    UPDATE [Doctors] SET [Designation] = N'Thạc sĩ, Bác sĩ - Da liễu' WHERE [UserId] = N'legacy-6' AND [Designation] = N'MBBS, DDV (Dermatology)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Da liễu, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-6' AND [About] = N'Skin, hair & nail care.';
    UPDATE [Doctors] SET [Speciality] = N'Da liễu' WHERE [Speciality] = N'Dermatology';
    UPDATE [AspNetUsers] SET [FullName] = N'Đặng Anh Tuấn' WHERE [Id] = N'legacy-7' AND [FullName] = N'Dr. Tareq Amin';
    UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa I - Nhi khoa' WHERE [UserId] = N'legacy-7' AND [Designation] = N'MBBS, DCH, MD (Pediatrics)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Nhi khoa, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-7' AND [About] = N'Child health & vaccination.';
    UPDATE [Doctors] SET [Speciality] = N'Nhi khoa' WHERE [Speciality] = N'Pediatrics';
    UPDATE [AspNetUsers] SET [FullName] = N'Vũ Mai Phương' WHERE [Id] = N'legacy-8' AND [FullName] = N'Dr. Nusrat Jahan';
    UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa II - Sản phụ khoa' WHERE [UserId] = N'legacy-8' AND [Designation] = N'MBBS, FCPS (Gynae & Obs)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Sản phụ khoa, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-8' AND [About] = N'Women’s health & maternity care.';
    UPDATE [Doctors] SET [Speciality] = N'Sản phụ khoa' WHERE [Speciality] = N'Gynecology & Obstetrics';
    UPDATE [AspNetUsers] SET [FullName] = N'Bùi Quang Minh' WHERE [Id] = N'legacy-9' AND [FullName] = N'Dr. Imran Kabir';
    UPDATE [Doctors] SET [Designation] = N'Thạc sĩ, Bác sĩ - Tai Mũi Họng' WHERE [UserId] = N'legacy-9' AND [Designation] = N'MBBS, DLO (ENT)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Tai Mũi Họng, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-9' AND [About] = N'Ear, nose & throat diseases.';
    UPDATE [Doctors] SET [Speciality] = N'Tai Mũi Họng' WHERE [Speciality] = N'ENT';
    UPDATE [AspNetUsers] SET [FullName] = N'Đỗ Thanh Hương' WHERE [Id] = N'legacy-10' AND [FullName] = N'Dr. Sanjida Rahman';
    UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa I - Mắt' WHERE [UserId] = N'legacy-10' AND [Designation] = N'MBBS, DO (Ophthalmology)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Mắt, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-10' AND [About] = N'Eye specialist & cataract clinic.';
    UPDATE [Doctors] SET [Speciality] = N'Mắt' WHERE [Speciality] = N'Ophthalmology';
    UPDATE [AspNetUsers] SET [FullName] = N'Hoàng Việt Anh' WHERE [Id] = N'legacy-11' AND [FullName] = N'Dr. Rakibul Islam';
    UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa II - Tiêu hóa' WHERE [UserId] = N'legacy-11' AND [Designation] = N'MBBS, MD (Gastroenterology)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Tiêu hóa, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-11' AND [About] = N'Liver & digestive diseases.';
    UPDATE [Doctors] SET [Speciality] = N'Tiêu hóa' WHERE [Speciality] = N'Gastroenterology';
    UPDATE [AspNetUsers] SET [FullName] = N'Phan Thị Hạnh' WHERE [Id] = N'legacy-12' AND [FullName] = N'Dr. Shirin Akhter';
    UPDATE [Doctors] SET [Designation] = N'Thạc sĩ, Bác sĩ - Thận nội khoa' WHERE [UserId] = N'legacy-12' AND [Designation] = N'MBBS, MD (Nephrology)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Thận nội khoa, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-12' AND [About] = N'Kidney diseases & dialysis advice.';
    UPDATE [Doctors] SET [Speciality] = N'Thận nội khoa' WHERE [Speciality] = N'Nephrology';
    UPDATE [AspNetUsers] SET [FullName] = N'Võ Thành Đạt' WHERE [Id] = N'legacy-13' AND [FullName] = N'Dr. Arif Chowdhury';
    UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa I - Tiết niệu' WHERE [UserId] = N'legacy-13' AND [Designation] = N'MBBS, MS (Urology)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Tiết niệu, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-13' AND [About] = N'Urinary & male reproductive health.';
    UPDATE [Doctors] SET [Speciality] = N'Tiết niệu' WHERE [Speciality] = N'Urology';
    UPDATE [AspNetUsers] SET [FullName] = N'Dương Thùy Linh' WHERE [Id] = N'legacy-14' AND [FullName] = N'Dr. Tahmina Sultana';
    UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa II - Nội tiết' WHERE [UserId] = N'legacy-14' AND [Designation] = N'MBBS, MD (Endocrinology)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Nội tiết, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-14' AND [About] = N'Diabetes & hormone disorders.';
    UPDATE [Doctors] SET [Speciality] = N'Nội tiết' WHERE [Speciality] = N'Endocrinology';
    UPDATE [AspNetUsers] SET [FullName] = N'Ngô Trung Kiên' WHERE [Id] = N'legacy-15' AND [FullName] = N'Dr. Jubayer Ahmed';
    UPDATE [Doctors] SET [Designation] = N'Thạc sĩ, Bác sĩ - Ung bướu' WHERE [UserId] = N'legacy-15' AND [Designation] = N'MBBS, MD (Oncology)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Ung bướu, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-15' AND [About] = N'Cancer diagnosis & chemotherapy advice.';
    UPDATE [Doctors] SET [Speciality] = N'Ung bướu' WHERE [Speciality] = N'Oncology';
    UPDATE [AspNetUsers] SET [FullName] = N'Trịnh Bảo Châu' WHERE [Id] = N'legacy-16' AND [FullName] = N'Dr. Rukhsana Parvin';
    UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa I - Tâm thần' WHERE [UserId] = N'legacy-16' AND [Designation] = N'MBBS, MD (Psychiatry)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Tâm thần, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-16' AND [About] = N'Mental health & counseling.';
    UPDATE [Doctors] SET [Speciality] = N'Tâm thần' WHERE [Speciality] = N'Psychiatry';
    UPDATE [AspNetUsers] SET [FullName] = N'Đinh Hữu Phúc' WHERE [Id] = N'legacy-17' AND [FullName] = N'Dr. Morshed Alam';
    UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa II - Hô hấp' WHERE [UserId] = N'legacy-17' AND [Designation] = N'MBBS, MD (Pulmonology)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Hô hấp, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-17' AND [About] = N'Asthma, COPD & lung care.';
    UPDATE [Doctors] SET [Speciality] = N'Hô hấp' WHERE [Speciality] = N'Pulmonology';
    UPDATE [AspNetUsers] SET [FullName] = N'Hồ Khánh Linh' WHERE [Id] = N'legacy-18' AND [FullName] = N'Dr. Sumaiya Khan';
    UPDATE [Doctors] SET [Designation] = N'Thạc sĩ, Bác sĩ - Cơ xương khớp' WHERE [UserId] = N'legacy-18' AND [Designation] = N'MBBS, MD (Rheumatology)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Cơ xương khớp, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-18' AND [About] = N'Arthritis & autoimmune diseases.';
    UPDATE [Doctors] SET [Speciality] = N'Cơ xương khớp' WHERE [Speciality] = N'Rheumatology';
    UPDATE [AspNetUsers] SET [FullName] = N'Lý Đức Thịnh' WHERE [Id] = N'legacy-19' AND [FullName] = N'Dr. Nafis Rahim';
    UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa I - Huyết học' WHERE [UserId] = N'legacy-19' AND [Designation] = N'MBBS, MD (Hematology)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Huyết học, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-19' AND [About] = N'Blood disorders & anemia clinic.';
    UPDATE [Doctors] SET [Speciality] = N'Huyết học' WHERE [Speciality] = N'Hematology';
    UPDATE [AspNetUsers] SET [FullName] = N'Mai Hải Yến' WHERE [Id] = N'legacy-20' AND [FullName] = N'Dr. Tasnim Noor';
    UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa II - Răng Hàm Mặt' WHERE [UserId] = N'legacy-20' AND [Designation] = N'BDS, FCPS (Oral & Maxillofacial Surgery)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Răng Hàm Mặt, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-20' AND [About] = N'Oral surgery & dental implants.';
    UPDATE [Doctors] SET [Speciality] = N'Răng Hàm Mặt' WHERE [Speciality] = N'Dental Surgery';
    UPDATE [AspNetUsers] SET [FullName] = N'Tạ Minh Quân' WHERE [Id] = N'legacy-21' AND [FullName] = N'Dr. Reaz Ahmed';
    UPDATE [Doctors] SET [Designation] = N'Thạc sĩ, Bác sĩ - Gây mê hồi sức' WHERE [UserId] = N'legacy-21' AND [Designation] = N'MBBS, MD (Anesthesiology)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Gây mê hồi sức, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-21' AND [About] = N'Perioperative & pain management.';
    UPDATE [Doctors] SET [Speciality] = N'Gây mê hồi sức' WHERE [Speciality] = N'Anesthesiology';
    UPDATE [AspNetUsers] SET [FullName] = N'Cao Ngọc Ánh' WHERE [Id] = N'legacy-22' AND [FullName] = N'Dr. Lamia Hossain';
    UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa I - Phục hồi chức năng' WHERE [UserId] = N'legacy-22' AND [Designation] = N'MBBS, MD (PM&R)';
    UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Phục hồi chức năng, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-22' AND [About] = N'Rehab, physio & pain clinic.';
    UPDATE [Doctors] SET [Speciality] = N'Phục hồi chức năng' WHERE [Speciality] = N'Physical Medicine & Rehabilitation';
    UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm máu tổng quát', [Description] = N'Phân tích các chỉ số máu trong gói xét nghiệm tổng quát.' WHERE [Id] = 1 AND [Name] = N'Blood Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Chụp X-quang ngực', [Description] = N'Chụp X-quang vùng ngực theo chỉ định của bác sĩ.' WHERE [Id] = 2 AND [Name] = N'X-Ray Chest';
    UPDATE [DiagnosticServices] SET [Name] = N'Tổng phân tích nước tiểu', [Description] = N'Phân tích các thành phần trong mẫu nước tiểu.' WHERE [Id] = 3 AND [Name] = N'Urine Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm phân', [Description] = N'Phân tích mẫu phân theo yêu cầu xét nghiệm.' WHERE [Id] = 4 AND [Name] = N'Stool Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Chụp cộng hưởng từ não', [Description] = N'Chụp cộng hưởng từ vùng não theo chỉ định.' WHERE [Id] = 5 AND [Name] = N'MRI Brain';
    UPDATE [DiagnosticServices] SET [Name] = N'Điện tâm đồ', [Description] = N'Ghi nhận hoạt động điện của tim.' WHERE [Id] = 6 AND [Name] = N'ECG';
    UPDATE [DiagnosticServices] SET [Name] = N'Siêu âm ổ bụng', [Description] = N'Khảo sát các cơ quan trong ổ bụng bằng siêu âm.' WHERE [Id] = 7 AND [Name] = N'Ultrasound Abdomen';
    UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm chức năng tuyến giáp', [Description] = N'Định lượng các chỉ số đánh giá chức năng tuyến giáp.' WHERE [Id] = 8 AND [Name] = N'Thyroid Function Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Chụp cắt lớp vi tính ngực', [Description] = N'Chụp cắt lớp vi tính vùng ngực.' WHERE [Id] = 9 AND [Name] = N'CT Scan Chest';
    UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm mỡ máu', [Description] = N'Định lượng cholesterol và các thành phần lipid máu.' WHERE [Id] = 10 AND [Name] = N'Lipid Profile';
    UPDATE [DiagnosticServices] SET [Name] = N'Chụp nhũ ảnh', [Description] = N'Chụp X-quang tuyến vú theo chỉ định.' WHERE [Id] = 11 AND [Name] = N'Mammography';
    UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm chức năng gan', [Description] = N'Phân tích các chỉ số đánh giá chức năng gan.' WHERE [Id] = 12 AND [Name] = N'Liver Function Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Đo mật độ xương', [Description] = N'Khảo sát mật độ khoáng của xương.' WHERE [Id] = 13 AND [Name] = N'Bone Density Scan';
    UPDATE [DiagnosticServices] SET [Name] = N'Định lượng huyết sắc tố', [Description] = N'Định lượng hemoglobin trong máu.' WHERE [Id] = 14 AND [Name] = N'Hemoglobin Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Siêu âm tim', [Description] = N'Khảo sát cấu trúc và hoạt động của tim bằng siêu âm.' WHERE [Id] = 15 AND [Name] = N'Echo Cardiogram';
    UPDATE [DiagnosticServices] SET [Name] = N'Nghiệm pháp dung nạp glucose', [Description] = N'Đo đường huyết theo quy trình nghiệm pháp dung nạp glucose.' WHERE [Id] = 16 AND [Name] = N'Glucose Tolerance Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Chụp PET', [Description] = N'Chụp hình ảnh chuyển hóa bằng kỹ thuật PET theo chỉ định.' WHERE [Id] = 17 AND [Name] = N'PET Scan';
    UPDATE [DiagnosticServices] SET [Name] = N'Định lượng vitamin D', [Description] = N'Định lượng vitamin D trong mẫu máu.' WHERE [Id] = 18 AND [Name] = N'Vitamin D Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Nội soi đại tràng', [Description] = N'Thực hiện nội soi kiểm tra đại tràng theo chỉ định.' WHERE [Id] = 19 AND [Name] = N'Colonoscopy Preparation';
    UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm dị ứng', [Description] = N'Xét nghiệm các chỉ số dị ứng theo danh mục dịch vụ.' WHERE [Id] = 20 AND [Name] = N'Allergy Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Tổng phân tích tế bào máu', [Description] = N'Đếm và phân tích hồng cầu, bạch cầu, tiểu cầu.' WHERE [Id] = 21 AND [Name] = N'Complete Blood Count (CBC)';
    UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm chức năng thận', [Description] = N'Phân tích các chỉ số đánh giá chức năng thận.' WHERE [Id] = 22 AND [Name] = N'Kidney Function Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Định lượng HbA1c', [Description] = N'Định lượng HbA1c trong mẫu máu.' WHERE [Id] = 23 AND [Name] = N'HbA1c Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Định lượng PSA', [Description] = N'Định lượng kháng nguyên đặc hiệu tuyến tiền liệt.' WHERE [Id] = 24 AND [Name] = N'PSA Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm tế bào cổ tử cung', [Description] = N'Phân tích tế bào từ mẫu cổ tử cung.' WHERE [Id] = 25 AND [Name] = N'Pap Smear';
    UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm tốc độ máu lắng', [Description] = N'Đo tốc độ lắng của hồng cầu.' WHERE [Id] = 26 AND [Name] = N'ESR Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Định lượng vitamin B12', [Description] = N'Định lượng vitamin B12 trong mẫu máu.' WHERE [Id] = 27 AND [Name] = N'Vitamin B12 Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Định lượng sắt huyết thanh', [Description] = N'Định lượng sắt trong huyết thanh.' WHERE [Id] = 28 AND [Name] = N'Iron Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Sinh thiết tủy xương', [Description] = N'Lấy và phân tích mẫu tủy xương theo chỉ định.' WHERE [Id] = 29 AND [Name] = N'Bone Marrow Biopsy';
    UPDATE [DiagnosticServices] SET [Name] = N'Chụp mạch máu', [Description] = N'Khảo sát hình ảnh mạch máu theo chỉ định.' WHERE [Id] = 30 AND [Name] = N'Angiography';
    UPDATE [DiagnosticServices] SET [Name] = N'Đo mật độ xương DEXA', [Description] = N'Đo mật độ khoáng xương bằng kỹ thuật DEXA.' WHERE [Id] = 31 AND [Name] = N'DEXA Scan';
    UPDATE [DiagnosticServices] SET [Name] = N'Theo dõi điện tâm đồ Holter', [Description] = N'Ghi nhận điện tim liên tục bằng thiết bị Holter.' WHERE [Id] = 32 AND [Name] = N'Holter Monitor';
    UPDATE [DiagnosticServices] SET [Name] = N'Điện tâm đồ gắng sức', [Description] = N'Ghi nhận điện tim trong quá trình thực hiện nghiệm pháp gắng sức.' WHERE [Id] = 33 AND [Name] = N'Stress Test';
    UPDATE [DiagnosticServices] SET [Name] = N'Thông tim chẩn đoán', [Description] = N'Thực hiện thông tim phục vụ chẩn đoán theo chỉ định.' WHERE [Id] = 34 AND [Name] = N'Cardiac Catheterization';
    UPDATE [DiagnosticServices] SET [Name] = N'Sinh thiết mô', [Description] = N'Lấy mẫu mô và chuyển phân tích giải phẫu bệnh.' WHERE [Id] = 35 AND [Name] = N'Biopsy';
    UPDATE [DiagnosticServices] SET [Name] = N'Chọc hút tế bào bằng kim nhỏ', [Description] = N'Lấy mẫu tế bào bằng kỹ thuật chọc hút kim nhỏ.' WHERE [Id] = 36 AND [Name] = N'Fine Needle Aspiration';
    UPDATE [DiagnosticServices] SET [Name] = N'Điện não đồ', [Description] = N'Ghi nhận hoạt động điện của não.' WHERE [Id] = 37 AND [Name] = N'EEG';
    UPDATE [DiagnosticServices] SET [Name] = N'Điện cơ', [Description] = N'Ghi nhận hoạt động điện của cơ và dây thần kinh.' WHERE [Id] = 38 AND [Name] = N'EMG';
    UPDATE [DiagnosticServices] SET [Name] = N'Nội soi tiêu hóa', [Description] = N'Khảo sát đường tiêu hóa bằng nội soi.' WHERE [Id] = 39 AND [Name] = N'Endoscopy';
    UPDATE [DiagnosticServices] SET [Name] = N'Nội soi phế quản', [Description] = N'Khảo sát đường thở và phế quản bằng nội soi.' WHERE [Id] = 40 AND [Name] = N'Bronchoscopy';
    UPDATE [DiagnosticServices] SET [Name] = N'Nội soi bàng quang', [Description] = N'Khảo sát bàng quang và niệu đạo bằng nội soi.' WHERE [Id] = 41 AND [Name] = N'Cystoscopy';
    UPDATE [DiagnosticServices] SET [Name] = N'Đo chức năng hô hấp', [Description] = N'Đo các thông số thông khí của phổi.' WHERE [Id] = 42 AND [Name] = N'Spirometry';
    UPDATE [DiagnosticServices] SET [Name] = N'Đo thính lực', [Description] = N'Đo khả năng nghe theo quy trình thính lực.' WHERE [Id] = 43 AND [Name] = N'Audiometry';
    UPDATE [DiagnosticServices] SET [Category] = N'Xét nghiệm' WHERE [Category] = N'Laboratory';
    UPDATE [DiagnosticServices] SET [Category] = N'Chẩn đoán hình ảnh' WHERE [Category] = N'Radiology';
    UPDATE [DiagnosticServices] SET [Category] = N'Tim mạch' WHERE [Category] = N'Cardiology';
    UPDATE [DiagnosticServices] SET [Category] = N'Giải phẫu bệnh' WHERE [Category] = N'Pathology';
    UPDATE [DiagnosticServices] SET [Category] = N'Thần kinh' WHERE [Category] = N'Neurology';
    UPDATE [DiagnosticServices] SET [Category] = N'Tiêu hóa' WHERE [Category] = N'Gastroenterology';
    UPDATE [DiagnosticServices] SET [Category] = N'Hô hấp' WHERE [Category] = N'Pulmonology';
    UPDATE [DiagnosticServices] SET [Category] = N'Tiết niệu' WHERE [Category] = N'Urology';
    UPDATE [DiagnosticServices] SET [Category] = N'Tai Mũi Họng' WHERE [Category] = N'Otolaryngology';
    UPDATE [DiagnosticServices] SET [SampleType] = N'Máu' WHERE [SampleType] = N'Blood';
    UPDATE [DiagnosticServices] SET [SampleType] = N'Nước tiểu' WHERE [SampleType] = N'Urine';
    UPDATE [DiagnosticServices] SET [SampleType] = N'Phân' WHERE [SampleType] = N'Stool';
    UPDATE [DiagnosticServices] SET [SampleType] = N'Không lấy mẫu' WHERE [SampleType] = N'N/A';
    UPDATE [DiagnosticServices] SET [SampleType] = N'Mẫu cổ tử cung' WHERE [SampleType] = N'Cervical';
    UPDATE [DiagnosticServices] SET [SampleType] = N'Tủy xương' WHERE [SampleType] = N'Bone Marrow';
    UPDATE [DiagnosticServices] SET [SampleType] = N'Mô' WHERE [SampleType] = N'Tissue';
    UPDATE [HealthChecks] SET [Name] = N'Gói khám sức khỏe cơ bản', [Features] = N'Xét nghiệm máu
    Phân tích nước tiểu
    Khám và tư vấn tổng quát
    Đánh giá chỉ số khối cơ thể' WHERE [Id] = 1 AND [Name] = N'Basic Health Check';
    UPDATE [HealthChecks] SET [Name] = N'Gói khám sức khỏe nâng cao', [Features] = N'Tổng phân tích tế bào máu
    Xét nghiệm chức năng gan
    Kiểm tra tim mạch
    Xét nghiệm đường huyết
    Khám và tư vấn với bác sĩ' WHERE [Id] = 2 AND [Name] = N'Executive Health Check';
    UPDATE [HealthChecks] SET [Name] = N'Gói khám sức khỏe toàn diện', [Features] = N'Các hạng mục của gói nâng cao
    Xét nghiệm dấu ấn ung thư theo danh mục
    Kiểm tra tim mạch chuyên sâu
    Khám sức khỏe tổng quát
    Tư vấn cùng bác sĩ chuyên khoa' WHERE [Id] = 3 AND [Name] = N'Comprehensive Health Check';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016032026_VietnameseCatalogAndVnd'
)
BEGIN
    EXEC(N'ALTER TABLE [PackageBookings] ADD CONSTRAINT [CK_PackageBookings_CurrencyVnd] CHECK ([CurrencyCode] = N''VND'')');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016032026_VietnameseCatalogAndVnd'
)
BEGIN
    EXEC(N'ALTER TABLE [HealthChecks] ADD CONSTRAINT [CK_HealthChecks_CurrencyVnd] CHECK ([CurrencyCode] = N''VND'')');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016032026_VietnameseCatalogAndVnd'
)
BEGIN
    EXEC(N'ALTER TABLE [DiagnosticServices] ADD CONSTRAINT [CK_DiagnosticServices_CurrencyVnd] CHECK ([CurrencyCode] = N''VND'')');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016032026_VietnameseCatalogAndVnd'
)
BEGIN
    EXEC(N'ALTER TABLE [DiagnosticBookings] ADD CONSTRAINT [CK_DiagnosticBookings_CurrencyVnd] CHECK ([CurrencyCode] = N''VND'')');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016032026_VietnameseCatalogAndVnd'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261016032026_VietnameseCatalogAndVnd', N'8.0.28');
END;
GO

COMMIT;
GO

