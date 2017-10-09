using System.Collections.Generic;
using CustomerWebApi.Models;

namespace CustomerWebApi.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();
        // Customer Get(int id);
        // Customer Add(Customer customer);
        // void Update(int id, Customer customer);
        // void Delete(int id);
    }
}
