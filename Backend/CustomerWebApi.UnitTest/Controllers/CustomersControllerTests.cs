using System;
using CustomerWebApi.UnitTest.AutoFixture;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Xunit;
using FluentAssertions;
using CustomerWebApi.Controllers;
using CustomerWebApi.Models;
using Moq;
using CustomerWebApi.Services;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CustomerWebApi.UnitTest.Controllers
{
    public class CustomersControllerTests
    {
        private static IFixture GetFixture() => new Fixture()
                            .Customize(new AutoMoqCustomization())
                            .Customize(new MvcCoreControllerCustomization());

        public class Constructor
        {
            [Fact]
            public void Constructor_succeeds()
            {
                // Arrange
                var fixture = GetFixture();

                // Act
                Action action = () => fixture.Create<CustomersController>();

                // Assert
                action.ShouldNotThrow();
            }
        }

        public class Get
        {
            [Fact]
            public void Returns_non_null()
            {
                // Arrange
                var fixture = GetFixture();
                var sut = fixture.Create<CustomersController>();

                // Act
                var result = sut.Get();

                // Assert
                result.Should().NotBeNull();
            }

            [Fact]
            public void Returns_all_items()
            {
                // Arrange
                var fixture = GetFixture();

                var expectedCustomerItemCount = fixture.Create<int>();
                fixture.RepeatCount = expectedCustomerItemCount;
                var expectedCustomerItems = fixture.CreateMany<Customer>();

                var customerService = new Mock<ICustomerService>();
                customerService.Setup(m => m.GetAll()).Returns(expectedCustomerItems);
                fixture.Register<ICustomerService>(() => customerService.Object);

                var sut = fixture.Create<CustomersController>();

                // Act
                var result = sut.Get();

                // Assert
                var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
                var resultCustomerItems = okObjectResult.Value.Should().BeAssignableTo<IEnumerable<Customer>>().Subject;

                resultCustomerItems.Count().Should().Be(expectedCustomerItemCount);

                resultCustomerItems.ShouldAllBeEquivalentTo(expectedCustomerItems);
            }
        }
    }
}
