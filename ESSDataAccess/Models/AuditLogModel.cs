using ESSDataAccess.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class AuditLogModel
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int? UserId { get; set; }

        public AppUser? User { get; set; }

        public string EntityName { get; set; } = "Unknown";

        public string Action { get; set; } = "Unknown";

        public DateTime Timestamp { get; set; }

        public string Changes { get; set; } = "Unknown";
    }
}
