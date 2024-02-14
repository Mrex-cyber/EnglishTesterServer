using EnglishTesterServer.DAL.Repositories.Main_Page;
using EnglishTesterServer.DAL.Repositories.Tests;
using EnglishTesterServer.DAL.Repositories.Users;

namespace EnglishTesterServer.DAL.UnitsOfWork
{
    public class UnitOfWorkPlatform : IUnitOfWork
    {
        private PlatformContext context = new PlatformContext();

        private TestRepository _testRepository;
        private MainPageRepository _mainPageRepository;
        private UserRepository _userRepository;

        public UserRepository UserRepository
        {
            get
            {
                if (this._userRepository == null)
                {
                    this._userRepository = new UserRepository(context);
                }

                return this._userRepository;
            }
        }
        public TestRepository TestRepository
        {
            get
            {
                if (this._testRepository == null)
                {
                    this._testRepository = new TestRepository(context);
                }
                
                return this._testRepository;
            }
        }

        public MainPageRepository MainPageRepository
        {
            get
            {
                if (this._mainPageRepository == null)
                {
                    this._mainPageRepository = new MainPageRepository(context);
                }
                return this._mainPageRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;
        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
