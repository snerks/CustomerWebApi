using Microsoft.AspNetCore.Mvc;
using System;
using CustomerWebApi.Services;
using System.Linq;
using CustomerWebApi.Models;

namespace CustomerWebApi.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        public CustomersController(ICustomerService customerService)
        {
            CustomerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        }

        public ICustomerService CustomerService { get; }

        [HttpGet]
        public IActionResult Get()
        {
            var result = CustomerService.GetAll();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = CustomerService.Add(customer);

            return CreatedAtAction("Get", new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                CustomerService.Update(id, customer);
            }
            catch (Exception ex)
            {
                // return NoContent();
                return StatusCode(500, ex.Message);
            }

            return NoContent();
        }
    }
}
