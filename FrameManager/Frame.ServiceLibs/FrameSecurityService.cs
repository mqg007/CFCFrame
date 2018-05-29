using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Caching;
using System.IO;

using Frame.ServiceNodeInterfaces;
using Frame.ServiceInterfaces;
using WebServiceManager;
using Entitys.ComonEnti;
using System.Data;
using Common;
using FrameCommon;

namespace Frame.ServiceLibs
{
    public class FrameSecurityService : BaseServiceNoCheck, IFrameSecurity
    {
        #region 定义

        public BizExecFacade.CommonBizFacade _comBiz = null;

        #endregion

        public FrameSecurityService()
        {
            //这里具体服务语言包
            //string FrameSecurityi18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameSecurityi18nLang", ""), envirObj.I18nCurrLang);
            //i18nModuleCurrLang = this.GetI18nLang(FrameSecurityi18nLang);
                        
            DataTable servlangtmp = (DataTable)currCache.Get("i18nFrameSecurityi18nLang");
            if (servlangtmp != null)
            {
                if (currlang == envirObj.I18nCurrLang)
                {
                    i18nModuleCurrLang = servlangtmp;
                }
                else
                {
                    string FrameSecurityi18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameSecurityi18nLang", ""), envirObj.I18nCurrLang);                    
                    i18nModuleCurrLang = this.GetI18nLang(FrameSecurityi18nLang);
                }
            }
            else
            {
                string FrameSecurityi18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameSecurityi18nLang", ""), envirObj.I18nCurrLang);
                i18nModuleCurrLang = this.GetI18nLang(FrameSecurityi18nLang);
            }       
            
            _comBiz = new BizExecFacade.CommonBizFacade(envirObj);
        } 

