using ESSDataAccess;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSDataAccess.Models.Addons;
using ESSDataAccess.Models.Document;
using ESSDataAccess.Models.Form;
using ESSDataAccess.Models.Notice;
using ESSDataAccess.Models.Product;
using ESSDataAccess.Tenant;
using ESSDataAccess.Tenant.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EssWebPortal
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITenant _tenant;
        private readonly RoleManager<AppRole> _roleManager;

        private string Version = "3.0.0";

        public DataSeeder(AppDbContext db,
            UserManager<AppUser> userManager,
            ITenant tenant,
            RoleManager<AppRole> roleManager)
        {
            _context = db;
            _userManager = userManager;
            _tenant = tenant;
            _roleManager = roleManager;
        }


        public async Task SeedDataToDb()
        {
            _tenant.ClearTenant();

            if (!await _context.SASettings.AnyAsync())
            {
                await SeedSASettings();
            }

            if (!await _context.AddonSettings.AnyAsync())
            {
                await SeedAddonSettings();
            }

            await SeedPlans(true);

            if (!await _context.Users.AnyAsync() && !await _context.Tenants.AnyAsync())
            {
                await SeedSuperAdmin();
                await SeedTenant1();
                //await SeedTenant2();
            }

        }

        public async Task SeedLiveDataToDb()
        {
            _tenant.ClearTenant();

            if (!await _context.SASettings.AnyAsync())
            {
                await SeedSASettings();
            }

            if (!await _context.AddonSettings.AnyAsync())
            {
                await SeedNoAddonSettings();
            }

            await SeedPlans(false);

            if (!await _context.Users.AnyAsync() && !await _context.Tenants.AnyAsync())
            {
                await SeedSuperAdmin();
                await SeedTenant1();
            }
        }

        public async Task SeedTenant1()
        {
            var plan = await _context.Plans.FirstOrDefaultAsync(x => x.Name == "Platinum");

            var tenant = new TenantModel
            {
                Name = "tenant1",
                NormalizedName = "TENANT1",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                SubscriptionStatus = SubscriptionStatusEnum.Active,
                PlanId = plan.Id,
                Description = "demo"
            };

            await _context.AddAsync(tenant);
            await _context.SaveChangesAsync();

            //Admin
            AppUser user = new()
            {
                UserName = "1tenantadmin",
                NormalizedUserName = "1TENANTADMIN",
                FirstName = "1Tenant",
                LastName = "Admin",
                Designation = "Tenant Admin",
                PhoneNumber = "1234567890",
                TenantId = tenant.Id,
                CreatedAt = DateTime.Now,
                PhoneNumberConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, "123456");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            await CreateOrder(tenant.Id, user.Id);

            //Manager
            AppUser manager = new()
            {
                UserName = "1tenantmanager",
                NormalizedUserName = "1TENANTMANAGER",
                FirstName = "1 Tenant",
                LastName = "Manager",
                Designation = "System Admin",
                PhoneNumber = "11122344567",
                TenantId = tenant.Id,
                CreatedAt = DateTime.Now,
                PhoneNumberConfirmed = true
            };

            var manager_result = await _userManager.CreateAsync(manager, "123456");
            if (manager_result.Succeeded)
            {
                await _userManager.AddToRoleAsync(manager, UserRoles.Manager);
            }


            await SeedTeamsData(tenant.Id);
            await SeedSettings(tenant.Id);
            await SeedClientsAndScheduleData(tenant.Id);
            await SeedUsersData(tenant.Id);
            await SeedLeaveTypes(tenant.Id);
            await SeedNoticeBoard(tenant.Id);
            await SeedIpGroup(tenant.Id);
            await SeedGeolocationGroup(tenant.Id);
            await SeedForms(tenant.Id);
            await SeedProducts(tenant.Id);
            await SeedDocumentTypes(tenant.Id);

            await SeedExpenseTypes(tenant.Id);
        }


        public async Task SeedTenant2()
        {
            var plan = await _context.Plans.FirstOrDefaultAsync(x => x.Name == "Platinum");

            var tenant = new TenantModel
            {
                Name = "tenant2",
                NormalizedName = "TENANT2",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                SubscriptionStatus = SubscriptionStatusEnum.Active,
                PlanId = plan.Id,
                Description = "demo"
            };

            await _context.AddAsync(tenant);
            await _context.SaveChangesAsync();


            //Admin
            AppUser user = new()
            {
                UserName = "2tenantadmin",
                NormalizedUserName = "2TENANTADMIN",
                FirstName = "2Tenant",
                LastName = "Admin",
                Designation = "Tenant Admin",
                PhoneNumber = "1234567899",
                TenantId = tenant.Id,
                CreatedAt = DateTime.Now,
                PhoneNumberConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, "123456");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            await CreateOrder(tenant.Id, user.Id);

            //Manager
            AppUser manager = new()
            {
                UserName = "2tenantmanager",
                NormalizedUserName = "2TENANTMANAGER",
                FirstName = "2 Tenant",
                LastName = "Manager",
                Designation = "System Admin",
                PhoneNumber = "11122345567",
                TenantId = tenant.Id,
                CreatedAt = DateTime.Now,
                PhoneNumberConfirmed = true
            };

            var manager_result = await _userManager.CreateAsync(manager, "123456");
            if (manager_result.Succeeded)
            {
                await _userManager.AddToRoleAsync(manager, UserRoles.Manager);
            }


            await SeedTeamsData(tenant.Id);
            await SeedSettings(tenant.Id);
            await SeedClientsAndScheduleData(tenant.Id);
            await SeedUsersData(tenant.Id);
            await SeedLeaveTypes(tenant.Id);
            await SeedNoticeBoard(tenant.Id);
            await SeedIpGroup(tenant.Id);
            await SeedGeolocationGroup(tenant.Id);
            await SeedForms(tenant.Id);
            await SeedProducts(tenant.Id);
            await SeedDocumentTypes(tenant.Id);
            await SeedExpenseTypes(tenant.Id);
        }



        public async Task SeedSuperAdmin()
        {
            if (!await _context.Roles.AnyAsync())
            {
                await SeedRolesData();
            }


            if (!await _context.Users.AnyAsync(x => x.UserName == "superadmin"))
            {
                AppUser user = new()
                {
                    UserName = "superadmin",
                    NormalizedUserName = "SUPERADMIN",
                    FirstName = "Super",
                    LastName = "Admin",
                    Designation = "Super Admin",
                    PhoneNumber = "1111111111",
                    CreatedAt = DateTime.Now,
                    PhoneNumberConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "123456");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.SuperAdmin);
                }
            }
        }

        public async Task SeedSettings(int tenantId)
        {
            var now = DateTime.Now;
            SettingsModel settings = new SettingsModel()
            {
                Country = "USA",
                PhoneCountryCode = "+91",
                Currency = "USD",
                CurrencySymbol = "$",
                DistanceUnit = "KM",
                CreatedAt = now,
                UpdatedAt = now,
                TenantId = tenantId,
                CenterLatitude = "18.418983770139405",
                CenterLongitude = "49.67194361588897",
                MapZoomLevel = 3,
                OrderPrefix = tenantId + "FM-ORD",
                EmployeeCodePrefix = tenantId + "FM-EMP",
            };

            await _context.AddAsync(settings);
            await _context.SaveChangesAsync();

        }


        public async Task SeedAddonSettings()
        {
            var now = DateTime.Now;
            AddonSettingModel addon = new AddonSettingModel()
            {
                IsBreakModuleEnabled = true,
                IsUidLoginModuleEnabled = true,
                IsChatModuleEnabled = true,
                IsClientVisitModuleEnabled = true,
                IsDocumentModuleEnabled = true,
                IsExpenseModuleEnabled = true,
                IsLeaveModuleEnabled = true,
                IsGeofenceModuleEnabled = true,
                IsIpBasedAttendanceModuleEnabled = true,
                IsLoanModuleEnabled = true,
                IsQrCodeAttendanceModuleEnabled = true,
                IsNoticeModuleEnabled = true,
                IsDynamicFormModuleEnabled = true,
                IsDynamicQrCodeAttendanceEnabled = true,
                IsDataImportExportModuleEnabled = true,
                IsOfflineTrackingModuleEnabled = true,
                IsPaymentCollectionModuleEnabled = true,
                IsProductModuleEnabled = true,
                IsSiteModuleEnabled = true,
                IsTaskModuleEnabled = true,

                IsSalesTargetModuleEnabled = false,
                IsAiChatModuleEnabled = false,
                CreatedAt = now,
                UpdatedAt = now
            };

            await _context.AddAsync(addon);
            await _context.SaveChangesAsync();
        }

        public async Task SeedNoAddonSettings()
        {
            var now = DateTime.Now;
            AddonSettingModel addon = new AddonSettingModel()
            {
                IsChatModuleEnabled = true,
                IsExpenseModuleEnabled = true,
                IsClientVisitModuleEnabled = true,
                IsLeaveModuleEnabled = true,

                IsBreakModuleEnabled = false,
                IsUidLoginModuleEnabled = false,
                IsDocumentModuleEnabled = false,
                IsGeofenceModuleEnabled = false,
                IsIpBasedAttendanceModuleEnabled = false,
                IsLoanModuleEnabled = false,
                IsQrCodeAttendanceModuleEnabled = false,
                IsNoticeModuleEnabled = false,
                IsDynamicFormModuleEnabled = false,
                IsDynamicQrCodeAttendanceEnabled = false,
                IsDataImportExportModuleEnabled = false,
                IsOfflineTrackingModuleEnabled = false,
                IsPaymentCollectionModuleEnabled = false,
                IsProductModuleEnabled = false,
                IsSiteModuleEnabled = false,
                IsTaskModuleEnabled = false,

                IsSalesTargetModuleEnabled = false,
                IsAiChatModuleEnabled = false,
                CreatedAt = now,
                UpdatedAt = now
            };

            await _context.AddAsync(addon);
            await _context.SaveChangesAsync();
        }

        public async Task SeedSASettings()
        {
            var now = DateTime.Now;
            SASettingsModel saSettings = new SASettingsModel()
            {
                //Basic Settings
                AppName = "CZ Field Manager SaaS",
                AppVersion = Version,
                Country = "USA",
                PhoneCountryCode = "+91",
                Currency = "USD",
                CurrencySymbol = "$",

                //AppSettings
                MAppVersion = Version,
                MLocationUpdateIntervalType = "s",
                MLocationUpdateInterval = 5,
                MPrivacyPolicyLink = "https://privacypolicy.link",
                ApiBaseUrl = "http://192.168.0.190:44317/",
                WebBaseUrl = "http://192.168.0.190:44316/",

                //Paypal
                PaypalMode = PaypalModeEnum.Sandbox,
                PaypalClientId = "123456789",
                PaypalClientSecret = "Secret",

                //Dashbaord
                ExpenseDocumentPath = string.Empty,
                LeaveDocumentPath = string.Empty,
                PaySlipPath = string.Empty,

                //SMS
                TwilioAccountSid = string.Empty,
                TwilioAuthToken = string.Empty,
                TwilioFromNumber = string.Empty,
                TwilioStatus = false,

                //Dashboard
                OfflineCheckTime = 15,
                OfflineCheckTimeType = "minutes",

                //Payment
                DefaultPaymentGateway = PaymentGateway.Paypal,

                //Razorpay
                RazorPayKeyId = string.Empty,
                RazorPayKeySecret = string.Empty,

                BillingCycleDate = 1,
                DueDays = 7,
                OverDueDays = 3,

                InvoicePrefix = "FM-INV",

                //Addons
                IsAiChatModuleEnabled = false,
                IsBiometricVerificationRequired = true,
                IsBreakModuleEnabled = true,
                IsChatModuleEnabled = true,
                IsClientVisitModuleEnabled = true,
                IsDocumentModuleEnabled = true,
                IsDataImportExportModuleEnabled = false,
                IsSalesTargetModuleEnabled = true,
                IsSiteModuleEnabled = true,
                IsDynamicFormModuleEnabled = true,
                IsExpenseModuleEnabled = true,
                IsDynamicQrCodeAttendanceEnabled = true,
                IsGeofenceModuleEnabled = true,
                IsIpBasedAttendanceModuleEnabled = true,
                IsLeaveModuleEnabled = true,
                IsLoanModuleEnabled = true,
                IsNoticeModuleEnabled = true,
                IsOfflineTrackingModuleEnabled = true,
                IsPaymentCollectionModuleEnabled = true,
                IsProductModuleEnabled = true,
                IsQrCodeAttendanceModuleEnabled = true,
                IsTaskModuleEnabled = true,
                IsUidLoginModuleEnabled = true,
                CreatedAt = now,
                UpdatedAt = now,
            };

            await _context.SASettings.AddAsync(saSettings);
            await _context.SaveChangesAsync();

        }

        public async Task SeedExpenseTypes(int tenantId)
        {
            var now = DateTime.Now;

            List<ExpenseTypeModel> expenseTypes = new List<ExpenseTypeModel>
            {
                new ExpenseTypeModel
                {
                    Name = "Food",
                    IsImgRequired = true,
                    CreatedAt = now,
                    UpdatedAt = now,
                    TenantId = tenantId,
                },

                new ExpenseTypeModel
                {
                    Name = "Other",
                    IsImgRequired = false,
                    CreatedAt = now,
                    UpdatedAt = now,
                    TenantId = tenantId
                }
            };

            await _context.AddRangeAsync(expenseTypes);
            await _context.SaveChangesAsync();
        }

        public async Task SeedLeaveTypes(int tenantId)
        {
            var now = DateTime.Now;

            List<LeaveTypeModel> leaveTypes = new List<LeaveTypeModel>
            {
               new LeaveTypeModel {
                   Name = "General Leave",
                   CreatedAt = now,
                   UpdatedAt = now,
                   TenantId = tenantId,
                   IsImgRequired = false
                },
                new LeaveTypeModel{
                   Name = "Sick Leave",
                   CreatedAt = now,
                   UpdatedAt = now,
                   TenantId = tenantId,
                   IsImgRequired = true
                }
            };

            await _context.AddRangeAsync(leaveTypes);
            await _context.SaveChangesAsync();
        }



        public async Task SeedUsersData(int tenantId)
        {

            var team = await _context.Teams
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.TenantId == tenantId);

            var schedule = await _context.Schedules
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.TenantId == tenantId);

            var supervisors = await _userManager
                .GetUsersInRoleAsync(UserRoles.Manager);

            var supervisor = supervisors
                .Where(x => x.TenantId == tenantId)
                .First();

            AppUser employee = new()
            {
                UserName = "employee" + tenantId,
                NormalizedUserName = "EMPLOYEE" + tenantId,
                FirstName = "Ravi",
                LastName = "Chandran",
                PhoneNumber = tenantId % 2 == 0 ? "0987654321" : "1234567890",
                Designation = "Field Executive",
                PhoneNumberConfirmed = true,
                ParentId = supervisor.Id,
                TeamId = team.Id,
                ScheduleId = schedule.Id,
                PrimarySalesTarget = 1000,
                SecondarySalesTarget = 1000,
                AvailableLeaveCount = 30,
                AttendanceType = EmployeeAttendanceTypeEnum.None,
                TenantId = tenantId,
            };

            var employeeresult = await _userManager.CreateAsync(employee, "123456");
            if (employeeresult.Succeeded)
            {
                await _userManager.AddToRoleAsync(employee, UserRoles.Employee);
            }
        }


        public async Task CreateOrder(int tenantId, int userId)
        {
            var plan = await _context.Plans
                .FirstOrDefaultAsync(x => x.Name == "Diamond");

            var tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.Id == tenantId);

            Random random = new Random();
            var orderModel = new OrderModel
            {
                PlanId = plan.Id,
                TenantId = tenantId,
                CreatedBy = userId,
                PerEmployeeAmount = plan.Price,
                OrderId = random.Next(00000, 99999).ToString(),
                Description = "An order for " + "Demo user " + $"{tenantId}" + " of plan " + plan.Name + " " + plan.Price.ToString(),
                Status = SAOrderStatus.Success,
                PaymentGateway = PaymentGateway.Manual,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await _context.AddAsync(orderModel);
            await _context.SaveChangesAsync();

            tenant.PlanId = plan.Id;
            tenant.AvailableEmployeesCount = 1000;
            tenant.TotalEmployeesCount = 1000;
            tenant.SubscriptionStatus = SubscriptionStatusEnum.Active;
            tenant.UpdatedAt = DateTime.Now;
            _context.Tenants.Update(tenant);
            await _context.SaveChangesAsync();

            TenantSubscriptionModel subscription = new TenantSubscriptionModel
            {
                PlanId = plan.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StartDate = DateTime.Now,
                OrderId = orderModel.Id,
                IsPromotional = plan.IsPromotional,
                CreatedBy = tenant.Id,
                TenantId = tenant.Id,
                EndDate = DateTime.Now.AddYears(2),
            };

            await _context.AddAsync(subscription);
            await _context.SaveChangesAsync();
        }


        public async Task SeedClientsAndScheduleData(int tenantId)
        {
            if (!await _context.Clients.IgnoreQueryFilters()!.AnyAsync(x => x.TenantId == tenantId))
            {
                ClientModel client = new()
                {
                    Name = tenantId + "Test Solutions",
                    Address = "India",
                    Latitude = Convert.ToDecimal("23.613541"),
                    Longitude = Convert.ToDecimal("58.594109"),
                    PhoneNumber = "1234567890",
                    ContactPerson = "Abijith",
                    Email = tenantId + "test@gmail.com",
                    City = "Chennai",
                    Remarks = "test remarks",
                    Status = ClientStatus.Active,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    TenantId = tenantId,
                    Radius = 100

                };
                await _context.Clients.AddAsync(client);
                await _context.SaveChangesAsync();

                //create Schedule for client

                if (!await _context.Schedules!.IgnoreQueryFilters().AnyAsync(x => x.TenantId == tenantId))
                {
                    var now = DateTime.Now;
                    var startTime = "09:00";
                    var endTime = "18:00";

                    string startDateTime = now.AddDays(-2).ToString("yyyy-MM-dd ") + startTime;
                    string endDateTime = now.AddDays(15).ToString("yyyy-MM-dd ") + endTime;

                    ScheduleModel schedule = new()
                    {
                        Title = "Test Schedule",
                        Description = "Test schedule description",
                        StartTime = DateTime.ParseExact(startDateTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        EndTime = DateTime.ParseExact(endDateTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Status = ScheduleStatus.Active,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        TenantId = tenantId,
                        Sunday = false,
                        Monday = true,
                        Tuesday = true,
                        Wednesday = true,
                        Thursday = true,
                        Friday = true,
                        Saturday = true
                    };
                    await _context.Schedules.AddAsync(schedule);
                    await _context.SaveChangesAsync();

                }

            }
        }


        public async Task SeedPlans(bool isDemo = true)
        {
            if (await _context.Plans.AnyAsync())
                return;


            var now = DateTime.Now;
            List<PlanModel> plans = new List<PlanModel>
            {
                new PlanModel {
                    Name = "Silver",
                    Price = 199,
                    Description = "Silver Plan",
                    CreatedAt = now,
                    UpdatedAt = now,
                    Type = PlanType.Monthly,
                    //Addons
                    IsBreakModuleEnabled = isDemo,
                    IsUidLoginModuleEnabled = isDemo,
                    IsChatModuleEnabled = true,
                    IsClientVisitModuleEnabled = true,
                    IsDocumentModuleEnabled = isDemo,
                    IsExpenseModuleEnabled = true,
                    IsLeaveModuleEnabled = true,

                },
                new PlanModel {
                    Name = "Gold",
                    Price = 499,
                    Description = "Gold Plan",
                    CreatedAt = now,
                    UpdatedAt = now,
                    Type = PlanType.Monthly,
                    //Addons
                    IsBreakModuleEnabled = isDemo,
                    IsUidLoginModuleEnabled = isDemo,
                    IsChatModuleEnabled = true,
                    IsClientVisitModuleEnabled = true,
                    IsDocumentModuleEnabled = isDemo,
                    IsExpenseModuleEnabled = true,
                    IsLeaveModuleEnabled = true,
                    IsGeofenceModuleEnabled = isDemo,
                    IsIpBasedAttendanceModuleEnabled = isDemo,
                    IsLoanModuleEnabled = isDemo,
                    IsQrCodeAttendanceModuleEnabled = isDemo,
                    IsNoticeModuleEnabled = isDemo,
                },
                new PlanModel {
                    Name = "Platinum",
                    Price = 999,
                    Description = "Platinum Plan",
                    CreatedAt = now,
                    UpdatedAt = now,
                    Type = PlanType.Monthly,
                     //Addons
                    IsBreakModuleEnabled = isDemo,
                    IsUidLoginModuleEnabled = isDemo,
                    IsChatModuleEnabled = true,
                    IsClientVisitModuleEnabled = true,
                    IsDocumentModuleEnabled = isDemo,
                    IsExpenseModuleEnabled = true,
                    IsLeaveModuleEnabled = true,
                    IsGeofenceModuleEnabled = isDemo,
                    IsIpBasedAttendanceModuleEnabled = isDemo,
                    IsLoanModuleEnabled = isDemo,
                    IsQrCodeAttendanceModuleEnabled = isDemo,
                    IsNoticeModuleEnabled = isDemo,
                    IsDynamicFormModuleEnabled = isDemo,
                    IsDynamicQrCodeAttendanceEnabled = isDemo,
                },
                new PlanModel {
                    Name = "Diamond",
                    Price = 1999,
                    Description = "Diamond Plan",
                    CreatedAt = now,
                    UpdatedAt = now,
                    Type = PlanType.Monthly,
                     //Addons
                    IsBreakModuleEnabled = isDemo,
                    IsUidLoginModuleEnabled = isDemo,
                    IsChatModuleEnabled = true,
                    IsClientVisitModuleEnabled = true,
                    IsDocumentModuleEnabled = isDemo,
                    IsExpenseModuleEnabled = true,
                    IsLeaveModuleEnabled = true,
                    IsGeofenceModuleEnabled = isDemo,
                    IsIpBasedAttendanceModuleEnabled = isDemo,
                    IsLoanModuleEnabled = isDemo,
                    IsQrCodeAttendanceModuleEnabled = isDemo,
                    IsNoticeModuleEnabled = isDemo,
                    IsDynamicFormModuleEnabled = isDemo,
                    IsDynamicQrCodeAttendanceEnabled = isDemo,
                    IsDataImportExportModuleEnabled = isDemo,
                    IsOfflineTrackingModuleEnabled  = isDemo,
                    IsPaymentCollectionModuleEnabled = isDemo,
                    IsProductModuleEnabled = isDemo,
                    IsSiteModuleEnabled = isDemo,
                    IsTaskModuleEnabled = isDemo,
                },

            };

            await _context.AddRangeAsync(plans);
            await _context.SaveChangesAsync();
        }

        public async Task SeedRolesData()
        {
            //Create Role
            if (!await _context.Roles.AnyAsync())
            {
                List<AppRole> roles = new()
                {
                    new AppRole
                    {
                        Name = UserRoles.SuperAdmin,
                        NormalizedName = UserRoles.SuperAdmin.ToUpper(),
                    },
                    new AppRole
                    {
                        Name = UserRoles.Admin,
                        NormalizedName = UserRoles.Admin.ToUpper(),
                    },
                    new AppRole
                    {
                        Name = UserRoles.Accounts,
                        NormalizedName = UserRoles.Accounts.ToUpper(),
                    },
                    new AppRole
                    {
                        Name = UserRoles.Employee,
                        NormalizedName = UserRoles.Employee.ToUpper(),
                    },
                    new AppRole
                    {
                        Name = UserRoles.Manager,
                        NormalizedName = UserRoles.Manager.ToUpper(),
                    },
                    new AppRole
                    {
                        Name = UserRoles.HR,
                        NormalizedName = UserRoles.HR.ToUpper(),
                    }
                };
                foreach (var role in roles)
                {
                    await _roleManager.CreateAsync(role);
                }
            }
        }

        public async Task SeedTeamsData(int tenantId)
        {
            var team = new TeamModel
            {
                Name = "Default Team " + tenantId,
                Description = "Default Team Group " + tenantId,
                Status = TeamStatus.Active,
                TenantId = tenantId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _context.AddAsync(team);

            await _context.SaveChangesAsync();
        }

        public async Task SeedNoticeBoard(int tenantId)
        {
            List<NoticeModel> noticeModels = new List<NoticeModel>();

            noticeModels.Add(new NoticeModel
            {
                Title = "Notice 1",
                Description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English.",
                Status = CommonStatus.Active,
                TenantId = tenantId,
                NoticeFor = NoticeForEnum.All,
            });

            noticeModels.Add(new NoticeModel
            {
                Title = "Notice 2",
                Description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.",
                Status = CommonStatus.Active,
                TenantId = tenantId,
                NoticeFor = NoticeForEnum.All,
            });

            noticeModels.Add(new NoticeModel
            {
                Title = "Notice 3",
                Description = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                Status = CommonStatus.Active,
                TenantId = tenantId,
                NoticeFor = NoticeForEnum.All,
            });

            noticeModels.Add(new NoticeModel
            {
                Title = "Notice 4",
                Description = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of \"de Finibus Bonorum et Malorum\" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, \"Lorem ipsum dolor sit amet..\", comes from a line in section 1.10.32.",
                Status = CommonStatus.Active,
                TenantId = tenantId,
                NoticeFor = NoticeForEnum.All,
            });

            noticeModels.Add(new NoticeModel
            {
                Title = "Notice 5",
                Description = "The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.",
                Status = CommonStatus.Active,
                TenantId = tenantId,
                NoticeFor = NoticeForEnum.All,
            });

            await _context.AddRangeAsync(noticeModels);

            await _context.SaveChangesAsync();
        }

        public async Task SeedIpGroup(int tenantId)
        {
            IpGroupModel ipGroup = new IpGroupModel
            {
                Name = "Test IP Group",
                Description = "Test Group Description",
                TenantId = tenantId,
                Status = CommonStatus.Active,
            };

            await _context.AddAsync(ipGroup);
            await _context.SaveChangesAsync();

            IpAddressModel ipAddress = new IpAddressModel
            {
                Name = "Test Ip 1",
                Description = "Test Ip 1 Description",
                IpAddress = "192.168.1.110",
                IpGroupId = ipGroup.Id,
                TenantId = tenantId,
                IsEnabled = true,
            };

            await _context.AddAsync(ipAddress);
            await _context.SaveChangesAsync();

            IpAddressModel ipAddress2 = new IpAddressModel
            {
                Name = "Test Ip 2",
                Description = "Test Ip 2 Description",
                IpAddress = "192.168.29.157",
                IpGroupId = ipGroup.Id,
                TenantId = tenantId,
                IsEnabled = true,
            };

            await _context.AddAsync(ipAddress2);
            await _context.SaveChangesAsync();
        }

        public async Task SeedGeolocationGroup(int tenantId)
        {
            GeofenceGroupModel geofence = new GeofenceGroupModel
            {
                Name = "Test Geofence Group",
                Description = "Test Group Description",
                TenantId = tenantId,
                Status = CommonStatus.Active,
            };

            await _context.AddAsync(geofence);
            await _context.SaveChangesAsync();

            GeofenceModel geofence1 = new GeofenceModel
            {
                Name = "Test Geofence 1",
                Description = "Test Geofence 1 Description",
                Latitude = Convert.ToDecimal("23.613541"),
                Longitude = Convert.ToDecimal("58.594109"),
                Radius = 100,
                TenantId = tenantId,
                GeofenceGroupId = geofence.Id,
                IsEnabled = true,
            };


            await _context.AddAsync(geofence1);
            await _context.SaveChangesAsync();

            GeofenceModel geofence2 = new GeofenceModel
            {
                Name = "Test Geofence 2",
                Description = "Test Geofence 2 Description",
                Latitude = Convert.ToDecimal("23.613541"),
                Longitude = Convert.ToDecimal("58.594109"),
                Radius = 100,
                TenantId = tenantId,
                GeofenceGroupId = geofence.Id,
                IsEnabled = true,
            };

            await _context.AddAsync(geofence2);
            await _context.SaveChangesAsync();
        }

        public async Task SeedForms(int tenantId)
        {
            try
            {
                if (!await _context.Forms.AnyAsync())
                {
                    var form = new FormModel
                    {
                        Name = "Interest Survey",
                        Status = CommonStatus.Active,
                        Description = "Interest Survey Form",
                        TenantId = tenantId,
                        IsClientRequired = true,
                    };

                    await _context.AddAsync(form);
                    await _context.SaveChangesAsync();

                    List<FormFieldModel> formFields =
                    [
                        new FormFieldModel
                          {
                              Label = "Name",
                              Placeholder = "Enter your name",
                              IsRequired = true,
                              FormId = form.Id,
                               TenantId = tenantId,
                              FieldType = FormFieldType.Text,
                          },
                          new FormFieldModel
                          {
                              Label = "Email",
                              Placeholder = "Enter your email",
                              IsRequired = true,
                              FormId = form.Id,
                               TenantId = tenantId,
                              FieldType = FormFieldType.Email,
                          },
                          new FormFieldModel
                          {
                              Label = "Phone",
                              Placeholder = "Enter your phone",
                              IsRequired = true,
                              FormId = form.Id,
                               TenantId = tenantId,
                              FieldType = FormFieldType.Number,
                          },
                          new FormFieldModel
                          {
                              Label = "Area of interest",
                              Values = "Web Development,Mobile Development,Designing",
                              Placeholder = "Select your area of interest",
                              IsRequired = true,
                              FormId = form.Id,
                               TenantId = tenantId,
                              FieldType = FormFieldType.MultiSelect,
                          },
                          new  FormFieldModel
                          {
                              Label = "Message",
                              Placeholder = "Enter your message",
                              IsRequired = true,
                              FormId = form.Id,
                               TenantId = tenantId,
                              FieldType = FormFieldType.Text,
                          },
                          new  FormFieldModel
                          {
                              Label = "Currently Working",
                              Placeholder = "Enter your current working company",
                              IsRequired = true,
                              FormId = form.Id,
                               TenantId = tenantId,
                              FieldType = FormFieldType.Boolean,
                          },
                      ];

                    await _context.AddRangeAsync(formFields);
                    await _context.SaveChangesAsync();

                    var employee1 = await _context.Users
                   .FirstOrDefaultAsync(x => x.UserName == "employee1");
                    /*
                                        var employee2 = await _context.Users
                                            .FirstOrDefaultAsync(x => x.UserName == "employee2");
                    */
                    List<FormAssignmentModel> formAssignments = new List<FormAssignmentModel>
                    {
                         new FormAssignmentModel
                         {
                             FormId = form.Id,
                             UserId = employee1.Id,
                         },
                      /*   new FormAssignmentModel
                         {
                             FormId = form.Id,
                             UserId = employee2.Id,
                         }*/
                     };

                    await _context.AddRangeAsync(formAssignments);
                    await _context.SaveChangesAsync();
                }


            }
            catch (Exception ex)
            {
            }

        }

        public async Task SeedProducts(int tenantId)
        {
            try
            {
                if (!await _context.ProductCategories.AnyAsync())
                {
                    List<ProductCategoryModel> productCategories =
                    [
                        new ProductCategoryModel
                        {
                            Name = "Samsung",
                            Description = "Category 1 Description",
                            TenantId = tenantId,
                            Status = CommonStatus.Active,
                        },
                        new ProductCategoryModel
                        {
                            Name = "Iphone",
                            Description = "Category 1 Description",
                              TenantId = tenantId,
                            Status = CommonStatus.Active,
                        },
                    ];


                    await _context.AddRangeAsync(productCategories);
                    await _context.SaveChangesAsync();

                    List<ProductCategoryModel> subCategories =
                    [
                         new ProductCategoryModel
                    {
                        Name = "S21 Series",
                        Description = "Sub Category 1 Description",
                        Status = CommonStatus.Active,
                          TenantId = tenantId,
                        ParentId = productCategories[0].Id
                    },
                    new ProductCategoryModel
                    {
                        Name = "S20 Series",
                        Description = "Sub Category 1 Description",
                        Status = CommonStatus.Active,
                          TenantId = tenantId,
                        ParentId = productCategories[0].Id
                    },
                    new ProductCategoryModel
                    {
                        Name = "S23 Series",
                        Description = "Sub Category 1 Description",
                        Status = CommonStatus.Active,
                          TenantId = tenantId,
                        ParentId = productCategories[0].Id
                    },
                    new ProductCategoryModel
                    {
                        Name = "S22 Series",
                        Description = "Sub Category 1 Description",
                        Status = CommonStatus.Active,
                          TenantId = tenantId,
                        ParentId = productCategories[0].Id
                    },
                ];

                    await _context.AddRangeAsync(subCategories);
                    await _context.SaveChangesAsync();

                    List<ProductCategoryModel> subCategories2 =
                    [
                        new ProductCategoryModel
                    {
                        Name = "11 Series",
                        Description = "Sub Category 1 Description",
                        Status = CommonStatus.Active,
                          TenantId = tenantId,
                        ParentId = productCategories[1].Id
                    },
                    new ProductCategoryModel
                    {
                        Name = "12 Series",
                        Description = "Sub Category 1 Description",
                        Status = CommonStatus.Active,
                          TenantId = tenantId,
                        ParentId = productCategories[1].Id
                    },
                    new ProductCategoryModel
                    {
                        Name = "13 Series",
                        Description = "Sub Category 1 Description",
                        Status = CommonStatus.Active,
                          TenantId = tenantId,
                        ParentId = productCategories[1].Id
                    },
                    new ProductCategoryModel
                    {
                        Name = "14 Series",
                        Description = "Sub Category 1 Description",
                        Status = CommonStatus.Active,
                          TenantId = tenantId,
                        ParentId = productCategories[1].Id
                    },
                    new ProductCategoryModel
                    {
                        Name = "15 Series",
                        Description = "Sub Category 1 Description",
                        Status = CommonStatus.Active,
                          TenantId = tenantId,
                        ParentId = productCategories[1].Id
                    },
                ];

                    await _context.AddRangeAsync(subCategories2);
                    await _context.SaveChangesAsync();
                }

                if (!await _context.Products.AnyAsync())
                {
                    var s21 = await _context.ProductCategories
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(x => x.Name == "S21 Series");

                    var s20 = await _context.ProductCategories
                         .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(x => x.Name == "S20 Series");

                    var s23 = await _context.ProductCategories
                         .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(x => x.Name == "S23 Series");

                    var iphone12 = await _context.ProductCategories
                         .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(x => x.Name == "12 Series");

                    var iphone13 = await _context.ProductCategories
                         .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(x => x.Name == "13 Series");

                    var iphone14 = await _context.ProductCategories
                         .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(x => x.Name == "14 Series");

                    var iphone15 = await _context.ProductCategories
                         .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(x => x.Name == "15 Series");

                    var products = new List<ProductModel>
                {
                    new ProductModel
                    {
                        Name = "S21 FE 5G",
                        Description = "Product 1 Description",
                        ProductCategoryId = s21.Id,
                        ProductCode = "S21FE5G",
                        BasePrice = 700,
                          TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },
                    new ProductModel
                    {
                        Name = "S21 Plus",
                        Description = "Product 2 Description",
                        ProductCategoryId = s21.Id,
                        ProductCode = "S21Plus",
                        BasePrice = 900,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },
                    new ProductModel
                    {
                        Name = "iphone 12 128GB",
                        Description = "Product 3 Description",
                        ProductCategoryId = iphone12.Id,
                        ProductCode = "iphone12",
                        BasePrice = 300,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },
                    new ProductModel
                    {
                        Name = "iphone 12 Pro 128GB",
                        Description = "Product 4 Description",
                        ProductCategoryId = iphone12.Id,
                        ProductCode = "iphone12Pro",
                        BasePrice = 400,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },
                    new ProductModel
                    {
                         Name = "iphone 12 Pro max 128GB",
                        Description = "Product 5 Description",
                        ProductCategoryId = iphone12.Id,
                        ProductCode = "iphone12ProMax",
                        BasePrice = 500,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "S20 FE 5G",
                        Description = "Product 6 Description",
                        ProductCategoryId = s20.Id,
                        ProductCode = "S20FE5G",
                        BasePrice = 600,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "S20 Plus",
                        Description = "Product 7 Description",
                        ProductCategoryId = s20.Id,
                        ProductCode = "S20Plus",
                        BasePrice = 800,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "S23 FE 5G",
                        Description = "Product 8 Description",
                        ProductCategoryId = s23.Id,
                        ProductCode = "S23FE5G",
                        BasePrice = 900,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "S23 Plus",
                        Description = "Product 9 Description",
                        ProductCategoryId = s23.Id,
                        ProductCode = "S23Plus",
                        BasePrice = 1000,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "S23 Ultra",
                        Description = "Product 10 Description",
                        ProductCategoryId = s23.Id,
                        ProductCode = "S23Ultra",
                        BasePrice = 1100,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "S22 FE 5G GPS WIFI DATA 12GB RAM 256GB STORAGE DEMO",
                        Description = "Product 11 Description",
                        ProductCategoryId = s23.Id,
                        ProductCode = "S22FE5G",
                        BasePrice = 1200,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "Iphone 13 128GB",
                        Description = "Product 13 Description",
                        ProductCategoryId = iphone13.Id,
                        ProductCode = "Iphone13",
                        BasePrice = 1300,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "Iphone 13 Pro 128GB",
                        Description = "Product 14 Description",
                        ProductCategoryId = iphone13.Id,
                        ProductCode = "Iphone13Pro",
                        BasePrice = 1400,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "Iphone 13 Pro max 128GB",
                        Description = "Product 15 Description",
                        ProductCategoryId = iphone13.Id,
                        ProductCode = "Iphone13ProMax",
                        BasePrice = 1500,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "Iphone 14 128GB",
                        Description = "Product 16 Description",
                        ProductCategoryId = iphone14.Id,
                        ProductCode = "Iphone14",
                        BasePrice = 1600,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "Iphone 14 Pro 128GB",
                        Description = "Product 17 Description",
                        ProductCategoryId = iphone14.Id,
                        ProductCode = "Iphone14Pro",
                        BasePrice = 1700,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "Iphone 14 Pro max 128GB",
                        Description = "Product 18 Description",
                        ProductCategoryId = iphone14.Id,
                        ProductCode = "Iphone14ProMax",
                        BasePrice = 1800,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "Iphone 15 128GB",
                        Description = "Product 19 Description",
                        ProductCategoryId = iphone15.Id,
                        ProductCode = "Iphone15",
                        BasePrice = 1900,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "Iphone 15 Pro 128GB",
                        Description = "Product 20 Description",
                        ProductCategoryId = iphone15.Id,
                        ProductCode = "Iphone15Pro",
                        BasePrice = 2000,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "Iphone 15 Pro max 128GB",
                        Description = "Product 21 Description",
                        ProductCategoryId = iphone15.Id,
                        ProductCode = "Iphone15ProMax",
                        BasePrice = 2100,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "Iphone 15 Pro max 256GB",
                        Description = "Product 22 Description",
                        ProductCategoryId = iphone15.Id,
                        ProductCode = "Iphone15ProMax",
                        BasePrice = 2200,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "Iphone 15 Pro max 512GB",
                        Description = "Product 23 Description",
                        ProductCategoryId = iphone15.Id,
                        ProductCode = "Iphone15ProMax",
                        BasePrice = 2300,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "Iphone 15 Pro max 1TB",
                        Description = "Product 24 Description",
                        ProductCategoryId = iphone15.Id,
                        ProductCode = "Iphone15ProMax",
                        BasePrice = 2400,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "Iphone 15 Pro max 2TB",
                        Description = "Product 25 Description",
                        ProductCategoryId = iphone15.Id,
                        ProductCode = "Iphone15ProMax",
                        BasePrice = 2500,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "Iphone 15 Pro max 4TB",
                        Description = "Product 26 Description",
                        ProductCategoryId = iphone15.Id,
                        ProductCode = "Iphone15ProMax",
                        BasePrice = 2600,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },

                    new ProductModel
                    {
                        Name = "Iphone 15 Pro max 8TB",
                        Description = "Product 27 Description",
                        ProductCategoryId = iphone15.Id,
                        ProductCode = "Iphone15ProMax",
                        BasePrice = 2700,
                        TenantId = tenantId,
                        Status = CommonStatus.Active,
                    },
                };

                    await _context.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task SeedDocumentTypes(int tenantId)
        {
            List<DocumentTypeModel> documentTypes = new List<DocumentTypeModel>
            {
                new DocumentTypeModel
                {
                    Name = "NOC",
                    Status = CommonStatus.Active,
                    TenantId = tenantId,
                },
                new DocumentTypeModel
                {
                    Name = "Salary Slip",
                    Status = CommonStatus.Active,
                    TenantId = tenantId,
                },
            };

            await _context.AddRangeAsync(documentTypes);
            await _context.SaveChangesAsync();
        }
    }
}
