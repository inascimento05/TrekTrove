using FluentAssertions;
using Moq;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.SharedModule.Application.Mediators.SharedOperations.GetById;
using TrekTrove.Api.Modules.SharedModule.Domain.Entities;
using TrekTrove.Api.Modules.SharedModule.Domain.Interfaces;
using Xunit;

namespace TrekTrove.Api.Modules.SharedModule.Tests.Application.Mediators.SharedOperations.GetById
{
    public class GetSharedByIdHandlerTests
    {
        [Fact]
        public async Task Handle_ValidInput_ReturnsShared()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ISharedService>();
            var getSharedByIdHandler = new GetSharedByIdHandler(TemplateServiceMock.Object);
            var expectedShared = new Shared
            {
                Id = 1,
                Name = "Test Shared",
                Description = "Test Description"
            };

            TemplateServiceMock.Setup(service => service.GetSharedByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedShared);

            var request = new GetSharedByIdRequest(expectedShared.Id);

            // Act
            var result = await getSharedByIdHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeTrue();
            result.Invalid.Should().BeFalse();
            result.Notifications.Count.Should().Be(0);
            result.Data.Id.Should().Be(expectedShared.Id);
            result.Data.Name.Should().Be(expectedShared.Name);
            result.Data.Description.Should().Be(expectedShared.Description);

            // Verify
            TemplateServiceMock.Verify(service => service.GetSharedByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Handle_SharedGetByIdFails_ReturnsNotFoundErrorCode()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ISharedService>();
            var getSharedByIdHandler = new GetSharedByIdHandler(TemplateServiceMock.Object);

            TemplateServiceMock.Setup(service => service.GetSharedByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Shared)null);

            var request = new GetSharedByIdRequest(1);

            // Act
            var result = await getSharedByIdHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.NotFound);

            // Verify
            TemplateServiceMock.Verify(service => service.GetSharedByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Handle_SharedGetByIdFails_ReturnsInternalServerErrorErrorCode()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ISharedService>(); 
            var getSharedByIdHandler = new GetSharedByIdHandler(TemplateServiceMock.Object);

            TemplateServiceMock.Setup(service => service.GetSharedByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());
            
            var request = new GetSharedByIdRequest(1);

            // Act
            var result = await getSharedByIdHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.InternalServerError);

            // Verify
            TemplateServiceMock.Verify(service => service.GetSharedByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(GetSharedById_InvalidGetSharedByIdRequestsToTest))]
        public async Task Handle_NullOrZeroInput_ReturnsBadRequestErrorCode(GetSharedByIdRequest request)
        {
            // Arrange
            var TemplateServiceMock = new Mock<ISharedService>();
            var getSharedByIdHandler = new GetSharedByIdHandler(TemplateServiceMock.Object);

            // Act
            var result = await getSharedByIdHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.BadRequest);

            // Verify
            TemplateServiceMock.Verify(service => service.GetSharedByIdAsync(It.IsAny<int>()), Times.Never);
        }

        public static IEnumerable<object[]> GetSharedById_InvalidGetSharedByIdRequestsToTest =>
           new List<object[]>
           {
                new object[] { null },
                new object[] { new GetSharedByIdRequest(0) }
           };
    }
}
