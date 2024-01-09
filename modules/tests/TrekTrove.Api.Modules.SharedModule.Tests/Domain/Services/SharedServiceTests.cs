using FluentAssertions;
using Moq;
using TrekTrove.Api.Modules.SharedModule.Domain.Entities;
using TrekTrove.Api.Modules.SharedModule.Domain.Interfaces;
using TrekTrove.Api.Modules.SharedModule.Domain.Services;
using Xunit;

namespace TrekTrove.Api.Modules.SharedModule.Tests.Domain.Services
{
    public class SharedServiceTests
    {
        #region Create
        [Fact]
        public async Task CreateSharedAsync_ValidShared_CreatesShared()
        {
            // Arrange
            var expectedSharedId = 1;
            var Template = new Shared
            {
                Name = "Test Shared",
                Description = "Test Description"
            };

            var TemplateRepositoryMock = new Mock<ISharedRepository>();
            TemplateRepositoryMock.Setup(repo => repo.CreateSharedAsync(It.IsAny<Shared>()))
                .ReturnsAsync(expectedSharedId);

            var TemplateService = new SharedService(TemplateRepositoryMock.Object);

            // Act
            Template.Id = await TemplateService.CreateSharedAsync(Template);

            // Assert
            Template.Id.Should().Be(expectedSharedId);

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.CreateSharedAsync(It.IsAny<Shared>()), Times.Once);
        }

