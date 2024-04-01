namespace ESSCommon.Core.SMS
{
    public interface ISMS
    {
        Task<bool> SendSmsAsync(int userId, string message);
    }
}
