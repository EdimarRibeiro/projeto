using ProjectManager.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Db.Context
{
    public class UoW : IUnitOfWork
    {
        private readonly DbProjectManagerContext _dbContext;
        public UoW(DbProjectManagerContext dbProjectManagerContext)
        {
            _dbContext = dbProjectManagerContext;
        }

        public Task<bool> Commit()
        {
            var rowsA = _dbContext.SaveChanges();
            var success = (rowsA != 0);
            return new Task<bool>(() => { return success; });
        }

        public Task Rollback()
        {
            var changedEntries = _dbContext.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
