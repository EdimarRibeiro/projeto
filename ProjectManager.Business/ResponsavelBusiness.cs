using ProjectManager.Business.Interfaces.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ProjectManager.Business
{
    public class ResponsavelBusiness : _BusinessBase<Responsavel>, IResponsavelBusiness
    {
        public ResponsavelBusiness(IConfiguration configuration, IRepositoryBase<Responsavel> repository, IUnitOfWork uow) : base(configuration, repository, uow)
        {
        }

        public override async Task Cadastrar(Responsavel model)
        {
            if (model.Id == 0) model.Id = ProximoId(model);
            await _repository.Cadastrar(model);
            Commit();
        }

        public override async Task Cadastrar(List<Responsavel> models)
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

