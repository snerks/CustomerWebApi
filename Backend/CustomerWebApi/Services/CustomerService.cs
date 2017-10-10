using System;
using System.Collections.Generic;
using System.Linq;
using CustomerWebApi.DbContexts;
using CustomerWebApi.Models;

namespace CustomerWebApi.Services
{
    public class CustomerService : ICustomerService
    {
        public CustomerService(CustomerDbContext customerDbContext)
        {
            CustomerDbContext = customerDbContext ?? throw new ArgumentNullException(nameof(customerDbContext));
        }

        public CustomerDbContext CustomerDbContext { get; }

        public IEnumerable<Customer> GetAll()
        {
            return CustomerDbContext.Customers.ToList();
        }

        // Customer Get(int id);
        // Customer Add(Customer customer);
        // void Update(int id, Customer customer);
        // void Delete(int id);
    }
}
