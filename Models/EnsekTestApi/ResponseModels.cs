using Newtonsoft.Json;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.DataModels;

namespace ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi
{
    public class ResponseModels
    {
        public class LoginResponseModel
        {
            [JsonProperty("access_token")]
            public string? AccessToken { get; set; }
            [JsonProperty("message")]
            public string? Message { get; set; }
        }

        public class ResetResponseModel
        {
            [JsonProperty("message")]
            public string? Message { get; set; }
        }

        public class EnergyResponseModel
        {
            [JsonProperty("message")]
            public string Message { get; set; }
            [JsonProperty("electric")]
            public EnergyModel Electric { get; set; }
            [JsonProperty("gas")]
            public EnergyModel Gas { get; set; }
            [JsonProperty("nuclear")]
            public EnergyModel Nuclear { get; set; }
            [JsonProperty("oil")]
            public EnergyModel Oil { get; set; }
        }

        public class BuyResponseModel
        {
            [JsonProperty("message")]
            public string Message { get; set; }
        }

        public class OrderResponseModel
        {
            [JsonProperty("message")]
            public string Message { get; set; }

            public List<OrderModel> Orders { get; set; }
        }
    }
}