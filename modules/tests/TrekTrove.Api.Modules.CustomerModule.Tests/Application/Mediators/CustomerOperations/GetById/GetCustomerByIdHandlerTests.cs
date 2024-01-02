using FluentAssertions;
using Moq;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.GetById;
using TrekTrove.Api.Modules.CustomerModule.Domain.Entities;
using TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces;
using Xunit;

namespace TrekTrove.Api.Modules.CustomerModule.Tests.Application.Mediators.CustomerOperations.GetById
{
    public class GetCustomerByIdHandlerTests
    {
        [Fact]
        public async Task Handle_ValidInput_ReturnsCustomer()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ICustomerService>();
            var getCustomerByIdHandler = new GetCustomerByIdHandler(TemplateServiceMock.Object);
            var expectedCustomer = new Customer
            {
                Id = 1,
                Name = "Test Customer",
                Description = "Test Description"
            };

            TemplateServiceMock.Setup(service => service.GetCustomerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedCustomer);

            var request = new GetCustomerByIdRequest(expectedCustomer.Id);

            // Act
            var result = await getCustomerByIdHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeTrue();
            result.Invalid.Should().BeFalse();
            result.Notifications.Count.Should().Be(0);
            result.Data.Id.Should().Be(expectedCustomer.Id);
            result.Data.Name.Should().Be(expectedCustomer.Name);
            result.Data.Description.Should().Be(expectedCustomer.Description);

            // Verify
            TemplateServiceMock.Verify(service => service.GetCustomerByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CustomerGetByIdFails_ReturnsNotFoundErrorCode()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ICustomerService>();
            var getCustomerByIdHandler = new GetCustomerByIdHandler(TemplateServiceMock.Object);

            TemplateServiceMock.Setup(service => service.GetCustomerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Customer)null);

            var request = new GetCustomerByIdRequest(1);

            // Act
            var result = await getCustomerByIdHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.NotFound);

            // Verify
            TemplateServiceMock.Verify(service => service.GetCustomerByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CustomerGetByIdFails_ReturnsInternalServerErrorErrorCode()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ICustomerService>(); 
            var getCustomerByIdHandler = new GetCustomerByIdHandler(TemplateServiceMock.Object);

            TemplateServiceMock.Setup(service => service.GetCustomerByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());
            
            var request = new GetCustomerByIdRequest(1);

            // Act
            var result = await getCustomerByIdHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.InternalServerError);

            // Verify
            TemplateServiceMock.Verify(service => service.GetCustomerByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(GetCustomerById_InvalidGetCustomerByIdRequestsToTest))]
        public async Task Handle_NullOrZeroInput_ReturnsBadRequestErrorCode(GetCustomerByIdRequest request)
        {
            // Arrange
            var TemplateServiceMock = new Mock<ICustomerService>();
            var getCustomerByIdHandler = new GetCustomerByIdHandler(TemplateServiceMock.Object);

            // Act
            var result = await getCustomerByIdHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.BadRequest);

            // Verify
            TemplateServiceMock.Verify(service => service.GetCustomerByIdAsync(It.IsAny<int>()), Times.Never);
        }

        public static IEnumerable<object[]> GetCustomerById_InvalidGetCustomerByIdRequestsToTest =>
           new List<object[]>
           {
                new object[] { null },
                new object[] { new GetCustomerByIdRequest(0) }
           };
    }
}
