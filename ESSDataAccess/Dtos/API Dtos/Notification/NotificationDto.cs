using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSDataAccess.Dtos.API_Dtos.Notification
{
    public class NotificationDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public bool IsViewed { get; set; }
        public int userId { get; set; }
        public string CreatedOn { get; set; }
    }
}
