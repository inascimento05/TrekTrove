using FluentAssertions;
using Moq;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.GetAll;
using TrekTrove.Api.Modules.CustomerModule.Domain.Entities;
using TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces;
using Xunit;

namespace TrekTrove.Api.Modules.CustomerModule.Tests.Application.Mediators.CustomerOperations.GetAll
{
    public class GetAllCustomersHandlerTests
    {
        [Fact]
        public async Task Handle_ValidInput_ReturnsEmptyCustomersList()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ICustomerService>();
            var getAllCustomerHandler = new GetAllCustomersHandler(TemplateServiceMock.Object);
            var expectedCustomer = new List<Customer>();

            TemplateServiceMock.Setup(service => service.GetAllCustomersAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedCustomer);

            var request = new GetAllCustomersRequest(1, 10);

            // Act
            var response = await getAllCustomerHandler.Handle(request, default);

            // Assert
            response.Valid.Should().BeTrue();
            response.Invalid.Should().BeFalse();
            response.Notifications.Count.Should().Be(0);
            response.Data.Count().Should().Be(0);

            // Verify
            TemplateServiceMock.Verify(service => service.GetAllCustomersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidInput_ReturnsAllCustomers()
        {
            // Arrange
            var expectedCustomer = new List<Customer> {
                new Customer {
                    Id = 1,
                    Name = "Test Customer",
                    Description = "Test Description"
                },
                new Customer {
                    Id = 2,
                    Name = "Test Customer 2",
                    Description = "Test Description 2"
                }
            };

            var TemplateServiceMock = new Mock<ICustomerService>();
            TemplateServiceMock.Setup(service => service.GetAllCustomersAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedCustomer);

            var getAllCustomerHandler = new GetAllCustomersHandler(TemplateServiceMock.Object);
            var request = new GetAllCustomersRequest(1, 10);

            // Act
            var response = await getAllCustomerHandler.Handle(request, default);

            // Assert
            response.Valid.Should().BeTrue();
            response.Invalid.Should().BeFalse();
            response.Notifications.Count.Should().Be(0);
            response.Data.Count().Should().Be(2);

            // Verify
            TemplateServiceMock.Verify(service => service.GetAllCustomersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(GetAllCustomersRequest_ValidPaginationInputsToTest))]
        public async Task Handle_ValidPaginationInput_ReturnsCustomersPaginated(int page)
        {
            // Arrange
            var TemplatesFromDatabase = new List<Customer> {
                new Customer
                {
                    Id = 1,
                    Name = "Test Customer",
                    Description = "Test Description"
                },
                new Customer
                {
                    Id = 2,
                    Name = "Test Customer 2",
                    Description = "Test Description 2"
                },
            };
            var expectedCustomer = TemplatesFromDatabase.Skip(page - 1)
                .Take(1);

            var TemplateServiceMock = new Mock<ICustomerService>();
            TemplateServiceMock.Setup(service => service.GetAllCustomersAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedCustomer);

            var getAllCustomerHandler = new GetAllCustomersHandler(TemplateServiceMock.Object);
            var request = new GetAllCustomersRequest(page, 1);

            // Act
            var response = await getAllCustomerHandler.Handle(request, default);

            // Assert
            response.Valid.Should().BeTrue();
            response.Invalid.Should().BeFalse();
            response.Notifications.Count.Should().Be(0);
            response.Data.Count().Should().Be(1);
            response.Data.First().Id.Should().Be(TemplatesFromDatabase[(page - 1)].Id);
            response.Data.First().Name.Should().Be(TemplatesFromDatabase[(page - 1)].Name);
            response.Data.First().Description.Should().Be(TemplatesFromDatabase[(page - 1)].Description);

            // Verify
            TemplateServiceMock.Verify(service => service.GetAllCustomersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        public static IEnumerable<object[]> GetAllCustomersRequest_ValidPaginationInputsToTest =>
        new List<object[]> {
            new object[] { 1 },
                new object[] { 2 }
            };

        [Theory]
        [MemberData(nameof(GetAllCustomersRequest_ZeroInputsToTest))]
        public async Task Handle_ZeroInput_ReturnsCustomersPaginated(GetAllCustomersRequest request)
        {
            // Arrange
            var TemplatesFromDatabase = new List<Customer>();
            for (int i = 0; i < request.PageSize; i++)
            {
                TemplatesFromDatabase.Add(
                    new Customer
                    {
                        Id = i,
                        Name = $"Test Customer {i}",
                        Description = $"Test Description {i}"
                    }
                );
            }

            var TemplateServiceMock = new Mock<ICustomerService>();
            TemplateServiceMock.Setup(service => service.GetAllCustomersAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(TemplatesFromDatabase);

            var getAllCustomerHandler = new GetAllCustomersHandler(TemplateServiceMock.Object);

            // Act
            var result = await getAllCustomerHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeTrue();
            result.Invalid.Should().BeFalse();
            result.Notifications.Count.Should().Be(0);
            result.Data.Count().Should().BeLessThanOrEqualTo(request.PageSize);

            // Verify
            TemplateServiceMock.Verify(service => service.GetAllCustomersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
        public static IEnumerable<object[]> GetAllCustomersRequest_ZeroInputsToTest =>
            new List<object[]> {
                new object[] { new GetAllCustomersRequest(0, 10) },
                new object[] { new GetAllCustomersRequest(1, 0) },
                new object[] { new GetAllCustomersRequest(0, 0) }
            };
    }
}
