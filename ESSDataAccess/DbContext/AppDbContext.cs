using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSDataAccess.Models.Addons;
using ESSDataAccess.Models.Base;
using ESSDataAccess.Models.Chat;
using ESSDataAccess.Models.Contract;
using ESSDataAccess.Models.Document;
using ESSDataAccess.Models.Form;
using ESSDataAccess.Models.Loan;
using ESSDataAccess.Models.Logs;
using ESSDataAccess.Models.Notice;
using ESSDataAccess.Models.Order;
using ESSDataAccess.Models.Product;
using ESSDataAccess.Models.Qr;
using ESSDataAccess.Models.Site;
using ESSDataAccess.Models.Task;
using ESSDataAccess.Tenant;
using ESSDataAccess.Tenant.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ESSDataAccess.DbContext
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        private readonly int? tenantId;

        public AppDbContext(DbContextOptions<AppDbContext> options, ITenant tenantGetter) : base(options)
        {
            tenantId = tenantGetter.TenantId;
        }

        public DbSet<AddonSettingModel> AddonSettings { get; set; }

        public DbSet<InvoiceModel> Invoices { get; set; }

        //Logs

        public DbSet<GeofenceVerificationLogModel> GeofenceVerificationLogs { get; set; }

        public DbSet<IPVerificationLogModel> IPVerificationLogs { get; set; }

        public DbSet<QrVerificationLogModel> QrVerificationLogs { get; set; }

        public DbSet<DynamicQrVerificationLogModel> DynamicQrVerificationLogs { get; set; }



        //Site
        public DbSet<SiteModel> Sites { get; set; }

        public DbSet<ContractModel> Contracts { get; set; }


        public DbSet<EmployeeSettingsModel> EmployeeSettings { get; set; }


        //Qr Group

        public DbSet<DynamicQrModel> DynamicQrs { get; set; }

        public DbSet<QrCodeGroupModel> QrCodeGroups { get; set; }

        public DbSet<QrCodeModel> QrCodes { get; set; }


        //IP Groups
        public DbSet<IpGroupModel> IpGroups { get; set; }

        public DbSet<IpAddressModel>? IpAddresses { get; set; }

        //Geofence
        public DbSet<GeofenceGroupModel> GeofenceGroups { get; set; }

        public DbSet<GeofenceModel>? Geofences { get; set; }

        //Collection
        public DbSet<PaymentCollectionModel> PaymentCollections { get; set; }

        public DbSet<LoanRequestModel> LoanRequests { get; set; }

        public DbSet<SalesTargetModel> SalesTargets { get; set; }

        //Product
        public DbSet<ProductModel> Products { get; set; }

        public DbSet<ProductCategoryModel> ProductCategories { get; set; }

        public DbSet<ProductOrderModel> ProductOrders { get; set; }

        public DbSet<OrderLineModel> OrderLines { get; set; }

        //Task
        public DbSet<TaskModel> Tasks { get; set; }

        public DbSet<TaskUpdateModel> TaskUpdates { get; set; }

        //Notice
        public DbSet<NoticeModel> Notices { get; set; }

        public DbSet<UserNoticeModel> UserNotices { get; set; }

        public DbSet<TeamNoticeModel> TeamNotices { get; set; }

        //Document
        public DbSet<DocumentTypeModel> DocumentTypes { get; set; }

        public DbSet<DocumentRequestModel> DocumentRequests { get; set; }

        //Form
        public DbSet<FormModel>? Forms { get; set; }

        public DbSet<FormFieldModel>? FormFields { get; set; }

        public DbSet<FormEntryModel>? FormEntries { get; set; }

        public DbSet<FormAssignmentModel>? FormAssignments { get; set; }

        public DbSet<FormEntryFieldModel> FormEntryFields { get; set; }

        //tenants
        public DbSet<PushTokenModel>? PushTokens { get; set; }

        public DbSet<ESSDataAccess.Models.OrderModel> Orders { get; set; }

        public DbSet<TenantModel> Tenants { get; set; }

        public DbSet<PlanModel> Plans { get; set; }

        public DbSet<TenantSubscriptionModel> TenantSubscriptions { get; set; }

        //Settings
        public DbSet<SettingsModel> AppSettings { get; set; }

        public DbSet<SASettingsModel> SASettings { get; set; }

        //Expenses
        public DbSet<ExpenseTypeModel> ExpenseTypes { get; set; }
        public DbSet<ExpenseRequestModel> ExpenseRequests { get; set; }

        //Attedance
        public DbSet<AttendanceModel>? Attendances { get; set; }

        public DbSet<TrackingModel>? Trackings { get; set; }

        public DbSet<BreakModel> Breaks { get; set; }

        public DbSet<CheckInOutModel> CheckInOuts { get; set; }

        public DbSet<VisitModel>? Visits { get; set; }

        //Leave
        public DbSet<LeaveTypeModel>? LeaveTypes { get; set; }
        public DbSet<LeaveRequestModel>? LeaveRequests { get; set; }

        //Notifications
        public DbSet<NotificationModel>? Notifications { get; set; }

        //Schedule & Client
        public DbSet<ScheduleModel>? Schedules { get; set; }
        public DbSet<ClientModel>? Clients { get; set; }

        //Account
        public DbSet<ResetPasswordModel>? ResetPassword { get; set; }

        //Chats
        public DbSet<TeamModel> Teams { get; set; }

        public DbSet<ChatStatusModel> ChatStatus { get; set; }

        public DbSet<ChatModel> Chats { get; set; }

        //Device
        public DbSet<UserDeviceModel>? UserDevices { get; set; }

        //Other
        public DbSet<HolidayModel> Holidays { get; set; }

        public DbSet<AuditLogModel>? AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FormModel>()
               .HasMany(x => x.FormFields)
               .WithOne(x => x.Form)
               .HasForeignKey(x => x.FormId)
               .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<FormFieldModel>()
                .HasOne(x => x.Form)
                .WithMany(x => x.FormFields)
                .HasForeignKey(x => x.FormId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<FormEntryModel>()
                .HasOne(x => x.Form)
                .WithMany(x => x.FormEntries)
                .HasForeignKey(x => x.FormId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FormEntryModel>()
                .HasOne(x => x.User)
                .WithMany(x => x.FormEntries)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SettingsModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<ExpenseTypeModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<ExpenseRequestModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<DocumentTypeModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<DocumentRequestModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<AttendanceModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<TrackingModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<VisitModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<LeaveTypeModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<LeaveRequestModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<NotificationModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<ScheduleModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<ClientModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<ResetPasswordModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<TeamModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<ChatStatusModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<ChatModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<UserDeviceModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<HolidayModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<GeofenceVerificationLogModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<IPVerificationLogModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<QrVerificationLogModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<DynamicQrVerificationLogModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<SiteModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<ContractModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<EmployeeSettingsModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<DynamicQrModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<QrCodeGroupModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<QrCodeModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<IpGroupModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<IpAddressModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<GeofenceGroupModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<GeofenceModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<PaymentCollectionModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<LoanRequestModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<SalesTargetModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<ProductModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<ProductCategoryModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<ProductOrderModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<OrderLineModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<TaskModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<TaskUpdateModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<NoticeModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<UserNoticeModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<TeamNoticeModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<FormModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<FormFieldModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<FormEntryModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<FormAssignmentModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<FormEntryFieldModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<PushTokenModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<BreakModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            builder.Entity<CheckInOutModel>()
                .HasQueryFilter(x => x.TenantId == tenantId);


            builder.Entity<TenantSubscriptionModel>()
                .HasOne(x => x.Plan)
                .WithMany(x => x.TenantSubscriptions)
                .OnDelete(DeleteBehavior.Restrict);



            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseModel && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (tenantId.HasValue && tenantId != 0)
                {
                    ((BaseModel)entityEntry.Entity).TenantId = tenantId.Value;
                }

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseModel)entityEntry.Entity).CreatedAt = DateTime.Now;
                    ((BaseModel)entityEntry.Entity).UpdatedAt = DateTime.Now;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    ((BaseModel)entityEntry.Entity).UpdatedAt = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {


            var entries = ChangeTracker
           .Entries()
           .Where(e => e.Entity is BaseModel && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (tenantId.HasValue && tenantId != 0)
                {
                    ((BaseModel)entityEntry.Entity).TenantId = tenantId.Value;
                }

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseModel)entityEntry.Entity).CreatedAt = DateTime.Now;
                    ((BaseModel)entityEntry.Entity).UpdatedAt = DateTime.Now;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    ((BaseModel)entityEntry.Entity).UpdatedAt = DateTime.Now;
                }
            }


            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
