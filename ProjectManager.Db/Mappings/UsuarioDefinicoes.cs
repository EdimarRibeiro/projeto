using ProjectManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProjectManager.Db.Mappings
{
    class UsuarioDefinicoes
    {
        public static void CriarUsuarioDefinicoes(ModelBuilder modelBuilder)
        {
            //Primary key
            modelBuilder.Entity<Usuario>().HasKey(t => new {t.Id });
        }
    }
}