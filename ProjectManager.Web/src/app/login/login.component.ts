import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../auth/authentication.service';
import { Router } from '@angular/router';
import { MenuItem, Message, MessageService } from 'primeng/api';

@Component({
  templateUrl: './login.component.html'
})

export class LoginComponent implements OnInit {
  @Output() showChange = new EventEmitter();

  public modelForm: FormGroup;
  public modelFormConta: FormGroup;
  public errors: Message[];
  public dataSourceEstado = [];
  public dataSourceCidade = [];
  public registroConta = false;
  items: MenuItem[];
  public activeIndex = 0;

  constructor(private fb: FormBuilder, private authService: AuthenticationService, private router: Router,
    private messageService: MessageService) {
    this.createForm();
  }

  ngOnInit() {
    this.modelForm.reset({
      username: '',
      password: ''
    });   
  }

  private createForm() {
    this.modelForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  public login() {
    this.errors = [];
    this.authService.login(this.modelForm.value).subscribe((result) => {
      if (result.authenticated === true) {
        this.router.navigate(['']);
      } else {
        this.errors = [{ severity: 'error', summary: 'Falha na autenticação', detail: result.message }];
      }
    }, error => {
      this.errors = [{ severity: 'error', summary: 'Falha na autenticação', detail: error.message }];
    });
  }

  criarConta() {
    this.activeIndex = 0;
    this.registroConta = true;
  }

  back() {
    this.registroConta = false;
    this.showChange.emit(false);
    this.errors = [];
  }
}
