using Xunit;
using Moq;
using System.Threading.Tasks;
using EcommerceAPI.Services;
using EcommerceAPI.Repositories;
using EcommerceAPI.Models;

namespace EcommerceAPI.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _repoMock;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _repoMock = new Mock<IOrderRepository>();
            _service = new OrderService(_repoMock.Object);
        }

        [Fact]
        public async Task PlaceOrder_Should_Call_Repository()
        {
            // Arrange
            var order = new Order { UserId = 1 };

            // Act
            await _service.PlaceOrder(order);

            // Assert
            _repoMock.Verify(r => r.Add(order), Times.Once);
        }
    }
}