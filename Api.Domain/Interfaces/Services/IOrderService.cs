using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Entities;

namespace Api.Domain.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderEntity> Post(OrderDto order);
        Task<IEnumerable<OrderEntity>> GetAll();
    }
}