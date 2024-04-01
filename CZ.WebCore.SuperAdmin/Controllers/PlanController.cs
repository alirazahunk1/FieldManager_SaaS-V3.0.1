using ESSDataAccess;
using ESSDataAccess.DbContext;
using ESSWebPortal.Core.Plan;
using ESSWebPortal.Core.ViewModel.Plan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace CZ.WebCore.SuperAdmin.Controllers
{
    [Authorize(UserRoles.SuperAdmin)]
    public class PlanController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPlan _plan;
        private readonly IToastNotification _toastNotification;

        public PlanController(AppDbContext context,
            IPlan plan,
            IToastNotification toastNotification)
        {
            _context = context;
            _plan = plan;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Plans.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlanCreateUpdateVM vm)
        {
            if (vm == null) return View();
            if (ModelState.IsValid)
            {
                var result = await _plan.CreatePlan(vm);

                if (result)
                {
                    _toastNotification.AddSuccessToastMessage("Plan added successfully");
                    return RedirectToAction("Index");
                }
            }
            _toastNotification.AddErrorToastMessage("Something went wrong");
            return View(vm);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var plan = _context.Plans.FirstOrDefault(x => x.Id == id);

            if (plan == null) return NotFound();

            PlanCreateUpdateVM vm = new PlanCreateUpdateVM
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                CreatedAt = plan.CreatedAt,
                UpdatedAt = plan.UpdatedAt,
                Price = plan.Price,
                Type = plan.Type,
                IsPromotional = plan.IsPromotional,
                IsUidLoginModuleEnabled = plan.IsUidLoginModuleEnabled,
                IsClientVisitModuleEnabled = plan.IsClientVisitModuleEnabled,
                IsOfflineTrackingModuleEnabled = plan.IsOfflineTrackingModuleEnabled,
                IsDataImportExportModuleEnabled = plan.IsDataImportExportModuleEnabled,
                IsSiteModuleEnabled = plan.IsSiteModuleEnabled,
                IsQrCodeAttendanceModuleEnabled = plan.IsQrCodeAttendanceModuleEnabled,
                IsDynamicQrCodeAttendanceEnabled = plan.IsDynamicQrCodeAttendanceEnabled,
                IsBreakModuleEnabled = plan.IsBreakModuleEnabled,
                IsSalesTargetModuleEnabled = plan.IsSalesTargetModuleEnabled,
                IsAiChatModuleEnabled = plan.IsAiChatModuleEnabled,
                IsProductModuleEnabled = plan.IsProductModuleEnabled,
                IsTaskModuleEnabled = plan.IsTaskModuleEnabled,
                IsNoticeModuleEnabled = plan.IsNoticeModuleEnabled,
                IsDynamicFormModuleEnabled = plan.IsDynamicFormModuleEnabled,
                IsExpenseModuleEnabled = plan.IsExpenseModuleEnabled,
                IsLeaveModuleEnabled = plan.IsLeaveModuleEnabled,
                IsDocumentModuleEnabled = plan.IsDocumentModuleEnabled,
                IsChatModuleEnabled = plan.IsChatModuleEnabled,
                IsLoanModuleEnabled = plan.IsLoanModuleEnabled,
                IsPaymentCollectionModuleEnabled = plan.IsPaymentCollectionModuleEnabled,
                IsGeofenceModuleEnabled = plan.IsGeofenceModuleEnabled,
                IsIpBasedAttendanceModuleEnabled = plan.IsIpBasedAttendanceModuleEnabled,
                IsBiometricVerificationModuleEnabled = plan.IsBiometricVerificationModuleEnabled
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PlanCreateUpdateVM vm)
        {
            if (vm == null) return View();

            if (ModelState.IsValid)
            {
                var result = await _plan.EditPlan(vm);

                if (result)
                {
                    _toastNotification.AddSuccessToastMessage("Plan updated successfully");
                    return RedirectToAction("Index");
                }
            }
            _toastNotification.AddErrorToastMessage("Something went wrong");
            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return NotFound();

            var result = await _plan.DeletePlan(id);

            if (result)
            {
                _toastNotification.AddSuccessToastMessage("Plan deleted successfully");
                return RedirectToAction("Index");
            }

            _toastNotification.AddErrorToastMessage("Something went wrong");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await _context.Plans.FindAsync(id));
        }
    }
}
