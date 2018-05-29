import { NgModule, Component, Directive, ViewContainerRef, Type } from '@angular/core';

@Directive({
  selector: '[dynamic-load-component]'  
})
export class Dynamic_load_componentDirective {
  constructor(public viewContainerRef: ViewContainerRef) { };
}

export class Dynamic_componentItem {
  public componentSelectorName:string = "";
  constructor(public component: Type<any>, public componentParams: any) { };

}

export interface Dynamic_omponent {
  componentParams: any;
}


