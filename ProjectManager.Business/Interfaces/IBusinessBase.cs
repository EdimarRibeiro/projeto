using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Utils.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProjectManager.Business.Interfaces
{
    public interface IBusinessBase<T> where T : _EntityBase
    {
        public Task<T> ObterPorChave(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        public PagedResult<T> ObterTodos(Pagination pagination, string search, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        public Task<List<T>> ObterTodos(string search, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        public Task Cadastrar(T model);
        public Task Cadastrar(List<T> models);
        public Task Atualizar(T model);
        public Task Atualizar(List<T> models);
        public Task Excluir(T model);
        public Task ExcluirFisico(T model);
        public decimal ProximoId(T model);
        public int ProximoIntId(T model);
        public void Commit();
    }
}
