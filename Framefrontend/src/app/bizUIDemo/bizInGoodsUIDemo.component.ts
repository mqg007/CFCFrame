import { Component, OnInit } from '@angular/core';

import { MdDialog, MdDialogRef, MdDialogModule, MD_DIALOG_DATA } from '@angular/material';
import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams } from '../../app/common_module/common.service';

@Component({
  selector: 'app-bizInGoodsUIDemo',
  templateUrl: './bizInGoodsUIDemo.component.html',
  styleUrls: ['./bizInGoodsUIDemo.component.css']
})
export class BizInGoodsUIDemoComponent implements OnInit {
  
  constructor(public commonmodule: CommonRootService) {

  }
  
  ngOnInit() {

  }
}
