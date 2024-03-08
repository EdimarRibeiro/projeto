using ProjectManager.Db.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces.Repositories;

namespace ProjectManager.Db.Test.Repositories
{
    [TestClass]
    public class CidadeRepositoryTest : BaseRepositoryTest
    {
        private ICidadeRepository _cidadeRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _cidadeRepository = new CidadeRepository(GetDbProjectManagerContext());
        }

        [TestMethod, TestCategory("UnitTests")]
        public async Task TestMethodAsync()
        {
            var cidade = new Cidade() {Id = 9999, ExcluidoId = 0 };

            await _cidadeRepository.Cadastrar(cidade);
            var id = cidade.Id;
            var guid = cidade.IdExterno;
            Assert.IsTrue(guid != Guid.Empty);
            cidade.IdExterno = Guid.NewGuid();

            await _cidadeRepository.Atualizar(cidade);
            Assert.IsTrue(guid != cidade.IdExterno);

            await _cidadeRepository.Excluir(cidade);

            var newCidade = await _cidadeRepository.Pesquisar(a=> a.Id == id);

            Assert.IsTrue(newCidade.Count() == 0);
        }
    }
}
