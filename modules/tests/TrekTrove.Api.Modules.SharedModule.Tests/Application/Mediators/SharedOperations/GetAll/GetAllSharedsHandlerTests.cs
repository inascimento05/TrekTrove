using FluentAssertions;
using Moq;
using TrekTrove.Api.Modules.SharedModule.Application.Mediators.SharedOperations.GetAll;
using TrekTrove.Api.Modules.SharedModule.Domain.Entities;
using TrekTrove.Api.Modules.SharedModule.Domain.Interfaces;
using Xunit;

namespace TrekTrove.Api.Modules.SharedModule.Tests.Application.Mediators.SharedOperations.GetAll
{
    public class GetAllSharedsHandlerTests
    {
        [Fact]
        public async Task Handle_ValidInput_ReturnsEmptySharedsList()
        {
            // Arrange
            var TemplateServiceMock = new Mock<ISharedService>();
            var getAllSharedHandler = new GetAllSharedsHandler(TemplateServiceMock.Object);
            var expectedShared = new List<Shared>();

            TemplateServiceMock.Setup(service => service.GetAllSharedsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedShared);

            var request = new GetAllSharedsRequest(1, 10);

            // Act
            var response = await getAllSharedHandler.Handle(request, default);

            // Assert
            response.Valid.Should().BeTrue();
            response.Invalid.Should().BeFalse();
            response.Notifications.Count.Should().Be(0);
            response.Data.Count().Should().Be(0);

            // Verify
            TemplateServiceMock.Verify(service => service.GetAllSharedsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidInput_ReturnsAllShareds()
        {
            // Arrange
            var expectedShared = new List<Shared> {
                new Shared {
                    Id = 1,
                    Name = "Test Shared",
                    Description = "Test Description"
                },
                new Shared {
                    Id = 2,
                    Name = "Test Shared 2",
                    Description = "Test Description 2"
                }
            };

            var TemplateServiceMock = new Mock<ISharedService>();
            TemplateServiceMock.Setup(service => service.GetAllSharedsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedShared);

            var getAllSharedHandler = new GetAllSharedsHandler(TemplateServiceMock.Object);
            var request = new GetAllSharedsRequest(1, 10);

            // Act
            var response = await getAllSharedHandler.Handle(request, default);

            // Assert
            response.Valid.Should().BeTrue();
            response.Invalid.Should().BeFalse();
            response.Notifications.Count.Should().Be(0);
            response.Data.Count().Should().Be(2);

            // Verify
            TemplateServiceMock.Verify(service => service.GetAllSharedsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(GetAllSharedsRequest_ValidPaginationInputsToTest))]
        public async Task Handle_ValidPaginationInput_ReturnsSharedsPaginated(int page)
        {
            // Arrange
            var TemplatesFromDatabase = new List<Shared> {
                new Shared
                {
                    Id = 1,
                    Name = "Test Shared",
                    Description = "Test Description"
                },
                new Shared
                {
                    Id = 2,
                    Name = "Test Shared 2",
                    Description = "Test Description 2"
                },
            };
            var expectedShared = TemplatesFromDatabase.Skip(page - 1)
                .Take(1);

            var TemplateServiceMock = new Mock<ISharedService>();
            TemplateServiceMock.Setup(service => service.GetAllSharedsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedShared);

            var getAllSharedHandler = new GetAllSharedsHandler(TemplateServiceMock.Object);
            var request = new GetAllSharedsRequest(page, 1);

            // Act
            var response = await getAllSharedHandler.Handle(request, default);

            // Assert
            response.Valid.Should().BeTrue();
            response.Invalid.Should().BeFalse();
            response.Notifications.Count.Should().Be(0);
            response.Data.Count().Should().Be(1);
            response.Data.First().Id.Should().Be(TemplatesFromDatabase[(page - 1)].Id);
            response.Data.First().Name.Should().Be(TemplatesFromDatabase[(page - 1)].Name);
            response.Data.First().Description.Should().Be(TemplatesFromDatabase[(page - 1)].Description);

            // Verify
            TemplateServiceMock.Verify(service => service.GetAllSharedsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        public static IEnumerable<object[]> GetAllSharedsRequest_ValidPaginationInputsToTest =>
        new List<object[]> {
            new object[] { 1 },
                new object[] { 2 }
            };

        [Theory]
        [MemberData(nameof(GetAllSharedsRequest_ZeroInputsToTest))]
        public async Task Handle_ZeroInput_ReturnsSharedsPaginated(GetAllSharedsRequest request)
        {
            // Arrange
            var TemplatesFromDatabase = new List<Shared>();
            for (int i = 0; i < request.PageSize; i++)
            {
                TemplatesFromDatabase.Add(
                    new Shared
                    {
                        Id = i,
                        Name = $"Test Shared {i}",
                        Description = $"Test Description {i}"
                    }
                );
            }

            var TemplateServiceMock = new Mock<ISharedService>();
            TemplateServiceMock.Setup(service => service.GetAllSharedsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(TemplatesFromDatabase);

            var getAllSharedHandler = new GetAllSharedsHandler(TemplateServiceMock.Object);

            // Act
            var result = await getAllSharedHandler.Handle(request, default);

            // Assert
            result.Valid.Should().BeTrue();
            result.Invalid.Should().BeFalse();
            result.Notifications.Count.Should().Be(0);
            result.Data.Count().Should().BeLessThanOrEqualTo(request.PageSize);

            // Verify
            TemplateServiceMock.Verify(service => service.GetAllSharedsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
        public static IEnumerable<object[]> GetAllSharedsRequest_ZeroInputsToTest =>
            new List<object[]> {
                new object[] { new GetAllSharedsRequest(0, 10) },
                new object[] { new GetAllSharedsRequest(1, 0) },
                new object[] { new GetAllSharedsRequest(0, 0) }
            };
    }
}
