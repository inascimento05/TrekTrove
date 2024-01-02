using FluentAssertions;
using Moq;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Update;
using TrekTrove.Api.Modules.CustomerModule.Domain.Entities;
using TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces;
using Xunit;

namespace TrekTrove.Api.Modules.CustomerModule.Tests.Application.Mediators.CustomerOperations.Update
{
    public class UpdateCustomerHandlerTests {
        [Fact]
        public async Task Handle_ValidInput_ReturnUpdatedCustomer()
        {
            // Arrange
            var TemplateFromDatabase = new Customer
            {
                Id = 1,
                Name = "Test Customer",
                Description = "Test Description"
            };

            var TemplateToUpdate = new Customer
            {
                Id = TemplateFromDatabase.Id,
                Description = "Updated Test Description"
            };

            var expectedUpdatedCustomer = new Customer
            {
                Id = TemplateFromDatabase.Id,
                Name = TemplateFromDatabase.Name,
                Description = TemplateToUpdate.Description
            };

            var TemplateServiceMock = new Mock<ICustomerService>();
            TemplateServiceMock.Setup(service => service.UpdateCustomerAsync(It.IsAny<Customer>()))
                .ReturnsAsync(expectedUpdatedCustomer);

            var updateCustomerRequestHandler = new UpdateCustomerHandler(TemplateServiceMock.Object);
            var request = new UpdateCustomerRequest(TemplateToUpdate.Id, TemplateToUpdate.Name, TemplateToUpdate.Description);

            // Act
            var result = await updateCustomerRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeTrue();
            result.Invalid.Should().BeFalse();
            result.Notifications.Count.Should().Be(0);
            result.Data.Id.Should().Be(expectedUpdatedCustomer.Id);
            result.Data.Name.Should().Be(expectedUpdatedCustomer.Name);
            result.Data.Description.Should().Be(expectedUpdatedCustomer.Description);

            // Verify
            TemplateServiceMock.Verify(service => service.UpdateCustomerAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CustomerUpdateFails_ReturnsNotFoundErrorCode()
        {
            // Arrange
            var TemplateToUpdate = new Customer
            {
                Id = 1,
                Description = "Updated Test Description"
            };

            var TemplateServiceMock = new Mock<ICustomerService>();
            TemplateServiceMock.Setup(service => service.UpdateCustomerAsync(It.IsAny<Customer>()))
                .ReturnsAsync((Customer)null);

            var updateCustomerRequestHandler = new UpdateCustomerHandler(TemplateServiceMock.Object);
            var request = new UpdateCustomerRequest(TemplateToUpdate.Id, TemplateToUpdate.Name, TemplateToUpdate.Description);

            // Act
            var result = await updateCustomerRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.NotFound);

            // Verify
            TemplateServiceMock.Verify(service => service.UpdateCustomerAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CustomerUpdateFails_ReturnsInternalServerErrorErrorCode()
        {
            // Arrange
            var TemplateFromDatabase = new Customer
            {
                Id = 1,
                Name = "Test Customer",
                Description = "Test Description"
            };

            var TemplateToUpdate = new Customer
            {
                Id = TemplateFromDatabase.Id,
                Description = "Updated Test Description"
            };

            var TemplateServiceMock = new Mock<ICustomerService>();            
            TemplateServiceMock.Setup(service => service.UpdateCustomerAsync(It.IsAny<Customer>()))
                .Throws(new Exception());

            var updateCustomerRequestHandler = new UpdateCustomerHandler(TemplateServiceMock.Object);
            var request = new UpdateCustomerRequest(TemplateToUpdate.Id, TemplateToUpdate.Name, TemplateToUpdate.Description);

            // Act
            var result = await updateCustomerRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.InternalServerError);

            // Verify
            TemplateServiceMock.Verify(service => service.UpdateCustomerAsync(It.IsAny<Customer>()), Times.Once);
        }
        

        [Theory]
        [MemberData(nameof(UpdateCustomer_InvalidUpdateCustomerRequestsToTest))]
        public async Task Handle_CustomerUpdateFails_ReturnsBadRequestErrorCode(UpdateCustomerRequest request)
        {
            // Arrange
            var TemplateServiceMock = new Mock<ICustomerService>();
            var updateCustomerRequestHandler = new UpdateCustomerHandler(TemplateServiceMock.Object);

            // Act
            var result = await updateCustomerRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.BadRequest);

            // Verify
            TemplateServiceMock.Verify(service => service.UpdateCustomerAsync(It.IsAny<Customer>()), Times.Never);
        }
        public static IEnumerable<object[]> UpdateCustomer_InvalidUpdateCustomerRequestsToTest =>
           new List<object[]>
           {
                new object[] { null },
                new object[] { new UpdateCustomerRequest(0, string.Empty, string.Empty) }
           };
    }
}
