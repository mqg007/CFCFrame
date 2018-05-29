import { Component, OnInit } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';

import { Message } from 'primeng/primeng';
import { MdDialog, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';

import { ConfirmDialogComponent } from '../common_module/confirm.dialog.component';
import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams, EnvData } from '../common_module/common.service';

import { WorkbenchService } from './workbench.service';

@Component({
  selector: 'app-workbench-header',
  templateUrl: './workbench.header.component.html',
  styleUrls: ['./workbench.header.component.css'],
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
export class WorkbenchHeaderComponent implements OnInit {

  framelang: any = {};
  commonlang: any = {};
  consticky: boolean = false;
  errorinfos: any = [];
  frameenvStr: string;
  frameenv: any;
  quitFlag: string = "";  //动作识别
  username: string = ""; 

  constructor(private workbenchServ: WorkbenchService, public router: Router, public commonmodule: CommonRootService, private dialog: MdDialog) {
  };

  ngOnInit() {

    this.framelang = JSON.parse(this.commonmodule.getFrameI18nlang());
    this.commonlang = JSON.parse(this.commonmodule.getCommonI18nlang());

    this.frameenvStr = this.commonmodule.getToken();
    if (this.frameenvStr) {
      this.frameenv = JSON.parse(this.frameenvStr);
      if(this.frameenv.SysUserDict){
        this.username = this.frameenv.SysUserDict.USERID;
      }
    }

    //定时监控超时,每分钟
    setInterval(() => { this.timeOutFunction(); }, 60000);
  }

  /************************* quit login ********************************/
  relogin() {
    this.quitFlag = "Login";
    this.quitLog();
  }

  quitSystem() {
    let datas: any = {
      actionTitle: this.commonlang.noticeInfoTitle, actionContent: this.framelang.confirmQuitSystem,
      actionOkText: this.commonlang.action_cofirm, actionCancelText: this.commonlang.action_cancel
    };

    let dialogRef = this.dialog.open(ConfirmDialogComponent, { height: '100%', width: '100%', data: datas });
    dialogRef.afterClosed().subscribe(result => {
      if (result.operationstate == "yes") {
        this.quitFlag = "Quit";
        this.quitLog();
      }
      else if (result.operationstate == "cancel") {

      }
    });

  }

  //超时处理函数，直接退出系统
  timeOutFunction(): void {

    let nodecenterobj: any = JSON.parse(this.commonmodule.getNodeCenterAddr());
    let currDate: Date = new Date(); //当前时间

    let difvalue: number = this.commonmodule.dateTimeSeg(this.commonmodule.sessionMgt_onlineTime("", "R"), currDate.toJSON(), "m");
    let tiemoutValue: number = parseInt(nodecenterobj.timeOutMinutes);

    if (difvalue > tiemoutValue) {
      if (this.quitFlag != "TimeOut") {
        this.quitFlag = "TimeOut";
        this.quitLog();
      }
    }
  }

  quitLog() {
    let currqueryobj: any = {};
    currqueryobj.USERNAME = this.frameenv.SysUserDict.USERNAME;
    currqueryobj.USERID = this.frameenv.SysUserDict.USERID;

    let reqdata: RequestParams = new RequestParams();
    reqdata.AddrType = "FrameMgt";
    reqdata.GetDataUrl = "/FrameApi/QuitUserForLoginN";
    reqdata.Reqmethod = "post";
    reqdata.Reqdata = JSON.stringify(currqueryobj);
    reqdata.ReqdataType = "";
    reqdata.Reqtype = "quitsystem";
    reqdata.Reqi18n = this.commonmodule.getI18nlang();
    reqdata.ServIdentifer = "frameManager";
    reqdata.ServVersionIden = "1.0";

    this.workbenchServ.quitSystem(reqdata,
      res => this.quitSystemResdataFunc(res), res => this.quitSystemDisplaydataFunc(res), err => this.quitSystemErrorFunc(err));
  }

  quitSystemResdataFunc(data: any): any {
    return data;
  }
  quitSystemDisplaydataFunc(data: any): void {
    if (data.status == 200) {
      let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
      if (respdata.respflag == "1") {
        this.commonmodule.removeToken()
        this.commonmodule.removeUserMenu();
        if (this.quitFlag == "Login") {
          this.router.navigateByUrl("login");
          //this.location.go("http://localhost:4200/")
        }
        else if (this.quitFlag == "Quit") {
          //TODO 关闭浏览器,没有实现成功，曲线解决问题
          this.router.navigateByUrl("quitsystem");
        }
        else if (this.quitFlag == "TimeOut") {
          //超时直接退出系统
          this.router.navigateByUrl("quitsystem");
        }

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
  quitSystemErrorFunc(err: string) {
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
  }


  //关闭浏览器, 测试没有成功
  closeWindow(): void {
    if (navigator.userAgent.indexOf("MSIE") > 0) {
      if (navigator.userAgent.indexOf("MSIE 6.0") > 0) {
        window.opener = null;
        window.close();
      } else {
        window.open('', '_top');
        window.top.close();
      }
    }
    else if (navigator.userAgent.indexOf("Firefox") > 0) {
      window.location.href = 'about:blank ';
    } else {
      window.opener = null;
      window.open('', '_self', '');
      window.close();
    }
  }


}
