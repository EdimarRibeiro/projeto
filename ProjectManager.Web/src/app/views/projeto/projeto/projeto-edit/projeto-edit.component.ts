import { Component, OnInit } from '@angular/core';
import { BreadcrumbService } from '@services/layout/breadcrumb/breadcrumb.service';
import { Router } from '@angular/router';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { formatDate, Location } from '@angular/common';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Projeto } from '@interfaces/projeto/projeto';
import { ProjetoService } from '@services/projeto/projeto.service';
import { ProjetoResponsavelService } from '@services/projeto/projeto-responsavel.service';
import { ProjetoResponsavel } from '@interfaces/projeto/projeto-responsavel';
import { DiversosService } from '@services/diversos/diversos.service';
import { Entidade } from '@interfaces/geral/entidade';

@Component({
  templateUrl: './projeto-edit.component.html',
})

export class ProjetoEditComponent implements OnInit {
  public resultsResponsavel: any[];
  public isCadastraResponsavel = false;
  public dataSourceResponsavel: ProjetoResponsavel[];
  public DsStatus: Entidade[];
  public DsRisco: Entidade[];
  public DsSimNao: Entidade[];
  public DsResponsavel: any[];

  public responsavel;

  public formModel: FormGroup;
  public index = 0;
  public dados;
  public nomeBotao = '';
  public submitted = false;
  public salvando = false;

