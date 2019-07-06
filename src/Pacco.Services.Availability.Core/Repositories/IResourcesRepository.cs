using System;
using System.Threading.Tasks;
using Pacco.Services.Availability.Core.Entities;

namespace Pacco.Services.Availability.Core.Repositories
{
    public interface IResourcesRepository
    {
        Task<Resource> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Resource resource);
        Task UpdateAsync(Resource resource);
        Task DeleteAsync(Guid id);
    }
}