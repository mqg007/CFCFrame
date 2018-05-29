using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Data;

using WebServiceManager;
using Entitys.ComonEnti;
using BizExecFacade;
using Common;
using FrameCommon;
using Xxx.BizExecFacade;
using Xxx.ServiceInterfaces;
using Xxx.Entities;

namespace Xxx.ServiceLibs
{
    public class SomebizService : BaseService, ISomebiz
    {
        #region 定义

        CommonBizFacade _comBiz = null;
        BizFacadeSomebiz _comSomebiz = null;

        string baseXmlPath = APPConfig.GetAPPConfig().GetConfigValue("XmldataPath", "");

        #endregion


        public SomebizService()
        {
            //这里具体服务语言包
            DataTable servlangtmp = (DataTable)currCache.Get("i18nXxxi18nLang");
            if (servlangtmp != null)
            {
                if (currlang == envirObj.I18nCurrLang)
                {
                    i18nModuleCurrLang = servlangtmp;
                }
                else
                {
                    string XxxManageri18nLang = string.Format(this.baseXmlPath + APPConfig.GetAPPConfig().GetConfigValue("XxxManageri18nLang", ""), envirObj.I18nCurrLang);
                    i18nModuleCurrLang = this.GetI18nLang(XxxManageri18nLang);
                }
            }
            else
            {
                string XxxManageri18nLang = string.Format(this.baseXmlPath + APPConfig.GetAPPConfig().GetConfigValue("XxxManageri18nLang", ""), envirObj.I18nCurrLang);
                i18nModuleCurrLang = this.GetI18nLang(XxxManageri18nLang);
            }

            _comBiz = new CommonBizFacade(envirObj);
            _comSomebiz = new BizFacadeSomebiz(envirObj);
        }

        #region 字典相关        

        /// <summary>
        /// 获取全部系统字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetFrameDictAll(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                Xxx.Entities.SSY_FRAME_DICT model = json.Deserialize<Xxx.Entities.SSY_FRAME_DICT>(reqdata.reqdata);

                List<Xxx.Entities.SSY_FRAME_DICT> objResult = this._comSomebiz.GetFrameDictAll(model, base.envirObj.distManagerParam);

                if (UtilitysForT<Xxx.Entities.SSY_FRAME_DICT>.ListHasData(objResult))
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
        public string GetFrameDictPager(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                SSY_PagingParam pager = null;
                Xxx.Entities.SSY_FRAME_DICT model = null;
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "SSY_PagingParam".ToUpper())
                    {
                        pager = json.Deserialize<SSY_PagingParam>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "SSY_FRAME_DICT".ToUpper())
                    {
                        model = json.Deserialize<Xxx.Entities.SSY_FRAME_DICT>(objs[i].queryItemValue);
                    }
                }

                List<Xxx.Entities.SSY_FRAME_DICT> objResult = this._comSomebiz.GetFrameDictPager(model, base.envirObj.distManagerParam, pager);

                if (UtilitysForT<Xxx.Entities.SSY_FRAME_DICT>.ListHasData(objResult))
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
        public string OpBizObjectSingle_SSY_FRAME_DICT(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                //string errStr = string.Empty;
                List<string> errStr = new List<string>();
                List<Xxx.Entities.SSY_FRAME_DICT> tempList = new List<Xxx.Entities.SSY_FRAME_DICT>();
                List<string> opObjPropertyL = new List<string>();
                List<string> wherePropertyL = new List<string>();
                List<string> mainPropertyL = new List<string>();
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "objLists".ToUpper())
                    {
                        tempList = json.Deserialize<List<Xxx.Entities.SSY_FRAME_DICT>>(objs[i].queryItemValue);
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

                bool tmpflag = this._comBiz.OpBizObjectSingle<Xxx.Entities.SSY_FRAME_DICT>(tempList, opObjPropertyL, wherePropertyL, mainPropertyL, errStr,
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
        public string GetBizDictAll(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                Xxx.Entities.SSY_BIZ_DICT model = json.Deserialize<Xxx.Entities.SSY_BIZ_DICT>(reqdata.reqdata);

                List<Xxx.Entities.SSY_BIZ_DICT> objResult = this._comSomebiz.GetBizDictAll(model, base.envirObj.distManagerParam);

                if (UtilitysForT<Xxx.Entities.SSY_BIZ_DICT>.ListHasData(objResult))
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
        public string GetBizDictPager(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                SSY_PagingParam pager = null;
                Xxx.Entities.SSY_BIZ_DICT model = null;
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "SSY_PagingParam".ToUpper())
                    {
                        pager = json.Deserialize<SSY_PagingParam>(objs[i].queryItemValue);
                    }
                    if (objs[i].queryItemKey.ToUpper() == "SSY_BIZ_DICT".ToUpper())
                    {
                        model = json.Deserialize<Xxx.Entities.SSY_BIZ_DICT>(objs[i].queryItemValue);
                    }
                }

                List<Xxx.Entities.SSY_BIZ_DICT> objResult = this._comSomebiz.GetBizDictPager(model, base.envirObj.distManagerParam, pager);

                if (UtilitysForT<Xxx.Entities.SSY_BIZ_DICT>.ListHasData(objResult))
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
        public string OpBizObjectSingle_SSY_BIZ_DICT(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(reqdata.reqdata);

                //string errStr = string.Empty;
                List<string> errStr = new List<string>();
                List<Xxx.Entities.SSY_BIZ_DICT> tempList = new List<Xxx.Entities.SSY_BIZ_DICT>();
                List<string> opObjPropertyL = new List<string>();
                List<string> wherePropertyL = new List<string>();
                List<string> mainPropertyL = new List<string>();
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].queryItemKey.ToUpper() == "objLists".ToUpper())
                    {
                        tempList = json.Deserialize<List<Xxx.Entities.SSY_BIZ_DICT>>(objs[i].queryItemValue);
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


                bool tmpflag = this._comBiz.OpBizObjectSingle<Xxx.Entities.SSY_BIZ_DICT>(tempList, opObjPropertyL, wherePropertyL, mainPropertyL, errStr,
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
        public string GetDicts(string req)
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

                List<Xxx.Entities.SSY_BIZ_DICT> objResult = this._comSomebiz.GetDicts(DOMAINNAMEIDEN, BizCommon.GetDictType(dicttype), base.envirObj.distManagerParam);

                if (UtilitysForT<Xxx.Entities.SSY_BIZ_DICT>.ListHasData(objResult))
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
    }
}
