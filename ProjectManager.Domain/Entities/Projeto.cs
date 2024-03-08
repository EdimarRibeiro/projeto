using ProjectManager.Domain.Entities.MachineStates;
using ProjectManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Domain.Entities
{
    [Table("Projeto")]
    public class Projeto : _EntityBase
    {
        private ProjetoMachineStatus _statusStateMachine;

        [Key]
        public decimal Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataTermino { get; set; }
        public DateTime? DataCancelado { get; set; }
        public ProjetoStatus Status
        {
            get
            {
                if (_statusStateMachine == null)
                {
                    _statusStateMachine = new ProjetoMachineStatus();
                }

                return _statusStateMachine.CurrentState;
            }
            set
            {
                if (_statusStateMachine == null)
                {
                    _statusStateMachine = new ProjetoMachineStatus(value);
                }
                else
                {
                    var success = _statusStateMachine.TrySetState(value);

                    if (!success)
                        throw new Exception($"Transição de status inválida. De {Status} -> para {value}");
                }
            }
        }
        public ProjetoRisco Risco { get; set; }
        public decimal ResponsavelId { get; set; }
        public Int16 AtivoId { get; set; }
        public Int16 ExcluidoId { get; set; }
        public Guid IdExterno { get; set; }

        public virtual SimNao Excluido { get; set; }
        public virtual SimNao Ativo { get; set; }
        public virtual Responsavel Responsavel { get; set; }
        public virtual List<ProjetoResponsavel> ProjetoResponsavel { get; set; }
    }
}


