using Common_DTOs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DAL.Interfaces
{
    public interface IAreaRepository
    {
        Task<List<ProjectAreas>> GetAllAreasAsync();
        Task<ProjectAreas?> GetAreaByIdAsync(Guid areaId);
        Task<ProjectAreas> CreateAreaAsync(ProjectAreas area);
        Task<ProjectAreas> UpdateAreaAsync(ProjectAreas area);
        Task<bool> DeleteAreaAsync(Guid areaId); 
        Task<bool> AssignPropertyTypesToAreaAsync(Guid areaId, List<int> propertyTypeIds);

    }
}
