using ProjectManager.Business.Interfaces.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace ProjectManager.Business.Test
{
    [TestClass]
    public class ProjetoResponsavelBusinessTest
    {
        private IProjetoResponsavelBusiness? _projetoResponsavelBusiness;
        private IRepositoryBase<ProjetoResponsavel>? _business;
        private IConfiguration? _configuration;
        private IUnitOfWork? _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            _configuration = Substitute.For<IConfiguration>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _business = Substitute.For<IRepositoryBase<ProjetoResponsavel>>();
            _projetoResponsavelBusiness = new ProjetoResponsavelBusiness(_configuration, _business, _unitOfWork);
        }

        [TestMethod, TestCategory("UnitTests")]
        public async Task TestMethodAsync()
        {
            var projetoResponsavel = new ProjetoResponsavel();
            if (_business == null || _projetoResponsavelBusiness == null) throw new Exception("Falha na inicialização dos testes!");

            _business.Cadastrar(Arg.Any<ProjetoResponsavel>())
                .Returns(Task.CompletedTask);
            _business.ProximoId(Arg.Any<ProjetoResponsavel>())
                .Returns(1);

            await _projetoResponsavelBusiness.Cadastrar(projetoResponsavel);

            Assert.IsTrue(projetoResponsavel.Id != 0);
        }
    }
}
