import { Injectable } from '@angular/core';
import { Usuario } from '@interfaces/usuario/usuario';
import { UsuarioStorageService } from '@auth/usuario-storage.service';

@Injectable()
export class UsuarioPermissaoStorageService {

    private nameStorage = 'usuario-permissao';

    constructor(private usuarioStorageService: UsuarioStorageService) {
    }
/*
    public getUsuarioPermissao(): UsuarioGrupoPermissao {
        return <UsuarioGrupoPermissao>JSON.parse(localStorage.getItem(this.nameStorage));
    }
    */

    public setUsuarioPermissao(): UsuarioPermissaoStorageService {
        let usuario: Usuario = this.usuarioStorageService.getUsuarioLogado();
        return this;
    }

    public clear() {
        localStorage.removeItem(this.nameStorage);
    }
}
