using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Npgsql;
using ProjectManager.Db.Mappings;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Db.Context
{
    public class DbProjectManagerContext : DbContext
    {
        public readonly ILogger<DbProjectManagerContext> _logger;

        public DbProjectManagerContext(DbContextOptions dbContextOptions, ILogger<DbProjectManagerContext> logger) : base(dbContextOptions)
        {
            this._logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public void TestarTodasTabelas()
        {
            //-----------------------------------------------------------------           
            TesteEstruturaGeral();
        }

        private void TesteEstruturaGeral()
        {
            //Geral
            SimNao.Take(10).ToList();

            Responsavel.Take(10).ToList();

            Projeto.Take(10).ToList();

            Usuario.Take(10).ToList();

        }

        public DbSet<Proc_Gerador> Proc_Gerador { get; set; }       
        public DbSet<VitualDados> VitualDados { get; set; }
        //-----------------------------------------------------------------
    
        public DbSet<SimNao> SimNao { get; set; }
        public DbSet<Responsavel> Responsavel { get; set; }
        public DbSet<Projeto> Projeto { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            GeralDefinicoes.CriarGeralDefinicoes(modelBuilder);
            ProjetoDefinicoes.CriarProjetoDefinicoes(modelBuilder);
            UsuarioDefinicoes.CriarUsuarioDefinicoes(modelBuilder);           
        }

        public Proc_Gerador GetProc_Gerador(string tabela)
        {
            try
            {
                NpgsqlParameter ptabela = new NpgsqlParameter("@Tabela", tabela ?? (object)DBNull.Value);
                string sqlQuery = "SELECT PROC_GERADOR(@Tabela) as id";

                var dados = this.Proc_Gerador.FromSqlRaw(sqlQuery, ptabela)
                    .ToList();
                this.SaveChanges();
                return dados.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> EntidadeIsAutoGeradorAsync(string name)
        {
            try
            {
                string sqlQuery = "SELECT A.[name] AS tabela, B.[name] AS coluna, B.is_identity FROM sys.objects A " +
                                  "  JOIN sys.columns B ON B.[object_id] = A.[object_id] " +
                                  $" WHERE A.is_ms_shipped = 0 and B.is_identity = 1 and UPPER(A.[name]) = '{name}'";

                return await this.VitualDados.FromSqlRaw(sqlQuery).AnyAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
