import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments';
import { Observable } from 'rxjs';
import { Projeto } from '@interfaces/projeto/projeto';
import { Responsavel } from '@interfaces/geral/responsavel';

@Injectable()
export class ProjetoService {

    private URL = environment.baseServer + 'projeto';
    public empresaFilialId: number;

    constructor(private http: HttpClient) {
    }
    
    getAll(): Observable<Projeto[]> {
        return this.http.get<Projeto[]>(`${this.URL}`);
    }

    getResponsavelAuto(query): Observable<Responsavel[]> {
        return this.http.get<Responsavel[]>(`${this.URL}/responsavel/auto/${query}`);
    }

    getResponsavelCreate(): Observable<Responsavel> {
        return this.http.get<Responsavel>(`${this.URL}/responsavel`);
    }

    getAllGrid(page: number, search: string): Observable<Projeto[]> {
        return this.http.get<Projeto[]>(`${this.URL}/grid/?pagina=${page}&&pesquisa=${search}`);
    }

    getProjetoId(id): Observable<Projeto> {
        return this.http.get<Projeto>(`${this.URL}/${this.empresaFilialId}/${id}`);
    }    

    getId(id: number): Observable<Projeto> {
        return this.http.get<Projeto>(this.URL + '/' + id);
    }

    getStatusId(id: number): Observable<Projeto> {
        return this.http.get<Projeto>(this.URL + '/status/' + id);
    }

    getCancelarId(id: number): Observable<Projeto> {
        return this.http.get<Projeto>(this.URL + '/cancelar' + id);
    }

    salvar(projeto) {
        if (projeto.edit) {
            return this.http.put(this.URL + '/' + projeto.id, projeto);
        } else {
            return this.http.post(this.URL, projeto);
        }
    }

    excluir(projeto) {
        return this.http.delete(this.URL + '/' + projeto.id, projeto);
    }

}
