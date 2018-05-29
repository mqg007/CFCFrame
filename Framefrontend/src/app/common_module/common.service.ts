import { NgModule } from '@angular/core';
import { Injectable } from '@angular/core';
import { HttpModule, Headers, Http, Response, Request, RequestOptionsArgs, ResponseOptionsArgs, RequestOptions } from '@angular/http';
import { trigger, state, style, transition, animate, keyframes } from '@angular/animations';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';

//使用三方js库
declare var CryptoJS:any;

@Injectable()
export class CommonRootService {    

    constructor(private http: Http) { }

     //AES加密
    public encrypt(word: string, key: string, iv: string): string {
        let keys = CryptoJS.enc.Utf8.parse(key);
        let ivs = CryptoJS.enc.Utf8.parse(iv);
        let srcs = CryptoJS.enc.Utf8.parse(word);
        let encrypted = CryptoJS.AES.encrypt(srcs, keys, { iv: ivs, mode: CryptoJS.mode.CBC });
        return encrypted.toString();
    }

    public decrypt(word: string, key: string, iv: string) {
        let keys = CryptoJS.enc.Utf8.parse(key);
        let ivs = CryptoJS.enc.Utf8.parse(iv);
        let decrypt = CryptoJS.AES.decrypt(word, keys, { iv: ivs, mode: CryptoJS.mode.CBC });
        return CryptoJS.enc.Utf8.stringify(decrypt).toString();
    }

    public delayTimeMS(msseconds: number = 0): void {
        for (let t = Date.now(); Date.now() - t <= msseconds;);
    }

/*

    public encrypt2(data: string, key: string) {
        let crypto = require("crypto-browserify");
        let algorithm = "aes256";
        let encoding = "base64";
        var cipher = crypto.createCipher(algorithm, key);

        return cipher.update(data, "utf8", encoding) + cipher.final(encoding);
    }

    public decrypt2(data: string, key: string) {
        let crypto = require("crypto-browserify");
        let algorithm = "aes256";
        let encoding = "base64";
        var decipher = crypto.createDecipher(algorithm, key);
        return decipher.update(data, encoding, "utf8") + decipher.final("utf8");
    }

 
    //encryption for AES 
    encrypt(word: string, key: string, iv: string) {
        var key = CryptoJS.enc.Utf8.parse(key);
        var iv = CryptoJS.enc.Utf8.parse(iv);
        var srcs = CryptoJS.enc.Utf8.parse(word);
        var encrypted = CryptoJS.AES.encrypt(srcs, key, { iv: iv, mode: CryptoJS.mode.CBC });
        return encrypted.toString();
    }
    //Decrypted for AES
    decrypt(word: string, key: string, iv: string) {
        var key = CryptoJS.enc.Utf8.parse(key);
        var iv = CryptoJS.enc.Utf8.parse(iv);
        var decrypt = CryptoJS.AES.decrypt(word, key, { iv: iv, mode: CryptoJS.mode.CBC });
        return CryptoJS.enc.Utf8.stringify(decrypt).toString();
    }
  

*/

    public TestAlert(tipstr: string): void {
        alert("test public method for encapsulation！" + tipstr);        
    }

    //copy array, return a new array
    public copyArrayToNewArray(oldarr: string[]): string[] {
        let resarry: string[] = [];
        /*
        for (let i = 0; i < oldarr.length; i++) {
            resarry.push(oldarr[i]);
        }
        */
        resarry = JSON.parse(JSON.stringify(oldarr));

        return resarry;
    }

    //copy object, return a new object
    public copyObjectToNewObject(oldarr: any): any {
        let resarry: any;
        //resarry = oldarr;
        resarry = JSON.parse(JSON.stringify(oldarr));
        return resarry;
    }

