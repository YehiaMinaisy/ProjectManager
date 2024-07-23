using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectManagerWebAPI.Dtos.Project;
using ProjectManagerWebAPI.Repository.Interfaces;

namespace ProjectManagerWebAPI.Controllers
{
    [Route("api/Project/[action]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _repository;
        private readonly IMapper _mapper;

        public ProjectController(IProjectRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _repository.GetAllAsync();
            return Ok(projects);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var project = await _repository.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound(new NotFoundObjectResult("project not found"));

            }
            return Ok(project);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectDto projectDto)
        {
            var project = _mapper.Map<Models.Project>(projectDto);
            await _repository.CreateAsync(project);
            return CreatedAtAction(nameof(Get), new { id = project.ProjectId }, project);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProjectDto projectDto)
        {
            var project = await _repository.UpdateAsync(id, projectDto);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(projectDto);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var project = await _repository.DeleteAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            return NoContent();
        }


    }
}
