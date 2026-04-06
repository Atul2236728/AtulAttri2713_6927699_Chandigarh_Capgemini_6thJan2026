using EcommerceAPI.Models;

namespace EcommerceAPI.Services
{
    public interface IOrderService
    {
        Task PlaceOrder(Order order);
    }
}