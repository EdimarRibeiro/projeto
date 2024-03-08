using ProjectManager.Db.Context;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces.Repositories;

namespace ProjectManager.Db.Repositories
{
    public class UsuarioRepository : _RepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(DbProjectManagerContext db) : base(db)
        {
        }
    }
}

