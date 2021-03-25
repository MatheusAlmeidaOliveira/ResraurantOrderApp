using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data.Context;
using Api.Data.Repository;
using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.Data.Test
{
    public class BaseRepositoryTest
    {
        [Fact]
        public async Task WhenOrdersAreInsertInTheDataBase_ItShouldReturnTheSameOrders()
        {
            var options = new DbContextOptionsBuilder<MyContext>().UseInMemoryDatabase("DbTestContext").Options;
            var context = new MyContext(options);
            var repository = new BaseRepository<OrderEntity>(context);

            List<OrderEntity> orderList = FillTheList();
            await InsertTheListInTheDataBase(repository, orderList);

            IEnumerable<OrderEntity> result = await repository.SelectAsync();
            OrderEntity[] resultList = result.ToArray();

            for (int i = 0; i < resultList.Count(); i++)
            {
                Assert.NotNull(resultList[i]);
                Assert.Equal(resultList[i].Input, orderList[i].Input);
                Assert.Equal(resultList[i].Output, orderList[i].Output);
            }
        }

        private static async Task InsertTheListInTheDataBase(BaseRepository<OrderEntity> repository, List<OrderEntity> orderList)
        {
            foreach (var item in orderList)
                await repository.InsertAsync(item);
        }

        public static List<OrderEntity> FillTheList()
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
