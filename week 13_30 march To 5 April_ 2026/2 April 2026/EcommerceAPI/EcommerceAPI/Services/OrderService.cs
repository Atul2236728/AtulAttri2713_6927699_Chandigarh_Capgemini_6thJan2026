using EcommerceAPI.Repositories;
using EcommerceAPI.Models;

namespace EcommerceAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task PlaceOrder(Order order)
        {
            order.OrderDate = DateTime.Now;
            await _repo.Add(order);
        }
    }
}