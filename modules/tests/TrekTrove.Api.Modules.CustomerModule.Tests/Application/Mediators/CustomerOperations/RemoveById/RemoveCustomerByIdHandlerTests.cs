using FluentAssertions;
using Moq;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.RemoveById;
using TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces;
using Xunit;

namespace TrekTrove.Api.Modules.CustomerModule.Tests.Application.Mediators.CustomerOperations.RemoveById
{
    public class RemoveCustomerByIdHandlerTests
    {

        [Theory]
        [MemberData(nameof(RemoveCustomerById_ValidRemoveCustomerByIdResultsToTest))]
        public async Task Handle_ValidInput_ReturnsDeletedCustomer(bool expected)
        {
            // Arrange
            var TemplateServiceMock = new Mock<ICustomerService>();
            TemplateServiceMock.Setup(service => service.RemoveCustomerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expected);

            var deleteCustomerHandler = new RemoveCustomerByIdHandler(TemplateServiceMock.Object);
            var request = new RemoveCustomerByIdRequest(1);

            // Act
            var result = await deleteCustomerHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeTrue();
            result.Invalid.Should().BeFalse();
            result.Notifications.Count.Should().Be(0);
            result.Data.Should().Be(expected);

            // Verify
            TemplateServiceMock.Verify(service => service.RemoveCustomerByIdAsync(It.IsAny<int>()), Times.Once);
        }
        public static IEnumerable<object[]> RemoveCustomerById_ValidRemoveCustomerByIdResultsToTest =>
           new List<object[]>
           {
                new object[] { true },
                new object[] { false }
           };

        [Fact]
        public async Task Handle_RemoveCustomerByIdFails_ReturnsNotFoundErrorCode()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ICustomerService>();
            TemplateServiceMock.Setup(service => service.RemoveCustomerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((bool?)null);

            var deleteCustomerHandler = new RemoveCustomerByIdHandler(TemplateServiceMock.Object);
            var request = new RemoveCustomerByIdRequest(1);

            // Act
            var result = await deleteCustomerHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.NotFound);

            // Verify
            TemplateServiceMock.Verify(service => service.RemoveCustomerByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Handle_RemoveCustomerByIdFails_ReturnsInternalServerErrorErrorCode()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ICustomerService>();
            TemplateServiceMock.Setup(service => service.RemoveCustomerByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            var deleteCustomerHandler = new RemoveCustomerByIdHandler(TemplateServiceMock.Object);
            var request = new RemoveCustomerByIdRequest(1);

            // Act
            var result = await deleteCustomerHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.InternalServerError);

            // Verify
            TemplateServiceMock.Verify(service => service.RemoveCustomerByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(RemoveCustomerById_InvalidRemoveCustomerByIdRequestsToTest))]
        public async Task Handle_NullOrZeroInput_ReturnsBadRequestErrorCode(RemoveCustomerByIdRequest request)
        {
            // Arrange
            var TemplateServiceMock = new Mock<ICustomerService>();
            var deleteCustomerHandler = new RemoveCustomerByIdHandler(TemplateServiceMock.Object);

            // Act
            var result = await deleteCustomerHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.BadRequest);

            // Verify
            TemplateServiceMock.Verify(service => service.RemoveCustomerByIdAsync(It.IsAny<int>()), Times.Never);
        }

        public static IEnumerable<object[]> RemoveCustomerById_InvalidRemoveCustomerByIdRequestsToTest =>
           new List<object[]>
           {
                new object[] { null },
                new object[] { new RemoveCustomerByIdRequest(0) }
           };
    }
}
