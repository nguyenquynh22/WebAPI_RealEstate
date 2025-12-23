using Common_BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProjects([FromQuery] string? searchItem, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var projects = await _projectService.GetPagedProjectsAsync(searchItem, pageNumber, pageSize);
            return Ok(projects);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(Guid id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            return Ok(project);
        }
    }
}