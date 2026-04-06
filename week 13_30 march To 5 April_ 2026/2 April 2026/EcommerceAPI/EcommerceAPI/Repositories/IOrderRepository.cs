using EcommerceAPI.Models;

namespace EcommerceAPI.Repositories
{
    public interface IOrderRepository
    {
        Task Add(Order order);
        Task<List<Order>> GetAll();
    }
}