using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class GeofenceModel : BaseModel
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal Longitude { get; set; }

        public int Radius { get; set; }

        public bool IsEnabled { get; set; } = true;

        [ForeignKey(nameof(GeofenceGroup))]
        public int GeofenceGroupId { get; set; }

        public GeofenceGroupModel? GeofenceGroup { get; set; }
    }
}
