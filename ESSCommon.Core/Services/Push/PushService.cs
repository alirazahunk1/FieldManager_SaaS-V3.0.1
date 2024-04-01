using CorePush.Google;
using ESSCommon.Core.Enum;

namespace ESSCommon.Core.Services.Push
{
    public class PushService : IPush
    {
        private readonly FcmSettings _fcmSettings;

        public PushService(FcmSettings fcmSettings)
        {
            _fcmSettings = fcmSettings;
        }

        public async Task<bool> SendMessage(NotificationType type, string tokens, string message, string? title = null)
        {
            try
            {
                HttpClient client = new HttpClient();
                var fcm = new FcmSender(_fcmSettings, client);

                var payload = new
                {
                    to = tokens,
                    notification = new
                    {
                        title = title ?? type.ToString(),
                        body = message
                    }
                };

                var result = await fcm.SendAsync(payload);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
