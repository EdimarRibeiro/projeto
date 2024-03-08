import { Component } from '@angular/core';
import { ViewsComponent } from '@views/views.component';
import { UsuarioStorageService } from '@auth/usuario-storage.service';
import { HomeService } from '@services/top-bar/home.service';

@Component({
    selector: 'app-topbar',
    templateUrl: './topbar.component.html'
})
export class TopbarComponent {
    public usuarioLogado;
    public sistemaVersao = '';
    darkDemoStyle: HTMLStyleElement;

    constructor(public app: ViewsComponent,
        private usuarioStorageService: UsuarioStorageService,
        private serviceHome: HomeService,
    ) {
        this.usuarioLogado = usuarioStorageService.getUsuarioLogado();
        this.versaoSistema();
    }

    changeTheme(event: Event, theme: string, dark: boolean) {
        let themeLink: HTMLLinkElement = <HTMLLinkElement>document.getElementById('theme-css');
        themeLink.href = 'assets/themes/' + theme + '/theme-accent.css';

        if (dark) {
            if (!this.darkDemoStyle) {
                this.darkDemoStyle = document.createElement('style');
                this.darkDemoStyle.type = 'text/css';
                this.darkDemoStyle.innerHTML = '.implementation { background-color: #3f3f3f; color: #dedede} .implementation > h3, .implementation > h4{ color: #dedede}';
                document.body.appendChild(this.darkDemoStyle);
            }
        } else if (this.darkDemoStyle) {
            document.body.removeChild(this.darkDemoStyle);
            this.darkDemoStyle = null;
        }

        event.preventDefault();
    }


    limparCache() {
        window.history.forward();
        window.location.reload();
    }

    versaoSistema() {
        this.serviceHome.getVersao().subscribe(result => {
            this.sistemaVersao = result.versao;
        })
    }
}
