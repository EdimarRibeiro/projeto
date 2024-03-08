import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { tap, map, switchMap, catchError } from 'rxjs/operators';
import { AuthService } from 'ngx-auth';
import { TokenStorageService } from '@auth/token-storage.service';
import { environment } from '@environments';
import { Usuario } from '@interfaces/usuario/usuario';
import { UsuarioStorageService } from '@auth/usuario-storage.service';
import { HelperService } from '@auth/helper/helper.service';
import { PermissoesStorageService } from '@auth/permissoes-storage.service';
import { Router } from '@angular/router';
import { CryptyService } from './CryptyService';

interface AccessData {
    accessToken: string;
    authenticated: boolean;
    created: Date;
    expiration: Date;
    message: string;
    usuario: Usuario;
}

@Injectable()
export class AuthenticationService implements AuthService {

    private URL_LOGIN = environment.baseServer + 'login';
    private URL_REFRESH = environment.baseServer + 'login/refresh';

    constructor(private http: HttpClient,
                private tokenStorageService: TokenStorageService,
                private usuarioStorageService: UsuarioStorageService,
                private permissoesStorageService: PermissoesStorageService,
                private helperService: HelperService,
                private router: Router,
                private cryptyService : CryptyService) {
    }

    public isAuthorized(): Observable<boolean> {
        return this.tokenStorageService.getAccessToken().pipe(map(token => {
            return !!token && !this.helperService.isTokenExpired();
        }));
    }

    public getAccessToken(): Observable<string> {
        return this.tokenStorageService.getAccessToken();
    }

    public login({username, password}) {
        return this.http.post(this.URL_LOGIN, {username, password}).pipe(map((access: AccessData) => {
            if (access.authenticated === true) {
                this.saveAccessData(access);
                this.usuarioStorageService.setUsuarioLogado(access.usuario);
                //this.permissoesStorageService.setPermissoes(access.permissoes);
            }
            return access;
        }));
    }

    public cryptLogin(password : string) {
        return this.cryptyService.encrypt(password);
    }

    public logout(): void {
        this.tokenStorageService.clear();
        this.usuarioStorageService.clear();
        this.permissoesStorageService.clear();
        this.router.navigate(['/login']);
    }

    private saveAccessData({accessToken}: AccessData) {
        this.tokenStorageService.setAccessToken(accessToken);
    }

    refreshShouldHappen(response: HttpErrorResponse): boolean {
        return false;
    }

    refreshToken(): Observable<any> {
        return undefined;
    }

    verifyTokenRequest(url: string): boolean {
        return false;
    }

}
