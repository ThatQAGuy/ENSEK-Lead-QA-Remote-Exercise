using ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi;
using RestSharp;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.ExpectedResultsModel;
using System.Net;

namespace ENSEK_Lead_QA_Remote_Exercise.TestData.EnsekTestApi
{
    public class BuyTestData
    {
        public static IEnumerable<TestCaseData> PurchaseScenarios
        {
            get
            {
                yield return new TestCaseData(
                    new RequestModel
                    {
                        Url = "buy/{id}/{quantity}",
                        AccessToken = true,
                        HttpMethod = Method.Put,
                        UrlSegments = new Dictionary<string, string> { { "id", "3" }, { "quantity", "1" } }
                    },
                    new BuyExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.OK,
                        PricePerUnit = 0.47m,
                        QuantityOfUnits = 1,
                        UnitType = "kWh",
                        OrderRecord = new DataModels.OrderModel
                        {
                            Fuel = "Elec",
                            Quantity = 1,
                        }
                    }
                ).SetName("Purchase Electric - One Unit");

                yield return new TestCaseData(
                   new RequestModel
                   {
                       Url = "buy/{id}/{quantity}",
                       AccessToken = true,
                       HttpMethod = Method.Put,
                       UrlSegments = new Dictionary<string, string> { { "id", "3" }, { "quantity", "1600" } }
                   },
                   new BuyExpectedResultsModel
                   {
                       StatusCode = HttpStatusCode.OK,
                       PricePerUnit = 0.47m,
                       QuantityOfUnits = 1600,
                       UnitType = "kWh",
                       OrderRecord = new DataModels.OrderModel
                       {
                           Fuel = "Elec",
                           Quantity = 1600,
                       }
                   }
               ).SetName("Purchase Electric - Many Units");

                yield return new TestCaseData(
                                   new RequestModel
                                   {
                                       Url = "buy/{id}/{quantity}",
                                       AccessToken = true,
                                       HttpMethod = Method.Put,
                                       UrlSegments = new Dictionary<string, string> { { "id", "3" }, { "quantity", "10000" } }
                                   },
                                   new BuyExpectedResultsModel
                                   {
                                       StatusCode = HttpStatusCode.BadRequest,
                                       PricePerUnit = 0.47m,
                                       QuantityOfUnits = 10000,
                                       UnitType = "kWh",
                                       OrderRecord = new DataModels.OrderModel
                                       {
                                           Fuel = "elec",
                                           Quantity = 10000,
                                       }
                                   }
                               ).SetName("Purchase Electric - More Than Current Stock");

                yield return new TestCaseData(
                    new RequestModel
                    {
                        Url = "buy/{id}/{quantity}",
                        AccessToken = true,
                        HttpMethod = Method.Put,
                        UrlSegments = new Dictionary<string, string> { { "id", "1" }, { "quantity", "1" } }
                    },
                    new BuyExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.OK,
                        PricePerUnit = 0.34m,
                        QuantityOfUnits = 1,
                        UnitType = "m³",
                        OrderRecord = new DataModels.OrderModel
                        {
                            Fuel = "Gas",
                            Quantity = 1,
                        }
                    }
                ).SetName("Purchase Gas - One Unit");

                yield return new TestCaseData(
                   new RequestModel
                   {
                       Url = "buy/{id}/{quantity}",
                       AccessToken = true,
                       HttpMethod = Method.Put,
                       UrlSegments = new Dictionary<string, string> { { "id", "1" }, { "quantity", "1500" } }
                   },
                   new BuyExpectedResultsModel
                   {
                       StatusCode = HttpStatusCode.OK,
                       PricePerUnit = 0.34m,
                       QuantityOfUnits = 1500,
                       UnitType = "m³",
                       OrderRecord = new DataModels.OrderModel
                       {
                           Fuel = "Gas",
                           Quantity = 1500,
                       }
                   }
               ).SetName("Purchase Gas - Many Units");

                yield return new TestCaseData(
                    new RequestModel
                    {
                        Url = "buy/{id}/{quantity}",
                        AccessToken = true,
                        HttpMethod = Method.Put,
                        UrlSegments = new Dictionary<string, string> { { "id", "1" }, { "quantity", "10000" } }
                    },
                    new BuyExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        PricePerUnit = 0.34m,
                        QuantityOfUnits = 10000,
                        UnitType = "m³",
                        OrderRecord = new DataModels.OrderModel
                        {
                            Fuel = "Gas",
                            Quantity = 10000,
                        }
                    }
                ).SetName("Purchase Gas - More Than Current Stock");

