import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProtectedGuard } from 'ngx-auth';
import { ProjetoComponent } from './projeto/projeto.component';
import { ProjetoEditComponent } from './projeto/projeto-edit/projeto-edit.component';

const routes: Routes = [
    { path: 'projeto', component: ProjetoComponent, canActivate: [ProtectedGuard] },
    { path: 'projeto/edit', component: ProjetoEditComponent, canActivate: [ProtectedGuard] },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ProjetoRoutingModule {
}