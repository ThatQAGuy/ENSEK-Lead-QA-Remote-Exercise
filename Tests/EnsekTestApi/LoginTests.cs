using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using RestSharp;
using ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.ExpectedResultsModel;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.ResponseModels;
using Newtonsoft.Json;
using System.Text;
using ENSEK_Lead_QA_Remote_Exercise.TestData.EnsekTestApi;

namespace ENSEK_Lead_QA_Remote_Exercise.Tests.EnsekTestApi
{
    [AllureNUnit]
    [AllureFeature("Login Controls")]
    [AllureSuite("Login API End Point Tests")]
    [AllureTag("API Tests", "Automated")]
    [TestFixture, Order(2)]
    public class LoginTests
    {
        [TestCaseSource(typeof(LoginTestData), nameof(LoginTestData.LoginScenarios))]
        public void LoginToApi(RequestModel request, LoginExpectedResultsModel expectedResults)
        {
            AllureLifecycle.Instance.UpdateTestCase(testResult =>
            {
                testResult.parameters =
                [
                    new Allure.Net.Commons.Parameter
                    {
                        name = "Body",
                        value = request.Body.ToString()
                    },
                ];
            });
            RestResponse response = null;
            LoginResponseModel loginResponse = null;
            var isOrIsNot = expectedResults.AccessToken == true ? "is" : "is not";

            AllureApi.Step("Step 1 : Create Request Body", () =>
            {
                //Create Parameters Array
                var stepParameters = new List<Allure.Net.Commons.Parameter>
                {
                    new() { name = "Body", value = request.Body },
                };
                //Add Parameters Array To Step Information
                AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));              
            });

            AllureApi.Step($"Step 2 : Call {GlobalTestSetUp.EnsekTestAPI.BaseUrl + request.Url} with body", () =>
            {
                response = GlobalTestSetUp.EnsekTestAPI.Login(request);
                AllureApi.AddAttachment("Raw Response", "application/json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response, Formatting.Indented)), "json");
                loginResponse = JsonConvert.DeserializeObject<LoginResponseModel>(response.Content);
                Assert.That(response, Is.Not.Null, "Response is not null");
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
                    new() { name = "Actual", value = loginResponse.Message },
                };
                AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));

                Assert.That(loginResponse.Message, Is.EqualTo(expectedResults.Message));
            });
            
            AllureApi.Step($"ASSERT: Access Token {isOrIsNot} returned", () =>
            {
                using (Assert.EnterMultipleScope())
                {
                    if (expectedResults.AccessToken)
                    {
                        var stepParameters = new List<Allure.Net.Commons.Parameter>
                        {
                            new() { name = "Expected", value = "296" },
                            new() { name = "Actual", value = loginResponse.AccessToken.Length.ToString() },
                        };
                        AllureLifecycle.Instance.UpdateStep(s => s.parameters.AddRange(stepParameters));
                        Assert.That(loginResponse.AccessToken, Is.Not.Null);
                        Assert.That(loginResponse.AccessToken.Length, Is.EqualTo(296));
                    }
                    else
                    {
                        var stepParameters = new List<Allure.Net.Commons.Parameter>
                        {
                            new() { name = "Expected", value = "null" },
                            new() { name = "Actual", value = loginResponse.AccessToken },
                        };
                        Assert.That(loginResponse.AccessToken, Is.Null);
                    }                       
                }
            });
        }        
    }
}