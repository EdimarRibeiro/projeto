import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments';
import { Observable } from 'rxjs';
import { Usuario } from '@interfaces/usuario/usuario';

@Injectable()
export class UsuarioService {

    private URL = environment.baseServer + 'usuario';

    constructor(private http: HttpClient) {
    }

    getAll(): Observable<Usuario[]> {
        return this.http.get<Usuario[]>(this.URL);
    }

    getId(id): Observable<Usuario> {
        return this.http.get<Usuario>(this.URL + '/' + id);
    }

    getAllGrid(page: number, search: string): Observable<Usuario[]> {
        return this.http.get<Usuario[]>(`${this.URL}/grid/?pagina=${page}&&pesquisa=${search}`);
    }
    
    salvar(usuario) {
        if (usuario.edit) {
            return this.http.put(this.URL + '/' + usuario.id, usuario);
        } else {
            return this.http.post(this.URL, usuario);
        }
    }

    excluir(usuario) {
        return this.http.delete(this.URL + '/' + usuario.id, usuario);
    }
}
