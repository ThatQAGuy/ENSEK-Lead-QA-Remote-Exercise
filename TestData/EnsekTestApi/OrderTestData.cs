using ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi;
using RestSharp;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.ExpectedResultsModel;
using System.Net;

namespace ENSEK_Lead_QA_Remote_Exercise.TestData.EnsekTestApi
{
    public class OrderTestData
    {

        public static IEnumerable<TestCaseData> OrderVolumeTestData
        {
            get
            {
                yield return new TestCaseData(
                    new RequestModel
                    {
                        Url = "orders",
                        AccessToken = true,
                        HttpMethod = Method.Get,
                    },
                    new OrderExpectedResultsModel
                    {
                        StatusCode = HttpStatusCode.OK,
                        HistoricOrderCount = 5,
                    }
                ).SetName("Orders: Confirm 5 Historic Orders");
            }
        }
    }
}
