using ProjectManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Interfaces
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : _EntityBase
    {
        public DbSet<TEntity> Entidade { get; }
        Task Cadastrar(TEntity entity);
        Task Cadastrar(List<TEntity> entities);
        Task<List<TEntity>> ObterTodos();
        Task Atualizar(TEntity entity);
        Task Atualizar(List<TEntity> entities);
        Task Excluir(TEntity Entity);
        Task ExcluirFisico(TEntity entity);
        Task<IEnumerable<TEntity>> Pesquisar(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null, bool tracking = false);
        Task<int> Commit();
        decimal ProximoId(dynamic model);
        public int ProximoIntId(dynamic model);
        public abstract DbContext Context { get; }
    }
}
