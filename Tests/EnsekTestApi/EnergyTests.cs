using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.ExpectedResultsModel;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.ResponseModels;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.DataModels;
using ENSEK_Lead_QA_Remote_Exercise.TestData.EnsekTestApi;

namespace ENSEK_Lead_QA_Remote_Exercise.Tests.EnsekTestApi
{
    [AllureNUnit]
    [AllureFeature("Energy Controls")]
    [AllureSuite("Energy API End Point Tests")]
    [AllureTag("API Tests", "Automated")]
    [TestFixture, Order(3)]
    public class EnergyTests
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

        [TestCase(TestName = "Unauthorized User Cant Access Data")]
        public void UnauthorizedUserCantAccessData()
        {
            RestResponse energyResponse = null;
            AllureApi.Step("Step 1 : Call energy endpoint without access token", () =>
            {
                energyResponse = GlobalTestSetUp.EnsekTestAPI.GetCurrentEnergyStock(new RequestModel
                {
                    Url = "energy",
                    AccessToken = false,
                    HttpMethod = Method.Get,
                });
                AllureApi.AddAttachment("Raw Response", "application/json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(energyResponse, Formatting.Indented)), "json");
            });
            
            EnergyExpectedResultsModel expectedResults = new EnergyExpectedResultsModel
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Message = "Unauthorized"
            };

            EnergyResponseModel energyResponseModel = JsonConvert.DeserializeObject<EnergyResponseModel>(energyResponse.Content);
            using (Assert.EnterMultipleScope())
            {
                AllureApi.Step("Step 1 : HTTP Status Code Equals {expectedResults.StatusCode}", () =>
                {
                    Assert.That(energyResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
                });

                AllureApi.Step($"ASSERT : Response Message Equals {expectedResults.Message}", () =>
                {
                    Assert.That(energyResponseModel.Message, Is.EqualTo("Unauthorized"));
                });
            }
        }

        [TestCaseSource(typeof(EnergyTestData), nameof(EnergyTestData.EnergyConfigurations))]
        public void EnergyConfiguration(RequestModel request, EnergyExpectedResultsModel expectedResults)
        {
            var energyIdUnderTest = expectedResults.Electric is not null ? expectedResults.Electric.EnergyId : expectedResults.Gas is not null ? expectedResults.Gas.EnergyId : expectedResults.Nuclear is not null ? expectedResults.Nuclear.EnergyId : expectedResults.Oil is not null ? expectedResults.Oil.EnergyId : 0;
            AllureLifecycle.Instance.UpdateTestCase(testResult =>
            {
                
                testResult.parameters =
                [
                    new Allure.Net.Commons.Parameter
                    {
                        name = "EnergyId",
                        value = energyIdUnderTest.ToString()
                    },
                ];
            });
            RestResponse response = null;
            EnergyResponseModel energyResponse = null;

            AllureApi.Step($"Step 1 : Call {GlobalTestSetUp.EnsekTestAPI.BaseUrl + request.Url}", () =>
            {
                response = GlobalTestSetUp.EnsekTestAPI.GetCurrentEnergyStock(request);
                AllureApi.AddAttachment("Raw Response", "application/json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response, Formatting.Indented)), "json");
                energyResponse = JsonConvert.DeserializeObject<EnergyResponseModel>(response.Content);
                Assert.That(response, Is.Not.Null);
            });

            EnergyModel expectedResultsEnergy = null;
            EnergyModel resultsEnergy = null;
            switch (energyIdUnderTest)
            {
                case 1:
                    expectedResultsEnergy = expectedResults.Gas;
                    resultsEnergy = energyResponse.Gas;
                    break;
                case 2:
                    expectedResultsEnergy = expectedResults.Nuclear;
                    resultsEnergy = energyResponse.Nuclear;
                    break;
                case 3:
                    expectedResultsEnergy = expectedResults.Electric;
                    resultsEnergy = energyResponse.Electric;
                    break;
                case 4:
                    expectedResultsEnergy = expectedResults.Oil;
                    resultsEnergy = energyResponse.Oil;
                    break;
                default:
                    break;
            }

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

            AllureApi.Step($"ASSERT: Response EnergyId Equals {expectedResultsEnergy.EnergyId}", () =>
            {
                var stepParameters = new List<Allure.Net.Commons.Parameter>
                {
                    new() { name = "Expected", value = expectedResultsEnergy.EnergyId.ToString() },
                    new() { name = "Actual", value = resultsEnergy.EnergyId.ToString() },
                };
                AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));

                Assert.That(resultsEnergy.EnergyId, Is.EqualTo(expectedResultsEnergy.EnergyId));
            });

            AllureApi.Step($"ASSERT: Response Price Per Unit Equals {expectedResultsEnergy.PricePerUnit}", () =>
            {
                var stepParameters = new List<Allure.Net.Commons.Parameter>
                {
                    new() { name = "Expected", value = expectedResultsEnergy.PricePerUnit.ToString() },
                    new() { name = "Actual", value = resultsEnergy.PricePerUnit.ToString() },
                };
                AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));

                Assert.That(resultsEnergy.PricePerUnit, Is.EqualTo(expectedResultsEnergy.PricePerUnit));
            });

            AllureApi.Step($"ASSERT: Response Quantity Of Units Equals {expectedResultsEnergy.QuantityOfUnits}", () =>
            {
                var stepParameters = new List<Allure.Net.Commons.Parameter>
                {
                    new() { name = "Expected", value = expectedResultsEnergy.QuantityOfUnits.ToString() },
                    new() { name = "Actual", value = resultsEnergy.QuantityOfUnits.ToString() },
                };
                AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));

                Assert.That(resultsEnergy.QuantityOfUnits, Is.EqualTo(expectedResultsEnergy.QuantityOfUnits));
            });

            AllureApi.Step($"ASSERT: Response Unit Type Equals {expectedResultsEnergy.UnitType}", () =>
            {
                var stepParameters = new List<Allure.Net.Commons.Parameter>
                {
                    new() { name = "Expected", value = expectedResultsEnergy.UnitType.ToString() },
                    new() { name = "Actual", value = resultsEnergy.UnitType.ToString() },
                };
                AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));

                Assert.That(resultsEnergy.UnitType, Is.EqualTo(expectedResultsEnergy.UnitType));
            });
        }        
    }
}