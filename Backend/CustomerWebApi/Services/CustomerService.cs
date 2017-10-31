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

        public Customer Add(Customer customer)
        {
            CustomerDbContext.Customers.Add(customer);
            CustomerDbContext.SaveChanges();

            return customer;
        }

        public void Update(int id, Customer customer)
        {
            var existing = CustomerDbContext.Customers.FirstOrDefault(_ => _.Id == id);

            // if (existing == null)
            // {
            //     return;
            // }

            existing.Name = customer.Name;
        }

        // void Delete(int id);
    }
}
