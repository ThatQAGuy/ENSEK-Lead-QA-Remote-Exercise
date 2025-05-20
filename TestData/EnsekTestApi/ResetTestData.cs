using ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi;
using RestSharp;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.ExpectedResultsModel;
using System.Net;

namespace ENSEK_Lead_QA_Remote_Exercise.TestData.EnsekTestApi
{
    public class ResetTestData
    {
        public static IEnumerable<TestCaseData> AuthorizedUserScenarios
        {
            get
            {
                yield return new TestCaseData(
                    new RequestModel
                    {
                        Url = "reset",
                        AccessToken = true,
                        HttpMethod = Method.Post,
                    },
                    new ResetExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Success",
                    }
                ).SetName("Reset: Valid User");

                yield return new TestCaseData(
                    new RequestModel
                    {
                        Url = "reset",
                        AccessToken = false,
                        HttpMethod = Method.Post,
                    },
                    new ResetExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Message = "Unauthorized",
                    }
                ).SetName("Reset: Guest");
            }
        }



    }
}
