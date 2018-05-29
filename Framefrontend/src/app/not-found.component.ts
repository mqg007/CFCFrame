import { Component } from '@angular/core';
import {Location} from '@angular/common';
@Component({
  selector:'app-404',
  templateUrl:'./not-found.component.html',
  styleUrls:['./not-found.component.css']
})
export class PageNotFoundComponent {
  constructor(private location: Location) {
  }
  goBack() {
    this.location.back();
  }
}

/* 
import { Component } from '@angular/core';
import {Location}               from "@angular/common";
@Component({
  template: `
    <img class="PageNotFoundComponent" (click)="goBack()"  src="../assets/image/timg.jpg" alt="页面没找到">
  `,
  styles: [ ' .PageNotFoundComponent  { height: 800px  }' ],
})
export class PageNotFoundComponent {
  constructor(private location: Location) {
  };
  goBack(){
    this.location.back();
  }
}
 */