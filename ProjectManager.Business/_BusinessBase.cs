using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using ProjectManager.Business.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Domain.Utils.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProjectManager.Business
{
    public class _BusinessBase<T> : IBusinessBase<T> where T : _EntityBase, new()
    {
        protected IRepositoryBase<T> _repository;
        protected IUnitOfWork _uow;
        protected IConfiguration _configuration { get; set; }

        public _BusinessBase(IConfiguration configuration, IRepositoryBase<T> repository, IUnitOfWork uow)
        {
            _configuration = configuration;
            _repository = repository;
            _uow = uow;
        }

        public virtual async Task Atualizar(List<T> models)
        {
            foreach (var model in models)
                await _repository.Atualizar(model);
            Commit();
        }

        public virtual async Task Atualizar(T model)
        {
            await _repository.Atualizar(model);
            Commit();
        }

        public virtual async Task Cadastrar(List<T> models)
        {
            foreach (var model in models)
                await _repository.Cadastrar(model);
            Commit();
        }

        public virtual async Task Cadastrar(T model)
        {
            await _repository.Cadastrar(model);
            Commit();
        }

        public void Commit()
        {
            _uow.Commit();
        }

        public virtual async Task Excluir(T model)
        {
            await _repository.Excluir(model);
            Commit();
        }

        public virtual async Task ExcluirFisico(T model)
        {
            await _repository.ExcluirFisico(model);
            Commit();
        }

        public virtual decimal ProximoId(T model)
        {
            return _repository.ProximoId(model);
        }

        public virtual int ProximoIntId(T model)
        {
            return _repository.ProximoIntId(model);
        }

        public virtual async Task<T> ObterPorChave(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            return await Task.FromResult(_repository.Pesquisar(predicate, includes, false).Result.FirstOrDefault());
        }

        public virtual PagedResult<T> ObterTodos(Pagination pagination, string search, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            int pageSize = _configuration.GetValue<int>("RegistroPorPagina");
            if (pagination == null) new Pagination { PageSize = pageSize, Page = 20 };

            var model = _repository.Entidade.AsNoTracking();
            var excluido = Interpreter.ParsePredicate<T>("(ExcluidoId == 0)").Result;
            if (excluido != null)
            {
                if (predicate == null)
                    predicate = excluido;
                else
                    predicate = predicate.And(excluido);
            }
            if (predicate != null)
                model = model.Where(predicate);

            Expression<Func<T, bool>> where = null;
            if (!string.IsNullOrEmpty(search))
            {
                EvaluationResult<T, bool> evaluationResult = Interpreter.ParsePredicate<T>(search);
                where = evaluationResult.Result;
            }

            if (where != null)
            {
                model = model.Where(where);
            }
            
            if (includes != null)
            {
                model = includes(model);
            }
            return model.ToPagedResult<T>(pagination);
        }

        public virtual async Task<List<T>> ObterTodos(string search, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            Expression<Func<T, bool>> where = null;

            if (!string.IsNullOrEmpty(search))
            {
                where = Interpreter.ParsePredicate<T>(search).Result;
            }
            if (where != null)
            {
                predicate = predicate.And(where);
            }            
            var excluido = Interpreter.ParsePredicate<T>("(ExcluidoId == 0)").Result;
            if (excluido != null)
            {
                if (predicate == null)
                    predicate = excluido;
                else
                    predicate = predicate.And(excluido);
            }
            return await Task.FromResult(_repository.Pesquisar(predicate, includes).Result.ToList());
        }
    }
}
