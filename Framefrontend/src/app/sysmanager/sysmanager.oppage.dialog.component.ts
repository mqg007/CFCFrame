import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';

import { Message } from 'primeng/primeng';

import { MdDialog, MdDialogRef, MdDialogModule, MD_DIALOG_DATA } from '@angular/material';

import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams, CheckBizObjectRepatParams } from '../../app/common_module/common.service';

import { SysmanagerService } from './sysmanager.service';

@Component({
  selector: 'app-sysmanager-oppage-dialog',
  templateUrl: './sysmanager.oppage.dialog.component.html',
  styleUrls: ['./sysmanager.oppage.dialog.component.css']
})
export class SysmanagerOppageDialogComponent implements OnInit {

  framelang: any = {};
  commonlang: any = {};
  currselect: any = {};
  consticky: boolean = false;

  consavediv: boolean = false;
  errorinfos: any = [];

  constructor(private sysmanagerServ: SysmanagerService, private mainValueName: MainValueName, private checkparams: CheckBizObjectRepatParams,
    private dialogRef: MdDialogRef<SysmanagerOppageDialogComponent>,
    private commonmodule: CommonRootService, @Inject(MD_DIALOG_DATA) public data: any) {

  }

  ngOnInit() {

    this.framelang = JSON.parse(this.commonmodule.getFrameI18nlang());
    this.commonlang = JSON.parse(this.commonmodule.getCommonI18nlang());
    this.currselect = this.data;
    if (this.currselect.PAGEID == "") {
      this.currselect.OPFlag = "I";    
    }
    else {
      this.currselect.OPFlag = "M";
    }    
  }


  sureYes(): void {
    this.consavediv = true;

    if (this.currselect.PAGEURL != "") {
      this.currselect.PLATTYPES = "WEB";
    }
    else if (this.currselect.WPFPAGEURL != "") {
      this.currselect.PLATTYPES = "WPF";
    }
    else if (this.currselect.PAGECLASSNAME != "") {
      this.currselect.PLATTYPES = "WINFORM";
    }

    if (this.currselect.ISUSE == true) {
      this.currselect.ISUSE = "1";
    } else if (this.currselect.ISUSE == false) {
      this.currselect.ISUSE = "0";
    }

    if (!this.verityData()) {
      return;
    }

    if (this.currselect.OPFlag == "I") {
      this.executeDBCheck(this.currselect);
    }
    else if (this.currselect.OPFlag == "M") {
      this.saveData(); //修改时直接保存
    }
  }

  sureNo(): void {

    this.dialogRef.close({ "operationstate": "cancel" });

  }

  //保存
  saveData(): void {
    //提交数据进行保存

    let bizobjop = [{ queryItemKey: "objLists", queryItemValue: JSON.stringify([this.currselect]) },
    { queryItemKey: "opObjPropertyL", queryItemValue: JSON.stringify(["PAGEID", "PAGENAME", "PAGEPARENTID", "PAGEURL", "PAGETARGET", "PAGEIMG", "ISUSE", "PAGEMOUDAL", "SEQSORT", "PAGECLASSNAME", "PAGEASSEMBLY", "WPFPAGEURL", "WPFPAGETARGET", "PLATTYPES", "TIMESTAMPSS"]) },
    { queryItemKey: "wherePropertyL", queryItemValue: JSON.stringify(["PAGEID"]) },
    { queryItemKey: "mainPropertyL", queryItemValue: JSON.stringify(["PAGEID"]) }
    ];

    let reqdata: RequestParams = new RequestParams();
    reqdata.AddrType = "FrameMgt";
    reqdata.GetDataUrl = "/FrameApi/OpBizObjectSingle_SSY_PAGE_DICTN";
    reqdata.Reqmethod = "post";
    reqdata.Reqdata = JSON.stringify(bizobjop);
    reqdata.ReqdataType = "";
    reqdata.Reqtype = "savegroupdata";
    reqdata.Reqi18n = this.commonmodule.getI18nlang();
    reqdata.ServIdentifer = "frameManager";
    reqdata.ServVersionIden = "1.0";

    this.sysmanagerServ.savePageData(reqdata,
      res => this.getSavePageDataResdataFunc(res), res => this.getSavePageDataDisplaydataFunc(res), err => this.getSavePageDataErrorFunc(err));

  }

