using WebApiGitHubTrain.Dto;
using WebApiGitHubTrain.Models;

namespace WebApiGitHubTrain.Repositories
{
    public interface IOrder
    {
        public Task<List<Order>> GetAllOrders();
        public Task<OrderDetailsDto> GetOrderDetails(int id);
        public Task<OrderDetailsDto> MakeOrder(MakeOrderDto model);
        public Task<OrderDetailsDto> AddToOrder(int id, MakeOrderDto model);
        public Task<OrderDetailsDto> UpdateOrder(int id, ProductInOrder model);
        public Task<OrderDetailsDto> DeleteItemInOrder(int id);
        public Task<bool> DeleteOrder(int id);
    }
}
