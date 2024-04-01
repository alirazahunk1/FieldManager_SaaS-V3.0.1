using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Task
{
    public class TaskUpdateModel : BaseModel
    {
        [ForeignKey(nameof(Task))]
        public int TaskId { get; set; }

        public TaskModel? Task { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal? Latitude { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal? Longitude { get; set; }

        [MaxLength(700)]
        public string? Address { get; set; }

        [MaxLength(3000)]
        public string? FileUrl { get; set; }

        public bool IsAdmin { get; set; } = false;


        public TaskUpdateTypeEnum TaskUpdateType { get; set; }

    }

    public enum TaskUpdateTypeEnum
    {
        Comment,
        Location,
        Image,
        Document,
        Hold,
        UnHold,
        Start,
        Complete
    }
}
