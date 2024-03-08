import { Entidade } from '@interfaces/geral/entidade';
import { Responsavel } from '@interfaces/geral/responsavel';
import { Projeto } from './projeto';

export interface ProjetoResponsavel {
    id: number;
    projetoId: number;
    responsavelId?: number;
    ativoId: number;
    excluidoId: number;
    idExterno: string;
    excluido?: Entidade;
    ativo?: Entidade;
    projeto?: Projeto;
    responsavel?: Responsavel;
}
