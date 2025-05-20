using ENSEK_Lead_QA_Remote_Exercise.Models.EnsekTestApi;

namespace ENSEK_Lead_QA_Remote_Exercise.TestData.EnsekTestApi
{
    internal class OrderRecord : DataModels.OrderModel
    {
        public int EnergyId { get; set; }
        public int Quantity { get; set; }
        public string UnitType { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal TotalPrice { get; set; }
    }
}