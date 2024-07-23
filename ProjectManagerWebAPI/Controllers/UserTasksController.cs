using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectManagerWebAPI.Extentions;
using ProjectManagerWebAPI.Models;
using ProjectManagerWebAPI.Repository.Interfaces;

namespace ProjectManagerWebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserTasksController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITaskRepository _taskRepo;
        private readonly IUserTasksRepository _userTaskRepo;

        public UserTasksController(UserManager<Models.User> userManager,
            ITaskRepository taskRepository,
            IUserTasksRepository userTasksRepository)
        {
            _userManager = userManager;
            _taskRepo = taskRepository;
            _userTaskRepo = userTasksRepository;

        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> GetAllUserTasks([FromBody] string userName)
        {
            
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) 
            { 
                return NotFound();
            } 
            var userTasks = await _userTaskRepo.GetUserTasksAsync(user);
            return Ok(userTasks);
        }
        [HttpPost("{taskId:int}")]

        public async Task<IActionResult> AddUserTask([FromRoute]int taskId,[FromBody]string userName)
        {
            
            var user = await _userManager.FindByNameAsync(userName);
            var task = await _taskRepo.GetAsync(taskId);
            if (task == null)
            {
                return BadRequest("task not found");
            }
            var userTasks = await _userTaskRepo.GetUserTasksAsync(user);
            if (userTasks.Any(e => e.Id == taskId))
            {
                return BadRequest("User already asigned to this task");
            }
            var UserTask = new TaskUser
            {
                UsersId = user.Id,
                TasksId = taskId,
            };
            var result = await _userTaskRepo.CreateUserTaskAsync(UserTask);
            if (result == null)
            {
                return StatusCode(500, "User task Creatin failed");
            }
            return Created();



        }
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteUserTask([FromRoute] int id,[FromBody] string username)
        {
            
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest("User not Found");
            }
            var userTasks = await _userTaskRepo.GetUserTasksAsync(user);
            var task = userTasks.Where(x => x.Id == id).FirstOrDefault();
            if (task != null)
            {
                var result = await _userTaskRepo.DeleteUserTaskAsync(user, id);
                if (result == null)
                {
                    return StatusCode(500, "user's Task wasn't deleted");
                }
                return Ok();
            }
            else
            {
                return BadRequest("user can't delete this task");
            }



        }


    }
}