  //校验数据
  verityData(): boolean {
    let warningInfo: string = "";
    if (this.currselect.PAGEID == "") {
      warningInfo = warningInfo + this.framelang.frmOpPagesFunctionID + ":" + this.commonlang.label_chkGridDataVerifyEmpty;
    }
    if (this.currselect.PAGENAME == "" || this.currselect.PAGENAME == undefined) {
      warningInfo = warningInfo + this.framelang.frmOpPagesFunctionName + ":" + this.commonlang.label_chkGridDataVerifyEmpty;
    }
    if (this.currselect.PAGEPARENTID == "" || this.currselect.PAGENAME == undefined) {
      warningInfo = warningInfo + this.framelang.frmOpPagesParentFunctionID + ":" + this.commonlang.label_chkGridDataVerifyEmpty;
    }
    if (this.currselect.PAGEMOUDAL == "" || this.currselect.PAGENAME == undefined) {
      warningInfo = warningInfo + this.framelang.frmOpPagesModuleName + ":" + this.commonlang.label_chkGridDataVerifyEmpty;
    }

    if (warningInfo != "") {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: warningInfo });
      return false;
    }
    else {
      return true;
    }
  }; 

  //检查数据库中数据
  executeDBCheck(p_currdata: any): void {
    //检查页面PAGEID
    let bizobjop = [{ queryItemKey: "Bizobjectname", queryItemValue: "SSY_PAGE_DICT" },
    { queryItemKey: "Splitchar", queryItemValue: "|" },
    { queryItemKey: "WherePropertyL", queryItemValue: JSON.stringify(["PAGEID" + "|" + p_currdata.PAGEID]) }
    ];

    let reqdata: RequestParams = new RequestParams();
    reqdata.AddrType = "FrameMgt";
    reqdata.GetDataUrl = "/FrameApi/CheckBizObjectRepatN";
    reqdata.Reqmethod = "post";
    reqdata.Reqdata = JSON.stringify(bizobjop);
    reqdata.ReqdataType = "";
    reqdata.Reqtype = "checkbizobjrepeat";
    reqdata.Reqi18n = this.commonmodule.getI18nlang();
    reqdata.ServIdentifer = "frameManager";
    reqdata.ServVersionIden = "1.0";

    this.sysmanagerServ.executeDBCheck(reqdata,
      res => this.getExecuteDBCheckResdataFunc(res),
      res => this.getExecuteDBCheckDisplaydataFunc(res, p_currdata.PAGEID),
      err => this.getExecuteDBCheckErrorFunc(err));
  }

  getExecuteDBCheckResdataFunc(data: any): any {
    return data;
  }
  getExecuteDBCheckDisplaydataFunc(data: any, currinfo: string): void {
    if (data.status == 200) {
      let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
      if (respdata.respflag == "0") {
        this.saveData();
      }
      else {
        this.consavediv = false;
        this.errorinfos = [];
        this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: respdata.resptoolstr });
      }
    }
    else {
      this.consavediv = false;
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeError });
    }
  }
  getExecuteDBCheckErrorFunc(err: string) {
    this.consavediv = false;
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
  }


  //保存数据
  getSavePageDataResdataFunc(data: any): any {
    return data;
  }
  getSavePageDataDisplaydataFunc(data: any): void {
    this.consavediv = false;
    try {
      if (data.status == 200) {
        let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
        if (respdata.respflag == "1") {
          this.errorinfos = [];
          this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeSaveSuccess });
          //返回结果
          this.dialogRef.close({ "operationstate": "yes" });
        }
        else {
          this.errorinfos = [];
          this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: respdata.resptoolstr });
        }
      }
      else {
        this.errorinfos = [];
        this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpReturnError });
      }
    }
    catch (e) {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.anaReturnResultErr + e.message });
    }
  }
  getSavePageDataErrorFunc(err: string) {
    this.consavediv = false;
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
  }




}
