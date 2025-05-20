using RestSharp;

namespace ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi
{
    public class RequestModel
    {
        public Method HttpMethod;
        public string Url;
        public string Body;
        public bool AccessToken;
        public Dictionary<string, string> UrlSegments;
        public Dictionary<string, string> QueryParameters;
    }
}