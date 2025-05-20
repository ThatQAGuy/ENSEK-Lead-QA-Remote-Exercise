using RestSharp;
using RestSharp.Authenticators;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.DataModels;
using Newtonsoft.Json;
using static ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi.ResponseModels;
using ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi;
using ENSEK_Lead_QA_Remote_Exercise.Services.Interfaces;
using ENSEK_Lead_QA_Remote_Exercise.Tests.EnsekTestApi;

namespace ENSEK_Lead_QA_Remote_Exercise.Services
{
    public class EnsekTestAPI : IEnsekTestAPI
    {
        public string BaseUrl { get; set ; }

        public EnsekTestAPI()
        {
            BaseUrl = "https://qacandidatetest.ensek.io/ENSEK/";
        }

        private RestResponse Request(RequestModel requestModel)
        {
            var options = new RestClientOptions(BaseUrl);
            if (requestModel.AccessToken) 
            {
                var accessTokenRequest = new RequestModel
                {
                    Url = "login",
                    Body = Utils.SerializeRequestBodyIgnoreNulls(requestBody: new LoginModel { Username = "test", Password = "testing" }),
                    HttpMethod = Method.Post
                };
                var accessTokenResponse = GlobalTestSetUp.EnsekTestAPI.Login(accessTokenRequest);
                LoginResponseModel loginResponse = JsonConvert.DeserializeObject<LoginResponseModel>(accessTokenResponse.Content);
                options.Authenticator = new JwtAuthenticator(loginResponse.AccessToken);
            }            

            var client = new RestClient(options);
            client.AddDefaultHeader("Content-Type", "application/json");
            client.AddDefaultHeader("Accept", "application/json");

            RestRequest request = new RestRequest(requestModel.Url, requestModel.HttpMethod);
            if (requestModel.UrlSegments != null)
            {
                foreach (var segment in requestModel.UrlSegments)
                {
                    request.AddUrlSegment(segment.Key, segment.Value);
                }
            }
            if (requestModel.QueryParameters != null)
            {
                foreach (var query in requestModel.QueryParameters)
                {
                    request.AddQueryParameter(query.Key, query.Value);
                }
            }
            if (!string.IsNullOrEmpty(requestModel.Body))
            {
                request.AddJsonBody(requestModel.Body);
            }

            var response = client.Execute(request);

            return response;
        }

        public RestResponse Buy(RequestModel requestModel) => Request(requestModel);

        public RestResponse GetCurrentEnergyStock(RequestModel requestModel) => Request(requestModel);

        public RestResponse Login(RequestModel requestModel) => Request(requestModel);

        public RestResponse GetAllOrders(RequestModel requestModel) => Request(requestModel);

        public RestResponse DeleteOrderById(RequestModel requestModel) => Request(requestModel);

        public RestResponse UpdateOrderById(RequestModel requestModel) => Request(requestModel);

        public RestResponse GetOrderById(RequestModel requestModel) => Request(requestModel);

        public RestResponse ResetSystem(RequestModel requestModel) => Request(requestModel);
    }
}