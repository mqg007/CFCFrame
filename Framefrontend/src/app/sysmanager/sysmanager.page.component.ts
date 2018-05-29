import { Component, OnInit } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';

import { Message } from 'primeng/primeng';

import { MdDialog, MdDialogRef, MD_DIALOG_DATA, DialogPosition } from '@angular/material';

import { ConfirmDialogComponent } from '../common_module/confirm.dialog.component';
import { SysmanagerOppageDialogComponent } from './sysmanager.oppage.dialog.component';

import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams, EnvData, CheckBizObjectRepatParams } from '../common_module/common.service';

import { SysmanagerService } from './sysmanager.service';

@Component({
  selector: 'app-sysmanager-page',
  templateUrl: './sysmanager.page.component.html',
  styleUrls: ['./sysmanager.page.component.css']

})
export class SysmanagerPageComponent implements OnInit {

  framelang: any = {};
  commonlang: any = {};
  consticky: boolean = false;

  pagenames: any = [];
  pagenameQ: string = "";
  isuse: string = "";
  isTmp: boolean = false;

  condiv: boolean = false;
  consavediv: boolean = false;
  tottlecounts: number = 0;
  numPages: number = 0;
  currdata: any = [];
  bindcurrdata: any = [];
  olddata: any = [];
  submitdata: any = [];
  selectrows: any = {};
  addrow: any = {};
  judgeop: string = ""; //I 增  M 改

  errorinfos: any = [];


  constructor(private sysmanagerServ: SysmanagerService, private router: Router, private commonmodule: CommonRootService, public pagingParam: PagingParam,
    private mainValueName: MainValueName, private checkDBParams: CheckBizObjectRepatParams, private dialog: MdDialog) {

  }

  ngOnInit() {

    this.framelang = JSON.parse(this.commonmodule.getFrameI18nlang());
    this.commonlang = JSON.parse(this.commonmodule.getCommonI18nlang());
    this.getPageName();
  }

  //获取模块名称
  getPageName(): void {

    let reqdata: RequestParams = new RequestParams();
    let currreqobj = { params: "" };
    reqdata.AddrType = "FrameMgt";
    reqdata.GetDataUrl = "/FrameApi/GetPageN";
    reqdata.Reqmethod = "post";
    reqdata.Reqdata = "";
    reqdata.ReqdataType = "";
    reqdata.Reqtype = "getpagename";
    reqdata.Reqi18n = this.commonmodule.getI18nlang();
    reqdata.ServIdentifer = "frameManager";
    reqdata.ServVersionIden = "1.0";

    this.sysmanagerServ.getPageName(reqdata,
      res => this.getPageNameResdataFunc(res), res => this.getPageNameDisplaydataFunc(res), err => this.getPageNameErrorFunc(err));
  }
  getPageNameResdataFunc(data: any): any {
    return data;
  }
  getPageNameDisplaydataFunc(data: any): void {
    if (data.status == 200) {
      let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
      if (respdata.respflag == "1") {
        this.pagenames = JSON.parse(respdata.respdata);
      }
    }
  }
  getPageNameErrorFunc(err: string) {
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
    this.condiv = false;
  }


  //查询数据
  querydata(): void {
    this.getData(1);
  }

  getData(currpageindex: number): void {
    this.condiv = true;

    //this.pagingParam.PageSize = 5; //默认10
    this.pagingParam.PageIndex = currpageindex;

    let currqueryobj: any = {};
    currqueryobj.PAGEID = this.pagenameQ;
    currqueryobj.ISUSE = this.isuse;

    let pagereqobj = [{ queryItemKey: "SSY_PagingParam", queryItemValue: JSON.stringify(this.pagingParam) },
    { queryItemKey: "SSY_PAGE_DICT", queryItemValue: JSON.stringify(currqueryobj) }];

    let reqdata: RequestParams = new RequestParams();
    reqdata.AddrType = "FrameMgt";
    reqdata.GetDataUrl = "/FrameApi/GetPagePagerN";
    reqdata.Reqmethod = "post";
    reqdata.Reqdata = JSON.stringify(pagereqobj);
    reqdata.ReqdataType = "";
    reqdata.Reqtype = "getpagedata";
    reqdata.Reqi18n = this.commonmodule.getI18nlang();
    reqdata.ServIdentifer = "frameManager";
    reqdata.ServVersionIden = "1.0";

    this.sysmanagerServ.getPageData(reqdata,
      res => this.getPageDataResdataFunc(res), res => this.getPageDisplaydataFunc(res), err => this.getPageDataErrorFunc(err));
  }

