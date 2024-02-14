using EnglishTesterServer.DAL.Models.Entities;
using EnglishTesterServer.DAL.Models.Models;

namespace EnglishTesterServer.DAL.Repositories.Users
{
    public interface IUserRepository : ICrud<UserEntity>, IDisposable
    {
        UserEntity GetModelByCredentials(UserCredentialsModel credentials);
    }
}
