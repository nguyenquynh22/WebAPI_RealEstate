using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_DTOs.DTOs;
using Common_DTOs.Entities;

namespace Common_BLL.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectResponseDto>> GetAllProjectsAsync();

        Task<PagedResultDto<ProjectResponseDto>> GetPagedProjectsAsync(string? searchItem, int pageNumber, int pageSize);

        Task<ProjectResponseDto?> GetProjectByIdAsync(Guid projectId);
        Task<ProjectResponseDto> CreateProjectAsync(ProjectCreateRequestDto request);
        Task<ProjectResponseDto> UpdateProjectAsync(Guid projectId, ProjectUpdateRequestDto request);
        Task<bool> DeleteProjectAsync(Guid projectId);
        Task<bool> DeleteMultipleProjectsAsync(List<Guid> projectIds);
    }
}
