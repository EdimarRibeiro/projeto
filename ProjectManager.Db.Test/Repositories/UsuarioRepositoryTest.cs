using ProjectManager.Db.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces.Repositories;

namespace ProjectManager.Db.Test.Repositories
{
    [TestClass]
    public class UsuarioRepositoryTest : BaseRepositoryTest
    {
        private IUsuarioRepository? _usuarioRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _usuarioRepository = new UsuarioRepository(GetDbProjectManagerContext());
        }

        [TestMethod, TestCategory("UnitTests")]
        public async Task TestMethodAsync()
        {
            var usuario = new Usuario() { Id = 9999, ExcluidoId = 0 };
            if (_usuarioRepository == null) throw new Exception("Falha na inicialização dos testes!");

            await _usuarioRepository.Cadastrar(usuario);
            var id = usuario.Id;
            var guid = usuario.IdExterno;
            Assert.IsTrue(guid != Guid.Empty);
            usuario.IdExterno = Guid.NewGuid();

            await _usuarioRepository.Atualizar(usuario);
            Assert.IsTrue(guid != usuario.IdExterno);

            await _usuarioRepository.Excluir(usuario);

            var newUsuario = await _usuarioRepository.Pesquisar(a => a.Id == id);

            Assert.IsTrue(newUsuario.Count() == 0);
        }
    }
}
