using FluentAssertions;
using Moq;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Create;
using TrekTrove.Api.Modules.CustomerModule.Domain.Entities;
using TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces;
using Xunit;

namespace TrekTrove.Api.Modules.CustomerModule.Tests.Application.Mediators.CustomerOperations.Create
{
    public class CreateCustomerHandlerTests
    {
        [Fact]
        public async Task Handle_ValidInput_ReturnsCreatedCustomer()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ICustomerService>();
            var createCustomerRequestHandler = new CreateCustomerHandler(TemplateServiceMock.Object);
            var expectedCreatedCustomerId = 1;
            TemplateServiceMock.Setup(service => service.CreateCustomerAsync(It.IsAny<Customer>()))
                .ReturnsAsync(expectedCreatedCustomerId);

            var request = new CreateCustomerRequest("Test Customer", "Test Description");

            // Act
            var result = await createCustomerRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeTrue();
            result.Invalid.Should().BeFalse();
            result.Notifications.Count.Should().Be(0);
            result.Data.Id.Should().Be(expectedCreatedCustomerId);

            // Verify
            TemplateServiceMock.Verify(service => service.CreateCustomerAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CustomerCreationFails_ReturnsUnprocessableEntityErrorCode()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ICustomerService>();
            var createCustomerRequestHandler = new CreateCustomerHandler(TemplateServiceMock.Object);

            TemplateServiceMock.Setup(service => service.CreateCustomerAsync(It.IsAny<Customer>()))
                .ReturnsAsync(0);

            var request = new CreateCustomerRequest("Test Customer", "Test Description");

            // Act
            var result = await createCustomerRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.UnprocessableEntity);

            // Verify
            TemplateServiceMock.Verify(service => service.CreateCustomerAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CustomerCreationFails_ReturnsInternalServerErrorErrorCode()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ICustomerService>();
            var createCustomerRequestHandler = new CreateCustomerHandler(TemplateServiceMock.Object);

            TemplateServiceMock.Setup(service => service.CreateCustomerAsync(It.IsAny<Customer>()))
                .ThrowsAsync(new Exception());

            var request = new CreateCustomerRequest("Test Customer", "Test Description");

            // Act
            var result = await createCustomerRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.InternalServerError);

            // Verify
            TemplateServiceMock.Verify(service => service.CreateCustomerAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(CreateCustomer_InvalidCreateCustomerRequestsToTest))]
        public async Task Handle_NullOrEmptyInput_ReturnsBadRequestErrorCode(CreateCustomerRequest request)
        {
            // Arrange
            var TemplateServiceMock = new Mock<ICustomerService>();
            var createCustomerRequestHandler = new CreateCustomerHandler(TemplateServiceMock.Object);

            // Act
            var result = await createCustomerRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.BadRequest);

            // Verify
            TemplateServiceMock.Verify(service => service.CreateCustomerAsync(It.IsAny<Customer>()), Times.Never);
        }

        public static IEnumerable<object[]> CreateCustomer_InvalidCreateCustomerRequestsToTest =>
           new List<object[]>
           {
                new object[] { null },
                new object[] { new CreateCustomerRequest(null, "Test Description") },
                new object[] { new CreateCustomerRequest("Test Customer", null) },
                new object[] { new CreateCustomerRequest(string.Empty, "Test Description") },
                new object[] { new CreateCustomerRequest("Test Customer", string.Empty) },
           };
    }
}
