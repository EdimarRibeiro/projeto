using ProjectManager.Business.Interfaces.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManager.Business
{
    public class UsuarioBusiness : _BusinessBase<Usuario>, IUsuarioBusiness
    {
        public UsuarioBusiness(IConfiguration configuration, IRepositoryBase<Usuario> repository, IUnitOfWork uow) : base(configuration, repository, uow)
        {
        }

        public override async Task Cadastrar(Usuario model)
        {
            if (model.Id == 0) model.Id = ProximoId(model);
            await _repository.Cadastrar(model);
            Commit();
        }

        public override async Task Cadastrar(List<Usuario> models)
        {
            foreach (var model in models)
            {
                if (model.Id == 0) model.Id = ProximoId(model);
            }
            await _repository.Cadastrar(models);
            Commit();
        }
    }
}

