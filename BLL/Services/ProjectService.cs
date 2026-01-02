using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_BLL.Interfaces;
using Common_DAL.Interfaces;
using Common_DTOs.DTOs;
using Common_DTOs.Entities;

namespace Common_BLL.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<List<ProjectResponseDto>> GetAllProjectsAsync()
        {
            var entities = await _projectRepository.GetAllProjectsAsync();
            return entities.Select(e => MapToDto(e)).ToList();
        }

        public async Task<PagedResultDto<ProjectResponseDto>> GetPagedProjectsAsync(string? searchItem, int pageNumber, int pageSize)
        {
            var pagedEntities = await _projectRepository.GetPagedProjectsAsync(searchItem, pageNumber, pageSize);
            var totalCount = pagedEntities.Count();
            return new PagedResultDto<ProjectResponseDto>
            {
                Items = pagedEntities.Select(e => MapToDto(e)).ToList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ProjectResponseDto?> GetProjectByIdAsync(Guid projectId)
        {
            var entity = await _projectRepository.GetProjectByIdAsync(projectId);
            return entity == null ? null : MapToDto(entity);
        }
        public async Task<ProjectResponseDto> CreateProjectAsync(ProjectCreateRequestDto request)
        {
            var entity = new Project
            {
                ProjectId = Guid.NewGuid(),
                ProjectName = request.ProjectName,
                Description = request.Description ?? string.Empty,
                Location = request.Location ?? string.Empty,
                Developer = request.Developer ?? string.Empty,
                CreatedAt = DateTime.Now,
                Status = "Active"
            };

            var createdEntity = await _projectRepository.CreateProjectAsync(entity);
            return MapToDto(createdEntity);
        }
        public async Task<ProjectResponseDto> UpdateProjectAsync(Guid projectId, ProjectUpdateRequestDto request)
        {
            var existing = await _projectRepository.GetProjectByIdAsync(projectId);

            if (existing == null)
            {
                throw new KeyNotFoundException($"Project với ID {projectId} không tồn tại.");
            }
            if (!string.IsNullOrWhiteSpace(request.ProjectName)) existing.ProjectName = request.ProjectName;
            if (request.Description != null) existing.Description = request.Description;
            if (request.Location != null) existing.Location = request.Location;
            if (request.Developer != null) existing.Developer = request.Developer;

            existing.UpdatedAt = DateTime.Now;

            var updatedEntity = await _projectRepository.UpdateProjectAsync(existing);
            return MapToDto(updatedEntity);
        }
        public async Task<bool> DeleteProjectAsync(Guid projectId)
        {
            return await _projectRepository.DeleteProjectAsync(projectId);
        }

        public async Task<bool> DeleteMultipleProjectsAsync(List<Guid> projectIds)
        {
            if (projectIds == null || !projectIds.Any()) return false;
            return await _projectRepository.DeleteProjectAsync(projectIds);
        }

        private ProjectResponseDto MapToDto(Project entity)
        {
            return new ProjectResponseDto
            {
                ProjectId = entity.ProjectId,
                ProjectName = entity.ProjectName,
                Description = entity.Description,
                Location = entity.Location,
                Developer = entity.Developer,
                CreatedAt = entity.CreatedAt.ToString("dd/MM/yyyy HH:mm"),
                UpdatedAt = entity.UpdatedAt?.ToString("dd/MM/yyyy HH:mm")
            };
        }
    }
}
