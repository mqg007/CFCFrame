import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/operator/map';

import { Message } from 'primeng/primeng';

import { MdDialog, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';

import { ConfirmDialogComponent } from '../common_module/confirm.dialog.component';
import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams, EnvData, CheckBizObjectRepatParams, TextValue } from '../common_module/common.service';

import { SysmanagerService } from './sysmanager.service';

@Component({
  selector: 'app-sysmanager-resetpwd',
  templateUrl: './sysmanager.resetpwd.component.html',
  styleUrls: ['./sysmanager.resetpwd.component.css']

})
export class SysmanagerResetpwdComponent implements OnInit {

  framelang: any = {};
  commonlang: any = {};
  consticky: boolean = false;

  allUser: any = [];
  userCtrl: FormControl;
  users: TextValue[] = [];
  filterusersOver: Observable<TextValue[]>;
  curruser: any = {};
  selectObj:any = {};

  errorinfos: any = [];

  constructor(private sysmanagerServ: SysmanagerService, private router: Router, private commonmodule: CommonRootService, 
    private mainValueName: MainValueName, private checkDBParams: CheckBizObjectRepatParams, private dialog: MdDialog) {

    this.userCtrl = new FormControl();

  }
  ngOnInit() {

    this.framelang = JSON.parse(this.commonmodule.getFrameI18nlang());
    this.commonlang = JSON.parse(this.commonmodule.getCommonI18nlang());
    this.getAllUserData();

     //定时加载当前选定用户
    setInterval(() => {
      this.initCurrUser();
    }, 100);
  }

  //获取全部用户
  getAllUserData(): void {

    let currqueryobj: any = { USERID: "", USERNAME: "" };
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

  filterUsers(name: string): any[] {
    return this.users.filter(userobj =>
      userobj.text.toLowerCase().indexOf(name.toLowerCase()) === 0
    );
  }
  displayUserFn(curritem: TextValue): string {
    return curritem ? curritem.text : curritem.value;
  }

  //初始化当前选定用户
  initCurrUser(): void {
    if (this.selectObj.value != undefined) {
      for (let i = 0; i < this.allUser.length; i++) {
        if (this.allUser[i].USERID == this.selectObj.value) {
          this.curruser = this.commonmodule.copyObjectToNewObject(this.allUser[i]);
        }
      }     
    }
    else{
      this.curruser = {};
    }
  }

  getUserDictDataResdataFunc(data: any): any {
    return data;
  }
  getUserDictDataDisplaydataFunc(data: any): void {
    if (data.status == 200) {
      let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
      if (respdata.respflag == "1") {
        this.allUser = JSON.parse(respdata.respdata);
        let tmpUser = JSON.parse(respdata.respdata);
        for (let i = 0; i < tmpUser.length; i++) {
          this.users.push(new TextValue(tmpUser[i].USERNAME, tmpUser[i].USERID));
        }
        this.filterusersOver = this.userCtrl.valueChanges
          .startWith(null)
          .map(userobj => userobj && typeof userobj === 'object' ? userobj.text : userobj)
          .map(name => name ? this.filterUsers(name) : this.users.slice());
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

  //重置用户口令
  resetUserPWD(): void {

    let datas: any = {
      actionTitle: this.commonlang.noticeInfoTitle, actionContent: this.framelang.frmReSetUserPwdConfirmResetPwd,
      actionOkText: this.commonlang.action_cofirm, actionCancelText: this.commonlang.action_cancel
    };

    let dialogRef = this.dialog.open(ConfirmDialogComponent, { height: '100%', width: '100%', data: datas });
    dialogRef.afterClosed().subscribe(result => {
      if (result.operationstate == "yes") {

        let reqdata: RequestParams = new RequestParams();
        reqdata.AddrType = "FrameMgt";
        reqdata.GetDataUrl = "/FrameApi/ResetUserPWDN";
        reqdata.Reqmethod = "post";
        this.curruser.PASSWORD = ""; //重置口令,后台赋值默认口令
        reqdata.Reqdata = JSON.stringify(this.curruser);
        reqdata.ReqdataType = "";
        reqdata.Reqtype = "resetuserpwd";
        reqdata.Reqi18n = this.commonmodule.getI18nlang();
        reqdata.ServIdentifer = "frameManager";
        reqdata.ServVersionIden = "1.0";

        this.sysmanagerServ.resetUserPWD(reqdata,
          res => this.resetUserPWDResdataFunc(res), res => this.resetUserPWDDisplaydataFunc(res), err => this.resetUserPWDErrorFunc(err));

      }
      else if (result.operationstate == "cancel") {

      }
    });

  }

  resetUserPWDResdataFunc(data: any): any {
    return data;
  }
  resetUserPWDDisplaydataFunc(data: any): void {
    try {
      if (data.status == 200) {
        let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
        this.errorinfos = [];
        this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: respdata.resptoolstr });
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
  resetUserPWDErrorFunc(err: string) {
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
  }





}
