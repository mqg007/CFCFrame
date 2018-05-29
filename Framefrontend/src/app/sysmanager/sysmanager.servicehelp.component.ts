import { Component, OnInit } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';

import { Message } from 'primeng/primeng';

import {MdDialog, MdDialogRef, MD_DIALOG_DATA} from '@angular/material';

import {ConfirmDialogComponent} from '../common_module/confirm.dialog.component';
import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams, EnvData, CheckBizObjectRepatParams } from '../common_module/common.service';

import { SysmanagerService } from './sysmanager.service';

@Component({
  selector: 'app-sysmanager-servicehelp',
  templateUrl: './sysmanager.servicehelp.component.html',
  styleUrls: ['./sysmanager.servicehelp.component.css']

})
export class SysmanagerServicehelpComponent implements OnInit {

  framelang: any = {};
  commonlang: any = {};  
  errorinfos: any = [];
  consticky: boolean = false;

  frmHelpSystemName:string = "";
  frmHelpUseManualDoc:string = "";
  frmLinkMan:string = "";
  frmHelpTelephone:string = "";
  frmHelpQQ:string = "";
  frmHelpEmail:string = "";
  frmHelpCopyright:string = "";
  frmHelpWebSite:string = "";


  constructor(private sysmanagerServ: SysmanagerService, private router: Router, private commonmodule: CommonRootService, 
    private mainValueName: MainValueName, private checkDBParams: CheckBizObjectRepatParams, private dialog: MdDialog) {
      
  }

  ngOnInit() {

    this.framelang = JSON.parse(this.commonmodule.getFrameI18nlang());
    this.commonlang = JSON.parse(this.commonmodule.getCommonI18nlang());

    this.frmHelpSystemName = this.framelang.SystemName;
   

  } 



}