        #region 升级实现

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>d
        public string GetUserForLoginN(string req)
        {
            try
            {
                //解析参数实体 
                reqdata = this.AnaRequestData(req);
                SSY_USER_DICT model = this.json.Deserialize<SSY_USER_DICT>(reqdata.reqdata);
                StringBuilder toolStr = new StringBuilder();

                //准备日志参数实例
                List<SSY_LOGENTITY> ListBizLog = new List<SSY_LOGENTITY>();

                if (model.USERID.ToString().ToUpper() == "super".ToUpper())
                {
                    base.envirObj.distManagerParam.DistributeActionIden = DistributeActionIden.Query;
                }
                else
                {
                    base.envirObj.distManagerParam.DistributeActionIden = DistributeActionIden.TransAction;
                }

                //this.permitMaxLoginFailtCnt = APPConfig.GetAPPConfig().GetConfigValue("permitMaxLoginFailtCnt", "5");  //允许最大错误登录次数, 默认5次

                //解密口令, 客户端已经加密，这里无需解密,直接比较密码串
                //string iv128str = APPConfig.GetAPPConfig().GetConfigValue("ivpwd", "5CRc851hRywf7W3m"); 
                //string key256str = APPConfig.GetAPPConfig().GetConfigValue("keypwd", "nW8FnftasWp7AVZrmgr9sdaGNXsjMWiw"); 
                //byte[] key256 = Security.CreateKeyByte(key256str);
                //byte[] iv128 = Security.CreateKeyByte(iv128str);
                //model.PASSWORD = Security.DeAES(model.PASSWORD.ToString(), key256, iv128);

                List<SSY_USER_DICT> uds = this._comBiz.GetUserForLogin(model, base.envirObj.distManagerParam, ListBizLog);

                if (uds.Count > 0)
                {
                    if (model.PASSWORD.ToString() == uds[0].PASSWORD.ToString())
                    {
                        if (uds[0].ISUSE.ToString() == "0")
                        {
                            //判断是否禁用
                            resdata = this.MakeResponseData("0", this.GetI18nLangItem("loginerr_Enabled", this.i18nModuleCurrLang), string.Empty, string.Empty);
                        }
                        else if (uds[0].LOCKED.ToString() == "1")
                        {
                            //判断是否锁定
                            resdata = this.MakeResponseData("0", this.GetI18nLangItem("loginerr_Locked", this.i18nModuleCurrLang), string.Empty, string.Empty);
                        }
                        else
                        {
                            #region 允许登录后，正常验证处理

                            //判断是否登录
                            bool alreadyLonin = false;
                            if (Utility.ObjHasData(uds[0].ISLONIN) && Utility.ObjHasData(uds[0].FROMPLAT))
                            {
                                if (uds[0].ISLONIN == "Y" && uds[0].FROMPLAT.ToUpper() == base.envirObj.distManagerParam.DistributeDataNodes[0].Systemname.ToUpper())
                                {
                                    alreadyLonin = true;
                                }
                            }
                            if (alreadyLonin)
                            {
                                //检查密码安全补存提示信息
                                //resdata = BaseWebPage.MakeResponseData("0", string.Format(BaseUI.GetNoticeCfg("com0008", "CommonNoticeCfg"), uds[0].FROMPLAT.ToUpper()), string.Empty);
                                resdata = this.MakeResponseData("0", this.GetI18nLangItem("loginok_exist", this.i18nModuleCurrLang), string.Empty, string.Empty);
                            }
                            else
                            {
                                //检查是否启用密码安全策略 检查首次登陆  检查超过时间间隔
                                if (base.envirObj.distManagerParam.DistributeDataNodes[0].Isusepwdsecuritycheck == "Y")
                                {
                                    if (base.envirObj.distManagerParam.DistributeDataNodes[0].Pwdfirstcheck == "Y" && uds[0].ISFIRSTLOGIN == "Y")
                                    {
                                        //首次登陆提示
                                        toolStr.AppendLine(this.GetI18nLangItem("loginok_firstlogin", this.i18nModuleCurrLang));
                                    }

                                    //这里取服务器时间即可
                                    //Frame.ServiceLibs.FrameManagerService tmpop = new FrameManagerService();
                                    //string currTime = string.Empty;
                                    //currTime =  tmpop.GetSystemDateTimesN(string.Empty);  

                                    string currTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    if ((!string.IsNullOrEmpty(currTime)) && (!string.IsNullOrEmpty(uds[0].LASTLOGINTIME.ToString())))
                                    {
                                        TimeSpan ts = Convert.ToDateTime(currTime) - Convert.ToDateTime(uds[0].LASTLOGINTIME.ToString());
                                        if (ts.TotalHours >= int.Parse(base.envirObj.distManagerParam.DistributeDataNodes[0].Pwdintervalhours))
                                        {
                                            //超过时间提示
                                            toolStr.AppendLine(string.Format(this.GetI18nLangItem("loginok_oversecuritytime", this.i18nModuleCurrLang),
                                                base.envirObj.distManagerParam.DistributeDataNodes[0].Pwdintervalhours));
                                        }
                                    }
                                }

                                #region 获取令牌

                                //节点中心安全服务
                                //string FrameNodeSecurity = APPConfig.GetAPPConfig().GetConfigValue("NodeCenterMaster", "") +
                                //    APPConfig.GetAPPConfig().GetConfigValue(SSY_ServiceHost.FrameNodeSecurityService, "").TrimStart('/');

                                //调用普通节点中心服务获取令牌
                                DataRow drServ = this.GetServiceConfigOne("framenodesecu", "1.0", "normal", "frameNode", this.serviceConfig);
                                //string FrameNodeSecurity = base.envirObj.BizNodeAddr + "/" + drServ["servcodename"].ToString().TrimStart('/');
                                string FrameNodeSecurity = drServ["url_addr"].ToString().TrimStart('/') + "/" + drServ["servcodename"].ToString().TrimStart('/');

                                SSY_DYNAMICTOKEN tokenModel = new SSY_DYNAMICTOKEN();
                                tokenModel.Dynamictoken = "";
                                tokenModel.ID = "";
                                tokenModel.Remarks = model.USERID.ToString();  //暂存用户账户，用于节点中心获取令牌时重新验证
                                tokenModel.Timestampss = model.PASSWORD.ToString(); //暂存用户口令，用于节点中心获取令牌时重新验证

                                this.reqdata = new ReqData();
                                this.reqdata.reqdata = json.Serialize(tokenModel);

                                //动态调用服务获取令牌
                                string tokenstr = DynamicInvokeWCF.Create<IFrameNodeSecurity>(FrameNodeSecurity).GetToken(this.json.Serialize(this.reqdata));

                                //返回执行结果
                                if (string.IsNullOrEmpty(tokenstr))
                                {
                                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("loginok_notoken", this.i18nModuleCurrLang), string.Empty, string.Empty);
                                }
                                else
                                {
                                    //解析令牌
                                    RespData tmpToken = json.Deserialize<RespData>(tokenstr);

                                    if(tmpToken.respflag == "1")
                                    {
                                        //赋值当前登录用户数据
                                        base.envirObj.SysUserDict = uds[0];
                                        RespData tmpresdata = json.Deserialize<RespData>(tokenstr);

                                        //直接返回环境参数,去除口令和数据节点
                                        base.envirObj.TokenEncrpValue = tmpresdata.respdata;
                                        SysEnvironmentSerialize resTmp = new SysEnvironmentSerialize();
                                        resTmp = json.Deserialize<SysEnvironmentSerialize>(json.Serialize(base.envirObj));
                                        resTmp.SysUserDict.PASSWORD = ""; //不返回密码
                                        resTmp.distManagerParam = null; //不返回数据节点                                
                                                                        //赋值用户数据到框架环境变量
                                        ManagerSysEnvironment.GetSysEnvironmentSerialize2SysEnvironment(base.envirObj);

                                        if (string.IsNullOrEmpty(toolStr.ToString()))
                                        {
                                            resdata = this.MakeResponseData("1", this.GetI18nLangItem("loginok", this.i18nModuleCurrLang), json.Serialize(resTmp), string.Empty);
                                        }
                                        else
                                        {
                                            resdata = this.MakeResponseData("2", this.GetI18nLangItem("loginok", this.i18nModuleCurrLang), json.Serialize(resTmp), string.Empty);
                                        }
                                    }
                                    else
                                    {
                                        resdata = this.MakeResponseData("0", this.GetI18nLangItem("loginok_gettokenerr", this.i18nModuleCurrLang), string.Empty, string.Empty);
                                    }                                                                       
                                }

                                #endregion                                
                            }

                            #endregion
                        }
                    }
                    else
                    {
                        resdata = this.MakeResponseData("0", this.GetI18nLangItem("loginerr_userNotPassword", this.i18nModuleCurrLang), string.Empty, string.Empty);
                    }
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("loginerr_nocurruser", this.i18nModuleCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("loginerr_findexception", this.i18nModuleCurrLang) + ex.Message, string.Empty, string.Empty);
            }  

            return json.Serialize(resdata);            
        }

