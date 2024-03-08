import { Component, OnInit } from '@angular/core';
import { BreadcrumbService } from '@services/layout/breadcrumb/breadcrumb.service';
import { Usuario } from '@interfaces/usuario/usuario';
import { Router } from '@angular/router';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { UsuarioService } from '@services/usuario/usuario.service';
import { formatDate } from '@angular/common';

@Component({
  templateUrl: './usuario-edit.component.html',
  styleUrls: ['./usuario-edit.component.scss']
})

export class UsuarioEditComponent implements OnInit {
  public formModel: FormGroup;
  public dataSource = [];
  public dataSourceSimNao = [];
  public index = 0;
  public id;
  public nomeBotao = '';
  public submitted = false;
  public salvando = false;

  constructor(
    private service: UsuarioService,
    private breadcrumbService: BreadcrumbService,
    private fb: FormBuilder,
    private router: Router,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {

    this.breadcrumbService.setItems([
      { label: 'Gerencial' },
      { label: 'Usuários', routerLink: '/gerencial/usuario' },
      { label: 'Edição' }
    ]);
  }

  ngOnInit() {
    this.nomeBotao = 'Salvar e continuar';
    this.createForm();

    const id = Number.parseInt(this.router.paramsInheritanceStrategy.toString());

    if (id > 0) {
      this.editar(id);
    }

    this.service.getAll().subscribe(result => {
      this.dataSource = result;
    });
  }

  private createForm() {
    this.formModel = this.fb.group({
      id: null,
      login: new FormControl('', Validators.required),
      nome: new FormControl('', Validators.required),
      senha: new FormControl('', Validators.required),
      fotoUrl: new FormControl('', Validators.required),
      celular: null,
      cpfCnpj: null,
      alterarSenha: new FormControl("0", Validators.required),
      dataCadastro: null,
      dataExpiracao: null,
      emailVerificado: 0,
      ativoId: new FormControl("1", Validators.required),
      excluidoId: 0,
      idExterno: null,
      edit: false,
    });
  }


  async editar(id) {
    this.nomeBotao = 'Salvar'
    await this.service.getId(id).subscribe(async (result: Usuario) => {
      this.formModel.reset({
        id: result.id,
        login: result.login,
        senha: result.senha,
        nome: result.nome,
        celular: result.celular,
        cpfCnpj: result.cpfCnpj,
        fotoUrl: result.fotoUrl,
        alterarSenha: result.alterarSenha,
        dataCadastro: result.dataCadastro ? new Date(formatDate(result.dataCadastro, 'yyyy-MM-ddTHH:mm', 'en')) : null,
        dataExpiracao: result.dataExpiracao ? new Date(formatDate(result.dataExpiracao, 'yyyy-MM-ddTHH:mm', 'en')) : null,
        emailVerificado: result.emailVerificado,
        ativoId: result.ativoId,
        excluidoId: result.excluidoId,
        idExterno: result.idExterno,
        edit: true
      });
    });
  }

  onUpload(event) {
    for (let file of event.files) {      
    }
  }

  onClear() {
    this.formModel.value.logoMarca = '';
  }

  voltar() {
    this.router.navigate(['/gerencial/usuario/']);
  }

  async salvar() {
    this.submitted = true;
    if (this.formModel.valid && !this.salvando) {
      this.salvando = true;
      await this.service.salvar(this.formModel.value).subscribe((result) => {
        if (result != null) {
          this.id = result["id"],
            this.formModel.value.edit = true,
            this.editar(this.id);
        } else {
          this.router.navigate(['/gerencial/usuario/']);
        }
        this.salvando = false;
      }, error => {
        this.salvando = false;
        var msg = error.error ? error.error.split(':')[1].split('.')[0] : error.statusText;
        this.messageService.add({ key: '001', severity: 'info', summary: 'Falha ao salvar dados!', detail: msg });
      });
    }
  }
}
