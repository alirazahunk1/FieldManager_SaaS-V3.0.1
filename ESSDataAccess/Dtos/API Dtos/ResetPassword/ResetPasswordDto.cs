using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSDataAccess.Dtos.API_Dtos.ResetPassword
{
    public class ResetPasswordDto
    {
        public int UserId { get; set; }
        public string? Otp { get; set; }
        public string? Token { get; set; }
        public bool IsVerified { get; set; }
    }
}