        /// <summary>
        /// 获取用户信息仅仅为获取令牌验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetUserForLogin2N(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                SSY_USER_DICT model = this.json.Deserialize<SSY_USER_DICT>(reqdata.reqdata);

                List<SSY_USER_DICT> resobj = this._comBiz.GetUsers(model, base.envirObj.distManagerParam);

                //返回执行结果
                if (resobj.Count > 0)
                {
                    resdata = this.MakeResponseData("1", base.successStr, json.Serialize(resobj), string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", base.errorStr + this.GetI18nLangItem("nocurruser", this.i18nModuleCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("GetTokenVerifyUserErr", this.i18nModuleCurrLang) + ex.Message, string.Empty, string.Empty);
            }            

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public byte[] GetVerifyCodePic(string req)
        {
            //Bitmap bitmap = new Bitmap(width, height);
            //for (int i = 0; i < bitmap.Width; i++)
            //{
            //    for (int j = 0; j < bitmap.Height; j++)
            //    {
            //        bitmap.SetPixel(i, j, (Math.Abs(i - j) < 2) ? Color.Blue : Color.Yellow);
            //    }
            //}
            //MemoryStream ms = new MemoryStream();
            //bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //ms.Position = 0;
            //WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpeg";//可以换成其它格式的图片
            //return ms;
            return Common.Security.MakeVerifyCode();
        }

        #endregion



    }
}
