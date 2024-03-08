import { Component, OnInit } from '@angular/core';
import { BreadcrumbService } from '@services/layout/breadcrumb/breadcrumb.service';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { UsuarioService } from '@services/usuario/usuario.service';
import { FiltroDinamicoService } from '@components/filtro-dinamico/filtro-dinamico.service';

@Component({
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.scss']
})
export class UsuarioComponent implements OnInit {

  public configuracaoFiltro: any = {
    tabela: 'Usuario',
    filtroPadrao: { tipoCampo: "string", campo: 'Nome', operacao: 'contains' },
    dataSourceCampos: [
      { id: 'Id', descricao: 'Código', tipo: 'number' },
      { id: 'Nome', descricao: 'Nome', tipo: 'string' },
    ],
  }
  public paginaAtualConfig = {
    indexAtual: 0,
    numeroRows: null,
    numeroPagina: 1,
    ultimoIndex: null,
  }
    
  public paginacaoConfig = { numeroRows: 0, totalPagina: 0 ,numeroPagina:0,};
  public paginar: any;
  constructor(
    private service: UsuarioService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private fb: FormBuilder,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private serviceFiltroDinamico: FiltroDinamicoService) {

    this.breadcrumbService.setItems([
      { label: 'Gerencial' },
      { label: 'Usuários' }
    ]);
    this.router.paramsInheritanceStrategy = 'emptyOnly';
  }

  ngOnInit(): void {
    this.iniciarConfiguracaoFiltro();
  }

  edit(row) {
    if (row) {
      this.router.paramsInheritanceStrategy = row.id;
      this.router.navigate(['/gerencial/usuario/edit'], row.Id);
    }
  }

  excluir(row) {
    this.confirmationService.confirm({
      header: 'Deseja realmente excluir ?',
      message: row.nome,
      icon: 'pi pi-info-circle',

      accept: () => {
        this.service.excluir(row).subscribe(() => {
          this.messageService.add({ key: '001', severity: 'success', summary: 'Excluido!', detail: '' });
          this.serviceFiltroDinamico.dataSource = this.serviceFiltroDinamico.dataSource.filter(a => a.id !== row.id);
        }, err => {
          this.messageService.add({ key: '001', life: 5000, severity: 'error', summary: 'Não foi possivel Excluir!', detail: 'Verifique se os itens dessa tabela já foram excluídos - ' + err.status });
      });
      },
      reject: () => {
        this.messageService.add({ key: '001', severity: 'error', summary: 'Exclusão Cancelada!', detail: '' });
      }
    });
  }

  ///////////////////////////////// Paginação //////////////////////////////////////

  loadGrid(numeroGrid, pesquisa?) {
    return new Promise(resolve => {
      this.service.getAllGrid(numeroGrid, pesquisa).subscribe(result => {
        resolve({
          quantidadePorPagina: result["records"]?.length ?? 0,
          dados: result["records"] ?? [],
          quantidadeDadosTotais: result["totalRecords"] ?? 0,
        });
      });
    })
  }
  
  iniciarConfiguracaoFiltro() {
    this.serviceFiltroDinamico.loadGrid = (numeroGrid: any, pesquisa?: any) => this.loadGrid(numeroGrid, pesquisa);
    this.serviceFiltroDinamico.paginacaoConfig = this.paginacaoConfig;
    this.paginar = (evento, search) => this.serviceFiltroDinamico.paginar(evento, search)
    setTimeout(() => {
      this.serviceFiltroDinamico.filtrar();
    }, 600);
  }

  showChange(event) {
    if(event) this.paginacaoConfig.numeroRows = event.linhas;
  }  

  dataSource() {
    return this.serviceFiltroDinamico?.dataSource;
  }

}


