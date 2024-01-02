using TrekTrove.Api.Modules.CustomerModule.Domain.Entities;

namespace TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces
{
    public interface ICustomerService
    {
        Task<int> CreateCustomerAsync(Customer entity);
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllCustomersAsync(int pageNumber, int pageSize);
        Task<Customer> UpdateCustomerAsync(Customer entity);
        Task<bool?> RemoveCustomerByIdAsync(int id);
    }
}
