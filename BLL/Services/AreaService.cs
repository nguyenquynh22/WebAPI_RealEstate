using Common_BLL.Interfaces;
using Common_DAL.Interfaces;
using Common_DAL.Repositories;
using Common_DTOs.DTOs;
using Common_DTOs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_BLL.Services
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IProjectRepository _projectRepository; 

        public AreaService(IAreaRepository areaRepository, IProjectRepository projectRepository)
        {
            _areaRepository = areaRepository;
            _projectRepository = projectRepository;
        }
        private AreaResponseDto MapToDto(ProjectAreas entity)
        {
            return new AreaResponseDto
            {
                AreaId = entity.AreaId,
                ProjectId = entity.ProjectId,
                AreaName = entity.AreaName,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt.ToString("dd/MM/yyyy HH:mm"),
                UpdatedAt = entity.UpdatedAt?.ToString("dd/MM/yyyy HH:mm")
            };
        }

        public async Task<List<AreaResponseDto>> GetAllAreasAsync()
        {
            var entities = await _areaRepository.GetAllAreasAsync();
            return entities.Select(e => MapToDto(e)).ToList();
        }
        public async Task<AreaResponseDto?> GetAreaByIdAsync(Guid areaId)
        {
            var entity = await _areaRepository.GetAreaByIdAsync(areaId);
            if (entity == null) return null;
            return MapToDto(entity);
        }
        public async Task<AreaResponseDto> CreateAreaAsync(AreaCreateRequestDto request)
        {
            var projectExists = await _projectRepository.GetProjectByIdAsync(request.ProjectId);

            if (projectExists == null)
            {
                // Nếu không tìm thấy ProjectId, ném ra một lỗi để Controller xử lý (trả về 404 hoặc 400)
                throw new KeyNotFoundException($"Dự án với ID {request.ProjectId} không tồn tại.");
            }

            var newArea = new ProjectAreas
            {
                AreaId = Guid.NewGuid(), 
                ProjectId = request.ProjectId,
                AreaName = request.AreaName,
                Description = request.Description,
                CreatedAt = DateTime.Now
            };

            var createdEntity = await _areaRepository.CreateAreaAsync(newArea);
            return MapToDto(createdEntity);
        }
        public async Task<AreaResponseDto> UpdateAreaAsync(AreaUpdateRequestDto request)
        {
            var existing = await _areaRepository.GetAreaByIdAsync(request.AreaId);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Area với ID {request.AreaId} không tồn tại.");
            }
            existing.AreaName = request.AreaName;
            existing.Description = request.Description;
            existing.UpdatedAt = DateTime.Now;
            var updatedEntity = await _areaRepository.UpdateAreaAsync(existing);
            return MapToDto(updatedEntity);
        }
        public async Task<bool> DeleteAreaAsync(Guid areaId)
        {
            return await _areaRepository.DeleteAreaAsync(areaId);
        }
        public async Task<bool> AssignPropertyTypesToAreaAsync(Guid areaId, List<int> propertyTypeIds)
        {
            return await _areaRepository.AssignPropertyTypesToAreaAsync(areaId, propertyTypeIds);
        }
    }
}
