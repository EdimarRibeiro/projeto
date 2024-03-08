using ProjectManager.Db.Context;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces.Repositories;

namespace ProjectManager.Db.Repositories
{
    public class ProjetoRepository : _RepositoryBase<Projeto>, IProjetoRepository
    {
        public ProjetoRepository(DbProjectManagerContext db) : base(db)
        {
        }
    }
}

