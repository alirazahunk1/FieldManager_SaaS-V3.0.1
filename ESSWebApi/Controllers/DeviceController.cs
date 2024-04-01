using CZ.Api.Base;
using ESSDataAccess.Dtos.API_Dtos.MessagingToken;
using ESSWebApi.Dtos.Request.Device;
using ESSWebApi.Routes;
using ESSWebApi.Services.Device;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESSWebApi.Controllers
{
    [Authorize, ApiController]
    public class DeviceController : BaseController
    {
        private readonly IDevice _deviceRepo;

        public DeviceController(IDevice deviceRepo)
        {
            _deviceRepo = deviceRepo;
        }


        [HttpPost(APIRoutes.Device.MessagingToken)]
        public async Task<IActionResult> MessagingToken([FromBody] MessagingTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.DeviceType))
                return BadRequest("Device Type is required");

            if (string.IsNullOrEmpty(request.Token))
                return BadRequest("Token is required");

            MessageingTokenDto messageToken = new MessageingTokenDto
            {
                Token = request.Token,
                DeviceType = request.DeviceType,
                UserId = GetUserId()
            };

            var result = await _deviceRepo.AddMessagingToken(messageToken);
            if (!result) return BadRequest("Unable to add token");

            return Ok("Updated");

        }

        [HttpGet(APIRoutes.Device.checkDevice)]
        public async Task<IActionResult> CheckDevice(string deviceId, string deviceType)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return BadRequest("deviceId is required");
            }

            if (string.IsNullOrEmpty(deviceType))
            {
                return BadRequest("deviceType is required");
            }

            if (!deviceType.Equals("android") && !deviceType.Equals("ios"))
            {
                return BadRequest("Invalid device type");
            }

            var result = await _deviceRepo.CheckDevice(GetUserId(), deviceId.Trim(), deviceType.Trim());
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok("Verified");
        }

        [HttpPost(APIRoutes.Device.RegisterDevice)]
        public async Task<IActionResult> RegisterDevice([FromBody] RegisterDeviceRequest request)
        {
            if (string.IsNullOrEmpty(request.DeviceId))
            {
                return BadRequest("deviceId is required");
            }

            if (string.IsNullOrEmpty(request.DeviceType))
            {
                return BadRequest("deviceType is required");
            }

            if (request.DeviceId.Length < 6)
            {
                return BadRequest("Invalid deviceId");
            }

            if (request.DeviceType.Length < 3 || !(
                request.DeviceType.ToLower().Equals("android") ||
                request.DeviceType.ToLower().Equals("ios") ||
                request.DeviceType.ToLower().Equals("web")))
            {
                return BadRequest("Invalid deviceType");
            }

            var result = await _deviceRepo.RegisterDevice(GetUserId(), request);
            if (!result)
                return BadRequest("Unable to register the device");


            return Ok("Device Registered");
        }

        [HttpPost(APIRoutes.Device.UpdateDeviceStatus)]
        public async Task<IActionResult> UpdateStatus([FromBody] DeviceStatusUpdateRequest request)
        {
            var result = await _deviceRepo.UpdateDeviceStatus(GetUserId(), request);

            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok("Updated");
        }
    }
}
