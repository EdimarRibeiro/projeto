using ProjectManager.Business.Interfaces.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace ProjectManager.Business.Test
{
    [TestClass]
    public class UsuarioBusinessTest
    {
        private IUsuarioBusiness? _usuarioBusiness;
        private IRepositoryBase<Usuario>? _business;
        private IConfiguration? _configuration;
        private IUnitOfWork? _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            _configuration = Substitute.For<IConfiguration>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _business = Substitute.For<IRepositoryBase<Usuario>>();
            _usuarioBusiness = new UsuarioBusiness(_configuration, _business, _unitOfWork);
        }

        [TestMethod, TestCategory("UnitTests")]
        public async Task TestMethodAsync()
        {
            var usuario = new Usuario();
            if (_business == null || _usuarioBusiness == null) throw new Exception("Falha na inicialização dos testes!");

            _business.Cadastrar(Arg.Any<Usuario>())
                .Returns(Task.CompletedTask);
            _business.ProximoId(Arg.Any<Usuario>())
                .Returns(1);

            await _usuarioBusiness.Cadastrar(usuario);

            Assert.IsTrue(usuario.Id != 0);
        }
    }
}
