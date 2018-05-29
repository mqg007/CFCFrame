import { Component, OnInit } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';

import { Message } from 'primeng/primeng';

import { MdDialog, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';

import { ConfirmDialogComponent } from '../common_module/confirm.dialog.component';
import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams, EnvData, CheckBizObjectRepatParams } from '../common_module/common.service';

import { SysmanagerService } from './sysmanager.service';

@Component({
  selector: 'app-sysmanager-group',
  templateUrl: './sysmanager.group.component.html',
  styleUrls: ['./sysmanager.group.component.css']

})
export class SysmanagerGroupComponent implements OnInit {

  framelang: any = {};
  commonlang: any = {};
  consticky: boolean = false;

  groupnames: any = [];
  groupname: string = "";
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
  selectrows: any = [];

  errorinfos: any = [];

  constructor(private sysmanagerServ: SysmanagerService, private router: Router, private commonmodule: CommonRootService, public pagingParam: PagingParam,
    private mainValueName: MainValueName, private checkDBParams: CheckBizObjectRepatParams, private dialog: MdDialog) {

  }

  ngOnInit() {

    this.framelang = JSON.parse(this.commonmodule.getFrameI18nlang());
    this.commonlang = JSON.parse(this.commonmodule.getCommonI18nlang());
    this.getGroupName();
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
    currqueryobj.GROUPID = this.groupname;
    currqueryobj.ISUSE = this.isuse;

    let groupreqobj = [{ queryItemKey: "SSY_PagingParam", queryItemValue: JSON.stringify(this.pagingParam) },
    { queryItemKey: "SSY_GROUP_DICT", queryItemValue: JSON.stringify(currqueryobj) }];

    let reqdata: RequestParams = new RequestParams();
    reqdata.AddrType = "FrameMgt";
    reqdata.GetDataUrl = "/FrameApi/GetGroupPagerN";
    reqdata.Reqmethod = "post";
    reqdata.Reqdata = JSON.stringify(groupreqobj);
    reqdata.ReqdataType = "";
    reqdata.Reqtype = "getgroupdata";
    reqdata.Reqi18n = this.commonmodule.getI18nlang();
    reqdata.ServIdentifer = "frameManager";
    reqdata.ServVersionIden = "1.0";

    this.sysmanagerServ.getGroups(reqdata,
      res => this.getDataResdataFunc(res), res => this.getDataDisplaydataFunc(res), err => this.getDataErrorFunc(err));
  }

  getDataResdataFunc(data: any): any {
    return data;
  }
  getDataDisplaydataFunc(data: any): void {
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
        this.currdata = this.commonmodule.copyArrayToNewArray(binddata);
        this.olddata = this.commonmodule.copyArrayToNewArray(binddata);
        this.tottlecounts = +respdata.pagertottlecounts;

        this.bindcurrdata = [];
        this.bindcurrdata = this.commonmodule.copyArrayToNewArray(binddata);
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
  getDataErrorFunc(err: string) {
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
    this.condiv = false;
  }

  //翻页处理
  changepage(event: any): void {
    this.getData(event.page);
  }

  selectGridRow(event: any): void {

    this.isTmp = true; //联动模板添加

  }

  //添加
  addData(): void {
    if (this.tottlecounts > 0) {
      this.tottlecounts = 0;
      this.currdata = [];
      this.olddata = [];
    }
    else {
      //赋值新数据
      this.currdata = this.commonmodule.copyArrayToNewArray(this.bindcurrdata);
    }

    //支持复制，若选择了前面的row,新增后直接复制，没选的话增加空row
    if (this.selectrows.length > 0 && this.isTmp) {
      for (let i = 0; i < this.selectrows.length; i++) {
        let newd_entity: any = this.selectrows[i];
        newd_entity.GROUPID = "" + i + Math.floor(Math.random() * (32 + 1));
        this.currdata.push(this.commonmodule.copyObjectToNewObject(newd_entity));
      }
      this.selectrows = [];
    }
    else {
      let newd_entity: any = {};
      newd_entity.GROUPID = "tmp" + Math.floor(Math.random() * (10000 + 1));
      newd_entity.GROUPNAME = "";
      newd_entity.GROUPNO = "";
      newd_entity.ISUSE = true;
      this.currdata.push(this.commonmodule.copyObjectToNewObject(newd_entity));
    }

    this.bindcurrdata = [];
    this.bindcurrdata = this.commonmodule.copyArrayToNewArray(this.currdata);
  }

  //删除
  delData(): void {
    if (this.selectrows.length > 0) {

      let datas: any = {
        actionTitle: this.commonlang.noticeInfoTitle, actionContent: this.commonlang.noticeDelAction,
        actionOkText: this.commonlang.action_cofirm, actionCancelText: this.commonlang.action_cancel
      };

      let dialogRef = this.dialog.open(ConfirmDialogComponent, { height: '100%', width: '100%', data: datas });
      dialogRef.afterClosed().subscribe(result => {
        if (result.operationstate == "yes") {
          for (let i = 0; i < this.selectrows.length; i++) {
            //原生查询有问题，不知道为啥
            //let index = this.currdata.indexOf(this.selectrows[i]);            
            //this.currdata.splice(index, 1);

            let mainkeys: MainValueName[] = [{ PropName: "GROUPID" }];
            let tmpArrayCurrSelectRow: string[] = [];
            tmpArrayCurrSelectRow.push(this.selectrows[i]);
            let index = this.commonmodule.existsArrayElement(mainkeys, tmpArrayCurrSelectRow, this.currdata);
            if (index >= 0) {
              this.currdata.splice(index, 1);
            }
          }
          this.selectrows = [];
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
      });

    }
    else {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticePleaseSelectData });
    }
  }