                yield return new TestCaseData(
                    new RequestModel
                    {
                        Url = "buy/{id}/{quantity}",
                        AccessToken = true,
                        HttpMethod = Method.Put,
                        UrlSegments = new Dictionary<string, string> { { "id", "4" }, { "quantity", "1" } }
                    },
                    new BuyExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.OK,
                        PricePerUnit = 0.50m,
                        QuantityOfUnits = 1,
                        UnitType = "Litres",
                        OrderRecord = new DataModels.OrderModel
                        {
                            Fuel = "Oil",
                            Quantity = 1,
                        }
                    }
                ).SetName("Purchase Oil - One Unit");

                yield return new TestCaseData(
                   new RequestModel
                   {
                       Url = "buy/{id}/{quantity}",
                       AccessToken = true,
                       HttpMethod = Method.Put,
                       UrlSegments = new Dictionary<string, string> { { "id", "4" }, { "quantity", "10" } }
                   },
                   new BuyExpectedResultsModel
                   {
                       StatusCode = HttpStatusCode.OK,
                       PricePerUnit = 0.50m,
                       QuantityOfUnits = 10,
                       UnitType = "Litres",
                       OrderRecord = new DataModels.OrderModel
                       {
                           Fuel = "Oil",
                           Quantity = 10,
                       }
                   }
               ).SetName("Purchase Oil - Many Units");

                yield return new TestCaseData(
                    new RequestModel
                    {
                        Url = "buy/{id}/{quantity}",
                        AccessToken = true,
                        HttpMethod = Method.Put,
                        UrlSegments = new Dictionary<string, string> { { "id", "4" }, { "quantity", "10000" } }
                    },
                    new BuyExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        PricePerUnit = 0.50m,
                        QuantityOfUnits = 10000,
                        UnitType = "Litres",
                        OrderRecord = new DataModels.OrderModel
                        {
                            Fuel = "Oil",
                            Quantity = 10000,
                        }
                    }
                ).SetName("Purchase Oil - More Than Current Stock");

                yield return new TestCaseData(
                    new RequestModel
                    {
                        Url = "buy/{id}/{quantity}",
                        AccessToken = true,
                        HttpMethod = Method.Put,
                        UrlSegments = new Dictionary<string, string> { { "id", "2" }, { "quantity", "1" } }
                    },
                    new BuyExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.OK,
                        PricePerUnit = 0.56m,
                        QuantityOfUnits = 1,
                        UnitType = "MW",
                        OrderRecord = new DataModels.OrderModel
                        {
                            Fuel = "Nuclear",
                            Quantity = 1,
                        }
                    }
                ).SetName("Purchase Nuclear - One Unit");

                yield return new TestCaseData(
                   new RequestModel
                   {
                       Url = "buy/{id}/{quantity}",
                       AccessToken = true,
                       HttpMethod = Method.Put,
                       UrlSegments = new Dictionary<string, string> { { "id", "2" }, { "quantity", "10" } }
                   },
                   new BuyExpectedResultsModel
                   {
                       StatusCode = HttpStatusCode.OK,
                       PricePerUnit = 0.56m,
                       QuantityOfUnits = 1,
                       UnitType = "MW",
                       OrderRecord = new DataModels.OrderModel
                       {
                           Fuel = "Nuclear",
                           Quantity = 10,
                       }
                   }
               ).SetName("Purchase Nuclear - Many Units");

                yield return new TestCaseData(
                    new RequestModel
                    {
                        Url = "buy/{id}/{quantity}",
                        AccessToken = true,
                        HttpMethod = Method.Put,
                        UrlSegments = new Dictionary<string, string> { { "id", "2" }, { "quantity", "10000" } }
                    },
                    new BuyExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        PricePerUnit = 0.56m,
                        QuantityOfUnits = 1,
                        UnitType = "MW",
                        OrderRecord = new DataModels.OrderModel
                        {
                            Fuel = "Nuclear",
                            Quantity = 10000,
                        }
                    }
                ).SetName("Purchase Nuclear - More Than Current Stock");
            }
        }
    }
}