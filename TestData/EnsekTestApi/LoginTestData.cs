using ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi;
using RestSharp;
using System.Net;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.DataModels;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.ExpectedResultsModel;

namespace ENSEK_Lead_QA_Remote_Exercise.TestData.EnsekTestApi
{
    public class LoginTestData
    {
        public static IEnumerable<TestCaseData> LoginScenarios
        {
            get
            {
                yield return new TestCaseData(
                    new RequestModel
                    {
                        HttpMethod = Method.Post,
                        Url = "login",
                        Body = Utils.SerializeRequestBodyIgnoreNulls(requestBody: new LoginModel { Username = "test", Password = "testing" })
                    },
                    new LoginExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Success",
                        AccessToken = true
                    }
                ).SetName("Login : Valid Request");
                yield return new TestCaseData(
                    new RequestModel
                    {
                        HttpMethod = Method.Post,
                        Url = "login",
                        Body = Utils.SerializeRequestBodyIgnoreNulls(requestBody: new LoginModel { Username = "test" })
                    },
                    new LoginExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Bad Request",
                        AccessToken = false
                    }
                ).SetName("Login : Bad Request - Missing Password");
                yield return new TestCaseData(
                    new RequestModel
                    {
                        HttpMethod = Method.Post,
                        Url = "login",
                        Body = Utils.SerializeRequestBodyIgnoreNulls(requestBody: new LoginModel { Password = "testing" })
                    },
                    new LoginExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Bad Request",
                        AccessToken = false
                    }
                ).SetName("Login : Bad Request - Missing Username");
                yield return new TestCaseData(
                    new RequestModel
                    {
                        HttpMethod = Method.Post,
                        Url = "login",
                        Body = string.Empty
                    },
                    new LoginExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "The browser (or proxy) sent a request that this server could not understand.",
                        AccessToken = false
                    }
                ).SetName("Login : Bad Request - Missing Body");
                yield return new TestCaseData(
                    new RequestModel
                    {
                        HttpMethod = Method.Post,
                        Url = "login",
                        Body = Utils.SerializeRequestBodyIgnoreNulls(requestBody: new LoginModel { Username = "notarealaccount", Password = "notarealpassword" })
                    },
                    new LoginExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Message = "Unauthorized",
                        AccessToken = false
                    }
                ).SetName("Login : Unregistered User Request");
            }
        }
    }
}