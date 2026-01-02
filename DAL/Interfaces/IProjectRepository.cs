using Common_DTOs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DAL.Interfaces
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllProjectsAsync();
        Task<List<Project>> GetPagedProjectsAsync(string? searchTerm, int pageNumber, int pageSize);
        Task<Project?> GetProjectByIdAsync(Guid projectId);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(Guid projectId);
        Task<bool> DeleteProjectAsync(List<Guid> projectIds);
    }
}
