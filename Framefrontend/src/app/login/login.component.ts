import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, ActivatedRouteSnapshot, RouterState, RouterStateSnapshot } from '@angular/router';
import { Base64 } from 'js-base64';
//import NProgress from 'nprogress';

import { Message } from 'primeng/primeng';

import { CommonRootService, MainValueName, PagingParam, RequestParams, ResponseParams } from '../common_module/common.service';

import { LoginService } from './login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  nameModel: any;
  psModel: any;
  btnLogin: string = 'Login';
  loginerrortext: string = '';
  userToken: string;
  realname: string;
  condiv: boolean = false;
  autofocus: string = "autofocus";
  setLang: string = "";
  p_Username: string = "";
  p_Systemname: string = "";
  p_Password: string = "";
  loginerrortextArr: Message[] = [];
  consticky: boolean = false;
  remUsernameLabel: string = "";
  remUsername: boolean = false;
  remPasswordLabel: string = "";
  remPassword: boolean = false;
  noticeInfoTitle: string = "";
  enableLogin: boolean = false;


  commonI18nlang: any; //通用语言包
  frameI18nlang: any;//框架相关语言包
  nodecenteraddr: string = ""; //当前可用节点中心地址
  useservices: any; //当前应用服务配置
  biznodeaddr: any; //可用业务节点
  hasI18nFrameLang: boolean = false;
  hasCommonI18nlang: boolean = false;
  hasNodecenteraddr: boolean = false;
  hasUseservices: boolean = false;
  hasBiznodeaddress: boolean = false;

  delaySecond:number = 0; //登录告警提示延迟毫秒数
  delayCnt:any = 0;

  //框架环境变量
  frameEnv: any = {
    SysUserDict: {
      EMAIL: '', FROMPLAT: '', ISFIRSTLOGIN: '', ISFLAG: '', ISLONIN: '', ISUSE: '', LASTLOGINTIME: '', NOTE: '', OPFlag: '', PASSWORD: '',
      REGISTERDATE: '', TELEPHONE: '', TIMESTAMPSS: '', USERID: '', USERNAME: ''
    }, 
    TokenEncrpPublicKey: '', TokenEncrpType: '', TokenEncrpValue: '', Ips: '', BizNodeAddr: '', I18nCurrLang: '',
    distManagerParam: {
      DistributeActionIden: '0', DistributeDataNodes: [{
        DBFactoryName: '', Data_conn: '', Data_password: '', Data_schema: '', Data_user: '',
        Encryhashlenth: '', Encrykeystr: '', ID: '', Isconfigdb: '', Isencrydbconn: '', Isencrypwd: '', Isusepwdsecuritycheck: '', Pwdfirstcheck: '',
        Pwdintervalhours: '', Remarks: '', Securitycode: '', Systemname: '', Timestampss: '', Url_addr: '', Use_status: ''
      }],
      DistributeDataNode: { DbFactoryName: '', Connectionstring: '', DbSchema: '' }, DistriActionSqlParams: []
    }
  }

  constructor(public router: Router, private myService: LoginService, public commonmodule: CommonRootService
  ) {

  }

  ngOnInit() {

    //NProgress.done();
    this.setLang = "zh-CN";
    this.commonmodule.setI18nlang(this.setLang);

    this.frameEnv.I18nCurrLang = this.setLang;
    this.commonmodule.setToken(JSON.stringify(this.frameEnv));

    if (this.commonmodule.sessionMgt_remUsername("", "R")) {
      this.nameModel = this.commonmodule.sessionMgt_remUsername("", "R");
      this.remUsername = true;
    }

    if (this.commonmodule.sessionMgt_remPassword("", "R")) {
      this.psModel = this.commonmodule.sessionMgt_remPassword("", "R");
      this.remPassword = true;
    }   


    let reqdata: RequestParams = new RequestParams();
    reqdata.AddrType = "terminal";
    reqdata.GetDataUrl = "assets/i18nlang/i18nframe_" + this.setLang + ".json";
    reqdata.Reqmethod = "get";
    reqdata.Reqdata = "";
    reqdata.ReqdataType = "";
    reqdata.Reqtype = "";
    reqdata.Reqi18n = this.setLang;
    reqdata.ServIdentifer = "";
    reqdata.ServVersionIden = "";

    this.myService.getI18nFrameLang(reqdata,
      res => this.getFramei18nDataResdataFunc(res), res => this.getFramei18nDataDisplaydataFunc(res), err => this.getFramei18nDataErrorFunc(err));

    reqdata.GetDataUrl = "assets/i18nlang/i18ncommon_" + this.setLang + ".json";

    this.myService.getI18nCommonLang(reqdata,
      res => this.getCommoni18nDataResdataFunc(res), res => this.getCommoni18nDataDisplaydataFunc(res), err => this.getCommoni18nDataErrorFunc(err));

    reqdata.GetDataUrl = "assets/data/nodecenteraddr.json";
    this.myService.getNodeCenterAddr(reqdata,
      res => this.getNodeCenterAddressResdataFunc(res), res => this.getNodeCenterAddressDisplaydataFunc(res), err => this.getNodeCenterAddressErrorFunc(err));

    reqdata.GetDataUrl = "assets/data/useservices.json";
    this.myService.getUseServices(reqdata,
      res => this.getBizNodeServicesResdataFunc(res), res => this.getBizNodeServicesDisplaydataFunc(res), err => this.getBizNodeServicesErrorFunc(err));

    //定时加载获取业务节点，直到取得为止
    setInterval(() => {
      if(!this.hasBiznodeaddress){
        this.getBizNodeAddr();
      }
    }, 10);

  }  

  //控制登录按钮
  conLogin(): void {
    this.enableLogin = this.hasI18nFrameLang && this.hasCommonI18nlang && this.hasNodecenteraddr && this.hasUseservices && this.hasBiznodeaddress;
  }


  //加载i18nframelang 
  getFramei18nDataResdataFunc(data: any): any {
    return data;
  }
  getFramei18nDataDisplaydataFunc(data: any): void {
    if (data.status == 200) {
      this.frameI18nlang = JSON.parse(data._body);
      this.commonmodule.setFrameI18nlang(data._body);

      this.commonmodule.frameI18nlang = this.frameI18nlang;

      this.displayI18nFrameLang();
      this.hasI18nFrameLang = true;
      this.conLogin();

    }
  }

  getFramei18nDataErrorFunc(err: string) {
  }


  //加载i18ncommonlang
  getCommoni18nDataResdataFunc(data: any): any {
    return data;
  }
  getCommoni18nDataDisplaydataFunc(data: any): void {
    if (data.status == 200) {
      this.commonI18nlang = JSON.parse(data._body);
      this.commonmodule.setCommonI18nlang(data._body);

      this.commonmodule.commonI18nlang = this.commonI18nlang;

      this.displayI18nCommonLang();
      this.hasCommonI18nlang = true;
      this.conLogin();
    }
  }

  getCommoni18nDataErrorFunc(err: string) {
  }

  //加载节点中心地址
  getNodeCenterAddressResdataFunc(data: any): any {
    return data;
  }
  getNodeCenterAddressDisplaydataFunc(data: any): void {
    if (data.status == 200) {
      this.nodecenteraddr = JSON.parse(data._body);
      this.commonmodule.setNodeCenterAddr(data._body);
      this.hasNodecenteraddr = true;
      this.conLogin();
    }

  }

  getNodeCenterAddressErrorFunc(err: string) {
  }

  //加载业务节点服务配置
  getBizNodeServicesResdataFunc(data: any): any {
    return data;
  }
  getBizNodeServicesDisplaydataFunc(data: any): void {
    if (data.status == 200) {
      this.useservices = JSON.parse(data._body);
      this.commonmodule.setUseServices(data._body);
      this.hasUseservices = true;
      this.conLogin();
    }
  }

  getBizNodeServicesErrorFunc(err: string) {
  }


  //获取业务节点
  getBizNodeAddr(): void {

    let reqdata: RequestParams = new RequestParams();
    let currreqobj = { params: "" };
    reqdata.AddrType = "nodecenter";
    reqdata.GetDataUrl = "/FrameNodeApi/GetFrameParams";
    reqdata.Reqmethod = "post";
    reqdata.Reqdata = JSON.stringify(currreqobj);
    reqdata.ReqdataType = "";
    reqdata.Reqtype = "";
    reqdata.Reqi18n = this.setLang;
    reqdata.ServIdentifer = "framenodesecu";
    reqdata.ServVersionIden = "1.0";

    this.myService.getBizNodeAddr(reqdata,
      res => this.getBizNodeAddrResdataFunc(res), res => this.getBizNodeAddrDisplaydataFunc(res), err => this.getBizNodeAddrErrorFunc(err));
  }

  //加载业务节点配置
  getBizNodeAddrResdataFunc(data: any): any {
    return data;
  }
  getBizNodeAddrDisplaydataFunc(data: any): void {
    if (data.status == 200) {
      let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
      if (respdata.respflag == "1") {
        this.frameEnv = JSON.parse(respdata.respdata);       

        this.biznodeaddr = this.frameEnv.BizNodeAddr;
        this.commonmodule.setBizNodeAddr(this.biznodeaddr);

        this.frameEnv.BizNodeAddr = ""; //模块化
        this.commonmodule.setToken(JSON.stringify(this.frameEnv));        

        this.hasBiznodeaddress = true;
        this.conLogin();
      }
    }
  }


  getBizNodeAddrErrorFunc(err: string) {
  }


  //显示框架语言包信息
  displayI18nFrameLang(): void {
    this.p_Systemname = this.frameI18nlang.SystemName;
    this.btnLogin = this.frameI18nlang.frmLoginAction;
    this.p_Username = this.frameI18nlang.frmLoginUserName;
    this.p_Password = this.frameI18nlang.frmLoginUserPassword;
    this.remUsernameLabel = this.frameI18nlang.frmLoginremUsernameLabel;
    this.remPasswordLabel = this.frameI18nlang.frmLoginremPasswordLabel;
    document.title = this.frameI18nlang.SystemName;
  }

  //显示通用语言包信息
  displayI18nCommonLang(): void {
    this.noticeInfoTitle = this.commonI18nlang.noticeInfoTitle;
  }


  login() {
    this.condiv = true;
    this.btnLogin = this.frameI18nlang.frmLoginActionIng;
    if (!this.nameModel || !this.psModel) {
      this.loginerrortext = this.frameI18nlang.frmLoginInputUsername2Password;
      this.loginerrortextArr = [];
      this.loginerrortextArr.push({ severity: 'warn', summary: this.noticeInfoTitle, detail: this.loginerrortext });
    } else {
      //加密口令
      let enpwd: string = this.psModel;
      if (this.nameModel != "super") {
        let nodecenterobj: any = JSON.parse(this.commonmodule.getNodeCenterAddr());
        let key = nodecenterobj.keypwd;
        let iv = nodecenterobj.ivpwd;
        enpwd = this.commonmodule.encrypt(this.psModel, key, iv);
      } 
      this.userToken = `Basic ${Base64.encode(this.nameModel + ':' + this.psModel)}`;
      let reqdata: RequestParams = new RequestParams();
      let curruser = { userid: this.nameModel, password: enpwd };
      
      reqdata.AddrType = "FrameMgt";
      reqdata.GetDataUrl = "/FrameApi/GetUserForLoginN";
      reqdata.Reqmethod = "post";
      reqdata.Reqdata = JSON.stringify(curruser);
      reqdata.ReqdataType = "";
      reqdata.Reqi18n = this.setLang;
      reqdata.Reqtype = "login";
      reqdata.Reqi18n = this.setLang;
      reqdata.ServIdentifer = "framesecurity";
      reqdata.ServVersionIden = "1.0";
      
      this.myService.login(this.userToken, reqdata,
        res => this.loginresdataFunc(res), res => this.logindisplayresdataFunc(res), err => this.loginerrorFunc(err));
    }
  }

  loginresdataFunc(data: any): any {
    return data;
  }
  
  logindisplayresdataFunc(data: any): void {
    this.condiv = false;
    if (data.status == 200) {
      let respdata: ResponseParams = JSON.parse(JSON.parse(data._body));
      if (respdata.respflag == "1" || respdata.respflag == "2") {
        let frameEnvTmp:any = JSON.parse(respdata.respdata);

        this.commonmodule.sessionMgt_frameAllEnv(respdata.respdata, "S"); //保存全部用户信息

        this.frameEnv.TokenEncrpValue = frameEnvTmp.TokenEncrpValue; //令牌          
        this.frameEnv.SysUserDict.USERID = this.nameModel;
        this.frameEnv.SysUserDict.PASSWORD = this.psModel;

        this.commonmodule.setToken(JSON.stringify(this.frameEnv)); //保存框架环境
        this.commonmodule.frameEnv = this.frameEnv; //服务化框架环境
       
        this.realname = this.nameModel;
        if (this.remUsername) {
          this.commonmodule.sessionMgt_remUsername(this.nameModel, "S");
        }
        else {
          this.commonmodule.sessionMgt_remUsername("", "D");
        }

        if (this.remPassword) {
          this.commonmodule.sessionMgt_remPassword(this.psModel, "S");
        }
        else {
          this.commonmodule.sessionMgt_remPassword("", "D");
        }       

        if (respdata.respflag == "1") {
          //直接进入系统
          this.router.navigateByUrl('workbench');
        }
        else {
          //告警后进入系统
          this.delayCnt = setInterval(() => {
            if (this.delaySecond <= 5) {
              this.delaySecondEntrySystem(respdata.resptoolstr);
            }
          }, 1000);        

        }
      }
      else {        
        this.loginerrortext = respdata.resptoolstr;
        this.loginerrortextArr = [];
        this.loginerrortextArr.push({ severity: 'warn', summary: this.noticeInfoTitle, detail: this.loginerrortext });
      }
    } else if (data.status == 401) {
      this.loginerrortext = this.frameI18nlang.frmLoginErrorUsernameOrPassword;
      this.loginerrortextArr = [];
      this.loginerrortextArr.push({ severity: 'warn', summary: this.noticeInfoTitle, detail: this.loginerrortext });
    } else {
      this.loginerrortext = this.frameI18nlang.frmLoginServerBusy;
      this.loginerrortextArr = [];
      this.loginerrortextArr.push({ severity: 'warn', summary: this.noticeInfoTitle, detail: this.loginerrortext });
    }
  }

  loginerrorFunc(err: string) {
    this.condiv = false;
    this.loginerrortext = this.commonI18nlang.noticeHttpError_info + err;
    this.loginerrortextArr = [];
    this.loginerrortextArr.push({ severity: 'warn', summary: this.noticeInfoTitle, detail: this.loginerrortext });
  }

  //判断延时毫秒数据
  delaySecondEntrySystem(toolStr: string): void {
    if (this.delaySecond < 5) {
      this.delaySecond = this.delaySecond + 1;
      if (this.delaySecond == 1) {
        this.loginerrortext = toolStr;
        this.loginerrortextArr = [];
        this.loginerrortextArr.push({ severity: 'warn', summary: this.noticeInfoTitle, detail: this.loginerrortext });
      }
    }
    else {
      let tmp:any = clearInterval(this.delayCnt);
      this.router.navigateByUrl('workbench');
    }
  }



  /************************* get munus ********************************/
  getMenu() {
    let reqdata: RequestParams = new RequestParams();
    reqdata.AddrType = "terminal";
    reqdata.GetDataUrl = "assets/data/user-menu.json";
    reqdata.Reqmethod = "get";
    reqdata.Reqdata = "";
    reqdata.ReqdataType = "";
    reqdata.Reqtype = "";
    reqdata.Reqi18n = "en-GB";
    reqdata.Reqi18n = this.setLang;
    reqdata.ServIdentifer = "";
    reqdata.ServVersionIden = "";

    this.myService.getMenu(reqdata, res => this.getMenuresdataFunc(res), res => this.getMenudisplayresdataFunc(res),
      err => this.getMenuerrorFunc(err));
  }

  getMenuresdataFunc(data: any): any {
    return data;
  }
  getMenudisplayresdataFunc(data: any): void {
    if (data.status == 200) {
      this.commonmodule.setUserMenu(data._body);
    }
  }
  getMenuerrorFunc(err: string) {
    this.loginerrortext = this.commonI18nlang.noticeHttpError_info + err;
    this.loginerrortextArr = [];
    this.loginerrortextArr.push({ severity: 'warn', summary: this.noticeInfoTitle, detail: this.loginerrortext });
  }

  inputFocus() {
    this.loginerrortext = '';
    this.btnLogin = this.frameI18nlang.frmLoginAction;
    this.condiv = false;
  }
}
