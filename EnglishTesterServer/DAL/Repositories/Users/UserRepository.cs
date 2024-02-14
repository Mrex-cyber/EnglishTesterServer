using EnglishTesterServer.DAL.Models.Entities;
using EnglishTesterServer.DAL.Models.Models;

namespace EnglishTesterServer.DAL.Repositories.Users
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private PlatformContext _context;
        public UserRepository(PlatformContext context)
        {
            _context = context;
        }

        public bool AddEntity(UserEntity entity)
        {
            _context.Users.Add(entity);

            return true;
        }

        public IEnumerable<UserEntity> GetEntities()
        {
            throw new NotImplementedException();
        }

        public UserEntity GetEntityById(int id)
        {
            throw new NotImplementedException();
        }

        public bool RemoveEntity(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public bool UpdatedEntity(UserEntity entity)
        {
            throw new NotImplementedException();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public UserEntity GetModelByCredentials(UserCredentialsModel credentials)
        {
            UserEntity user = _context.Users
                .Where(u => 
                    u.Email == credentials.email 
                        && u.Password == credentials.password)
                .Single();

            return user;
        }
    }
}
