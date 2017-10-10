using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CustomerWebApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace CustomerWebApi.IntegrationTest.Controllers
{
    public class CustomersControllerTests
    {
        private TestServer TestServer { get; }
        private HttpClient HttpClient { get; }

        public CustomersControllerTests()
        {
            TestServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            HttpClient = TestServer.CreateClient();

            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Fact]
        public async Task Customers_Get_with_empty_db_returns_correct_result()
        {
            // Act
            var response = await HttpClient.GetAsync("/api/customers");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(responseString);
            customers.Count().Should().Be(0);
        }
    }
}
