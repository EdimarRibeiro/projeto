import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments';
import { Observable } from 'rxjs';
import { ProjetoResponsavel } from '@interfaces/projeto/projeto-responsavel';

@Injectable()
export class ProjetoResponsavelService {

    private URL = environment.baseServer + 'ProjetoResponsavel';

    constructor(private http: HttpClient) {
    }

    getAll(projetoId: number): Observable<ProjetoResponsavel[]> {
        return this.http.get<ProjetoResponsavel[]>(`${this.URL}/all/${projetoId}`);
    }

    getAllGrid(page: number, search: string): Observable<ProjetoResponsavel[]> {
        return this.http.get<ProjetoResponsavel[]>(`${this.URL}/grid/?pagina=${page}&&pesquisa=${search}`);
    }

    getProjetoResponsavelId(projetoId): Observable<ProjetoResponsavel> {
        return this.http.get<ProjetoResponsavel>(`${this.URL}/${projetoId}`);
    }    

    getId(id: number): Observable<ProjetoResponsavel> {
        return this.http.get<ProjetoResponsavel>(this.URL + '/' + id);
    }

    salvar(ProjetoResponsavel) {
        if (ProjetoResponsavel.edit) {
            return this.http.put(this.URL + '/' + ProjetoResponsavel.id, ProjetoResponsavel);
        } else {
            return this.http.post(this.URL, ProjetoResponsavel);
        }
    }

    excluir(ProjetoResponsavel) {
        return this.http.delete(this.URL + '/' + ProjetoResponsavel.id, ProjetoResponsavel);
    }

}
