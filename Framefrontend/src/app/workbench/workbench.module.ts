import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from '@angular/router';
import {FormsModule, FormControl, ReactiveFormsModule} from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';

import { MaterialModule} from '@angular/material';
import 'hammerjs';


import {
  GrowlModule, MessagesModule
} from 'primeng/primeng';

import { AccordionModule } from 'ngx-bootstrap';

import {PageNotFoundComponent} from '../not-found.component';

import {workbenchRoutes} from './workbench.routes';
import {WorkbenchComponent} from './workbench.component';
import {WorkbenchHeaderComponent} from './workbench.header.component';
import {WorkbenchBottomComponent} from './workbench.bottom.component';
import { WorkbenchNoPrevilegeComponent } from "./workbench.noprevilege.component";


@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    FlexLayoutModule,

     MessagesModule,
    GrowlModule,

    RouterModule.forChild(workbenchRoutes),
    MaterialModule.forRoot(),
    AccordionModule.forRoot()
  ],
  exports: [],
  declarations: [
    WorkbenchComponent,
    PageNotFoundComponent,
    WorkbenchHeaderComponent,
    WorkbenchBottomComponent,
    WorkbenchNoPrevilegeComponent
  ],
  providers: [

  ],
})
export class WorkbenchModule {
}
