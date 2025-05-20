using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi;
using RestSharp;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.ExpectedResultsModel;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.ResponseModels;
using Newtonsoft.Json;
using System.Text;
using ENSEK_Lead_QA_Remote_Exercise.TestData.EnsekTestApi;
using Newtonsoft.Json.Linq;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.DataModels;
using System.Net;

namespace ENSEK_Lead_QA_Remote_Exercise.Tests.EnsekTestApi
{
    [AllureNUnit]
    [AllureFeature("Buy Controls")]
    [AllureSuite("Buy API End Point Tests")]
    [AllureTag("API Tests", "Automated")]
    [TestFixture, Order(4)]
    public class BuyTests
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

        [TestCase(TestName = "Unauthorized User Cant Buy Fuel")]
        public void UnauthorizedUserCantAccessData()
        {
            RestResponse buyResponse = null;
            AllureApi.Step("Step 1 : Call energy endpoint without access token", () =>
            {
                buyResponse = GlobalTestSetUp.EnsekTestAPI.Buy(new RequestModel
                {
                    Url = "buy/{id}/{quantity}",
                    AccessToken = false,
                    HttpMethod = Method.Put,
                    UrlSegments = new Dictionary<string, string> { { "id", "3" }, { "quantity", "1" } }
                });
                AllureApi.AddAttachment("Raw Response", "application/json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(buyResponse, Formatting.Indented)), "json");
            });

            BuyExpectedResultsModel expectedResults = new BuyExpectedResultsModel
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Message = "Unauthorized"
            };

            BuyResponseModel buyResponseModel = JsonConvert.DeserializeObject<BuyResponseModel>(buyResponse.Content);
            using (Assert.EnterMultipleScope())
            {
                AllureApi.Step("Step 1 : HTTP Status Code Equals {expectedResults.StatusCode}", () =>
                {
                    Assert.That(buyResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
                });

                AllureApi.Step($"ASSERT : Response Message Equals {expectedResults.Message}", () =>
                {
                    Assert.That(buyResponseModel.Message, Is.EqualTo("Unauthorized"));
                });
            }
        }

        [TestCaseSource(typeof(BuyTestData), nameof(BuyTestData.PurchaseScenarios))]
        public void BuyEnergy(RequestModel request, BuyExpectedResultsModel expectedResults)
        {
            AllureLifecycle.Instance.UpdateTestCase(testResult =>
            {
                testResult.parameters =
                [
                    new Allure.Net.Commons.Parameter
                    {
                        name = "EnergyId",
                        value =  request.UrlSegments["id"].ToString()
                    },
                    new Allure.Net.Commons.Parameter
                    {
                        name = "Quantity",
                        value = request.UrlSegments["quantity"].ToString()
                    },
                ];
            });
            if (request.UrlSegments["id"] == "2") Assert.Inconclusive("SUT Does Not Allow For Nuclear Purchase Tests.");
            RestResponse response = null;
            BuyResponseModel buyResponse = null;

            AllureApi.Step($"Step 1 : Call {GlobalTestSetUp.EnsekTestAPI.BaseUrl + request.Url}", () =>
            {
                response = GlobalTestSetUp.EnsekTestAPI.GetCurrentEnergyStock(request);
                AllureApi.AddAttachment("Raw Response", "application/json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response, Formatting.Indented)), "json");
                buyResponse = JsonConvert.DeserializeObject<BuyResponseModel>(response.Content);
                Assert.That(response, Is.Not.Null);
            });

            buyResponse = JsonConvert.DeserializeObject<BuyResponseModel>(response.Content);
            var stringArray = buyResponse.Message.Split([" "], StringSplitOptions.RemoveEmptyEntries);

            AllureApi.Step($"ASSERT: HTTP Status Code Equals {expectedResults.StatusCode}", () =>
            {
                var stepParameters = new List<Allure.Net.Commons.Parameter>
                {
                    new() { name = "Expected", value = expectedResults.StatusCode.ToString() },
                    new() { name = "Actual", value = response.StatusCode.ToString() },
                };
                AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));

                Assert.That(response.StatusCode, Is.EqualTo(expectedResults.StatusCode));
            });

            using (Assert.EnterMultipleScope())
            {       
                AllureApi.Step($"ASSERT: Response Quantity Equals {expectedResults.QuantityOfUnits}", () =>
                {
                    var stepParameters = new List<Allure.Net.Commons.Parameter>
                    {
                        new() { name = "Expected", value = stringArray[3] },
                        new() { name = "Actual", value = expectedResults.QuantityOfUnits.ToString() },
                    };
                    AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));

                    Assert.That(stringArray[3], Is.EqualTo(expectedResults.QuantityOfUnits.ToString("0.##")));
                });

                AllureApi.Step($"ASSERT: Response Price Equals {(expectedResults.PricePerUnit * expectedResults.QuantityOfUnits).ToString("0.00")}", () =>
                {
                    var stepParameters = new List<Allure.Net.Commons.Parameter>
                    {
                        new() { name = "Expected", value = stringArray[9] },
                        new() { name = "Actual", value = (expectedResults.PricePerUnit * expectedResults.QuantityOfUnits).ToString("0.00") },
                    };
                    AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));

                    Assert.That(stringArray[9], Is.EqualTo((expectedResults.PricePerUnit * expectedResults.QuantityOfUnits).ToString("0.00")));
                });

                AllureApi.Step($"ASSERT: Response Unit of Measurement Equals {expectedResults.UnitType}", () =>
                {
                    var stepParameters = new List<Allure.Net.Commons.Parameter>
                    {
                        new() { name = "Expected", value = stringArray[4] },
                        new() { name = "Actual", value = expectedResults.UnitType },
                    };
                    AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));

                    Assert.That(stringArray[4], Is.EqualTo(expectedResults.UnitType));
                });

                //TODO: Add Assert for Order
                RestResponse orderResponse = null;
                AllureApi.Step($"Step 2 : Call {GlobalTestSetUp.EnsekTestAPI.BaseUrl}/orders", () =>
                {
                    var ordersRequest = new RequestModel
                    {
                        Url = "orders",
                        AccessToken = true,
                        HttpMethod = Method.Get,
                    };
                    orderResponse = GlobalTestSetUp.EnsekTestAPI.GetCurrentEnergyStock(ordersRequest);
                    AllureApi.AddAttachment("Raw Response", "application/json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(orderResponse, Formatting.Indented)), "json");
                    Assert.That(response, Is.Not.Null);
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

                AllureApi.Step($"ASSERT: Order is present", () =>
                {
                    OrderModel order = orderResponseModel.Orders.FirstOrDefault(o => o.Id == stringArray[stringArray.Length - 1].Replace(".", ""));
                    var stepParameters = new List<Allure.Net.Commons.Parameter>
                    {
                        new() { name = "Expected Id", value = (stringArray[stringArray.Length - 1].Replace(".", "")) },
                        new() { name = "Expected Fuel", value = expectedResults.OrderRecord.Fuel },
                        new() { name = "Actual Fuel", value = order.Fuel },
                        new() { name = "Expected Quantity", value = expectedResults.OrderRecord.Quantity.ToString() },
                        new() { name = "Actual Quantity", value = order.Quantity.ToString() },
                    };
                    AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));

                    Assert.That(order.Id, Is.EqualTo(stringArray[stringArray.Length - 1].Replace(".", "")));
                    Assert.That(order.Fuel, Is.EqualTo(expectedResults.OrderRecord.Fuel));
                    Assert.That(order.Quantity, Is.EqualTo(expectedResults.OrderRecord.Quantity));
                });
            }
        }
    }
}