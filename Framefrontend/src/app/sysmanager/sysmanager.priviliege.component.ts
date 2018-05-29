import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/operator/map';

import { Message, TreeNode } from 'primeng/primeng';

import { MdDialog, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';

import { ConfirmDialogComponent } from '../common_module/confirm.dialog.component';
import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams, EnvData, CheckBizObjectRepatParams, TextValue } from '../common_module/common.service';

import { SysmanagerService } from './sysmanager.service';

@Component({
  selector: 'app-sysmanager-priviliege',
  templateUrl: './sysmanager.priviliege.component.html',
  styleUrls: ['./sysmanager.priviliege.component.css']

})
export class SysmanagerPriviliegeComponent implements OnInit {

  framelang: any = {};
  commonlang: any = {};
  consticky: boolean = false;

  groupnames: any = [];
  groupid: string = "";

  allUser: any = [];
  users: TextValue[] = [];
  selectObj: any = {};
  userCtrl: FormControl;
  filterusersOver: Observable<TextValue[]>;
  curruserID: string = "";

  condiv: boolean = false;
  consavediv: boolean = false;
  tottlecounts: number = 0;
  currdata: any = [];
  bindcurrdata: any = [];
  olddata: any = [];
  submitdata: any = [];
  selectrows: any = {}

  errorinfos: any = [];
  functionTree: TreeNode[];
  selectedFunctions: TreeNode[];

  currgrouppages = []; //当前组所有功能
  currgrouppagesold = []; //当前组所有功能备份
  currgrouppages_submitdata = []; //需要到数据库处理的数据

  constructor(private sysmanagerServ: SysmanagerService, private router: Router, private commonmodule: CommonRootService, public pagingParam: PagingParam,
    private mainValueName: MainValueName, private checkDBParams: CheckBizObjectRepatParams, private dialog: MdDialog) {

    this.userCtrl = new FormControl();

  }