    //compare data array
    public compData(mainValueName: MainValueName[], opFlagName: string, p_currdata: string[], p_olddata: string[]): string[] {
        let p_submitdata = [];
        let mainValue = ""; //Primary key value, 
        let mainValueOld = ""; //Historical target primary key value
        let matchSuccess = false; //Match successful
        let findindex = 0; //Match the array index
        //Forward comparison, processing I M
        for (let i = 0; i < p_currdata.length; i++) {
            matchSuccess = false;
            findindex = 0;
            //Get primary key values, multiple string mosaics
            mainValue = "";
            for (let m = 0; m < mainValueName.length; m++) {
                mainValue = mainValue + p_currdata[i][mainValueName[m].PropName] + "|";
            }
            //Find the history directory primary key
            for (let j = 0; j < p_olddata.length; j++) {
                mainValueOld = "";
                for (let m = 0; m < mainValueName.length; m++) {
                    mainValueOld = mainValueOld + p_olddata[j][mainValueName[m].PropName] + "|";
                }

                if (mainValue == mainValueOld) {
                    matchSuccess = true;
                    findindex = j;
                    break;
                }
            }
            if (matchSuccess) {
                /*
                //Match, and then compare whether there is a change, there is a change, no neglect             
                //Need to remove the private property $$ hashKey               
                if (JSON.stringify(p_currdata[i]).substring(0, JSON.stringify(p_currdata[i]).lastIndexOf(',')) + "}" !=
                    JSON.stringify(p_olddata[findindex])) {
                    p_currdata[i][opFlagName] = "M";
                    p_submitdata.push(p_currdata[i]);
                }
                */
                if (JSON.stringify(p_currdata[i]) != JSON.stringify(p_olddata[findindex])) {
                    p_currdata[i][opFlagName] = "M";
                    p_submitdata.push(p_currdata[i]);
                }
            }
            else {
                //No match, is added
                p_currdata[i][opFlagName] = "I";
                p_submitdata.push(p_currdata[i]);
            }
        }
        //Reverse comparison, processing D
        for (let i = 0; i < p_olddata.length; i++) {
            matchSuccess = false;
            findindex = 0;
            //Get primary key values, multiple string mosaics
            mainValue = "";
            for (let m = 0; m < mainValueName.length; m++) {
                mainValue = mainValue + p_olddata[i][mainValueName[m].PropName] + "|";
            }
            //Find the history directory primary key
            for (let j = 0; j < p_currdata.length; j++) {
                mainValueOld = "";
                for (let m = 0; m < mainValueName.length; m++) {
                    mainValueOld = mainValueOld + p_currdata[j][mainValueName[m].PropName] + "|";
                }
                if (mainValue == mainValueOld) {
                    matchSuccess = true;
                    findindex = j;
                    break;
                }
            }
            if (matchSuccess) {
                //Match, the last has been dealt with, this time ignored             
            }
            else {
                //No match, is deleted
                p_olddata[i][opFlagName] = "D";
                p_submitdata.push(p_olddata[i]);
            }
        }

        return p_submitdata; //Returns the data to be submitted
    }

    //After the operation is successful, re-assign the new and old data
    public setCurrData(olddata: string[], currdata: string[], submitdata: string[]) {
        olddata = [];
        olddata = this.copyArrayToNewArray(currdata);
        submitdata = [];
    }

    //Checks whether there is currently new data in the current table data
    //checkprop, 要检查的重复字段，每个项都要独立检查
    /* 历史处理
    public checkHasData(checkprop: MainValueName[], currselectobj: string[], currdata: string[]) {
        let returnvalue = { execute: "", noticeinfo: "" };
        let hasData = true;
        let repdata = ""; //Repeat data, multiple attribute string splicing, with | segmentation
        if (currselectobj.length > 0) {
            //Check the current data
            for (let i = 0; i < currselectobj.length; i++) {
                for (let j = 0; j < checkprop.length; j++) {    
                    hasData = true;
                    let fields:string[] = checkprop[j].PropName.split("|"); //同时检查多个字段一起不能重复，用|分割字段
                     for (let m = 0; m < currdata.length; m++) {
                         let partRespData:string = "";
                         let partHasData:boolean = false;
                         for (let nf = 0; nf < fields.length; nf++) {
                             if (currselectobj[i][fields[nf]] == currdata[m][fields[nf]] 
                             //&& JSON.stringify(currdata[i]) != JSON.stringify(currselectobj[m])
                             //&& i != m 
                             ) {
                                 partHasData = partHasData && true;
                                 partRespData = partRespData + currdata[m][fields[nf]] + "|";
                             }
                             else {
                                 partHasData = false; //多字段时有一个不同就不重复
                                 break;
                             }
                         }
                         
                         if (partHasData) {
                             hasData = hasData && true;
                             repdata = repdata + partRespData + "|";
                             break;
                         }                         
                     }
                }
                if(repdata != ""){
                    break;  //发现有一行重复，后续不在进行
                }
            }

            if (repdata != "") {
                //exist
                returnvalue = { execute: "1", noticeinfo: repdata }
            }
            else {
                //does not exist
                returnvalue = { execute: "0", noticeinfo: repdata }
            }
        }
        else {
            //Did not find the data being checked
            returnvalue = { execute: "-1", noticeinfo: "falied. no find data!" }
        }
        return returnvalue;
    }
    */

    public checkHasData(checkprop: MainValueName[], currdata: string[]) {
        let returnvalue = { execute: "", noticeinfo: "" };
        let hasData = true;
        let repdata = ""; //Repeat data, multiple attribute string splicing, with | segmentation
        if (currdata.length > 0) {
            //Check the current data
            for (let i = 0; i < currdata.length; i++) {
                for (let j = 0; j < checkprop.length; j++) {
                    hasData = true;
                    let fields: string[] = checkprop[j].PropName.split("|"); //同时检查多个字段一起不能重复，用|分割字段
                    for (let m = 0; m < currdata.length; m++) {
                        let partRespData: string = "";
                        let partHasData: boolean = true;
                        for (let nf = 0; nf < fields.length; nf++) {
                            if (currdata[i][fields[nf]] == currdata[m][fields[nf]] && i != m) {
                                partHasData = partHasData && true;
                                partRespData = partRespData + currdata[m][fields[nf]] + "|";
                            }
                            else {
                                partHasData = false; //多字段时有一个不同就不重复
                                break;
                            }
                        }

                        if (partHasData) {
                            hasData = hasData && true;
                            repdata = repdata + partRespData + "|";
                            break;
                        }
                    }
                }
                if (repdata != "") {
                    break;  //发现有一行重复，后续不在进行
                }
            }

            if (repdata != "") {
                //exist
                returnvalue = { execute: "1", noticeinfo: repdata }
            }
            else {
                //does not exist
                returnvalue = { execute: "0", noticeinfo: repdata }
            }
        }
        else {
            //Did not find the data being checked
            returnvalue = { execute: "-1", noticeinfo: "falied. no find data!" }
        }
        return returnvalue;
    }

