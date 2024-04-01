using ESSDataAccess.Models;

namespace ESSWebPortal.Core.ViewModel.SuperAdmin
{
    public class PaypalSettingsVM
    {
        public string PaypalClientId { get; set; }

        public string PaypalClientSecret { get; set; }

        public PaypalModeEnum? PaypalMode { get; set; }
    }
}
