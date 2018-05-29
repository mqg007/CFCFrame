import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions, Request} from '@angular/http';
import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams} from '../common_module/common.service';


@Injectable()
export class WorkbenchService {
  constructor(public commonmodule: CommonRootService) {
  }

  //get biz munu 
  getBizMenu(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc:any): void{
    
    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);
    
  } 

 //退出系统
  quitSystem(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc:any): void{

    this.commonmodule.getHttpInfoDataAuth(reqdata, resdataFunc, displayresdataFunc, errorFunc);
    
  }
  
}//class end
