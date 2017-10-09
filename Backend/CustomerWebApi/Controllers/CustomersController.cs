using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using CustomerWebApi.Models;
using CustomerWebApi.Services;

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
            return Ok(CustomerService.GetAll());
        }
    }
}
