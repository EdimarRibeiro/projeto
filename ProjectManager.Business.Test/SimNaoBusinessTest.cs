using ProjectManager.Business.Interfaces.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace ProjectManager.Business.Test
{
    [TestClass]
    public class SimNaoBusinessTest
    {
        private ISimNaoBusiness? _simNaoBusiness;
        private IRepositoryBase<SimNao>? _business;
        private IConfiguration? _configuration;
        private IUnitOfWork? _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            _configuration = Substitute.For<IConfiguration>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _business = Substitute.For<IRepositoryBase<SimNao>>();
            _simNaoBusiness = new SimNaoBusiness(_configuration, _business, _unitOfWork);
        }

        [TestMethod, TestCategory("UnitTests")]
        public async Task TestMethodAsync()
        {
            var simNao = new SimNao();
            if (_business == null || _simNaoBusiness == null) throw new Exception("Falha na inicialização dos testes!");

            _business.Cadastrar(Arg.Any<SimNao>())
                .Returns(Task.CompletedTask);
            _business.ProximoId(Arg.Any<SimNao>())
                .Returns(1);

            await _simNaoBusiness.Cadastrar(simNao);

            Assert.IsTrue(simNao.Id != 0);
        }
    }
}
