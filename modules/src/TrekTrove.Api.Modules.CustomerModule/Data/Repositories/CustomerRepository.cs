using Dapper;
using TrekTrove.Api.Modules.CustomerModule.Domain.Entities;
using TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces;
using System.Data;

namespace TrekTrove.Api.Modules.CustomerModule.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnection _dbConnection;

        public CustomerRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> CreateCustomerAsync(Customer entity)
        {
            const string query = "INSERT INTO Customer (Name, Description) VALUES (@Name, @Description); SELECT SCOPE_IDENTITY();";
            var id = await _dbConnection.ExecuteScalarAsync<int>(query, entity);
            return id;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageNumber = 10;
            }

            int offset = (pageNumber - 1) * pageSize;

            const string query = "SELECT * FROM Customer ORDER BY Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var param = new { Offset = offset, PageSize = pageSize };
            return await _dbConnection.QueryAsync<Customer>(query, param);
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            const string query = "SELECT * FROM Customer WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Customer>(query, new { Id = id });
        }

        public async Task<Customer> UpdateCustomerAsync(Customer entity)
        {
            const string query = "UPDATE Customer SET Name = @Name, Description = @Description OUTPUT INSERTED.* WHERE Id = @Id";
            var param = new { Id = entity.Id, Name = entity.Name, Description = entity.Description };
            await _dbConnection.ExecuteAsync(query, param);

            return entity;
        }

        public async Task<bool> DeleteCustomerByIdAsync(int id)
        {
            const string query = "DELETE FROM Customer WHERE Id = @Id";
            var parameters = new { Id = id };
            int rowsAffected = await _dbConnection.ExecuteAsync(query, parameters);

            return rowsAffected > 0;
        }
    }
}
