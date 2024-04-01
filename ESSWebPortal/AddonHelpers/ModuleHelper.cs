using System.Reflection;

namespace ESSWebPortal.AddonHelpers
{
    public static class ModuleHelper
    {
        public static bool CheckModuleExists(ModuleEnum module)
        {
            var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();

            var assemblyName = GetAssemblyName(module);

            if (assemblies.Any(x => x.Name == assemblyName))
            {
                return true;
            }

            return false;
        }

        public static bool CheckApiModuleExists(List<ApiModule> apiModules, ApiModuleEnum module)
        {
            var assemblyName = GetAssemblyName(module);

            if (apiModules.Any(x => x.Name == assemblyName))
            {
                return true;
            }

            return false;
        }

        public static ApiModuleStatus GetApiModuleStatus(List<ApiModule> apiModules)
        {
            var status = new ApiModuleStatus();

            status.IsDocumentRequestModuleExists = CheckApiModuleExists(apiModules, ApiModuleEnum.DocumentRequest);
            status.IsProductOrderModuleExists = CheckApiModuleExists(apiModules, ApiModuleEnum.ProductOrder);
            status.IsDynamicFormsExists = CheckApiModuleExists(apiModules, ApiModuleEnum.DynamicForms);
            status.IsNoticeBoardExists = CheckApiModuleExists(apiModules, ApiModuleEnum.NoticeBoard);
            status.IsPaymentCollectionExists = CheckApiModuleExists(apiModules, ApiModuleEnum.PaymentCollection);
            status.IsLoanRequestExists = CheckApiModuleExists(apiModules, ApiModuleEnum.LoanRequest);
            status.IsBreakExists = CheckApiModuleExists(apiModules, ApiModuleEnum.Break);
            status.IsIpAddressExists = CheckApiModuleExists(apiModules, ApiModuleEnum.IpAddressAttendance);
            status.IsGeofenceExists = CheckApiModuleExists(apiModules, ApiModuleEnum.Geofence);
            status.IsQrAttendanceExists = CheckApiModuleExists(apiModules, ApiModuleEnum.QrCodeAttendance);
            status.IsDynamicQrExists = CheckApiModuleExists(apiModules, ApiModuleEnum.DynamicQrAttendance);
            status.IsTaskExists = CheckApiModuleExists(apiModules, ApiModuleEnum.TaskSystem);
            return status;
        }

        public static string GetModuleVersionString(ModuleEnum module)
        {
            var assemblyName = GetAssemblyName(module);
            if (string.IsNullOrEmpty(assemblyName))
            {
                return "";
            }

            var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            var assembly = assemblies.FirstOrDefault(x => x.Name == assemblyName);
            if (assembly == null)
            {
                return "";
            }

            return "V" + assembly.Version.ToString();
        }

        public static string GetApiModuleVersionString(List<ApiModule> apiModules, ApiModuleEnum apiModule)
        {
            var assemblyName = GetAssemblyName(apiModule);
            if (string.IsNullOrEmpty(assemblyName))
            {
                return "";
            }

            var assembly = apiModules.FirstOrDefault(x => x.Name == assemblyName);
            if (assembly == null)
            {
                return "";
            }

            return "V" + assembly.Version.ToString();
        }

        public static string GetModuleVersion(ModuleEnum module)
        {
            var assemblyName = GetAssemblyName(module);
            if (string.IsNullOrEmpty(assemblyName))
            {
                return "";
            }

            var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            var assembly = assemblies.FirstOrDefault(x => x.Name == assemblyName);
            if (assembly == null)
            {
                return "";
            }

            return "Version " + assembly.Version.ToString();
        }

