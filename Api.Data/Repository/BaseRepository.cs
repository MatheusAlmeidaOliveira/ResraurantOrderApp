using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Data.Context;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly MyContext _context;
        private DbSet<T> _dataset;
        public BaseRepository(MyContext context)
        {
            _context = context;
            _dataset = context.Set<T>();
        }

        public async Task<T> InsertAsync(T item)
        {
            try
            {
                if(item.Id == Guid.Empty)
                    item.Id = Guid.NewGuid();

                item.CreateAt = DateTime.UtcNow;
                _dataset.Add(item);

                await _context.SaveChangesAsync();

                return item;    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<T>> SelectAsync()
        {
            try
            {
                return await _dataset.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}