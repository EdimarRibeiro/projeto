import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ViewsComponent } from './views.component';

const routes: Routes = [
    {
        path: '', component: ViewsComponent, children: [
            { path: 'projeto', loadChildren: () => import('./projeto/projeto.module').then(m => m.ProjetoModule) },
            { path: 'gerencial', loadChildren: () => import('./gerencial/gerencial.module').then(m => m.GerencialModule) },       
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ViewsRoutingModule {
}
