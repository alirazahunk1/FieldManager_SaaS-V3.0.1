using ESSCommon.Core.Enum;

namespace ESSCommon.Core.Services.Push
{
    public interface IPush
    {
        Task<bool> SendMessage(NotificationType type, string tokens, string message, string? title = null);
    }
}
