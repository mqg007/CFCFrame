import { Component, OnInit  } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';
import { Location } from "@angular/common";


import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams, EnvData } from '../common_module/common.service';

import { WorkbenchService } from './workbench.service';

@Component({
  selector: 'app-workbench',
  templateUrl: './workbench.component.html',
  styleUrls: ['./workbench.component.css'],
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
export class WorkbenchComponent implements OnInit {

  /************************* define  ********************************/
  menus: any = [];                                    //menu
  state: string = 'inactive';                      //menu state
  username: string                                 //head account name
  frameenvStr:string;
  frameenv:any;
  constructor(private location: Location, private workbenchServ: WorkbenchService, public router: Router,
    public commonmodule: CommonRootService) {
  };

  ngOnInit() {
    
    this.frameenvStr= this.commonmodule.getToken();
    if (this.frameenvStr) {
      this.frameenv = JSON.parse(this.frameenvStr);
      if(this.frameenv.SysUserDict){
        this.username = this.frameenv.SysUserDict.USERID;
      }      
    }

    if (this.frameenv.SysUserDict.USERID == "" || this.frameenv.SysUserDict.USERID == undefined) {
      this.router.navigateByUrl("login");
    }
    else{
      this.getBizMenu();
    }      
  } 

  //获取业务菜单
  getBizMenu(): void {
   
    let reqdata: RequestParams = new RequestParams();
    reqdata.AddrType = "FrameMgt";
    reqdata.GetDataUrl = "/FrameApi/GetPagesN";
    reqdata.Reqmethod = "post";
    reqdata.Reqdata = JSON.stringify(this.frameenv.SysUserDict);
    reqdata.ReqdataType = "";
    reqdata.Reqtype = "initmenu";
    reqdata.Reqi18n = this.frameenv.I18nCurrLang;
    reqdata.ServIdentifer = "frameManager";
    reqdata.ServVersionIden = "1.0";


    this.workbenchServ.getBizMenu(reqdata,
      res => this.getBizMenuResdataFunc(res), res => this.getBizMenuDisplaydataFunc(res), err => this.getBizMenuErrorFunc(err));
  }

  //加载菜单
  getBizMenuResdataFunc(data: any): any {
    return data;
  }
  getBizMenuDisplaydataFunc(data: any): void {
    if (data.status == 200) {
      let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
      if (respdata.respflag == "1") {
        //组织菜单
        let currmenus:any = JSON.parse(respdata.respdata);
        for (let i = 0; i < currmenus.length; i++) {
          //一级菜单
          if (currmenus[i].PAGEID.length < 4 && currmenus[i].PAGEID.length > 2) {
            let onemenu: any = {
              "menuid": "",
              "name": "",
              "icon": "",
              "link": "01",
              "sorted": "",
              "widgetname": "",
              "selectorname": "",
              "children": []
            };
            onemenu.menuid = currmenus[i].PAGEID;
            onemenu.name = currmenus[i].PAGENAME;
            onemenu.icon = currmenus[i].PAGEIMG;
            onemenu.link = currmenus[i].PAGEID;
            onemenu.sorted = currmenus[i].SEQSORT;
            onemenu.widgetname = currmenus[i].PAGEID;
            onemenu.selectorname = currmenus[i].PAGEID;
            
            let onecchildrens:any = [];
            for (let two = 0; two < currmenus.length; two++) {
              //二级菜单
              if (currmenus[two].PAGEPARENTID == currmenus[i].PAGEID) {
                let onechildren: any = {
                  "menuid": "",
                  "name": "",
                  "icon": "",
                  "link": "",
                  "sorted": "1",
                  "children": []
                };

                onechildren.menuid = currmenus[two].PAGEID;
                onechildren.name = currmenus[two].PAGENAME;
                onechildren.icon = currmenus[two].PAGEIMG;
                onechildren.link = currmenus[i].PAGEID + "/" +currmenus[two].PAGEID;
                onechildren.sorted = currmenus[two].SEQSORT;

                let onecchildrensons:any = [];
                for (var thr = 0; thr < currmenus.length; thr++) {
                  //三级菜单
                  if (currmenus[thr].PAGEPARENTID == currmenus[two].PAGEID) {
                    let onechildrenson: any = {
                      "menuid": "",
                      "name": "",
                      "icon": "",
                      "link": "",
                      "sorted": "1",
                      "children": []
                    };

                    onechildrenson.menuid = currmenus[thr].PAGEID;
                    onechildrenson.name = currmenus[thr].PAGENAME;
                    onechildrenson.icon = currmenus[thr].PAGEIMG;
                    onechildrenson.link = currmenus[two].PAGEID + "/" + currmenus[thr].PAGEID;
                    onechildrenson.sorted = currmenus[thr].SEQSORT;
                    onecchildrensons.push(onechildrenson);

                    onechildren.children = onecchildrensons;
                  }
                }
                onecchildrens.push(onechildren);
              }              
            }            
            onemenu.children = onecchildrens;
            this.menus.push(onemenu);
          }
        }

        //保存业务菜单备用        
        this.commonmodule.sessionMgt_bizMenu(JSON.stringify(this.menus), "S");       
      }
    }
  }

  getBizMenuErrorFunc(err: string) {
  } 
  
}
