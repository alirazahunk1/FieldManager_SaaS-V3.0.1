using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class ResetPasswordModel:BaseModel
    {
        [ForeignKey(nameof(Employee))]
        public int UserId { get; set; }
        public AppUser? Employee { get; set; }
        [MaxLength(10)]
        public string? Otp { get; set; }
        public string? Token { get; set; }
        public bool IsVerified { get; set; } = false;
    }
}
