﻿using TrekTrove.Api.Modules.SharedModule.Domain.Entities;

namespace TrekTrove.Api.Modules.SharedModule.Domain.Interfaces
{
    public interface ISharedRepository
    {
        Task<int> CreateSharedAsync(Shared entity);
        Task<Shared> GetSharedByIdAsync(int id);
        Task<IEnumerable<Shared>> GetAllSharedsAsync(int pageNumber, int pageSize);
        Task<Shared> UpdateSharedAsync(Shared entity);
        Task<bool> DeleteSharedByIdAsync(int id);
    }
}
