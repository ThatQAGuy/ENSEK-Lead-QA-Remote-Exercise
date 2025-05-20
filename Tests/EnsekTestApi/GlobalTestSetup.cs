using ENSEK_Lead_QA_Remote_Exercise.Services;

namespace ENSEK_Lead_QA_Remote_Exercise.Tests.EnsekTestApi
{
    [SetUpFixture]
    public class GlobalTestSetUp
    {
        public static EnsekTestAPI EnsekTestAPI { get; private set; }

        [OneTimeSetUp]
        public void GlobalTestSetup()
        {
            EnsekTestAPI = new EnsekTestAPI();
        }

        [OneTimeTearDown]
        public void GlobalTestTearDown()
        {
            
        }
    }
}