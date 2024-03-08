using ProjectManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProjectManager.Db.Mappings
{
    class GeralDefinicoes
    {
        public static void CriarGeralDefinicoes(ModelBuilder modelBuilder)
        {
            //Primary key            
            modelBuilder.Entity<SimNao>().HasKey(t => new { t.Id });
        }
    }
}