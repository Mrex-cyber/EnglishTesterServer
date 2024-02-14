using System.Data.Entity;

namespace EnglishTesterServer.DAL.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public void Save();
        protected void Dispose(bool disposing);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
