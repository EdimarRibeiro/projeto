using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectManager.Domain.Entities;
using System;

namespace ProjectManager.Db.Mappings
{
    class ProjetoDefinicoes
    {
        public static void CriarProjetoDefinicoes(ModelBuilder modelBuilder)
        {
            //Primary key
            modelBuilder.Entity<Responsavel>().HasKey(t => new { t.Id });
            modelBuilder.Entity<Projeto>().HasKey(t => new { t.Id });
            modelBuilder.Entity<ProjetoResponsavel>().HasKey(t => new { t.Id });

            //Foreign key
            modelBuilder.Entity<Projeto>().HasMany(e => e.ProjetoResponsavel).WithOne(e => e.Projeto).HasForeignKey(e => new { e.ProjetoId });
            modelBuilder.Entity<Responsavel>().HasMany(e => e.ProjetoResponsavel).WithOne(e => e.Responsavel).HasForeignKey(e => new { e.ResponsavelId });

            //Definições de campo
            modelBuilder.Entity<Projeto>().Property(e => e.DataInicio).HasColumnType("datetime").HasConversion(new ValueConverter<DateTime, DateTime>(
                    v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, DateTimeKind.Utc) : v,
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                ));
            modelBuilder.Entity<Projeto>().Property(e => e.DataTermino).HasColumnType("datetime").HasConversion(new ValueConverter<DateTime, DateTime>(
                    v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, DateTimeKind.Utc) : v,
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                ));
            modelBuilder.Entity<Projeto>().Property(e => e.DataCancelado).HasColumnType("datetime").HasConversion(new ValueConverter<DateTime, DateTime>(
                    v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, DateTimeKind.Utc) : v,
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                ));
        }
    }
}