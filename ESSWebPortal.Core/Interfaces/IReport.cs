namespace ESSWebPortal.Core.Interfaces
{
    public interface IReport
    {

        Task<string> GenerateAttendanceReport(string filePath, string type, DateTime monthYear, List<string> ids, bool isAll = false);

        Task<string> GenerateTimeLineReport(string filePath, DateTime monthYear);

        Task<string> GenerateVisitReport(string filePath, string type, DateTime monthYear, List<string> ids, bool isAll = false);
    }
}
