using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectManagerWebAPI.Dtos.Task;
using ProjectManagerWebAPI.Repository.Interfaces;

namespace ProjectManagerWebAPI.Controllers
{
    [ApiController]
    [Route("api/Task/[action]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _repository;
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepo;
        private readonly IUserTasksRepository _userTaskRepo;

        public TaskController(ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            IUserTasksRepository userTasksRepository,
            IMapper mapper)
        {

            _repository = taskRepository;
            _mapper = mapper;
            _projectRepo = projectRepository;
            _userTaskRepo = userTasksRepository;
        }
        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var tasks = await _repository.GetAllAsync();
            var tasksDto = new List<TaskDto>();
            foreach (var task in tasks)
            {
                tasksDto.Add(_mapper.Map<TaskDto>(task));
            }

            return Ok(tasksDto);
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var task = await _repository.GetAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<TaskDto>(task);
            result.Users = await _userTaskRepo.GetTaskUsersAsync(id);
            return Ok(result);

        }
        [HttpPost("{projectId}")]
        public async Task<IActionResult> Create([FromBody] AddTaskDto taskDto, [FromRoute] int projectId)
        {
            if (!await _projectRepo.ProjectExist(projectId))
            {
                return BadRequest("Project Doesn't exist");
            }


            var task = _mapper.Map<Models.Task>(taskDto);

            await _repository.CreateAsync(task);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, _mapper.Map<TaskDto>(task));

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] AddTaskDto taskDto)
        {

            var task = await _repository.UpdateAsync(id, _mapper.Map<Models.Task>(taskDto));
            if (task == null)
            {
                return NotFound("Task not found");
            }

            return Ok(taskDto);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var task = await _repository.DeleteAsync(id);
            if (task == null)
            {
                return NotFound("Task doesn't exist");
            }
            return NoContent();

        }
    }
}
