using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Application.Controllers;
using Api.Domain.Dtos;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test
{
    public class OrderControllerTest
    {
        private OrderController _controller;
        private Mock<IOrderService> _serviceMock;
        private OrderEntity _orderEntity = new OrderEntity
        {
            Id = Guid.NewGuid(),
            Input = "morning, 1, 2, 3",
            Output = "eggs, toast, coffee",
            CreateAt = DateTime.UtcNow
        };
        private OrderDto _orderDto = new OrderDto
        {
            Input = "morning, 1, 2, 3"
        };
        public OrderControllerTest()
        {
            _serviceMock = new Mock<IOrderService>();
            _controller = new OrderController(_serviceMock.Object);
        }

        [Fact]
        public async Task WhenPostGetsCallWithAValidRequisition_ItShouldReturnCreated()
        {
            _serviceMock.Setup(servi => servi.Post(It.IsAny<OrderDto>())).ReturnsAsync(_orderEntity);

            var result = await _controller.Post(_orderDto);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as OrderEntity;
            Assert.NotNull(resultValue);
            Assert.Equal(_orderDto.Input, resultValue.Input);
        }

        [Fact]
        public async Task WhenPostGetsCallWithAnInvalidRequisition_ItShouldReturnABadRequest()
        {
            _serviceMock.Setup(servi => servi.Post(It.IsAny<OrderDto>())).ReturnsAsync(_orderEntity);

            _controller.ModelState.AddModelError("Input", "It's a require field");

            var result = await _controller.Post(_orderDto);
            Assert.True(result is BadRequestObjectResult);
        }

        [Fact]
        public async Task WhenGetAllGetsCallWithAValidRequisition_ItShouldReturnOk()
        {
            List<OrderEntity> orderList = MakesAListOfOrderEntity();
            _serviceMock.Setup(servi => servi.GetAll()).ReturnsAsync(orderList);

            var result = await _controller.GetAll();
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as IEnumerable<OrderEntity>;
            Assert.True(resultValue.Count() == 4);
        }

        [Fact]
        public async Task WhenGetAllGetsCallWithAnInvalidRequisition_ItShouldReturnBadRequest()
        {
            List<OrderEntity> orderList = MakesAListOfOrderEntity();
            _serviceMock.Setup(servi => servi.GetAll()).ReturnsAsync(orderList);
            _controller.ModelState.AddModelError("Id", "Invalid Format");

            var result = await _controller.GetAll();
            Assert.True(result is BadRequestObjectResult);
        }

        private static List<OrderEntity> MakesAListOfOrderEntity()
        {
            List<OrderEntity> orderList = new List<OrderEntity>
            {
                new OrderEntity{Input = "morning, 3, 3, 3, 3, 3, 3, 3", Output = "coffee(x7)"},
                new OrderEntity{Input = "morning, 1, 2, 3, 4", Output = "eggs, toast, coffee, error"},
                new OrderEntity{Input = "night, 1, 2, 3, 4", Output = "steak, potato, wine, cake"},
                new OrderEntity{Input = "night, 1, 2, 3, 5", Output = "steak, potato, wine, error"}
            };

            return orderList;
        }
    }
}
