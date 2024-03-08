using ProjectManager.Business.Interfaces.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace ProjectManager.Business.Test
{
    [TestClass]
    public class ResponsavelBusinessTest
    {
        private IResponsavelBusiness? _responsavelBusiness;
        private IRepositoryBase<Responsavel>? _business;
        private IConfiguration? _configuration;
        private IUnitOfWork? _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            _configuration = Substitute.For<IConfiguration>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _business = Substitute.For<IRepositoryBase<Responsavel>>();
            _responsavelBusiness = new ResponsavelBusiness(_configuration, _business, _unitOfWork);
        }

        [TestMethod, TestCategory("UnitTests")]
        public async Task TestMethodAsync()
        {
            var responsavel = new Responsavel();
            if (_business == null || _responsavelBusiness == null) throw new Exception("Falha na inicialização dos testes!");

            _business.Cadastrar(Arg.Any<Responsavel>())
                .Returns(Task.CompletedTask);
            _business.ProximoId(Arg.Any<Responsavel>())
                .Returns(1);

            await _responsavelBusiness.Cadastrar(responsavel);

            Assert.IsTrue(responsavel.Id != 0);
        }
    }
}
