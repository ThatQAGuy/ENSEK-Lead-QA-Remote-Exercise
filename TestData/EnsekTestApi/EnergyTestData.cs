using ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi;
using RestSharp;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.DataModels;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.ExpectedResultsModel;
using System.Net;

namespace ENSEK_Lead_QA_Remote_Exercise.TestData.EnsekTestApi
{
    public class EnergyTestData
    {
        public static IEnumerable<TestCaseData> EnergyConfigurations
        {
            get
            {
                yield return new TestCaseData(
                    new RequestModel
                    {
                        Url = "energy",
                        AccessToken = true,
                        HttpMethod = Method.Get,
                    },
                    new EnergyExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Success",
                        Electric = new EnergyModel
                        {
                            EnergyId = 3,
                            PricePerUnit = 0.47m,
                            QuantityOfUnits = 4322,
                            UnitType = "kWh"
                        }
                    }
                ).SetName("Electric: Valid user can retrieve stock data");
                yield return new TestCaseData(
                    new RequestModel
                    {
                        Url = "energy",
                        AccessToken = true,
                        HttpMethod = Method.Get,
                    },
                    new EnergyExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Success",
                        Gas = new EnergyModel
                        {
                            EnergyId = 1,
                            PricePerUnit = 0.34m,
                            QuantityOfUnits = 3000,
                            UnitType = "m³"
                        }
                    }
                ).SetName("Gas: Valid user can retrieve stock data");
                yield return new TestCaseData(
                    new RequestModel
                    {
                        Url = "energy",
                        AccessToken = true,
                        HttpMethod = Method.Get,
                    },
                    new EnergyExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Success",
                        Nuclear = new EnergyModel
                        {
                            EnergyId = 2,
                            PricePerUnit = 0.56m,
                            QuantityOfUnits = 0,
                            UnitType = "MW"
                        },
                    }
                ).SetName("Nuclear: Valid user can retrieve stock data");
                yield return new TestCaseData(
                    new RequestModel
                    {
                        Url = "energy",
                        AccessToken = true,
                        HttpMethod = Method.Get,
                    },
                    new EnergyExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Success",
                        Oil = new EnergyModel
                        {
                            EnergyId = 4,
                            PricePerUnit = 0.50m,
                            QuantityOfUnits = 20,
                            UnitType = "Litres"
                        }
                    }
                ).SetName("Oil: Valid user can retrieve stock data");
            }
        }
    }
}
