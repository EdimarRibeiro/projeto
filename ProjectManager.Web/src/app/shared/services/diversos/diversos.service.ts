import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments';
import { Observable, of } from 'rxjs';
import { Entidade } from '@interfaces/geral/entidade';
import { Responsavel } from '@interfaces/geral/responsavel';

@Injectable({ providedIn: 'root' })
export class DiversosService {
    constructor(private http: HttpClient) {}

    getAllSimNao(): Observable<Entidade[]> {
        return this.http.get<Entidade[]>(environment.baseServer + 'simnao');
    }
    
    getAllResponsavel(): Observable<Responsavel[]> {
        return this.http.get<Responsavel[]>(environment.baseServer + 'responsavel');
    }

    getResponsavelAuto(query): Observable<Responsavel[]> {
        return this.http.get<Responsavel[]>(`${environment.baseServer }responsavel/auto/${query}`);
    }

    getAllStatus(): Observable<Entidade[]> {
        const status: Entidade[] = [
            { id: 0, descricao: "Análise" },
            { id: 1, descricao: "Análise Realizada" },
            { id: 2, descricao: "Análise Aprovada" },
            { id: 3, descricao: "Iniciado" },
            { id: 4, descricao: "Planejado" },
            { id: 5, descricao: "Andamento" },
            { id: 6, descricao: "Encerrado" },
            { id: 7, descricao: "Cancelado" },
          ];

        return of(status);
    }

    getAllRisco(): Observable<Entidade[]> {
        const status: Entidade[] = [
            { id: 0, descricao: "Baixo" },
            { id: 1, descricao: "Médio" },
            { id: 2, descricao: "Alto" },
          ];
        return of(status);
    }
}