using ESSDataAccess.Dtos.API_Dtos.MessagingToken;
using ESSWebApi.Dtos.Request.Device;
using ESSWebApi.Dtos.Result;

namespace ESSWebApi.Services.Device
{
    public interface IDevice
    {
        Task<bool> RegisterDevice(int userId, RegisterDeviceRequest request);

        Task<BaseResult> CheckDevice(int userId, string deviceId, string deviceType);

        Task<BaseResult> UpdateDeviceStatus(int userId, DeviceStatusUpdateRequest request);

        Task<bool> AddMessagingToken(MessageingTokenDto data);
    }
}
