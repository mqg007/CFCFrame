import { Component, OnInit, ComponentFactoryResolver, ViewChild, ViewContainerRef } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';

import {MdDialog, MdDialogRef, MD_DIALOG_DATA} from '@angular/material';

import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams } from '../../app/common_module/common.service';
@Component({
  selector: 'app-bizUIDemo',
  templateUrl: './bizUIDemo.component.html',
  styleUrls: ['./bizUIDemo.component.css'],
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
export class BizUIDemoComponent implements OnInit { 

  /************************* define  ********************************/  
  state: string = 'inactive';                      //menu state
  groups: any[] = [];
  parmenuid:string = "001";
  conmenu:string = "";
  playMenu:boolean = true;
  

  constructor(public router: Router, public commonmodule: CommonRootService) { }

  ngOnInit() {
    this.conmenu = "../../assets/image/menu-hide.png";
    this.initLeftMenu(this.parmenuid);
  }  

  initLeftMenu(parent:any):void {
    let menus:any = JSON.parse(this.commonmodule.sessionMgt_bizMenu("", "R")); 
    for (let i = 0; i < menus.length; i++) {
        if(menus[i].menuid == parent){
          if(menus[i].children.length > 0){
              this.groups = menus[i].children;
          }          
          break;
        }
    } 
  }

  controlSubs(menucount:any):boolean {
    if(menucount.children != null){
      return menucount.children.length > 0;
    }   
   return false;
  }

  conMenuPlay():void{
  /*   if(this.playMenu){
      this.conmenu = "../../assets/image/menu-show.png";
    }
    else{
      this.conmenu = "../../assets/image/menu-hide.png";
    } */
    this.playMenu = !this.playMenu;
  }

}
