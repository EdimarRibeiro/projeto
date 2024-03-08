import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProtectedGuard, PublicGuard } from 'ngx-auth';
import { PoliticasComponent } from './politicas/politicas.component';

const routes: Routes = [
    { path: 'politicas', component: PoliticasComponent },
    { path: 'login', loadChildren: () => import('./login/login.module').then(m => m.LoginModule), canActivate: [PublicGuard] },
    { path: '', loadChildren: () => import('./views/views.module').then(m => m.ViewsModule), canActivate: [ProtectedGuard] },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
