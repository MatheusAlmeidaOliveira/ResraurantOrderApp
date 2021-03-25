using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Repository;
using Api.Domain.Interfaces.Services;

namespace Api.Service.Services
{
    public class OrderService : IOrderService
    {
        private IRepository<OrderEntity> _repository;

        public OrderService(IRepository<OrderEntity> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<OrderEntity>> GetAll()
        {
            return await _repository.SelectAsync();
        }
        
        public async Task<OrderEntity> Post(OrderDto order)
        {
            List<string> resultList = new List<string>();
            const byte periodOfTheDay = 0;
            string[] formatedOrder = FormatsTheOrder(order);
            string result = ReturnsTheOutputResult(resultList, periodOfTheDay, formatedOrder);
            ChecksIfTheOrderIsEmpyt(result);

            var orderEntity = new OrderEntity
            {
                Input = order.Input,
                Output = result
            };

            return await _repository.InsertAsync(orderEntity);
        }

        private static void ChecksIfTheOrderIsEmpyt(string result)
        {
            if (String.IsNullOrWhiteSpace(result))
                throw new Exception("The order doesn't have any meal's orders");
        }

        private static string ReturnsTheOutputResult(List<string> resultList, byte periodOfTheDay, string[] formatedOrder)
        {
            BaseOrderService baseOrderService;
            if (ChecksPeriodOfTheDay(formatedOrder[periodOfTheDay]))
                baseOrderService = new MorningOrderService();
            else
                baseOrderService = new NightOrderService();    
                
            resultList = baseOrderService.CreatesTheOrderOutPut(formatedOrder);
            return String.Join(", ", resultList);
        }

        private static bool ChecksPeriodOfTheDay(string period)
        {
            if (ChecksInvalidPeriodOfTheDay(period))
                throw new Exception("Invalid period of the day");

            if(period == "morning")
                return true;

            return false;    
        }

        private static bool ChecksInvalidPeriodOfTheDay(string period)
        {
            return period != "morning" && period != "night";
        }

        private static string[] FormatsTheOrder(OrderDto order)
        {
            var orderSplit = order.Input.Replace(" ", "").Split(',');
            return orderSplit;
        }
    }
}