    //根据识别字段查询数组元素是否存在
    public existsArrayElement(checkprop: MainValueName[], currselectobj: string[], currdata: string[]): number {
        let indexRow: number = -1;
        for (let m = 0; m < currdata.length; m++) {
            let partRespData: string = "";
            let partHasData: boolean = true;
            for (let nf = 0; nf < checkprop.length; nf++) {
                if (currdata[m][checkprop[nf].PropName] == currselectobj[0][checkprop[nf].PropName]) {
                    partHasData = partHasData && true;
                    partRespData = partRespData + currdata[m][checkprop[nf].PropName] + "|";
                }
                else {
                    partHasData = false; //多字段时有一个不同就不重复
                    break;
                }
            }

            if (partHasData) {
                indexRow = m;
                break;
            }
        }

        return indexRow;
    }


    //set i18nlang
    public setI18nlang(token: string) {
        sessionStorage.setItem("sessioni18nlang", token);
    }
    //get i18nlang
    public getI18nlang(): string {
        var resobjs = "";
        if (sessionStorage.getItem("sessioni18nlang") != null && sessionStorage.getItem("sessioni18nlang").toString() != "") {
            resobjs = sessionStorage.getItem("sessioni18nlang");
        }

        return resobjs;
    }
    //remove i18nlang
    public removeI18nlang() {
        sessionStorage.removeItem('sessioni18nlang');
    }

    //set i18nframelang
    public setFrameI18nlang(currframelang: string) {
        sessionStorage.setItem("sessioni18nframelang", currframelang);
    }
    //get i18nframelang
    public getFrameI18nlang(): string {
        var resobjs = "";
        if (sessionStorage.getItem("sessioni18nframelang") != null && sessionStorage.getItem("sessioni18nframelang").toString() != "") {
            resobjs = sessionStorage.getItem("sessioni18nframelang");
        }

        return resobjs;
    }
    //remove i18nframelang
    public removeFrameI18nlang() {
        sessionStorage.removeItem('sessioni18nframelang');
    }

    //set i18ncommonlang
    public setCommonI18nlang(token: string) {
        sessionStorage.setItem("sessioni18ncommonlang", token);
    }
    //get i18nlang
    public getCommonI18nlang(): string {
        var resobjs = "";
        if (sessionStorage.getItem("sessioni18ncommonlang") != null && sessionStorage.getItem("sessioni18ncommonlang").toString() != "") {
            resobjs = sessionStorage.getItem("sessioni18ncommonlang");
        }

        return resobjs;
    }
    //remove i18nlang
    public removeCommonI18nlang() {
        sessionStorage.removeItem('sessioni18ncommonlang');
    }

    //管理会话 remusername
    public sessionMgt_remUsername(username: string, actionflag: string) {
        var resobjs: any;
        if (actionflag == "R") {
            if (localStorage.getItem("sessionremusername") != null && localStorage.getItem("sessionremusername").toString() != "") {
                resobjs = localStorage.getItem("sessionremusername");
            }
        }
        else if (actionflag == "S") {
            localStorage.setItem("sessionremusername", username);
        }
        else if (actionflag == "D") {
            localStorage.removeItem('sessionremusername');
        }
        return resobjs;

    }

    //管理会话 rempassword
    public sessionMgt_remPassword(pwd: string, actionflag: string): any {
        var resobjs: any;
        if (actionflag == "R") {
            if (localStorage.getItem("sessionrempassword") != null && localStorage.getItem("sessionrempassword").toString() != "") {
                resobjs = localStorage.getItem("sessionrempassword");
            }
        }
        else if (actionflag == "S") {
            localStorage.setItem("sessionrempassword", pwd);
        }
        else if (actionflag == "D") {
            localStorage.removeItem('sessionrempassword');
        }
        return resobjs;
    }
    //end

    //set Token
    public setToken(token: string) {
        sessionStorage.setItem("sessiontoken", token);
    }
    //get Token
    public getToken(): string {
        var resobjs = "";
        if (sessionStorage.getItem("sessiontoken") != null && sessionStorage.getItem("sessiontoken").toString() != "") {
            resobjs = sessionStorage.getItem("sessiontoken");
        }

        return resobjs;
    }
    //remove token
    public removeToken() {
        sessionStorage.removeItem('sessiontoken');
    }

