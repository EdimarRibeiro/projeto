import { Entidade } from "@interfaces/geral/entidade";

export interface Usuario {
    id: number;
    login: string;
    senha: string;
    nome: string;
    celular: string;
    fotoUrl: string;
    cpfCnpj: string;
    alterarSenha: number;
    dataCadastro: Date;
    dataExpiracao?: Date;
    emailVerificado?: number;
    ativoId: number;
    excluidoId: number;
    idExterno: string;
    HeEmailVerificado?: Entidade;
    HealterarSenha?: Entidade; 
    ativo?: Entidade;
    excluido?: Entidade;
}
