import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions, Request} from '@angular/http';
import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams} from '../common_module/common.service';

@Injectable()
export class LoginService {
  constructor(public commonmodule: CommonRootService) {}

  //i18nframelang
  getI18nFrameLang(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc:any): void{
    
    this.commonmodule.getHttpInfoData(reqdata, resdataFunc, displayresdataFunc, errorFunc);
    
  } 

  //i18ncommonlang
  getI18nCommonLang(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc:any): void{
    
    this.commonmodule.getHttpInfoData(reqdata, resdataFunc, displayresdataFunc, errorFunc);
    
  } 

  //useservices
  getUseServices(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc:any): void{
    
    this.commonmodule.getHttpInfoData(reqdata, resdataFunc, displayresdataFunc, errorFunc);
    
  } 

  //nodecenteraddr
  getNodeCenterAddr(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc:any): void{
    
    this.commonmodule.getHttpInfoData(reqdata, resdataFunc, displayresdataFunc, errorFunc);
    
  } 

  //biznodeaddr
  getBizNodeAddr(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc:any): void{
    
    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);
    
  } 

  //Login 
  login(userToken:string, reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc:any): void{
    
    //this.commonmodule.getHttpInfoData(reqdata, resdataFunc, displayresdataFunc, errorFunc);
     this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);
    
  } 

  //get munu 
  getMenu(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc:any): void{
    
    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);
    
  } 


  
}//class end
