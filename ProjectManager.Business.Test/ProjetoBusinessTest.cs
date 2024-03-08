using ProjectManager.Business.Interfaces.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace ProjectManager.Business.Test
{
    [TestClass]
    public class ProjetoBusinessTest
    {
        private IProjetoBusiness? _projetoBusiness;
        private IRepositoryBase<Projeto>? _business;
        private IConfiguration? _configuration;
        private IUnitOfWork? _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            _configuration = Substitute.For<IConfiguration>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _business = Substitute.For<IRepositoryBase<Projeto>>();
            _projetoBusiness = new ProjetoBusiness(_configuration, _business, _unitOfWork);
        }

        [TestMethod, TestCategory("UnitTests")]
        public async Task TestMethodAsync()
        {
            var projeto = new Projeto();
            if (_business == null || _projetoBusiness == null) throw new Exception("Falha na inicialização dos testes!");

            _business.Cadastrar(Arg.Any<Projeto>())
                .Returns(Task.CompletedTask);
            _business.ProximoId(Arg.Any<Projeto>())
                .Returns(1);

            await _projetoBusiness.Cadastrar(projeto);

            Assert.IsTrue(projeto.Id != 0);
        }
    }
}
