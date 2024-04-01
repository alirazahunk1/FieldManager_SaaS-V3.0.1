using ESSDataAccess.DbContext;
using ESSDataAccess.Tenant.Models;
using ESSWebPortal.Core.ViewModel.Plan;
using Microsoft.EntityFrameworkCore;

namespace ESSWebPortal.Core.Plan
{
    public class PlanService : IPlan
    {
        private readonly AppDbContext _context;

        public PlanService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlanViewModel>> GetAll()
        {
            return await _context.Plans
                .Where(x => x.Status == PlanStatus.Active)
                 .Select(x => new PlanViewModel
                 {
                     Id = x.Id,
                     Description = x.Description,
                     Name = x.Name,
                     Price = x.Price,
                     Type = x.Type,
                     IsAiChatModuleEnabled = x.IsAiChatModuleEnabled,
                     IsBiometricVerificationModuleEnabled = x.IsBiometricVerificationModuleEnabled,
                     IsBreakModuleEnabled = x.IsBreakModuleEnabled,
                     IsChatModuleEnabled = x.IsChatModuleEnabled,
                     IsClientVisitModuleEnabled = x.IsClientVisitModuleEnabled,
                     IsDataImportExportModuleEnabled = x.IsDataImportExportModuleEnabled,
                     IsDocumentModuleEnabled = x.IsDocumentModuleEnabled,
                     IsDynamicFormModuleEnabled = x.IsDynamicFormModuleEnabled,
                     IsDynamicQrCodeAttendanceEnabled = x.IsDynamicQrCodeAttendanceEnabled,
                     IsExpenseModuleEnabled = x.IsExpenseModuleEnabled,
                     IsGeofenceModuleEnabled = x.IsGeofenceModuleEnabled,
                     IsIpBasedAttendanceModuleEnabled = x.IsIpBasedAttendanceModuleEnabled,
                     IsLeaveModuleEnabled = x.IsLeaveModuleEnabled,
                     IsLoanModuleEnabled = x.IsLoanModuleEnabled,
                     IsNoticeModuleEnabled = x.IsNoticeModuleEnabled,
                     IsOfflineTrackingModuleEnabled = x.IsOfflineTrackingModuleEnabled,
                     IsPaymentCollectionModuleEnabled = x.IsPaymentCollectionModuleEnabled,
                     IsProductModuleEnabled = x.IsProductModuleEnabled,
                     IsQrCodeAttendanceModuleEnabled = x.IsQrCodeAttendanceModuleEnabled,
                     IsSalesTargetModuleEnabled = x.IsSalesTargetModuleEnabled,
                     IsSiteModuleEnabled = x.IsSiteModuleEnabled,
                     IsTaskModuleEnabled = x.IsTaskModuleEnabled,
                     IsUidLoginModuleEnabled = x.IsUidLoginModuleEnabled,
                     CreatedAt = x.CreatedAt,
                     UpdatedAt = x.UpdatedAt,
                 }).AsNoTracking()
                 .ToListAsync();
        }

        public async Task<bool> CreatePlan(PlanCreateUpdateVM vm)
        {
            PlanModel planModel = new PlanModel
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                Type = vm.Type,
                IsPromotional = vm.IsPromotional,
                IsAiChatModuleEnabled = vm.IsAiChatModuleEnabled,
                IsBreakModuleEnabled = vm.IsBreakModuleEnabled,
                IsChatModuleEnabled = vm.IsChatModuleEnabled,
                IsClientVisitModuleEnabled = vm.IsClientVisitModuleEnabled,
                IsDataImportExportModuleEnabled = vm.IsDataImportExportModuleEnabled,
                IsDocumentModuleEnabled = vm.IsDocumentModuleEnabled,
                IsDynamicFormModuleEnabled = vm.IsDynamicFormModuleEnabled,
                IsDynamicQrCodeAttendanceEnabled = vm.IsDynamicQrCodeAttendanceEnabled,
                IsExpenseModuleEnabled = vm.IsExpenseModuleEnabled,
                IsGeofenceModuleEnabled = vm.IsGeofenceModuleEnabled,
                IsIpBasedAttendanceModuleEnabled = vm.IsIpBasedAttendanceModuleEnabled,
                IsLeaveModuleEnabled = vm.IsLeaveModuleEnabled,
                IsLoanModuleEnabled = vm.IsLoanModuleEnabled,
                IsNoticeModuleEnabled = vm.IsNoticeModuleEnabled,
                IsOfflineTrackingModuleEnabled = vm.IsOfflineTrackingModuleEnabled,
                IsPaymentCollectionModuleEnabled = vm.IsPaymentCollectionModuleEnabled,
                IsProductModuleEnabled = vm.IsProductModuleEnabled,
                IsQrCodeAttendanceModuleEnabled = vm.IsQrCodeAttendanceModuleEnabled,
                IsSalesTargetModuleEnabled = vm.IsSalesTargetModuleEnabled,
                IsSiteModuleEnabled = vm.IsSiteModuleEnabled,
                IsTaskModuleEnabled = vm.IsTaskModuleEnabled,
                IsUidLoginModuleEnabled = vm.IsUidLoginModuleEnabled,
                IsBiometricVerificationModuleEnabled = vm.IsBiometricVerificationModuleEnabled,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = PlanStatus.Active
            };

