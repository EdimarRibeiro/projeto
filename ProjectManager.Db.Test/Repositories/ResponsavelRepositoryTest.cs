using ProjectManager.Db.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces.Repositories;

namespace ProjectManager.Db.Test.Repositories
{
    [TestClass]
    public class ResponsavelRepositoryTest : BaseRepositoryTest
    {
        private IResponsavelRepository? _responsavelRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _responsavelRepository = new ResponsavelRepository(GetDbProjectManagerContext());
        }

        [TestMethod, TestCategory("UnitTests")]
        public async Task TestMethodAsync()
        {
            var responsavel = new Responsavel() { Id = 999};
            if (_responsavelRepository == null) throw new Exception("Falha na inicialização dos testes!");

            await _responsavelRepository.Cadastrar(responsavel);
            var id = responsavel.Id;
            var guid = responsavel.IdExterno;
            Assert.IsTrue(guid != Guid.Empty);
            responsavel.IdExterno = Guid.NewGuid();

            await _responsavelRepository.Atualizar(responsavel);
            Assert.IsTrue(guid != responsavel.IdExterno);

            await _responsavelRepository.Excluir(responsavel);

            var newResponsavel = await _responsavelRepository.Pesquisar(a => a.Id == id);

            Assert.IsTrue(newResponsavel.Count() == 0);
        }
    }
}
