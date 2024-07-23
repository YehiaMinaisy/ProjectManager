using AutoMapper;
using ProjectManagerWebAPI.Dtos.Project;
using ProjectManagerWebAPI.Dtos.Task;
using ProjectManagerWebAPI.Dtos.User;

namespace ProjectManagerWebAPI.Dtos.AutoMapper
{
    public class MyAutoMapper : Profile
    {


        public MyAutoMapper()
        {


            CreateMap<Models.Project, ProjectDto>().ReverseMap();
            CreateMap<TaskDto, Models.Task>();
            CreateMap<Models.Task, TaskDto>();

            CreateMap<Models.User, UserDto>().ReverseMap();
            CreateMap<RegisterUserDto, Models.User>();
            CreateMap<Models.Project, GetProjectDto>();
            CreateMap<AddTaskDto, Models.Task>();
        }
    }
}
