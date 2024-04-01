using ESSWebPortal.Core.SuperAdmin;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ESSWebPortal.AddonHelpers
{
    public class ApiModuleHelper
    {
        private readonly ISASettings _saSettings;

        public ApiModuleHelper(ISASettings dbSettings)
        {
            _saSettings = dbSettings;
        }

        public async Task<ApiModuleResult?> GetApiModuleStatus()
        {
            var apiBaseUrl = await _saSettings.GetApiBaseUrl();

            if (string.IsNullOrEmpty(apiBaseUrl))
            {
                return null;
            }
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    client.BaseAddress = new Uri(apiBaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync("getAssemblies");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        var obj = JsonConvert.DeserializeObject<ApiModuleResult>(result);

                        if (obj != null && obj.Status == "success")
                        {
                            obj.IsSuccess = true;
                            return obj;
                        }

                        return new ApiModuleResult
                        {
                            IsSuccess = false,
                        };
                    }
                    else
                    {
                        return new ApiModuleResult
                        {
                            IsSuccess = false
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                return new ApiModuleResult
                {
                    IsSuccess = false
                };
            }

        }


        public class ApiModuleResult
        {
            public bool IsSuccess { get; set; } = false;

            [JsonProperty("statusCode")]
            public int StatusCode { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            public List<ApiModule>? Data { get; set; }
        }
    }


    public class ApiModule
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

    }
}
