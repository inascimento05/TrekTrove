using FluentAssertions;
using Moq;
using TrekTrove.Api.Modules.CustomerModule.Domain.Entities;
using TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces;
using TrekTrove.Api.Modules.CustomerModule.Domain.Services;
using Xunit;

namespace TrekTrove.Api.Modules.CustomerModule.Tests.Domain.Services
{
    public class CustomerServiceTests
    {
        #region Create
        [Fact]
        public async Task CreateCustomerAsync_ValidCustomer_CreatesCustomer()
        {
            // Arrange
            var expectedCustomerId = 1;
            var Template = new Customer
            {
                Name = "Test Customer",
                Description = "Test Description"
            };

            var TemplateRepositoryMock = new Mock<ICustomerRepository>();
            TemplateRepositoryMock.Setup(repo => repo.CreateCustomerAsync(It.IsAny<Customer>()))
                .ReturnsAsync(expectedCustomerId);

            var TemplateService = new CustomerService(TemplateRepositoryMock.Object);

            // Act
            Template.Id = await TemplateService.CreateCustomerAsync(Template);

            // Assert
            Template.Id.Should().Be(expectedCustomerId);

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.CreateCustomerAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task CreateCustomerAsync_NullInput_ThrowsException()
        {
            // Arrange
            var TemplateRepositoryMock = new Mock<ICustomerRepository>();
            var TemplateService = new CustomerService(TemplateRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => TemplateService.CreateCustomerAsync((Customer)null));

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.CreateCustomerAsync(It.IsAny<Customer>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(CreateCustomer_InvalidInputsToTest))]
        public async Task CreateCustomerAsync_InvalidInput_ThrowsException(Customer Template)
        {
            // Arrange
            var TemplateRepositoryMock = new Mock<ICustomerRepository>();
            var TemplateService = new CustomerService(TemplateRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => TemplateService.CreateCustomerAsync(Template));

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.CreateCustomerAsync(It.IsAny<Customer>()), Times.Never);
        }

        public static IEnumerable<object[]> CreateCustomer_InvalidInputsToTest =>
            new List<object[]>
                {
                    new object[] { new Customer { Name = null, Description = "Test Description" } },
                    new object[] { new Customer { Name = "Test Customer", Description = null } },
                    new object[] { new Customer { Name = string.Empty, Description = "Test Description" } },
                    new object[] { new Customer { Name = "Test Customer", Description = string.Empty } },
                    new object[] { new Customer { Name = "Test Customer".PadRight(101,'.'), Description = "Test Description" } },
                    new object[] { new Customer { Name = "Test Customer", Description = "Test Description".PadRight(201,'.') } },
                };
        #endregion

