using ProjectManager.Db.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces.Repositories;

namespace ProjectManager.Db.Test.Repositories
{
    [TestClass]
    public class ProjetoRepositoryTest : BaseRepositoryTest
    {
        private IProjetoRepository? _projetoRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _projetoRepository = new ProjetoRepository(GetDbProjectManagerContext());
        }

        [TestMethod, TestCategory("UnitTests")]
        public async Task TestMethodAsync()
        {
            var projeto = new Projeto() { Id = 9999 };
            if (_projetoRepository == null) throw new Exception("Falha na inicialização dos testes!");

            await _projetoRepository.Cadastrar(projeto);
            var id = projeto.Id;
            var guid = projeto.IdExterno;
            Assert.IsTrue(guid != Guid.Empty);
            projeto.IdExterno = Guid.NewGuid();

            await _projetoRepository.Atualizar(projeto);
            Assert.IsTrue(guid != projeto.IdExterno);

            await _projetoRepository.Excluir(projeto);

            var newProjeto = await _projetoRepository.Pesquisar(a => a.Id == id);

            Assert.IsTrue(newProjeto.Count() == 0);
        }
    }
}
