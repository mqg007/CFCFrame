import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';

import { Message } from 'primeng/primeng';

import { MdDialog, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';

import { ConfirmDialogComponent } from '../common_module/confirm.dialog.component';
import { SysmanagerLogDetailDialogComponent } from './sysmanager.logdetail.dialog.component';
import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams, EnvData, CheckBizObjectRepatParams } from '../common_module/common.service';
import { I18nDateService } from '../common_module/i18ndate.service';


import { SysmanagerService } from './sysmanager.service';

@Component({
  selector: 'app-sysmanager-querylog',
  templateUrl: './sysmanager.querylog.component.html',
  styleUrls: ['./sysmanager.querylog.component.css']
})
export class SysmanagerQuerylogComponent implements OnInit {

  framelang: any = {};
  commonlang: any = {};
  consticky: boolean = false;
  
  initDate: any = {};
  usernameq: string = "";
  remarkq: string = "";
  startdateRECORDTIMEQ: any = ""; //开始日期
  enddateRECORDTIMEQ: any = ""; //结束日期

  condiv: boolean = false;
  tottlecounts: number = 0;
  numPages: number = 0;
  currdata: any = [];
  bindcurrdata: any = [];

  errorinfos: any = [];
  value: Date;

  constructor(private sysmanagerServ: SysmanagerService, private router: Router, private commonmodule: CommonRootService, public pagingParam: PagingParam,
    private mainValueName: MainValueName, private checkDBParams: CheckBizObjectRepatParams, private dialog: MdDialog, private i18nDate: I18nDateService) {

  }


  ngOnInit() {

    this.framelang = JSON.parse(this.commonmodule.getFrameI18nlang());
    this.commonlang = JSON.parse(this.commonmodule.getCommonI18nlang());

    this.initDate = this.i18nDate.getCurrDateLocalConfig(this.commonmodule.getI18nlang());

  }


  //查询数据
  querydata(): void {
    this.getData(1);
  }

  getData(currpageindex: number): void {
    this.condiv = true;

    //this.pagingParam.PageSize = 5; //默认10
    this.pagingParam.PageIndex = currpageindex;

    let currqueryobj: any = { USERNAME: "", RECORDTIME: "", DESCRIPTIONS: "" };
    currqueryobj.USERNAMES = this.usernameq;
    currqueryobj.RECORDTIME = this.startdateRECORDTIMEQ + "|" + this.enddateRECORDTIMEQ;
    currqueryobj.DESCRIPTIONS = this.remarkq;

    if (this.startdateRECORDTIMEQ > this.enddateRECORDTIMEQ) {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.labelCompDate });
      this.condiv = false;
      return;
    }

    let groupreqobj = [{ queryItemKey: "SSY_PagingParam", queryItemValue: JSON.stringify(this.pagingParam) },
    { queryItemKey: "SSY_LOGENTITY", queryItemValue: JSON.stringify(currqueryobj) }];

    let reqdata: RequestParams = new RequestParams();
    reqdata.AddrType = "FrameMgt";
    reqdata.GetDataUrl = "/FrameApi/GetLogDataPagerN";
    reqdata.Reqmethod = "post";
    reqdata.Reqdata = JSON.stringify(groupreqobj);
    reqdata.ReqdataType = "";
    reqdata.Reqtype = "getlogdata";
    reqdata.Reqi18n = this.commonmodule.getI18nlang();
    reqdata.ServIdentifer = "frameManager";
    reqdata.ServVersionIden = "1.0";

    this.sysmanagerServ.getLogData(reqdata,
      res => this.getLogDataResdataFunc(res), res => this.getLogDataDisplaydataFunc(res), err => this.getLogDataErrorFunc(err));
  }

  getLogDataResdataFunc(data: any): any {
    return data;
  }
  getLogDataDisplaydataFunc(data: any): void {
    this.condiv = false;
    this.bindcurrdata = [];
    this.tottlecounts = 0;
    if (data.status == 200) {
      let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
      if (respdata.respflag == "1") {
        this.tottlecounts = +respdata.pagertottlecounts;
        this.bindcurrdata = this.commonmodule.copyArrayToNewArray(JSON.parse(respdata.respdata));
      }
      else {
        this.errorinfos = [];
        this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeNoFoundData });
      }
    }
    else {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeError });
    }
  }
  getLogDataErrorFunc(err: string) {
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
    this.condiv = false;
  }

  //翻页处理
  changepage(event: any): void {
    this.getData(event.page);
  }

  doubleClickGrid(event: any): void {
    this.openOperationZone(event.data);
  }

  openOperationZone(currrowobj: any): void {
    let datas: any = currrowobj;
    let dialogRef = this.dialog.open(SysmanagerLogDetailDialogComponent, { height: '100%', width: '100%', data: datas });
    dialogRef.afterClosed().subscribe(result => {
      if (result.operationstate == "cancel") {
      }
    });
  }

}
