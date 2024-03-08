import { NgModule } from '@angular/core';
import { AUTH_SERVICE, AuthModule, PROTECTED_FALLBACK_PAGE_URI, PUBLIC_FALLBACK_PAGE_URI } from 'ngx-auth';
import { TokenStorageService } from '@auth/token-storage.service';
import { AuthenticationService } from '@auth/authentication.service';
import { UsuarioStorageService } from '@auth/usuario-storage.service';
import { HelperService } from '@auth/helper/helper.service';
import { UsuarioPermissaoStorageService } from '@auth/usuario-permissao-storage.service';
import { PermissoesStorageService } from '@auth/permissoes-storage.service';

export function factory(authenticationService: AuthenticationService) {
    return authenticationService;
}

@NgModule({
    imports: [AuthModule],
    declarations: [],
    providers: [
        HelperService,
        UsuarioStorageService,
        PermissoesStorageService,
        UsuarioPermissaoStorageService,
        TokenStorageService,
        AuthenticationService,    
        {provide: PROTECTED_FALLBACK_PAGE_URI, useValue: '/'},
        {provide: PUBLIC_FALLBACK_PAGE_URI, useValue: '/login'},
        {
            provide: AUTH_SERVICE,
            deps: [AuthenticationService],
            useFactory: factory
        }
    ]
})
export class AuthenticationModule {
}
