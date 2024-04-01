
namespace ESSWebApi.Dtos.Request.Device
{
    public class RegisterDeviceRequest
    {

        public string DeviceType { get; set; }

        public string DeviceId { get; set; }

        public string Brand { get; set; }

        public string Board { get; set; }

        public string SdkVersion { get; set; }

        public string Model { get; set; }
    }
}
