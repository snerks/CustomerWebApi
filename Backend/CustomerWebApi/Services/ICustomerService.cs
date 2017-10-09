using System.Collections.Generic;
using CustomerWebApi.Models;

namespace CustomerWebApi.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();
        // Person Get(int id);
        // Person Add(Person person);
        // void Update(int id, Person person);
        // void Delete(int id);
    }
}
