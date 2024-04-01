using ESSDataAccess.Enum;
using ESSDataAccess.Models.Base;
using ESSDataAccess.Models.Logs;
using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models
{
    public class GeofenceGroupModel : BaseModel
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public CommonStatus Status { get; set; } = CommonStatus.Active;

        public List<GeofenceModel>? Geofences { get; set; }

        public List<GeofenceVerificationLogModel>? GeofenceVerificationLogs { get; set; }
    }
}
