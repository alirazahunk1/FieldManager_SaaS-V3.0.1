using ESSCommon.Core.Services.Notification;
using ESSDataAccess.DbContext;
using ESSDataAccess.Dtos.API_Dtos.MessagingToken;
using ESSDataAccess.Models;
using ESSWebApi.Dtos.Request.Device;
using ESSWebApi.Dtos.Result;
using Microsoft.EntityFrameworkCore;

namespace ESSWebApi.Services.Device
{
    public class DeviceRepo : IDevice
    {
        private readonly AppDbContext _context;
        private readonly INotification _notification;

        public DeviceRepo(AppDbContext context,
            INotification notification)
        {
            _context = context;
            _notification = notification;
        }
        public async Task<bool> AddMessagingToken(MessageingTokenDto data)
        {
            var device = await _context.UserDevices.
                AsNoTracking()
               .FirstOrDefaultAsync(x => x.UserId == data.UserId);

            if (device != null)
            {
                device.Token = data.Token;
                device.BatteryPercentage = 0;
                device.IsGPSOn = false;
                device.IsOnline = false;
                device.IsWifiOn = false;
                device.SignalStrength = 1;
                device.UpdatedAt = DateTime.Now;
                _context.Update(device);
                await _context.SaveChangesAsync();
            }
            else
            {
                return false;
            }

            return true;
        }

        public async Task<BaseResult> CheckDevice(int userId, string deviceId, string deviceType)
        {
            var device = await _context.UserDevices
               .Include(x => x.User)
               .FirstOrDefaultAsync(x => x.UserId == userId);

            if (device != null && device.User.UserName.Equals("employee"))
            {
                return new BaseResult
                {
                    IsSuccess = true,
                };
            }

            if (device != null)
            {
                //Allowed case
                if (device.DeviceId == deviceId && device.DeviceType == deviceType)
                {
                    return new BaseResult
                    {
                        IsSuccess = true,
                    };
                }

                return new BaseResult
                {
                    Message = "Already registered with other device"
                };
            }
            else
            {
                return new BaseResult
                {
                    Message = "Not registered"
                };
            }
        }

        public async Task<bool> RegisterDevice(int userId, RegisterDeviceRequest request)
        {
            var oldDevice = await _context.UserDevices
              .FirstOrDefaultAsync(x => x.UserId == userId);

            //Delete old device
            if (oldDevice != null)
            {
                _context.Remove(oldDevice);
                await _context.SaveChangesAsync();
            }

            UserDeviceModel userDevice = new UserDeviceModel
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserId = userId,
                DeviceId = request.DeviceId,
                DeviceType = request.DeviceType,
                Board = request.Board,
                Brand = request.Brand,
                Model = request.Model,
                CreatedBy = userId,
                SdkVersion = request.SdkVersion,
            };

            await _context.AddAsync(userDevice);

            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<BaseResult> UpdateDeviceStatus(int userId, DeviceStatusUpdateRequest request)
        {
            var device = await _context.UserDevices
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (device != null)
            {
                if (device.IsGPSOn && !request.IsGPSOn)
                {
                    await _notification.SendGpsWarning(userId, GpsStatusEnum.Off);
                }

                if (!device.IsGPSOn && request.IsGPSOn)
                {
                    await _notification.SendGpsWarning(userId, GpsStatusEnum.On);
                }

                device.UpdatedAt = DateTime.Now;
                device.BatteryPercentage = request.BatteryPercentage;
                device.IsGPSOn = request.IsGPSOn;
                device.IsWifiOn = request.IsWifiOn;
                device.SignalStrength = request.SignalStrength;
                device.Latitude = request.Latitude;
                device.Longitude = request.Longitude;
                device.IsMock = request.IsMock;

                _context.Update(device);
                await _context.SaveChangesAsync();
            }


            return new BaseResult
            {
                IsSuccess = true,
            };
        }
    }
}
