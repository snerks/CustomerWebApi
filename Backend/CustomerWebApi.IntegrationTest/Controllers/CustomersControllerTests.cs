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
            result.BaseAddress.Should().Be("http://localhost/");

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
                    var responseString = await response.Content.ReadAsStringAsync();
                    responseString.Should().Be("[]");
                    var customers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(responseString);
                    customers.Count().Should().Be(0);
                }
            }
        }

        [Fact]
        public async Task Customer_Post_with_valid_item_succeeds()
        {
            // Arrange
            var fixture = new Fixture();

            var customerToAdd = fixture.Build<Customer>().Without(c => c.Id).Create();

            var content = JsonConvert.SerializeObject(customerToAdd);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            using (var httpClient = GetHttpClient())
            {
                // Act
                using (HttpResponseMessage response = await httpClient.PostAsync("/api/customers", stringContent))
                {
                    // Assert
                    response.EnsureSuccessStatusCode();
                    response.StatusCode.Should().Be(HttpStatusCode.Created);
                    response.Headers.Location.Should().Be("http://localhost/api/Customers?id=1");

                    var responseString = await response.Content.ReadAsStringAsync();
                    var person = JsonConvert.DeserializeObject<Customer>(responseString);
                    person.Id.Should().Be(1);
                }
            }
        }

        [Fact]
        public async Task Customer_Post_with_invalid_item_returns_correct_result()
        {
            // Arrange
            var fixture = new Fixture();
            var customerToAdd =
                fixture.Build<Customer>()
                .Without(c => c.Id)
                .Without(c => c.Name)
                .Create();

            var content = JsonConvert.SerializeObject(customerToAdd);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            using (var httpClient = GetHttpClient())
            {
                // Act
                using (HttpResponseMessage response = await httpClient.PostAsync("/api/customers", stringContent))
                {
                    // Assert
                    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

                    var responseString = await response.Content.ReadAsStringAsync();
                    responseString.Should().Contain("The Name field is required");
                }
            }
        }

        [Fact]
        public async Task Customer_Put_with_valid_item_returns_correct_result()
        {
            // Arrange
            var fixture = new Fixture();

            var customerToAdd = fixture.Build<Customer>().Without(c => c.Id).Create();
            var customerToAddContent = JsonConvert.SerializeObject(customerToAdd);
            var customerToAddStringContent = new StringContent(customerToAddContent, Encoding.UTF8, "application/json");

            var customerToUpdate = fixture.Build<Customer>().With(c => c.Id, 1).Create();
            var customerToUpdateContent = JsonConvert.SerializeObject(customerToUpdate);
            var customerToUpdateStringContent = new StringContent(customerToUpdateContent, Encoding.UTF8, "application/json");

            using (var httpClient = GetHttpClient())
            {
                // Act
                using (HttpResponseMessage response = await httpClient.PostAsync("/api/customers", customerToAddStringContent))
                {
                    // Assert
                    response.EnsureSuccessStatusCode();
                    response.StatusCode.Should().Be(HttpStatusCode.Created);
                    response.Headers.Location.Should().Be("http://localhost/api/Customers?id=1");

                    var responseString = await response.Content.ReadAsStringAsync();
                    var person = JsonConvert.DeserializeObject<Customer>(responseString);
                    person.Id.Should().Be(1);
                }

                using (HttpResponseMessage response = await httpClient.PutAsync("/api/customers/1", customerToUpdateStringContent))
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    responseString.Should().Be("");

                    response.EnsureSuccessStatusCode();
                    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
                }
            }
        }
    }
}