  constructor(private service: ProjetoService,
    private serviceProjetoResp: ProjetoResponsavelService,
    private serviceTipo: DiversosService,
    private serviceResponsavel: DiversosService,
    private breadcrumbService: BreadcrumbService,
    private fb: FormBuilder,
    private router: Router,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {

    this.breadcrumbService.setItems([
      { label: 'Projetos' },
      { label: 'Projeto', routerLink: '/projeto/projeto' },
      { label: 'Edição' }
    ]);
  }

  ngOnInit() {
    this.nomeBotao = 'Salvar e continuar';
    this.createForm();
    const projetoId = Number.parseInt(this.router.paramsInheritanceStrategy.toString());
    this.loadConfDataSet().then(() => {
      if (projetoId > 0) {
        this.loadResponsavel(projetoId).then(() => {
          this.editar(projetoId);
        });
      }
      this.formModel.controls["dataInicio"].setValue(new Date(formatDate(new Date(), 'yyyy-MM-ddTHH:mm', 'en')));
      this.formModel.controls["dataCancelamento"].disable();
    });
  }

  private createForm() {
    this.formModel = this.fb.group({
      empresaFilialId: null,
      id: null,
      responsavelId: new FormControl('', Validators.required),
      nome: new FormControl('', Validators.required),
      descricao: new FormControl('', Validators.required),
      dataInicio: new FormControl('', Validators.required),
      dataCancelamento: null,
      dataTermino: null,
      risco: new FormControl('', Validators.required),
      status: new FormControl('', Validators.required),
      ativoId: new FormControl("1", Validators.required),
      idExterno: null,
      edit: false
    });
  }

  async editar(projetoId) {
    this.nomeBotao = 'Salvar';
    await this.service.getId(projetoId).subscribe((result: Projeto) => {
      var status = this.DsStatus.filter(a => a.id === result.status)[0];
      var risco = this.DsRisco.filter(a => a.id === result.risco)[0];
      this.DsResponsavel = [result?.responsavel];
      this.formModel.reset({
        edit: true,
        id: result.id,
        nome: result.nome,
        descricao: result.descricao,
        dataInicio: result.dataInicio ? new Date(formatDate(result.dataInicio, 'yyyy-MM-ddTHH:mm', 'en')) : null,
        dataCancelamento: result.dataCancelamento ? new Date(formatDate(result.dataCancelamento, 'yyyy-MM-ddTHH:mm', 'en')) : null,
        dataTermino: new Date(result.dataTermino) > new Date("0001-01-01 03:06:28") ? new Date(formatDate(result.dataTermino, 'yyyy-MM-ddTHH:mm', 'en')) : null,
        responsavelId: { id: result.responsavelId, nome: result?.responsavel?.nome, sobrenome:  result?.responsavel?.sobrenome,  descricao: result.responsavelId + " - " +  result?.responsavel?.nome },
        status: { id: result.status, descricao: status.descricao },
        risco: { id: result.risco, descricao: risco.descricao },
        idExterno: result.idExterno,
        ativoId: result.ativoId,
      });
    });
  }

  async salvar() {
    this.submitted = true;
    if (this.formModel.valid && !this.salvando) {
      const form = JSON.parse(JSON.stringify(this.formModel.value));

      await this.service.salvar(this.FormartarModel(form)).subscribe((result) => {
        this.salvando = false;
        this.router.navigate(["/projeto/projeto"]);
      }, error => {
        this.salvando = false;
        var msg = error.error ? error.error.split(':')[1].split('.')[0] : error.statusText;
        this.messageService.add({ key: '001', severity: 'info', summary: 'Falha ao salvar dados!', detail: msg });
      });
    }
  }

  FormartarModel(model) {
    if (model.responsavelId)
      model.responsavelId = model.responsavelId.id;

    if (model.status)
      model.status = model.status.id

    if (model.risco)
      model.risco = model.risco.id

    return model;
  }

  pesquisarResponsavel(event: any) {
    this.serviceResponsavel.getResponsavelAuto(event.query).subscribe((result) => {
      const data = result.reduce((current, next) => {
        if (next.ativoId === 1) {
          current.push({
            id: next.id,
            nome: next.nome,
            sobrenome: next.sobrenome,
            descricao: next.id + " - " + next.nome,
          });
          return current;
        }
      }, []);
      this.DsResponsavel = data;
    });
  }

  criarResponsavel() {
    this.service.getResponsavelCreate().subscribe((result) => {
      const resp = { id: result.id, descricao: result.id + " - " + result.nome, nome: result.nome, sobrenome: result.sobrenome };
      this.DsResponsavel = [resp];
      this.formModel.controls['responsavelId'].setValue(resp);

    });
  }

  searchResponsavel(event) {
    this.service.getResponsavelAuto(event.query).subscribe((result: any) => {
      const data = result.reduce((current, next) => {
        if (next.ativoId === 1) {
          current.push({
            id: next.id,
            nome: next.nome,
            sobrenome: next.sobrenome,
            descricao: next.id + " - " + next.nome,
          });
          return current;
        }
      }, []);

      this.resultsResponsavel = data;
    });
  }

  //responsavel
  public adicionaResponsavel(id) {
    const resp = { projetoId: id, responsavelId: null, ativoId: 1, excluidoId: 0, edit: false};
    this.serviceProjetoResp.salvar(resp).subscribe(resp=> {
      this.loadResponsavel(id);
    });
  }

  closeCadastraResponsavel() {
    this.isCadastraResponsavel = false;
    this.loadResponsavel(this.formModel.value.id);
  }

  loadResponsavel(projetoId) {
    return new Promise((resolve) => {
      this.serviceProjetoResp.getAll(projetoId).subscribe((result) => {
        this.dataSourceResponsavel = result;
        resolve(true);
      });
    });
  }

  editResponsavel(row) {
    if (row) {
      this.responsavel = {
        data: { projetoId: row.projetoId, id: row.id },
        title: this.formModel.value.nome,
        filtro: this.dataSourceResponsavel,
      };
      this.isCadastraResponsavel = true;
    }
  }

  excluirResponsavel(row) {
    this.confirmationService.confirm({
      header: "Deseja realmente excluir ?",
      message: row.responsavel?.id + ' - ' + row.responsavel?.nome,
      icon: "pi pi-info-circle",

      accept: () => {
        this.serviceProjetoResp.excluir(row).subscribe(() => {
          this.messageService.add({
            key: "001",
            severity: "success",
            summary: "Excluido!",
            detail: "",
          });
          this.dataSourceResponsavel = this.dataSourceResponsavel.filter((a) =>
            a.projetoId == row.projetoId &&
            a.id !== row.id
          );
        });
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

  loadConfDataSet() {
    return new Promise((resolve) => {
      this.serviceTipo.getAllStatus().subscribe((result) => {
        const data = result.reduce((current, next) => {
          current.push({ id: next.id, descricao: next.descricao });
          return current;
        }, []);
        this.DsStatus = data;
        this.serviceTipo.getAllRisco().subscribe((result) => {
          const dataRisco = result.reduce((current, next) => {
            current.push({ id: next.id, descricao: next.descricao });
            return current;
          }, []);
          this.DsRisco = dataRisco;
          resolve(true);
        });
      });
    });
  }
}
