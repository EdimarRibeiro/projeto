using ProjectManager.Db.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces.Repositories;

namespace ProjectManager.Db.Test.Repositories
{
    [TestClass]
    public class SimNaoRepositoryTest : BaseRepositoryTest
    {
        private ISimNaoRepository? _simNaoRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _simNaoRepository = new SimNaoRepository(GetDbProjectManagerContext());
        }

        [TestMethod, TestCategory("UnitTests")]
        public async Task TestMethodAsync()
        {
            var simNao = new SimNao() { Id = 9999, ExcluidoId = 0 };
            if (_simNaoRepository == null) throw new Exception("Falha na inicialização dos testes!");

            await _simNaoRepository.Cadastrar(simNao);
            var id = simNao.Id;
            var guid = simNao.IdExterno;
            Assert.IsTrue(guid != Guid.Empty);
            simNao.IdExterno = Guid.NewGuid();

            await _simNaoRepository.Atualizar(simNao);
            Assert.IsTrue(guid != simNao.IdExterno);

            await _simNaoRepository.Excluir(simNao);

            var newSimNao = await _simNaoRepository.Pesquisar(a => a.Id == id);

            Assert.IsTrue(newSimNao.Count() == 0);
        }
    }
}
