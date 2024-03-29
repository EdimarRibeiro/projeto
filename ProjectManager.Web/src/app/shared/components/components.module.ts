import { NgModule } from "@angular/core";
import { CommonModule, DatePipe } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { LoadingComponent } from "./loading/loading.component";

//////////////////  PrimeNG  ///////////////

import { DataViewModule } from "primeng/dataview";
import { ToastModule } from "primeng/toast";
import { InputTextModule } from "primeng/inputtext";
import { TooltipModule } from "primeng/tooltip";
import { ButtonModule } from "primeng/button";
import { DialogModule } from "primeng/dialog";
import { MessageService } from "primeng/api";
import { AccordionModule } from "primeng/accordion";
import { InputSwitchModule } from "primeng/inputswitch";
import { DropdownModule } from "primeng/dropdown";
import { InputNumberModule } from "primeng/inputnumber";
import { ChipModule } from "primeng/chip";
import { CalendarModule } from "primeng/calendar";
import { StepsModule } from "primeng/steps";
import { AutoCompleteModule } from "primeng/autocomplete";
import { ProgressSpinnerModule } from "primeng/progressspinner";
import { QRCodeModule } from "angularx-qrcode";
import { InputMaskModule } from "primeng/inputmask";
import { BadgeModule } from "primeng/badge";

//////////////////////// Sistema /////////////////

import { CarouselButtonComponent } from "@components/carousel-button/carousel-button.component";
import { KeyboardComponent } from "@components/keyboard/keyboard.component";
import { AlertComponent } from "@components/alert/alert.component";
import { FiltroDinamicoComponent } from "./filtro-dinamico/filtro-dinamico.component";
import { PermissoesStorageService } from "@auth/permissoes-storage.service";
import { FiltroDinamicoService } from "./filtro-dinamico/filtro-dinamico.service";
import { UsuarioStorageService } from "@auth/usuario-storage.service";
import { HomeService } from "@services/top-bar/home.service";

const APP_COMPONENT = [
  CarouselButtonComponent,
  KeyboardComponent,
  AlertComponent,
  FiltroDinamicoComponent,
  LoadingComponent,
];

const APP_MODULES = [QRCodeModule];

const PRIMENG_MODULES = [
  ButtonModule,
  DialogModule,
  AccordionModule,
  DataViewModule,
  InputSwitchModule,
  ToastModule,
  TooltipModule,
  DropdownModule,
  InputNumberModule,
  ChipModule,
  CalendarModule,
  StepsModule,
  InputMaskModule,
  BadgeModule
];

@NgModule({
  declarations: [APP_COMPONENT],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    APP_MODULES,
    PRIMENG_MODULES,
    InputTextModule,
    AutoCompleteModule,
    ProgressSpinnerModule,
  ],
  providers: [
    MessageService,
    DatePipe,
    HomeService,
    UsuarioStorageService,
    PermissoesStorageService,
    FiltroDinamicoService,
  ],
  exports: [APP_COMPONENT],
})
export class ComponentsModule { }
