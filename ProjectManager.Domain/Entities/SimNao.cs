using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Domain.Entities
{
    [Table("SimNao")]
    public class SimNao : _EntityBase
    {
        [Key]
        public Int16 Id { get; set; }

        public string Descricao { get; set; }

        public Int16 ExcluidoId { get; set; }

        public Guid IdExterno { get; set; }


        //TODO: ADICIONAR NAS RESPECTIVAS CLASSES

    }
}


