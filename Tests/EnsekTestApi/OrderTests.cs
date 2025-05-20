using Allure.NUnit.Attributes;
using Allure.NUnit;
using Allure.Net.Commons;
using ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi;
using RestSharp;
using Newtonsoft.Json;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.ExpectedResultsModel;
using System.Net;
using System.Text;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.ResponseModels;
using Newtonsoft.Json.Linq;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.DataModels;
using ENSEK_Lead_QA_Remote_Exercise.TestData.EnsekTestApi;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace ENSEK_Lead_QA_Remote_Exercise.Tests.EnsekTestApi
{
    [AllureNUnit]
    [AllureFeature("Order Controls")]
    [AllureSuite("Order API End Point Tests")]
    [AllureTag("API Tests", "Automated")]
    [TestFixture, Order(5)]
    public class OrderTests
    {
        [AllureBefore]
        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            AllureApi.Step("Reset System Under Test", () =>
            {
                GlobalTestSetUp.EnsekTestAPI.ResetSystem(new RequestModel
                {
                    Url = "reset",
                    AccessToken = true,
                    HttpMethod = Method.Post,
                });
            });
        }

        //Historic Order Count
        [Order(1)]
        [TestCaseSource(typeof(OrderTestData), nameof(OrderTestData.OrderVolumeTestData))]
        public void AmountOfOrders(RequestModel request, OrderExpectedResultsModel expectedResults)
        {
            AllureLifecycle.Instance.UpdateTestCase(testResult =>
            {
                testResult.parameters =
                [
                    new Allure.Net.Commons.Parameter
                    {
                        name = "Expected Order Count",
                        value = expectedResults.HistoricOrderCount.ToString()
                    },
                ];
            });

            RestResponse orderResponse = null;
            AllureApi.Step($"Step 1 : Call {GlobalTestSetUp.EnsekTestAPI.BaseUrl}orders endpoint", () =>
            {
                orderResponse = GlobalTestSetUp.EnsekTestAPI.GetCurrentEnergyStock(new RequestModel
                {
                    Url = "orders",
                    AccessToken = false,
                    HttpMethod = Method.Get,
                });
                AllureApi.AddAttachment("Raw Response", "application/json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(orderResponse, Formatting.Indented)), "json");
            });

            OrderResponseModel orderResponseModel = new OrderResponseModel();
            JToken token = JToken.Parse(orderResponse.Content);
            if (token.Type == JTokenType.Array)
            {
                orderResponseModel.Orders = JsonConvert.DeserializeObject<List<OrderModel>>(orderResponse.Content);
            }
            else
            {
                orderResponseModel = JsonConvert.DeserializeObject<OrderResponseModel>(orderResponse.Content);
            }

            AllureApi.Step($"ASSERT : HTTP Status Code Equals {expectedResults.StatusCode}", () =>
            {
                var stepParameters = new List<Allure.Net.Commons.Parameter>
                    {
                        new() { name = "Expected", value = expectedResults.StatusCode.ToString() },
                        new() { name = "Actual", value = orderResponse.StatusCode.ToString() },
                    };
                AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));
                Assert.That(orderResponse.StatusCode, Is.EqualTo(expectedResults.StatusCode));
            });
            using (Assert.EnterMultipleScope())
            {               
                AllureApi.Step($"ASSERT : Response Count Of Historic Orders Equals {expectedResults.HistoricOrderCount}", () =>
                {
                    var stepParameters = new List<Allure.Net.Commons.Parameter>
                    {
                        new() { name = "Expected", value = expectedResults.HistoricOrderCount.ToString() },
                        new() { name = "Actual", value = orderResponseModel.Orders.Count().ToString() },
                    };
                    AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));
                    Assert.That(orderResponseModel.Orders.Count(), Is.EqualTo(expectedResults.HistoricOrderCount));
                });
            }
        }

        //GET ALL
        [Order(2)]
        [TestCase(true, TestName = "Authorized User Can Get All Orders")]
        [TestCase(false, TestName = "Unauthorized User Cant Get All Orders")]
        public void GetAllOrders(bool authorized)
        {
            RestResponse orderResponse = null;
            AllureApi.Step($"Step 1 : Call {GlobalTestSetUp.EnsekTestAPI.BaseUrl}orders endpoint", () =>
            {
                orderResponse = GlobalTestSetUp.EnsekTestAPI.GetCurrentEnergyStock(new RequestModel
                {
                    Url = "orders",
                    AccessToken = true,
                    HttpMethod = Method.Get,
                });
                AllureApi.AddAttachment("Raw Response", "application/json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(orderResponse, Formatting.Indented)), "json");
            });

            OrderExpectedResultsModel expectedResults = new OrderExpectedResultsModel
            {
                StatusCode = authorized ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                Message = authorized ? null : "Unauthorized",
            };

            OrderResponseModel orderResponseModel = new OrderResponseModel();
            JToken token = JToken.Parse(orderResponse.Content);
            if (token.Type == JTokenType.Array)
            {
                orderResponseModel.Orders = JsonConvert.DeserializeObject<List<OrderModel>>(orderResponse.Content);
            }
            else
            {
                orderResponseModel = JsonConvert.DeserializeObject<OrderResponseModel>(orderResponse.Content);
            }
            AllureApi.Step($"ASSERT : HTTP Status Code Equals {expectedResults.StatusCode}", () =>
            {
                var stepParameters = new List<Allure.Net.Commons.Parameter>
                    {
                        new() { name = "Expected", value = expectedResults.StatusCode.ToString() },
                        new() { name = "Actual", value = orderResponse.StatusCode.ToString() },
                    };
                AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));
                Assert.That(orderResponse.StatusCode, Is.EqualTo(expectedResults.StatusCode));
            });
            using (Assert.EnterMultipleScope())
            {
                AllureApi.Step($"ASSERT : Response Message Equals {expectedResults.Message}", () =>
                {
                    var stepParameters = new List<Allure.Net.Commons.Parameter>
                    {
                        new() { name = "Expected", value = expectedResults.Message },
                        new() { name = "Actual", value = orderResponseModel.Message },
                    };
                    AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));
                    Assert.That(orderResponseModel.Message, Is.EqualTo(expectedResults.Message));
                });

                if (authorized)
                {
                    AllureApi.Step($"ASSERT : List of orders returned", () =>
                    {
                        var stepParameters = new List<Allure.Net.Commons.Parameter>
                    {
                        new() { name = "Expected", value = ">0" },
                        new() { name = "Actual", value = orderResponseModel.Orders.Count().ToString() },
                    };
                        AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));
                        Assert.That(orderResponseModel.Message, Is.EqualTo(expectedResults.Message));
                    });
                }                
            }
        }

        //GET BY ID
        [Order(3)]
        [TestCase(true, "080d9823-e874-4b5b-99ff-2021f2a59b25", TestName = "Authorized User Can Retrieve Order Using Order Id")]
        [TestCase(false, "080d9823-e874-4b5b-99ff-2021f2a59b25", TestName = "Unauthorized User Cant Retrieve Order Using Order Id")]
        public void RetrieveOrderById(bool authorized, string id)
        {
            RestResponse orderResponse = null;
            AllureApi.Step($"Step 1 : Call {GlobalTestSetUp.EnsekTestAPI.BaseUrl}orders/{id} endpoint", () =>
            {
                orderResponse = GlobalTestSetUp.EnsekTestAPI.GetCurrentEnergyStock(new RequestModel
                {
                    Url = "orders/{id}",
                    AccessToken = authorized,
                    HttpMethod = Method.Get,
                    UrlSegments = new Dictionary<string, string>
                    {
                        { "id", id }
                    }
                });
                AllureApi.AddAttachment("Raw Response", "application/json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(orderResponse, Formatting.Indented)), "json");
            });

            EnergyExpectedResultsModel expectedResults = new EnergyExpectedResultsModel
            {
                StatusCode = authorized ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                Message = authorized ? null : "Unauthorized"
            };

            OrderModel returnedOrder = JsonConvert.DeserializeObject<OrderModel>(orderResponse.Content);

            AllureApi.Step($"ASSERT : HTTP Status Code Equals {expectedResults.StatusCode}", () =>
            {
                var stepParameters = new List<Allure.Net.Commons.Parameter>
                    {
                        new() { name = "Expected", value = expectedResults.StatusCode.ToString() },
                        new() { name = "Actual", value = orderResponse.StatusCode.ToString() },
                    };
                AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));
                Assert.That(orderResponse.StatusCode, Is.EqualTo(expectedResults.StatusCode));
            });

            using (Assert.EnterMultipleScope())
            {
                AllureApi.Step($"ASSERT : Response Message Equals {expectedResults.Message}", () =>
                {
                    var stepParameters = new List<Allure.Net.Commons.Parameter>
                    {
                        new() { name = "Expected", value = expectedResults.Message },
                        new() { name = "Actual", value = returnedOrder.Message },
                    };
                    AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));
                    Assert.That(returnedOrder.Message, Is.EqualTo(expectedResults.Message));
                });

                if (authorized)
                {
                    OrderRecord order = new OrderRecord
                    {
                        Id = "080d9823-e874-4b5b-99ff-2021f2a59b25",
                        Fuel = "electric",
                        Quantity = 23,
                        Time = DateTime.Parse("Mon, 7 Feb 2022 00:01:24 GMT")
                    };
                    AllureApi.Step($"ASSERT : Correct Order Model Returned", () =>
                    {
                        var stepParameters = new List<Allure.Net.Commons.Parameter>
                        {
                            new() { name = "Expected Id", value = order.Id },
                            new() { name = "Actual Id", value = returnedOrder.Id },

                            new() { name = "Expected Fuel", value = order.Fuel },
                            new() { name = "Actual Fuel", value = returnedOrder.Fuel },

                            new() { name = "Expected Quantity", value = order.Quantity.ToString() },
                            new() { name = "Actual Quantity", value = returnedOrder.Quantity.ToString() },

                            new() { name = "Expected Time", value = order.Time.ToString() },
                            new() { name = "Actual Time", value = returnedOrder.Time.ToString() },
                        };
                        AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));
                        Assert.That(returnedOrder.Id, Is.EqualTo(order.Id));
                        Assert.That(returnedOrder.Fuel, Is.EqualTo(order.Fuel));
                        Assert.That(returnedOrder.Quantity, Is.EqualTo(order.Quantity));
                        Assert.That(returnedOrder.Time, Is.EqualTo(order.Time));
                    });
                }
            }
        }

        //UPDATE
        [Order(5)]
        [TestCase(true, "080d9823-e874-4b5b-99ff-2021f2a59b25", 50, "gas", TestName = "Authorized User Can Update Order Using Order Id")]
        [TestCase(false, "080d9823-e874-4b5b-99ff-2021f2a59b25", 500, "nuclear", TestName = "Unauthorized User Cant Update Order Using Order Id")]
        public void UpdateOrderById(bool authorized, string id, int newQuantity, string newFuel)
        {
            RestResponse orderResponse = null;
            AllureApi.Step($"Step 1 : Call {GlobalTestSetUp.EnsekTestAPI.BaseUrl}orders/{id}", () =>
            {
                orderResponse = GlobalTestSetUp.EnsekTestAPI.GetCurrentEnergyStock(new RequestModel
                {
                    Url = "orders/{id}",
                    AccessToken = authorized,
                    HttpMethod = Method.Put,
                    UrlSegments = new Dictionary<string, string>
                    {
                        { "id", id }
                    },
                    Body = JsonConvert.SerializeObject(new OrderModel
                    {
                        Fuel = newFuel,
                        Quantity = newQuantity,
                    })
                });
                AllureApi.AddAttachment("Raw Response", "application/json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(orderResponse, Formatting.Indented)), "json");
            });

            EnergyExpectedResultsModel expectedResults = new EnergyExpectedResultsModel
            {
                StatusCode = authorized ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                Message = authorized ? null : "Unauthorized"
            };

            OrderModel returnedOrder = JsonConvert.DeserializeObject<OrderModel>(orderResponse.Content);
            AllureApi.Step($"ASSERT : HTTP Status Code Equals {expectedResults.StatusCode}", () =>
            {
                var stepParameters = new List<Allure.Net.Commons.Parameter>
                    {
                        new() { name = "Expected", value = expectedResults.StatusCode.ToString() },
                        new() { name = "Actual", value = orderResponse.StatusCode.ToString() },
                    };
                AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));
                Assert.That(orderResponse.StatusCode, Is.EqualTo(expectedResults.StatusCode));
            });
            using (Assert.EnterMultipleScope())
            {               
                AllureApi.Step($"ASSERT : Response Message Equals {expectedResults.Message}", () =>
                {
                    var stepParameters = new List<Allure.Net.Commons.Parameter>
                    {
                        new() { name = "Expected", value = expectedResults.Message },
                        new() { name = "Actual", value = returnedOrder.Message },
                    };
                    AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));
                    Assert.That(returnedOrder.Message, Is.EqualTo(expectedResults.Message));
                });

                if (authorized)
                {
                    OrderRecord order = new OrderRecord
                    {
                        Id = "080d9823-e874-4b5b-99ff-2021f2a59b25",
                        Fuel = newFuel,
                        Quantity = newQuantity,
                        Time = DateTime.Parse("Mon, 7 Feb 2022 00:01:24 GMT")
                    };
                    AllureApi.Step($"ASSERT : Correct Order Model Returned", () =>
                    {
                        var stepParameters = new List<Allure.Net.Commons.Parameter>
                        {
                            new() { name = "Expected Id", value = order.Id },
                            new() { name = "Actual Id", value = returnedOrder.Id },

                            new() { name = "Expected Fuel", value = order.Fuel },
                            new() { name = "Actual Fuel", value = returnedOrder.Fuel },

                            new() { name = "Expected Quantity", value = order.Quantity.ToString() },
                            new() { name = "Actual Quantity", value = returnedOrder.Quantity.ToString() },

                            new() { name = "Expected Time", value = order.Time.ToString() },
                            new() { name = "Actual Time", value = returnedOrder.Time.ToString() },
                        };
                        AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));
                        Assert.That(returnedOrder.Id, Is.EqualTo(order.Id));
                        Assert.That(returnedOrder.Fuel, Is.EqualTo(order.Fuel));
                        Assert.That(returnedOrder.Quantity, Is.EqualTo(order.Quantity));
                        Assert.That(returnedOrder.Time, Is.EqualTo(order.Time));
                    });
                }
            }
        }
        
        [Order(4)]
        [TestCase(true, "31fc32da-bccb-44ab-9352-4f43fc44ed4b", TestName = "Authorized User Can Deleted Order Using Order Id")]
        [TestCase(false, "2cdd6f69-95df-437e-b4d3-e772472db8de", TestName = "Unauthorized User Cant Deleted Order Using Order Id")]
        public void DeleteOrderById(bool authorized, string id)
        {
            RestResponse orderResponse = null;
            AllureApi.Step($"Step 1 : Call {GlobalTestSetUp.EnsekTestAPI.BaseUrl}orders/{id}", () =>
            {
                orderResponse = GlobalTestSetUp.EnsekTestAPI.GetCurrentEnergyStock(new RequestModel
                {
                    Url = "orders/{id}",
                    AccessToken = authorized,
                    HttpMethod = Method.Delete,
                    UrlSegments = new Dictionary<string, string>
                    {
                        { "id", id }
                    }
                });
                AllureApi.AddAttachment("Raw Response", "application/json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(orderResponse, Formatting.Indented)), "json");
            });

            EnergyExpectedResultsModel expectedResults = new EnergyExpectedResultsModel
            {
                StatusCode = authorized ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                Message = authorized ? "Success" : "Unauthorized"
            };            

            OrderResponseModel orderResponseModel = JsonConvert.DeserializeObject<OrderResponseModel>(orderResponse.Content);

            using (Assert.EnterMultipleScope())
            {
                AllureApi.Step($"ASSERT : HTTP Status Code Equals {expectedResults.StatusCode}", () =>
                {
                    var stepParameters = new List<Allure.Net.Commons.Parameter>
                    {
                        new() { name = "Expected", value = expectedResults.StatusCode.ToString() },
                        new() { name = "Actual", value = orderResponse.StatusCode.ToString() },
                    };
                    AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));
                    Assert.That(orderResponse.StatusCode, Is.EqualTo(expectedResults.StatusCode));
                });

                AllureApi.Step($"ASSERT : Response Message Equals {expectedResults.Message}", () =>
                {
                    var stepParameters = new List<Allure.Net.Commons.Parameter>
                    {
                        new() { name = "Expected", value = expectedResults.Message },
                        new() { name = "Actual", value = orderResponseModel.Message },
                    };
                    AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));
                    Assert.That(orderResponseModel.Message, Is.EqualTo(expectedResults.Message));
                });
            }
        }
    }
}