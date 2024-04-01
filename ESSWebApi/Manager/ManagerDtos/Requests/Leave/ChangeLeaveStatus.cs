namespace ESSWebApi.Manager.ManagerDtos.Requests.Leave
{
    public class ChangeLeaveStatus
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }
    }
}
