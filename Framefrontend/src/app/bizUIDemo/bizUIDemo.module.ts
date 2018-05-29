import { NgModule }       from '@angular/core';
import { CommonModule } from '@angular/common';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { FlexLayoutModule } from '@angular/flex-layout';
import { RouterModule} from "@angular/router";

import { MaterialModule} from '@angular/material';
import 'hammerjs';

import { AccordionModule } from 'ngx-bootstrap';

import { Dynamic_load_componentDirective, Dynamic_componentItem, Dynamic_omponent } from "../common_module/dynamic.load.component.directive";

import {BizUIDemoRoutes} from './bizUIDemo.routes';
import {BizUIDemoComponent } from './bizUIDemo.component';
import {BizCustomerUIDemoCardComponent} from './bizCustomerUIDemo.component';
import {BizInGoodsUIDemoComponent} from './bizInGoodsUIDemo.component';
import {BizOutGoodsUIDemoComponent} from './bizOutGoodsUIDemo.component';

@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    FlexLayoutModule,  
    RouterModule.forChild(BizUIDemoRoutes),    
    AccordionModule.forRoot(), 
    MaterialModule.forRoot()    
  ],
  declarations: [BizUIDemoComponent, BizCustomerUIDemoCardComponent ,
  BizInGoodsUIDemoComponent, BizOutGoodsUIDemoComponent],    
  entryComponents: [],
  exports:[RouterModule]
})
export class BizUIDemoModule {
  
}
