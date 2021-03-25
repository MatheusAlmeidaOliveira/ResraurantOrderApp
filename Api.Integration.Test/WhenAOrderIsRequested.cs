using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Entities;
using Newtonsoft.Json;
using Xunit;

namespace Api.Integration.Test
{
    public class WhenAOrderIsRequested : BaseIntegration
    {
        [Fact]
        public async Task ItIsPossibleToMakeAPostAndGetOfOrder()
        {
            var orderDto = new OrderDto
            {
                Input = "night, 1, 2, 3"
            };

            var response = await PostJsonAsync(orderDto, hostApi, client);
            var postResult = await response.Content.ReadAsStringAsync();
            var postRecord = JsonConvert.DeserializeObject<OrderEntity>(postResult);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(orderDto.Input, postRecord.Input);
            Assert.NotNull(postRecord.Output);
            Assert.True(postRecord.Id != default(Guid));

            response = await client.GetAsync(hostApi);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var listFromJson = JsonConvert.DeserializeObject<IEnumerable<OrderEntity>>(jsonResult);
            Assert.NotNull(listFromJson);
            Assert.True(listFromJson.Count() > 0);
            Assert.True(listFromJson.Where(record => record.Id == postRecord.Id).Count() == 1);
        }
    }
}
