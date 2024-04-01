using ESSDataAccess.Models.Base;

namespace ESSDataAccess.Models
{
    public class HolidayModel : BaseModel
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }
    }
}
