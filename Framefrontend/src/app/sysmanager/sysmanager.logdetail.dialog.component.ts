import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';

import { Message } from 'primeng/primeng';

import { MdDialog, MdDialogRef, MdDialogModule, MD_DIALOG_DATA } from '@angular/material';

import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams, CheckBizObjectRepatParams } from '../../app/common_module/common.service';

import { SysmanagerService } from './sysmanager.service';

@Component({
  selector: 'app-sysmanager-logdetail-dialog',
  templateUrl: './sysmanager.logdetail.dialog.component.html',
  styleUrls: ['./sysmanager.logdetail.dialog.component.css']
})
export class SysmanagerLogDetailDialogComponent implements OnInit {

  framelang: any = {};
  commonlang: any = {};
  currselect: any = {};
  consticky: boolean = false;
 
  errorinfos: any = [];

  constructor(private sysmanagerServ: SysmanagerService, private mainValueName: MainValueName, private checkparams: CheckBizObjectRepatParams,
    private dialogRef: MdDialogRef<SysmanagerLogDetailDialogComponent>,
    private commonmodule: CommonRootService, @Inject(MD_DIALOG_DATA) public data: any) {

  }

  ngOnInit() {

    this.framelang = JSON.parse(this.commonmodule.getFrameI18nlang());
    this.commonlang = JSON.parse(this.commonmodule.getCommonI18nlang());
    this.currselect = this.data;    
  }

  sureNo(): void {

    this.dialogRef.close({ "operationstate": "cancel" });

  }
}
