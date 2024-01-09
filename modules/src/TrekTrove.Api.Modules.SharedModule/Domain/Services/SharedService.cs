using TrekTrove.Api.Modules.SharedModule.Domain.Entities;
using TrekTrove.Api.Modules.SharedModule.Domain.Interfaces;

namespace TrekTrove.Api.Modules.SharedModule.Domain.Services
{
    public class SharedService : ISharedService
    {
        private readonly ISharedRepository _repository;

        public SharedService(ISharedRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateSharedAsync(Shared entity)
        {
            ValidadeCreateShared(entity);

            return await _repository.CreateSharedAsync(entity);
        }

        public async Task<IEnumerable<Shared>> GetAllSharedsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentException(nameof(pageNumber), "PageNumber should be greater or equals than 1.");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentException(nameof(pageSize), "PageSize should be greater or equals than 1.");
            }

            return await _repository.GetAllSharedsAsync(pageNumber, pageSize);
        }

        public async Task<Shared> GetSharedByIdAsync(int id)
        {
            ValidadeSharedId(id);

            return await _repository.GetSharedByIdAsync(id);
        }

        public async Task<Shared> UpdateSharedAsync(Shared entity)
        {
            ValidateUpdateShared(entity);

            var entityFromDatabase = await this.GetSharedByIdAsync(entity.Id);

            if (entityFromDatabase == null)
            {
                return (Shared)null;
            }

            var entityToUpdate = entityFromDatabase;
            entityToUpdate.Name = entity.Name ?? entityFromDatabase.Name;
            entityToUpdate.Description = entity.Description ?? entityFromDatabase.Description;

            var updateShared = await _repository.UpdateSharedAsync(entityToUpdate);

            return updateShared;
        }

        public async Task<bool?> RemoveSharedByIdAsync(int id)
        {
            ValidadeSharedId(id);

            var entityFromDatabase = await this.GetSharedByIdAsync(id);

            if (entityFromDatabase == null)
            {
                return null;
            }

            return await _repository.DeleteSharedByIdAsync(id);
        }

        private void ValidadeSharedId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id), "Id should be greater or equals than 1.");
            }
        }

        private void ValidateSharedNameLength(string name)
        {
            if (!string.IsNullOrEmpty(name) && name.Length > 100)
            {
                throw new ArgumentException("Shared name cannot be longer than 100 characters.");
            }
        }

        private void ValidateSharedDescriptionLength(string description)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 200)
            {
                throw new ArgumentException("Shared description cannot be longer than 200 characters.");
            }
        }

        private void ValidadeSharedNameNullOrEmpty(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Shared name is a required field.");
            }
        }

        private void ValidadeSharedDescriptionNullOrEmpty(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Shared description is a required field.");
            }
        }

        private void ValidadeSharedNull(Shared entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Shared cannot be null.");
            }
        }

        private void ValidadeCreateShared(Shared entity) {
            ValidateUpdateShared(entity);
            ValidadeSharedNameNullOrEmpty(entity.Name);
            ValidadeSharedDescriptionNullOrEmpty(entity.Description);
        }
        
        private void ValidateUpdateShared(Shared entity) {
            ValidadeSharedNull(entity);
            ValidateSharedNameLength(entity.Name);
            ValidateSharedDescriptionLength(entity.Description);
        }
    }
}
