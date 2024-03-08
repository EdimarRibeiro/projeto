import { Injectable } from '@angular/core';
import { Usuario } from '@interfaces/usuario/usuario';

@Injectable()
export class UsuarioStorageService {

    private nameStorage = 'usuario';

    public getUsuarioLogado(): Usuario {
        return <Usuario>JSON.parse(localStorage.getItem(this.nameStorage));
    }

    public setUsuarioLogado(usuario: Usuario): UsuarioStorageService {
        localStorage.setItem(this.nameStorage, JSON.stringify(usuario));
        return this;
    }

    public clear() {
        localStorage.removeItem(this.nameStorage);
    }
}
