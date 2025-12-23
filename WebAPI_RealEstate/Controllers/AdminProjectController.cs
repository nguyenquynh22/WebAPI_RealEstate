using Common_BLL.Interfaces;
using Common_DTOs.DTOs;
using Common_Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminApi.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [ApiController]
    [Route("api/admin/[controller]")] 
    public class AdminProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public AdminProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects([FromQuery] string? searchItem, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var pagedProjects = await _projectService.GetPagedProjectsAsync(searchItem, pageNumber, pageSize);
            return Ok(pagedProjects);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectCreateRequestDto request)
        {
            var createdProject = await _projectService.CreateProjectAsync(request);
            return Ok(createdProject);
        }

        [HttpPut("{projectId}")]
        public async Task<IActionResult> UpdateProject(Guid projectId, [FromBody] ProjectUpdateRequestDto request)
        {
            var updatedProject = await _projectService.UpdateProjectAsync(projectId, request);
            return Ok(updatedProject);
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            var result = await _projectService.DeleteProjectAsync(projectId);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("bulk-delete")]
        public async Task<IActionResult> DeleteMultipleProjects([FromBody] List<Guid> projectIds)
        {
            var result = await _projectService.DeleteMultipleProjectsAsync(projectIds);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}