  //保存
  saveData(): void {
    this.consavediv = true;

    //赋值新数据
    this.currdata = this.commonmodule.copyArrayToNewArray(this.bindcurrdata);

    //校验数据
    if (!this.verifyData(this.currdata)) {
      this.consavediv = false;
      return;
    }

    //先清除临时增加的主键值
    for (let i = 0; i < this.currdata.length; i++) {
      if (this.currdata[i]["GROUPID"].substring(0, 3) == "tmp") {
        this.currdata[i]["GROUPID"] = "";
      }
    }
    //先比较变更结果，只提交有变更的数据,该函数处理比较结果并copy到submitdata数组
    let mainkeys: MainValueName[] = [{ PropName: "GROUPID" }];
    this.submitdata = this.commonmodule.compData(mainkeys, "OPFlag", this.currdata, this.olddata);
    if (this.submitdata.length <= 0) {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeDataNoChange });
      this.consavediv = false;
      return;
    }


    //查重复，当前表格 
    if (!this.checkCurrTableData(this.submitdata, this.currdata)) {
      this.consavediv = false;
      return;
    }

    //检查数据库重复, 需要分别检查用户组和编号
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
        checkNewDataSubmit.push(checkNewData[i]["GROUPNAME"] + "|" + checkNewData[i]["GROUPNO"]);
      }
      let bizobjop = [{ queryItemKey: "bizObjectName", queryItemValue: "SSY_GROUP_DICT" },
      { queryItemKey: "fields", queryItemValue: "GROUPNAME|GROUPNO" },
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
      let groupreqobjop: any = [{ queryItemKey: "objLists", queryItemValue: JSON.stringify(this.submitdata) },
      { queryItemKey: "opObjPropertyL", queryItemValue: JSON.stringify(["GROUPID", "GROUPNAME", "GROUPDESC", "ISUSE", "GROUPNO", "TIMESTAMPSS"]) },
      { queryItemKey: "wherePropertyL", queryItemValue: JSON.stringify(["GroupID"]) },
      { queryItemKey: "mainPropertyL", queryItemValue: JSON.stringify(["GroupID"]) }
      ];
      let reqdata: RequestParams = new RequestParams();
      reqdata.AddrType = "FrameMgt";
      reqdata.GetDataUrl = "/FrameApi/OpBizObjectSingle_SSY_GROUP_DICTN";
      reqdata.Reqmethod = "post";
      reqdata.Reqdata = JSON.stringify(groupreqobjop);
      reqdata.ReqdataType = "";
      reqdata.Reqtype = "savegroupdata";
      reqdata.Reqi18n = this.commonmodule.getI18nlang();
      reqdata.ServIdentifer = "frameManager";
      reqdata.ServVersionIden = "1.0";

      this.sysmanagerServ.saveGroupData(reqdata,
        res => this.getSaveDataResdataFunc(res), res => this.getSaveDataDisplaydataFunc(res), err => this.getSaveDataErrorFunc(err));
    }
  }

  //校验当前表格内数据是否合规
  verifyData(p_currdata: any): boolean {
    let checkpropparam: MainValueName[] = [{ PropName: "GROUPNAME" }, { PropName: "GROUPNO" }];
    let founderrs: string = "";
    let founderrone: string = "";
    let hasErr: boolean = false;
    for (let i = 0; i < p_currdata.length; i++) {
      hasErr = false;
      founderrone = "";
      for (let j = 0; j < checkpropparam.length; j++) {
        if (checkpropparam[j].PropName == "GROUPNAME") {
          if (p_currdata[i][checkpropparam[j].PropName] == "") {
            hasErr = true;
            founderrone = founderrone + " " + this.framelang.frmOpGroupGroupName + this.commonlang.label_chkGridDataVerifyEmpty;
          }
        }

        if (checkpropparam[j].PropName == "GROUPNO") {
          if (p_currdata[i][checkpropparam[j].PropName] == "") {
            hasErr = true;
            founderrone = founderrone + " " + this.framelang.frmOpGroupGroupColumnNo + this.commonlang.label_chkGridDataVerifyEmpty;
          }
        }
      }
      if (hasErr) {
        founderrs = founderrs + founderrone;
      }
    }

    if (founderrs != "") {
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: founderrs });
      return false;
    }
    else {
      return true;
    }
  }

  //检查当前表格内数据是否重复
  checkCurrTableData(p_currdata: any, p_olddata: any): boolean {
    //待检查属性
    let checkpropparam: MainValueName[] = [{ PropName: "GROUPNAME" }, { PropName: "GROUPNO" }];
    //var checkresult = this.commonmodule.checkHasData(checkpropparam, p_currdata, p_olddata);
    var checkresult = this.commonmodule.checkHasData(checkpropparam, p_olddata);
    if (checkresult.execute == "1") {
      //发现重复数据
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.label_chkGridDataRep + checkresult.noticeinfo });
      return false;
    }
    else if (checkresult.execute == "-1") {
      //检查重复键发生异常
      this.errorinfos = [];
      this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.label_chkGridDataRepErr + checkresult.noticeinfo });
      return false;
    }

    return true;

  }

  getSaveDataResdataFunc(data: any): any {
    return data;
  }
  getSaveDataDisplaydataFunc(data: any): void {
    this.consavediv = false;
    try {
      if (data.status == 200) {
        let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
        if (respdata.respflag == "1") {
          this.errorinfos = [];
          this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeSaveSuccess });
          this.tottlecounts = this.currdata.length;
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
  getSaveDataErrorFunc(err: string) {
    this.consavediv = false;
    this.errorinfos = [];
    this.errorinfos.push({ severity: 'warn', summary: this.commonlang.noticeInfoTitle, detail: this.commonlang.noticeHttpError_info + err });
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
          let groupreqobjop: any = [{ queryItemKey: "objLists", queryItemValue: JSON.stringify(this.submitdata) },
          { queryItemKey: "opObjPropertyL", queryItemValue: JSON.stringify(["GROUPID", "GROUPNAME", "GROUPDESC", "ISUSE", "GROUPNO", "TIMESTAMPSS"]) },
          { queryItemKey: "wherePropertyL", queryItemValue: JSON.stringify(["GroupID"]) },
          { queryItemKey: "mainPropertyL", queryItemValue: JSON.stringify(["GroupID"]) }
          ];
          let reqdata: RequestParams = new RequestParams();
          reqdata.AddrType = "FrameMgt";
          reqdata.GetDataUrl = "/FrameApi/OpBizObjectSingle_SSY_GROUP_DICTN";
          reqdata.Reqmethod = "post";
          reqdata.Reqdata = JSON.stringify(groupreqobjop);
          reqdata.ReqdataType = "";
          reqdata.Reqtype = "savegroupdata";
          reqdata.Reqi18n = this.commonmodule.getI18nlang();
          reqdata.ServIdentifer = "frameManager";
          reqdata.ServVersionIden = "1.0";

          this.sysmanagerServ.saveGroupData(reqdata,
            res => this.getSaveDataResdataFunc(res), res => this.getSaveDataDisplaydataFunc(res), err => this.getSaveDataErrorFunc(err));
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
