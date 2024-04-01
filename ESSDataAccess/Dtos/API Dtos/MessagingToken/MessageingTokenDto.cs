using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSDataAccess.Dtos.API_Dtos.MessagingToken
{
    public class MessageingTokenDto
    {
        public string DeviceType { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
    }
}
