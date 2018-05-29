using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Data;

using Frame.ServiceInterfaces;
using WebServiceManager;
using Entitys.ComonEnti;
using BizExecFacade;
using Common;
using FrameCommon;

namespace Frame.ServiceLibs
{
    public class FrameManagerService : BaseService, IFrameManager
    {
        #region 定义

        CommonBizFacade _comBiz = null;

        #endregion

        public FrameManagerService()
        {
            //这里具体服务语言包
            DataTable servlangtmp = (DataTable)currCache.Get("i18nFrameManageri18nLang");
            if (servlangtmp != null)
            {
                if (currlang == envirObj.I18nCurrLang)
                {
                    i18nModuleCurrLang = servlangtmp;
                }
                else
                {
                    string FrameManageri18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameManageri18nLang", ""), envirObj.I18nCurrLang);
                    i18nModuleCurrLang = this.GetI18nLang(FrameManageri18nLang);
                }
            }
            else
            {
                string FrameManageri18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameManageri18nLang", ""), envirObj.I18nCurrLang);
                i18nModuleCurrLang = this.GetI18nLang(FrameManageri18nLang);
            }

            _comBiz = new CommonBizFacade(envirObj);
        }
              

        #region 升级接口实现

        #region 系统权限相关

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetUsersN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                SSY_USER_DICT model = json.Deserialize<SSY_USER_DICT>(reqdata.reqdata);

                List<SSY_USER_DICT> objResult = this._comBiz.GetUsers(model, base.envirObj.distManagerParam);

                if (UtilitysForT<SSY_USER_DICT>.ListHasData(objResult))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(objResult), string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetAllUsersN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                SSY_USER_DICT model = json.Deserialize<SSY_USER_DICT>(reqdata.reqdata);

                List<SSY_USER_DICT> objResult = this._comBiz.GetAllUsers(model, base.envirObj.distManagerParam);

                if (UtilitysForT<SSY_USER_DICT>.ListHasData(objResult))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(objResult), string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 用户退出系统
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string QuitUserForLoginN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                SSY_USER_DICT model = json.Deserialize<SSY_USER_DICT>(reqdata.reqdata);

                //准备日志参数实例
                List<SSY_LOGENTITY> ListBizLog = new List<SSY_LOGENTITY>();

                //决定业务操作动作
                base.envirObj.distManagerParam.DistributeActionIden = DistributeActionIden.TransAction;

                string resstr = this._comBiz.QuitUserForLogin(model, base.envirObj.distManagerParam, ListBizLog);                  

                if (string.IsNullOrEmpty(resstr))
                {
                    resdata = this.MakeResponseData("1", base.successStr, string.Empty, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("quitSystemFailed", this.i18nModuleCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("quitSystemErr", this.i18nModuleCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }


        /// <summary>
        /// 获取功能信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetPagesN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                SSY_USER_DICT model = json.Deserialize<SSY_USER_DICT>(reqdata.reqdata);

                DataSet ds = this._comBiz.GetPages(model, base.envirObj.distManagerParam);   

                if (Common.Utility.DsHasData(ds))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(UtilitysForT<SSY_PAGE_DICT>.GetListsObj(ds.Tables[0])), string.Empty);                   
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);           
        }

        /// <summary>
        /// 获取用户组信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetGroupN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                SSY_GROUP_DICT model = json.Deserialize<SSY_GROUP_DICT>(reqdata.reqdata);

                List<SSY_GROUP_DICT> objResult = this._comBiz.GetGroup(model, base.envirObj.distManagerParam);

