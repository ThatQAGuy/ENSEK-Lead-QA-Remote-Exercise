using ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi;
using RestSharp;

namespace ENSEK_Lead_QA_Remote_Exercise.Services.Interfaces
{
    public interface IEnsekTestAPI
    {
        string BaseUrl { get; set; }

        RestResponse Buy(RequestModel requestModel);

        RestResponse GetCurrentEnergyStock(RequestModel requestModel);

        RestResponse Login(RequestModel requestModel);

        RestResponse GetAllOrders(RequestModel requestModel);

        RestResponse DeleteOrderById(RequestModel requestModel);

        RestResponse UpdateOrderById(RequestModel requestModel);

        RestResponse GetOrderById(RequestModel requestModel);

        RestResponse ResetSystem(RequestModel requestModel);
    }
}