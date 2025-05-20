using Newtonsoft.Json;

namespace ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi
{
    public class DataModels
    {
        public class LoginModel
        {
            [JsonProperty("username")]
            public string? Username { get; set; }
            [JsonProperty("password")]
            public string? Password { get; set; }
        }

        public class EnergyModel
        {
            [JsonProperty("energy_id")]
            public int EnergyId { get; set; }
            [JsonProperty("price_per_unit")]
            public decimal PricePerUnit { get; set; }
            [JsonProperty("quantity_of_units")]
            public int QuantityOfUnits { get; set; }
            [JsonProperty("unit_type")]
            public string UnitType { get; set; }
        }

        public class OrderModel
        {
            [JsonProperty("message")]
            public string Message { get; set; }
            [JsonProperty("fuel")]
            public string Fuel { get; set; }
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("quantity")]
            public int Quantity { get; set; }
            [JsonProperty("time")]
            public DateTime Time { get; set; }
        }
    }
}