        private static string GetAssemblyName(ModuleEnum module)
        {
            string name = "";
            switch (module)
            {
                case ModuleEnum.DocumentRequest:
                    name = "CZ.Web.DocumentRequest";
                    break;
                case ModuleEnum.DynamicForms:
                    name = "CZ.Web.DynamicForm";
                    break;
                case ModuleEnum.LoanRequest:
                    name = "CZ.Web.Loan";
                    break;
                case ModuleEnum.ProductOrder:
                    name = "CZ.Web.ProductOrder";
                    break;
                case ModuleEnum.Geofence:
                    name = "CZ.Web.Geofence";
                    break;
                case ModuleEnum.IpAddressAttendance:
                    name = "CZ.Web.IpAddress";
                    break;
                case ModuleEnum.QrCodeAttendance:
                    name = "CZ.Web.QrAttendance";
                    break;
                case ModuleEnum.TaskSystem:
                    name = "CZ.Web.TaskSystem";
                    break;
                case ModuleEnum.NoticeBoard:
                    name = "CZ.Web.NoticeBoard";
                    break;
                case ModuleEnum.PaymentCollection:
                    name = "CZ.Web.PaymentCollection";
                    break;
                case ModuleEnum.AiChat:
                    name = "CZ.Web.AiChat";
                    break;
                case ModuleEnum.OfflineTracking:
                    name = "CZ.Web.OfflineTracking";
                    break;
                case ModuleEnum.UidLogin:
                    name = "CZ.Web.UidLogin";
                    break;
                case ModuleEnum.DataImportExport:
                    name = "CZ.Web.DataImportExport";
                    break;
                case ModuleEnum.Site:
                    name = "CZ.Web.Site";
                    break;
                case ModuleEnum.Break:
                    name = "CZ.Web.Break";
                    break;
                case ModuleEnum.DynamicQrAttendance:
                    name = "CZ.Web.DynamicQr";
                    break;
                default:
                    return "";
            }

            return name;
        }

        private static string GetAssemblyName(ApiModuleEnum module)
        {
            string name = "";
            switch (module)
            {
                case ApiModuleEnum.DocumentRequest:
                    name = "CZ.Api.DocumentRequest";
                    break;
                case ApiModuleEnum.DynamicForms:
                    name = "CZ.Api.DynamicForm";
                    break;
                case ApiModuleEnum.LoanRequest:
                    name = "CZ.Api.Loan";
                    break;
                case ApiModuleEnum.ProductOrder:
                    name = "CZ.Api.ProductOrder";
                    break;
                case ApiModuleEnum.Geofence:
                    name = "CZ.Api.Geofence";
                    break;
                case ApiModuleEnum.IpAddressAttendance:
                    name = "CZ.Api.IpAddress";
                    break;
                case ApiModuleEnum.QrCodeAttendance:
                    name = "CZ.Api.QrAttendance";
                    break;
                case ApiModuleEnum.TaskSystem:
                    name = "CZ.Api.TaskSystem";
                    break;
                case ApiModuleEnum.NoticeBoard:
                    name = "CZ.Api.NoticeBoard";
                    break;
                case ApiModuleEnum.PaymentCollection:
                    name = "CZ.Api.PaymentCollection";
                    break;
                case ApiModuleEnum.OfflineTracking:
                    name = "CZ.Api.OfflineTracking";
                    break;
                case ApiModuleEnum.UidLogin:
                    name = "CZ.Api.UidLogin";
                    break;
                case ApiModuleEnum.Site:
                    break;
                case ApiModuleEnum.Break:
                    name = "CZ.Api.Break";
                    break;
                case ApiModuleEnum.DynamicQrAttendance:
                    name = "CZ.Api.DynamicQr";
                    break;
                default:
                    return "";
            }

            return name;
        }

    }

    public class ApiModuleStatus
    {
        public bool IsDocumentRequestModuleExists { get; set; }

        public bool IsProductOrderModuleExists { get; set; }

        public bool IsDynamicFormsExists { get; set; }

        public bool IsNoticeBoardExists { get; set; }

        public bool IsPaymentCollectionExists { get; set; }

        public bool IsLoanRequestExists { get; set; }

        public bool IsBreakExists { get; set; }

        public bool IsIpAddressExists { get; set; }

        public bool IsGeofenceExists { get; set; }

        public bool IsQrAttendanceExists { get; set; }

        public bool IsDynamicQrExists { get; set; }

        public bool IsTaskExists { get; set; }
    }


    public enum ModuleEnum
    {
        DocumentRequest,
        DynamicForms,
        LoanRequest,
        ProductOrder,
        Geofence,
        IpAddressAttendance,
        QrCodeAttendance,
        TaskSystem,
        NoticeBoard,
        PaymentCollection,
        AiChat,
        OfflineTracking,
        UidLogin,
        DataImportExport,
        Site,
        Break,
        DynamicQrAttendance
    }

    public enum ApiModuleEnum
    {
        DocumentRequest,
        DynamicForms,
        LoanRequest,
        ProductOrder,
        Geofence,
        IpAddressAttendance,
        QrCodeAttendance,
        TaskSystem,
        NoticeBoard,
        PaymentCollection,
        OfflineTracking,
        UidLogin,
        Site,
        Break,
        DynamicQrAttendance
    }

}
