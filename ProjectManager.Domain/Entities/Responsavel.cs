using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Domain.Entities
{
    public class Responsavel : _EntityBase
    {
        [Key]
        public decimal Id { get; set; }
        public string Nome { get; set; }    
        public string Sobrenome { get; set; }
        public string Email {  get; set; }
        public string Codigo {  get; set; }
        public Int16 AtivoId { get; set; }
        public Int16 ExcluidoId { get; set; }
        public Guid IdExterno { get; set; }

        public virtual SimNao Excluido { get; set; }
        public virtual SimNao Ativo { get; set; }

        [JsonIgnore]
        public virtual List<ProjetoResponsavel> ProjetoResponsavel { get; set; }
    }
}