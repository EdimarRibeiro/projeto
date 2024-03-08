using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectManager.Domain.Entities
{
    [Table("ProjetoResponsavel")]
    public class ProjetoResponsavel : _EntityBase
    {
        [Key]
        public decimal Id { get; set; }
        public decimal ProjetoId { get; set; }
        public decimal? ResponsavelId { get; set; }
        public Int16 AtivoId { get; set; }
        public Int16 ExcluidoId { get; set; }
        public Guid IdExterno { get; set; }

        public virtual SimNao Excluido { get; set; }
        public virtual SimNao Ativo { get; set; }
        public virtual Responsavel Responsavel { get; set; }
        [JsonIgnore]
        public virtual Projeto Projeto { get; set; }
    }
}


