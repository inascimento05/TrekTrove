using TrekTrove.Api.Modules.CustomerModule.Domain.Entities;
using TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces;

namespace TrekTrove.Api.Modules.CustomerModule.Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateCustomerAsync(Customer entity)
        {
            ValidadeCreateCustomer(entity);

            return await _repository.CreateCustomerAsync(entity);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentException(nameof(pageNumber), "PageNumber should be greater or equals than 1.");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentException(nameof(pageSize), "PageSize should be greater or equals than 1.");
            }

            return await _repository.GetAllCustomersAsync(pageNumber, pageSize);
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            ValidadeCustomerId(id);

            return await _repository.GetCustomerByIdAsync(id);
        }

        public async Task<Customer> UpdateCustomerAsync(Customer entity)
        {
            ValidateUpdateCustomer(entity);

            var entityFromDatabase = await this.GetCustomerByIdAsync(entity.Id);

            if (entityFromDatabase == null)
            {
                return (Customer)null;
            }

            var entityToUpdate = entityFromDatabase;
            entityToUpdate.Name = entity.Name ?? entityFromDatabase.Name;
            entityToUpdate.Description = entity.Description ?? entityFromDatabase.Description;

            var updateCustomer = await _repository.UpdateCustomerAsync(entityToUpdate);

            return updateCustomer;
        }

        public async Task<bool?> RemoveCustomerByIdAsync(int id)
        {
            ValidadeCustomerId(id);

            var entityFromDatabase = await this.GetCustomerByIdAsync(id);

            if (entityFromDatabase == null)
            {
                return null;
            }

            return await _repository.DeleteCustomerByIdAsync(id);
        }

        private void ValidadeCustomerId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id), "Id should be greater or equals than 1.");
            }
        }

        private void ValidateCustomerNameLength(string name)
        {
            if (!string.IsNullOrEmpty(name) && name.Length > 100)
            {
                throw new ArgumentException("Customer name cannot be longer than 100 characters.");
            }
        }

        private void ValidateCustomerDescriptionLength(string description)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 200)
            {
                throw new ArgumentException("Customer description cannot be longer than 200 characters.");
            }
        }

        private void ValidadeCustomerNameNullOrEmpty(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Customer name is a required field.");
            }
        }

        private void ValidadeCustomerDescriptionNullOrEmpty(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Customer description is a required field.");
            }
        }

        private void ValidadeCustomerNull(Customer entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Customer cannot be null.");
            }
        }

        private void ValidadeCreateCustomer(Customer entity) {
            ValidateUpdateCustomer(entity);
            ValidadeCustomerNameNullOrEmpty(entity.Name);
            ValidadeCustomerDescriptionNullOrEmpty(entity.Description);
        }
        
        private void ValidateUpdateCustomer(Customer entity) {
            ValidadeCustomerNull(entity);
            ValidateCustomerNameLength(entity.Name);
            ValidateCustomerDescriptionLength(entity.Description);
        }
    }
}
