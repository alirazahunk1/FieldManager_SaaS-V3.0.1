namespace ESSWebApi.Manager
{
    public static class ManagerRoutes
    {
        private const string Prefix = "api";

        private const string Version = "v1";

        private const string Base = Prefix + "/" + Version + "/" + "manager/";

        public const string GetDashboardData = Base + "getDashboardData";

        public const string GetEmployeesStatus = Base + "getEmployeesStatus";

        public const string GetAllLeaveRequests = Base + "getAllLeaveRequests";

        public const string ChangeLeaveStatus = Base + "changeLeaveStatus";

        public const string GetAllExpenseRequests = Base + "getAllExpenseRequests";

        public const string ChangeExpenseStatus = Base + "changeExpenseStatus";
    }
}
