using Newtonsoft.Json;
using System.Net;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.DataModels;

namespace ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi
{
    public class ExpectedResultsModel
    {
        public class LoginExpectedResultsModel
        {
            public HttpStatusCode StatusCode { get; set; }
            public string Message { get; set; }
            public bool AccessToken { get; set; }
        }

        public class ResetExpectedResultsModel
        {
            public HttpStatusCode StatusCode { get; set; }
            public string Message { get; set; }
        }

        public class EnergyExpectedResultsModel
        {
            public HttpStatusCode StatusCode { get; set; }
            public string Message { get; set; }
            public EnergyModel Electric { get; set; }
            public EnergyModel Gas { get; set; }
            public EnergyModel Nuclear { get; set; }
            public EnergyModel Oil { get; set; }
        }

        public class BuyExpectedResultsModel
        {
            public HttpStatusCode StatusCode { get; set; }
            public string Message { get; set; }
            public decimal PricePerUnit { get; set; }
            public int QuantityOfUnits { get; set; }
            public string UnitType { get; set; }
            public OrderModel OrderRecord { get; set; }
        }

        public class OrderExpectedResultsModel
        {
            public HttpStatusCode StatusCode { get; set; }
            public string Message { get; set; }
            public int HistoricOrderCount { get; set; }
            public List<OrderModel> Orders { get; set; }
        }
    }
}