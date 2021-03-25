using System;
using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Repository;
using Api.Domain.Interfaces.Services;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test
{
    public class OrderServiceTest
    {
        private IOrderService _service;
        private Mock<IRepository<OrderEntity>> _repositoryMock;
        private OrderDto _orderDto = new OrderDto();

        public OrderServiceTest()
        {
            _repositoryMock = new Mock<IRepository<OrderEntity>>();
            _service = new OrderService(_repositoryMock.Object);
        }

        [Theory]
        [InlineData("morning, 1", "eggs")]
        [InlineData("morning, 2", "toast")]
        [InlineData("morning, 3", "coffee")]
        [InlineData("morning, 3, 3, 3, 3, 3, 3, 3", "coffee(x7)")]
        [InlineData("morning, 4", "error")]
        [InlineData("morning, 1, 2, 3", "eggs, toast, coffee")]
        [InlineData("morning, 2, 1, 3", "eggs, toast, coffee")]
        [InlineData("morning, 1, 2, 3, 4", "eggs, toast, coffee, error")]
        [InlineData("morning, 1, 2, 3, 3, 3", "eggs, toast, coffee(x3)")]
        [InlineData("night, 1", "steak")]
        [InlineData("night, 2", "potato")]
        [InlineData("night, 2, 2, 2, 2", "potato(x4)")]
        [InlineData("night, 3", "wine")]
        [InlineData("night, 4", "cake")]
        [InlineData("night, 1, 2, 3, 4", "steak, potato, wine, cake")]
        [InlineData("night, 1, 2, 2, 4", "steak, potato(x2), cake")]
        [InlineData("night, 1, 2, 3, 5", "steak, potato, wine, error")]
        [InlineData("night, 1, 1, 2, 3, 5", "steak, error")]
        public async Task WhenTheInputIsInTheRightFormat_TheOutputShouldReturnTheCorrectOne(string input, string expectedOutput)
        {
            _orderDto.Input = input;

            var result = await _service.Post(_orderDto);
            _repositoryMock.Verify(repository => repository.InsertAsync(It.Is<OrderEntity>(order => order.Output == expectedOutput)));
        }

        [Theory]
        [InlineData("afternoon, 1")]
        [InlineData(" , 1, 2, 3")]
        public async Task WhenPeriodOfTheDayIsInvalid_ItShouldThrowAnException(string input)
        {
            try
            {
                _orderDto.Input = input;

                var result = await _service.Post(_orderDto);
            }
            catch (Exception ex)
            {
                string message = "Invalid period of the day";
                Assert.Equal(message, ex.Message);
            }
        }

        [Fact]
        public async Task WhenTheOrderIsEmpyt_ItShouldThrowAnException()
        {
            try
            {
                _orderDto.Input = "morning";
                
                var result = await _service.Post(_orderDto);
            }
            catch (Exception ex)
            {
                string message = "The order doesn't have any meal's orders";
                Assert.Equal(message, ex.Message);
            }
        }
    }
}
