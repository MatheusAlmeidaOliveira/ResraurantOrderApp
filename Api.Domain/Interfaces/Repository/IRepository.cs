using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;

namespace Api.Domain.Interfaces.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> InsertAsync(T item);
        Task<IEnumerable<T>> SelectAsync();
    }
}