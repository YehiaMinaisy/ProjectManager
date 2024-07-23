using Microsoft.EntityFrameworkCore;
using ProjectManagerWebAPI.Data;
using ProjectManagerWebAPI.Dtos.User;
using ProjectManagerWebAPI.Models;
using ProjectManagerWebAPI.Repository.Interfaces;

namespace ProjectManagerWebAPI.Repository
{
    public class UserTasksRepository : IUserTasksRepository
    {
        private readonly ProjectManagerDatabaseContext _context;

        public UserTasksRepository(ProjectManagerDatabaseContext context)
        {
            _context = context;
        }

        public async Task<TaskUser> CreateUserTaskAsync(TaskUser taskUser)
        {
            await _context.TaskUsers.AddAsync(taskUser);
            await _context.SaveChangesAsync();
            return taskUser;
        }

        public async Task<TaskUser> DeleteUserTaskAsync(User user, int id)
        {
            var userTask = await _context.TaskUsers.FirstOrDefaultAsync(x => x.UsersId == user.Id && x.TasksId == id);
            if (userTask == null)
            {
                return null;
            }
            _context.TaskUsers.Remove(userTask);
            await _context.SaveChangesAsync();
            return userTask;
        }

        public async Task<List<UserDto>> GetTaskUsersAsync(int id)
        {
            return await _context.TaskUsers.Where(u => u.TasksId == id)
                .Select(userTask => new UserDto
                {
                    UserName = userTask.Users.UserName,
                    Id = userTask.Users.Id,
                    Email = userTask.Users.Email,
                    PhoneNumber = userTask.Users.PhoneNumber,

                }).ToListAsync();
        }

        public async Task<List<Models.Task>> GetUserTasksAsync(User user)
        {

            return await _context.TaskUsers.Where(u => u.UsersId == user.Id)
                .Select(task => new Models.Task
                {
                    Id = task.TasksId,
                    Name = task.Tasks.Name,
                    Description = task.Tasks.Description,
                    DueDate = task.Tasks.DueDate,
                    ProjectId = task.Tasks.ProjectId,
                    ParentId = task.Tasks.ParentId,
                    SubTasks = task.Tasks.SubTasks,
                    AttachmentData = task.Tasks.AttachmentData,
                    AttachmentName = task.Tasks.AttachmentName,
                    AttachmentType = task.Tasks.AttachmentType,

                }).ToListAsync();
        }
    }
}