            await _context.Plans.AddAsync(planModel);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeletePlan(int id)
        {
            var plan = await _context.Plans.FindAsync(id);

            if (plan != null)
            {
                _context.Remove(plan);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> EditPlan(PlanCreateUpdateVM vm)
        {
            var plan = await _context.Plans.FirstOrDefaultAsync(x => x.Id == vm.Id);

            if (plan == null) return false;

            if (plan.Name != vm.Name) plan.Name = vm.Name;
            if (plan.Description != vm.Description) plan.Description = vm.Description;
            if (plan.Price != vm.Price) plan.Price = vm.Price;
            if (plan.Type != vm.Type) plan.Type = vm.Type;
            if (plan.IsPromotional != vm.IsPromotional) plan.IsPromotional = vm.IsPromotional;

            if (plan.IsAiChatModuleEnabled != vm.IsAiChatModuleEnabled)
                plan.IsAiChatModuleEnabled = vm.IsAiChatModuleEnabled;

            if (plan.IsBreakModuleEnabled != vm.IsBreakModuleEnabled)
                plan.IsBreakModuleEnabled = vm.IsBreakModuleEnabled;

            if (plan.IsChatModuleEnabled != vm.IsChatModuleEnabled)
                plan.IsChatModuleEnabled = vm.IsChatModuleEnabled;

            if (plan.IsClientVisitModuleEnabled != vm.IsClientVisitModuleEnabled)
                plan.IsClientVisitModuleEnabled = vm.IsClientVisitModuleEnabled;

            if (plan.IsDataImportExportModuleEnabled != vm.IsDataImportExportModuleEnabled)
                plan.IsDataImportExportModuleEnabled = vm.IsDataImportExportModuleEnabled;

            if (plan.IsDocumentModuleEnabled != vm.IsDocumentModuleEnabled)
                plan.IsDocumentModuleEnabled = vm.IsDocumentModuleEnabled;

            if (plan.IsDynamicFormModuleEnabled != vm.IsDynamicFormModuleEnabled)
                plan.IsDynamicFormModuleEnabled = vm.IsDynamicFormModuleEnabled;

            if (plan.IsDynamicQrCodeAttendanceEnabled != vm.IsDynamicQrCodeAttendanceEnabled)
                plan.IsDynamicQrCodeAttendanceEnabled = vm.IsDynamicQrCodeAttendanceEnabled;

            if (plan.IsExpenseModuleEnabled != vm.IsExpenseModuleEnabled)
                plan.IsExpenseModuleEnabled = vm.IsExpenseModuleEnabled;

            if (plan.IsGeofenceModuleEnabled != vm.IsGeofenceModuleEnabled)
                plan.IsGeofenceModuleEnabled = vm.IsGeofenceModuleEnabled;

            if (plan.IsIpBasedAttendanceModuleEnabled != vm.IsIpBasedAttendanceModuleEnabled)
                plan.IsIpBasedAttendanceModuleEnabled = vm.IsIpBasedAttendanceModuleEnabled;

            if (plan.IsLeaveModuleEnabled != vm.IsLeaveModuleEnabled)
                plan.IsLeaveModuleEnabled = vm.IsLeaveModuleEnabled;

            if (plan.IsLoanModuleEnabled != vm.IsLoanModuleEnabled)
                plan.IsLoanModuleEnabled = vm.IsLoanModuleEnabled;

            if (plan.IsNoticeModuleEnabled != vm.IsNoticeModuleEnabled)
                plan.IsNoticeModuleEnabled = vm.IsNoticeModuleEnabled;

            if (plan.IsOfflineTrackingModuleEnabled != vm.IsOfflineTrackingModuleEnabled)
                plan.IsOfflineTrackingModuleEnabled = vm.IsOfflineTrackingModuleEnabled;

            if (plan.IsPaymentCollectionModuleEnabled != vm.IsPaymentCollectionModuleEnabled)
                plan.IsPaymentCollectionModuleEnabled = vm.IsPaymentCollectionModuleEnabled;

            if (plan.IsProductModuleEnabled != vm.IsProductModuleEnabled)
                plan.IsProductModuleEnabled = vm.IsProductModuleEnabled;

            if (plan.IsQrCodeAttendanceModuleEnabled != vm.IsQrCodeAttendanceModuleEnabled)
                plan.IsQrCodeAttendanceModuleEnabled = vm.IsQrCodeAttendanceModuleEnabled;

            if (plan.IsSalesTargetModuleEnabled != vm.IsSalesTargetModuleEnabled)
                plan.IsSalesTargetModuleEnabled = vm.IsSalesTargetModuleEnabled;

            if (plan.IsSiteModuleEnabled != vm.IsSiteModuleEnabled)
                plan.IsSiteModuleEnabled = vm.IsSiteModuleEnabled;

            if (plan.IsTaskModuleEnabled != vm.IsTaskModuleEnabled)
                plan.IsTaskModuleEnabled = vm.IsTaskModuleEnabled;

            if (plan.IsUidLoginModuleEnabled != vm.IsUidLoginModuleEnabled)
                plan.IsUidLoginModuleEnabled = vm.IsUidLoginModuleEnabled;

            if (plan.IsBiometricVerificationModuleEnabled != vm.IsBiometricVerificationModuleEnabled)
                plan.IsBiometricVerificationModuleEnabled = vm.IsBiometricVerificationModuleEnabled;

            plan.UpdatedAt = DateTime.Now;

            _context.Plans.Update(plan);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task ChangeStatus(int id)
        {
            var plan = await _context.Plans.FindAsync(id);
            if (plan != null)
            {
                plan.Status = plan.Status == PlanStatus.Active ? PlanStatus.InActive : PlanStatus.Active;

                _context.Update(plan);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PlanViewModel> GetById(int id)
        {
            return await _context.Plans
               .Where(x => x.Id == id)
                .Select(x => new PlanViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    Price = x.Price,
                    Type = x.Type,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    IsAiChatModuleEnabled = x.IsAiChatModuleEnabled,
                    IsBiometricVerificationModuleEnabled = x.IsBiometricVerificationModuleEnabled,
                    IsUidLoginModuleEnabled = x.IsUidLoginModuleEnabled,
                    IsBreakModuleEnabled = x.IsBreakModuleEnabled,
                    IsChatModuleEnabled = x.IsChatModuleEnabled,
                    IsClientVisitModuleEnabled = x.IsClientVisitModuleEnabled,
                    IsDataImportExportModuleEnabled = x.IsDataImportExportModuleEnabled,
                    IsDocumentModuleEnabled = x.IsDocumentModuleEnabled,
                    IsDynamicFormModuleEnabled = x.IsDynamicFormModuleEnabled,
                    IsDynamicQrCodeAttendanceEnabled = x.IsDynamicQrCodeAttendanceEnabled,
                    IsExpenseModuleEnabled = x.IsExpenseModuleEnabled,
                    IsGeofenceModuleEnabled = x.IsGeofenceModuleEnabled,
                    IsIpBasedAttendanceModuleEnabled = x.IsIpBasedAttendanceModuleEnabled,
                    IsLeaveModuleEnabled = x.IsLeaveModuleEnabled,
                    IsLoanModuleEnabled = x.IsLoanModuleEnabled,
                    IsNoticeModuleEnabled = x.IsNoticeModuleEnabled,
                    IsOfflineTrackingModuleEnabled = x.IsOfflineTrackingModuleEnabled,
                    IsPaymentCollectionModuleEnabled = x.IsPaymentCollectionModuleEnabled,
                    IsProductModuleEnabled = x.IsProductModuleEnabled,
                    IsQrCodeAttendanceModuleEnabled = x.IsQrCodeAttendanceModuleEnabled,
                    IsSalesTargetModuleEnabled = x.IsSalesTargetModuleEnabled,
                    IsSiteModuleEnabled = x.IsSiteModuleEnabled,
                    IsTaskModuleEnabled = x.IsTaskModuleEnabled,
                }).AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetPlanDetailsForUser(string userId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(userId));

            if (user != null && user.TenantId != null)
            {
                if (user.UserName.Equals("1tenantadminuser") || user.UserName.Equals("2tenantadminuser"))
                {
                    return "Demo Plan valid till - " + DateTime.Now.AddMonths(1).ToString("dd/MMM/yyyy");
                }

                var subscription = await _context.TenantSubscriptions
                .Include(x => x.Plan)
                .Include(x => x.Tenant)
                .Where(x => x.TenantId == user.TenantId)
                .OrderByDescending(x => x.EndDate)
                .Select(x => new TenantSubscriptionModel
                {
                    Plan = new PlanModel { Name = x.Plan.Name },
                    EndDate = x.EndDate
                }).AsNoTracking()
                .FirstOrDefaultAsync();

                if (subscription == null) return "";

                return $"{subscription.Plan.Name} (Valid till:{subscription.EndDate.ToString("dd/MMM/yyyy")})";
            }
            else return "";
        }
    }
}