        [Fact]
        public async Task CreateSharedAsync_NullInput_ThrowsException()
        {
            // Arrange
            var TemplateRepositoryMock = new Mock<ISharedRepository>();
            var TemplateService = new SharedService(TemplateRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => TemplateService.CreateSharedAsync((Shared)null));

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.CreateSharedAsync(It.IsAny<Shared>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(CreateShared_InvalidInputsToTest))]
        public async Task CreateSharedAsync_InvalidInput_ThrowsException(Shared Template)
        {
            // Arrange
            var TemplateRepositoryMock = new Mock<ISharedRepository>();
            var TemplateService = new SharedService(TemplateRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => TemplateService.CreateSharedAsync(Template));

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.CreateSharedAsync(It.IsAny<Shared>()), Times.Never);
        }

        public static IEnumerable<object[]> CreateShared_InvalidInputsToTest =>
            new List<object[]>
                {
                    new object[] { new Shared { Name = null, Description = "Test Description" } },
                    new object[] { new Shared { Name = "Test Shared", Description = null } },
                    new object[] { new Shared { Name = string.Empty, Description = "Test Description" } },
                    new object[] { new Shared { Name = "Test Shared", Description = string.Empty } },
                    new object[] { new Shared { Name = "Test Shared".PadRight(101,'.'), Description = "Test Description" } },
                    new object[] { new Shared { Name = "Test Shared", Description = "Test Description".PadRight(201,'.') } },
                };
        #endregion

        #region Read
        [Fact]
        public async Task GetAllSharedsAsync_ValidPagination_ReturnZeroShareds()
        {
            // Arrange
            var expectedShared = new List<Shared>();

            var TemplateRepositoryMock = new Mock<ISharedRepository>();
            TemplateRepositoryMock.Setup(repo => repo.GetAllSharedsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedShared);

            var TemplateService = new SharedService(TemplateRepositoryMock.Object);

            // Act
            var Templates = await TemplateService.GetAllSharedsAsync(1, 10);

            // Assert
            Templates.Count().Should().Be(0);

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetAllSharedsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAllSharedsAsync_ValidPagination_ReturnShareds()
        {
            // Arrange
            var expectedShared = new List<Shared> {
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

            var TemplateRepositoryMock = new Mock<ISharedRepository>();
            TemplateRepositoryMock.Setup(repo => repo.GetAllSharedsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedShared);

            var TemplateService = new SharedService(TemplateRepositoryMock.Object);

            // Act
            var Templates = await TemplateService.GetAllSharedsAsync(1, 10);

            // Assert
            Templates.Count().Should().BeGreaterThanOrEqualTo(1);

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetAllSharedsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(GetAllSharedsAsync_ValidPaginationToTest))]
        public async Task GetAllSharedsAsync_ValidPaginationPagination_ReturnShareds(int page)
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

            var returnedShareds = TemplatesFromDatabase.Skip(page - 1)
                .Take(1);

            var TemplateRepositoryMock = new Mock<ISharedRepository>();
            TemplateRepositoryMock.Setup(repo => repo.GetAllSharedsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(returnedShareds);

            var TemplateService = new SharedService(TemplateRepositoryMock.Object);

            // Act
            var Templates = await TemplateService.GetAllSharedsAsync(page, 1);

            // Assert
            Templates.Count().Should().Be(1);
            Templates.First().Id.Should().Be(TemplatesFromDatabase[(page - 1)].Id);
            Templates.First().Name.Should().Be(TemplatesFromDatabase[(page - 1)].Name);
            Templates.First().Description.Should().Be(TemplatesFromDatabase[(page - 1)].Description);

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetAllSharedsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        public static IEnumerable<object[]> GetAllSharedsAsync_ValidPaginationToTest =>
            new List<object[]>
                {
                    new object[] { 1 },
                    new object[] { 2 }
                };

        [Theory]
        [MemberData(nameof(GetAllSharedsAsync_InvalidPaginationToTest))]
        public async Task GetAllSharedsAsync_InvalidPaginationPagination_ThrowsException(int pageNumber, int pageSize)
        {
            // Arrange
            var TemplateRepositoryMock = new Mock<ISharedRepository>();
            var TemplateService = new SharedService(TemplateRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => TemplateService.GetAllSharedsAsync(pageNumber, pageSize));

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetAllSharedsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        public static IEnumerable<object[]> GetAllSharedsAsync_InvalidPaginationToTest =>
            new List<object[]>
                {
                    new object[] { null, 10 },
                    new object[] { 1, null },
                    new object[] { 0, 10 },
                    new object[] { 1, 0 }
                };

        [Fact]
        public async Task GetSharedByIdAsync_ValidId_GetShared()
        {
            // Arrange
            var expectedShared = new Shared
            {
                Id = 1,
                Name = "Test Shared",
                Description = "Test Description"
            };

            var TemplateRepositoryMock = new Mock<ISharedRepository>();
            TemplateRepositoryMock.Setup(repo => repo.GetSharedByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedShared);

            var TemplateService = new SharedService(TemplateRepositoryMock.Object);

            // Act
            var Template = await TemplateService.GetSharedByIdAsync(expectedShared.Id);

            // Assert
            Template.Id.Should().Be(expectedShared.Id);
            Template.Name.Should().Be(expectedShared.Name);
            Template.Description.Should().Be(expectedShared.Description);

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetSharedByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetSharedByIdAsync_InvalidInput_ThrowsException()
        {
            // Arrange
            var TemplateRepositoryMock = new Mock<ISharedRepository>();
            var TemplateService = new SharedService(TemplateRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => TemplateService.GetSharedByIdAsync(0));

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetSharedByIdAsync(It.IsAny<int>()), Times.Never);
        }
        #endregion

        #region Update
        [Fact]
        public async Task UpdateSharedAsync_UpdateDescription_UpdatesShared()
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

            var expectedShared = new Shared
            {
                Id = TemplateFromDatabase.Id,
                Name = TemplateFromDatabase.Name,
                Description = TemplateToUpdate.Description
            };

            var TemplateRepositoryMock = new Mock<ISharedRepository>();
            TemplateRepositoryMock.Setup(repo => repo.GetSharedByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(TemplateFromDatabase);

            TemplateRepositoryMock.Setup(repo => repo.UpdateSharedAsync(It.IsAny<Shared>()))
                .ReturnsAsync(expectedShared);

            var TemplateService = new SharedService(TemplateRepositoryMock.Object);

            // Act
            var updatedShared = await TemplateService.UpdateSharedAsync(TemplateToUpdate);

            // Assert
            updatedShared.Id.Should().Be(expectedShared.Id);
            updatedShared.Name.Should().Be(expectedShared.Name);
            updatedShared.Description.Should().Be(expectedShared.Description);

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetSharedByIdAsync(It.IsAny<int>()), Times.Once);
            TemplateRepositoryMock.Verify(repo => repo.UpdateSharedAsync(It.IsAny<Shared>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSharedAsync_NullInput_ThrowsException()
        {
            // Arrange
            var TemplateRepositoryMock = new Mock<ISharedRepository>();
            var TemplateService = new SharedService(TemplateRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => TemplateService.UpdateSharedAsync((Shared)null));

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetSharedByIdAsync(It.IsAny<int>()), Times.Never);
            TemplateRepositoryMock.Verify(repo => repo.UpdateSharedAsync(It.IsAny<Shared>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(UpdateShared_InvalidInputsToTest))]
        public async Task UpdateSharedAsync_InvalidInput_ThrowsException(Shared Template)
        {
            // Arrange
            var TemplateRepositoryMock = new Mock<ISharedRepository>();
            var TemplateService = new SharedService(TemplateRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => TemplateService.UpdateSharedAsync(Template));

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetSharedByIdAsync(It.IsAny<int>()), Times.Never);
            TemplateRepositoryMock.Verify(repo => repo.UpdateSharedAsync(It.IsAny<Shared>()), Times.Never);
        }

        public static IEnumerable<object[]> UpdateShared_InvalidInputsToTest =>
            new List<object[]>
                {
                    new object[] { new Shared { Name = "Test Shared", Description = "Test Description" } },
                    new object[] { new Shared { Id = 1, Name = "Test Shared".PadRight(101,'.'), Description = "Test Description" } },
                    new object[] { new Shared { Id = 1, Name = "Test Shared", Description = "Test Description".PadRight(201,'.') } },
                };
        #endregion

        #region Delete
        [Fact]
        public async Task RemoveSharedByIdAsync_ValidShareds_RemovesShared()
        {
            // Arrange
            var TemplateFromDatabase = new Shared
            {
                Id = 1,
                Name = "Test Shared",
                Description = "Test Description"
            };
            var expected = true;

            var TemplateRepositoryMock = new Mock<ISharedRepository>();
            TemplateRepositoryMock.Setup(repo => repo.GetSharedByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(TemplateFromDatabase);
            TemplateRepositoryMock.Setup(repo => repo.DeleteSharedByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expected);

            var TemplateService = new SharedService(TemplateRepositoryMock.Object);

            // Act
            var isDeleted = await TemplateService.RemoveSharedByIdAsync(1);

            // Assert
            isDeleted.Should().Be(expected);

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.DeleteSharedByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task RemoveSharedByIdAsync_InvalidInput_ThrowsException()
        {
            // Arrange
            var TemplateRepositoryMock = new Mock<ISharedRepository>();
            var TemplateService = new SharedService(TemplateRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => TemplateService.RemoveSharedByIdAsync(0));

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.DeleteSharedByIdAsync(It.IsAny<int>()), Times.Never);
        }
        #endregion
    }
}
