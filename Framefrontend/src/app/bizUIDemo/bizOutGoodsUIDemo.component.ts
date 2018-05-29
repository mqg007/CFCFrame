import { Component, OnInit } from '@angular/core';

import { MdDialog, MdDialogRef, MdDialogModule, MD_DIALOG_DATA } from '@angular/material';
import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams } from '../../app/common_module/common.service';

@Component({
  selector: 'app-bizOutGoodsUIDemo',
  templateUrl: './bizOutGoodsUIDemo.component.html',
  styleUrls: ['./bizOutGoodsUIDemo.component.css']
})
export class BizOutGoodsUIDemoComponent implements OnInit {
  
  constructor(public commonmodule: CommonRootService) {

  }
  
  ngOnInit() {

  }
}
