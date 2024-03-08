using ProjectManager.Db.Context;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces.Repositories;

namespace ProjectManager.Db.Repositories
{
    public class ProjetoResponsavelRepository : _RepositoryBase<ProjetoResponsavel>, IProjetoResponsavelRepository
    {
        public ProjetoResponsavelRepository(DbProjectManagerContext db) : base(db)
        {
        }
    }
}

