using Microsoft.AspNetCore.Mvc;
using System;
using CustomerWebApi.Services;
using System.Linq;

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

        public IActionResult Get()
        {
            var result = CustomerService.GetAll();
            return Ok(result);
        }
    }
}
