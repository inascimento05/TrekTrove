using Dapper;
using TrekTrove.Api.Modules.SharedModule.Domain.Entities;
using TrekTrove.Api.Modules.SharedModule.Domain.Interfaces;
using System.Data;

namespace TrekTrove.Api.Modules.SharedModule.Data.Repositories
{
    public class SharedRepository : ISharedRepository
    {
        private readonly IDbConnection _dbConnection;

        public SharedRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> CreateSharedAsync(Shared entity)
        {
            const string query = "INSERT INTO Shared (Name, Description) VALUES (@Name, @Description); SELECT SCOPE_IDENTITY();";
            var id = await _dbConnection.ExecuteScalarAsync<int>(query, entity);
            return id;
        }

        public async Task<IEnumerable<Shared>> GetAllSharedsAsync(int pageNumber, int pageSize)
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

            const string query = "SELECT * FROM Shared ORDER BY Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var param = new { Offset = offset, PageSize = pageSize };
            return await _dbConnection.QueryAsync<Shared>(query, param);
        }

        public async Task<Shared> GetSharedByIdAsync(int id)
        {
            const string query = "SELECT * FROM Shared WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Shared>(query, new { Id = id });
        }

        public async Task<Shared> UpdateSharedAsync(Shared entity)
        {
            const string query = "UPDATE Shared SET Name = @Name, Description = @Description OUTPUT INSERTED.* WHERE Id = @Id";
            var param = new { Id = entity.Id, Name = entity.Name, Description = entity.Description };
            await _dbConnection.ExecuteAsync(query, param);

            return entity;
        }

        public async Task<bool> DeleteSharedByIdAsync(int id)
        {
            const string query = "DELETE FROM Shared WHERE Id = @Id";
            var parameters = new { Id = id };
            int rowsAffected = await _dbConnection.ExecuteAsync(query, parameters);

            return rowsAffected > 0;
        }
    }
}
