using Allure.Net.Commons;
using Newtonsoft.Json;

namespace ENSEK_Lead_QA_Remote_Exercise
{
    public static class Utils
    {
        public static string SerializeRequestBodyIgnoreNulls(object requestBody)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };
            return JsonConvert.SerializeObject(requestBody, settings);
        }
    }
}