import {BrowserModule} from "@angular/platform-browser";
import {NgModule} from "@angular/core";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {CommonModule} from "@angular/common";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {RouterModule} from '@angular/router';
import { FlexLayoutModule } from '@angular/flex-layout';
import {PathLocationStrategy, LocationStrategy, HashLocationStrategy} from '@angular/common';

import { MaterialModule} from '@angular/material';
import 'hammerjs';
 
import{ AlertModule, ButtonsModule  } from 'ngx-bootstrap';

import {
  GrowlModule,
  MessagesModule,
  CheckboxModule,
  DataTableModule  
} from 'primeng/primeng';

import {ConfirmDialogComponent} from '../app/common_module/confirm.dialog.component';
import { CommonRootService, MainValueName, PagingParam, RequestParams,  ResponseParams, EnvData, CheckBizObjectRepatParams} from '../app/common_module/common.service';
import { I18nDateService } from '../app/common_module/i18ndate.service';
import { Dynamic_load_componentDirective } from '../app/common_module/dynamic.load.component.directive';

import {AppComponent} from "./app.component";
import {LoginComponent} from './login/login.component';
import {QuitLoginComponent} from './login/quitlogin.component';
import {appRoutes} from './app.routes';
import {LoginService} from './login/login.service';
import {Preload} from './preloading';
import {WorkbenchService} from './workbench/workbench.service';

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent, 
        QuitLoginComponent,
        ConfirmDialogComponent,
        Dynamic_load_componentDirective
    ],
    imports: [
        CommonModule,
        BrowserModule,
        FormsModule,
        RouterModule,
        ReactiveFormsModule,
        BrowserAnimationsModule,
        FlexLayoutModule,
        RouterModule.forRoot(
          appRoutes,
          { preloadingStrategy: Preload }
        ),  
        MaterialModule.forRoot(),

        AlertModule.forRoot(), 
        ButtonsModule.forRoot(),

        MessagesModule,
        GrowlModule,
        CheckboxModule,
        DataTableModule   
    ],
    providers: [
      Preload,
      LoginService,
      WorkbenchService,
      CommonRootService,MainValueName, PagingParam, RequestParams,  ResponseParams, EnvData, CheckBizObjectRepatParams,I18nDateService,
      {provide: LocationStrategy, useClass: HashLocationStrategy}
    ],
    entryComponents: [ ConfirmDialogComponent ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
