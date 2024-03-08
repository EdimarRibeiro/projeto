using ProjectManager.Db.Context;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces.Repositories;

namespace ProjectManager.Db.Repositories
{
    public class SimNaoRepository : _RepositoryBase<SimNao>, ISimNaoRepository
    {
        public SimNaoRepository(DbProjectManagerContext db) : base(db)
        {
        }
    }
}

