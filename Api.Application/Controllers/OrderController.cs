using System;
using System.Net;
using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private IOrderService _service;
        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] OrderDto order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.Post(order);
                if (result == null)
                    return BadRequest();

                return Created("GetAll", result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Ok(await _service.GetAll());
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
