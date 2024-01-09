using FluentAssertions;
using Moq;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.SharedModule.Application.Mediators.SharedOperations.RemoveById;
using TrekTrove.Api.Modules.SharedModule.Domain.Interfaces;
using Xunit;

namespace TrekTrove.Api.Modules.SharedModule.Tests.Application.Mediators.SharedOperations.RemoveById
{
    public class RemoveSharedByIdHandlerTests
    {

        [Theory]
        [MemberData(nameof(RemoveSharedById_ValidRemoveSharedByIdResultsToTest))]
        public async Task Handle_ValidInput_ReturnsDeletedShared(bool expected)
        {
            // Arrange
            var TemplateServiceMock = new Mock<ISharedService>();
            TemplateServiceMock.Setup(service => service.RemoveSharedByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expected);

            var deleteSharedHandler = new RemoveSharedByIdHandler(TemplateServiceMock.Object);
            var request = new RemoveSharedByIdRequest(1);

            // Act
            var result = await deleteSharedHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeTrue();
            result.Invalid.Should().BeFalse();
            result.Notifications.Count.Should().Be(0);
            result.Data.Should().Be(expected);

            // Verify
            TemplateServiceMock.Verify(service => service.RemoveSharedByIdAsync(It.IsAny<int>()), Times.Once);
        }
        public static IEnumerable<object[]> RemoveSharedById_ValidRemoveSharedByIdResultsToTest =>
           new List<object[]>
           {
                new object[] { true },
                new object[] { false }
           };

        [Fact]
        public async Task Handle_RemoveSharedByIdFails_ReturnsNotFoundErrorCode()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ISharedService>();
            TemplateServiceMock.Setup(service => service.RemoveSharedByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((bool?)null);

            var deleteSharedHandler = new RemoveSharedByIdHandler(TemplateServiceMock.Object);
            var request = new RemoveSharedByIdRequest(1);

            // Act
            var result = await deleteSharedHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.NotFound);

            // Verify
            TemplateServiceMock.Verify(service => service.RemoveSharedByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Handle_RemoveSharedByIdFails_ReturnsInternalServerErrorErrorCode()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ISharedService>();
            TemplateServiceMock.Setup(service => service.RemoveSharedByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            var deleteSharedHandler = new RemoveSharedByIdHandler(TemplateServiceMock.Object);
            var request = new RemoveSharedByIdRequest(1);

            // Act
            var result = await deleteSharedHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.InternalServerError);

            // Verify
            TemplateServiceMock.Verify(service => service.RemoveSharedByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(RemoveSharedById_InvalidRemoveSharedByIdRequestsToTest))]
        public async Task Handle_NullOrZeroInput_ReturnsBadRequestErrorCode(RemoveSharedByIdRequest request)
        {
            // Arrange
            var TemplateServiceMock = new Mock<ISharedService>();
            var deleteSharedHandler = new RemoveSharedByIdHandler(TemplateServiceMock.Object);

            // Act
            var result = await deleteSharedHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.BadRequest);

            // Verify
            TemplateServiceMock.Verify(service => service.RemoveSharedByIdAsync(It.IsAny<int>()), Times.Never);
        }

        public static IEnumerable<object[]> RemoveSharedById_InvalidRemoveSharedByIdRequestsToTest =>
           new List<object[]>
           {
                new object[] { null },
                new object[] { new RemoveSharedByIdRequest(0) }
           };
    }
}