                if (UtilitysForT<SSY_GROUP_DICT>.ListHasData(objResult))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(objResult), string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 获取用户组信息(带分页)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetGroupPagerN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                SSY_PagingParam pager = null;
                SSY_GROUP_DICT model = null;
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "SSY_PagingParam".ToUpper())
                    {
                        pager = json.Deserialize<SSY_PagingParam>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "SSY_GROUP_DICT".ToUpper())
                    {
                        model = json.Deserialize<SSY_GROUP_DICT>(objs[i].queryItemValue);
                    }
                }

                List<SSY_GROUP_DICT> objResult = this._comBiz.GetGroup(model, base.envirObj.distManagerParam, pager);

                if (UtilitysForT<SSY_GROUP_DICT>.ListHasData(objResult))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(objResult), pager.TotalSize.ToString());
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);           
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetUserdictPagerN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                SSY_PagingParam pager = null;
                SSY_USER_DICT model = null;
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "SSY_PagingParam".ToUpper())
                    {
                        pager = json.Deserialize<SSY_PagingParam>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "SSY_USER_DICT".ToUpper())
                    {
                        model = json.Deserialize<SSY_USER_DICT>(objs[i].queryItemValue);
                    }
                }

                List<SSY_USER_DICT> objResult = this._comBiz.GetUserdict(model, base.envirObj.distManagerParam, pager);

                if (UtilitysForT<SSY_USER_DICT>.ListHasData(objResult))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(objResult), pager.TotalSize.ToString());
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }               
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 操作用户组，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string OpBizObjectSingle_SSY_GROUP_DICTN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                //string errStr = string.Empty;
                List<string> errStr = new List<string>();
                List<SSY_GROUP_DICT> tempList = new List<SSY_GROUP_DICT>();
                List<string> opObjPropertyL = new List<string>();
                List<string> wherePropertyL = new List<string>();
                List<string> mainPropertyL = new List<string>();
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "objLists".ToUpper())
                    {
                        tempList = json.Deserialize<List<SSY_GROUP_DICT>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "opObjPropertyL".ToUpper())
                    {
                        opObjPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "wherePropertyL".ToUpper())
                    {
                        wherePropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "mainPropertyL".ToUpper())
                    {
                        mainPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                }

                //处理数据
                for (int xx = 0; xx < tempList.Count; xx++)
                {
                    //给主键赋值
                    //if (string.IsNullOrEmpty(tempList[xx].GROUPID.ToString()))
                    //{
                    //    //无含义id，保证唯一
                    //    tempList[xx].GROUPID = this._comBiz.GetID(MakeIDType.YMDHMS_3, "yyyyMMddHHmmss", null, base.envirObj.distManagerParam);
                    //}

                    if (tempList[xx].OPFlag.ToString().ToUpper() == "I")
                    {
                        //无含义id，保证唯一
                        tempList[xx].GROUPID = xx.ToString() + this._comBiz.GetID(MakeIDType.YMDHMS_3, "yyyyMMddHHmmss", null, base.envirObj.distManagerParam);
                    }

                    //处理时间戳
                    tempList[xx].TIMESTAMPSS = this._comBiz.GetSystemDateTime(base.envirObj.distManagerParam);

                    //转换bool值
                    if (tempList[xx].ISUSE.ToString().ToUpper() == "TRUE")
                    {
                        tempList[xx].ISUSE = "1";
                    }
                    else
                    {
                        tempList[xx].ISUSE = "0";
                    }
                }

                //事物联动
                base.envirObj.distManagerParam.DistributeActionIden = DistributeActionIden.TransAction;

                //准备日志参数实例，增加日志内容，若需要的话
                List<SSY_LOGENTITY> ListBizLog = new List<SSY_LOGENTITY>();
                //记录操作日志
                string currRecordLogTime =  System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SSY_LOGENTITY logenti = LogCommon.CreateLogDataEnt(LogTypeDomain.Biz, LogLevelOption.Normal, currRecordLogTime,
                                    "", "", LogAction.Modify, "SSY_GROUP_DICT",
                                    Utility.GetList2String(opObjPropertyL, "|"),
                                    json.Serialize(tempList), 
                                    base.envirObj.SysUserDict.USERID.ToString(), 
                                    "",
                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_managerGroup", this.i18nModuleCurrLang),
                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_managerGroupResult", this.i18nModuleCurrLang),
                                    base.envirObj.distManagerParam.DistributeDataNodes[0].Systemname, "");
                 ListBizLog.Add(logenti);

                bool tmpflag = this._comBiz.OpBizObjectSingle<SSY_GROUP_DICT>(tempList, opObjPropertyL, wherePropertyL, mainPropertyL, errStr,
                    base.envirObj.distManagerParam, ListBizLog);

                if (tmpflag)
                {
                    resdata = this.MakeResponseData("1", base.successStr, string.Empty, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", base.errorStr + Utility.GetList2String(errStr, "|"), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("operateErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }


        /// <summary>
        /// 操作用户，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string OpBizObjectSingle_SSY_USER_DICTN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                //string errStr = string.Empty;
                List<string> errStr = new List<string>();
                List<SSY_USER_DICT> tempList = new List<SSY_USER_DICT>();
                List<string> opObjPropertyL = new List<string>();
                List<string> wherePropertyL = new List<string>();
                List<string> mainPropertyL = new List<string>();
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "objLists".ToUpper())
                    {
                        tempList = json.Deserialize<List<SSY_USER_DICT>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "opObjPropertyL".ToUpper())
                    {
                        opObjPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "wherePropertyL".ToUpper())
                    {
                        wherePropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "mainPropertyL".ToUpper())
                    {
                        mainPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                }

                //处理数据

                //加密默认口令
                string iv128str = APPConfig.GetAPPConfig().GetConfigValue("ivpwd", "5CRc851hRywf7W3m");
                string key256str = APPConfig.GetAPPConfig().GetConfigValue("keypwd", "nW8FnftasWp7AVZrmgr9sdaGNXsjMWiw");
                byte[] key256 = Security.CreateKeyByte(key256str);
                byte[] iv128 = Security.CreateKeyByte(iv128str);
                string defaultPWD = Security.EnAES(APPConfig.GetAPPConfig().GetConfigValue("ResetDefaultPwd", "1234567"), key256, iv128);

                for (int xx = 0; xx < tempList.Count; xx++)
                {
                    if (tempList[xx].OPFlag.ToString().ToUpper() == "I")
                    {
                        //注册时间
                        tempList[xx].REGISTERDATE = this._comBiz.GetSystemDateTime(base.envirObj.distManagerParam);

                        //默认口令
                        tempList[xx].PASSWORD = defaultPWD;
                    }

                    //处理时间戳
                    tempList[xx].TIMESTAMPSS = this._comBiz.GetSystemDateTime(base.envirObj.distManagerParam);

                    //转换bool值
                    if (tempList[xx].ISUSE.ToString().ToUpper() == "TRUE")
                    {
                        tempList[xx].ISUSE = "1";
                    }
                    else
                    {
                        tempList[xx].ISUSE = "0";
                    }
                }

                //事物联动
                base.envirObj.distManagerParam.DistributeActionIden = DistributeActionIden.TransAction;

                //准备日志参数实例，增加日志内容，若需要的话
                List<SSY_LOGENTITY> ListBizLog = new List<SSY_LOGENTITY>();
                //记录操作日志
                string currRecordLogTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SSY_LOGENTITY logenti = LogCommon.CreateLogDataEnt(LogTypeDomain.Biz, LogLevelOption.Normal, currRecordLogTime,
                                    "", "", LogAction.Modify, "SSY_USER_DICT",
                                    Utility.GetList2String(opObjPropertyL, "|"),
                                    json.Serialize(tempList),
                                    base.envirObj.SysUserDict.USERID.ToString(),
                                    "",
                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_managerUser", this.i18nModuleCurrLang),
                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_managerUserResult", this.i18nModuleCurrLang),
                                    base.envirObj.distManagerParam.DistributeDataNodes[0].Systemname, "");
                ListBizLog.Add(logenti);

                bool tmpflag = this._comBiz.OpBizObjectSingle<SSY_USER_DICT>(tempList, opObjPropertyL, wherePropertyL, mainPropertyL, errStr,
                    base.envirObj.distManagerParam, ListBizLog);

                if (tmpflag)
                {
                    resdata = this.MakeResponseData("1", base.successStr, string.Empty, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", base.errorStr + Utility.GetList2String(errStr, "|"), string.Empty, string.Empty);
                }                        
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("operateErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }


        /// <summary>
        /// 获取页面信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetPageN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                SSY_PAGE_DICT model = json.Deserialize<SSY_PAGE_DICT>(reqdata.reqdata);

                List<SSY_PAGE_DICT> objResult = this._comBiz.GetPage(model, base.envirObj.distManagerParam);

                if (UtilitysForT<SSY_PAGE_DICT>.ListHasData(objResult))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(objResult), string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 获取页面信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetPagePagerN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                SSY_PagingParam pager = null;
                SSY_PAGE_DICT model = null;
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "SSY_PagingParam".ToUpper())
                    {
                        pager = json.Deserialize<SSY_PagingParam>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "SSY_PAGE_DICT".ToUpper())
                    {
                        model = json.Deserialize<SSY_PAGE_DICT>(objs[i].queryItemValue);
                    }
                }

                List<SSY_PAGE_DICT> objResult = this._comBiz.GetPagePager(model, base.envirObj.distManagerParam, pager);              

                if (UtilitysForT<SSY_PAGE_DICT>.ListHasData(objResult))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(objResult), pager.TotalSize.ToString());
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 操作页面，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string OpBizObjectSingle_SSY_PAGE_DICTN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                //string errStr = string.Empty;
                List<string> errStr = new List<string>();
                List<SSY_PAGE_DICT> tempList = new List<SSY_PAGE_DICT>();
                List<string> opObjPropertyL = new List<string>();
                List<string> wherePropertyL = new List<string>();
                List<string> mainPropertyL = new List<string>();
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "objLists".ToUpper())
                    {
                        tempList = json.Deserialize<List<SSY_PAGE_DICT>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "opObjPropertyL".ToUpper())
                    {
                        opObjPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "wherePropertyL".ToUpper())
                    {
                        wherePropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "mainPropertyL".ToUpper())
                    {
                        mainPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                }

                //处理数据
                for (int xx = 0; xx < tempList.Count; xx++)
                {
                    //处理时间戳
                    tempList[xx].TIMESTAMPSS = this._comBiz.GetSystemDateTime(base.envirObj.distManagerParam);                   
                }

                //事物联动
                base.envirObj.distManagerParam.DistributeActionIden = DistributeActionIden.TransAction;

                //准备日志参数实例，增加日志内容，若需要的话
                List<SSY_LOGENTITY> ListBizLog = new List<SSY_LOGENTITY>();
                //记录操作日志
                string currRecordLogTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SSY_LOGENTITY logenti = LogCommon.CreateLogDataEnt(LogTypeDomain.Biz, LogLevelOption.Normal, currRecordLogTime,
                                    "", "", LogAction.Modify, "SSY_PAGE_DICT",
                                    Utility.GetList2String(opObjPropertyL, "|"),
                                    json.Serialize(tempList),
                                    base.envirObj.SysUserDict.USERID.ToString(),
                                    "",
                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_managerFunction", this.i18nModuleCurrLang),
                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_managerFunctionResult", this.i18nModuleCurrLang),
                                    base.envirObj.distManagerParam.DistributeDataNodes[0].Systemname, "");
                ListBizLog.Add(logenti);

                bool tmpflag = this._comBiz.OpBizObjectSingle<SSY_PAGE_DICT>(tempList, opObjPropertyL, wherePropertyL, mainPropertyL, errStr,
                    base.envirObj.distManagerParam, ListBizLog);

                if (tmpflag)
                {
                    resdata = this.MakeResponseData("1", base.successStr, string.Empty, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", base.errorStr + Utility.GetList2String(errStr, "|"), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("operateErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }


        /// <summary>
        /// 获取用户组与用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetGroupUserPagerN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                SSY_PagingParam pager = null;
                SSY_USER_GROUP_DICT model = null;
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "SSY_PagingParam".ToUpper())
                    {
                        pager = json.Deserialize<SSY_PagingParam>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "SSY_USER_GROUP_DICT".ToUpper())
                    {
                        model = json.Deserialize<SSY_USER_GROUP_DICT>(objs[i].queryItemValue);
                    }
                }

                List<SSY_USER_GROUP_DICT> objResult = this._comBiz.GetGroupUserPager(model, base.envirObj.distManagerParam, pager);             

                if (UtilitysForT<SSY_USER_GROUP_DICT>.ListHasData(objResult))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(objResult), pager.TotalSize.ToString());
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 操作用户组与用户，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string OpBizObjectSingle_SSY_GROUP_USERN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                //string errStr = string.Empty;
                List<string> errStr = new List<string>();
                List<SSY_USER_GROUP_DICT> tempList = new List<SSY_USER_GROUP_DICT>();
                List<string> opObjPropertyL = new List<string>();
                List<string> wherePropertyL = new List<string>();
                List<string> mainPropertyL = new List<string>();
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "objLists".ToUpper())
                    {
                        tempList = json.Deserialize<List<SSY_USER_GROUP_DICT>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "opObjPropertyL".ToUpper())
                    {
                        opObjPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "wherePropertyL".ToUpper())
                    {
                        wherePropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "mainPropertyL".ToUpper())
                    {
                        mainPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                }

                //处理数据
                for (int xx = 0; xx < tempList.Count; xx++)
                {
                    //处理时间戳
                    tempList[xx].TIMESTAMPSS = this._comBiz.GetSystemDateTime(base.envirObj.distManagerParam);
                }

                //事物联动
                base.envirObj.distManagerParam.DistributeActionIden = DistributeActionIden.TransAction;

                //准备日志参数实例，增加日志内容，若需要的话
                List<SSY_LOGENTITY> ListBizLog = new List<SSY_LOGENTITY>();
                //记录操作日志
                string currRecordLogTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SSY_LOGENTITY logenti = LogCommon.CreateLogDataEnt(LogTypeDomain.Biz, LogLevelOption.Normal, currRecordLogTime,
                                    "", "", LogAction.Modify, "SSY_USER_GROUP_DICT",
                                    Utility.GetList2String(opObjPropertyL, "|"),
                                    json.Serialize(tempList),
                                    base.envirObj.SysUserDict.USERID.ToString(),
                                    "",
                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_managerGroupUser", this.i18nModuleCurrLang),
                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_managerGroupUserResult", this.i18nModuleCurrLang),
                                    base.envirObj.distManagerParam.DistributeDataNodes[0].Systemname, "");
                ListBizLog.Add(logenti);

                bool tmpflag = this._comBiz.OpBizObjectSingle<SSY_USER_GROUP_DICT>(tempList, opObjPropertyL, wherePropertyL, mainPropertyL, errStr,
                    base.envirObj.distManagerParam, ListBizLog);

                if (tmpflag)
                {
                    resdata = this.MakeResponseData("1", base.successStr, string.Empty, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", base.errorStr + Utility.GetList2String(errStr, "|"), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("operateErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }


        /// <summary>
        /// 获取角色权限配置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetGroupPageMgtN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                SSY_GROUP_PAGE_DICT model = json.Deserialize<SSY_GROUP_PAGE_DICT>(reqdata.reqdata);

                SSY_PAGE_GROUP_MQT resobj = this._comBiz.GetGroupPageMgt(model, base.envirObj.distManagerParam);

                if (UtilitysForT<SSY_GROUP_PAGE_DICT>.ListHasData(resobj.SSY_GROUP_PAGE_DICT))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(resobj), string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 操作用户组与功能权限，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string OpBizObjectSingle_SSY_GROUP_PAGE_DICTN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                //string errStr = string.Empty;
                List<string> errStr = new List<string>();
                List<SSY_GROUP_PAGE_DICT> tempList = new List<SSY_GROUP_PAGE_DICT>();
                List<string> opObjPropertyL = new List<string>();
                List<string> wherePropertyL = new List<string>();
                List<string> mainPropertyL = new List<string>();
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "objLists".ToUpper())
                    {
                        tempList = json.Deserialize<List<SSY_GROUP_PAGE_DICT>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "opObjPropertyL".ToUpper())
                    {
                        opObjPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "wherePropertyL".ToUpper())
                    {
                        wherePropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "mainPropertyL".ToUpper())
                    {
                        mainPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                }

                //处理数据
                for (int xx = 0; xx < tempList.Count; xx++)
                {
                    //处理时间戳
                    tempList[xx].TIMESTAMPSS = this._comBiz.GetSystemDateTime(base.envirObj.distManagerParam);
                }

                //事物联动
                base.envirObj.distManagerParam.DistributeActionIden = DistributeActionIden.TransAction;

                //准备日志参数实例，增加日志内容，若需要的话
                List<SSY_LOGENTITY> ListBizLog = new List<SSY_LOGENTITY>();
                //记录操作日志
                string currRecordLogTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SSY_LOGENTITY logenti = LogCommon.CreateLogDataEnt(LogTypeDomain.Biz, LogLevelOption.Normal, currRecordLogTime,
                                    "", "", LogAction.Modify, "SSY_GROUP_PAGE_DICT",
                                    Utility.GetList2String(opObjPropertyL, "|"),
                                    json.Serialize(tempList),
                                    base.envirObj.SysUserDict.USERID.ToString(),
                                    "",
                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_managerGroupPage", this.i18nModuleCurrLang),
                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_managerGroupPageResult", this.i18nModuleCurrLang),
                                    base.envirObj.distManagerParam.DistributeDataNodes[0].Systemname, "");
                ListBizLog.Add(logenti);

                bool tmpflag = this._comBiz.OpBizObjectSingle<SSY_GROUP_PAGE_DICT>(tempList, opObjPropertyL, wherePropertyL, mainPropertyL, errStr,
                    base.envirObj.distManagerParam, ListBizLog);               

                if (tmpflag)
                {
                    resdata = this.MakeResponseData("1", base.successStr, string.Empty, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", base.errorStr + Utility.GetList2String(errStr, "|"), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("operateErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 重置默认密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string ResetUserPWDN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                SSY_USER_DICT model = json.Deserialize<SSY_USER_DICT>(reqdata.reqdata);              

                //准备日志参数实例, 
                List<SSY_LOGENTITY> ListBizLog = new List<SSY_LOGENTITY>();

                //重置口令时，将口令重置为默认口令
                if(model.PASSWORD.ToString() == "")
                {
                    //加密默认口令
                    string iv128str = APPConfig.GetAPPConfig().GetConfigValue("ivpwd", "5CRc851hRywf7W3m");
                    string key256str = APPConfig.GetAPPConfig().GetConfigValue("keypwd", "nW8FnftasWp7AVZrmgr9sdaGNXsjMWiw");
                    byte[] key256 = Security.CreateKeyByte(key256str);
                    byte[] iv128 = Security.CreateKeyByte(iv128str);
                    string defaultPWD = Security.EnAES(APPConfig.GetAPPConfig().GetConfigValue("ResetDefaultPwd", "1234567"), key256, iv128);
                    model.PASSWORD = defaultPWD;
                }

                //事物联动
                base.envirObj.distManagerParam.DistributeActionIden = DistributeActionIden.TransAction;

                bool tmpflag = this._comBiz.ResetUserPWD(model, base.envirObj.distManagerParam, ListBizLog);             

                if (tmpflag)
                {
                    resdata = this.MakeResponseData("1", base.successStr, string.Empty, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", base.errorStr +  this.GetI18nLangItem("resetPasswordFailed", this.i18nModuleCurrLang), 
                        string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("resetPasswordErr", this.i18nModuleCurrLang) + ex.Message, 
                    string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        #endregion

        #region 字典相关        

        /// <summary>
        /// 获取全部系统字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetFrameDictAllN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                SSY_FRAME_DICT model = json.Deserialize<SSY_FRAME_DICT>(reqdata.reqdata);

                List<SSY_FRAME_DICT> objResult = this._comBiz.GetFrameDictAll(model, base.envirObj.distManagerParam);

                if (UtilitysForT<SSY_FRAME_DICT>.ListHasData(objResult))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(objResult), string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetFrameDictPagerN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                SSY_PagingParam pager = null;
                SSY_FRAME_DICT model = null;
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "SSY_PagingParam".ToUpper())
                    {
                        pager = json.Deserialize<SSY_PagingParam>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "SSY_FRAME_DICT".ToUpper())
                    {
                        model = json.Deserialize<SSY_FRAME_DICT>(objs[i].queryItemValue);
                    }
                }

                List<SSY_FRAME_DICT> objResult = this._comBiz.GetFrameDictPager(model, base.envirObj.distManagerParam, pager);

                if (UtilitysForT<SSY_FRAME_DICT>.ListHasData(objResult))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(objResult), pager.TotalSize.ToString());
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 操作业务表，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string OpBizObjectSingle_SSY_FRAME_DICTN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                //string errStr = string.Empty;
                List<string> errStr = new List<string>();
                List<SSY_FRAME_DICT> tempList = new List<SSY_FRAME_DICT>();
                List<string> opObjPropertyL = new List<string>();
                List<string> wherePropertyL = new List<string>();
                List<string> mainPropertyL = new List<string>();
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "objLists".ToUpper())
                    {
                        tempList = json.Deserialize<List<SSY_FRAME_DICT>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "opObjPropertyL".ToUpper())
                    {
                        opObjPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "wherePropertyL".ToUpper())
                    {
                        wherePropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "mainPropertyL".ToUpper())
                    {
                        mainPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                }

                //处理数据
                for (int xx = 0; xx < tempList.Count; xx++)
                {
                    if (tempList[xx].OPFlag.ToString().ToUpper() == "I")
                    {
                        //无含义id，保证唯一
                        tempList[xx].SSY_FRAME_DICTID = xx.ToString() + this._comBiz.GetID(MakeIDType.YMDHMS_4, "yyyyMMddHHmmss", null, base.envirObj.distManagerParam);
                    }

                    //处理时间戳
                    tempList[xx].TIMESTAMPSS = this._comBiz.GetSystemDateTime(base.envirObj.distManagerParam);                   
                }

                //事物联动
                base.envirObj.distManagerParam.DistributeActionIden = DistributeActionIden.TransAction;

                //准备日志参数实例，增加日志内容，若需要的话
                List<SSY_LOGENTITY> ListBizLog = new List<SSY_LOGENTITY>();
                //记录操作日志
                string currRecordLogTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SSY_LOGENTITY logenti = LogCommon.CreateLogDataEnt(LogTypeDomain.Biz, LogLevelOption.Normal, currRecordLogTime,
                                    "", "", LogAction.Modify, "SSY_FRAME_DICT",
                                    Utility.GetList2String(opObjPropertyL, "|"),
                                    json.Serialize(tempList),
                                    base.envirObj.SysUserDict.USERID.ToString(),
                                    "",
                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_managerFrameDict", this.i18nModuleCurrLang),
                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_managerFrameDictResult", this.i18nModuleCurrLang),
                                    base.envirObj.distManagerParam.DistributeDataNodes[0].Systemname, "");
                ListBizLog.Add(logenti);

                bool tmpflag = this._comBiz.OpBizObjectSingle<SSY_FRAME_DICT>(tempList, opObjPropertyL, wherePropertyL, mainPropertyL, errStr,
                    base.envirObj.distManagerParam, ListBizLog);               

                if (tmpflag)
                {
                    resdata = this.MakeResponseData("1", base.successStr, string.Empty, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", base.errorStr + Utility.GetList2String(errStr, "|"), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("operateErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }


        /// <summary>
        /// 获取全部业务字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetBizDictAllN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                SSY_BIZ_DICT model = json.Deserialize<SSY_BIZ_DICT>(reqdata.reqdata);  

                List<SSY_BIZ_DICT> objResult = this._comBiz.GetBizDictAll(model, base.envirObj.distManagerParam);

                if (UtilitysForT<SSY_BIZ_DICT>.ListHasData(objResult))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(objResult), string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetBizDictPagerN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                SSY_PagingParam pager = null;
                SSY_BIZ_DICT model = null;
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "SSY_PagingParam".ToUpper())
                    {
                        pager = json.Deserialize<SSY_PagingParam>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "SSY_BIZ_DICT".ToUpper())
                    {
                        model = json.Deserialize<SSY_BIZ_DICT>(objs[i].queryItemValue);
                    }
                }

                List<SSY_BIZ_DICT> objResult = this._comBiz.GetBizDictPager(model, base.envirObj.distManagerParam, pager);                            

                if (UtilitysForT<SSY_BIZ_DICT>.ListHasData(objResult))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(objResult), pager.TotalSize.ToString());
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 操作业务表，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string OpBizObjectSingle_SSY_BIZ_DICTN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                //string errStr = string.Empty;
                List<string> errStr = new List<string>();
                List<SSY_BIZ_DICT> tempList = new List<SSY_BIZ_DICT>();
                List<string> opObjPropertyL = new List<string>();
                List<string> wherePropertyL = new List<string>();
                List<string> mainPropertyL = new List<string>();
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "objLists".ToUpper())
                    {
                        tempList = json.Deserialize<List<SSY_BIZ_DICT>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "opObjPropertyL".ToUpper())
                    {
                        opObjPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "wherePropertyL".ToUpper())
                    {
                        wherePropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "mainPropertyL".ToUpper())
                    {
                        mainPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                }

                //处理数据
                for (int xx = 0; xx < tempList.Count; xx++)
                {
                    if (tempList[xx].OPFlag.ToString().ToUpper() == "I")
                    {
                        //无含义id，保证唯一
                        tempList[xx].SSY_BIZ_DICTID = xx.ToString() + this._comBiz.GetID(MakeIDType.YMDHMS_4, "yyyyMMddHHmmss", null, base.envirObj.distManagerParam);
                    }

                    //处理时间戳
                    tempList[xx].TIMESTAMPSS = this._comBiz.GetSystemDateTime(base.envirObj.distManagerParam);
                }

                //事物联动
                base.envirObj.distManagerParam.DistributeActionIden = DistributeActionIden.TransAction;

                //准备日志参数实例，增加日志内容，若需要的话
                List<SSY_LOGENTITY> ListBizLog = new List<SSY_LOGENTITY>();
                //记录操作日志
                string currRecordLogTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SSY_LOGENTITY logenti = LogCommon.CreateLogDataEnt(LogTypeDomain.Biz, LogLevelOption.Normal, currRecordLogTime,
                                    "", "", LogAction.Modify, "SSY_BIZ_DICT",
                                    Utility.GetList2String(opObjPropertyL, "|"),
                                    json.Serialize(tempList),
                                    base.envirObj.SysUserDict.USERID.ToString(),
                                    "",
                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_managerBizDict", this.i18nModuleCurrLang),
                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_managerBizDictResult", this.i18nModuleCurrLang),
                                    base.envirObj.distManagerParam.DistributeDataNodes[0].Systemname, "");
                ListBizLog.Add(logenti);


                bool tmpflag = this._comBiz.OpBizObjectSingle<SSY_BIZ_DICT>(tempList, opObjPropertyL, wherePropertyL, mainPropertyL, errStr,
                    base.envirObj.distManagerParam, ListBizLog);

                if (tmpflag)
                {
                    resdata = this.MakeResponseData("1", base.successStr, string.Empty, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", base.errorStr + Utility.GetList2String(errStr, "|"), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("operateErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }        

        /// <summary>
        /// 获取某个字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetDictsN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                string DOMAINNAMEIDEN = string.Empty;
                string dicttype = string.Empty;

                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "DOMAINNAMEIDEN".ToUpper())
                    {
                        DOMAINNAMEIDEN = objs[i].queryItemValue;
                    }
                    if (objs[i].queryItemKey.ToUpper() == "dicttype".ToUpper())
                    {
                        dicttype = objs[i].queryItemValue;
                    }
                }

                List<SSY_BIZ_DICT> objResult = this._comBiz.GetDicts(DOMAINNAMEIDEN, BizCommon.GetDictType(dicttype), base.envirObj.distManagerParam);

                if (UtilitysForT<SSY_BIZ_DICT>.ListHasData(objResult))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(objResult), string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        #endregion

        #region 公共部分

        /// <summary>
        /// 获取系统ID字符串
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetIDN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                string makeIDTypeStr = string.Empty; 
                string formmat = string.Empty;
                string makeCustemTypeIDStr = string.Empty;

                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "makeIDTypeStr".ToUpper())
                    {
                        makeIDTypeStr = objs[i].queryItemValue;
                    }
                    if (objs[i].queryItemKey.ToUpper() == "formmat".ToUpper())
                    {
                        formmat = objs[i].queryItemValue;
                    }
                    if (objs[i].queryItemKey.ToUpper() == "makeCustemTypeIDStr".ToUpper())
                    {
                        makeCustemTypeIDStr = objs[i].queryItemValue;
                    }                  
                }

                string ids = this._comBiz.GetID(BizCommon.GetCurrMakeIDType(makeIDTypeStr), formmat, BizCommon.GetMakeCustemTypeID(makeCustemTypeIDStr),
                    base.envirObj.distManagerParam);

                if (!string.IsNullOrEmpty(ids))
                {
                    resdata = this.MakeResponseData("1", string.Empty, ids, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noMakeID", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("MakeIDErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 获取系统服务器时间
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetSystemDateTimesN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                string systimestr = this._comBiz.GetSystemDateTime(base.envirObj.distManagerParam);

                if (!string.IsNullOrEmpty(systimestr))
                {
                    resdata = this.MakeResponseData("1", string.Empty, systimestr, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindSystem", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }       

        /// <summary>
        /// 操作通用实体，动作有增、改、删(目前仅用于日志记录)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string OpBizObjectSingle_SSY_LOGENTITYN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                //string errStr = string.Empty;
                List<string> errStr = new List<string>();
                List<SSY_LOGENTITY> tempList = new List<SSY_LOGENTITY>();
                List<string> opObjPropertyL = new List<string>();
                List<string> wherePropertyL = new List<string>();
                List<string> mainPropertyL = new List<string>();
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "objLists".ToUpper())
                    {
                        tempList = json.Deserialize<List<SSY_LOGENTITY>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "opObjPropertyL".ToUpper())
                    {
                        opObjPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "wherePropertyL".ToUpper())
                    {
                        wherePropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "mainPropertyL".ToUpper())
                    {
                        mainPropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                }

                //事物联动
                base.envirObj.distManagerParam.DistributeActionIden = DistributeActionIden.TransAction;

                bool tmpflag = this._comBiz.OpBizObjectSingle<SSY_LOGENTITY>(tempList, opObjPropertyL, wherePropertyL, mainPropertyL, errStr,
                    base.envirObj.distManagerParam);

                if (tmpflag)
                {
                    resdata = this.MakeResponseData("1", base.successStr, string.Empty, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", base.errorStr + Utility.GetList2String(errStr, "|"), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("operateErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 通用查询数据实体是否存在数据库中
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string CheckBizObjectRepatN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                string bizobjectname = string.Empty;
                List<string> wherePropertyL = new List<string>();
                List<string> errStr = new List<string>();
                string splitchar = string.Empty;
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "bizobjectname".ToUpper())
                    {
                        bizobjectname = objs[i].queryItemValue;
                    }
                    if (objs[i].queryItemKey.ToUpper() == "wherePropertyL".ToUpper())
                    {
                        wherePropertyL = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "splitchar".ToUpper())
                    {
                        splitchar = objs[i].queryItemValue;
                    }
                }
                               
                bool tmpflag = this._comBiz.CheckBizObjectRepat(bizobjectname, wherePropertyL, splitchar, base.envirObj.distManagerParam, errStr);

                if (tmpflag)
                {
                    resdata = this.MakeResponseData("1", string.Empty, string.Empty, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);          
        }

        /// <summary>
        /// 通用查询数据实体是否存在数据库中,批量查询，支持分别查询一个字段、一组字段是否存在,可查询多条记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string CheckBizObjectsRepat(string req)
        {
            List<string> resStr = new List<string>(); 
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                string bizObjectName = string.Empty;
                string fields = string.Empty;
                List<string> fieldsValue = new List<string>();
                string splitChar = string.Empty;
                string splitCharSub = string.Empty;

                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "bizObjectName".ToUpper())
                    {
                        bizObjectName = objs[i].queryItemValue;
                    }
                    if (objs[i].queryItemKey.ToUpper() == "fields".ToUpper())
                    {
                        fields = objs[i].queryItemValue;
                    }
                    if (objs[i].queryItemKey.ToUpper() == "fieldsValue".ToUpper())
                    {
                        fieldsValue = json.Deserialize<List<string>>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "splitChar".ToUpper())
                    {
                        splitChar = objs[i].queryItemValue;
                    }
                    if (objs[i].queryItemKey.ToUpper() == "splitCharSub".ToUpper())
                    {
                        splitCharSub = objs[i].queryItemValue;
                    }
                }

                resStr = this._comBiz.CheckBizObjectsRepat(bizObjectName, fields, fieldsValue, splitChar, splitCharSub, base.envirObj.distManagerParam);

                if (resStr.Count  > 0)
                {
                    if(resStr.Count == 1)
                    {
                        if(resStr[0].Substring(0, 3) == "err")
                        {
                            resStr[0] = this.GetI18nLangItem("checkDataRepatErr", this.i18nCommonCurrLang) + resStr[0]; 
                            resdata = this.MakeResponseData("0", json.Serialize(resStr), string.Empty, string.Empty);
                        }
                        else
                        {
                            resdata = this.MakeResponseData("0", json.Serialize(resStr), string.Empty, string.Empty);
                        }
                    }
                    else
                    {
                        resdata = this.MakeResponseData("0", json.Serialize(resStr), string.Empty, string.Empty);
                    }                    
                }
                else
                {
                    resdata = this.MakeResponseData("1", string.Empty, string.Empty, string.Empty);                    
                }
            }
            catch (Exception ex)
            {
                resStr.Add(this.GetI18nLangItem("checkDataRepatErr", this.i18nCommonCurrLang) + ex.Message);
                resdata = this.MakeResponseData("0", json.Serialize(resStr), string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 通用获取单个实体所有数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetEntityAllDataForComN(string req)
        {
            try
            {                
                reqdata = this.AnaRequestData(req);
                string tablename = reqdata.reqdata;

                DataTable dttmp = this._comBiz.GetEntityAllDataForCommon(tablename, base.envirObj.distManagerParam);                

                if (Utility.DtHasData(dttmp))
                {                                       
                    List<Dictionary<string, object>> parentRow = Common.Utility.Datatable2Json(dttmp);
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(parentRow), string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }



        #endregion

        #region 系统日志相关

        /// <summary>
        /// 获取系统日志
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetLogDataPagerN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                SSY_PagingParam pager = null;
                SSY_LOGENTITY model = null;
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "SSY_PagingParam".ToUpper())
                    {
                        pager = json.Deserialize<SSY_PagingParam>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "SSY_LOGENTITY".ToUpper())
                    {
                        model = json.Deserialize<SSY_LOGENTITY>(objs[i].queryItemValue);
                    }
                }

                List<SSY_LOGENTITY> objResult = this._comBiz.GetLogDataPager(model, base.envirObj.distManagerParam, pager);

                if (UtilitysForT<SSY_LOGENTITY>.ListHasData(objResult))
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(objResult), pager.TotalSize.ToString());
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindData", this.i18nCommonCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("findDataErr", this.i18nCommonCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

      

        #endregion

        #endregion
    }
}
