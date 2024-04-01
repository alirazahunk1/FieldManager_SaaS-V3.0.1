using ESSDataAccess.DbContext;
using Microsoft.EntityFrameworkCore;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;

namespace ESSCommon.Core.SMS
{
    public class SMSService : ISMS
    {
        private readonly AppDbContext _context;

        public SMSService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SendSmsAsync(int userId, string message)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null && !string.IsNullOrEmpty(user.PhoneNumber))
            {
                var settings = await _context.SASettings
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync();

                if (!settings.TwilioStatus)
                    return false;


                if (string.IsNullOrEmpty(settings.PhoneCountryCode)
                     || string.IsNullOrEmpty(settings.TwilioFromNumber)
                    || string.IsNullOrEmpty(settings.TwilioAccountSid)
                    || string.IsNullOrEmpty(settings.TwilioAuthToken))
                    return false;

                TwilioClient.Init(settings.TwilioAccountSid, settings.TwilioAuthToken);

                try
                {
                    var response = MessageResource.Create(
                        body: message,
                        from: new Twilio.Types.PhoneNumber(settings.TwilioFromNumber),
                        to: new Twilio.Types.PhoneNumber(settings.PhoneCountryCode + user.PhoneNumber)
                    );

                    // Console.WriteLine(response.Sid);
                    return true;
                }
                catch (ApiException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine($"Twilio Error {e.Code} - {e.MoreInfo}");
                }
            }

            return false;
        }
    }
}
