import { Injectable } from '@angular/core';
import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams } from '../common_module/common.service';

@Injectable()
export class SysmanagerService {
  constructor(private commonmodule: CommonRootService) { }


  //获取用户组名
  getGroupName(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  getGroupNameTest(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any, delayedCnt:number): void {

    this.commonmodule.getHttpInfoDataAuthWithDelayed(reqdata, resdataFunc, displayresdataFunc, errorFunc, delayedCnt);

  }

  //获取用户组
  getGroups(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //执行数据库检查
  executeDBCheck(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //保存用户组
  saveGroupData(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //获取页面名称
  getPageName(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //获取页面
  getPageData(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //保存页面
  savePageData(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //获取用户信息
  getUserdictData(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }
  

  //获取用户信息
  getUserdictDataTest(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any, delayedCnt:number): void {

    this.commonmodule.getHttpInfoDataAuthWithDelayed(reqdata, resdataFunc, displayresdataFunc, errorFunc, delayedCnt);

  }

  //保存用户
  saveUserdictData(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //获取用户组与用户信息
  getGroup_UserData(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //获取用户组与用户信息
  getGroup_UserDataTest(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any, delayedCnt:number): void {

    this.commonmodule.getHttpInfoDataAuthWithDelayed(reqdata, resdataFunc, displayresdataFunc, errorFunc, delayedCnt);

  }

  //保存用户组与用户
  saveGroup_UserData(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //获取用户组与功能
  getgrouppagedata(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

 //获取用户组与功能
  getgrouppagedataTest(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any, delayedCnt:number): void {

    this.commonmodule.getHttpInfoDataAuthWithDelayed(reqdata, resdataFunc, displayresdataFunc, errorFunc, delayedCnt);

  }

  //保存用户组与功能
  savegrouppagedata(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //获取全部用户信息
  getUserdictAllData(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //重置用户默认密码
  resetUserPWD(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //修改用户密码
  modifyUserPWD(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //查询日志信息
  getLogData(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }


  //获取系统字典 
  getSystemDictData(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }


  //保存系统字典
  saveSystemDictData(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //获取全部系统字典
  getFrameDicts(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //获取业务字典
  getBizDictData(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //保存业务字典
  saveBizDictData(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

  //获取全部业务字典
  getBizDicts(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }

 //检查数据库是否存在
  checkBizObjectsRepat(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);

  }



}//class end
