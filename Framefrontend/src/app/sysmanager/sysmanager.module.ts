import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { FlexLayoutModule } from '@angular/flex-layout';
import { RouterModule } from "@angular/router";

import { MaterialModule } from '@angular/material';
import 'hammerjs';

import {
  DataTableModule, SharedModule, PaginatorModule, SliderModule, InputSwitchModule, GrowlModule,PanelModule,
  MessagesModule,TreeModule,CalendarModule
} from 'primeng/primeng';

import { AlertModule, ButtonsModule, AccordionModule, PaginationModule } from 'ngx-bootstrap';

import { sysmanagerRoutes } from './sysmanager.routes';
import { SysmanagerEntryComponent } from "./sysmanager.entry.component";
import { SysmanagerGroupComponent } from "./sysmanager.group.component";
import { SysmanagerPageComponent } from "./sysmanager.page.component";
import { SysmanagerUserComponent } from "./sysmanager.user.component";
import { SysmanagerService } from './sysmanager.service';
import { SysmanagerOppageDialogComponent } from './sysmanager.oppage.dialog.component';
import { SysmanagerPriviliegeComponent } from "./sysmanager.priviliege.component";

import { SysmanagerResetpwdComponent } from "./sysmanager.resetpwd.component";
import { SysmanagerModifypwdComponent } from "./sysmanager.modifypwd.component";
import { SysmanagerQuerylogComponent } from "./sysmanager.querylog.component";
import { SysmanagerLogDetailDialogComponent } from './sysmanager.logdetail.dialog.component';
import { SysmanagerServicehelpComponent } from "./sysmanager.servicehelp.component";
import { SysmanagerComdictComponent } from "./sysmanager.comdict.component";
import { SysmanagerBizdictComponent } from "./sysmanager.bizdict.component";





@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    FlexLayoutModule,
    RouterModule.forChild(sysmanagerRoutes),

    DataTableModule,
    SharedModule,
    PaginatorModule,
    SliderModule,
    InputSwitchModule,
    MessagesModule,
    GrowlModule,
    PanelModule,
    TreeModule,
    CalendarModule,

    MaterialModule.forRoot(),

    //Ng2BootstrapModule,
    AlertModule.forRoot(),    
    ButtonsModule.forRoot(),
    AccordionModule.forRoot(),
    PaginationModule.forRoot()  
    
  ],
  declarations: [SysmanagerEntryComponent, SysmanagerGroupComponent, SysmanagerPageComponent, SysmanagerOppageDialogComponent,SysmanagerLogDetailDialogComponent,
  SysmanagerUserComponent, SysmanagerPriviliegeComponent,SysmanagerResetpwdComponent, SysmanagerModifypwdComponent,SysmanagerQuerylogComponent,
  SysmanagerServicehelpComponent,SysmanagerComdictComponent,SysmanagerBizdictComponent
  ], 
  providers: [
    SysmanagerService
  ],
  entryComponents: [ SysmanagerOppageDialogComponent, SysmanagerLogDetailDialogComponent ],
  exports: [RouterModule]
})
export class SysmanagerModule {
}
