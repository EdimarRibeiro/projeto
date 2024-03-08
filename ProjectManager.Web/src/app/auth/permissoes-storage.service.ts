import { Injectable } from '@angular/core';
import { Usuario } from '@interfaces/usuario/usuario';

@Injectable()
export class PermissoesStorageService {
  private nameUsuario = 'usuario';
  
  public clear() {
    localStorage.removeItem(this.nameUsuario);
  }

  public getSequencial() {
    return new Promise(resolve => {

      let sequencial = Number.parseInt(localStorage.getItem('sequancial_001'), 0);
      if ((!sequencial) || (sequencial === 500) || (sequencial === 0)) {
        sequencial = 1;
      } else {
        ++sequencial;
      }
      localStorage.setItem('sequancial_001', sequencial.toString());
      resolve(sequencial);
    });
  }

  public getUsuario(): Usuario {
    const usuario = localStorage.getItem(this.nameUsuario);
    if (usuario != 'undefined') {
      return <Usuario>JSON.parse(usuario);
    } else {
      return null;
    }
  }
}
