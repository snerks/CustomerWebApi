using System;
using System.Linq;
using CustomerWebApi.DbContexts;
using CustomerWebApi.Models;
using CustomerWebApi.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Xunit;

namespace CustomerWebApi.UnitTest.Services
{
    public class CustomerServiceTests
    {
        public class Constructor
        {
            private IFixture GetFixture()
            {
                return new Fixture().Customize(new AutoMoqCustomization());
            }

            private readonly DbContextOptions<CustomerDbContext> dbContextOptions;

            public Constructor()
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
                dbContextOptionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
                dbContextOptions = dbContextOptionsBuilder.Options;
            }

            [Fact]
            public void Constructor_succeeds()
            {
                using (var customerDbContext = new CustomerDbContext(dbContextOptions))
                {
                    // Arrange
                    var fixture = GetFixture();

                    fixture.Register<CustomerDbContext>(() => customerDbContext);

                    // Act
                    Action action = () => fixture.Create<CustomerService>();

                    // Assert
                    action.ShouldNotThrow();
                }
            }
        }

        public class GetAll
        {
            private IFixture GetFixture()
            {
                return new Fixture().Customize(new AutoMoqCustomization());
            }

            private readonly DbContextOptions<CustomerDbContext> dbContextOptions;

            public GetAll()
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
                dbContextOptionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
                dbContextOptions = dbContextOptionsBuilder.Options;
            }

            [Fact]
            public void Returns_non_null()
            {
                using (var customerDbContext = new CustomerDbContext(dbContextOptions))
                {
                    // Arrange
                    var fixture = GetFixture();
                    fixture.Register<CustomerDbContext>(() => customerDbContext);

                    var sut = fixture.Create<CustomerService>();

                    // Act
                    var result = sut.GetAll();

                    // Assert
                    result.Should().NotBeNull();
                }
            }

            [Fact]
            public void Returns_all_items()
            {
                // Arrange
                var fixture = GetFixture();

                var expectedCustomerItemCount = fixture.Create<int>();
                fixture.RepeatCount = expectedCustomerItemCount;
                var expectedCustomerItems = fixture.CreateMany<Customer>();

                using (var customerDbContext = new CustomerDbContext(dbContextOptions))
                {
                    customerDbContext.Customers.AddRange(expectedCustomerItems);
                    customerDbContext.SaveChanges();

                    fixture.Register<CustomerDbContext>(() => customerDbContext);
                    var sut = fixture.Create<CustomerService>();

                    // Act
                    var result = sut.GetAll();

                    // Assert
                    result.Count().Should().Be(expectedCustomerItemCount);
                    result.ShouldAllBeEquivalentTo(expectedCustomerItems);
                }
            }
        }
    }
}
