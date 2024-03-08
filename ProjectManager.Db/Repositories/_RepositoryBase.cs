using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ProjectManager.Db.Context;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Domain.Utils.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ProjectManager.Db.Repositories
{
    public class _RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : _EntityBase, new()
    {
        protected readonly DbProjectManagerContext _db;
        protected readonly DbSet<TEntity> _dbSet;

        public _RepositoryBase(DbProjectManagerContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }

        public DbSet<TEntity> Entidade
        {
            get { return _dbSet; }
        }

        public DbContext Context
        {
            get { return _db; }
        }

        public async Task<IEnumerable<TEntity>> Pesquisar(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null, bool tracking = false )
        {
            var entity = tracking ? _dbSet : _dbSet.AsNoTracking();
            if (predicate != null) entity = entity.Where(predicate);
            if (includes != null) entity = includes(entity);
            return await entity.ToListAsync();
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task Cadastrar(TEntity entity)
        {
            SetValueExternalId(entity);
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task Cadastrar(List<TEntity> entities)
        {
            entities.ForEach(entity => SetValueExternalId(entity));
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            await Task.FromResult(_dbSet.Update(entity));
        }

        public virtual async Task Atualizar(List<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
            await Task.CompletedTask;
        }

        public virtual async Task Excluir(TEntity entity)
        {
            PropertyInfo excluidoId = entity.GetType().GetProperties().Where(y => y.Name == "ExcluidoId").FirstOrDefault();
            if (excluidoId != null && excluidoId.GetValue(entity) != null)
                excluidoId.SetValue(entity, (short)1);

            await Task.FromResult(_dbSet.Update(entity));
        }

        public virtual async Task ExcluirFisico(TEntity entity)
        {
            await Task.FromResult(_dbSet.Remove(entity));
        }

        public async Task<int> Commit()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db?.Dispose();
        }

        public decimal ProximoId(dynamic model)
        {
            var name = model.ToString().Split(".")[3];
            return (_db.GetProc_Gerador(name)).Id;
        }

        public int ProximoIntId(dynamic model)
        {
            var name = model.ToString().Split(".")[3];
            return decimal.ToInt32((_db.GetProc_Gerador(name).Id));
        }

        private bool TabelaAutoGerador(string name)
        {
           return _db.EntidadeIsAutoGeradorAsync(name.ToUpper()).Result;
        }

        private static void SetValueExternalId(TEntity entity)
        {
            PropertyInfo idExterno = entity.GetType().GetProperties().Where(y => y.Name == "IdExterno").FirstOrDefault();
            PropertyInfo ativoId = entity.GetType().GetProperties().Where(y => y.Name == "AtivoId").FirstOrDefault();
            PropertyInfo excluidoId = entity.GetType().GetProperties().Where(y => y.Name == "ExcluidoId").FirstOrDefault();

            if (idExterno != null && (Guid)idExterno.GetValue(entity) == Guid.Empty)
                idExterno.SetValue(entity, Guid.NewGuid());

            if (ativoId != null && ativoId.GetValue(entity) == null)
                ativoId.SetValue(entity, 1);

            if (excluidoId != null && excluidoId.GetValue(entity) == null)
                excluidoId.SetValue(entity, 0);
        }
    }
}