    //Manage frameAllEnv,包含全部用户信息
    public sessionMgt_frameAllEnv(allframeenv: string, actionflag: string): any {
        var resobjs: any;
        if (actionflag == "R") {
            if (localStorage.getItem("sessionframeAllEnv") != null && localStorage.getItem("sessionframeAllEnv").toString() != "") {
                resobjs = localStorage.getItem("sessionframeAllEnv");
            }
        }
        else if (actionflag == "S") {
            localStorage.setItem("sessionframeAllEnv", allframeenv);
        }
        else if (actionflag == "D") {
            localStorage.removeItem('sessionframeAllEnv');
        }
        return resobjs;
    }

    //Manage onlineTime,记录当前在线时间
    public sessionMgt_onlineTime(currrOnLineTime: string, actionflag: string): any {
        var resobjs: any;
        if (actionflag == "R") {
            if (localStorage.getItem("sessiononlineTime") != null && localStorage.getItem("sessiononlineTime").toString() != "") {
                resobjs = localStorage.getItem("sessiononlineTime");
            }
        }
        else if (actionflag == "S") {
            localStorage.setItem("sessiononlineTime", currrOnLineTime);
        }
        else if (actionflag == "D") {
            localStorage.removeItem('sessiononlineTime');
        }
        return resobjs;
    }

    //set usermenu
    public setUserMenu(usermenu: string) {
        sessionStorage.setItem("sessionusermenu", usermenu);
    }
    //get usermenu
    public getUserMenu(): string {
        var resobjs = "";
        if (sessionStorage.getItem("sessionusermenu") != null && sessionStorage.getItem("sessionusermenu").toString() != "") {
            resobjs = sessionStorage.getItem("sessionusermenu");
        }

        return resobjs;
    }
    //remove usermenu
    public removeUserMenu() {
        sessionStorage.removeItem('sessionusermenu');
    }

    //Management Framework Language Pack
    public getI18nFramePackage(): string {
        var resobjs = "";
        if (sessionStorage["sessionI18nFramePackage"] != null && sessionStorage["sessionI18nFramePackage"].toString() != "") {
            resobjs = sessionStorage["sessionI18nFramePackage"];
        }
        return resobjs;
    }

    public setI18nFramePackage(str: string) {
        sessionStorage.setItem("sessionI18nFramePackage", str);
    }
    //remove I18nFramePackage
    public removeI18nFramePackage() {
        sessionStorage.removeItem('sessionI18nFramePackage');
    }

    //Manage common language packs
    public getI18nCommonPackage(): string {
        var resobjs = "";
        //if (sessionStorage.getItem("sessionI18nCommonPackage") != null && sessionStorage.getItem("sessionI18nCommonPackage").toString() != "") {
        //    resobjs = sessionStorage.getItem("sessionI18nCommonPackage");
        //}

        if (sessionStorage["sessionI18nCommonPackage"] != null && sessionStorage["sessionI18nCommonPackage"].toString() != "") {
            resobjs = sessionStorage["sessionI18nCommonPackage"];
        }

        return resobjs;
    }
    public setI18nCommonPackage(str: string) {
        sessionStorage.setItem("sessionI18nCommonPackage", str);
    }
    public removeI18nCommonPackage() {
        sessionStorage.removeItem('sessionI18nCommonPackage');
    }

    //Manage nodecenteraddr
    public getNodeCenterAddr(): string {
        var resobjs = "";
        if (sessionStorage["sessionnodecenteraddr"] != null && sessionStorage["sessionnodecenteraddr"].toString() != "") {
            resobjs = sessionStorage["sessionnodecenteraddr"];
        }

        return resobjs;
    }
    public setNodeCenterAddr(str: string) {
        sessionStorage.setItem("sessionnodecenteraddr", str);
    }
    public removeNodeCenterAddr() {
        sessionStorage.removeItem('sessionnodecenteraddr');
    }

    //Manage useservices
    public getUseServices(): string {
        var resobjs = "";
        if (sessionStorage["sessionuseservices"] != null && sessionStorage["sessionuseservices"].toString() != "") {
            resobjs = sessionStorage["sessionuseservices"];
        }

        return resobjs;
    }
    public setUseServices(str: string) {
        sessionStorage.setItem("sessionuseservices", str);
    }
    public removeUseServices() {
        sessionStorage.removeItem('sessionuseservices');
    }

    //Manage biznodeaddr
    public getBizNodeAddr(): string {
        var resobjs = "";
        if (sessionStorage["sessionbiznodeaddr"] != null && sessionStorage["sessionbiznodeaddr"].toString() != "") {
            resobjs = sessionStorage["sessionbiznodeaddr"];
        }

        return resobjs;
    }
    public setBizNodeAddr(str: string) {
        sessionStorage.setItem("sessionbiznodeaddr", str);
    }
    public removeBizNodeAddr() {
        sessionStorage.removeItem('sessionbiznodeaddr');
    }