        #region Read
        [Fact]
        public async Task GetAllCustomersAsync_ValidPagination_ReturnZeroCustomers()
        {
            // Arrange
            var expectedCustomer = new List<Customer>();

            var TemplateRepositoryMock = new Mock<ICustomerRepository>();
            TemplateRepositoryMock.Setup(repo => repo.GetAllCustomersAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedCustomer);

            var TemplateService = new CustomerService(TemplateRepositoryMock.Object);

            // Act
            var Templates = await TemplateService.GetAllCustomersAsync(1, 10);

            // Assert
            Templates.Count().Should().Be(0);

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetAllCustomersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ValidPagination_ReturnCustomers()
        {
            // Arrange
            var expectedCustomer = new List<Customer> {
                new Customer
                {
                    Id = 1,
                    Name = "Test Customer",
                    Description = "Test Description"
                },
                new Customer
                {
                    Id = 2,
                    Name = "Test Customer 2",
                    Description = "Test Description 2"
                },
            };

            var TemplateRepositoryMock = new Mock<ICustomerRepository>();
            TemplateRepositoryMock.Setup(repo => repo.GetAllCustomersAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedCustomer);

            var TemplateService = new CustomerService(TemplateRepositoryMock.Object);

            // Act
            var Templates = await TemplateService.GetAllCustomersAsync(1, 10);

            // Assert
            Templates.Count().Should().BeGreaterThanOrEqualTo(1);

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetAllCustomersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(GetAllCustomersAsync_ValidPaginationToTest))]
        public async Task GetAllCustomersAsync_ValidPaginationPagination_ReturnCustomers(int page)
        {
            // Arrange
            var TemplatesFromDatabase = new List<Customer> {
                new Customer
                {
                    Id = 1,
                    Name = "Test Customer",
                    Description = "Test Description"
                },
                new Customer
                {
                    Id = 2,
                    Name = "Test Customer 2",
                    Description = "Test Description 2"
                },
            };

            var returnedCustomers = TemplatesFromDatabase.Skip(page - 1)
                .Take(1);

            var TemplateRepositoryMock = new Mock<ICustomerRepository>();
            TemplateRepositoryMock.Setup(repo => repo.GetAllCustomersAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(returnedCustomers);

            var TemplateService = new CustomerService(TemplateRepositoryMock.Object);

            // Act
            var Templates = await TemplateService.GetAllCustomersAsync(page, 1);

            // Assert
            Templates.Count().Should().Be(1);
            Templates.First().Id.Should().Be(TemplatesFromDatabase[(page - 1)].Id);
            Templates.First().Name.Should().Be(TemplatesFromDatabase[(page - 1)].Name);
            Templates.First().Description.Should().Be(TemplatesFromDatabase[(page - 1)].Description);

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetAllCustomersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        public static IEnumerable<object[]> GetAllCustomersAsync_ValidPaginationToTest =>
            new List<object[]>
                {
                    new object[] { 1 },
                    new object[] { 2 }
                };

        [Theory]
        [MemberData(nameof(GetAllCustomersAsync_InvalidPaginationToTest))]
        public async Task GetAllCustomersAsync_InvalidPaginationPagination_ThrowsException(int pageNumber, int pageSize)
        {
            // Arrange
            var TemplateRepositoryMock = new Mock<ICustomerRepository>();
            var TemplateService = new CustomerService(TemplateRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => TemplateService.GetAllCustomersAsync(pageNumber, pageSize));

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetAllCustomersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        public static IEnumerable<object[]> GetAllCustomersAsync_InvalidPaginationToTest =>
            new List<object[]>
                {
                    new object[] { null, 10 },
                    new object[] { 1, null },
                    new object[] { 0, 10 },
                    new object[] { 1, 0 }
                };

        [Fact]
        public async Task GetCustomerByIdAsync_ValidId_GetCustomer()
        {
            // Arrange
            var expectedCustomer = new Customer
            {
                Id = 1,
                Name = "Test Customer",
                Description = "Test Description"
            };

            var TemplateRepositoryMock = new Mock<ICustomerRepository>();
            TemplateRepositoryMock.Setup(repo => repo.GetCustomerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedCustomer);

            var TemplateService = new CustomerService(TemplateRepositoryMock.Object);

            // Act
            var Template = await TemplateService.GetCustomerByIdAsync(expectedCustomer.Id);

            // Assert
            Template.Id.Should().Be(expectedCustomer.Id);
            Template.Name.Should().Be(expectedCustomer.Name);
            Template.Description.Should().Be(expectedCustomer.Description);

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetCustomerByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_InvalidInput_ThrowsException()
        {
            // Arrange
            var TemplateRepositoryMock = new Mock<ICustomerRepository>();
            var TemplateService = new CustomerService(TemplateRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => TemplateService.GetCustomerByIdAsync(0));

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetCustomerByIdAsync(It.IsAny<int>()), Times.Never);
        }
        #endregion

        #region Update
        [Fact]
        public async Task UpdateCustomerAsync_UpdateDescription_UpdatesCustomer()
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

            var expectedCustomer = new Customer
            {
                Id = TemplateFromDatabase.Id,
                Name = TemplateFromDatabase.Name,
                Description = TemplateToUpdate.Description
            };

            var TemplateRepositoryMock = new Mock<ICustomerRepository>();
            TemplateRepositoryMock.Setup(repo => repo.GetCustomerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(TemplateFromDatabase);

            TemplateRepositoryMock.Setup(repo => repo.UpdateCustomerAsync(It.IsAny<Customer>()))
                .ReturnsAsync(expectedCustomer);

            var TemplateService = new CustomerService(TemplateRepositoryMock.Object);

            // Act
            var updatedCustomer = await TemplateService.UpdateCustomerAsync(TemplateToUpdate);

            // Assert
            updatedCustomer.Id.Should().Be(expectedCustomer.Id);
            updatedCustomer.Name.Should().Be(expectedCustomer.Name);
            updatedCustomer.Description.Should().Be(expectedCustomer.Description);

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetCustomerByIdAsync(It.IsAny<int>()), Times.Once);
            TemplateRepositoryMock.Verify(repo => repo.UpdateCustomerAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomerAsync_NullInput_ThrowsException()
        {
            // Arrange
            var TemplateRepositoryMock = new Mock<ICustomerRepository>();
            var TemplateService = new CustomerService(TemplateRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => TemplateService.UpdateCustomerAsync((Customer)null));

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetCustomerByIdAsync(It.IsAny<int>()), Times.Never);
            TemplateRepositoryMock.Verify(repo => repo.UpdateCustomerAsync(It.IsAny<Customer>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(UpdateCustomer_InvalidInputsToTest))]
        public async Task UpdateCustomerAsync_InvalidInput_ThrowsException(Customer Template)
        {
            // Arrange
            var TemplateRepositoryMock = new Mock<ICustomerRepository>();
            var TemplateService = new CustomerService(TemplateRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => TemplateService.UpdateCustomerAsync(Template));

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.GetCustomerByIdAsync(It.IsAny<int>()), Times.Never);
            TemplateRepositoryMock.Verify(repo => repo.UpdateCustomerAsync(It.IsAny<Customer>()), Times.Never);
        }

        public static IEnumerable<object[]> UpdateCustomer_InvalidInputsToTest =>
            new List<object[]>
                {
                    new object[] { new Customer { Name = "Test Customer", Description = "Test Description" } },
                    new object[] { new Customer { Id = 1, Name = "Test Customer".PadRight(101,'.'), Description = "Test Description" } },
                    new object[] { new Customer { Id = 1, Name = "Test Customer", Description = "Test Description".PadRight(201,'.') } },
                };
        #endregion

        #region Delete
        [Fact]
        public async Task RemoveCustomerByIdAsync_ValidCustomers_RemovesCustomer()
        {
            // Arrange
            var TemplateFromDatabase = new Customer
            {
                Id = 1,
                Name = "Test Customer",
                Description = "Test Description"
            };
            var expected = true;

            var TemplateRepositoryMock = new Mock<ICustomerRepository>();
            TemplateRepositoryMock.Setup(repo => repo.GetCustomerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(TemplateFromDatabase);
            TemplateRepositoryMock.Setup(repo => repo.DeleteCustomerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expected);

            var TemplateService = new CustomerService(TemplateRepositoryMock.Object);

            // Act
            var isDeleted = await TemplateService.RemoveCustomerByIdAsync(1);

            // Assert
            isDeleted.Should().Be(expected);

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.DeleteCustomerByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task RemoveCustomerByIdAsync_InvalidInput_ThrowsException()
        {
            // Arrange
            var TemplateRepositoryMock = new Mock<ICustomerRepository>();
            var TemplateService = new CustomerService(TemplateRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => TemplateService.RemoveCustomerByIdAsync(0));

            // Verify
            TemplateRepositoryMock.Verify(repo => repo.DeleteCustomerByIdAsync(It.IsAny<int>()), Times.Never);
        }
        #endregion
    }
}
