namespace ESSCommon.Core.SharedModels
{
    public class TwilioSettings
    {
        public string AccountSid { get; set; }

        public string AuthToken { get; set; }

        public string FromNumber { get; set; }

        public bool IsEnabled { get; set; }
    }
}
