import { Component, OnInit } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';

import { Message } from 'primeng/primeng';
import { MdDialog, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';

import { ConfirmDialogComponent } from '../common_module/confirm.dialog.component';
import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams, EnvData } from '../common_module/common.service';

import { WorkbenchService } from './workbench.service';

@Component({
  selector: 'app-workbench-noprevilege',
  templateUrl: './workbench.noprevilege.component.html',
  styleUrls: ['./workbench.noprevilege.component.css'],
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
export class WorkbenchNoPrevilegeComponent implements OnInit {

  framelang: any = {};
  commonlang: any = {};
  consticky: boolean = false;
  errorinfos: any = [];
  frameenvStr: string;
  frameenv: any;

  constructor(private workbenchServ: WorkbenchService, public router: Router, public commonmodule: CommonRootService, private dialog: MdDialog) {
  };

  ngOnInit() {

    this.framelang = JSON.parse(this.commonmodule.getFrameI18nlang());
    this.commonlang = JSON.parse(this.commonmodule.getCommonI18nlang());

    this.frameenvStr = this.commonmodule.getToken();
    if (this.frameenvStr) {
      this.frameenv = JSON.parse(this.frameenvStr);
    }

   
  }

  


}
