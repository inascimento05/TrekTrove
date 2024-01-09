using FluentAssertions;
using Moq;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.SharedModule.Application.Mediators.SharedOperations.Create;
using TrekTrove.Api.Modules.SharedModule.Domain.Entities;
using TrekTrove.Api.Modules.SharedModule.Domain.Interfaces;
using Xunit;

namespace TrekTrove.Api.Modules.SharedModule.Tests.Application.Mediators.SharedOperations.Create
{
    public class CreateSharedHandlerTests
    {
        [Fact]
        public async Task Handle_ValidInput_ReturnsCreatedShared()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ISharedService>();
            var createSharedRequestHandler = new CreateSharedHandler(TemplateServiceMock.Object);
            var expectedCreatedSharedId = 1;
            TemplateServiceMock.Setup(service => service.CreateSharedAsync(It.IsAny<Shared>()))
                .ReturnsAsync(expectedCreatedSharedId);

            var request = new CreateSharedRequest("Test Shared", "Test Description");

            // Act
            var result = await createSharedRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeTrue();
            result.Invalid.Should().BeFalse();
            result.Notifications.Count.Should().Be(0);
            result.Data.Id.Should().Be(expectedCreatedSharedId);

            // Verify
            TemplateServiceMock.Verify(service => service.CreateSharedAsync(It.IsAny<Shared>()), Times.Once);
        }

        [Fact]
        public async Task Handle_SharedCreationFails_ReturnsUnprocessableEntityErrorCode()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ISharedService>();
            var createSharedRequestHandler = new CreateSharedHandler(TemplateServiceMock.Object);

            TemplateServiceMock.Setup(service => service.CreateSharedAsync(It.IsAny<Shared>()))
                .ReturnsAsync(0);

            var request = new CreateSharedRequest("Test Shared", "Test Description");

            // Act
            var result = await createSharedRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.UnprocessableEntity);

            // Verify
            TemplateServiceMock.Verify(service => service.CreateSharedAsync(It.IsAny<Shared>()), Times.Once);
        }

        [Fact]
        public async Task Handle_SharedCreationFails_ReturnsInternalServerErrorErrorCode()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ISharedService>();
            var createSharedRequestHandler = new CreateSharedHandler(TemplateServiceMock.Object);

            TemplateServiceMock.Setup(service => service.CreateSharedAsync(It.IsAny<Shared>()))
                .ThrowsAsync(new Exception());

            var request = new CreateSharedRequest("Test Shared", "Test Description");

            // Act
            var result = await createSharedRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.InternalServerError);

            // Verify
            TemplateServiceMock.Verify(service => service.CreateSharedAsync(It.IsAny<Shared>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(CreateShared_InvalidCreateSharedRequestsToTest))]
        public async Task Handle_NullOrEmptyInput_ReturnsBadRequestErrorCode(CreateSharedRequest request)
        {
            // Arrange
            var TemplateServiceMock = new Mock<ISharedService>();
            var createSharedRequestHandler = new CreateSharedHandler(TemplateServiceMock.Object);

            // Act
            var result = await createSharedRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.BadRequest);

            // Verify
            TemplateServiceMock.Verify(service => service.CreateSharedAsync(It.IsAny<Shared>()), Times.Never);
        }

        public static IEnumerable<object[]> CreateShared_InvalidCreateSharedRequestsToTest =>
           new List<object[]>
           {
                new object[] { null },
                new object[] { new CreateSharedRequest(null, "Test Description") },
                new object[] { new CreateSharedRequest("Test Shared", null) },
                new object[] { new CreateSharedRequest(string.Empty, "Test Description") },
                new object[] { new CreateSharedRequest("Test Shared", string.Empty) },
           };
    }
}
