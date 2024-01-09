using FluentAssertions;
using Moq;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.SharedModule.Application.Mediators.SharedOperations.Update;
using TrekTrove.Api.Modules.SharedModule.Domain.Entities;
using TrekTrove.Api.Modules.SharedModule.Domain.Interfaces;
using Xunit;

namespace TrekTrove.Api.Modules.SharedModule.Tests.Application.Mediators.SharedOperations.Update
{
    public class UpdateSharedHandlerTests {
        [Fact]
        public async Task Handle_ValidInput_ReturnUpdatedShared()
        {
            // Arrange
            var TemplateFromDatabase = new Shared
            {
                Id = 1,
                Name = "Test Shared",
                Description = "Test Description"
            };

            var TemplateToUpdate = new Shared
            {
                Id = TemplateFromDatabase.Id,
                Description = "Updated Test Description"
            };

            var expectedUpdatedShared = new Shared
            {
                Id = TemplateFromDatabase.Id,
                Name = TemplateFromDatabase.Name,
                Description = TemplateToUpdate.Description
            };

            var TemplateServiceMock = new Mock<ISharedService>();
            TemplateServiceMock.Setup(service => service.UpdateSharedAsync(It.IsAny<Shared>()))
                .ReturnsAsync(expectedUpdatedShared);

            var updateSharedRequestHandler = new UpdateSharedHandler(TemplateServiceMock.Object);
            var request = new UpdateSharedRequest(TemplateToUpdate.Id, TemplateToUpdate.Name, TemplateToUpdate.Description);

            // Act
            var result = await updateSharedRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeTrue();
            result.Invalid.Should().BeFalse();
            result.Notifications.Count.Should().Be(0);
            result.Data.Id.Should().Be(expectedUpdatedShared.Id);
            result.Data.Name.Should().Be(expectedUpdatedShared.Name);
            result.Data.Description.Should().Be(expectedUpdatedShared.Description);

            // Verify
            TemplateServiceMock.Verify(service => service.UpdateSharedAsync(It.IsAny<Shared>()), Times.Once);
        }

        [Fact]
        public async Task Handle_SharedUpdateFails_ReturnsNotFoundErrorCode()
        {
            // Arrange
            var TemplateToUpdate = new Shared
            {
                Id = 1,
                Description = "Updated Test Description"
            };

            var TemplateServiceMock = new Mock<ISharedService>();
            TemplateServiceMock.Setup(service => service.UpdateSharedAsync(It.IsAny<Shared>()))
                .ReturnsAsync((Shared)null);

            var updateSharedRequestHandler = new UpdateSharedHandler(TemplateServiceMock.Object);
            var request = new UpdateSharedRequest(TemplateToUpdate.Id, TemplateToUpdate.Name, TemplateToUpdate.Description);

            // Act
            var result = await updateSharedRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.NotFound);

            // Verify
            TemplateServiceMock.Verify(service => service.UpdateSharedAsync(It.IsAny<Shared>()), Times.Once);
        }

        [Fact]
        public async Task Handle_SharedUpdateFails_ReturnsInternalServerErrorErrorCode()
        {
            // Arrange
            var TemplateFromDatabase = new Shared
            {
                Id = 1,
                Name = "Test Shared",
                Description = "Test Description"
            };

            var TemplateToUpdate = new Shared
            {
                Id = TemplateFromDatabase.Id,
                Description = "Updated Test Description"
            };

            var TemplateServiceMock = new Mock<ISharedService>();            
            TemplateServiceMock.Setup(service => service.UpdateSharedAsync(It.IsAny<Shared>()))
                .Throws(new Exception());

            var updateSharedRequestHandler = new UpdateSharedHandler(TemplateServiceMock.Object);
            var request = new UpdateSharedRequest(TemplateToUpdate.Id, TemplateToUpdate.Name, TemplateToUpdate.Description);

            // Act
            var result = await updateSharedRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.InternalServerError);

            // Verify
            TemplateServiceMock.Verify(service => service.UpdateSharedAsync(It.IsAny<Shared>()), Times.Once);
        }
        

        [Theory]
        [MemberData(nameof(UpdateShared_InvalidUpdateSharedRequestsToTest))]
        public async Task Handle_SharedUpdateFails_ReturnsBadRequestErrorCode(UpdateSharedRequest request)
        {
            // Arrange
            var TemplateServiceMock = new Mock<ISharedService>();
            var updateSharedRequestHandler = new UpdateSharedHandler(TemplateServiceMock.Object);

            // Act
            var result = await updateSharedRequestHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeFalse();
            result.Invalid.Should().BeTrue();
            result.Notifications.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Error.Should().Be(ErrorCode.BadRequest);

            // Verify
            TemplateServiceMock.Verify(service => service.UpdateSharedAsync(It.IsAny<Shared>()), Times.Never);
        }
        public static IEnumerable<object[]> UpdateShared_InvalidUpdateSharedRequestsToTest =>
           new List<object[]>
           {
                new object[] { null },
                new object[] { new UpdateSharedRequest(0, string.Empty, string.Empty) }
           };
    }
}
