import { Responsavel } from "@interfaces/geral/responsavel";

export interface Projeto {
    id: number;
    nome: string;
    descricao: string;
    dataInicio: Date;
    dataTermino: Date;
    dataCancelamento: Date;
    status: number;
    risco: number;
    responsavelId: number;
    ativoId: number;
    excluidoId: number;
    idExterno: string;
    responsavel?: Responsavel;
}
