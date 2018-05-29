import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';

import { MdDialog, MdDialogRef, MdDialogModule, MD_DIALOG_DATA } from '@angular/material'; 

import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams } from '../../app/common_module/common.service';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm.dialog.component.html',
  styleUrls: ['./confirm.dialog.component.css']
})
export class ConfirmDialogComponent implements OnInit {

  actionTitle: string = "";
  actionContent: string = "";
  actionOkText: string = "";
  actionCancelText: string = "";

  constructor(private dialogRef: MdDialogRef<ConfirmDialogComponent>, private commonmodule: CommonRootService, @Inject(MD_DIALOG_DATA) public data: any) {  }

  ngOnInit() {

    this.actionTitle = this.data.actionTitle;
    this.actionContent = this.data.actionContent; 
    this.actionOkText = this.data.actionOkText; 
    this.actionCancelText = this.data.actionCancelText; 
  }


  sureYes(): void {

    this.dialogRef.close({ "operationstate": "yes" });

  }

  sureNo(): void {

    this.dialogRef.close({ "operationstate": "cancel" });

  }

}
