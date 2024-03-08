using ProjectManager.Db.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces.Repositories;

namespace ProjectManager.Db.Test.Repositories
{
    [TestClass]
    public class ProjetoResponsavelRepositoryTest : BaseRepositoryTest
    {
        private IProjetoResponsavelRepository? _projetoResponsavelRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _projetoResponsavelRepository = new ProjetoResponsavelRepository(GetDbProjectManagerContext());
        }

        [TestMethod, TestCategory("UnitTests")]
        public async Task TestMethodAsync()
        {
            var projetoResponsavel = new ProjetoResponsavel() { Id = 9999 };
            if (_projetoResponsavelRepository == null) throw new Exception("Falha na inicialização dos testes!");

            await _projetoResponsavelRepository.Cadastrar(projetoResponsavel);
            var id = projetoResponsavel.Id;
            var guid = projetoResponsavel.IdExterno;
            Assert.IsTrue(guid != Guid.Empty);
            projetoResponsavel.IdExterno = Guid.NewGuid();

            await _projetoResponsavelRepository.Atualizar(projetoResponsavel);
            Assert.IsTrue(guid != projetoResponsavel.IdExterno);

            await _projetoResponsavelRepository.Excluir(projetoResponsavel);

            var newProjetoResponsavel = await _projetoResponsavelRepository.Pesquisar(a => a.Id == id);

            Assert.IsTrue(newProjetoResponsavel.Count() == 0);
        }
    }
}
