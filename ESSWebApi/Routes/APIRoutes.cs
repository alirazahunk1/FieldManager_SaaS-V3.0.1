namespace ESSWebApi.Routes
{
    public class APIRoutes
    {
        public const string Prefix = "api";

        public const string Version = "v1";

        public const string Base = Prefix + "/" + Version + "/";


        public partial class Settings
        {
            private const string NewBase = Base + "settings/";

            public const string GetAppSettings = NewBase + "getAppSettings";

            public const string GetModuleSettings = NewBase + "getModuleSettings";
        }

        public partial class Chat
        {
            private const string NewBase = Base + "chat/";

            public const string PostChat = NewBase + "postChat";

            public const string PostImageChat = NewBase + "postImageChat";

            public const string GetChats = NewBase + "getChats";

            public const string GetFileChat = NewBase + "getFileChat";
        }


        public partial class Visit
        {
            private const string NewBase = Base + "visit/";

            public const string Create = NewBase + "create";

            public const string Delete = NewBase + "delete";

            public const string GetAll = NewBase + "getAll";

            public const string GetHistory = NewBase + "getHistory";

            public const string GetVisitsCount = NewBase + "getVisitsCount";
        }

        public partial class SignBoard
        {
            private const string NewBase = Base + "signBoard/";

            public const string CreateRequest = NewBase + "createRequest";

            public const string DeleteRequest = NewBase + "deleteRequest";

            public const string GetRequests = NewBase + "getRequests";
        }

        public partial class Leave
        {
            public const string GetLeaveTypes = Base + "getLeaveTypes";
            public const string CreateLeaveRequest = Base + "createLeaveRequest";
            public const string UploadLeaveDocument = Base + "uploadLeaveDocument";
            public const string GetLeaveRequests = Base + "getLeaveRequests";
            public const string DeleteLeaveRequest = Base + "deleteLeaveRequest";
        }

        public partial class Expense
        {
            private const string NewBase = Base + "expense/";

            public const string GetExpenseTypes = NewBase + "getExpenseTypes";
            public const string CreateExpenseRequest = NewBase + "createExpenseRequest";
            public const string UploadExpenseDocument = NewBase + "uploadExpenseDocument";
            public const string GetExpenseRequests = NewBase + "getExpenseRequests";
            public const string DeleteExpenseRequest = NewBase + "deleteExpenseRequest";
        }

        public partial class Client
        {
            public const string NewBase = Base + "client/";

            public const string GetAll = NewBase + "getAllClients";
            public const string GetById = NewBase + "getClientById";
            public const string Create = NewBase + "create";
            public const string Update = NewBase + "update";
            public const string Delete = NewBase + "delete";
            public const string Search = NewBase + "search";
        }

        public class Auth
        {
            public const string Login = Base + "login";
            public const string ForgotPassword = Base + "forgotPassword";
            public const string CheckUsername = Base + "checkUsername";
            public const string CheckPhoneNumber = Base + "checkPhoneNumber";
            public const string VerifyOtp = Base + "verifyOtp";
            public const string ResetPassword = Base + "resetPassword";
            public const string ChangePassword = Base + "changePassword";
            public const string UploadUserProfile = Base + "uploadUserProfile";


        }

        public static class Attendance
        {
            private const string NewBase = Base + "attendance/";

            public const string StatusChecking = NewBase + "checkStatus";

            public const string StatusUpdate = NewBase + "statusUpdate";

            public const string GetAttendanceHistory = NewBase + "getAttendanceHistory";

            public const string CheckInOut = NewBase + "checkInOut";

            public const string GetSchedule = NewBase + "getSchedule";

            public const string PostProof = NewBase + "postProof";

            public const string StartStopBreak = NewBase + "startStopBreak";

            public const string CanCheckOut = NewBase + "canCheckOut";

            public const string SetEarlyCheckoutReason = NewBase + "setEarlyCheckoutReason";
        }

        public static class User
        {
            public const string GetDashboardData = Base + "getDashboardData";
        }


        public static class Device
        {
            public const string RegisterDevice = Base + "registerDevice";
            public const string checkDevice = Base + "checkDevice";
            public const string UpdateDeviceStatus = Base + "updateDeviceStatus";
            public const string MessagingToken = Base + "messagingToken";
        }
        public class Seeder
        {
            public const string AddCustomData = Base + "addData";
            public const string RemoveCustomData = Base + "removeData";

        }
        public class Notification
        {
            public const string AddNotification = Base + "addNotification";
            public const string SetasRead = Base + "setasRead";
            public const string GetNotifications = Base + "getNotifications";
            public const string GetNotificationDetails = Base + "getNotificationDetails";


        }
    }
}