  ngOnInit() {

    this.framelang = JSON.parse(this.commonmodule.getFrameI18nlang());
    this.commonlang = JSON.parse(this.commonmodule.getCommonI18nlang());
    this.initBizFunction();
    this.commonmodule.delayTimeMS(100);
    this.getGroupName();
    this.commonmodule.delayTimeMS(100);
    this.getUserData();

    //定时加载加载当前选定用户
    setInterval(() => {
      this.initCurrUser();
    }, 100);
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
          this.curruserID = this.allUser[i].USERID;
        }
      }     
    }
    else{
      this.curruserID = "";
    }
  }


  //转换业务功能
  initBizFunction(): void {
    let functionTreeArray: any = [];
    let bizFunction: any = JSON.parse(this.commonmodule.sessionMgt_bizMenu("", "R"));
    for (let i = 0; i < bizFunction.length; i++) {
      //一级
      let onemenu: any = {
        "label": "",
        "data": "",
        "icon": "",
        "expandedIcon": "",
        "collapsedIcon": "",
        "children": []
      };
      onemenu.data = bizFunction[i].menuid;
      onemenu.label = bizFunction[i].name;
      onemenu.icon = bizFunction[i].icon;
      onemenu.expandedIcon = bizFunction[i].icon;
      onemenu.collapsedIcon = bizFunction[i].icon;

      let onecchildrens: any = [];
      for (let j = 0; j < bizFunction[i].children.length; j++) {
        let twomenu: any = {
          "label": "",
          "data": "",
          "icon": "",
          "expandedIcon": "",
          "collapsedIcon": "",
          "children": []
        };

        twomenu.data = bizFunction[i].children[j].menuid;
        twomenu.label = bizFunction[i].children[j].name;
        twomenu.icon = bizFunction[i].children[j].icon;
        twomenu.expandedIcon = bizFunction[i].children[j].icon;
        twomenu.collapsedIcon = bizFunction[i].children[j].icon;
        let twochildrens: any = [];
        for (let m = 0; m < bizFunction[i].children[j].children.length; m++) {
          let threemenu: any = {
            "label": "",
            "data": "",
            "icon": "",
            "expandedIcon": "",
            "collapsedIcon": "",
            "children": []
          };

          threemenu.data = bizFunction[i].children[j].children[m].menuid;
          threemenu.label = bizFunction[i].children[j].children[m].name;
          threemenu.icon = bizFunction[i].children[j].children[m].icon;
          threemenu.expandedIcon = bizFunction[i].children[j].children[m].icon;
          threemenu.collapsedIcon = bizFunction[i].children[j].children[m].icon;
          twochildrens.push(threemenu);
        }
        twomenu.children = twochildrens;

        onecchildrens.push(twomenu);
      }
      onemenu.children = onecchildrens;

      functionTreeArray.push(onemenu);
    }

    this.functionTree = functionTreeArray;
  }

  //清楚菜单联动状态
  clearMenuState(): void {
    for (let i = 0; i < this.functionTree.length; i++) {
      if (this.functionTree[i].partialSelected) {
        this.functionTree[i].partialSelected = false
      }
      for (let m = 0; m < this.functionTree[i].children.length; m++) {
        if (this.functionTree[i].children[m].partialSelected) {
          this.functionTree[i].children[m].partialSelected = false
        }
      }
    }
  }

  //转换当前组业务功能
  //BizFunction2Menu  业务功能到菜单树  Menu2BizFunction  菜单树都业务功能 
  initCurrGroupBizFunction(opaction: string): void {
    if (opaction == "BizFunction2Menu") {
      this.selectedFunctions = [];
      for (let i = 0; i < this.functionTree.length; i++) {
        //处理二级
        if (this.functionTree[i].children.length > 0) {
          let oneChildCount: any = 0;
          for (let m = 0; m < this.functionTree[i].children.length; m++) {
            //处理三级
            if (this.functionTree[i].children[m].children.length > 0) {
              let twoChildCount: any = 0;
              for (let n = 0; n < this.functionTree[i].children[m].children.length; n++) {
                for (let j = 0; j < this.currgrouppages.length; j++) {
                  if (this.functionTree[i].children[m].children[n].data == this.currgrouppages[j].PAGEID && this.groupid == this.currgrouppages[j].GROUPID) {
                    this.selectedFunctions.push(this.functionTree[i].children[m].children[n]);
                    twoChildCount = twoChildCount + 1;
                    break;
                  }
                }
              }

              //全部选中加载时，加载父节点
              if (twoChildCount == this.functionTree[i].children[m].children.length) {
                this.functionTree[i].children[m].partialSelected = false;
                this.selectedFunctions.push(this.functionTree[i].children[m]);
                oneChildCount = oneChildCount + 1;
              }
              else {
                if (this.functionTree[i].children[m].children.length == 1) {

                }
                else {
                  if (twoChildCount > 0) {
                    this.functionTree[i].children[m].partialSelected = true;
                    this.selectedFunctions.push(this.functionTree[i].children[m]);
                  }

                }

              }
            }
            else {
              for (let j = 0; j < this.currgrouppages.length; j++) {
                if (this.functionTree[i].children[m].data == this.currgrouppages[j].PAGEID && this.groupid == this.currgrouppages[j].GROUPID) {
                  this.selectedFunctions.push(this.functionTree[i].children[m]);
                  oneChildCount = oneChildCount + 1;
                  break;
                }
              }
            }

          }

          //全部选中加载时，加载父节点
          if (oneChildCount == this.functionTree[i].children.length) {
            this.functionTree[i].partialSelected = false;
            this.selectedFunctions.push(this.functionTree[i]);

          }
          else {
            if (this.functionTree[i].children.length == 1) {

            }
            else {
              if (oneChildCount > 0) {
                this.functionTree[i].partialSelected = true;
                this.selectedFunctions.push(this.functionTree[i]);
              }

            }

          }
        }
        else {
          //处理一级
          for (let j = 0; j < this.currgrouppages.length; j++) {
            if (this.functionTree[i].data == this.currgrouppages[j].PAGEID && this.groupid == this.currgrouppages[j].GROUPID) {
              this.selectedFunctions.push(this.functionTree[i]);
              break;
            }
          }
        }

      }

    }
    else if (opaction == "Menu2BizFunction") {
      this.currgrouppages = [];
      for (let i = 0; i < this.selectedFunctions.length; i++) {
        if (!this.checkMenuExists(this.currgrouppages, this.selectedFunctions[i])) {
          this.currgrouppages.push({ GROUPID: this.groupid, PAGEID: this.selectedFunctions[i].data, OPFlag: "" });
        }

        if (this.selectedFunctions[i].parent != undefined) {
          if (!this.checkMenuExists(this.currgrouppages, this.selectedFunctions[i].parent)) {
            this.currgrouppages.push({ GROUPID: this.groupid, PAGEID: this.selectedFunctions[i].parent.data, OPFlag: "" });
          }

          if (this.selectedFunctions[i].parent.parent != undefined) {
            if (!this.checkMenuExists(this.currgrouppages, this.selectedFunctions[i].parent.parent)) {
              this.currgrouppages.push({ GROUPID: this.groupid, PAGEID: this.selectedFunctions[i].parent.parent.data, OPFlag: "" });
            }
          }
        }

      }

    }

  }

  //检查是否存在配置中
  checkMenuExists(pCurrGroupPages: any, pCurrTreeNode: TreeNode): boolean {
    for (let i = 0; i < pCurrGroupPages.length; i++) {
      if (pCurrGroupPages[i].PAGEID == pCurrTreeNode.data) {
        return true;
      }
    }

    return false;
  }

  //获取当前组已有功能
  getGroupFunctions(): void {

    let reqdata: RequestParams = new RequestParams();
    let currreqobj: any = { PAGEID: "", GROUPID: "" };
    currreqobj.PAGEID = "";
    currreqobj.GROUPID = this.groupid;

    if (this.groupid != "") {
        reqdata.AddrType = "FrameMgt";
      reqdata.GetDataUrl = "/FrameApi/GetGroupPageMgtN";
      reqdata.Reqmethod = "post";
      reqdata.Reqdata = JSON.stringify(currreqobj);
      reqdata.ReqdataType = "";
      reqdata.Reqtype = "getgrouppagedata";
      reqdata.Reqi18n = this.commonmodule.getI18nlang();
      reqdata.ServIdentifer = "frameManager";
      reqdata.ServVersionIden = "1.0";

      this.sysmanagerServ.getgrouppagedata(reqdata,
      res => this.getGroupPageDataResdataFunc(res), res => this.getGroupPageDataDisplaydataFunc(res), 
      err => this.getGroupPageDataErrorFunc(err));


     /*  this.sysmanagerServ.getgrouppagedataTest(reqdata,
        res => this.getGroupPageDataResdataFunc(res), 
        res => this.getGroupPageDataDisplaydataFunc(res), 
        err => this.getGroupPageDataErrorFunc(err), 5000); */
    }


  }
  getGroupPageDataResdataFunc(data: any): any {
    return data;
  }
  getGroupPageDataDisplaydataFunc(data: any): void {
    if (data.status == 200) {
      let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
      if (respdata.respflag == "1") {
        let funcresults = JSON.parse(respdata.respdata);
        this.currgrouppages = this.commonmodule.copyArrayToNewArray(funcresults.SSY_GROUP_PAGE_DICT);
        this.currgrouppagesold = this.commonmodule.copyArrayToNewArray(funcresults.SSY_GROUP_PAGE_DICT);

        //初始化当前组已有功能
        this.initCurrGroupBizFunction("BizFunction2Menu");

      }
      else {
        this.selectedFunctions = [];
        this.currgrouppages = [];
        this.currgrouppagesold = [];
        this.clearMenuState();
      }
    }
  }
  getGroupPageDataErrorFunc(err: string) {
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
  }

  //保存用户组和业务功能设置
  saveGroupFunctions(): void {
    if (this.groupid == "") {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.framelang.frmOpPrivilegeSelectGroup });
      return
    }
    else if (this.selectedFunctions.length <= 0) {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.framelang.frmOpPrivilegeSelectFunction });
      return
    }

    this.initCurrGroupBizFunction("Menu2BizFunction");

    //保存用户组功能配置
    //先比较变更结果，只提交有变更的数据,该函数处理比较结果并copy到submitdata数组
    let mainkeys: MainValueName[] = [{ PropName: "GROUPID" }, { PropName: "PAGEID" }];
    this.currgrouppages_submitdata = this.commonmodule.compData(mainkeys, "OPFlag", this.currgrouppages, this.currgrouppagesold);
    if (this.currgrouppages_submitdata.length <= 0) {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeDataNoChange });
      return;
    }

    let bizobjop = [{ queryItemKey: "objLists", queryItemValue: JSON.stringify(this.currgrouppages_submitdata) },
    { queryItemKey: "opObjPropertyL", queryItemValue: JSON.stringify(["PAGEID", "GROUPID", "TIMESTAMPSS"]) },
    { queryItemKey: "wherePropertyL", queryItemValue: JSON.stringify(["PAGEID", "GROUPID"]) },
    { queryItemKey: "mainPropertyL", queryItemValue: JSON.stringify(["PAGEID", "GROUPID"]) }
    ];

    let reqdata: RequestParams = new RequestParams();
    reqdata.AddrType = "FrameMgt";
    reqdata.GetDataUrl = "/FrameApi/OpBizObjectSingle_SSY_GROUP_PAGE_DICTN";
    reqdata.Reqmethod = "post";
    reqdata.Reqdata = JSON.stringify(bizobjop);
    reqdata.ReqdataType = "";
    reqdata.Reqtype = "savegrouppagedata";
    reqdata.Reqi18n = this.commonmodule.getI18nlang();
    reqdata.ServIdentifer = "frameManager";
    reqdata.ServVersionIden = "1.0";

    this.sysmanagerServ.savegrouppagedata(reqdata,
      res => this.saveGroupPageDataResdataFunc(res), res => this.saveGroupPageDataDisplaydataFunc(res), err => this.saveGroupPageDataErrorFunc(err));

  }

  saveGroupPageDataResdataFunc(data: any): any {
    return data;
  }
  saveGroupPageDataDisplaydataFunc(data: any): void {
    try {
      if (data.status == 200) {
        let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
        if (respdata.respflag == "1") {
          this.currgrouppagesold = [];
          this.currgrouppagesold = this.commonmodule.copyArrayToNewArray(this.currgrouppages);
          this.currgrouppages_submitdata = [];

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

  saveGroupPageDataErrorFunc(err: string) {
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
    this.querydata();
  }

  //选择用户组
  selectGroup(): void {
    this.querydata();

    this.commonmodule.delayTimeMS(1000);

    this.getGroupFunctions();
    
  }

  //获取用户组
  getGroupName(): void {

    let reqdata: RequestParams = new RequestParams();
    let currreqobj = { params: "" };
    reqdata.AddrType = "FrameMgt";
    reqdata.GetDataUrl = "/FrameApi/GetGroupN";
    reqdata.Reqmethod = "post";
    reqdata.Reqdata = "";
    reqdata.ReqdataType = "";
    reqdata.Reqtype = "getgroupname";
    reqdata.Reqi18n = this.commonmodule.getI18nlang();
    reqdata.ServIdentifer = "frameManager";
    reqdata.ServVersionIden = "1.0";

    this.sysmanagerServ.getGroupName(reqdata,
    res => this.getGroupNameResdataFunc(res), res => this.getGroupNameDisplaydataFunc(res), err => this.getGroupNameErrorFunc(err));

    //增加延时处理
   /*  this.sysmanagerServ.getGroupNameTest(reqdata,
      res => this.getGroupNameResdataFunc(res), res => this.getGroupNameDisplaydataFunc(res), err => this.getGroupNameErrorFunc(err), 3000); */

  }
  getGroupNameResdataFunc(data: any): any {
    return data;
  }
  getGroupNameDisplaydataFunc(data: any): void {
    if (data.status == 200) {
      let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
      if (respdata.respflag == "1") {
        this.groupnames = JSON.parse(respdata.respdata);
      }
    }
  }
  getGroupNameErrorFunc(err: string) {
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
  }


  //获取全部用户
  getUserData(): void {
    let currqueryobj: any = { USERID: "", USERNAME: "" };
    let reqdata: RequestParams = new RequestParams();
    reqdata.AddrType = "FrameMgt";
    reqdata.GetDataUrl = "/FrameApi/GetAllUsersN";
    reqdata.Reqmethod = "post";
    reqdata.Reqdata = JSON.stringify(currqueryobj);
    reqdata.ReqdataType = "";
    reqdata.Reqtype = "getuserdictdata";
    reqdata.Reqi18n = this.commonmodule.getI18nlang();
    reqdata.ServIdentifer = "frameManager";
    reqdata.ServVersionIden = "1.0";

    this.sysmanagerServ.getUserdictData(reqdata,
    res => this.getUserDictDataResdataFunc(res), res => this.getUserDictDataDisplaydataFunc(res), err => this.getUserDictDataErrorFunc(err));

  }

  getUserDictDataResdataFunc(data: any): any {
    return data;
  }
  getUserDictDataDisplaydataFunc(data: any): void {
    this.condiv = false;
    this.bindcurrdata = [];
    this.tottlecounts = 0;
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
    }
  }
  getUserDictDataErrorFunc(err: string) {
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
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
    currqueryobj.GROUPID = this.groupid;

    if (this.groupid != "") {
      let pagereqobj = [{ queryItemKey: "SSY_PagingParam", queryItemValue: JSON.stringify(this.pagingParam) },
      { queryItemKey: "SSY_USER_GROUP_DICT", queryItemValue: JSON.stringify(currqueryobj) }];

      let reqdata: RequestParams = new RequestParams();
      reqdata.AddrType = "FrameMgt";
      reqdata.GetDataUrl = "/FrameApi/GetGroupUserPagerN";
      reqdata.Reqmethod = "post";
      reqdata.Reqdata = JSON.stringify(pagereqobj);
      reqdata.ReqdataType = "";
      reqdata.Reqtype = "getgroupuserdata";
      reqdata.Reqi18n = this.commonmodule.getI18nlang();
      reqdata.ServIdentifer = "frameManager";
      reqdata.ServVersionIden = "1.0";

      this.sysmanagerServ.getGroup_UserData(reqdata,
        res => this.getGroup_UserDataResdataFunc(res), res => this.getGroup_UserDataDisplaydataFunc(res), err => this.getGroup_UserDataErrorFunc(err));

      //this.sysmanagerServ.getGroup_UserDataTest(reqdata,
      //res => this.getGroup_UserDataResdataFunc(res), res => this.getGroup_UserDataDisplaydataFunc(res), err => this.getGroup_UserDataErrorFunc(err), 3000);


    }


  }

  getGroup_UserDataResdataFunc(data: any): any {
    return data;
  }
  getGroup_UserDataDisplaydataFunc(data: any): void {
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

        for (let i = 0; i < binddata.length; i++) {
          binddata[i]["groupuserid"] = binddata[i]["GROUPID"] + binddata[i]["USERID"];
        }

        this.currdata = binddata;
        this.olddata = this.commonmodule.copyArrayToNewArray(binddata);
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
  getGroup_UserDataErrorFunc(err: string) {
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
    this.condiv = false;
  }

  //翻页处理
  changepage(event: any): void {
    this.getData(event.page);
  }

  //删除
  delData(): void {
    if (this.selectrows.GROUPID != "" && this.selectrows.GROUPID != undefined) {
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
          if (this.olddata.length > 0) {
            //自动保存
            this.saveData("D");
          }
        }
        else if (result.operationstate == "cancel") {

        }
      });

    }
    else {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticePleaseSelectData });
    }
  }

  //保存
  saveData(opaction: string): void {
    if (opaction == "D") {
      //先比较变更结果，只提交有变更的数据,该函数处理比较结果并copy到submitdata数组
      let mainkeys: MainValueName[] = [{ PropName: "GROUPID" }, { PropName: "USERID" }];
      this.submitdata = this.commonmodule.compData(mainkeys, "OPFlag", this.currdata, this.olddata);
      if (this.submitdata.length <= 0) {
        this.errorinfos = [];
        this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeDataNoChange });
        return;
      }
    }
    else if (opaction == "I") {
      //构造新数据
      this.submitdata = [];
      /* 直接使用USERID
      for (let i = 0; i < this.users.length; i++) {
        if (this.users[i]["USERNAME"] == this.selectusername) {
          this.curruserID = this.users[i]["USERID"];
          break;
        }
      }
      */
      this.submitdata.push({ GROUPID: this.groupid, USERID: this.curruserID, OPFlag: "I" })
      //查重复，当前表格 
      if (!this.checkCurrData(this.olddata, this.curruserID)) {
        return;
      }
    }

    //只检查新增数据，修改数据不做检查，确定那个数据项修改了太复杂了，交给数据库唯一性检查
    let checkNewData = [];
    for (let i = 0; i < this.submitdata.length; i++) {
      if (this.submitdata[i]["OPFlag"] == "I") {
        checkNewData.push(this.commonmodule.copyObjectToNewObject(this.submitdata[i]));
      }
    }
    if (checkNewData.length > 0) {
      //组织检查重复数据
      let checkNewDataSubmit = [];
      for (let i = 0; i < checkNewData.length; i++) {
        checkNewDataSubmit.push(checkNewData[i]["USERID"] + ";" + checkNewData[i]["GROUPID"]);
      }
      let bizobjop = [{ queryItemKey: "bizObjectName", queryItemValue: "SSY_USER_GROUP_DICT" },
      { queryItemKey: "fields", queryItemValue: "USERID;GROUPID" },
      { queryItemKey: "splitChar", queryItemValue: "|" },
      { queryItemKey: "splitCharSub", queryItemValue: ";" },
      { queryItemKey: "fieldsValue", queryItemValue: JSON.stringify(checkNewDataSubmit) }
      ];

      let reqdata: RequestParams = new RequestParams();
      reqdata.AddrType = "FrameMgt";
      reqdata.GetDataUrl = "/FrameApi/CheckBizObjectsRepat";
      reqdata.Reqmethod = "post";
      reqdata.Reqdata = JSON.stringify(bizobjop);
      reqdata.ReqdataType = "";
      reqdata.Reqtype = "CheckBizObjectsRepat";
      reqdata.Reqi18n = this.commonmodule.getI18nlang();
      reqdata.ServIdentifer = "frameManager";
      reqdata.ServVersionIden = "1.0";

      this.sysmanagerServ.checkBizObjectsRepat(reqdata,
        res => this.checkBizObjectsRepatResdataFunc(res), res => this.checkBizObjectsRepatDisplaydataFunc(res), err => this.checkBizObjectsRepatErrorFunc(err));
    }
    else {
      //提交数据进行保存
      let bizobjop = [{ queryItemKey: "objLists", queryItemValue: JSON.stringify(this.submitdata) },
      { queryItemKey: "opObjPropertyL", queryItemValue: JSON.stringify(["USERID", "GROUPID", "TIMESTAMPSS"]) },
      { queryItemKey: "wherePropertyL", queryItemValue: JSON.stringify(["USERID", "GROUPID"]) },
      { queryItemKey: "mainPropertyL", queryItemValue: JSON.stringify(["USERID", "GROUPID"]) }
      ];

      let reqdata: RequestParams = new RequestParams();
      reqdata.AddrType = "FrameMgt";
      reqdata.GetDataUrl = "/FrameApi/OpBizObjectSingle_SSY_GROUP_USERN";
      reqdata.Reqmethod = "post";
      reqdata.Reqdata = JSON.stringify(bizobjop);
      reqdata.ReqdataType = "";
      reqdata.Reqtype = "savegroupuserdata";
      reqdata.Reqi18n = this.commonmodule.getI18nlang();
      reqdata.ServIdentifer = "frameManager";
      reqdata.ServVersionIden = "1.0";

      this.sysmanagerServ.saveGroup_UserData(reqdata,
        res => this.getsaveGroup_UserDataResdataFunc(res), res => this.getsaveGroup_UserDataDisplaydataFunc(res), err => this.getsaveGroup_UserDataErrorFunc(err));

    }


  }

  getsaveGroup_UserDataResdataFunc(data: any): any {
    return data;
  }
  getsaveGroup_UserDataDisplaydataFunc(data: any): void {
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

    this.querydata();
  }
  getsaveGroup_UserDataErrorFunc(err: string) {
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
    this.querydata();
  }

  //检查当前数据是否存在
  checkCurrData(p_currdata: any, p_curruser: any): boolean {
    //待检查属性
    if (this.groupid == "") {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.framelang.frmOpPrivilegeSelectGroup });
      return false;
    }
    else if (this.curruserID == "") {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.framelang.frmOpPrivilegeSelectUser });
      return false;
    }
    else {
      let hasData: boolean = false;
      for (let i = 0; i < p_currdata.length; i++) {
        if (p_currdata[i]["GROUPID"] == this.groupid && p_currdata[i]["USERID"] == p_curruser) {
          hasData = true;
          break;
        }
      }
      if (hasData) {
        //发现重复数据
        this.errorinfos = [];
        this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.framelang.frmOpPrivilegeCurrUserExists });
        return false;
      }
    }

    return true;
  }


  checkBizObjectsRepatResdataFunc(data: any): any {
    return data;
  }
  checkBizObjectsRepatDisplaydataFunc(data: any): void {
    try {
      if (data.status == 200) {
        let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
        if (respdata.respflag == "1") {
          //提交数据进行保存
          let bizobjop = [{ queryItemKey: "objLists", queryItemValue: JSON.stringify(this.submitdata) },
          { queryItemKey: "opObjPropertyL", queryItemValue: JSON.stringify(["USERID", "GROUPID", "TIMESTAMPSS"]) },
          { queryItemKey: "wherePropertyL", queryItemValue: JSON.stringify(["USERID", "GROUPID"]) },
          { queryItemKey: "mainPropertyL", queryItemValue: JSON.stringify(["USERID", "GROUPID"]) }
          ];

          let reqdata: RequestParams = new RequestParams();
          reqdata.AddrType = "FrameMgt";
          reqdata.GetDataUrl = "/FrameApi/OpBizObjectSingle_SSY_GROUP_USERN";
          reqdata.Reqmethod = "post";
          reqdata.Reqdata = JSON.stringify(bizobjop);
          reqdata.ReqdataType = "";
          reqdata.Reqtype = "savegroupuserdata";
          reqdata.Reqi18n = this.commonmodule.getI18nlang();
          reqdata.ServIdentifer = "frameManager";
          reqdata.ServVersionIden = "1.0";

          this.sysmanagerServ.saveGroup_UserData(reqdata,
            res => this.getsaveGroup_UserDataResdataFunc(res), res => this.getsaveGroup_UserDataDisplaydataFunc(res), err => this.getsaveGroup_UserDataErrorFunc(err));
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
        this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpReturnError });
      }
    }
    catch (e) {
      this.consavediv = false;
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.anaReturnResultErr + e.message });
    }
  }
  checkBizObjectsRepatErrorFunc(err: string) {
    this.consavediv = false;
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
  }


}
