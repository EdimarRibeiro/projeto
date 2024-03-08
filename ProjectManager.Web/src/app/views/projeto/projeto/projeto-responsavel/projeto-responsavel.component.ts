import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { ProjetoResponsavelService } from '@services/projeto/projeto-responsavel.service';
import { DiversosService } from '@services/diversos/diversos.service';
import { Responsavel } from '@interfaces/geral/responsavel';

@Component({
  templateUrl: './projeto-responsavel.component.html',
  selector: 'app-projeto-responsavel',
})

export class ProjetoResponsavelComponent implements OnInit {
  @Output() showChange = new EventEmitter();
  @Input() show = false;
  @Input() data;
  public DsResponsavel: Responsavel[];

  public formModel: FormGroup;
  public submitted = false;
  public salvando = false;

  constructor(private service: ProjetoResponsavelService,
    private serviceTipo: DiversosService,
    private fb: FormBuilder, private messageService: MessageService,
  ) {
  }

  async ngOnInit() {
    await this.createForm();
    let projetoId = null;
    let id = null;

    if (this.data['data']) {
      projetoId = this.data['data'].projetoId;
      id = this.data['data'].id;
      this.loadConfDataSet().then(() => {
        if ((projetoId > 0) && (id)) {
          this.editar(id);
        } else {
          this.formModel.reset({
            edit: false,
            projetoId: projetoId,
            ativoId: 1,
          });
        }
      });
    }
  }

  private createForm() {
    this.formModel = this.fb.group({
      id: null,
      projetoId: null,
      responsavelId: new FormControl('', Validators.required),
      ativoId: new FormControl("1", Validators.required),
      idExterno: null,
      edit: false
    });
  }

  back() {
    this.showChange.emit(false);
  }

  async editar(id) {
    await this.service.getId(id).subscribe(result => {
      var responsavel = this.DsResponsavel.filter(a => a.id === result.responsavelId)[0];
      this.formModel.reset({
        edit: true,
        id: result.id,
        projetoId: result.projetoId,
        responsavelId: {id : result.responsavelId, nome : responsavel.nome},
        ativoId: result.ativoId,
        idExterno: result.idExterno,
      });
    });
  }

  async salvar() {
    this.submitted = true;
    if (this.formModel.valid && !this.salvando) {
      var model = JSON.parse(JSON.stringify(this.formModel.value));
      this.salvando = true;
      await this.service.salvar(this.FormartarModel(model)).subscribe((result) => {
        this.back();
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
    return model;
  }

  loadConfDataSet() {
    return new Promise(async (resolve) => {
      this.serviceTipo.getAllResponsavel().subscribe((result: Responsavel[]) => {
        const data = result.reduce((current, next) => {
          current.push({ id: next.id, nome: next.nome });
          return current;
        }, []);
        this.DsResponsavel = data;        
      });
    });
  }
}