using ProjectManagerWebAPI.Models;

namespace ProjectManagerWebAPI.Service
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
