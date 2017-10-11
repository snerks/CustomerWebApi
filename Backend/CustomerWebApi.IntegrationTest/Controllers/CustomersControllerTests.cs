using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CustomerWebApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using Xunit;

namespace CustomerWebApi.IntegrationTest.Controllers
{
    public class CustomersControllerTests
    {
        private TestServer TestServer { get; }

        public CustomersControllerTests()
        {
            TestServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        }

        private HttpClient GetHttpClient()
        {
            var result = TestServer.CreateClient();

            result.DefaultRequestHeaders.Accept.Clear();
            result.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return result;
        }

        [Fact]
        public async Task Customers_Get_with_empty_db_returns_correct_result()
        {
            // Arrange
            using (var httpClient = GetHttpClient())
            {
                // Act
                using (HttpResponseMessage response = await httpClient.GetAsync("/api/customers"))
                {
                    // Assert
                    response.EnsureSuccessStatusCode();
                    // var responseString = await response.Content.ReadAsStringAsync();
                    // var customers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(responseString);
                    // customers.Count().Should().Be(0);
                }
            }
        }

        // [Fact]
        // public async Task Customer_Post_with_valid_item_succeeds()
        // {
        //     // Arrange
        //     var fixture = new Fixture();

        //     var customerToAdd = fixture.Build<Customer>().Without(c => c.Id).Create();

        //     var content = JsonConvert.SerializeObject(customerToAdd);
        //     var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

        //     // Act
        //     var response = await HttpClient.PostAsync("/api/customers", stringContent);

        //     // Assert
        //     response.EnsureSuccessStatusCode();
        //     response.StatusCode.Should().Be(HttpStatusCode.Created);
        //     response.Headers.Location.Should().Be("http://localhost/api/Customers?id=1");

        //     var responseString = await response.Content.ReadAsStringAsync();
        //     var person = JsonConvert.DeserializeObject<Customer>(responseString);
        //     person.Id.Should().Be(1);
        // }
    }
}
