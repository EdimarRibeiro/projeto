using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Domain.Entities
{
    [Table("Usuario")]
    public class Usuario : _EntityBase
    {
        [Key]
        public decimal Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Celular { get; set; }
        public string CnpjCpf { get; set; }
        public string FotoUrl { get; set; }

        [ForeignKey("HeAlterarSenha")]
        public Int16 AlterarSenha { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataExpiracao { get; set; }
        [ForeignKey("HeEmailVerificado")]
        public Int16 EmailVerificado { get; set; }
        public Int16 AtivoId { get; set; }
        public Int16 ExcluidoId { get; set; }
        public Guid IdExterno { get; set; }
        public virtual SimNao HeAlterarSenha { get; set; }

        public virtual SimNao Excluido { get; set; }

        public virtual SimNao Ativo { get; set; }

        public virtual SimNao HeEmailVerificado { get; set; }       
    }
}


