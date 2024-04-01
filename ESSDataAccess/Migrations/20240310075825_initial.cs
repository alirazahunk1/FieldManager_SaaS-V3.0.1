using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESSDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddonSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsProductModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsTaskModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsNoticeModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDynamicFormModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsExpenseModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsLeaveModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDocumentModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsChatModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsLoanModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsPaymentCollectionModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsGeofenceModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsIpBasedAttendanceModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsUidLoginModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsClientVisitModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsOfflineTrackingModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDataImportExportModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsSiteModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsQrCodeAttendanceModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDynamicQrCodeAttendanceEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsBreakModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsSalesTargetModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsAiChatModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddonSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhoneCountryCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CurrencySymbol = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    DistanceUnit = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CenterLatitude = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CenterLongitude = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MapZoomLevel = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    MapKey = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LogoURL = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    VerifyClientNumber = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeCodePrefix = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OrderPrefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Access = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Radius = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DynamicQrs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UniqueId = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    Pin = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    QrValue = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    QrLastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QrUpdateInterval = table.Column<int>(type: "int", nullable: true),
                    QrExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicQrs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    IsImgRequired = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ForFor = table.Column<int>(type: "int", nullable: false),
                    IsClientRequired = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeofenceGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeofenceGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IpGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    IsImgRequired = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    NoticeFor = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsPromotional = table.Column<bool>(type: "bit", nullable: false),
                    IsProductModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsTaskModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsNoticeModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDynamicFormModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsExpenseModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsLeaveModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDocumentModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsChatModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsLoanModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsPaymentCollectionModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsGeofenceModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsIpBasedAttendanceModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsUidLoginModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsClientVisitModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsOfflineTrackingModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDataImportExportModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsSiteModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsQrCodeAttendanceModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDynamicQrCodeAttendanceEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsBreakModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsSalesTargetModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsAiChatModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsBiometricVerificationModuleEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QrCodeGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrCodeGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SASettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhoneCountryCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CurrencySymbol = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    MAppVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MLocationUpdateIntervalType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MLocationUpdateInterval = table.Column<int>(type: "int", nullable: false),
                    MPrivacyPolicyLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApiBaseUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WebBaseUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TwilioStatus = table.Column<bool>(type: "bit", nullable: false),
                    TwilioAccountSid = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TwilioAuthToken = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    TwilioFromNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    OfflineCheckTimeType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OfflineCheckTime = table.Column<int>(type: "int", nullable: false),
                    PaypalClientId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaypalClientSecret = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaypalMode = table.Column<int>(type: "int", nullable: true),
                    LeaveDocumentPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PaySlipPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ExpenseDocumentPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DefaultPaymentGateway = table.Column<int>(type: "int", nullable: false),
                    RazorPayKeyId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RazorPayKeySecret = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BillingCycleDate = table.Column<int>(type: "int", nullable: false),
                    DueDays = table.Column<int>(type: "int", nullable: false),
                    OverDueDays = table.Column<int>(type: "int", nullable: false),
                    InvoicePrefix = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IsProductModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsTaskModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsNoticeModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDynamicFormModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsExpenseModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsLeaveModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDocumentModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsChatModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsLoanModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsPaymentCollectionModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsGeofenceModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsIpBasedAttendanceModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsUidLoginModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsClientVisitModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsOfflineTrackingModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDataImportExportModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsSiteModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsQrCodeAttendanceModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDynamicQrCodeAttendanceEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsBreakModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsSalesTargetModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsAiChatModuleEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsBiometricVerificationRequired = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SASettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAutoCheckOutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AutoCheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Sunday = table.Column<bool>(type: "bit", nullable: false),
                    Monday = table.Column<bool>(type: "bit", nullable: false),
                    Tuesday = table.Column<bool>(type: "bit", nullable: false),
                    Wednesday = table.Column<bool>(type: "bit", nullable: false),
                    Thursday = table.Column<bool>(type: "bit", nullable: false),
                    Friday = table.Column<bool>(type: "bit", nullable: false),
                    Saturday = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsChatEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    FieldType = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Placeholder = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    MinLength = table.Column<int>(type: "int", nullable: true),
                    MaxLength = table.Column<int>(type: "int", nullable: true),
                    DefaultValues = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Values = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormFields_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Geofences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    Radius = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    GeofenceGroupId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Geofences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Geofences_GeofenceGroups_GeofenceGroupId",
                        column: x => x.GeofenceGroupId,
                        principalTable: "GeofenceGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IpAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpGroupId = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IpAddresses_IpGroups_IpGroupId",
                        column: x => x.IpGroupId,
                        principalTable: "IpGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PlanId = table.Column<int>(type: "int", nullable: true),
                    TotalEmployeesCount = table.Column<int>(type: "int", nullable: false),
                    AvailableEmployeesCount = table.Column<int>(type: "int", nullable: false),
                    SubscriptionStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenants_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Stocks = table.Column<int>(type: "int", nullable: true),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsTaxable = table.Column<bool>(type: "bit", nullable: false),
                    IsDiscountable = table.Column<bool>(type: "bit", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsNew = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QrCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QrCodeGroupId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    QrCode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QrCodes_QrCodeGroups_QrCodeGroupId",
                        column: x => x.QrCodeGroupId,
                        principalTable: "QrCodeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    Radius = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsAttendanceEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AttendanceType = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    DynamicQrId = table.Column<int>(type: "int", nullable: true),
                    QrGroupId = table.Column<int>(type: "int", nullable: true),
                    GeofenceGroupId = table.Column<int>(type: "int", nullable: true),
                    IpGroupId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sites_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sites_DynamicQrs_DynamicQrId",
                        column: x => x.DynamicQrId,
                        principalTable: "DynamicQrs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sites_GeofenceGroups_GeofenceGroupId",
                        column: x => x.GeofenceGroupId,
                        principalTable: "GeofenceGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sites_IpGroups_IpGroupId",
                        column: x => x.IpGroupId,
                        principalTable: "IpGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sites_QrCodeGroups_QrGroupId",
                        column: x => x.QrGroupId,
                        principalTable: "QrCodeGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeamNotices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoticeId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamNotices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamNotices_Notices_NoticeId",
                        column: x => x.NoticeId,
                        principalTable: "Notices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamNotices_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeesCount = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    PerEmployeeAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UniqueId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImgUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AlternatePhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfJoining = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BaseSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    PrimarySalesTarget = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    SecondarySalesTarget = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    AvailableLeaveCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GeofenceGroupId = table.Column<int>(type: "int", nullable: true),
                    IpGroupId = table.Column<int>(type: "int", nullable: true),
                    QrCodeGroupId = table.Column<int>(type: "int", nullable: true),
                    DynamicQrId = table.Column<int>(type: "int", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    AttendanceType = table.Column<int>(type: "int", nullable: true),
                    SalaryType = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_DynamicQrs_DynamicQrId",
                        column: x => x.DynamicQrId,
                        principalTable: "DynamicQrs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_GeofenceGroups_GeofenceGroupId",
                        column: x => x.GeofenceGroupId,
                        principalTable: "GeofenceGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_IpGroups_IpGroupId",
                        column: x => x.IpGroupId,
                        principalTable: "IpGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_QrCodeGroups_QrCodeGroupId",
                        column: x => x.QrCodeGroupId,
                        principalTable: "QrCodeGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LateReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EarlyCheckOutReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    ApprovedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShiftId = table.Column<int>(type: "int", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Schedules_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Schedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attendances_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Changes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chats_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    IsTyping = table.Column<bool>(type: "bit", nullable: false),
                    StatusMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatStatus_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestedById = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActionTakenById = table.Column<int>(type: "int", nullable: true),
                    ActionTakenOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApproverRemarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DocumentPath = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentRequests_AspNetUsers_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentRequests_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DynamicQrVerificationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    QrCode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DynamicQrId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicQrVerificationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicQrVerificationLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DynamicQrVerificationLogs_DynamicQrs_DynamicQrId",
                        column: x => x.DynamicQrId,
                        principalTable: "DynamicQrs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DynamicQrVerificationLogs_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeSettings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttendanceId = table.Column<int>(type: "int", nullable: true),
                    ForDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Approvedby = table.Column<int>(type: "int", nullable: true),
                    ApprovedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpenseTypeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ApprovedAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ApproverRemarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpenseRequests_ExpenseTypes_ExpenseTypeId",
                        column: x => x.ExpenseTypeId,
                        principalTable: "ExpenseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormAssignments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FormAssignments_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormAssignments_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormEntries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormEntries_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FormEntries_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GeofenceVerificationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    GeofenceGroupId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeofenceVerificationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeofenceVerificationLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GeofenceVerificationLogs_GeofenceGroups_GeofenceGroupId",
                        column: x => x.GeofenceGroupId,
                        principalTable: "GeofenceGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeofenceVerificationLogs_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "IPVerificationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IP = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    IPGroupId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPVerificationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IPVerificationLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IPVerificationLogs_IpGroups_IPGroupId",
                        column: x => x.IPGroupId,
                        principalTable: "IpGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IPVerificationLogs_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaveTypeId = table.Column<int>(type: "int", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true),
                    ApproverRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ApprovedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoanRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ApprovedAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    ApproverRemarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActionTakenOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    IsViewed = table.Column<bool>(type: "bit", nullable: false),
                    FromUserId = table.Column<int>(type: "int", nullable: true),
                    ToUserId = table.Column<int>(type: "int", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    EmployeesCount = table.Column<int>(type: "int", nullable: false),
                    PerEmployeeAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PaymentResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentGateway = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentCollections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProofUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentCollections_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentCollections_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    OrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserRemarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CancelRemarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdminRemarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProcessedById = table.Column<int>(type: "int", nullable: true),
                    ProcessedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedById = table.Column<int>(type: "int", nullable: true),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOrders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductOrders_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PushTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PushTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QrVerificationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    QrCode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    QrGroupId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrVerificationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QrVerificationLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QrVerificationLogs_QrCodeGroups_QrGroupId",
                        column: x => x.QrGroupId,
                        principalTable: "QrCodeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QrVerificationLogs_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ResetPassword",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Otp = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetPassword", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResetPassword_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Period = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Target = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Achieved = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(2,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Incentive = table.Column<decimal>(type: "decimal(30,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesTargets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TaskType = table.Column<int>(type: "int", nullable: false),
                    AssignedById = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    IsGeoFenceEnabled = table.Column<bool>(type: "bit", nullable: false),
                    MaxRadius = table.Column<int>(type: "int", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ForDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartFormId = table.Column<int>(type: "int", nullable: true),
                    EndFormId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tasks_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Board = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SdkVersion = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatteryPercentage = table.Column<int>(type: "int", nullable: false),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    IsGPSOn = table.Column<bool>(type: "bit", nullable: false),
                    IsWifiOn = table.Column<bool>(type: "bit", nullable: false),
                    SignalStrength = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: true),
                    IsMock = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDevices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserNotices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoticeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNotices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserNotices_Notices_NoticeId",
                        column: x => x.NoticeId,
                        principalTable: "Notices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Breaks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AttendanceId = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breaks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Breaks_Attendances_AttendanceId",
                        column: x => x.AttendanceId,
                        principalTable: "Attendances",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CheckInOuts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttendanceId = table.Column<int>(type: "int", nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckInOuts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckInOuts_Attendances_AttendanceId",
                        column: x => x.AttendanceId,
                        principalTable: "Attendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trackings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttendanceId = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    Altitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    Bearing = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    Speed = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: true),
                    IsMock = table.Column<bool>(type: "bit", nullable: false),
                    BatteryPercentage = table.Column<int>(type: "int", nullable: false),
                    IsGPSOn = table.Column<bool>(type: "bit", nullable: false),
                    IsWifiOn = table.Column<bool>(type: "bit", nullable: false),
                    SignalStrength = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    Accuracy = table.Column<int>(type: "int", nullable: true),
                    Activity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsOffline = table.Column<bool>(type: "bit", nullable: false),
                    OfflineSyncedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trackings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trackings_Attendances_AttendanceId",
                        column: x => x.AttendanceId,
                        principalTable: "Attendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Visits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttendanceId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visits_Attendances_AttendanceId",
                        column: x => x.AttendanceId,
                        principalTable: "Attendances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Visits_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormEntryFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormEntryId = table.Column<int>(type: "int", nullable: false),
                    FormFieldId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormEntryFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormEntryFields_FormEntries_FormEntryId",
                        column: x => x.FormEntryId,
                        principalTable: "FormEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormEntryFields_FormFields_FormFieldId",
                        column: x => x.FormFieldId,
                        principalTable: "FormFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPromotional = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantSubscriptions_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenantSubscriptions_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TenantSubscriptions_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderLines_ProductOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "ProductOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskUpdates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    TaskUpdateType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskUpdates_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DynamicQrId",
                table: "AspNetUsers",
                column: "DynamicQrId",
                unique: true,
                filter: "[DynamicQrId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GeofenceGroupId",
                table: "AspNetUsers",
                column: "GeofenceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IpGroupId",
                table: "AspNetUsers",
                column: "IpGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_QrCodeGroupId",
                table: "AspNetUsers",
                column: "QrCodeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ScheduleId",
                table: "AspNetUsers",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SiteId",
                table: "AspNetUsers",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TeamId",
                table: "AspNetUsers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EmployeeId",
                table: "Attendances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ShiftId",
                table: "Attendances",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_SiteId",
                table: "Attendances",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Breaks_AttendanceId",
                table: "Breaks",
                column: "AttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_TeamId",
                table: "Chats",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_UserId",
                table: "Chats",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatStatus_UserId",
                table: "ChatStatus",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckInOuts_AttendanceId",
                table: "CheckInOuts",
                column: "AttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ClientId",
                table: "Contracts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_SiteId",
                table: "Contracts",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentRequests_DocumentTypeId",
                table: "DocumentRequests",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentRequests_RequestedById",
                table: "DocumentRequests",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicQrVerificationLogs_DynamicQrId",
                table: "DynamicQrVerificationLogs",
                column: "DynamicQrId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicQrVerificationLogs_SiteId",
                table: "DynamicQrVerificationLogs",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicQrVerificationLogs_UserId",
                table: "DynamicQrVerificationLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSettings_UserId",
                table: "EmployeeSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseRequests_ExpenseTypeId",
                table: "ExpenseRequests",
                column: "ExpenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseRequests_UserId",
                table: "ExpenseRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FormAssignments_FormId",
                table: "FormAssignments",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormAssignments_TeamId",
                table: "FormAssignments",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_FormAssignments_UserId",
                table: "FormAssignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FormEntries_ClientId",
                table: "FormEntries",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_FormEntries_FormId",
                table: "FormEntries",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormEntries_UserId",
                table: "FormEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FormEntryFields_FormEntryId",
                table: "FormEntryFields",
                column: "FormEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_FormEntryFields_FormFieldId",
                table: "FormEntryFields",
                column: "FormFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFields_FormId",
                table: "FormFields",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_Geofences_GeofenceGroupId",
                table: "Geofences",
                column: "GeofenceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GeofenceVerificationLogs_GeofenceGroupId",
                table: "GeofenceVerificationLogs",
                column: "GeofenceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GeofenceVerificationLogs_SiteId",
                table: "GeofenceVerificationLogs",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_GeofenceVerificationLogs_UserId",
                table: "GeofenceVerificationLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PlanId",
                table: "Invoices",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_TenantId",
                table: "Invoices",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_IpAddresses_IpGroupId",
                table: "IpAddresses",
                column: "IpGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_IPVerificationLogs_IPGroupId",
                table: "IPVerificationLogs",
                column: "IPGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_IPVerificationLogs_SiteId",
                table: "IPVerificationLogs",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_IPVerificationLogs_UserId",
                table: "IPVerificationLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_UserId",
                table: "LeaveRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanRequests_UserId",
                table: "LoanRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ToUserId",
                table: "Notifications",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_OrderId",
                table: "OrderLines",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_ProductId",
                table: "OrderLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AppUserId",
                table: "Orders",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PlanId",
                table: "Orders",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCollections_ClientId",
                table: "PaymentCollections",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCollections_UserId",
                table: "PaymentCollections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrders_ClientId",
                table: "ProductOrders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrders_UserId",
                table: "ProductOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PushTokens_UserId",
                table: "PushTokens",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QrCodes_QrCodeGroupId",
                table: "QrCodes",
                column: "QrCodeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_QrVerificationLogs_QrGroupId",
                table: "QrVerificationLogs",
                column: "QrGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_QrVerificationLogs_SiteId",
                table: "QrVerificationLogs",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_QrVerificationLogs_UserId",
                table: "QrVerificationLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResetPassword_UserId",
                table: "ResetPassword",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesTargets_UserId",
                table: "SalesTargets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_ClientId",
                table: "Sites",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_DynamicQrId",
                table: "Sites",
                column: "DynamicQrId",
                unique: true,
                filter: "[DynamicQrId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_GeofenceGroupId",
                table: "Sites",
                column: "GeofenceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_IpGroupId",
                table: "Sites",
                column: "IpGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_QrGroupId",
                table: "Sites",
                column: "QrGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ClientId",
                table: "Tasks",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_EmployeeId",
                table: "Tasks",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SiteId",
                table: "Tasks",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskUpdates_TaskId",
                table: "TaskUpdates",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamNotices_NoticeId",
                table: "TeamNotices",
                column: "NoticeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamNotices_TeamId",
                table: "TeamNotices",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_PlanId",
                table: "Tenants",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSubscriptions_OrderId",
                table: "TenantSubscriptions",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantSubscriptions_PlanId",
                table: "TenantSubscriptions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSubscriptions_TenantId",
                table: "TenantSubscriptions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Trackings_AttendanceId",
                table: "Trackings",
                column: "AttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_UserId",
                table: "UserDevices",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserNotices_NoticeId",
                table: "UserNotices",
                column: "NoticeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotices_UserId",
                table: "UserNotices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_AttendanceId",
                table: "Visits",
                column: "AttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_ClientId",
                table: "Visits",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddonSettings");

            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Breaks");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "ChatStatus");

            migrationBuilder.DropTable(
                name: "CheckInOuts");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "DocumentRequests");

            migrationBuilder.DropTable(
                name: "DynamicQrVerificationLogs");

            migrationBuilder.DropTable(
                name: "EmployeeSettings");

            migrationBuilder.DropTable(
                name: "ExpenseRequests");

            migrationBuilder.DropTable(
                name: "FormAssignments");

            migrationBuilder.DropTable(
                name: "FormEntryFields");

            migrationBuilder.DropTable(
                name: "Geofences");

            migrationBuilder.DropTable(
                name: "GeofenceVerificationLogs");

            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "IpAddresses");

            migrationBuilder.DropTable(
                name: "IPVerificationLogs");

            migrationBuilder.DropTable(
                name: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "LoanRequests");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OrderLines");

            migrationBuilder.DropTable(
                name: "PaymentCollections");

            migrationBuilder.DropTable(
                name: "PushTokens");

            migrationBuilder.DropTable(
                name: "QrCodes");

            migrationBuilder.DropTable(
                name: "QrVerificationLogs");

            migrationBuilder.DropTable(
                name: "ResetPassword");

            migrationBuilder.DropTable(
                name: "SalesTargets");

            migrationBuilder.DropTable(
                name: "SASettings");

            migrationBuilder.DropTable(
                name: "TaskUpdates");

            migrationBuilder.DropTable(
                name: "TeamNotices");

            migrationBuilder.DropTable(
                name: "TenantSubscriptions");

            migrationBuilder.DropTable(
                name: "Trackings");

            migrationBuilder.DropTable(
                name: "UserDevices");

            migrationBuilder.DropTable(
                name: "UserNotices");

            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "ExpenseTypes");

            migrationBuilder.DropTable(
                name: "FormEntries");

            migrationBuilder.DropTable(
                name: "FormFields");

            migrationBuilder.DropTable(
                name: "LeaveTypes");

            migrationBuilder.DropTable(
                name: "ProductOrders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Notices");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Sites");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "DynamicQrs");

            migrationBuilder.DropTable(
                name: "GeofenceGroups");

            migrationBuilder.DropTable(
                name: "IpGroups");

            migrationBuilder.DropTable(
                name: "QrCodeGroups");

            migrationBuilder.DropTable(
                name: "Plans");
        }
    }
}
