using ESSDataAccess.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models
{
    public class ExpenseTypeModel : BaseModel
    {
        [MaxLength(60)]
        public string Name { get; set; }

        [DisplayName("Is Image Required")]
        public bool IsImgRequired { get; set; }

        public List<ExpenseRequestModel>? ExpenseRequests { get; set; }
    }
}
