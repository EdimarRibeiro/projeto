import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UsuarioComponent } from './usuario/usuario.component';
import { UsuarioEditComponent } from './usuario/usuario-edit/usuario-edit.component';
import { ProtectedGuard } from 'ngx-auth';

const routes: Routes = [

    { path: 'usuario', component: UsuarioComponent, canActivate: [ProtectedGuard] },
    { path: 'usuario/edit', component: UsuarioEditComponent, canActivate: [ProtectedGuard] },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class GerencialRoutingModule {
}