  getPageDataResdataFunc(data: any): any {
    return data;
  }
  getPageDisplaydataFunc(data: any): void {
    this.condiv = false;
    this.bindcurrdata = [];
    this.tottlecounts = 0;
    this.currdata = [];
    this.olddata = [];
    if (data.status == 200) {
      let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
      if (respdata.respflag == "1") {
        let binddata: any = [];
        binddata = JSON.parse(respdata.respdata);
        for (var i = 0; i < binddata.length; i++) {
          if (binddata[i]["ISUSE"] == "1") {
            binddata[i]["ISUSE"] = true;
          }
          else {
            binddata[i]["ISUSE"] = false;
          }
        }
        this.currdata = binddata;
        this.olddata = this.commonmodule.copyArrayToNewArray(binddata);;
        this.tottlecounts = +respdata.pagertottlecounts;

        this.bindcurrdata = [];
        this.bindcurrdata = this.commonmodule.copyArrayToNewArray(this.currdata);
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
  getPageDataErrorFunc(err: string) {
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
    this.condiv = false;
  }

  //翻页处理
  changepage(event: any): void {
    this.getData(event.page);
  }

  //添加
  addData(): void {
    if (this.selectrows.PAGEID != "" && this.selectrows.PAGEID != undefined && this.isTmp) {
      this.addrow = JSON.parse(JSON.stringify(this.selectrows));
    }
    else {
      this.addrow = {};
      this.addrow.ISUSE = true;
    }

    this.addrow.PAGEID = "";
    this.judgeop = "I";
    this.openOperationZone(this.addrow);
  }

  //删除
  delData(): void {
    this.consavediv = true;
    if (this.selectrows.PAGEID != "" && this.selectrows.PAGEID != undefined) {
      let datas: any = {
        actionTitle: this.commonlang.noticeInfoTitle, actionContent: this.commonlang.noticeDelAction,
        actionOkText: this.commonlang.action_cofirm, actionCancelText: this.commonlang.action_cancel
      };

      let dialogRef = this.dialog.open(ConfirmDialogComponent, { height: '100%', width: '100%', data: datas });
      dialogRef.afterClosed().subscribe(result => {
        if (result.operationstate == "yes") {
          //原生查询有问题，不知道为啥
          //let index = this.currdata.indexOf(this.selectrows);
          //this.currdata.splice(index, 1);

          let mainkeys: MainValueName[] = [{ PropName: "PAGEID" }];
          let tmpArrayCurrSelectRow: string[] = [];
          tmpArrayCurrSelectRow.push(this.selectrows);
          let index = this.commonmodule.existsArrayElement(mainkeys, tmpArrayCurrSelectRow, this.currdata);
          if (index >= 0) {
            this.currdata.splice(index, 1);
          }

          this.selectrows = {};
          this.bindcurrdata = [];
          this.bindcurrdata = this.commonmodule.copyArrayToNewArray(this.currdata);
          if (this.olddata.length == 0 && this.currdata.length > 0) {
            //自动保存
            this.saveData();
          }
          else if (this.olddata.length > 0) {
            //自动保存
            this.saveData();
          }
        }
        else if (result.operationstate == "cancel") {
        }
        this.consavediv = false;
      });

    }
    else {
      this.consavediv = false;
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticePleaseSelectData });
    }
  }

  //保存
  saveData(): void {
    //先比较变更结果，只提交有变更的数据,该函数处理比较结果并copy到submitdata数组
    let mainkeys: MainValueName[] = [{ PropName: "PAGEID" }];
    this.submitdata = this.commonmodule.compData(mainkeys, "OPFlag", this.currdata, this.olddata);
    if (this.submitdata.length <= 0) {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeDataNoChange });
      return;
    }

    //提交数据进行保存
    let bizobjop = [{ queryItemKey: "objLists", queryItemValue: JSON.stringify(this.submitdata) },
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

  getSavePageDataResdataFunc(data: any): any {
    return data;
  }
  getSavePageDataDisplaydataFunc(data: any): void {
    try {
      if (data.status == 200) {
        let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
        if (respdata.respflag == "1") {
          this.errorinfos = [];
          this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeSaveSuccess });
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
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
  }

  doubleClickGrid(event: any): void {
    this.selectrows = event.data;
    this.judgeop = "M";
    this.openOperationZone(this.selectrows);
  }

  selectGridRow(event: any): void {

    this.isTmp = true; //联动模板添加

  }

  openOperationZone(currrowobj: any): void {
    let datas: any = currrowobj;
    let dialogRef = this.dialog.open(SysmanagerOppageDialogComponent, { height: '100%', width: '100%', data: datas });
    dialogRef.afterClosed().subscribe(result => {
      if (result.operationstate == "yes") {
        if (this.judgeop == "I") {
          if (this.tottlecounts > 0) {
            this.tottlecounts = 0;
            this.currdata = [];
            this.olddata = [];
          }

          if (datas.ISUSE == "1") {
            datas.ISUSE = true;
          } else if (datas.ISUSE == "0") {
            datas.ISUSE = false;
          }

          this.currdata.push(datas);
          this.selectrows = {};
          this.bindcurrdata = [];
          this.bindcurrdata = this.commonmodule.copyArrayToNewArray(this.currdata);
        }
        else {
          if (this.selectrows.ISUSE == "1") {
            this.selectrows.ISUSE = true;
          } else if (this.selectrows.ISUSE == "0") {
            this.selectrows.ISUSE = false;
          }
        }

        this.errorinfos = [];
        this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeSaveSuccess });
      }
      else if (result.operationstate == "cancel") {
      }
    });
  }
}