    //Manage bizmenu
    public sessionMgt_bizMenu(pwd: string, actionflag: string): any {
        var resobjs: any;
        if (actionflag == "R") {
            if (localStorage.getItem("sessionbizmenu") != null && localStorage.getItem("sessionbizmenu").toString() != "") {
                resobjs = localStorage.getItem("sessionbizmenu");
            }
        }
        else if (actionflag == "S") {
            localStorage.setItem("sessionbizmenu", pwd);
        }
        else if (actionflag == "D") {
            localStorage.removeItem('sessionbizmenu');
        }
        return resobjs;
    }

     //整体封装session管理
     public sessionMgt(sessionName:string, sessionValue: string, actionflag: string): any {
        let resobjs: any;
        let sessionNames:string = "session" + sessionName;
        if (actionflag == "R") {
            if (localStorage.getItem(sessionNames) != null && localStorage.getItem(sessionNames).toString() != "") {
                resobjs = localStorage.getItem(sessionNames);
            }
        }
        else if (actionflag == "S") {
            localStorage.setItem(sessionNames, sessionValue);
        }
        else if (actionflag == "D") {
            localStorage.removeItem(sessionNames);
        }
        return resobjs;
    }

    //end  

   

    //http encapsulation
    public getHttpInfoData(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {
        if (reqdata.Reqmethod == "") {
            reqdata.Reqmethod = "get";
        }
        if (reqdata.ReqdataType == "") {
            reqdata.ReqdataType = "application/json";
        }
        let dataTypeStr = 'json';
        let currReqAddr: string = this.getCurrServAddr(reqdata);
        let selecttoken: string = this.getToken();
        
        this.sessionMgt_onlineTime(new Date().toJSON(), "S"); //设置当前交互时间

        //let headers = new Headers({ 'Content-Type': reqdata.ReqdataType, dataType: dataTypeStr, 'Authorization': 'Basic ' + btoa("" + ":" + selecttoken) });
        let headers = new Headers({ 'Content-Type': reqdata.ReqdataType, 'token': selecttoken });
        let options = new RequestOptions({ headers: headers });

        let bizparams: any = { 'req': JSON.stringify(reqdata) };

        if (reqdata.Reqmethod == "post") {
            this.http.post(currReqAddr, bizparams, options)
                .map(resdataFunc)
                .catch(err => this.getHandlerError(err, errorFunc))
                .subscribe(displayresdataFunc)
        }
        else {
            this.http.get(currReqAddr, options)
                .map(resdataFunc)
                .catch(err => this.getHandlerError(err, errorFunc))
                .subscribe(displayresdataFunc)
        }
    }
    //http encapsulation with token
    public getHttpInfoDataAuth(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {
        if (reqdata.Reqmethod == "") {
            reqdata.Reqmethod = "get";
        }
        if (reqdata.ReqdataType == "") {
            reqdata.ReqdataType = "application/json";
        }
        let dataTypeStr = 'json';
        let currReqAddr: string = this.getCurrServAddr(reqdata);
        let selecttoken: string = this.getToken();

        this.sessionMgt_onlineTime(new Date().toJSON(), "S"); //设置当前交互时间

        //let headers = new Headers({ 'Content-Type': reqdata.ReqdataType, dataType: dataTypeStr, 'Authorization': 'Basic ' + btoa("" + ":" + selecttoken) });
        let headers = new Headers({ 'Content-Type': reqdata.ReqdataType, 'token': selecttoken });
        let options = new RequestOptions({ headers: headers });

        let bizparams: any = { 'req': JSON.stringify(reqdata) };

        if (reqdata.Reqmethod == "post") {
            this.http.post(currReqAddr, bizparams, options)
                .map(resdataFunc)
                .catch(err => this.getHandlerError(err, errorFunc))
                .subscribe(displayresdataFunc)
        }
        else {
            this.http.get(currReqAddr, options)
                .map(resdataFunc)
                .catch(err => this.getHandlerError(err, errorFunc))
                .subscribe(displayresdataFunc)
        }
    }


    //http encapsulation, use delayed(单位毫秒)     
    public getHttpInfoDataWithDelayed(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any, delayedCnt: number): void {
        if (reqdata.Reqmethod == "") {
            reqdata.Reqmethod = "get";
        }
        if (reqdata.ReqdataType == "") {
            reqdata.ReqdataType = "application/json";
        }
        let dataTypeStr = 'json';
        let currReqAddr: string = this.getCurrServAddr(reqdata);
        let selecttoken: string = this.getToken();

        this.sessionMgt_onlineTime(new Date().toJSON(), "S"); //设置当前交互时间

        //let headers = new Headers({ 'Content-Type': reqdata.ReqdataType, dataType: dataTypeStr, 'Authorization': 'Basic ' + btoa("" + ":" + selecttoken) });
        let headers = new Headers({ 'Content-Type': reqdata.ReqdataType, 'token': selecttoken });
        let options = new RequestOptions({ headers: headers });

        let bizparams: any = { 'req': JSON.stringify(reqdata) };

        if (reqdata.Reqmethod == "post") {
            if (delayedCnt > 0) {
                this.http.post(currReqAddr, bizparams, options)
                    .map(resdataFunc)
                    .catch(err => this.getHandlerError(err, errorFunc))
                    .debounceTime(delayedCnt)
                    .subscribe(displayresdataFunc)
            }
            else {
                this.http.post(currReqAddr, bizparams, options)
                    .map(resdataFunc)
                    .catch(err => this.getHandlerError(err, errorFunc))
                    .debounceTime(delayedCnt)
                    .subscribe(displayresdataFunc)
            }
        }
        else {
            if (delayedCnt > 0) {
                this.http.get(currReqAddr, options)
                    .map(resdataFunc)
                    .catch(err => this.getHandlerError(err, errorFunc))
                    .debounceTime(delayedCnt)
                    .subscribe(displayresdataFunc)
            }
            else {
                this.http.get(currReqAddr, options)
                    .map(resdataFunc)
                    .catch(err => this.getHandlerError(err, errorFunc))
                    .debounceTime(delayedCnt)
                    .subscribe(displayresdataFunc)
            }
        }
    }

    //http encapsulation with token, use delayed(单位毫秒)     
    public getHttpInfoDataAuthWithDelayed(reqdata: RequestParams, resdataFunc: any, displayresdataFunc: any, errorFunc: any, delayedCnt: number): void {
        if (reqdata.Reqmethod == "") {
            reqdata.Reqmethod = "get";
        }
        if (reqdata.ReqdataType == "") {
            reqdata.ReqdataType = "application/json";
        }
        let dataTypeStr = 'json';
        let currReqAddr: string = this.getCurrServAddr(reqdata);
        let selecttoken: string = this.getToken();

        this.sessionMgt_onlineTime(new Date().toJSON(), "S"); //设置当前交互时间

        //let headers = new Headers({ 'Content-Type': reqdata.ReqdataType, dataType: dataTypeStr, 'Authorization': 'Basic ' + btoa("" + ":" + selecttoken) });
        let headers = new Headers({ 'Content-Type': reqdata.ReqdataType, 'token': selecttoken });
        let options = new RequestOptions({ headers: headers });

        let bizparams: any = { 'req': JSON.stringify(reqdata) };

        if (reqdata.Reqmethod == "post") {
            if (delayedCnt > 0) {
                this.http.post(currReqAddr, bizparams, options)
                    .map(resdataFunc)
                    .catch(err => this.getHandlerError(err, errorFunc))
                    .debounceTime(delayedCnt)
                    .subscribe(displayresdataFunc)
            }
            else {
                this.http.post(currReqAddr, bizparams, options)
                    .map(resdataFunc)
                    .catch(err => this.getHandlerError(err, errorFunc))
                    .debounceTime(delayedCnt)
                    .subscribe(displayresdataFunc)
            }

        }
        else {
            if (delayedCnt > 0) {
                this.http.get(currReqAddr, options)
                    .map(resdataFunc)
                    .catch(err => this.getHandlerError(err, errorFunc))
                    .debounceTime(delayedCnt)
                    .subscribe(displayresdataFunc)
            }
            else {
                this.http.get(currReqAddr, options)
                    .map(resdataFunc)
                    .catch(err => this.getHandlerError(err, errorFunc))
                    .debounceTime(delayedCnt)
                    .subscribe(displayresdataFunc)
            }
        }
    }

    //http excaption handler
    public getHandlerErrorBak(error: Response | any) {
        let errMsg: string;
        if (error instanceof Response) {
            let body = error.json() || '';
            let err = body.error || JSON.stringify(body);
            errMsg = err;
        } else {
            errMsg = error.message ? error.message : error.toString();
        }
        alert(errMsg);
        return Observable.throw(errMsg);
    }

    public getHandlerError(error: Response | any, userErrFunc: any) {
        let errMsg: string;
        if (error instanceof Response) {
            let body = error.json() || '';
            let err = body.error || JSON.stringify(body);
            errMsg = err;
        } else {
            errMsg = error.message ? error.message : error.toString();
        }
        userErrFunc(errMsg);
        return Observable.throw(errMsg);
    }

    //获取当前服务
    public getCurrentServ(serviden: string, versioniden: string, servtype: string, servconfig: any): any {
        let reqServ = {}; //返回当前服务 
        if (servconfig != "" && servconfig != undefined) {
            let servconfigArrr: any = JSON.parse(servconfig);
            if (servconfigArrr.length > 0) {
                for (let i = 0; i < servconfigArrr.length; i++) {
                    if (servconfigArrr[i]["serviden"] == serviden && servconfigArrr[i]["version"] == versioniden) {
                        reqServ = servconfigArrr[i];
                        break;
                    }
                }
            }
        }

        return reqServ;
    }

    //获取当前请求地址
    //reqdata.Addrtype参数值为nodecenter(节点中心地址)、xxx(业务节点地址)、terminal(终端网页地址)
    public getCurrServAddr(reqdata: RequestParams): string {
        let resServAddr: string = "";
        let servconfigmgt: any = this.getCurrentServ(reqdata.ServIdentifer, reqdata.ServVersionIden, reqdata.ServVersionIden, this.getUseServices());
        if (servconfigmgt) {
            if (servconfigmgt.url_addr != "" && servconfigmgt.url_addr != undefined) {
                resServAddr = servconfigmgt.url_addr + "/" + servconfigmgt.servcodename + reqdata.GetDataUrl;
            }
            else {
                if (reqdata.AddrType == "nodecenter") {
                    //TODO  预留自动监控节点中心状态，变更可用节点中心地址
                    let nodecenterobj: any = JSON.parse(this.getNodeCenterAddr());
                    let nodecenteraddress: string = "";
                    if (nodecenterobj.master_node_state == "1") {
                        nodecenteraddress = nodecenterobj.master_node;
                    }
                    else if (nodecenterobj.slave_node_state == "1") {
                        nodecenteraddress = nodecenterobj.slave_node;
                    }
                    resServAddr = nodecenteraddress + "/" + servconfigmgt.servcodename + reqdata.GetDataUrl;
                    //resServAddr = "/api/" + servconfigmgt.servcodename + reqdata.GetDataUrl; 
                }                
                else if (reqdata.AddrType == "terminal") {
                    resServAddr = reqdata.GetDataUrl;
                }
                else if (reqdata.AddrType == "PlcService") {
                    resServAddr = reqdata.Reqtype + reqdata.GetDataUrl;
                }
                else {
                    //获取请求模块当前可用节点地址
                    let bizNodeList:any[] = JSON.parse(this.getBizNodeAddr());
                    let moudineAdd:string = "";
                    for (let i = 0; i < bizNodeList.length; i++) {
                        if (bizNodeList[i].Moudiden == reqdata.AddrType) {
                            moudineAdd = bizNodeList[i].Url_addr;
                            break;
                        }
                    }
                    resServAddr = moudineAdd + "/" + servconfigmgt.servcodename + reqdata.GetDataUrl;
                }
            }
        }

        return resServAddr;
    }

     //http encapsulation webapi请求
     public getHttpInfoDataWebapi(reqdata: Reqobj, resdataFunc: any, displayresdataFunc: any, errorFunc: any): void {
        if (reqdata.reqmethod == "") {
            reqdata.reqmethod = "get";
        }
        if (reqdata.reqdatatype == "") {
            reqdata.reqdatatype = "application/json; charset=utf-8"; 
            //reqdata.reqdatatype = "application/x-www-form-urlencoded;charset=UTF-8";
        }
        let dataTypeStr = 'json';

        let selecttoken: string = this.getToken();
        this.sessionMgt_onlineTime(new Date().toJSON(), "S"); //设置当前交互时间

        let headers = new Headers();
        headers.append('Content-Type', reqdata.reqdatatype);  
        let options = new RequestOptions({ headers: headers});

        //仅仅用于封装mir接口 JSON.parse(reqdata.reqdatas)
        if (reqdata.reqmethod == "post") {
            this.http.post(reqdata.dataUrl,  reqdata, options)
                .map(resdataFunc)
                .catch(err => this.getHandlerError(err, errorFunc))
                .subscribe(displayresdataFunc)
        }
        else {
            this.http.get(reqdata.dataUrl, options)
                .map(resdataFunc)
                .catch(err => this.getHandlerError(err, errorFunc))
                .subscribe(displayresdataFunc)
        }
    }

    //计算时间差,resFlag表示时间差的类型 d 相差天数  h 相差小时数 m 相差分钟数 s 相差秒数, ms 相差秒数,其他后续再扩展 
    public dateTimeSeg(startTimeStr: string, endTimeStr: string, resFlag: string): number {
        let difValue: number = 0; //相差的数值

        let startTime: Date = new Date(startTimeStr);
        let endTime: Date = new Date(endTimeStr);

        let startDateMS: number = startTime.getTime(); //到日期的毫秒数
        let endDateMS: number = endTime.getTime(); //到日期的毫秒数
        //相差总毫秒数
        let tottleDiffMS: number = endDateMS - startDateMS;

        if (resFlag == "d") {

            difValue = Math.floor(tottleDiffMS / (24 * 3600 * 1000));

        }
        else if (resFlag == "h") {

            difValue = Math.floor(tottleDiffMS / (3600 * 1000));

        }
        else if (resFlag == "m") {

            difValue = Math.floor(tottleDiffMS / (60 * 1000));

        }
        else if (resFlag == "s") {

            difValue = Math.floor(tottleDiffMS / (1000));

        }
        else if (resFlag == "ms") {

            difValue = tottleDiffMS;

        }

        return difValue;

    }

    //框架环境,放到令牌中，令牌不能含有中文
    public frameEnv: any = {
        SysUserDict: {
            EMAIL: '', FROMPLAT: '', ISFIRSTLOGIN: '', ISFLAG: '', ISLONIN: '', ISUSE: '', LASTLOGINTIME: '', NOTE: '', OPFlag: '', PASSWORD: '',
            REGISTERDATE: '', TELEPHONE: '', TIMESTAMPSS: '', USERID: '', USERNAME: ''
        }, TokenEncrpPublicKey: '', TokenEncrpType: '', TokenEncrpValue: '', Ips: '', BizNodeAddr: '', I18nCurrLang: '',
        distManagerParam: {
            DistributeActionIden: '0', DistributeDataNodes: [{
                DBFactoryName: '', Data_conn: '', Data_password: '', Data_schema: '', Data_user: '',
                Encryhashlenth: '', Encrykeystr: '', ID: '', Isconfigdb: '', Isencrydbconn: '', Isencrypwd: '', Isusepwdsecuritycheck: '', Pwdfirstcheck: '',
                Pwdintervalhours: '', Remarks: '', Securitycode: '', Systemname: '', Timestampss: '', Url_addr: '', Use_status: ''
            }],
            DistributeDataNode: { DbFactoryName: '', Connectionstring: '', DbSchema: '' }, DistriActionSqlParams: []
        }
    }

    public commonI18nlang: any = {}; //通用语言包
    public frameI18nlang: any = {};//框架相关语言包

}

//Public structure definition

//Set the range of values for comparison data
export class MainValueName {
    public PropName: string = "";
}

//Frame paging parameters
export class PagingParam {
    public TotalSize: number = 0;
    public PageIndex: number = 1;
    public PageSize: number = 10;
    public SortType: string = "1"; //SortType Asc = 1, Desc = 2 , defualt value = "1"
}

//request parameters encapsulation
export class RequestParams {
    public GetDataUrl: string = "";
    public Reqtype: string = "";
    public Reqmethod: string = "";
    public ReqdataType: string = "";
    public Reqdata: string = "";
    public Reqi18n: string = "";
    public ServIdentifer: string = "";
    public ServVersionIden: string = "";
    //AddrType 参数值为nodecenter(节点中心地址)、biznode(业务节点地址)、terminal(终端网页地址)
    public AddrType: string = "";
}

//response parameters encapsulation
export class ResponseParams {
    public respflag: string = "";
    public resptoolstr: string = "";
    public respdata: string = "";
    public pagertottlecounts: string = "";
}


//request parameters encapsulation webapi
export class Reqobj {
    public dataUrl:string = "";
    public reqtype: string = "";
    public reqmethod: string = "";
    public reqdatatype: string = "";
    public sn: string = "";
    public output: string = "";
    public reqiden: string = "";
    public reqdataiden: string = "";
    public reqdatas: string = "";
    public reqi18n: string = "";
    public reqversion: string = "";

    constructor(_dataUrl:string,  _reqtype:string, _reqmethod:string, _reqdatatype:string,
         _sn: string, _output: string, _reqdataiden: string, _reqdatas: string, _reqi18n: string,
         _reqiden: string, _reqversion: string) {
        this.dataUrl = _dataUrl;
        this.reqtype = _reqtype;
        this.reqmethod = _reqmethod;
        this.reqdatatype = _reqdatatype;
        this.sn = _sn;
        this.output = _output;
        this.reqdataiden = _reqdataiden;
        this.reqdatas = _reqdatas;
        this.reqi18n = _reqi18n;        
        this.reqiden = _reqiden;
        this.reqversion = _reqversion;
    }
}
export class Reqdata {
    public key: string = "";
    public value: string = "";
}
export class response {
    public status: string = "";
    public datetime: string = "";
    public respdatas: string = "";
    public tottlerecords: string = "";
    public resperrordatas: string = "";
}

//检查数据库参数
export class CheckBizObjectRepatParams {
    public Bizobjectname: string = "";
    public WherePropertyL: any[] = [];
    public Splitchar: string = "";
}

//键值对构造器
export class TextValue {
  constructor(public text: string, public value: string) { }
}



//环境参数
export class EnvData {
    public beforeUrl = ''; //api prefix address
    public sessionStorageIdenPrefix = ""; // 会话名称识别前缀，    
}

//页面跳转动画
export const pageAnimation = trigger('pageAnimation', [
    state('in', style({ opacity: 1, transform: 'translateY(0)' })),
    transition('void => *', [
        style({
            opacity: 1,
            transform: 'translateY(40px)'
        }),
        animate('400ms  ease-out')
    ]),
    transition('* => void', [
        animate('400ms  ease-out', style({
            opacity: 0,
            transform: 'translateY(40px)'
        }))
    ])
]);
//rotateY 90 度动画
export const tagAnimation = trigger('tagAnimation', [
    state('inactive', style({ transform: 'rotateY(0deg)' })),
    state('active', style({ transform: 'rotateY(90deg)' })),
    transition('active => inactive', animate('300ms ease-out'))
]);
//right25度动画
export const right25Animation = trigger('right25Animation', [
    state('inactive', style({ transform: 'translateX(0)' })),
    state('active', style({ transform: 'translateX(-25px)' })),
    transition('inactive <=> active', animate('400ms ease-out'))
]);
//left25度动画
export const left25Animation = trigger('left25Animation', [
    state('inactive', style({ transform: 'translateX(0)' })),
    transition('void => *', [
        style({
            transform: 'translateX(25px)'
        }),
        animate('500ms 200ms ease-out')
    ]),
    transition('* => void', [
        animate('500ms  200ms ease-out', style({
            transform: 'translateX(25px)'
        }))
    ])
])


