using ProjectManager.Business.Interfaces.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManager.Business
{
    public class SimNaoBusiness : _BusinessBase<SimNao>, ISimNaoBusiness
    {
        public SimNaoBusiness(IConfiguration configuration, IRepositoryBase<SimNao> repository, IUnitOfWork uow) : base(configuration, repository, uow)
        {
        }

        public override async Task Cadastrar(SimNao model)
        {
            if (model.Id == 0) model.Id = (Int16)ProximoId(model);
            await _repository.Cadastrar(model);
            Commit();
        }

        public override async Task Cadastrar(List<SimNao> models)
        {
            foreach (var model in models)
            {
                if (model.Id == 0) model.Id = (Int16)ProximoId(model);
            }
            await _repository.Cadastrar(models);
            Commit();
        }
    }
}

