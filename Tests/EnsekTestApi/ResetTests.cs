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

namespace ENSEK_Lead_QA_Remote_Exercise.Tests.EnsekTestApi
{
    [AllureNUnit]
    [AllureFeature("Reset Controls")]
    [AllureSuite("Reset API End Point Tests")]
    [AllureTag("API Tests", "Automated")]
    [TestFixture, Order(1)]
    public class ResetTests
    {
        [TestCaseSource(typeof(ResetTestData), nameof(ResetTestData.AuthorizedUserScenarios))]
        public void ResetSystemUnderTest(RequestModel request, ResetExpectedResultsModel expectedResults)
        {
            AllureLifecycle.Instance.UpdateTestCase(testResult =>
            {
                testResult.parameters =
                [
                    new Allure.Net.Commons.Parameter
                    {
                        name = "Authorized",
                        value = $"Has Valid Access Token: {request.AccessToken.ToString()}"
                    },
                ];
            });
            RestResponse response = null;
            ResetResponseModel resetResponse = null;

            AllureApi.Step($"Step 1 : Call {GlobalTestSetUp.EnsekTestAPI.BaseUrl + request.Url}", () =>
            {
                response = GlobalTestSetUp.EnsekTestAPI.ResetSystem(request);
                AllureApi.AddAttachment("Raw Response", "application/json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response, Formatting.Indented)), "json");
                resetResponse = JsonConvert.DeserializeObject<ResetResponseModel>(response.Content);
                Assert.That(response, Is.Not.Null);
            });

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

            AllureApi.Step($"ASSERT: Response Message Equals {expectedResults.Message}", () =>
            {
                var stepParameters = new List<Allure.Net.Commons.Parameter>
                {
                    new() { name = "Expected", value = expectedResults.Message },
                    new() { name = "Actual", value = resetResponse.Message },
                };
                AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));

                Assert.That(resetResponse.Message, Is.EqualTo(expectedResults.Message));
            });
        }
    }
}