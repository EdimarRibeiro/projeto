using System;
using System.Collections.Generic;

namespace ProjectManager.Domain.Entities.MachineStates
{
    public sealed class ProjetoMachineStatus : MachineStatus<ProjetoStatus>
    {
        public ProjetoMachineStatus(ProjetoStatus initialState = ProjetoStatus.Analise, Action<ProjetoStatus> onStateChanged = null) : base(
                  new StateSettings[]
                    {
                        new StateSettings
                        {
                            FromState = ProjetoStatus.Analise,
                            PossibleToStates = new []
                            {
                                ProjetoStatus.AnaliseRealizada, ProjetoStatus.Cancelado
                            }
                        },
                        new StateSettings
                        {
                            FromState = ProjetoStatus.AnaliseRealizada,
                            PossibleToStates = new []
                            {
                                ProjetoStatus.AnaliseAprovada, ProjetoStatus.Cancelado
                            }
                        },
                        new StateSettings
                        {
                            FromState = ProjetoStatus.AnaliseAprovada,
                            PossibleToStates = new []
                            {
                                ProjetoStatus.Iniciado, ProjetoStatus.Cancelado
                            }
                        },
                        new StateSettings
                        {
                            FromState = ProjetoStatus.Iniciado,
                            PossibleToStates = new []
                            {
                                ProjetoStatus.Planejado, ProjetoStatus.Cancelado
                            }
                        },

                        new StateSettings
                        {
                            FromState = ProjetoStatus.Planejado,
                            PossibleToStates = new []
                            {
                                ProjetoStatus.Andamento, ProjetoStatus.Cancelado
                            }
                        },
                        new StateSettings
                        {
                            FromState = ProjetoStatus.Andamento,
                            PossibleToStates = new []
                            {
                                ProjetoStatus.Encerrado
                            }
                        },
                        new StateSettings
                        {
                            FromState = ProjetoStatus.Encerrado,
                            PossibleToStates = new List<ProjetoStatus>()
                        },
                        new StateSettings
                        {
                            FromState = ProjetoStatus.Cancelado,
                            PossibleToStates = new List<ProjetoStatus>()
                        }
                        ,
                    },
                  initialState, onStateChanged
            )
        { }
    }
}
