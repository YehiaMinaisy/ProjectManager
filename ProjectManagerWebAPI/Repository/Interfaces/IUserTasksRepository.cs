using ProjectManagerWebAPI.Dtos.User;
using ProjectManagerWebAPI.Models;

namespace ProjectManagerWebAPI.Repository.Interfaces
{
    public interface IUserTasksRepository
    {
        public Task<List<Models.Task>> GetUserTasksAsync(Models.User user);
        public Task<TaskUser> CreateUserTaskAsync(TaskUser taskUser);
        public Task<TaskUser> DeleteUserTaskAsync(User user, int id);
        public Task<List<UserDto>> GetTaskUsersAsync(int id);
    }
}
