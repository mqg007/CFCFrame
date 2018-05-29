import { Component, OnInit } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';

import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams, EnvData } from '../common_module/common.service';

import { WorkbenchService } from './workbench.service';

@Component({
  selector: 'app-workbench-bottom',
  templateUrl: './workbench.bottom.component.html',
  styleUrls: ['./workbench.bottom.component.css'],
  animations: [
    trigger('menuState', [
      state('inactive', style({
        left: '0px'
      })),
      state('active', style({
        left: '-110px'
      })),
      transition('inactive => active', animate('200ms ease-in')),
      transition('active => inactive', animate('200ms ease-out'))
    ]),
    trigger('routerState', [
      state('inactive', style({
        marginLeft: '170px'
      })),
      state('active', style({
        marginLeft: '50px'
      })),
      transition('inactive => active', animate('200ms ease-in')),
      transition('active => inactive', animate('200ms ease-out'))
    ]),
    trigger('imgState', [
      state('inactive', style({
        left: '16px'
      })),
      state('active', style({
        left: '123px'
      })),
      transition('inactive => active', animate('200ms ease-in')),
      transition('active => inactive', animate('200ms ease-out'))
    ])
  ]
})
export class WorkbenchBottomComponent implements OnInit { 

  constructor(private workbenchServ: WorkbenchService, public router: Router, public commonmodule: CommonRootService) {
  };

  ngOnInit() {
  
  }
 
}
