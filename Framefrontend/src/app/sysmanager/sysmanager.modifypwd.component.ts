import { Component, OnInit } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';

import { Message } from 'primeng/primeng';

import { MdDialog, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';

import { ConfirmDialogComponent } from '../common_module/confirm.dialog.component';
import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams, EnvData, CheckBizObjectRepatParams } from '../common_module/common.service';

import { SysmanagerService } from './sysmanager.service';

@Component({
  selector: 'app-sysmanager-modifypwd',
  templateUrl: './sysmanager.modifypwd.component.html',
  styleUrls: ['./sysmanager.modifypwd.component.css']

})
export class SysmanagerModifypwdComponent implements OnInit {

  frameenvStr: string = "";
  frameenv: any = {};
  framelang: any = {};
  commonlang: any = {};
  consticky: boolean = false;

  useridq: any = "";
  usernameq: any = "";
  userpwdoldq: any = "";
  userpwdnewq: any = "";
  userpwdnew2q: any = "";
  password: any = "";

  errorinfos: any = [];

  constructor(private sysmanagerServ: SysmanagerService, private router: Router, private commonmodule: CommonRootService,
    private mainValueName: MainValueName, private checkDBParams: CheckBizObjectRepatParams, private dialog: MdDialog) {

  }

  ngOnInit() {

    this.framelang = JSON.parse(this.commonmodule.getFrameI18nlang());
    this.commonlang = JSON.parse(this.commonmodule.getCommonI18nlang());

    this.frameenvStr = this.commonmodule.getToken();
    if (this.frameenvStr) {
      this.frameenv = JSON.parse(this.frameenvStr);
      if (this.frameenv.SysUserDict) {
        this.useridq = this.frameenv.SysUserDict.USERID;
        this.password = this.frameenv.SysUserDict.PASSWORD;

        let currqueryobj: any = { USERID: this.useridq, USERNAME: "" };
        let reqdata: RequestParams = new RequestParams();
        reqdata.AddrType = "FrameMgt";
        reqdata.GetDataUrl = "/FrameApi/GetAllUsersN";
        reqdata.Reqmethod = "post";
        reqdata.Reqdata = JSON.stringify(currqueryobj);
        reqdata.ReqdataType = "";
        reqdata.Reqtype = "getuserdictalldata";
        reqdata.Reqi18n = this.commonmodule.getI18nlang();
        reqdata.ServIdentifer = "frameManager";
        reqdata.ServVersionIden = "1.0";

        this.sysmanagerServ.getUserdictData(reqdata,
          res => this.getUserDictDataResdataFunc(res), res => this.getUserDictDataDisplaydataFunc(res), err => this.getUserDictDataErrorFunc(err));


      }
    }
  }

  getUserDictDataResdataFunc(data: any): any {
    return data;
  }
  getUserDictDataDisplaydataFunc(data: any): void {
    if (data.status == 200) {
      let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
      if (respdata.respflag == "1") {
        let userone: any = JSON.parse(respdata.respdata);
        this.usernameq = userone[0].USERNAME;
      }
      else {
        this.errorinfos = [];
        this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: respdata.resptoolstr });
      }
    }
  }
  getUserDictDataErrorFunc(err: string) {
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
  }



  //修改
  modifyUserPWD(): void {

    let currqueryobj: any = { USERID: this.useridq, USERNAME: this.usernameq, PASSWORD: this.userpwdnewq };

    //密码检查
    if (this.userpwdoldq == "") {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.framelang.frmModifyUserPwdOldPwdNotEmpty });
      //ComFnServ.noticeInfo($rootScope.i18nFramePackage.frmModifyUserPwdOldPwdNotEmpty);
      return;
    }
    if (this.userpwdnewq == "") {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.framelang.frmModifyUserPwdNewPwdNotEmpty });
      //ComFnServ.noticeInfo($rootScope.i18nFramePackage.frmModifyUserPwdNewPwdNotEmpty);
      return;
    }
    if (this.userpwdnewq.length < 6 || this.userpwdnewq.length > 13) {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.framelang.frmModifyUserPwdNewPwdCheck });
      //ComFnServ.noticeInfo($rootScope.i18nFramePackage.frmModifyUserPwdNewPwdCheck);
      return;
    }
    if (this.userpwdnew2q == "") {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.framelang.frmModifyUserPwdSureNewPwdNotEmpty });
      //ComFnServ.noticeInfo($rootScope.i18nFramePackage.frmModifyUserPwdSureNewPwdNotEmpty);
      return;
    }
    if (this.userpwdnew2q.length < 6 || this.userpwdnew2q.length > 13) {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.framelang.frmModifyUserPwdSureNewPwdCheck });
      //ComFnServ.noticeInfo($rootScope.i18nFramePackage.frmModifyUserPwdSureNewPwdCheck);
      return;
    }
    if (this.userpwdnewq != this.userpwdnew2q) {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.framelang.frmModifyUserPwdNew2OldPwdNotEquals });
      //ComFnServ.noticeInfo($rootScope.i18nFramePackage.frmModifyUserPwdNew2OldPwdNotEquals);
      return;
    }
    if (this.password != this.userpwdoldq) {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.framelang.frmModifyUserPwdOldPwdError });
      //ComFnServ.noticeInfo($rootScope.i18nFramePackage.frmModifyUserPwdOldPwdError);
      return;
    }

    //TODO 预留补充密码强弱提示

    let datas: any = {
      actionTitle: this.commonlang.noticeInfoTitle, actionContent: this.framelang.frmModifyUserPwdConfirmModifyPwd,
      actionOkText: this.commonlang.action_cofirm, actionCancelText: this.commonlang.action_cancel
    };

    let dialogRef = this.dialog.open(ConfirmDialogComponent, { height: '100%', width: '100%', data: datas });
    dialogRef.afterClosed().subscribe(result => {
      if (result.operationstate == "yes") {

        let reqdata: RequestParams = new RequestParams();
        reqdata.AddrType = "FrameMgt";
        reqdata.GetDataUrl = "/FrameApi/ResetUserPWDN";
        reqdata.Reqmethod = "post";

        //加密口令
        let nodecenterobj: any = JSON.parse(this.commonmodule.getNodeCenterAddr());
        let key = nodecenterobj.keypwd;
        let iv = nodecenterobj.ivpwd;
        let enpwd = this.commonmodule.encrypt(this.userpwdnewq, key, iv);
        currqueryobj.PASSWORD = enpwd;
        
        reqdata.Reqdata = JSON.stringify(currqueryobj);
        reqdata.ReqdataType = "";
        reqdata.Reqtype = "modifyuserpwd";
        reqdata.Reqi18n = this.commonmodule.getI18nlang();
        reqdata.ServIdentifer = "frameManager";
        reqdata.ServVersionIden = "1.0";

        this.sysmanagerServ.modifyUserPWD(reqdata,
          res => this.modifyUserPWDResdataFunc(res), res => this.modifyUserPWDDisplaydataFunc(res), err => this.modifyUserPWDErrorFunc(err));

      }
      else if (result.operationstate == "cancel") {

      }
    });

  }

  modifyUserPWDResdataFunc(data: any): any {
    return data;
  }
  modifyUserPWDDisplaydataFunc(data: any): void {
    if (data.status == 200) {
      let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: respdata.resptoolstr });
    }
    else {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeError });
    }
  }
  modifyUserPWDErrorFunc(err: string) {
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
  }

  //清空
  clearEmptyPWD(): void {
    this.userpwdoldq = "";
    this.userpwdnewq = "";
    this.userpwdnew2q = "";
  }


}
