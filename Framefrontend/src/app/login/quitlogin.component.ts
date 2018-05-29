import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';
import { Base64 } from 'js-base64';
//import NProgress from 'nprogress';

import { Message } from 'primeng/primeng';

import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams } from '../common_module/common.service';

@Component({
  selector: 'app-quitlogin',
  templateUrl: './quitlogin.component.html',
  styleUrls: ['./quitlogin.component.css']
})
export class QuitLoginComponent implements OnInit {

  framelang: any = {};
  commonlang: any = {};

  constructor(public router: Router, public commonmodule: CommonRootService) { }

  ngOnInit() {
    //NProgress.done();
    this.framelang = JSON.parse(this.commonmodule.getFrameI18nlang());
    this.commonlang = JSON.parse(this.commonmodule.getCommonI18nlang());
  }

  reLogin(): void {
    this.router.navigateByUrl("login");
  }

}
