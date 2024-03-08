import { Component, OnInit } from "@angular/core";
import { BreadcrumbService } from "@services/layout/breadcrumb/breadcrumb.service";
import { Router } from "@angular/router";
import { ConfirmationService, MessageService } from "primeng/api";

import { FiltroDinamicoService } from "@components/filtro-dinamico/filtro-dinamico.service";
import { ProjetoService } from "@services/projeto/projeto.service";
import { DiversosService } from "@services/diversos/diversos.service";
import { Entidade } from "@interfaces/geral/entidade";
import { Responsavel } from "@interfaces/geral/responsavel";

@Component({
  templateUrl: "./projeto.component.html",
})
export class ProjetoComponent implements OnInit {
  public paginar: any;
  public paginacaoConfig = { numeroRows: 0, totalPagina: 0, numeroPagina: 0, };
  public dataRisco = [];
  public dataStatus = [];
  public configuracaoFiltro: any = {
    tabela: "Projeto",
    filtroPadrao: { tipoCampo: "dateTime", campo: "DataInicio", operacao: "in" },
    dataSourceCampos: [
      { id: "Risco", descricao: "Risco", tipo: "lista", idTipo: "number", dataSource: this.dataRisco },
      { id: "Status", descricao: "Status", tipo: "lista", idTipo: "number", dataSource: this.dataStatus },
      { id: "DataInicio", descricao: "Data Início", tipo: "dateTime", operacao: "in" },
    ],
  };

  constructor(
    private service: ProjetoService,
    private serviceTipo: DiversosService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private serviceFiltroDinamico: FiltroDinamicoService,
  ) {
    this.breadcrumbService.setItems([
      { label: "Projeto", routerLink: "/projeto/projeto" },
    ]);
    this.router.paramsInheritanceStrategy = "emptyOnly";
  }

  ngOnInit(): void {
    this.loadConfDataSet().then(() => {
      this.iniciarConfiguracaoFiltro();
    });
  }

  edit(row) {
    if (row) {
      this.router.paramsInheritanceStrategy = row.id;
      this.router.navigate(["/projeto/projeto/edit"], row.id);
    }
  }

  excluir(row) {
    this.confirmationService.confirm({
      header: "Deseja realmente excluir ?",
      message: row.nome,
      icon: "pi pi-info-circle",

      accept: () => {
        this.service.excluir(row).subscribe(
          () => {
            this.messageService.add({
              key: "001",
              severity: "success",
              summary: "Excluido!",
              detail: row.nome,
            });
            this.iniciarConfiguracaoFiltro();
          },
          (err) => {
            this.messageService.add({
              key: "001",
              life: 5000,
              severity: "error",
              summary: "Não foi possivel Excluir!",
              detail: "Verifique se os itens dessa tabela já foram excluídos " + err.status,
            });
          }
        );
      },
      reject: () => {
        this.messageService.add({
          key: "001",
          severity: "error",
          summary: "Exclusão Cancelada!",
          detail: "",
        });
      },
    });
  }

  ///////////////////////////////// Paginação //////////////////////////////////////
  iniciarConfiguracaoFiltro() {
    this.serviceFiltroDinamico.loadGrid = (numeroGrid: any, pesquisa?: any) => this.loadGrid(numeroGrid, pesquisa);
    this.serviceFiltroDinamico.paginacaoConfig = this.paginacaoConfig;
    this.paginar = (evento, search) => this.serviceFiltroDinamico.paginar(evento, search);
    setTimeout(() => {
      this.serviceFiltroDinamico.filtrar();
    }, 600);
  }

  showChange(event) {
    if (event) this.paginacaoConfig.numeroRows = event.linhas;
  }

  loadConfDataSet() {
    return new Promise(async (resolve) => {
      this.serviceTipo.getAllStatus().subscribe((result: Entidade[]) => {
        const dataSit = result.reduce((current, next) => {
          current.push({ id: next.id, descricao: next.descricao });
          return current;
        }, []);
        this.dataStatus = dataSit;
        this.serviceTipo.getAllRisco().subscribe((result: Entidade[]) => {
          const dataRisco = result.reduce((current, next) => {
            current.push({ id: next.id, descricao: next.descricao });
            return current;
          }, []);
          this.dataRisco = dataRisco;
          this.configuracaoFiltro = {
            tabela: "Projeto",
            filtroPadrao: { tipoCampo: "dateTime", campo: "DataInicio", operacao: "in" },
            dataSourceCampos: [
              { id: "Id", descricao: "Número", tipo: "number" },
              { id: "Status", descricao: "Status", tipo: "lista", idTipo: "number", dataSource: this.dataStatus },
              { id: "DataCancelamento", descricao: "Data Cancel.", tipo: "dateTime", operacao: "in" },
              { id: "DataInicio", descricao: "Data Inicio", tipo: "dateTime", operacao: "in" },
            ],
          };
          resolve(true);
        });
      });
    });
  }

  dataSource() {
    return this.serviceFiltroDinamico?.dataSource;
  }

  loadGrid(numeroGrid, pesquisa?) {
    return new Promise(async (resolve) => {
      await this.service.getAllGrid(numeroGrid, pesquisa).subscribe((result) => {
        resolve({
          quantidadePorPagina: result["records"]?.length ?? 0,
          dados: result["records"] ?? [],
          quantidadeDadosTotais: result["totalRecords"] ?? 0,
        });
      });
    });
  }


  status(row) {
    return new Promise(async (resolve) => {
      await this.service.getStatusId(row.id).subscribe((result) => {
        row.status = result.status;
        resolve(true);
      });
    });
  }

  cancelar(row) {
    return new Promise(async (resolve) => {
      await this.service.getCancelarId(row.id).subscribe((result) => {
        row.status = result.status;
        resolve(true);
      });
    });
  }
}
