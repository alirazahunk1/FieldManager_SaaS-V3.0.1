using ESSDataAccess;
using ESSWebPortal.AddonHelpers;
using ESSWebPortal.Core.SuperAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESSWebPortal.Controllers
{
    [Authorize(UserRoles.SuperAdmin)]
    public class AddonController : Controller
    {
        private readonly ISASettings _dbSettings;

        public AddonController(ISASettings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        public async Task<IActionResult> Index()
        {
            var addons = await _dbSettings.GetModuleSettings();
            return View(addons);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeLoanModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            if (!ModuleHelper.CheckModuleExists(ModuleEnum.LoanRequest))
            {
                return BadRequest("Module is not installed");
            }

            var result = await _dbSettings.ChangeLoanModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Loan Module Status Changed");
            }
        }


        [HttpPost]
        public async Task<IActionResult> ChangeExpenseModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            var result = await _dbSettings.ChangeExpenseModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Expense Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeLeaveModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            var result = await _dbSettings.ChangeLeaveModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Leave Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDocumentModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return BadRequest("You cannot save settings in demo mode");
            }

            if (!ModuleHelper.CheckModuleExists(ModuleEnum.DocumentRequest))
            {
                return BadRequest("Module is not installed");
            }

            var result = await _dbSettings.ChangeDocumentModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Document Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeChatModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            var result = await _dbSettings.ChangeChatModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Chat Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProductModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            if (!ModuleHelper.CheckModuleExists(ModuleEnum.ProductOrder))
            {
                return BadRequest("Module is not installed");
            }

            var result = await _dbSettings.ChangeProductModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Product Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeTaskModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            if (!ModuleHelper.CheckModuleExists(ModuleEnum.TaskSystem))
            {
                return BadRequest("Module is not installed");
            }

            var result = await _dbSettings.ChangeTaskModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Task Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeNoticeModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            if (!ModuleHelper.CheckModuleExists(ModuleEnum.NoticeBoard))
            {
                return BadRequest("Module is not installed");
            }

            var result = await _dbSettings.ChangeNoticeModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Notice Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDynamicFormModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            if (!ModuleHelper.CheckModuleExists(ModuleEnum.DynamicForms))
            {
                return BadRequest("Module is not installed");
            }

            var result = await _dbSettings.ChangeFormModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Form Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePaymentCollectionModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            if (!ModuleHelper.CheckModuleExists(ModuleEnum.PaymentCollection))
            {
                return BadRequest("Module is not installed");
            }

            var result = await _dbSettings.ChangePaymentCollectionModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Payment Collection Module Status Changed");
            }
        }


        [HttpPost]
        public async Task<IActionResult> ChangeAiChatModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            if (!ModuleHelper.CheckModuleExists(ModuleEnum.AiChat))
            {
                return BadRequest("Module is not installed");
            }

            var result = await _dbSettings.ChangeAiChatModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("AI Chat Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeOfflineTrackingModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            /*      if (!ModuleHelper.CheckModuleExists(ModuleEnum.OfflineTracking))
                  {
                      return BadRequest("Module is not installed");
                  }*/

            var result = await _dbSettings.ChangeOfflineTrackingModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Client Visit Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeGeofenceTrackingModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            if (!ModuleHelper.CheckModuleExists(ModuleEnum.Geofence))
            {
                return BadRequest("Module is not installed");
            }

            var result = await _dbSettings.ChangeGeofenceModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Client Visit Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeIpBasedAttendanceModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            if (!ModuleHelper.CheckModuleExists(ModuleEnum.IpAddressAttendance))
            {
                return BadRequest("Module is not installed");
            }

            var result = await _dbSettings.ChangeIpBasedAttendanceModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Client Visit Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUidLoginModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            /* if (!ModuleHelper.CheckModuleExists(ModuleEnum.UidLogin))
             {
                 return BadRequest("Module is not installed");
             }*/

            var result = await _dbSettings.ChangeUidLoginModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Client Visit Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDataImportExportModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            if (!ModuleHelper.CheckModuleExists(ModuleEnum.DataImportExport))
            {
                return BadRequest("Module is not installed");
            }

            var result = await _dbSettings.ChangeDataImportExportModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Client Visit Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSiteModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            if (!ModuleHelper.CheckModuleExists(ModuleEnum.Site))
            {
                return BadRequest("Module is not installed");
            }

            var result = await _dbSettings.ChangeSiteModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Client Visit Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeQrCodeAttendanceModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            if (!ModuleHelper.CheckModuleExists(ModuleEnum.QrCodeAttendance))
            {
                return BadRequest("Module is not installed");
            }

            var result = await _dbSettings.ChangeQrCodeAttendanceModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Client Visit Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeBreakModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }


            var result = await _dbSettings.ChangeBreakModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Client Visit Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeClientVisitModuleStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            var result = await _dbSettings.ChangeClientVisitModuleStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Client Visit Module Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDynamicQrCodeAttendanceStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            var result = await _dbSettings.ChangeDynamicQrCodeAttendanceStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Dynamic QR Code Attendance Status Changed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeBiometricVerificationStatus()
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                return Json("You cannot save settings in demo mode");
            }

            var result = await _dbSettings.ChangeBiometricVerificationStatus();

            if (!result)
            {
                return Json("Unable to save the settings");
            }
            else
            {
                return Json("Biometric Verification Status Changed");
            }
        }
    }
}
