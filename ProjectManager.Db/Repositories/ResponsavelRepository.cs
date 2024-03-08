using ProjectManager.Db.Context;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces.Repositories;

namespace ProjectManager.Db.Repositories
{
    public class ResponsavelRepository : _RepositoryBase<Responsavel>, IResponsavelRepository
    {
        public ResponsavelRepository(DbProjectManagerContext db) : base(db)
        {
        }
    }
}

