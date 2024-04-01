using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using ESSDataAccess.Models.Site;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Task
{
    public class TaskModel : BaseModel
    {
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Display(Name = "Task Type")]
        public TaskTypeEnum TaskType { get; set; }

        public int AssignedById { get; set; }

        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }

        public AppUser? Employee { get; set; }

        [ForeignKey(nameof(Client))]
        public int? ClientId { get; set; }

        public ClientModel? Client { get; set; }

        [ForeignKey(nameof(Site))]
        public int? SiteId { get; set; }

        public SiteModel? Site { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal? Latitude { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal? Longitude { get; set; }

        public bool IsGeoFenceEnabled { get; set; }

        public int MaxRadius { get; set; }

        [Display(Name = "Start Date Time")]
        public DateTime? StartDateTime { get; set; }

        [Display(Name = "End Date Time")]
        public DateTime? EndDateTime { get; set; }

        [Display(Name = "For Date")]
        public DateTime ForDate { get; set; }

        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.New;

        public DateTime? DueDate { get; set; }


        public int? StartFormId { get; set; }


        public int? EndFormId { get; set; }


        public List<TaskUpdateModel>? TaskUpdates { get; set; }
    }

    public enum TaskTypeEnum
    {
        [Display(Name = "Open Task")]
        OpenTask,
        [Display(Name = "Client Based Task")]
        ClientBasedTask
    }

    public enum TaskStatusEnum
    {
        New,
        InProgress,
        Completed,
        Cancelled,
        Hold,
        Rejected,
        Reassigned,
        Reopened,
        Resolved,
        Closed
    }
}
