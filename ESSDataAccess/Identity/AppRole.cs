using Microsoft.AspNetCore.Identity;

namespace ESSDataAccess.Identity
{
    public class AppRole : IdentityRole<int>
    {
        //[Column(TypeName = "NVARCHAR(MAX)")]
        public string? Access { get; set; }

    }
}
